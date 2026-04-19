using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Lab : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double L => First;
    public double A => Second;
    public double B => Third;
    
    protected override bool IsTripletAchromatic => A == 0.0 && B == 0.0;
    
    public Lab(double l, double a, double b) : this(l, a, b, Limitation.None) {}
    public Lab(double l) : this(l, 0, 0, Limitation.Achromatic) {}
    internal Lab(double l, double a, double b, Limitation limitation) : base(l, a, b, limitation) {}
    
    protected override string String => $"{L:F2} {A:+0.00;-0.00;0.00} {B:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * LAB is a transform of XYZ 
     * Forward: https://en.wikipedia.org/wiki/CIELAB_color_space#From_CIEXYZ_to_CIELAB
     * Reverse: https://en.wikipedia.org/wiki/CIELAB_color_space#From_CIELAB_to_CIEXYZ
     */

    // ReSharper disable InconsistentNaming
    private const double delta = 6.0 / 29.0;
    // ReSharper restore InconsistentNaming

    internal static Lab FromXyz(Xyz xyz)
    {
        var (x, y, z) = xyz;
        var whitePoint = xyz.WhitePoint;
        var xRatio = x / whitePoint.X;
        var yRatio = y / whitePoint.Y;
        var zRatio = z / whitePoint.Z;
        
        double F(double t) => t > Math.Pow(delta, 3) ? CubeRoot(t) : t * (1 / 3.0) * Math.Pow(delta, -2) + 4.0 / 29.0;
        var l = 116 * F(yRatio) - 16;
        var a = 500 * (F(xRatio) - F(yRatio));
        var b = 200 * (F(yRatio) - F(zRatio));
        return new Lab(l, a, b, xyz.Limitation);
    }

    internal static Xyz ToXyz(Lab lab, WhitePoint whitePoint)
    {
        var (l, a, b) = lab;
        double F(double t) => t > delta ? Math.Pow(t, 3.0) : 3 * Math.Pow(delta, 2) * (t - 4.0 / 29.0);
        var x = whitePoint.X * F((l + 16) / 116.0 + a / 500.0);
        var y = whitePoint.Y * F((l + 16) / 116.0);
        var z = whitePoint.Z * F((l + 16) / 116.0 - b / 200.0);
        return new Xyz(x, y, z, whitePoint, lab.Limitation);
    }
}