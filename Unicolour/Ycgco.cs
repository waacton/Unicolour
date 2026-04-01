namespace Wacton.Unicolour;

public record Ycgco : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double Y => First;
    public double Cg => Second;
    public double Co => Third;
    
    protected override bool IsAchromatic => Cg == 0.0 && Co == 0.0;
    
    public Ycgco(double y, double cg, double co) : this(y, cg, co, Limitation.None) {}
    internal Ycgco(double y, double cg, double co, Limitation limitation) : base(y, cg, co, limitation) {}
    
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
        return new Ycgco(y, cg, co, rgb.Limitation);
    }
    
    internal static Rgb ToRgb(Ycgco ycgco)
    {
        var (y, cg, co) = ycgco;
        var r = y + co - cg;
        var g = y + cg;
        var b = y - co - cg;
        return new Rgb(r, g, b, ycgco.Limitation);
    }
}