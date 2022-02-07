namespace Wacton.Unicolour;

public static class Comparison
{
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

    // https://en.wikipedia.org/wiki/Color_difference#CIE76
    public static double DeltaE76(this Unicolour colour1, Unicolour colour2)
    {
        var squaredDiffL = Math.Pow(colour1.Lab.L - colour2.Lab.L, 2);
        var squaredDiffA = Math.Pow(colour1.Lab.A - colour2.Lab.A, 2);
        var squaredDiffB = Math.Pow(colour1.Lab.B - colour2.Lab.B, 2);
        return Math.Sqrt(squaredDiffL + squaredDiffA + squaredDiffB);
    }
    
    // TODO: https://en.wikipedia.org/wiki/Color_difference#CIE94
    // private static double DeltaE94(this Unicolour colour1, Unicolour colour2) => throw new NotImplementedException("Use Delta E 76 instead");
        
    // TODO: https://en.wikipedia.org/wiki/Color_difference#CIEDE2000
    // private static double DeltaE00(this Unicolour colour1, Unicolour colour2) => throw new NotImplementedException("Use Delta E 76 instead");
}