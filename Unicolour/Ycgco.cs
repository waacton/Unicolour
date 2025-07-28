namespace Wacton.Unicolour;

public record Ycgco : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double Y => First;
    public double Cg => Second;
    public double Co => Third;
    public double ConstrainedY => ConstrainedFirst;
    public double ConstrainedCg => ConstrainedSecond;
    public double ConstrainedCo => ConstrainedThird;
    protected override double ConstrainedFirst => Y.Clamp(0.0, 1.0);
    protected override double ConstrainedSecond => Cg.Clamp(-0.5, 0.5);
    protected override double ConstrainedThird => Co.Clamp(-0.5, 0.5);
    internal override bool IsGreyscale => Cg.Equals(0.0) && Co.Equals(0.0); // Y = 0 does not imply black; Y = 1 does not imply white

    public Ycgco(double y, double cg, double co) : this(y, cg, co, ColourHeritage.None) {}
    internal Ycgco(double y, double cg, double co, ColourHeritage heritage) : base(y, cg, co, heritage) {}
    
    protected override string String => $"{Y:F3} {Cg:+0.000;-0.000;0.000} {Co:+0.000;-0.000;0.000}";
    public override string ToString() => base.ToString();
    
    /*
     * YCgCo is a transform of RGB
     * Forward: https://en.wikipedia.org/wiki/YCoCg#Conversion_with_the_RGB_color_model
     * Reverse: https://en.wikipedia.org/wiki/YCoCg#Conversion_with_the_RGB_color_model
     */
    
    internal static Ycgco FromRgb(Rgb rgb)
    {
        var (r, g, b) = rgb;
        var y = 0.25 * r + 0.5 * g + 0.25 * b;
        var cg = -0.25 * r + 0.5 * g - 0.25 * b;
        var co = 0.5 * r - 0.5 * b;
        return new Ycgco(y, cg, co, ColourHeritage.From(rgb));
    }
    
    internal static Rgb ToRgb(Ycgco ycgco)
    {
        var (y, cg, co) = ycgco;
        var r = y + co - cg;
        var g = y + cg;
        var b = y - co - cg;
        return new Rgb(r, g, b, ColourHeritage.From(ycgco));
    }
}