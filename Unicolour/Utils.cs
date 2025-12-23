using System.Globalization;

namespace Wacton.Unicolour;

internal static class Utils
{
    internal const string Unnamed = "(unnamed)";
    
    // based on smallest value required for monochromatic light to be treated as intersecting the spectral locus (see: Intersect.IsOnSegment)
    // only intended for when a little bit of rounding is necessary
    internal static bool IsEffectivelyZero(this double x) => Math.Abs(x) < 5e-14;
    internal static double Clamp(this double x, double min, double max) => x < min ? min : x > max ? max : x;
    internal static double Clamp(this int x, int min, int max) => x < min ? min : x > max ? max : x;
    internal static double CubeRoot(double x) => x < 0 ? -Power(-x, 1 / 3.0) : Power(x, 1 / 3.0);
    internal static double ToDegrees(double radians) => radians * (180.0 / Math.PI);
    internal static double ToRadians(double degrees) => degrees * (Math.PI / 180.0);
    
    internal static double Modulo(this double value, double modulus, bool inclusive = false)
    {
        if (double.IsNaN(value))
        {
            return double.NaN;
        }

        if (inclusive && value == modulus)
        {
            return value; // e.g. 360 % 360 returns 360 instead of 0 because inclusive
        }

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

    internal static bool UseOldPower = false;
    
    internal static double Power(double number, int exponent)
    {
        if (UseOldPower) return Math.Pow(number, exponent);
        // if (exponent > 15) return Math.Pow(number, exponent);
        // if (exponent < 0) return Math.Pow(number, exponent);
    
        var result = 1.0;
        for (var i = 1; i <= exponent; i++)
        {
            result *= number;
        }

        return result;
    }

    internal static double Power(double number, double exponent) => Math.Pow(number, exponent);
    
    internal static (double r, double g, double b, double a) ParseHex(string colourHex)
    {
        var hex = colourHex.TrimStart('#');
        if (hex.Length is not (6 or 8))
        {
            throw new ArgumentException($"{colourHex} contains invalid number of characters");
        }
        
        var r = Parse(hex, 0) / 255.0;
        var g = Parse(hex, 2) / 255.0;
        var b = Parse(hex, 4) / 255.0;
        var a = hex.Length == 8 ? Parse(hex, 6) / 255.0 : 1.0;
        return (r, g, b, a);
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
    
    internal static ColourTriplet ToLchTriplet(double lightness, double axis1, double axis2)
    {
        var chroma = Math.Sqrt(Power(axis1, 2) + Power(axis2, 2));
        var hue = ToDegrees(Math.Atan2(axis2, axis1));
        return new ColourTriplet(lightness, chroma, hue.Modulo(360.0));
    }
    
    internal static (double lightness, double axis1, double axis2) FromLchTriplet(ColourTriplet lchTriplet)
    {
        var (l, c, h) = lchTriplet;
        var axis1 = c * Math.Cos(ToRadians(h));
        var axis2 = c * Math.Sin(ToRadians(h));
        return (l, axis1, axis2);
    }
    
    internal static double Distance(Chromaticity start, Chromaticity end)
    {
        return Math.Sqrt(Power(end.X - start.X, 2) + Power(end.Y - start.Y, 2));
    }
    
    internal static (double radius, double angle) Polar(Chromaticity start, Chromaticity end)
    {
        var radius = Distance(start, end);
        var angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
        return (radius, ToDegrees(angle).Modulo(360));
    }
}