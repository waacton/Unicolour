namespace Wacton.Unicolour;

public static class Companding
{
    public static double InverseStandardRgb(double value)
    {
        return value <= 0.04045 
            ? value / 12.92 
            : Math.Pow((value + 0.055) / 1.055, 2.4);
    }
    
    public static double InverseGamma(double value, double gamma) => Math.Pow(value, gamma);
}