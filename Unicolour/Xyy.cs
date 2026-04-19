namespace Wacton.Unicolour;

public record Xyy : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public Chromaticity Chromaticity => new(First, Second);
    public double Luminance => Third;
    
    internal WhitePoint WhitePoint { get; }
    protected override bool IsTripletAchromatic => Chromaticity == WhitePoint.Chromaticity;

    public Xyy(double x, double y, double luminance, WhitePoint whitePoint) : this(x, y, luminance, whitePoint, Limitation.None) {}
    public Xyy(double luminance, WhitePoint whitePoint) : this(whitePoint.Chromaticity.X, whitePoint.Chromaticity.Y, luminance, whitePoint, Limitation.Achromatic) {}
    internal Xyy(double x, double y, double luminance, WhitePoint whitePoint, Limitation limitation) : base(x, y, luminance, limitation)
    {
        WhitePoint = whitePoint;
    }

    protected override string String => $"{Chromaticity.X:F4} {Chromaticity.Y:F4} {Luminance:F4}";
    public override string ToString() => base.ToString();
    
    /*
     * XYY is a transform of XYZ (in terms of Unicolour implementation)
     * Forward: https://en.wikipedia.org/wiki/CIE_1931_color_space#CIE_xy_chromaticity_diagram_and_the_CIE_xyY_color_space
     * Reverse: https://en.wikipedia.org/wiki/CIE_1931_color_space#CIE_xy_chromaticity_diagram_and_the_CIE_xyY_color_space
     */
    
    internal static Xyy FromXyz(Xyz xyz)
    {
        var (x, y, z) = xyz;
        var (chromaticity, luminance) = FromXyz(x, y, z, fallback: xyz.WhitePoint.Chromaticity);
        return new Xyy(chromaticity.X, chromaticity.Y, luminance, xyz.WhitePoint, xyz.Limitation);
    }

    // this is separated out to allow Xyz -> Xyy conversion without white point context
    // only intended for use to define actual white points (which themselves are the context!)
    internal static (Chromaticity chromaticity, double luminance) FromXyz(double x, double y, double z, Chromaticity fallback)
    {
        var normalisation = x + y + z;
        var useFallback = normalisation == 0.0;

        var chromaticity = useFallback ? fallback : new Chromaticity(x / normalisation, y / normalisation);
        var luminance = useFallback ? 0 : y;
        return (chromaticity, luminance);
    }
    
    internal static Xyz ToXyz(Xyy xyy)
    {
        var (x, y, z) = ToXyz(xyy.Chromaticity, xyy.Luminance);
        return new Xyz(x, y, z, xyy.WhitePoint, xyy.Limitation);
    }
    
    // this is separated out to allow Xyy -> Xyz conversion without white point context
    // only intended for use to define actual white points (which themselves are the context!)
    internal static (double x, double y, double z) ToXyz(Chromaticity chromaticity, double luminance)
    {
        // X and Z become a vertical asymptote when chromaticity.Y == 0, so neither positive or negative infinity are suitable
        // implementations typically fall back to (0, 0, 0) black but that seems misleading
        // this approach preserves the luminance while making it clear that chromaticity.Y of 0 is not a valid value (despite it being a valid coordinate)
        var useFallback = chromaticity.Y == 0.0;
        if (useFallback)
        {
            return (double.NaN, luminance, double.NaN);
        }
        
        var factor = luminance / chromaticity.Y;
        var x = factor * chromaticity.X;
        var y = luminance;
        var z = factor * (1 - chromaticity.X - chromaticity.Y);
        return (x, y, z);
    }
}