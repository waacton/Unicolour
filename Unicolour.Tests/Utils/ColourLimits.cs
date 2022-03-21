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
    
    /*
     * to the best of my knowledge these represent "extremes" of the LAB colour space
     * where L remains in the middle of the range at 50
     * red      = L:50 A:+128 B:000 --> R:1.139093 G:-0.440911 B:0.489153
     * green    = L:50 A:-128 B:000 --> R:-0.573680 G:0.606029 B:0.456624
     * yellow   = L:50 A:000 B:+128 --> R:0.582705 G:0.454891 B:-0.268797
     * blue     = L:50 A:000 B:-128 --> R:-0.840632 G:0.542790 B:1.355088
     * ----------
     * could easily be wrong...
     */
    public static readonly Dictionary<string, Unicolour> Lab = new()
    {
        {"red", Unicolour.FromRgb(1, 0, 0.489153)},
        {"green", Unicolour.FromRgb(0, 0.606029, 0.456624)},
        {"yellow", Unicolour.FromRgb(0.582705, 0.454891, 0)},
        {"blue", Unicolour.FromRgb(0, 0.542790, 1)}
    };
}