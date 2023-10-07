namespace Wacton.Unicolour.Tests.Utils;

using System.Collections.Generic;

internal static class ColourLimits
{
    /*
     * test LAB values calculated using http://www.brucelindbloom.com/index.html?ColorCalculator.html
     * using D65 reference white + sRGB model + sRGB gamma (default configuration for Unicolour)
     */

    public static readonly Dictionary<ColourLimit, Unicolour> Rgb = new()
    {
        { ColourLimit.Black, Unicolour.FromRgb(0, 0, 0) }, // lab --> 0, 0, 0
        { ColourLimit.White, Unicolour.FromRgb(1, 1, 1) }, // lab --> 100, +0, -0
        { ColourLimit.Red, Unicolour.FromRgb(1, 0, 0) }, // lab --> 53.2408, +80.0925, +67.2032
        { ColourLimit.Green, Unicolour.FromRgb(0, 1, 0) }, // lab --> 87.7347, -86.1827, +83.1793
        { ColourLimit.Blue, Unicolour.FromRgb(0, 0, 1) }, // lab --> 32.297, +79.1875, -107.8602
        { ColourLimit.Cyan, Unicolour.FromRgb(0, 1, 1) },
        { ColourLimit.Magenta, Unicolour.FromRgb(1, 0, 1) },
        { ColourLimit.Yellow, Unicolour.FromRgb(1, 1, 0) }
    };
}

public enum ColourLimit
{
    Black,
    White,
    Red,
    Green,
    Blue,
    Cyan,
    Magenta,
    Yellow
}