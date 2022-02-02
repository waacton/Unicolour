namespace Wacton.Unicolour.Tests.Lookups;

using System.Collections.Generic;

internal static class ColourLimits
{
    public static readonly Dictionary<string, Unicolour> Rgb = new()
    {
        {"black", Unicolour.FromRgb(0, 0, 0)},  // lab --> 0, 0, 0
        {"white", Unicolour.FromRgb(1, 1, 1)},  // lab --> 100, +0.01, -0.0
        {"red", Unicolour.FromRgb(1, 0, 0)},    // lab --> 53.23, +80.11, +67.22
        {"green", Unicolour.FromRgb(0, 1, 0)},  // lab --> 87.74, -86.18, +83.18
        {"blue", Unicolour.FromRgb(0, 0, 1)}    // lab --> 32.30, +79.20, -107.86
    };
    
    /*
     * to the best of my knowledge these represent "extremes" of the LAB colour space
     * where L remains in the middle of the range at 50
     * red      = L:50 A:+128 B:000
     * green    = L:50 A:-128 B:000
     * yellow   = L:50 A:000 B:+128
     * blue     = L:50 A:000 B:-128
     * ----------
     * could easily be wrong...
     */
    private static Unicolour GetUnicolour(double r, double g, double b) => Unicolour.FromRgb(r / 255.0, g / 255.0, b / 255.0);
    public static readonly Dictionary<string, Unicolour> Lab = new()
    {
        {"red", GetUnicolour(255, 0, 124.7312453744219)},                   // lab via RGB --> 54.80, +84.32, +5.90
        {"green", GetUnicolour(0, 154.5289567694383, 116.42724277049872)},  // lab via RGB --> 56.64, -43.99, +10.41
        {"yellow", GetUnicolour(148.5935680937267, 116.00552286250596, 0)}, // lab via RGB --> 50.50, +3.46, +56.64
        {"blue", GetUnicolour(0, 138.39087099867112, 255)}                  // lab via RGB --> 57.57, +12.37, -66.32
    };
}