namespace Wacton.Unicolour;

public static class Measurement
{
    public static double Luminance(this Unicolour colour)
    {
        var rgb = colour.Rgb;
        double F(double channel) => channel <= 0.03928 ? channel / 12.92 : Math.Pow((channel + 0.055) / 1.055, 2.4);
        return 0.2126 * F(rgb.R) + 0.7152 * F(rgb.G) + 0.0722 * F(rgb.B);
    }
}