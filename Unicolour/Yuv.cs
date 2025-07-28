namespace Wacton.Unicolour;

public record Yuv : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double Y => First;
    public double U => Second;
    public double V => Third;
    public double ConstrainedY => ConstrainedFirst;
    public double ConstrainedU => ConstrainedSecond;
    public double ConstrainedV => ConstrainedThird;
    protected override double ConstrainedFirst => Y.Clamp(0.0, 1.0);
    protected override double ConstrainedSecond => U.Clamp(-UMax, UMax);
    protected override double ConstrainedThird => V.Clamp(-VMax, VMax);
    internal override bool IsGreyscale => U.Equals(0.0) && V.Equals(0.0); // Y = 0 does not imply black; Y = 1 does not imply white

    public Yuv(double y, double u, double v) : this(y, u, v, ColourHeritage.None) {}
    internal Yuv(ColourTriplet triplet, ColourHeritage heritage) : this(triplet.First, triplet.Second, triplet.Third, heritage) {}
    internal Yuv(double y, double u, double v, ColourHeritage heritage) : base(y, u, v, heritage) {}
    
    protected override string String => $"{Y:F3} {U:+0.000;-0.000;0.000} {V:+0.000;-0.000;0.000}";
    public override string ToString() => base.ToString();
    
    /*
     * YUV is a transform of RGB
     * Forward: https://en.wikipedia.org/wiki/Y%E2%80%B2UV#Conversion_to/from_RGB
     * Reverse: https://en.wikipedia.org/wiki/Y%E2%80%B2UV#Conversion_to/from_RGB
     */

    // basically the same transform as YPbPr with YccConfig.Bt601, except different scaling than -0.5 to 0.5
    internal const double Wr = 0.299;
    internal const double Wb = 0.114;
    internal const double Wg = 1 - Wr - Wb;
    internal const double UMax = 0.436;
    internal const double VMax = 0.614;
    
    internal static Yuv FromRgb(Rgb rgb)
    {
        var (r, g, b) = rgb;
        var y = Wr * r + Wg * g + Wb * b;
        var u = UMax * ((b - y) / (1 - Wb));
        var v = VMax * ((r - y) / (1 - Wr));
        return new Yuv(y, u, v, ColourHeritage.From(rgb));
    }
    
    internal static Rgb ToRgb(Yuv yuv)
    {
        var (y, u, v) = yuv;
        var r = v * ((1 - Wr) / VMax) + y;
        var b = u * ((1 - Wb) / UMax) + y;
        var g = (y - Wr * r - Wb * b) / Wg;
        return new Rgb(r, g, b, ColourHeritage.From(yuv));
    }
}