namespace Wacton.Unicolour;

using System;

public class Xyz
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Xyz(double x, double y, double z)
    {
        x.Check(0.0, 1.0, "X");
        y.Check(0.0, 1.0, "X");
        z.Check(0.0, 1.0, "X");

        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString() => $"{Math.Round(X, 1)} {Math.Round(Y, 1)} {Math.Round(Z, 1)}";
}