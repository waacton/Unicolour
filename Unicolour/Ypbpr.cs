namespace Wacton.Unicolour;

public record Ypbpr : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double Y => First;
    public double Pb => Second;
    public double Pr => Third;
    
    protected override bool IsAchromatic => Pb == 0.0 && Pr == 0.0;
    
    public Ypbpr(double y, double pb, double pr) : this(y, pb, pr, Limitation.None) {}
    internal Ypbpr(double y, double pb, double pr, Limitation limitation) : base(y, pb, pr, limitation) {}
    
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
        return new Ypbpr(y, pb, pr, rgb.Limitation);
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
        return new Rgb(r, g, b, ypbpr.Limitation);
    }
}