using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

// using 1 nm segments since that is the granularity of the standard observer CMFs
internal class SpectralBoundary : Boundary
{
    private readonly Chromaticity whiteChromaticity;
    private readonly Lazy<double> minNegativeWavelength;
    private readonly Lazy<double> maxNegativeWavelength;
    
    internal const int MinWavelength = 360;
    internal const int MaxWavelength = 700;
    
    private Segment lineOfPurples => segments.Value.Last();
    internal double MinNegativeWavelength => minNegativeWavelength.Value;
    internal double MaxNegativeWavelength => maxNegativeWavelength.Value;
    
    internal SpectralBoundary(Observer observer, Chromaticity white) : base(GetPoints(observer, white))
    {
        whiteChromaticity = white;
        
        minNegativeWavelength = new Lazy<double>(() =>
        {
            // possible to be null if user sets white point as monochromatic light (i.e. on the boundary itself)
            var intersects = GetIntersects(lineOfPurples.End);
            return intersects == null ? double.NaN : -GetWavelength(intersects.Value.far);
        });
        
        maxNegativeWavelength = new Lazy<double>(() =>
        {
            // possible to be null if user sets white point as monochromatic light (i.e. on the boundary itself)
            var intersects = GetIntersects(lineOfPurples.Start);
            return intersects == null ? double.NaN : -GetWavelength(intersects.Value.far);
        });
    }
    
    internal (Intersect near, Intersect far)? GetIntersects(Chromaticity sample) => GetIntersects(sample, whiteChromaticity);
    
    // if an intersect lies simultaneously on spectral locus AND line of purples, always prefer spectral locus
    protected override (Intersect[] nearIntersects, Intersect[] farIntersects) FilterIntersects(Intersect[] nearIntersects, Intersect[] farIntersects)
    {
        nearIntersects = nearIntersects.Length > 1
            ? nearIntersects.Where(x => x.Segment != lineOfPurples).ToArray()
            : nearIntersects;
        
        farIntersects = farIntersects.Length > 1
            ? farIntersects.Where(x => x.Segment != lineOfPurples).ToArray()
            : farIntersects;

        return (nearIntersects, farIntersects);
    }
    
    internal (double dominantWavelength, double excitationPurity)? GetWavelengthAndPurity(Chromaticity sample)
    {
        var intersects = GetIntersects(sample);
        if (!intersects.HasValue) return null;

        var (near, far) = intersects.Value;
        
        // the dominant intersect isn't always the near intersect
        // e.g. a sample slightly above white should use segment in the green area but line of purples is closer
        var dominant = far.DistanceFromSample < far.DistanceFromReference ? far : near;
        var complementary = dominant == near ? far : near;

        var dominantWavelength = dominant.Segment == lineOfPurples ? -GetWavelength(complementary) : GetWavelength(dominant);
        var excitationPurity = Distance(sample, whiteChromaticity) / dominant.DistanceFromReference;
        return (dominantWavelength, excitationPurity);
    }
    
    internal Chromaticity GetChromaticity(double dominantWavelength, double purity)
    {
        var convergedDominantWavelength = dominantWavelength >= 0
            ? dominantWavelength.Clamp(MinWavelength, MaxWavelength)
            : dominantWavelength.Clamp(MinNegativeWavelength, MaxNegativeWavelength);
        
        var wavelength = Math.Abs(convergedDominantWavelength);
        var startWavelength = (int)Math.Floor(wavelength);
        var segment = GetSegment(startWavelength);
        
        var distance = wavelength - startWavelength;
        var pureChromaticity = new Chromaticity(
            Interpolation.Linear(segment.Start.X, segment.End.X, distance),
            Interpolation.Linear(segment.Start.Y, segment.End.Y, distance)
        );
        
        var useLineOfPurples = dominantWavelength < 0;
        if (useLineOfPurples)
        {
            var intersect = new Intersect(lineOfPurples, pureChromaticity, whiteChromaticity);
            pureChromaticity = intersect.Point;
        }

        return new(
            Interpolation.Linear(whiteChromaticity.X, pureChromaticity.X, purity),
            Interpolation.Linear(whiteChromaticity.Y, pureChromaticity.Y, purity)
        );
    }
    
    private double GetWavelength(Intersect intersect)
    {
        var index = Array.IndexOf(segments.Value, intersect.Segment);
        var startWavelength = MinWavelength + index;
        var endWavelength = startWavelength + 1;
        var distance = intersect.DistanceFromStart / intersect.Segment.Length;
        var wavelength = Interpolation.Linear(startWavelength, endWavelength, distance);
        return wavelength;
    }
    
    private Segment GetSegment(int startWavelength)
    {
        return startWavelength switch
        {
            <= MinWavelength => segments.Value.First(),
            >= MaxWavelength => segments.Value.Last(),
            _ => segments.Value[startWavelength - MinWavelength]
        };
    }
    
    private static Lazy<Chromaticity[]> GetPoints(Observer observer, Chromaticity white)
    {
        return new Lazy<Chromaticity[]>(() =>
        {
            var points = new List<Chromaticity>();
            for (var wavelength = MinWavelength; wavelength <= MaxWavelength; wavelength++)
            {
                var point = GetChromaticity(wavelength, observer, white);
                points.Add(point);
            }

            return points.ToArray();
        });
    }
    
    private static Chromaticity GetChromaticity(int wavelength, Observer observer, Chromaticity white)
    {
        var xyz = Xyz.FromSpd(Spd.Monochromatic(wavelength), observer);
        var xyy = Xyy.FromXyz(xyz, white);
        var chromaticity = xyy.Chromaticity;
        return chromaticity;
    }
}