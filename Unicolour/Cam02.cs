namespace Wacton.Unicolour;

public record Cam02
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    public ColourTriplet Triplet => new(X, Y, Z);

    internal bool ConvertedFromMonochrome { get; }
    
    public Cam02(double x, double y, double z) : this(x, y, z, false) {}
    internal Cam02(double x, double y, double z, bool convertedFromMonochrome)
    {
        X = x;
        Y = y;
        Z = z;
        ConvertedFromMonochrome = convertedFromMonochrome;
    }

    public override string ToString() => $"{Math.Round(X, 2)} {Math.Round(Y, 2)} {Math.Round(Z, 2)}";
}