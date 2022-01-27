namespace Wacton.Unicolour;

internal static class Utils
{
    public static void Guard(this double value, double min, double max, string name)
    {
        if (value < min) throw new InvalidOperationException($"{name} cannot be less than {min}");
        if (value > max) throw new InvalidOperationException($"{name} cannot be more than {max}");
    }
    
    public static double Modulo(this double value, double modulus)
    {
        var remainder = value % modulus;
        if (remainder == 0.0)
        {
            return remainder;
        }
        
        // handles negatives, e.g. -10 % 360 returns 350 instead of -10
        // don't "add a negative" if both values are negative
        var useSubtraction = remainder < 0 ^ modulus < 0;
        return useSubtraction ? modulus + remainder : remainder; 
    }
}