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
    public static void AssertRandomLchabColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomLchabColours, action);
    public static void AssertRandomLuvColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomLuvColours, action);
    public static void AssertRandomLchuvColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomLchuvColours, action);
    public static void AssertRandomOklabColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomOklabColours, action);
    public static void AssertRandomOklchColours(Action<ColourTriplet> action) => AssertItems(TestColours.RandomOklchColours, action);
    
    private static void AssertItems<T>(List<T> itemsToAssert, Action<T> assertAction)
    {
        foreach (var itemToAssert in itemsToAssert)
        {
            assertAction(itemToAssert);
        }
    }

    public static void AssertColourTriplet(ColourTriplet actual, ColourTriplet expected, double tolerance, int? hueIndex = null, string? info = null)
    {
        var details = $"Expected --- {expected}\nActual ----- {actual}";
        string FailMessage(string channel) => $"{(info == null ? string.Empty : $"{info} · ")}{channel}\n{details}";
        AssertTripletValue(actual.First, expected.First, tolerance, FailMessage("Channel 1"), hueIndex == 0);
        AssertTripletValue(actual.Second, expected.Second, tolerance, FailMessage("Channel 2"));
        AssertTripletValue(actual.Third, expected.Third, tolerance, FailMessage("Channel 3"), hueIndex == 2);
    }

    private static void AssertTripletValue(double actual, double expected, double tolerance, string failMessage, bool isHue = false)
    {
        if (!isHue) Assert.That(actual, Is.EqualTo(expected).Within(tolerance), failMessage);
        else AssertNormalisedForHue(actual, expected, tolerance, failMessage);
    }

    private static void AssertNormalisedForHue(double actualHue, double expectedHue, double tolerance, string failMessage)
    {
        double Normalise(double value) => value / 360.0;
        var actual = Normalise(actualHue);
        var expected = Normalise(expectedHue);
        var expectedPlus360 = Normalise(expectedHue + 360);
        var expectedMinus360 = Normalise(expectedHue - 360);
        Assert.That(actual, 
            Is.EqualTo(expected).Within(tolerance)
            .Or.EqualTo(expectedPlus360).Within(tolerance)
            .Or.EqualTo(expectedMinus360).Within(tolerance), 
            failMessage);
    }
}