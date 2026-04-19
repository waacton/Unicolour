using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

// using 1 nm segments since that is the granularity of the standard observer CMFs
internal class SpectralBoundary : Boundary
{
    private Segment lineOfPurples => segments.Value.Last();
    
    private readonly Lazy<double> minNegativeWavelength;
    private readonly Lazy<double> maxNegativeWavelength;
    
    internal const int MinWavelength = 360;
    internal const int MaxWavelength = 700;
    
    internal WhitePoint WhitePoint { get; }
    internal double MinNegativeWavelength => minNegativeWavelength.Value;
    internal double MaxNegativeWavelength => maxNegativeWavelength.Value;
    
    internal SpectralBoundary(Observer observer, WhitePoint whitePoint) : base(GetPoints(observer, whitePoint))
    {
        WhitePoint = whitePoint;
        
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
    
    internal (Intersect near, Intersect far)? GetIntersects(Chromaticity sample) => GetIntersects(sample, WhitePoint.Chromaticity);
    
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
    
    internal (double wavelength, double purity)? GetWavelengthAndPurity(Chromaticity sample, bool forDominant = true)
    {
        var intersects = GetIntersects(sample);
        if (!intersects.HasValue) return null;

        var (near, far) = intersects.Value;
        
        // the dominant intersect isn't always the near intersect
        // e.g. a sample slightly above white should use segment in the green area but line of purples is closer
        var dominant = far.DistanceFromSample < far.DistanceFromReference ? far : near;
        var complementary = dominant == near ? far : near;

        var primary = forDominant ? dominant : complementary;
        var secondary = forDominant ? complementary : dominant;
        
        var wavelength = primary.Segment == lineOfPurples ? -GetWavelength(secondary) : GetWavelength(primary);
        var purity = Distance(sample, WhitePoint.Chromaticity) / primary.DistanceFromReference;
        return (wavelength, purity);
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
            var intersect = new Intersect(lineOfPurples, pureChromaticity, WhitePoint.Chromaticity);
            pureChromaticity = intersect.Point;
        }

        return new(
            Interpolation.Linear(WhitePoint.Chromaticity.X, pureChromaticity.X, purity),
            Interpolation.Linear(WhitePoint.Chromaticity.Y, pureChromaticity.Y, purity)
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
    
    private static Lazy<Chromaticity[]> GetPoints(Observer observer, WhitePoint whitePoint)
    {
        return new Lazy<Chromaticity[]>(() =>
        {
            List<Chromaticity> points = [];
            for (var wavelength = MinWavelength; wavelength <= MaxWavelength; wavelength++)
            {
                var point = GetChromaticity(wavelength, observer, whitePoint);
                points.Add(point);
            }

            return points.ToArray();
        });
    }
    
    private static Chromaticity GetChromaticity(int wavelength, Observer observer, WhitePoint whitePoint)
    {
        var xyz = Xyz.FromSpd(Spd.Monochromatic(wavelength), observer, whitePoint);
        var xyy = Xyy.FromXyz(xyz);
        var chromaticity = xyy.Chromaticity;
        return chromaticity;
    }
}