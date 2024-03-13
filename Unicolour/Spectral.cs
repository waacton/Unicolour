namespace Wacton.Unicolour;

internal class Spectral
{
    private readonly Observer observer;
    private readonly Chromaticity white;
    private readonly Lazy<List<Segment>> boundarySegments;
    
    internal Spectral(Observer observer, Chromaticity white)
    {
        this.observer = observer;
        this.white = white;
        boundarySegments = new Lazy<List<Segment>>(GetBoundarySegments);
    }
    
    internal Intersects? FindBoundaryIntersects(Chromaticity sample)
    {
        if (sample == white) return null;
        var whiteToSampleLine = Line.FromPoints(white.Xy, sample.Xy);
        
        var intersects = boundarySegments.Value
            .Select(segment => GetIntersect(segment, whiteToSampleLine, sample))
            .Where(intersect => intersect.IsOnSegment && !double.IsInfinity(intersect.DistanceToSample))
            .OrderByDescending(intersect => intersect.Segment.StartWavelength)
            .ToList();

        if (!intersects.Any()) return null;
        
        var minDistanceToSample = intersects.Min(x => x.DistanceToSample);
        var nearIntersects = intersects.Where(x => Utils.IsEffectivelyZero(x.DistanceToSample - minDistanceToSample)).ToList();
        var farIntersects = intersects.Except(nearIntersects).ToList();

        // filter line of purple if there are other candidates that are just as close
        FilterLineOfPurple(nearIntersects);
        FilterLineOfPurple(farIntersects);
        void FilterLineOfPurple(List<Intersect> closestIntersects)
        {
            if (closestIntersects.Count > 1)
            {
                closestIntersects.RemoveAll(x => x.Segment.IsLineOfPurples);
            }
        }
        
        var near = nearIntersects.First();
        var far = farIntersects.First();
        return new Intersects(near, far, sample, white);
    }
    
    private List<Segment> GetBoundarySegments()
    {
        var result = new List<Segment>();
        for (var startNm = 360; startNm < 700; startNm++)
        {
            result.Add(GetSegment(startNm, startNm + 1));
        }
        
        result.Add(GetSegment(700, 360));
        return result;
    }
    
    private Segment GetSegment(int startWavelength, int endWavelength)
    {
        var startChromaticity = GetChromaticity(startWavelength);
        var endChromaticity = GetChromaticity(endWavelength);
        var line = Line.FromPoints(startChromaticity.Xy, endChromaticity.Xy);
        return new Segment(startWavelength, endWavelength, startChromaticity, endChromaticity, line);
    }
    
    private readonly Dictionary<int, Chromaticity> wavelengthToChromaticity = new();
    private Chromaticity GetChromaticity(int wavelength)
    {
        if (wavelengthToChromaticity.ContainsKey(wavelength))
        {
            return wavelengthToChromaticity[wavelength];
        }
        
        var xyz = Xyz.FromSpd(new Spd { { wavelength, 1.0 } }, observer);
        var xyy = Xyy.FromXyz(xyz, white);
        var chromaticity = xyy.Chromaticity;
        wavelengthToChromaticity.Add(wavelength, chromaticity);
        return chromaticity;
    }

    private Intersect GetIntersect(Segment segment, Line whiteToSampleLine, Chromaticity sample)
    {
        var (x, y) = whiteToSampleLine.GetIntersect(segment.Line);
        var intersectChromaticity = new Chromaticity(x, y);
        var distanceToSample = Distance(sample, intersectChromaticity);
        var distanceToWhite = Distance(white, intersectChromaticity);
        return new Intersect(segment, intersectChromaticity, distanceToSample, distanceToWhite);
    }

    private static double Distance(Chromaticity chromaticity1, Chromaticity chromaticity2)
    {
        return Math.Sqrt(Math.Pow(chromaticity2.X - chromaticity1.X, 2) + Math.Pow(chromaticity2.Y - chromaticity1.Y, 2));
    }

    internal record Intersects(Intersect Near, Intersect Far, Chromaticity Sample, Chromaticity White)
    {
        internal Intersect Near { get; } = Near;
        internal Intersect Far { get; } = Far;
        internal Chromaticity Sample { get; } = Sample;
        internal Chromaticity White { get; } = White;
        
        internal double DominantWavelength()
        {
            var useNegativeComplement = Near.Segment.IsLineOfPurples;
            return useNegativeComplement ? -Far.Wavelength : Near.Wavelength;
        }

        internal double ExcitationPurity()
        {
            var sampleToWhiteDistance = Distance(Sample, White);
            return sampleToWhiteDistance / Near.DistanceToWhite;
        }
        
        internal bool IsImaginary()
        {
            var excitationPurity = ExcitationPurity();
            var isEffectivelyOne = (1.0 - excitationPurity).IsEffectivelyZero();
            return excitationPurity > 1 && !isEffectivelyOne;
        }

        public override string ToString() => $"{Near} & {Far}";
    }

    internal record Segment(int StartWavelength, int EndWavelength, Chromaticity StartChromaticity, Chromaticity EndChromaticity, Line Line)
    {
        internal int StartWavelength { get; } = StartWavelength;
        internal int EndWavelength { get; } = EndWavelength;
        internal bool IsLineOfPurples => EndWavelength < StartWavelength;
        internal Chromaticity StartChromaticity { get; } = StartChromaticity;
        internal Chromaticity EndChromaticity { get; } = EndChromaticity;
        internal Line Line { get; } = Line;
        internal double Length => Distance(StartChromaticity, EndChromaticity);

        public override string ToString() => $"{StartWavelength} \u27f6 {EndWavelength}";
    }
    
    internal record Intersect(Segment Segment, Chromaticity Chromaticity, double DistanceToSample, double DistanceToWhite)
    {
        internal Segment Segment { get; } = Segment;
        internal Chromaticity Chromaticity { get; } = Chromaticity;
        internal double DistanceToSample { get; } = DistanceToSample;
        internal double DistanceToWhite { get; } = DistanceToWhite;
        
        internal double DistanceFromStart => Distance(Chromaticity, Segment.StartChromaticity);
        internal double DistanceFromEnd => Distance(Chromaticity, Segment.EndChromaticity);
        internal double SegmentLengthViaIntersect => DistanceFromStart + DistanceFromEnd;
        internal double SegmentLengthDifference => Math.Abs(SegmentLengthViaIntersect - Segment.Length); // the closer to zero, the closer it lies on the segment
        internal bool IsOnSegment => SegmentLengthDifference.IsEffectivelyZero();
        internal double SegmentInterpolationAmount => DistanceFromStart / SegmentLengthViaIntersect;
        internal double Wavelength => Interpolation.Interpolate(Segment.StartWavelength, Segment.EndWavelength, SegmentInterpolationAmount);
        
        public override string ToString() => $"{Segment} · {Wavelength:F2} nm · {DistanceToSample:F4} from sample";
    }

    public override string ToString() => $"Spectral locus for {observer}";
}