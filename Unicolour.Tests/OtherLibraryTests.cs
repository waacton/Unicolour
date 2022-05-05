namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Factories;
using Wacton.Unicolour.Tests.Utils;

public class OtherLibraryTests
{
    private static readonly ITestColourFactory OpenCvFactory = new OpenCvFactory();
    private static readonly ITestColourFactory ColourfulFactory = new ColourfulFactory();
    private static readonly ITestColourFactory ColorMineFactory = new ColorMineFactory();
    private static readonly ITestColourFactory SixLaborsFactory = new SixLaborsFactory();
    
    private delegate Unicolour UnicolourFromTuple((double first, double second, double third) tuple, double alpha = 1.0);
    private delegate TestColour TestColourFromTuple(ColourTriplet triplet);
    
    private static bool IsWindows() => Environment.OSVersion.Platform == PlatformID.Win32NT;

    [Test] 
    public void OpenCvWindows()
    {
        // I've given up trying to make OpenCvSharp work in a dockerised unix environment...
        Assume.That(IsWindows());
        
        // not testing from LCHab / LCHuv because OpenCV does not support them
        AssertUtils.AssertNamedColours(namedColour => AssertFromHex(namedColour.Hex!, OpenCvFactory));
        AssertUtils.AssertRandomHexColours(hex => AssertFromHex(hex, OpenCvFactory));
        AssertUtils.AssertRandomRgb255Colours(triplet => AssertFromRgb255(triplet, OpenCvFactory));
        AssertUtils.AssertRandomRgbColours(triplet => AssertTriplet(triplet, Unicolour.FromRgb, OpenCvFactory.FromRgb));
        AssertUtils.AssertRandomHsbColours(triplet => AssertTriplet(triplet, Unicolour.FromHsb, OpenCvFactory.FromHsb));
        AssertUtils.AssertRandomHslColours(triplet => AssertTriplet(triplet, Unicolour.FromHsl, OpenCvFactory.FromHsl));
        AssertUtils.AssertRandomXyzColours(triplet => AssertTriplet(triplet, Unicolour.FromXyz, OpenCvFactory.FromXyz)); 
        AssertUtils.AssertRandomLabColours(triplet => AssertTriplet(triplet, Unicolour.FromLab, OpenCvFactory.FromLab));
        AssertUtils.AssertRandomLuvColours(triplet => AssertTriplet(triplet, Unicolour.FromLuv, OpenCvFactory.FromLuv));
    }
    
    [Test] // in order to test OpenCV in a non-windows environment, this looks up a stored precomputed value
    public void OpenCvCrossPlatform() => AssertUtils.AssertNamedColours(namedColour => AssertFromCsvData(namedColour.Hex!, namedColour.Name!));

    [Test]
    public void Colourful()
    {
        // not testing from HSB / HSL because Colourful doesn't support them
        // not testing from LUV / LCHuv because Colourful appears to give wrong values (XYZ clamping?)
        AssertUtils.AssertNamedColours(namedColour => AssertFromHex(namedColour.Hex!, ColourfulFactory));
        AssertUtils.AssertRandomHexColours(hex => AssertFromHex(hex, ColourfulFactory));
        AssertUtils.AssertRandomRgb255Colours(triplet => AssertFromRgb255(triplet, ColourfulFactory));
        AssertUtils.AssertRandomRgbColours(triplet => AssertTriplet(triplet, Unicolour.FromRgb, ColourfulFactory.FromRgb));
        AssertUtils.AssertRandomXyzColours(triplet => AssertTriplet(triplet, Unicolour.FromXyz, ColourfulFactory.FromXyz)); 
        AssertUtils.AssertRandomLabColours(triplet => AssertTriplet(triplet, Unicolour.FromLab, ColourfulFactory.FromLab));
        AssertUtils.AssertRandomLchabColours(triplet => AssertTriplet(triplet, Unicolour.FromLchab, ColourfulFactory.FromLchab));
    }
    
    [Test]
    public void ColorMine()
    {
        // not testing from RGB [0-1] because ColorMine RGB only accepts 0-255
        // not testing from XYZ / LAB / LCHab / LUV because ColorMine does a terrible job
        // not testing from LCHuv because ColorMine does not support it
        AssertUtils.AssertNamedColours(namedColour => AssertFromHex(namedColour.Hex!, ColorMineFactory));
        AssertUtils.AssertRandomHexColours(hex => AssertFromHex(hex, ColorMineFactory));
        AssertUtils.AssertRandomRgb255Colours(triplet => AssertFromRgb255(triplet, ColorMineFactory));
        AssertUtils.AssertRandomHsbColours(triplet => AssertTriplet(triplet, Unicolour.FromHsb, ColorMineFactory.FromHsb));
        AssertUtils.AssertRandomHslColours(triplet => AssertTriplet(triplet, Unicolour.FromHsl, ColorMineFactory.FromHsl));
    }
    
    [Test]
    public void SixLabors()
    {
        // not testing from LAB / LUV / LCHuv because SixLabors appears to give wrong values (XYZ clamping?)
        // not testing from LCHab because SixLabors does not support it
        AssertUtils.AssertNamedColours(namedColour => AssertFromHex(namedColour.Hex!, SixLaborsFactory));
        AssertUtils.AssertRandomHexColours(hex => AssertFromHex(hex, SixLaborsFactory));
        AssertUtils.AssertRandomRgb255Colours(triplet => AssertFromRgb255(triplet, SixLaborsFactory));
        AssertUtils.AssertRandomRgbColours(triplet => AssertTriplet(triplet, Unicolour.FromRgb, SixLaborsFactory.FromRgb));
        AssertUtils.AssertRandomHsbColours(triplet => AssertTriplet(triplet, Unicolour.FromHsb, SixLaborsFactory.FromHsb));
        AssertUtils.AssertRandomHslColours(triplet => AssertTriplet(triplet, Unicolour.FromHsl, SixLaborsFactory.FromHsl));
        AssertUtils.AssertRandomXyzColours(triplet => AssertTriplet(triplet, Unicolour.FromXyz, SixLaborsFactory.FromXyz)); 
    }
    
    private static void AssertTriplet(ColourTriplet triplet, UnicolourFromTuple getUnicolour, TestColourFromTuple getTestColour)
    {
        var unicolour = getUnicolour(triplet.Tuple);
        var testColour = getTestColour(triplet);
        AssertTestColour(unicolour, testColour);
    }

    private static void AssertFromHex(string hex, ITestColourFactory testColourFactory)
    {
        var unicolour = Unicolour.FromHex(hex);
        var (r255, g255, b255, _) = SystemColorUtils.HexToRgb255(hex);
        var testColour = testColourFactory.FromRgb255(r255, g255, b255, $"HEX [{hex}]");
        AssertHex(unicolour, hex);
        AssertTestColour(unicolour, testColour);
    }
    
    private static void AssertFromRgb255(ColourTriplet triplet, ITestColourFactory testColourFactory)
    {
        var (first, second, third) = triplet;
        var r255 = (int)first;
        var g255 = (int)second;
        var b255 = (int)third;
        var unicolour = Unicolour.FromRgb255(r255, g255, b255);
        var testColour = testColourFactory.FromRgb255(r255, g255, b255);
        AssertTestColour(unicolour, testColour);
    }

    private static void AssertFromCsvData(string hex, string name)
    {
        var unicolour = Unicolour.FromHex(hex);
        var otherLibColour = OpenCvCsvFactory.FromName(name);
        AssertHex(unicolour, hex);
        AssertTestColour(unicolour, otherLibColour);
    }

    private static void AssertHex(Unicolour unicolour, string hex)
    {
        var hasAlpha = hex.Length is 8 or 9;
        var expectedHex = hasAlpha ? hex[..^2] : hex;
        var expectedA = hasAlpha ? hex.Substring(hex.Length - 2, 2) : "FF";
        Assert.That(unicolour.Hex.Contains(expectedHex.ToUpper()));
        Assert.That(unicolour.Alpha.Hex, Is.EqualTo(expectedA.ToUpper()));
    }
    
    private static void AssertTestColour(Unicolour unicolour, TestColour testColour)
    {
        var colourName = testColour.Name;
        var tolerances = testColour.Tolerances;
        if (colourName == null) throw new ArgumentException("Malformed test colour: no name");
        if (tolerances == null) throw new ArgumentException("Malformed test colour: no tolerances");

        var unicolourRgb = testColour.IsRgbConstrained ? unicolour.Rgb.ConstrainedTriplet : unicolour.Rgb.Triplet;
        var unicolourRgbLinear = testColour.IsRgbLinearConstrained ? unicolour.Rgb.ConstrainedTripletLinear : unicolour.Rgb.TripletLinear;
        AssertColourTriplet(unicolourRgb, testColour.Rgb, tolerances.Rgb, $"{colourName} -> RGB");
        AssertColourTriplet(unicolourRgbLinear, testColour.RgbLinear, tolerances.RgbLinear, $"{colourName} -> RGB Linear");
        AssertColourTriplet(unicolour.Xyz.Triplet, testColour.Xyz, tolerances.Xyz, $"{colourName} -> XYZ");
        AssertColourTriplet(unicolour.Lab.Triplet, testColour.Lab, tolerances.Lab, $"{colourName} -> LAB");
        AssertColourTriplet(unicolour.Luv.Triplet, testColour.Luv, tolerances.Luv, $"{colourName} -> LUV");
        
        if (testColour.ExcludeFromHsxTests)
        {
            var reasons = string.Join(", ", testColour.ExcludeFromHsxTestReasons);
            Console.WriteLine($"Excluded test colour {colourName} -> HSB/HSL because: {reasons}");
        }
        else
        {
            AssertColourTriplet(unicolour.Hsb.ConstrainedTriplet, testColour.Hsb, tolerances.Hsb, $"{colourName} -> HSB", 0);
            AssertColourTriplet(unicolour.Hsl.ConstrainedTriplet, testColour.Hsl, tolerances.Hsl, $"{colourName} -> HSL", 0);
        }
        
        if (testColour.ExcludeFromLchTests)
        {
            var reasons = string.Join(", ", testColour.ExcludeFromLchTestReasons);
            Console.WriteLine($"Excluded test colour {colourName} -> LCH because: {reasons}");
        }
        else
        {
            AssertColourTriplet(unicolour.Lchab.ConstrainedTriplet, testColour.Lchab, tolerances.Lchab, $"{colourName} -> LCHab", 2);
            AssertColourTriplet(unicolour.Lchuv.ConstrainedTriplet, testColour.Lchuv, tolerances.Lchuv, $"{colourName} -> LCHuv", 2);
        }
    }
    
    private static void AssertColourTriplet(ColourTriplet actual, ColourTriplet? expected, double tolerance, string info, int? hueIndex = null)
    {
        if (expected == null) return;
        AssertUtils.AssertColourTriplet(actual, expected, tolerance, hueIndex, info);
    }
}