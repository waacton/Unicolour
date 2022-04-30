namespace Wacton.Unicolour;

public record Xyz
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    public ColourTriplet Triplet => new(X, Y, Z);
    
    // I don't know of a way to accurately report whether an XYZ triplet is monochrome
    // one option is to check that ratios match (i.e. X / WhitePoint.X == Y / WhitePoint.Y etc.)
    // but this means making assumptions about floating-point comparison tolerances
    // so for now, XYZ colour is only labelled as monochrome when converted to from a known monochrome colour in a different space
    internal bool ConvertedFromMonochrome { get; }
    
    public Xyz(double x, double y, double z) : this(x, y, z, false) {}
    internal Xyz(double x, double y, double z, bool convertedFromMonochrome)
    {
        X = x;
        Y = y;
        Z = z;
        ConvertedFromMonochrome = convertedFromMonochrome;
    }

    public override string ToString() => $"{Math.Round(X, 2)} {Math.Round(Y, 2)} {Math.Round(Z, 2)}";
}