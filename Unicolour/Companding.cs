namespace Wacton.Unicolour;

public static class Companding
{
    public static double StandardRgb(double value)
    {
        return value <= 0.0031308
            ? 12.92 * value
            : 1.055 * Math.Pow(value, 1 / 2.4) - 0.055;
    }
    
    public static double InverseStandardRgb(double value)
    {
        return value <= 0.04045 
            ? value / 12.92 
            : Math.Pow((value + 0.055) / 1.055, 2.4);
    }

    public static double Gamma(double value, double gamma) => Math.Pow(value, 1 / gamma);
    
    public static double InverseGamma(double value, double gamma) => Math.Pow(value, gamma);
}