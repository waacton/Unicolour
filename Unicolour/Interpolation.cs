namespace Wacton.Unicolour;

public static class Interpolation
{
    // TODO: include alpha as optional
    public static Unicolour InterpolateHsb(this Unicolour startColour, Unicolour endColour, double distance)
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
}