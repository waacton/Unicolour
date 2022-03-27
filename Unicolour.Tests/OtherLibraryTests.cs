namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Factories;
using Wacton.Unicolour.Tests.Utils;

public class OtherLibraryTests
{
    private static readonly OpenCvFactory OpenCvFactory = new();
    private static readonly ColourfulFactory ColourfulFactory = new();
    private static readonly ColorMineFactory ColorMineFactory = new();
    private static readonly SixLaborsFactory SixLaborsFactory = new();
    
    private static bool IsWindows() => Environment.OSVersion.Platform == PlatformID.Win32NT;

    [Test] 
    public void OpenCvWindows()
    {
        // I've given up trying to make OpenCvSharp work in a dockerised unix environment...
        Assume.That(IsWindows());
        AssertUtils.AssertNamedColours(namedColour => AssertFromHex(namedColour.Hex!, OpenCvFactory));
        AssertUtils.AssertRandomHexColours(hex => AssertFromHex(hex, OpenCvFactory));
        AssertUtils.AssertRandomRgb255Colours(tuple => AssertFromRgb255(tuple, OpenCvFactory));
        AssertUtils.AssertRandomRgbColours(tuple => AssertFromRgb(tuple, OpenCvFactory));
        AssertUtils.AssertRandomHsbColours(tuple => AssertFromHsb(tuple, OpenCvFactory)); 
        AssertUtils.AssertRandomHslColours(tuple => AssertFromHsl(tuple, OpenCvFactory)); 
    }
    
    [Test] 
    public void OpenCvCrossPlatform()
    {
        // in order to test OpenCV in a non-windows environment, this looks up a stored precomputed value
        AssertUtils.AssertNamedColours(namedColour => AssertFromCsvData(namedColour.Hex!, namedColour.Name!));
    }

    [Test]
    public void Colourful()
    {
        // no asserting random HSB colours because Colourful doesn't support HSB/HSL
        AssertUtils.AssertNamedColours(namedColour => AssertFromHex(namedColour.Hex!, ColourfulFactory));
        AssertUtils.AssertRandomHexColours(hex => AssertFromHex(hex, ColourfulFactory));
        AssertUtils.AssertRandomRgb255Colours(tuple => AssertFromRgb255(tuple, ColourfulFactory));
        AssertUtils.AssertRandomRgbColours(tuple => AssertFromRgb(tuple, ColourfulFactory));
    }
    
    [Test]
    public void ColorMine()
    {
        // no asserting random RGB 0-1 colours because ColorMine only accepts RGB 255
        AssertUtils.AssertNamedColours(namedColour => AssertFromHex(namedColour.Hex!, ColorMineFactory));
        AssertUtils.AssertRandomHexColours(hex => AssertFromHex(hex, ColorMineFactory));
        AssertUtils.AssertRandomRgb255Colours(tuple => AssertFromRgb255(tuple, ColorMineFactory)); 
        AssertUtils.AssertRandomHsbColours(tuple => AssertFromHsb(tuple, ColorMineFactory)); 
        AssertUtils.AssertRandomHslColours(tuple => AssertFromHsl(tuple, ColorMineFactory)); 
    }
    
    [Test]
    public void SixLabors()
    {
        AssertUtils.AssertNamedColours(namedColour => AssertFromHex(namedColour.Hex!, SixLaborsFactory));
        AssertUtils.AssertRandomHexColours(hex => AssertFromHex(hex, SixLaborsFactory));
        AssertUtils.AssertRandomRgb255Colours(tuple => AssertFromRgb255(tuple, SixLaborsFactory)); 
        AssertUtils.AssertRandomRgbColours(tuple => AssertFromRgb(tuple, SixLaborsFactory));
        AssertUtils.AssertRandomHsbColours(tuple => AssertFromHsb(tuple, SixLaborsFactory)); 
        AssertUtils.AssertRandomHslColours(tuple => AssertFromHsl(tuple, SixLaborsFactory)); 
    }
    
    private static void AssertFromHex(string hex, ITestColourFactory testColourFactory)
    {
        var unicolour = Unicolour.FromHex(hex);
        var (r255, g255, b255, _) = SystemColorUtils.HexToRgb255(hex);
        var testColour = testColourFactory.FromRgb255(r255, g255, b255);
        AssertHex(unicolour, hex);
        AssertTestColour(unicolour, testColour, $"HEX [{hex}]");
    }
    
    private static void AssertFromRgb255(ColourTuple tuple, ITestColourFactory testColourFactory)
    {
        var (first, second, third) = tuple;
        var r255 = (int)(first / 255.0);
        var g255 = (int)(second / 255.0);
        var b255 = (int)(third / 255.0);
        var unicolour = Unicolour.FromRgb255(r255, g255, b255);
        var testColour = testColourFactory.FromRgb255(r255, g255, b255);
        AssertTestColour(unicolour, testColour, $"RGB [{unicolour.Rgb.Tuple255}]");
    }
    
    private static void AssertFromRgb(ColourTuple tuple, ITestColourFactory testColourFactory)
    {
        var (r, g, b) = tuple;
        var unicolour = Unicolour.FromRgb(r, g, b);
        var testColour = testColourFactory.FromRgb(r, g, b);
        AssertTestColour(unicolour, testColour, $"RGB [{unicolour.Rgb}]");
    }
    
    private static void AssertFromHsb(ColourTuple tuple, ITestColourFactory testColourFactory)
    {
        var (h, s, b) = tuple;
        var unicolour = Unicolour.FromHsb(h, s, b);
        var testColour = testColourFactory.FromHsb(h, s, b);
        AssertTestColour(unicolour, testColour, $"HSB [{unicolour.Hsb}]");
    }
    
    private static void AssertFromHsl(ColourTuple tuple, ITestColourFactory testColourFactory)
    {
        var (h, s, l) = tuple;
        var unicolour = Unicolour.FromHsl(h, s, l);
        var testColour = testColourFactory.FromHsl(h, s, l);
        AssertTestColour(unicolour, testColour, $"HSL [{unicolour.Hsl}]");
    }
    
    private static void AssertFromCsvData(string hex, string name)
    {
        var unicolour = Unicolour.FromHex(hex);
        var otherLibColour = OpenCvCsvFactory.FromName(name);
        AssertHex(unicolour, hex);
        AssertTestColour(unicolour, otherLibColour, $"NAME [{name}]");
    }

    private static void AssertHex(Unicolour unicolour, string hex)
    {
        var hasAlpha = hex.Length is 8 or 9;
        var expectedRgb = hasAlpha ? hex[..^2] : hex;
        var expectedA = hasAlpha ? hex.Substring(hex.Length - 2, 2) : "FF";
        Assert.That(unicolour.Rgb.Hex.Contains(expectedRgb.ToUpper()));
        Assert.That(unicolour.Alpha.Hex, Is.EqualTo(expectedA.ToUpper()));
    }
    
    private static void AssertTestColour(Unicolour unicolour, TestColour testColour, string source)
    {
        var colourName = testColour.Name;
        var tolerances = testColour.Tolerances;
        if (colourName == null) throw new ArgumentException("Malformed test colour: no name");
        if (tolerances == null) throw new ArgumentException("Malformed test colour: no tolerances");

        AssertColourTuple(unicolour.Rgb.Tuple, testColour.Rgb, tolerances.Rgb, $"{source} -> RGB");
        AssertColourTuple(unicolour.Rgb.TupleLinear, testColour.RgbLinear, tolerances.RgbLinear, $"{source} -> RGB Linear");
        AssertColourTuple(unicolour.Xyz.Tuple, testColour.Xyz, tolerances.Xyz, $"{source} -> XYZ");
        AssertColourTuple(unicolour.Lab.Tuple, testColour.Lab, tolerances.Lab, $"{source} -> LAB");

        if (testColour.ExcludeFromHueBasedTest)
        {
            var reasons = string.Join(", ", testColour.ExcludeFromHueBasedTestReasons);
            Console.WriteLine($"Excluded test colour {source} -> HSB [{unicolour.Hsb}] / HSL [{unicolour.Hsl}] because: {reasons}");
            return;
        }
        
        AssertColourTuple(unicolour.Hsb.Tuple, testColour.Hsb, tolerances.Hsb, $"{source} -> HSB", true);
        AssertColourTuple(unicolour.Hsl.Tuple, testColour.Hsl, tolerances.Hsl, $"{source} -> HSL", true);
    }
    
    private static void AssertColourTuple(ColourTuple unicolourTuple, ColourTuple? testTuple, double tolerance, string details, bool hasHue = false)
    {
        if (testTuple == null) return;
        AssertUtils.AssertColourTuple(unicolourTuple, testTuple, tolerance, hasHue, details);
    }
}