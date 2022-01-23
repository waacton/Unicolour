namespace Wacton.Unicolour;

public static class Extensions
{
    public static Unicolour Interpolate(this Unicolour startColour, Unicolour endColour, double distance)
    {
        var startHsb = startColour.Hsb;
        var endHsb = endColour.Hsb;
        
        double CalculateValue(double startValue, double endValue)
        {
            var difference = endValue - startValue;
            return startValue + (difference * distance);
        }

        var hUpDiff = Math.Abs(startHsb.H - endHsb.H);
        var hDownDiff = Math.Abs(Math.Min(startHsb.H, endHsb.H) + 360 - Math.Max(startHsb.H, endHsb.H));

        var h = hUpDiff <= hDownDiff
            ? CalculateValue(startHsb.H, endHsb.H)
            : CalculateValue(Math.Min(startHsb.H, endHsb.H) + 360, Math.Max(startHsb.H, endHsb.H));
        var s = CalculateValue(startHsb.S, endHsb.S);
        var b = CalculateValue(startHsb.B, endHsb.B);
        return Unicolour.FromHsb(Utils.Modulo(h, 360), s, b);
    }
    
    // https://www.w3.org/WAI/WCAG21/Techniques/general/G18.html#tests
    // minimal recommended contrast ratio is 4.5, or 3 for larger font-sizes
    public static double Contrast(this Unicolour colour1, Unicolour colour2)
    {
        var luminance1 = colour1.Luminance();
        var luminance2 = colour2.Luminance();
        var l1 = Math.Max(luminance1, luminance2); // lighter of the colours
        var l2 = Math.Min(luminance1, luminance2); // darker of the colours
        return (l1 + 0.05) / (l2 + 0.05);
    }
        
    public static double Luminance(this Unicolour colour)
    {
        var rgb = colour.Rgb;
        double F(double channel) => channel <= 0.03928 ? channel / 12.92 : Math.Pow((channel + 0.055) / 1.055, 2.4);
        return 0.2126 * F(rgb.R) + 0.7152 * F(rgb.G) + 0.0722 * F(rgb.B);
    }
    
    // https://en.wikipedia.org/wiki/Color_difference#CIE76
    public static double DeltaE76(this Unicolour colour1, Unicolour colour2)
    {
        var squaredDiffL = Math.Pow(colour1.Lab.L - colour2.Lab.L, 2);
        var squaredDiffA = Math.Pow(colour1.Lab.A - colour2.Lab.A, 2);
        var squaredDiffB = Math.Pow(colour1.Lab.B - colour2.Lab.B, 2);
        return Math.Sqrt(squaredDiffL + squaredDiffA + squaredDiffB);
    }
    
    // https://en.wikipedia.org/wiki/Color_difference#CIE94
    private static double DeltaE94(this Unicolour colour1, Unicolour colour2) => throw new NotImplementedException("Use Delta E 76 instead");
        
    // https://en.wikipedia.org/wiki/Color_difference#CIEDE2000
    private static double DeltaE00(this Unicolour colour1, Unicolour colour2) => throw new NotImplementedException("Use Delta E 76 instead");
}