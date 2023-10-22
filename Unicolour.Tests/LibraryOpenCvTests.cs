namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.OtherLibraries;
using Wacton.Unicolour.Tests.Utils;

/*
 * not testing from HWB / xyY / LCHab / LCHuv / HSLuv / HPLuv / ICtCp / JzAzBz / JzCzHz / Oklab / Oklch / CAM02 / CAM16 / HCT --- does not support them
 * (also I've given up trying to make OpenCvSharp work in a dockerised unix environment...)
 */
public class LibraryOpenCvTests : LibraryTestBase
{
    private static readonly ITestColourFactory OpenCvFactory = new OpenCvFactory();
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void Named(TestColour namedColour) => RunIfWindows(() => AssertFromHex(namedColour.Hex!, OpenCvFactory));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void Hex(string hex) => RunIfWindows(() => AssertFromHex(hex, OpenCvFactory));

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void Rgb255(ColourTriplet triplet) => RunIfWindows(() => AssertFromRgb255(triplet, OpenCvFactory));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))] 
    public void Rgb(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromRgb, OpenCvFactory.FromRgb));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))] 
    public void Hsb(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromHsb, OpenCvFactory.FromHsb));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HslTriplets))] 
    public void Hsl(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromHsl, OpenCvFactory.FromHsl));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))] 
    public void Xyz(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromXyz, OpenCvFactory.FromXyz)); 
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LabTriplets))] 
    public void Lab(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromLab, OpenCvFactory.FromLab));
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LuvTriplets))] 
    public void Luv(ColourTriplet triplet) => RunIfWindows(() => AssertTriplet(triplet, Unicolour.FromLuv, OpenCvFactory.FromLuv));

    // in order to test OpenCV in a non-windows environment, this looks up a stored precomputed value
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void CrossPlatform(TestColour namedColour) => AssertFromCsvData(namedColour.Hex!, namedColour.Name!);

    private static void RunIfWindows(Action action)
    {
        bool IsWindows() => Environment.OSVersion.Platform == PlatformID.Win32NT;
        Assume.That(IsWindows());
        action();
    }
    
    private static void AssertFromCsvData(string hex, string name)
    {
        var unicolour = Unicolour.FromHex(hex);
        var otherLibColour = OpenCvCsvFactory.FromName(name);
        AssertHex(unicolour, hex);
        AssertTestColour(unicolour, otherLibColour);
    }
}