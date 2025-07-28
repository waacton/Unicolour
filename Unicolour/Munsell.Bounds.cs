namespace Wacton.Unicolour;

public partial record Munsell
{
    // there are 10 letter bands, so each letter band represents 36° of 360° (R = 0-36°, YR = 36-72°, ..., RR = 324-360°)
    // there are 4 numbers in each letter band of 36°, so each number represents 9° of 36° (2.5 = 9°, 5 = 18°, 7.5 = 27°, 10 = 36°)
    internal static readonly string[] Hues = { "R", "YR", "Y", "GY", "G", "BG", "B", "PB", "P", "RP" };
    internal const double DegreesPerHueLetter = 36;
    internal const double DegreesPerHueNumber = 9;
    private const int ValueStep = 1;
    private const double LowValueStep = 0.2;
    private const int ChromaStep = 2;
    
    internal static Bounds GetBounds(Munsell munsell)
    {
        var (h, v, c) = munsell.ConstrainedTriplet;
        
        // these are the naive bounds, and will be adjusted if not available in the dataset
        // e.g. the chroma must exist for both hue/value/lowerChroma and hue/value/upperChroma to be used for interpolation
        //      if it doesn't, a different chroma that exists for both will be used
        var (lowerH, upperH) = ToIntervals(h, DegreesPerHueNumber);
        var (lowerV, upperV) = ToIntervals(v, v < 1.0 ? LowValueStep : ValueStep);
        var (lowerC, upperC) = ToIntervals(c, ChromaStep);
        return new Bounds(lowerH.Modulo(360), upperH.Modulo(360), lowerV, upperV, lowerC, upperC);
    }
    
    private static (double lower, double upper) ToIntervals(double number, double interval)
    {
        var scaled = number / interval;
        var lower = Math.Floor(scaled) * interval;
        var upper = Math.Ceiling(scaled) * interval;
        return (Math.Round(lower, 1), Math.Round(upper, 1));
    }
    
    internal record Bounds(double LowerH, double UpperH, double LowerV, double UpperV, double LowerC, double UpperC)
    {
        internal double LowerH { get; } = LowerH;
        internal double UpperH { get; } = UpperH;
        internal double LowerV { get; } = LowerV;
        internal double UpperV { get; } = UpperV;
        internal double LowerC { get; } = LowerC;
        internal double UpperC { get; } = UpperC;
        
        internal ((double lower, double upper) forLowerV, (double lower, double upper) forUpperV) BoundC => 
            (GetChromaBoundsWithinData(useLowerV: true), GetChromaBoundsWithinData(useLowerV: false));
        
        private (double lower, double upper) GetChromaBoundsWithinData(bool useLowerV)
        {
            // interpolation along the chroma ovoid is the core of the algorithm
            // so need to find a single chroma that is available for both hues at this value
            var v = useLowerV ? LowerV : UpperV;
            var rangeLowerH = GetChromaRange(LowerH, v);
            var rangeUpperH = GetChromaRange(UpperH, v);
            var maxSharedC = Math.Min(rangeLowerH.max, rangeUpperH.max);
            
            double lowerC;
            double upperC;

            if (maxSharedC == 0)
            {
                // if max shared C is 0, for at least one of the hues there is no chroma data at all
                // so find the smallest C for which one of the hues does have chroma data for
                // to enable approximation even with limited data
                upperC = Math.Max(rangeLowerH.min, rangeUpperH.min);
                lowerC = 0; 
            }
            else if (UpperC > maxSharedC) 
            {
                // if upper > max shared, requested chroma doesn't exist in the munsell dataset for both hues
                // so return the two highest available chroma values for extrapolation
                upperC = maxSharedC;
                lowerC = Math.Max(maxSharedC - ChromaStep, 0); // when there is no chroma data at all, max chroma is 0, so avoid negative chroma
            }
            else
            {
                upperC = Math.Max(UpperC, 0);
                lowerC = Math.Max(LowerC, 0);
            }

            return (lowerC, upperC);
        }
        
        private static readonly Dictionary<(double h, double v), (int min, int max)> ChromaRange = new();
        internal static (int min, int max) GetChromaRange(double h, double v)
        {
            var key = (h, v);
            if (ChromaRange.TryGetValue(key, out var range))
            {
                return range;
            }
        
            var nodes = Node.LookupCache.Values.Where(x => x.Hue == h && x.Value == v).ToArray();
            range = !nodes.Any() ? (0, 0) : (nodes.Min(x => x.Chroma), nodes.Max(x => x.Chroma));
            ChromaRange[key] = range;
            return range;
        }
    }
}

