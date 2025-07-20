namespace Wacton.Unicolour.Experimental;

internal record MunsellBounds(Munsell.MunsellHue LowerH, Munsell.MunsellHue UpperH, double LowerV, double UpperV, double LowerC, double UpperC)
{
    internal Munsell.MunsellHue LowerH { get; } = LowerH;
    internal Munsell.MunsellHue UpperH { get; } = UpperH;
    internal double LowerV { get; } = LowerV;
    internal double UpperV { get; } = UpperV;
    internal double LowerC { get; } = LowerC;
    internal double UpperC { get; } = UpperC;

    internal (double lower, double upper) GetChromaBoundsWithinData(bool useLowerV)
    {
        // interpolation along the chroma ovoid is the core of the algorithm
        // so need to find a single chroma that is available for both hues at this value
        var v = useLowerV ? LowerV : UpperV;
        var maxForLowerH = MunsellCache.GetChromaLimits(LowerH, v).max;
        var maxForUpperH = MunsellCache.GetChromaLimits(UpperH, v).max;
        var maxC = Math.Min(maxForLowerH, maxForUpperH);
        
        double lowerC;
        double upperC;
        
        // if upper > max, requested chroma doesn't exist in the munsell dataset
        // so return the two highest available chroma values for extrapolation
        if (UpperC > maxC) 
        {
            upperC = maxC;
            lowerC = Math.Max(upperC - 2, 0); // when there is no chroma data at all, max chroma is 0, so avoid negative chroma
        }
        else
        {
            upperC = UpperC;
            lowerC = LowerC;
        }

        return (lowerC, upperC);
    }
    
    internal readonly (int min, int max)[] ChromaLimits =
    {
        MunsellCache.GetChromaLimits(LowerH, LowerV),
        MunsellCache.GetChromaLimits(UpperH, LowerV),
        MunsellCache.GetChromaLimits(LowerH, UpperV),
        MunsellCache.GetChromaLimits(UpperH, UpperV)
    };
    
    internal int ClosestUpperChromaLimit => ChromaLimits.Select(x => x.max).Min();
    internal double ChromaLimitScale => ClosestUpperChromaLimit == 0.0 ? 0.0 : UpperC / ClosestUpperChromaLimit;
    
    public override string ToString()
    {
        return $"H: {LowerH} \u27f6 {UpperH} · V: {LowerV} \u27f6 {UpperV} · C: {LowerC:G} \u27f6 {UpperC:G}";
    }
}