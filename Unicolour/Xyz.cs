namespace Wacton.Unicolour;

using System;

public class Xyz
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Xyz(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString() => $"{Math.Round(X, 2)} {Math.Round(Y, 2)} {Math.Round(Z, 2)}";
}