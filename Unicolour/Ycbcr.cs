namespace Wacton.Unicolour;

public record Ycbcr : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double Y => First;
    public double Cb => Second;
    public double Cr => Third;
    public double ConstrainedY => ConstrainedFirst;
    public double ConstrainedCb => ConstrainedSecond;
    public double ConstrainedCr => ConstrainedThird;
    protected override double ConstrainedFirst => Y.Clamp(0.0, 255.0);
    protected override double ConstrainedSecond => Cb.Clamp(0.0, 255.0);
    protected override double ConstrainedThird => Cr.Clamp(0.0, 255.0);
    internal override bool IsGreyscale => Cb.Equals(128.0) && Cr.Equals(128.0); // Y = 0 does not imply black; Y = 1 does not imply white

    public Ycbcr(double y, double cb, double cr) : this(y, cb, cr, ColourHeritage.None) {}
    internal Ycbcr(double y, double cb, double cr, ColourHeritage heritage) : base(y, cb, cr, heritage) {}
    
    protected override string String => $"{Y:F0} {Cb:0} {Cr:0}";
    public override string ToString() => base.ToString();
    
    /*
     * YCbCr is a transform of YPbPr
     * Forward: https://en.wikipedia.org/wiki/YCbCr#ITU-R_BT.601_conversion
     * Reverse: https://en.wikipedia.org/wiki/YCbCr#ITU-R_BT.601_conversion
     */
    
    internal static Ycbcr FromYpbpr(Ypbpr ypbpr, YbrConfiguration ybrConfig)
    {
        var rangeY = ybrConfig.RangeY;
        var rangeC = ybrConfig.RangeC;
        
        var (y, pb, pr) = ypbpr;
        var yDigital = rangeY.min + (rangeY.max - rangeY.min) * y;
        var cb = 128 + (rangeC.max - rangeC.min) * pb;
        var cr = 128 + (rangeC.max - rangeC.min) * pr;
        return new Ycbcr(yDigital, cb, cr, ColourHeritage.From(ypbpr));
    }
    
    internal static Ypbpr ToYpbpr(Ycbcr ycbcr, YbrConfiguration ybrConfig)
    {
        var rangeY = ybrConfig.RangeY;
        var rangeC = ybrConfig.RangeC;
        
        var (yDigital, cb, cr) = ycbcr;
        var y = (yDigital - rangeY.min) / (rangeY.max - rangeY.min);
        var pb = (cb - 128) / (rangeC.max - rangeC.min);
        var pr = (cr - 128) / (rangeC.max - rangeC.min);
        return new Ypbpr(y, pb, pr, ColourHeritage.From(ycbcr));
    }
}