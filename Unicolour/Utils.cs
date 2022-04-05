namespace Wacton.Unicolour;

using System.Globalization;

internal static class Utils
{
    public static double Clamp(this double value, double min, double max) => value < min ? min : value > max ? max : value;
    
    public static double CubeRoot(double x) => x < 0 ? -Math.Pow(-x, 1 / 3.0) : Math.Pow(x, 1 / 3.0);

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
    
    public static (int r255, int g255, int b255, int a255) ParseColourHex(string colourHex)
    {
        var hex = colourHex.TrimStart('#');
        if (hex.Length is not (6 or 8))
        {
            throw new ArgumentException($"{colourHex} contains invalid number of characters");
        }
        
        var r255 = Parse(hex, 0);
        var g255 = Parse(hex, 2);
        var b255 = Parse(hex, 4);
        var a255 = hex.Length == 8 ? Parse(hex, 6) : 255;
        return (r255, g255, b255, a255);
    }

    private static int Parse(string hex, int startIndex)
    {
        var chars = hex.Substring(startIndex, 2).ToUpper();
        if (chars.Any(x => !Uri.IsHexDigit(x)))
        {
            throw new ArgumentException($"{chars} cannot be parsed as hex");
        }

        return int.Parse(chars, NumberStyles.HexNumber);
    }
}