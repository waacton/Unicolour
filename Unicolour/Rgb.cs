namespace Wacton.Unicolour;

using System;

public class Rgb
{
    public double R { get; }
    public double G { get; }
    public double B { get; }

    public Rgb(double r, double g, double b)
    {
        r.Check(0.0, 1.0, "Red");
        g.Check(0.0, 1.0, "Green");
        b.Check(0.0, 1.0, "Blue");

        R = r;
        G = g;
        B = b;
    }

    public override string ToString() => $"{Math.Round(R * 255)} {Math.Round(G * 255)} {Math.Round(B * 255)}";
}