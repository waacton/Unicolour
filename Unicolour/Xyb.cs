using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Xyb : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double X => First;
    public double Y => Second;
    public double B => Third;
    
    internal override bool IsGreyscale => Y <= 0.0 || (X.Equals(0.0) && B.Equals(0.0));

    public Xyb(double x, double y, double b) : this(x, y, b, ColourHeritage.None) {}
    internal Xyb(double x, double y, double b, ColourHeritage heritage) : base(x, y, b, heritage) {}
    
    protected override string String => $"{X:+0.000;-0.000;0.000} {Y:F3} {B:+0.000;-0.000;0.000}";
    public override string ToString() => base.ToString();
    
    /*
     * XYB is a transform of RGB Linear
     * Forward: https://ds.jpeg.org/whitepapers/jpeg-xl-whitepaper.pdf
     * Reverse: n/a - not provided, using own implementation
     * 
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    /*
     * NOTE: the final step of B -= Y is not documented in the JPEG XL white paper
     * but apparently the intention is that B = sGamma should be B = sGamma - Y 🤷
     * (at least it makes the colour greyscale when X == 0 and B == 0, in the same way as LAB-like spaces)
     * --------------------
     * 💩💩💩 the only mention of this "subtract Y from B" is just some random Twitter post and a 10 second YouTube video
     * so it's not particularly convincing that this extra step, which is left out of the white paper, should be performed
     * https://github.com/Evercoder/culori/issues/200 · https://twitter.com/jonsneyers/status/1605321352143331328 · https://www.youtube.com/watch?v=rvhf6feXw7w
     * modern colour spaces seem to make it hard to implement things accurately (see also: Oklab)
     */

    private const double Bias = 0.00379307325527544933;
    private static readonly double CubeRootBias = CubeRoot(Bias);
    
    private static readonly Matrix RgbToLmsMatrix = new(new[,]
    {
        { 0.3, 0.622, 0.078 },
        { 0.23, 0.692, 0.078 },
        { 0.24342268924547819, 0.20476744424496821, 0.55180986650955360 }
    });
    
    private static readonly Matrix LmsToXybMatrix = new(new[,]
    {
        { 0.5, -0.5, 0.0 },
        { 0.5, 0.5, 0.0 },
        { 0.0, 0.0, 1.0 }
    });
    
    internal static Xyb FromRgbLinear(RgbLinear rgb)
    {
        var rgbMatrix = Matrix.From(rgb);
        var lmsMixMatrix = RgbToLmsMatrix.Multiply(rgbMatrix).Select(value => value + Bias);
        var lmsGammaMatrix = lmsMixMatrix.Select(mix => CubeRoot(mix) - CubeRootBias);
        var xybMatrix = LmsToXybMatrix.Multiply(lmsGammaMatrix);
        var (x, y, b) = xybMatrix.ToTriplet();
        b -= y;
        return new Xyb(x, y, b, ColourHeritage.From(rgb));
    }
    
    internal static RgbLinear ToRgbLinear(Xyb xyb)
    {
        var (x, y, b) = xyb;
        b += y;
        var xybMatrix = Matrix.From(x, y, b);
        var lmsGammaMatrix = LmsToXybMatrix.Inverse().Multiply(xybMatrix);
        var lmsMixMatrix = lmsGammaMatrix.Select(gamma => Math.Pow(gamma + CubeRootBias, 3));
        var rgbMatrix = RgbToLmsMatrix.Inverse().Multiply(lmsMixMatrix.Select(mix => mix - Bias));
        var (red, green, blue) = rgbMatrix.ToTriplet();
        return new RgbLinear(red, green, blue, ColourHeritage.From(xyb));
    }
}