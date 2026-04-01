using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Luv : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double L => First;
    public double U => Second;
    public double V => Third;
    
    protected override bool IsAchromatic => U == 0.0 && V == 0.0;
    
    public Luv(double l, double u, double v) : this(l, u, v, Limitation.None) {}
    internal Luv(double l, double u, double v, Limitation limitation) : base(l, u, v, limitation) {}
    
    protected override string String => $"{L:F2} {U:+0.00;-0.00;0.00} {V:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * LUV is a transform of XYZ 
     * Forward: https://en.wikipedia.org/wiki/CIELUV#The_forward_transformation
     * Reverse: https://en.wikipedia.org/wiki/CIELUV#The_reverse_transformation
     */
    
    internal static Luv FromXyz(Xyz xyz)
    {
        var (x, y, z) = xyz;
        var (xRef, yRef, zRef) = xyz.WhitePoint;

        (x, y, z) = Scale(x, y, z);
        (xRef, yRef, zRef) = Scale(xRef, yRef, zRef);

        double U(double xu, double yu, double zu) => 4 * xu / (xu + 15 * yu + 3 * zu);
        double V(double xv, double yv, double zv) => 9 * yv / (xv + 15 * yv + 3 * zv);
        var uPrime = U(x, y, z);
        var uPrimeRef = U(xRef, yRef, zRef);
        var vPrime = V(x, y, z);
        var vPrimeRef = V(xRef, yRef, zRef);
        
        var yRatio = y / yRef;
        var l = yRatio > Math.Pow(6.0 / 29.0, 3) ? 116 * CubeRoot(yRatio) - 16 : Math.Pow(29 / 3.0, 3) * yRatio;
        var u = 13 * l * (uPrime - uPrimeRef);
        var v = 13 * l * (vPrime - vPrimeRef);
        
        l = double.IsNaN(l) ? 0.0 : l;
        u = double.IsNaN(u) ? 0.0 : u;
        v = double.IsNaN(v) ? 0.0 : v;
        return new Luv(l, u, v, xyz.Limitation);
    }
    
    internal static Xyz ToXyz(Luv luv, WhitePoint whitePoint)
    {
        var (l, u, v) = luv;
        
        var (xRef, yRef, zRef) = whitePoint;
        (xRef, yRef, zRef) = Scale(xRef, yRef, zRef);
        
        double U(double x, double y, double z) => 4 * x / (x + 15 * y + 3 * z);
        double V(double x, double y, double z) => 9 * y / (x + 15 * y + 3 * z);
        var uPrimeRef = U(xRef, yRef, zRef);
        var uPrime = u / (13 * l) + uPrimeRef;
        var vPrimeRef = V(xRef, yRef, zRef);
        var vPrime = v / (13 * l) + vPrimeRef;

        var y = (l > 8 ? yRef * Math.Pow((l + 16) / 116.0, 3) : yRef * l * Math.Pow(3 / 29.0, 3)) / 100.0;
        var x = y * (9 * uPrime / (4 * vPrime));
        var z = y * ((12 - 3 * uPrime - 20 * vPrime) / (4 * vPrime));
        
        x = double.IsNaN(x) || double.IsInfinity(x) ? 0.0 : x;
        y = double.IsNaN(y) || double.IsInfinity(y) ? 0.0 : y;
        z = double.IsNaN(z) || double.IsInfinity(z) ? 0.0 : z;
        return new Xyz(x, y, z, whitePoint, luv.Limitation);
    }
    
    private static (double x, double y, double z) Scale(double x, double y, double z) => (x * 100, y * 100, z * 100);
}