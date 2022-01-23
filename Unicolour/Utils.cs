namespace Wacton.Unicolour;

internal static class Utils
{
    public static void Check(this double value, double min, double max, string name)
    {
        if (value < min) throw new InvalidOperationException($"{name} cannot be less than {min}");
        if (value > max) throw new InvalidOperationException($"{name} cannot be more than {max}");
    }
    
    public static double Modulo(double value, double modulus)
    {
        var remainder = value % modulus;
        return remainder < 0 ? modulus + remainder : remainder; // handles negatives, e.g. -10 % 360 returns 350 instead of -10
    }
}