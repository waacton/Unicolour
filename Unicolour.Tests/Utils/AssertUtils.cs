namespace Wacton.Unicolour.Tests.Utils;

using System;
using Wacton.Unicolour.Tests.Lookups;

internal static class AssertUtils
{
    public static void AssertNamedColour(Action<TestColour> action)
    {
        foreach (var namedColour in TestColours.NamedColours)
        {
            action(namedColour);
        }
    }

    public static void AssertRandomRgb255Colour(Action<int, int, int> action)
    {
        foreach (var (r, g, b) in TestColours.RandomRGB255s)
        {
            action(r, g, b);
        }
    }
    
    public static void AssertRandomRgbColour(Action<double, double, double> action)
    {
        foreach (var (r, g, b) in TestColours.RandomRGBs)
        {
            action(r, g, b);
        }
    }
    
    public static void AssertRandomHsbColour(Action<double, double, double> action)
    {
        foreach (var (r, g, b) in TestColours.RandomHSBs)
        {
            action(r, g, b);
        }
    }
}