namespace Wacton.Unicolour;

internal class SpectralBoundary
{
    private readonly Observer observer;
    private readonly Chromaticity whiteChromaticity;
    private readonly Lazy<List<Segment>> boundarySegments;
    private readonly Lazy<Segment> lineOfPurples;
    private readonly Lazy<double> minNegativeWavelength;
    private readonly Lazy<double> maxNegativeWavelength;

    internal const int MinWavelength = 360;
    internal const int MaxWavelength = 700;
    internal double MinNegativeWavelength => minNegativeWavelength.Value;
    internal double MaxNegativeWavelength => maxNegativeWavelength.Value;
    
    internal SpectralBoundary(Observer observer, Chromaticity whiteChromaticity)
    {
        this.observer = observer;
        this.whiteChromaticity = whiteChromaticity;
        boundarySegments = new Lazy<List<Segment>>(GenerateBoundarySegments);
        lineOfPurples = new Lazy<Segment>(() => boundarySegments.Value.Single(segment => segment.IsLineOfPurples));
        
        minNegativeWavelength = new Lazy<double>(() =>
        {
            // possible to be null if user sets white point as monochromatic light (i.e. on the boundary itself)
            var intersects = FindIntersects(lineOfPurples.Value.EndChromaticity);
            return intersects == null ? double.NaN : -intersects.Far.Wavelength;
        });
        
        maxNegativeWavelength = new Lazy<double>(() =>
        {
            // possible to be null if user sets white point as monochromatic light (i.e. on the boundary itself)
            var intersects = FindIntersects(lineOfPurples.Value.StartChromaticity);
            return intersects == null ? double.NaN : -intersects.Far.Wavelength;
        });
    }

    internal Intersects? FindIntersects(Chromaticity sample) => FindIntersects(sample, whiteChromaticity);
    private Intersects? FindIntersects(Chromaticity sample, Chromaticity white)
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
        var nearIntersects = intersects.Where(x => (x.DistanceToSample - minDistanceToSample).IsEffectivelyZero()).ToList();
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
        
        // although an unlikely edge case, it is possible for there to be no far intersect if only 1 intersect is found
        // e.g. when the white point itself is on the boundary and the sample is outside the boundary
        var near = nearIntersects.First();
        var far = !farIntersects.Any() ? near : farIntersects.First();
        return new Intersects(sample, white, near, far);
    } 
    
    internal Chromaticity GetChromaticity(double dominantWavelength, double purity)
    {
        var convergedDominantWavelength = dominantWavelength >= 0
            ? dominantWavelength.Clamp(MinWavelength, MaxWavelength)
            : dominantWavelength.Clamp(MinNegativeWavelength, MaxNegativeWavelength);
        
        var wavelength = Math.Abs(convergedDominantWavelength);
        var segment = boundarySegments.Value
            .Where(segment => wavelength >= segment.StartWavelength && wavelength <= segment.EndWavelength)
            .OrderBy(segment => segment.StartWavelength)
            .First();
        
        var wavelengthDelta = Math.Abs(segment.EndWavelength - segment.StartWavelength);
        var distance = (wavelengthDelta - (segment.EndWavelength - wavelength)) / wavelengthDelta;
        var pureChromaticity = new Chromaticity(
            Interpolation.Interpolate(segment.StartChromaticity.X, segment.EndChromaticity.X, distance),
            Interpolation.Interpolate(segment.StartChromaticity.Y, segment.EndChromaticity.Y, distance)
        );
        
        var useLineOfPurples = dominantWavelength < 0;
        if (useLineOfPurples)
        {
            var whiteToSampleLine = Line.FromPoints(pureChromaticity.Xy, whiteChromaticity.Xy);
            var intersect = GetIntersect(lineOfPurples.Value, whiteToSampleLine, pureChromaticity);
            pureChromaticity = intersect.Chromaticity;
        }

        return new(
            Interpolation.Interpolate(whiteChromaticity.X, pureChromaticity.X, purity),
            Interpolation.Interpolate(whiteChromaticity.Y, pureChromaticity.Y, purity)
        );
    }
    
    internal bool IsOutside(Chromaticity chromaticity)
    {
        /*
         * although FindBoundaryIntersects takes a "white" chromaticity as an argument
         * it's really just a reference point for calculating intersects
         * and "white" is a more natural naming for every other purpose except this one
         * so using the given chromaticity as a "white point override"
         */
        const double sampleOffset = 0.00001;
        var offsetSample = new Chromaticity(chromaticity.X, chromaticity.Y + sampleOffset);
        var intersects = FindIntersects(offsetSample, chromaticity);

        // no intersects; point is definitely outside the locus
        if (intersects == null) return true;
        
        var near = intersects.Near.Chromaticity;
        var far = intersects.Far.Chromaticity;
        
        // two distinct intersect; if point is not between them it is outside the locus
        if (far != near) return !IsBetween(chromaticity.Y, near.Y, far.Y);
        
        // single intersect; if point is not the intersect it is outside the locus
        var sameX = (chromaticity.X - near.X).IsEffectivelyZero();
        var sameY = (chromaticity.Y - near.Y).IsEffectivelyZero();
        var isIntersectPoint = sameX && sameY;
        return !isIntersectPoint;
    }
    
    private List<Segment> GenerateBoundarySegments()
    {
        var result = new List<Segment>();
        for (var startNm = MinWavelength; startNm < MaxWavelength; startNm++)
        {
            // using 1 nm segments since that is the granularity of the standard observer CMFs
            result.Add(CreateSegment(startNm, startNm + 1));
        }
        
        result.Add(CreateSegment(MaxWavelength, MinWavelength));
        return result;
    }
    
    private Segment CreateSegment(int startWavelength, int endWavelength)
    {
        var startChromaticity = GetChromaticity(startWavelength);
        var endChromaticity = GetChromaticity(endWavelength);
        var line = Line.FromPoints(startChromaticity.Xy, endChromaticity.Xy);
        return new Segment(startWavelength, endWavelength, startChromaticity, endChromaticity, line);
    }
    
    private readonly Dictionary<int, Chromaticity> wavelengthToChromaticityCache = new();
    private Chromaticity GetChromaticity(int wavelength)
    {
        if (wavelengthToChromaticityCache.TryGetValue(wavelength, out var cached))
        {
            return cached;
        }
        
        var xyz = Xyz.FromSpd(Spd.Monochromatic(wavelength), observer);
        var xyy = Xyy.FromXyz(xyz, whiteChromaticity);
        var chromaticity = xyy.Chromaticity;
        wavelengthToChromaticityCache.Add(wavelength, chromaticity);
        return chromaticity;
    }

    private Intersect GetIntersect(Segment segment, Line whiteToSampleLine, Chromaticity sample)
    {
        var (x, y) = whiteToSampleLine.GetIntersect(segment.Line);
        var intersectChromaticity = new Chromaticity(x, y);
        var distanceToSample = Distance(sample, intersectChromaticity);
        var distanceToWhite = Distance(whiteChromaticity, intersectChromaticity);
        return new Intersect(segment, intersectChromaticity, distanceToSample, distanceToWhite);
    }

    private static double Distance(Chromaticity chromaticity1, Chromaticity chromaticity2)
    {
        return Math.Sqrt(Math.Pow(chromaticity2.X - chromaticity1.X, 2) + Math.Pow(chromaticity2.Y - chromaticity1.Y, 2));
    }
    
    private static bool IsBetween(double value, double near, double far)
    {
        var lower = Math.Min(near, far);
        var upper = Math.Max(near, far);
        var isAboveLower = value > lower || (value - lower).IsEffectivelyZero();
        var isBelowUpper = value < upper || (upper - value).IsEffectivelyZero();
        return isAboveLower && isBelowUpper;
    }

    internal record Intersects(Chromaticity Sample, Chromaticity White, Intersect Near, Intersect Far)
    {
        internal Chromaticity Sample { get; } = Sample;
        internal Chromaticity White { get; } = White;
        internal Intersect Near { get; } = Near;
        internal Intersect Far { get; } = Far;
        private Intersect Dominant => Far.DistanceToSample < Far.DistanceToWhite ? Far : Near;
        private Intersect Complementary => Dominant == Near ? Far : Near;

        // dominant distant to white = 0 is an extremely unlikely edge case
        // where the white point itself is on the boundary and the sample is outside the boundary
        // resulting in only 1 intersect, which is the location of white point
        internal double DominantWavelength => Dominant.Segment.IsLineOfPurples ? -Complementary.Wavelength : Dominant.Wavelength;
        internal double ExcitationPurity => Dominant.DistanceToWhite == 0 ? double.NaN : Distance(Sample, White) / Dominant.DistanceToWhite;

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