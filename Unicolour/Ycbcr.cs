namespace Wacton.Unicolour;

public record Ycbcr : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double Y => First;
    public double Cb => Second;
    public double Cr => Third;
    
    protected override bool IsTripletAchromatic => Cb == 128.0 && Cr == 128.0;
    
    public Ycbcr(double y, double cb, double cr) : this(y, cb, cr, Limitation.None) {}
    public Ycbcr(double y) : this(y, 128, 128, Limitation.Achromatic) {}
    internal Ycbcr(double y, double cb, double cr, Limitation limitation) : base(y, cb, cr, limitation) {}

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
        return new Ycbcr(yDigital, cb, cr, ypbpr.Limitation);
    }
    
    internal static Ypbpr ToYpbpr(Ycbcr ycbcr, YbrConfiguration ybrConfig)
    {
        var rangeY = ybrConfig.RangeY;
        var rangeC = ybrConfig.RangeC;
        
        var (yDigital, cb, cr) = ycbcr;
        var y = (yDigital - rangeY.min) / (rangeY.max - rangeY.min);
        var pb = (cb - 128) / (rangeC.max - rangeC.min);
        var pr = (cr - 128) / (rangeC.max - rangeC.min);
        return new Ypbpr(y, pb, pr, ycbcr.Limitation);
    }
}