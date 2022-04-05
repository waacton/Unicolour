namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;
using NUnit.Framework;

internal static class AssertUtils
{
    public static void AssertNamedColours(Action<TestColour> action) => AssertItems(TestColours.NamedColours, action);
    public static void AssertRandomHexColours(Action<string> action) => AssertItems(TestColours.RandomHexColours, action);
    public static void AssertRandomRgb255Colours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomRgb255Colours, action);
    public static void AssertRandomRgbColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomRgbColours, action);
    public static void AssertRandomHsbColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomHsbColours, action);
    public static void AssertRandomHslColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomHslColours, action);
    public static void AssertRandomXyzColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomXyzColours, action);
    public static void AssertRandomLabColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomLabColours, action);
    public static void AssertRandomOklabColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomOklabColours, action);
    
    private static void AssertItems<T>(List<T> itemsToAssert, Action<T> assertAction)
    {
        foreach (var itemToAssert in itemsToAssert)
        {
            assertAction(itemToAssert);
        }
    }

    public static void AssertColourTriplet(ColourTriplet actual, ColourTriplet expected, double tolerance, bool hasHue = false, string? details = null)
    {
        double NormalisedFirst(double value) => hasHue ? value / 360.0 : value;
        
        string FailMessage(string channel) => $"{channel}{(details == null ? string.Empty : $" ({details})")}";
        Assert.That(NormalisedFirst(actual.First), Is.EqualTo(NormalisedFirst(expected.First)).Within(tolerance), FailMessage("Channel 1"));
        Assert.That(actual.Second, Is.EqualTo(expected.Second).Within(tolerance), FailMessage("Channel 2"));
        Assert.That(actual.Third, Is.EqualTo(expected.Third).Within(tolerance), FailMessage("Channel 3"));
    }
}