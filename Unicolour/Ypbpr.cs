namespace Wacton.Unicolour;

public record Ypbpr : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double Y => First;
    public double Pb => Second;
    public double Pr => Third;
    public double ConstrainedY => ConstrainedFirst;
    public double ConstrainedPb => ConstrainedSecond;
    public double ConstrainedPr => ConstrainedThird;
    protected override double ConstrainedFirst => Y.Clamp(0.0, 1.0);
    protected override double ConstrainedSecond => Pb.Clamp(-0.5, 0.5);
    protected override double ConstrainedThird => Pr.Clamp(-0.5, 0.5);
    internal override bool IsGreyscale => Pb.Equals(0.0) && Pr.Equals(0.0); // Y = 0 does not imply black; Y = 1 does not imply white

    public Ypbpr(double y, double pb, double pr) : this(y, pb, pr, ColourHeritage.None) {}
    internal Ypbpr(double y, double pb, double pr, ColourHeritage heritage) : base(y, pb, pr, heritage) {}
    
    protected override string String => $"{Y:F3} {Pb:+0.000;-0.000;0.000} {Pr:+0.000;-0.000;0.000}";
    public override string ToString() => base.ToString();
    
    /*
     * YPbPr is a transform of RGB
     * Forward: https://en.wikipedia.org/wiki/YCbCr#R'G'B'_to_Y%E2%80%B2PbPr
     * Reverse: https://en.wikipedia.org/wiki/YCbCr#R'G'B'_to_Y%E2%80%B2PbPr
     */
    
    internal static Ypbpr FromRgb(Rgb rgb, YbrConfiguration ybrConfig)
    {
        var kr = ybrConfig.Kr;
        var kb = ybrConfig.Kb;
        var kg = ybrConfig.Kg;
        
        var (r, g, b) = rgb;
        var y = kr * r + kg * g + kb * b;
        var pb = 0.5 * ((b - y) / (1 - kb));
        var pr = 0.5 * ((r - y) / (1 - kr));
        return new Ypbpr(y, pb, pr, ColourHeritage.From(rgb));
    }
    
    internal static Rgb ToRgb(Ypbpr ypbpr, YbrConfiguration ybrConfig)
    {
        var kr = ybrConfig.Kr;
        var kb = ybrConfig.Kb;
        var kg = ybrConfig.Kg;
        
        var (y, pb, pr) = ypbpr;
        var r = pr * ((1 - kr) / 0.5) + y;
        var b = pb * ((1 - kb) / 0.5) + y;
        var g = (y - kr * r - kb * b) / kg;
        return new Rgb(r, g, b, ColourHeritage.From(ypbpr));
    }
}