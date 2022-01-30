namespace Wacton.Unicolour.Tests.Utils;

using System;
using Wacton.Unicolour.Tests.Lookups;

internal static class AssertUtils
{
    public static void AssertNamedColours(Action<TestColour> action)
    {
        foreach (var namedColour in TestColours.NamedColours)
        {
            action(namedColour);
        }
    }

    public static void AssertRandomRgb255Colours(Action<int, int, int> action)
    {
        foreach (var (r, g, b) in TestColours.RandomRGB255s)
        {
            action(r, g, b);
        }
    }
    
    public static void AssertRandomRgbColours(Action<double, double, double> action)
    {
        foreach (var (r, g, b) in TestColours.RandomRGBs)
        {
            action(r, g, b);
        }
    }
    
    public static void AssertRandomHsbColours(Action<double, double, double> action)
    {
        foreach (var (r, g, b) in TestColours.RandomHSBs)
        {
            action(r, g, b);
        }
    }
}