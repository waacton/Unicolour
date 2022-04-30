namespace Wacton.Unicolour;

public record Rgb
{
    public double R { get; }
    public double G { get; }
    public double B { get; }
    public ColourTriplet Triplet => new(R, G, B);

    public double ConstrainedR => R.Clamp(0.0, 1.0);
    public double ConstrainedG => G.Clamp(0.0, 1.0);
    public double ConstrainedB => B.Clamp(0.0, 1.0);
    public ColourTriplet ConstrainedTriplet => new(ConstrainedR, ConstrainedG, ConstrainedB);

    public int R255 => (int) Math.Round(ConstrainedR * 255);
    public int G255 => (int) Math.Round(ConstrainedG * 255);
    public int B255 => (int) Math.Round(ConstrainedB * 255);
    public ColourTriplet Triplet255 => new(R255, G255, B255);

    private readonly Func<double, double> inverseCompand;
    public double RLinear => inverseCompand(R);
    public double GLinear => inverseCompand(G);
    public double BLinear => inverseCompand(B);
    public ColourTriplet TripletLinear => new(RLinear, GLinear, BLinear);
    
    public double ConstrainedRLinear => RLinear.Clamp(0.0, 1.0);
    public double ConstrainedGLinear => GLinear.Clamp(0.0, 1.0);
    public double ConstrainedBLinear => BLinear.Clamp(0.0, 1.0);
    public ColourTriplet ConstrainedTripletLinear => new(ConstrainedRLinear, ConstrainedGLinear, ConstrainedBLinear);

    public string Hex => $"#{R255:X2}{G255:X2}{B255:X2}";

    // TODO: move this detail (and anything similar) to readme?
    // not using a precision-based tolerance comparison because initial values are always provided by external code
    // and do not want to make assumptions about the intentions of those values (e.g. R set to 1/3.0 and G set to 0.33333333, won't assume they "should" be the same)
    internal bool IsMonochrome => ConvertedFromMonochrome || ConstrainedR.Equals(ConstrainedG) && ConstrainedG.Equals(ConstrainedB);
    internal bool ConvertedFromMonochrome { get; }

    public Rgb(double r, double g, double b, Configuration config): this(r, g, b, config, false) {}
    internal Rgb(double r, double g, double b, Configuration config, bool convertedFromMonochrome)
    {
        R = r;
        G = g;
        B = b;
        inverseCompand = config.InverseCompand;
        ConvertedFromMonochrome = convertedFromMonochrome;
    }
    
    public override string ToString() => $"{R255} {G255} {B255}";
}