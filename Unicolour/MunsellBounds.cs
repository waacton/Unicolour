namespace Wacton.Unicolour;

internal record MunsellBounds(MunsellHue LowerH, MunsellHue UpperH, double LowerV, double UpperV, int LowerC, int UpperC)
{
    internal MunsellHue LowerH { get; } = LowerH;
    internal MunsellHue UpperH { get; } = UpperH;
    internal double LowerV { get; } = LowerV;
    internal double UpperV { get; } = UpperV;
    internal int LowerC { get; } = LowerC;
    internal int UpperC { get; } = UpperC;

    internal ((int lower, int upper) lowerV, (int lower, int upper) upperV) BoundC => 
        (GetChromaBoundsWithinData(useLowerV: true), GetChromaBoundsWithinData(useLowerV: false));

    private (int lower, int upper) GetChromaBoundsWithinData(bool useLowerV)
    {
        // interpolation along the chroma ovoid is the core of the algorithm
        // so need to find a single chroma that is available for both hues at this value
        var v = useLowerV ? LowerV : UpperV;
        var rangeLowerH = MunsellCache.GetChromaRange(LowerH, v);
        var rangeUpperH = MunsellCache.GetChromaRange(UpperH, v);
        var maxSharedC = Math.Min(rangeLowerH.max, rangeUpperH.max);
        
        int lowerC;
        int upperC;

        if (maxSharedC == 0)
        {
            // if max shared C is 0, for at least one of the hues there is no chroma data at all
            // so find the smallest C for which one of the hues does have chroma data for
            // to enable approximation even with limited data
            var maxIndependentC = Math.Max(rangeLowerH.min, rangeUpperH.min);
            upperC = maxIndependentC;
            lowerC = 0; 
        }
        else if (UpperC > maxSharedC) 
        {
            // if upper > max shared, requested chroma doesn't exist in the munsell dataset for both hues
            // so return the two highest available chroma values for extrapolation
            upperC = maxSharedC;
            lowerC = Math.Max(upperC - 2, 0); // when there is no chroma data at all, max chroma is 0, so avoid negative chroma
        }
        else
        {
            upperC = UpperC;
            lowerC = LowerC;
        }

        return (lowerC, upperC);
    }


    internal (int min, int max)[] ChromaRanges => new[]
    {
        MunsellCache.GetChromaRange(LowerH, LowerV),
        MunsellCache.GetChromaRange(UpperH, LowerV),
        MunsellCache.GetChromaRange(LowerH, UpperV),
        MunsellCache.GetChromaRange(UpperH, UpperV)
    };

    internal bool IsSparseChroma => ChromaRanges.Count(x => x == (0, 0)) > 0;
    internal int ClosestUpperChromaLimit => ChromaRanges.Select(x => x.max).Min();
    internal double ChromaLimitScale => ClosestUpperChromaLimit == 0.0 ? 0.0 : UpperC / (double)ClosestUpperChromaLimit;
    
    public override string ToString()
    {
        return $"H: {LowerH} \u27f6 {UpperH} · V: {LowerV} \u27f6 {UpperV} · C: {LowerC:G} \u27f6 {UpperC:G}";
    }
}