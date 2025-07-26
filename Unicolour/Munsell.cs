namespace Wacton.Unicolour;

public record Munsell : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    internal MunsellHue Hue { get; }
    public (double number, string letter) H => (Hue.Number, Hue.Letter);
    public double V => Second;
    public double C => Third;
    protected override double ConstrainedFirst => Hue.Degrees.Modulo(360.0);
    protected override double ConstrainedSecond => Math.Max(V, 0);
    protected override double ConstrainedThird => Math.Max(C, 0);
    internal override bool IsGreyscale => V <= 0.0 || C <= 0.0;

    protected override string String => UseAsHued ? $"{H.number:0.##}{H.letter} {V:0.##}/{C:0.##}" : $"N {V:0.##}/";
    public override string ToString() => base.ToString();

    public Munsell(double h1, string h2, double v, double c) : this(new MunsellHue(h1, h2), v, c, ColourHeritage.None) { }
    public Munsell(double v) : this(new MunsellHue(0), v, 0, ColourHeritage.Greyscale) { }
    internal Munsell(double h, double v, double c) : this(new MunsellHue(h), v, c, ColourHeritage.None) { }
    internal Munsell(double h, double v, double c, ColourHeritage heritage) : this(new MunsellHue(h), v, c, heritage) { }
    private Munsell(MunsellHue h, double v, double c, ColourHeritage heritage) : base(h.Degrees, v, c, heritage)
    {
        Hue = h;
    }
    
    internal static MunsellBounds GetBounds(Munsell munsell)
    {
        var (h, v, c) = munsell.ConstrainedTriplet;
        
        // these are the naive bounds, and will be adjusted if not available in the dataset
        // e.g. the chroma must exist for both hue/value/lowerChroma and hue/value/upperChroma to be used for interpolation
        //      if it doesn't, a different chroma that exists for both will be used
        var (lowerH, upperH) = MunsellFuncs.ToIntervals(h, Node.DegreesPerHueNumber);
        var (lowerV, upperV) = MunsellFuncs.ToIntervals(v, v < 1.0 ? 0.2 : 1);
        var (lowerC, upperC) = MunsellFuncs.ToIntervals(c, 2);
        return new MunsellBounds(new(lowerH.Modulo(360)), new(upperH.Modulo(360)), lowerV, upperV, lowerC, upperC);
    }

    // only for potential debugging or diagnostics
    internal MunsellFuncs.XyyToMunsellSearchResult? XyyToMunsellSearchResult;
}