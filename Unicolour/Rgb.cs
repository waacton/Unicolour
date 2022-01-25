namespace Wacton.Unicolour;

using System;

public class Rgb
{
    public double R { get; }
    public double G { get; }
    public double B { get; }

    public int R255 => (int) Math.Round(R * 255);
    public int G255 => (int) Math.Round(G * 255);
    public int B255 => (int) Math.Round(B * 255);
    public string Hex => $"#{R255:X}{G255:X}{B255:X}";

    public double RLinear => LinearCorrection(R);
    public double GLinear => LinearCorrection(G);
    public double BLinear => LinearCorrection(B);

    public Rgb(double r, double g, double b)
    {
        r.Check(0.0, 1.0, "Red");
        g.Check(0.0, 1.0, "Green");
        b.Check(0.0, 1.0, "Blue");

        R = r;
        G = g;
        B = b;
    }

    // https://en.wikipedia.org/wiki/SRGB#From_sRGB_to_CIE_XYZ
    private static double LinearCorrection(double value)
    {
        return value <= 0.04045 
            ? value / 12.92 
            : Math.Pow((value + 0.055) / 1.055, 2.4);
    }

    public override string ToString() => $"{R255} {G255} {B255} {Hex}";
}