namespace Wacton.Unicolour.Tests.OtherLibraries;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class LibraryTestBase
{
    private const bool PrintExclusions = false;

    internal delegate Unicolour UnicolourFromTuple((double first, double second, double third) tuple, double alpha = 1.0);
    internal delegate TestColour TestColourFromTuple(ColourTriplet triplet);
    
    internal static void AssertTriplet(ColourTriplet triplet, UnicolourFromTuple getUnicolour, TestColourFromTuple getTestColour)
    {
        var unicolour = getUnicolour(triplet.Tuple);
        var testColour = getTestColour(triplet);
        AssertTestColour(unicolour, testColour);
    }

    internal static void AssertFromHex(string hex, ITestColourFactory testColourFactory)
    {
        var unicolour = Unicolour.FromHex(hex);
        var (r255, g255, b255, _) = SystemColorUtils.HexToRgb255(hex);
        var testColour = testColourFactory.FromRgb255(r255, g255, b255, $"HEX [{hex}]");
        AssertHex(unicolour, hex);
        AssertTestColour(unicolour, testColour);
    }
    
    internal static void AssertFromRgb255(ColourTriplet triplet, ITestColourFactory testColourFactory)
    {
        var (first, second, third) = triplet;
        var r255 = (int)first;
        var g255 = (int)second;
        var b255 = (int)third;
        var unicolour = Unicolour.FromRgb255(r255, g255, b255);
        var testColour = testColourFactory.FromRgb255(r255, g255, b255);
        AssertTestColour(unicolour, testColour);
    }

    internal static void AssertHex(Unicolour unicolour, string hex)
    {
        var hasAlpha = hex.Length is 8 or 9;
        var expectedHex = hasAlpha ? hex[..^2] : hex;
        var expectedA = hasAlpha ? hex.Substring(hex.Length - 2, 2) : "FF";
        Assert.That(unicolour.Hex.Contains(expectedHex.ToUpper()));
        Assert.That(unicolour.Alpha.Hex, Is.EqualTo(expectedA.ToUpper()));
    }
    
    internal static void AssertTestColour(Unicolour unicolour, TestColour testColour)
    {
        var colourName = testColour.Name;
        var tolerances = testColour.Tolerances;
        if (colourName == null) throw new ArgumentException("Malformed test colour: no name");
        if (tolerances == null) throw new ArgumentException("Malformed test colour: no tolerances");
        
        if (testColour.ExcludeFromAllTests)
        {
            PrintExclusion(colourName, "all colour spaces", string.Join(", ", testColour.ExcludeFromAllTestReasons));
            return;
        }

        var unicolourRgb = testColour.IsRgbConstrained ? unicolour.Rgb.ConstrainedTriplet : unicolour.Rgb.Triplet;
        var unicolourRgbLinear = testColour.IsRgbLinearConstrained ? unicolour.Rgb.Linear.ConstrainedTriplet : unicolour.Rgb.Linear.Triplet;
        AssertTriplet(unicolourRgb, testColour.Rgb, tolerances.Rgb, $"{colourName} -> RGB");
        AssertTriplet(unicolourRgbLinear, testColour.RgbLinear, tolerances.RgbLinear, $"{colourName} -> RGB Linear");
        AssertTriplet(unicolour.Xyz.Triplet, testColour.Xyz, tolerances.Xyz, $"{colourName} -> XYZ");
        AssertTriplet(unicolour.Lab.Triplet, testColour.Lab, tolerances.Lab, $"{colourName} -> LAB");
        AssertTriplet(unicolour.Luv.Triplet, testColour.Luv, tolerances.Luv, $"{colourName} -> LUV");
        
        if (testColour.ExcludeFromXyyTests)
        {
            PrintExclusion(colourName, "xyY", string.Join(", ", testColour.ExcludeFromXyyTestReasons));
        }
        else
        {
            AssertTriplet(unicolour.Xyy.Triplet, testColour.Xyy, tolerances.Xyy, $"{colourName} -> xyY");
        }

        if (testColour.ExcludeFromHsxTests)
        {
            PrintExclusion(colourName, "HSB/HSL", string.Join(", ", testColour.ExcludeFromHsxTestReasons));
        }
        else
        {
            AssertTriplet(unicolour.Hsb.ConstrainedTriplet, testColour.Hsb, tolerances.Hsb, $"{colourName} -> HSB");
            AssertTriplet(unicolour.Hsl.ConstrainedTriplet, testColour.Hsl, tolerances.Hsl, $"{colourName} -> HSL");
        }
        
        if (testColour.ExcludeFromLchTests)
        {
            PrintExclusion(colourName, "LCH", string.Join(", ", testColour.ExcludeFromLchTestReasons));
        }
        else
        {
            AssertTriplet(unicolour.Lchab.ConstrainedTriplet, testColour.Lchab, tolerances.Lchab, $"{colourName} -> LCHab");
            AssertTriplet(unicolour.Lchuv.ConstrainedTriplet, testColour.Lchuv, tolerances.Lchuv, $"{colourName} -> LCHuv");
        }
    }

    private static void AssertTriplet(ColourTriplet actual, ColourTriplet? expected, double tolerance, string info)
    {
        if (expected == null) return;
        AssertUtils.AssertTriplet(actual, expected, tolerance, info);
    }

    private static void PrintExclusion(string colourName, string excludedTestName, string reasons)
    {
        if (!PrintExclusions) return;
        Console.WriteLine($"Excluded test colour {colourName} -> {excludedTestName}, because: {reasons}");
    }
}