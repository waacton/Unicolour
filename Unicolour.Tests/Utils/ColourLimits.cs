namespace Wacton.Unicolour.Tests.Utils;

using System.Collections.Generic;

internal static class ColourLimits
{
    /*
     * test LAB values calculated using http://www.brucelindbloom.com/index.html?ColorCalculator.html
     * using D65 reference white + sRGB model + sRGB gamma (default configuration for Unicolour)
     */
    
    public static readonly Dictionary<string, Unicolour> Rgb = new()
    {
        {"black", Unicolour.FromRgb(0, 0, 0)},  // lab --> 0, 0, 0
        {"white", Unicolour.FromRgb(1, 1, 1)},  // lab --> 100, +0, -0
        {"red", Unicolour.FromRgb(1, 0, 0)},    // lab --> 53.2408, +80.0925, +67.2032
        {"green", Unicolour.FromRgb(0, 1, 0)},  // lab --> 87.7347, -86.1827, +83.1793
        {"blue", Unicolour.FromRgb(0, 0, 1)}    // lab --> 32.297, +79.1875, -107.8602
    };
    
    public static readonly Dictionary<string, Unicolour> Lab = new()
    {
        {"red", Unicolour.FromLab(50, 128, 0)},
        {"green", Unicolour.FromLab(50, -128, 0)},
        {"yellow", Unicolour.FromLab(50, 0, 128)},
        {"blue", Unicolour.FromLab(50, 0, -128)}
    };
}