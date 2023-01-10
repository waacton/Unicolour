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
    
    /*
     * OPENCV:
     * not testing from HWB / xyY / LCHab / LCHuv / HSLuv / HPLuv / JzAzBz / JzCzHz / Oklab / Oklch --- does not support them
     * (also I've given up trying to make OpenCvSharp work in a dockerised unix environment...)
     */
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void OpenCvWindowsNamed(TestColour namedColour) => RunIfWindows(() => AssertFromHex(namedColour.Hex!, OpenCvFactory));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void OpenCvWindowsHex(string hex) => RunIfWindows(() => AssertFromHex(hex, OpenCvFactory));

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void OpenCvWindowsRgb255(ColourTriplet triplet) => RunIfWindows(() => AssertFromRgb255(triplet, OpenCvFactory));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))] 
    public void OpenCvWindowsRgb(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromRgb, OpenCvFactory.FromRgb));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))] 
    public void OpenCvWindowsHsb(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromHsb, OpenCvFactory.FromHsb));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HslTriplets))] 
    public void OpenCvWindowsHsl(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromHsl, OpenCvFactory.FromHsl));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))] 
    public void OpenCvWindowsXyz(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromXyz, OpenCvFactory.FromXyz)); 
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LabTriplets))] 
    public void OpenCvWindowsLab(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromLab, OpenCvFactory.FromLab));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LuvTriplets))] 
    public void OpenCvWindowsLuv(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromLuv, OpenCvFactory.FromLuv));

    // in order to test OpenCV in a non-windows environment, this looks up a stored precomputed value
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void OpenCvCrossPlatform(TestColour namedColour) => AssertFromCsvData(namedColour.Hex!, namedColour.Name!);
    
    /*
     * COLOURFUL:
     * not testing from HSB / HSL / HWB / HSLuv / HPLuv / Oklab / Oklch --- does not support them
     * not testing from LUV / LCHuv --- appears to give wrong values (XYZ clamping?)
     * not testing JzAzBz / JzCzHz --- generates different values, due to multiplying XYZ by different values
     * (Jzazbz paper is ambiguous about XYZ input, more details here https://github.com/nschloe/colorio/issues/41 - Unicolour aims to match plots of colour datasets like Colorio)
     */
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ColourfulNamed(TestColour namedColour) => AssertFromHex(namedColour.Hex!, ColourfulFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void ColourfulHex(string hex) => AssertFromHex(hex, ColourfulFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ColourfulRgb255(ColourTriplet triplet) => AssertFromRgb255(triplet, ColourfulFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void ColourfulRgb(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromRgb, ColourfulFactory.FromRgb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ColourfulXyz(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromXyz, ColourfulFactory.FromXyz);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ColourfulXyy(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromXyy, ColourfulFactory.FromXyy);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LabTriplets))]
    public void ColourfulLab(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromLab, ColourfulFactory.FromLab);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchabTriplets))]
    public void ColourfulLchab(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromLchab, ColourfulFactory.FromLchab);
    
    /* COLORMINE:
     * not testing from HWB / LCHuv / HSLuv / HPLuv / JzAzBz / JzCzHz / Oklab / Oklch --- does not support them
     * not testing from RGB [0-1] --- RGB only accepts 0-255
     * not testing from XYZ / xyY / LAB / LCHab / LUV --- does a terrible job
     */
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ColorMineNamed(TestColour namedColour) => AssertFromHex(namedColour.Hex!, ColorMineFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void ColorMineHex(string hex) => AssertFromHex(hex, ColorMineFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void ColorMineRgb255(ColourTriplet triplet) => AssertFromRgb255(triplet, ColorMineFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))] 
    public void ColorMineHsb(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromHsb, ColorMineFactory.FromHsb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HslTriplets))] 
    public void ColorMineHsl(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromHsl, ColorMineFactory.FromHsl);
    
    /*
     * SIXLABORS:
     * not testing from HWB / LCHab / HSLuv / HPLuv / JzAzBz / JzCzHz / Oklab / Oklch --- does not support them
     * not testing from LAB / LUV / LCHuv --- appears to give wrong values (XYZ clamping?)
     */
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void SixLaborsNamed(TestColour namedColour) => AssertFromHex(namedColour.Hex!, SixLaborsFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void SixLaborsHex(string hex) => AssertFromHex(hex, SixLaborsFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void SixLaborsRgb255(ColourTriplet triplet) => AssertFromRgb255(triplet, SixLaborsFactory);

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void SixLaborsRgb(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromRgb, SixLaborsFactory.FromRgb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))]
    public void SixLaborsHsb(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromHsb, SixLaborsFactory.FromHsb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HslTriplets))]
    public void SixLaborsHsl(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromHsl, SixLaborsFactory.FromHsl);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void SixLaborsXyz(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromXyz, SixLaborsFactory.FromXyz);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void SixLaborsXyy(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromXyy, SixLaborsFactory.FromXyy);

    private static void RunIfWindows(Action action)
    {
        bool IsWindows() => Environment.OSVersion.Platform == PlatformID.Win32NT;
        Assume.That(IsWindows());
        action();
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
        
        if (testColour.ExcludeFromAllTests)
        {
            var reasons = string.Join(", ", testColour.ExcludeFromAllTestReasons);
            Console.WriteLine($"Excluded test colour {colourName} -> all colour spaces because: {reasons}");
            return;
        }

        var unicolourRgb = testColour.IsRgbConstrained ? unicolour.Rgb.ConstrainedTriplet : unicolour.Rgb.Triplet;
        var unicolourRgbLinear = testColour.IsRgbLinearConstrained ? unicolour.Rgb.Linear.ConstrainedTriplet : unicolour.Rgb.Linear.Triplet;
        AssertColourTriplet(unicolourRgb, testColour.Rgb, tolerances.Rgb, $"{colourName} -> RGB");
        AssertColourTriplet(unicolourRgbLinear, testColour.RgbLinear, tolerances.RgbLinear, $"{colourName} -> RGB Linear");
        AssertColourTriplet(unicolour.Xyz.Triplet, testColour.Xyz, tolerances.Xyz, $"{colourName} -> XYZ");
        AssertColourTriplet(unicolour.Lab.Triplet, testColour.Lab, tolerances.Lab, $"{colourName} -> LAB");
        AssertColourTriplet(unicolour.Luv.Triplet, testColour.Luv, tolerances.Luv, $"{colourName} -> LUV");
        
        if (testColour.ExcludeFromXyyTests)
        {
            var reasons = string.Join(", ", testColour.ExcludeFromXyyTestReasons);
            Console.WriteLine($"Excluded test colour {colourName} -> xyY because: {reasons}");
        }
        else
        {
            AssertColourTriplet(unicolour.Xyy.Triplet, testColour.Xyy, tolerances.Xyy, $"{colourName} -> xyY");
        }

        if (testColour.ExcludeFromHsxTests)
        {
            var reasons = string.Join(", ", testColour.ExcludeFromHsxTestReasons);
            Console.WriteLine($"Excluded test colour {colourName} -> HSB/HSL because: {reasons}");
        }
        else
        {
            AssertColourTriplet(unicolour.Hsb.ConstrainedTriplet, testColour.Hsb, tolerances.Hsb, $"{colourName} -> HSB");
            AssertColourTriplet(unicolour.Hsl.ConstrainedTriplet, testColour.Hsl, tolerances.Hsl, $"{colourName} -> HSL");
        }
        
        if (testColour.ExcludeFromLchTests)
        {
            var reasons = string.Join(", ", testColour.ExcludeFromLchTestReasons);
            Console.WriteLine($"Excluded test colour {colourName} -> LCH because: {reasons}");
        }
        else
        {
            AssertColourTriplet(unicolour.Lchab.ConstrainedTriplet, testColour.Lchab, tolerances.Lchab, $"{colourName} -> LCHab");
            AssertColourTriplet(unicolour.Lchuv.ConstrainedTriplet, testColour.Lchuv, tolerances.Lchuv, $"{colourName} -> LCHuv");
        }
    }
    
    private static void AssertColourTriplet(ColourTriplet actual, ColourTriplet? expected, double tolerance, string info)
    {
        if (expected == null) return;
        AssertUtils.AssertColourTriplet(actual, expected, tolerance, info);
    }
}