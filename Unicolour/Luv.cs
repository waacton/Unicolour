using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Luv : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double L => First;
    public double U => Second;
    public double V => Third;
    
    protected override bool IsTripletAchromatic => U == 0.0 && V == 0.0;
    
    public Luv(double l, double u, double v) : this(l, u, v, Limitation.None) {}
    public Luv(double l) : this(l, 0, 0, Limitation.Achromatic) {}
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

        var uPrime = GetU(x, y, z);
        var uPrimeRef = GetU(xRef, yRef, zRef);
        var vPrime = GetV(x, y, z);
        var vPrimeRef = GetV(xRef, yRef, zRef);
        
        var yRatio = y / yRef;
        var l = yRatio > Math.Pow(6.0 / 29.0, 3) ? 116 * CubeRoot(yRatio) - 16 : Math.Pow(29 / 3.0, 3) * yRatio;
        var u = l == 0.0 || uPrime - uPrimeRef == 0.0 ? 0.0 : 13 * l * (uPrime - uPrimeRef);
        var v = l == 0.0 || vPrime - vPrimeRef == 0.0 ? 0.0 : 13 * l * (vPrime - vPrimeRef);
        return new Luv(l, u, v, xyz.Limitation);
    }
    
    internal static Xyz ToXyz(Luv luv, WhitePoint whitePoint)
    {
        var (l, u, v) = luv;
        
        var (xRef, yRef, zRef) = whitePoint;
        (xRef, yRef, zRef) = Scale(xRef, yRef, zRef);
        
        var uPrimeRef = GetU(xRef, yRef, zRef);
        var uPrime = l == 0.0 ? 0.0 : u / (13 * l) + uPrimeRef;
        var vPrimeRef = GetV(xRef, yRef, zRef);
        var vPrime = l == 0.0 ? 0.0 : v / (13 * l) + vPrimeRef;

        var y = (l > 8 ? yRef * Math.Pow((l + 16) / 116.0, 3) : yRef * l * Math.Pow(3 / 29.0, 3)) / 100.0;
        var x = vPrime == 0.0 ? 0.0 : y * (9 * uPrime / (4 * vPrime));
        var z = vPrime == 0.0 ? 0.0 : y * ((12 - 3 * uPrime - 20 * vPrime) / (4 * vPrime));
        return new Xyz(x, y, z, whitePoint, luv.Limitation);
    }

    private static double GetU(double x, double y, double z)
    {
        var factor = x + 15 * y + 3 * z;
        return factor == 0.0 ? 0.0 : 4 * x / factor;
    }
    
    private static double GetV(double x, double y, double z)
    {
        var factor = x + 15 * y + 3 * z;
        return factor == 0.0 ? 0.0 : 9 * y / factor;
    }
    
    private static (double x, double y, double z) Scale(double x, double y, double z) => (x * 100, y * 100, z * 100);
}