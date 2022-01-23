namespace Wacton.Unicolour;

using System;

public class Unicolour : IEquatable<Unicolour>
{
    public Hsb Hsb { get; }
    public Rgb Rgb => Converter.HsbToRgb(Hsb);
    public Xyz Xyz => Converter.RgbToXyz(Rgb);
    public Lab Lab => Converter.XyzToLab(Xyz);
    public double A { get; }

    private Unicolour(double h, double s, double b, double a)
    {
        Hsb = new Hsb(h, s, b);
        A = a;
    }

    public static Unicolour FromHsb(double h, double s, double b, double a = 1.0) => new(h, s, b, a);

    public static Unicolour FromRgb(int r, int g, int b, int a = 255) => FromRgb(r / 255.0, g / 255.0, b / 255.0, a / 255.0);
    
    public static Unicolour FromRgb(double r, double g, double b, double a = 255)
    {
        var rgb = new Rgb(r, g, b);
        var hsb = Converter.RgbToHsb(rgb);
        return new Unicolour(hsb.H, hsb.S, hsb.B, a);
    }

    public override string ToString() => $"HSB:[{Hsb}] RGB:[{Rgb}] A:{Math.Round(A * 100, 1)}";

    public bool Equals(Unicolour? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(Hsb, other.Hsb) && A.Equals(other.A);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Unicolour) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Hsb, A);
    }
}