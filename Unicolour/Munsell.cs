namespace Wacton.Unicolour;

// TODO: handle grey ("N" hue e.g. N 5/ ... or 0 chroma e.g. 10YR 5/0)
// TODO: clamp hue between 0 - 10, clamp value to 10, handle extreme chroma values
public record Munsell : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    internal MunsellHue Hue { get; }
    public (double number, string letter) H => (Hue.Number, Hue.Letter);
    public double V => Second;
    public double C => Third;
    internal override bool IsGreyscale => V <= 0.0 || C <= 0.0; // TODO: what about V > 1? probably

    protected override string String => UseAsHued ? $"{H.number:0.##}{H.letter} {V:0.##}/{C:0.##}" : $"N {V:0.##}/";
    public override string ToString() => base.ToString();
    

    private readonly Lazy<MunsellBounds> bounds;
    internal MunsellBounds Bounds => bounds.Value;

    public Munsell(double h1, string h2, double v, double c) : this(new MunsellHue(h1, h2), v, c) { }
    internal Munsell(double h, double v, double c) : this(new MunsellHue(h), v, c) { }
    private Munsell(MunsellHue h, double v, double c) : base(h.Degrees, v, c, ColourHeritage.None)
    {
        Hue = h;
        bounds = new Lazy<MunsellBounds>(GetBounds);
    }

    // public Munsell(double greyNumber)
    // {
    //     // HueDegrees = 0;
    //     // Value = null;
    //     // Chroma = null;
    // }

    private MunsellBounds GetBounds()
    {
        // these are the naive bounds, and will be adjusted if not available in the dataset
        // e.g. the chroma must exist for both hue/value/lowerChroma and hue/value/upperChroma to be used for interpolation
        //      if it doesn't, a different chroma that exists for both will be used
        var (lowerH, upperH)= MunsellFuncs.ToIntervals(Hue.Degrees, Node.DegreesPerHueNumber);
        var (lowerV, upperV) = MunsellFuncs.ToIntervals(V, V < 1.0 ? 0.2 : 1);
        var (lowerC, upperC) = MunsellFuncs.ToIntervals(C, 2);
        return new MunsellBounds(new(lowerH), new(upperH), lowerV, upperV, (int)lowerC, (int)upperC);
    }

    // only for potential debugging or diagnostics
    internal MunsellFuncs.XyyToMunsellSearchResult? XyyToMunsellSearchResult;
}