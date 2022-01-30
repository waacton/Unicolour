namespace Wacton.Unicolour.Tests;

using System;
using System.Drawing;
using NUnit.Framework;
using Wacton.Unicolour;
using Wacton.Unicolour.Tests.Lookups;
using Wacton.Unicolour.Tests.Utils;


public class OtherLibraryTests
{
    private static bool IsWindows() => Environment.OSVersion.Platform == PlatformID.Win32NT;

    /*
     * OPENCV:    doesn't expose linear RGB, doesn't directly convert HSV -> XYZ/LAB
     * COLOURFUL: doesn't support HSB!
     * COLORMINE: doesn't expose linear RGB
     * --------------------
     * at this point I'm pretty sure OpenCV doesn't calculate RGB -> LAB correctly
     * since all other libraries and online tools calculate the same LAB as Unicolour
     * the LAB test tolerances are so large is not really worth testing against
     */
    private static readonly Tolerances OpenCvTolerances = new() { Rgb = 0.005, Hsb = 0.005, Xyz = 0.0005, Lab = 50.0 };
    private static readonly Tolerances ColourfulTolerances = new() { Rgb = 0.00000000001, RgbLinear = 0.00000000001, Xyz = 0.0005, Lab = 0.05 };
    private static readonly Tolerances ColorMineTolerances = new() { Rgb = 0.00000000001, Hsb = 0.0005, Xyz = 0.0005, Lab = 0.0005 };

    private delegate TestColour ToOtherLibFromNamed(int r, int g, int b, string name);
    private delegate TestColour ToOtherLibFromRgb255(int r, int g, int b);
    private delegate TestColour ToOtherLibFromRgb(double r, double g, double b);
    private delegate TestColour ToOtherLibFromHsb(double h, double s, double b);
    private delegate TestColour ToOtherLibFromStored(string name); // specifically for OpenCv on non-Windows
    
    
    [Test] 
    public void OpenCvWindows()
    {
        // I've given up trying to make OpenCvSharp work in a dockerised unix environment...
        Assume.That(IsWindows());
        AssertUtils.AssertNamedColours(namedColour => AssertFromNamed(namedColour, OpenCvUtils.FromRgb255, OpenCvTolerances));
        AssertUtils.AssertRandomRgb255Colours((r, g, b) => AssertFromRgb255(r, g, b, OpenCvUtils.FromRgb255, OpenCvTolerances)); 
        AssertUtils.AssertRandomRgbColours((r, g, b) => AssertFromRgb(r, g, b, OpenCvUtils.FromRgb, OpenCvTolerances));
        AssertUtils.AssertRandomHsbColours((h, s, b) => AssertFromHsb(h, s, b, OpenCvUtils.FromHsb, OpenCvTolerances));
    }
    
    [Test] 
    public void OpenCvCrossPlatform()
    {
        // in order to test OpenCV in a non-windows environment, this looks up a stored precomputed value
        TestColour GetStoredOpenCvColour(string name) => TestColours.GetOpenCvColour(name);
        AssertUtils.AssertNamedColours(namedColour => AssertFromStored(namedColour, GetStoredOpenCvColour, OpenCvTolerances));
    }

    [Test]
    public void Colourful()
    {
        AssertUtils.AssertNamedColours(namedColour => AssertFromNamed(namedColour, ColourfulUtils.FromRgb255, ColourfulTolerances));
        AssertUtils.AssertRandomRgb255Colours((r, g, b) => AssertFromRgb255(r, g, b, ColourfulUtils.FromRgb255, ColourfulTolerances)); 
        AssertUtils.AssertRandomRgbColours((r, g, b) => AssertFromRgb(r, g, b, ColourfulUtils.FromRgb, ColourfulTolerances));
    }
    
    [Test]
    public void ColorMine()
    {
        AssertUtils.AssertNamedColours(namedColour => AssertFromNamed(namedColour, ColorMineUtils.FromRgb255, ColorMineTolerances));
        AssertUtils.AssertRandomRgb255Colours((r, g, b) => AssertFromRgb255(r, g, b, ColorMineUtils.FromRgb255, ColorMineTolerances)); 
        AssertUtils.AssertRandomHsbColours((h, s, b) => AssertFromHsb(h, s, b, ColorMineUtils.FromHsb, ColorMineTolerances)); 
    }
    
    private static void AssertFromNamed(TestColour namedColour, ToOtherLibFromNamed toOtherLibColour, Tolerances tolerances)
    {
        var (r, g, b) = HexToRgb255(namedColour.Hex!);
        var unicolour = Unicolour.FromRgb(r, g, b);
        var otherLibColour = toOtherLibColour(r, g, b, namedColour.Name!);
        AssertOtherColour(unicolour, otherLibColour, tolerances);
    }
    
    private static void AssertFromStored(TestColour namedColour, ToOtherLibFromStored toOtherLibColour, Tolerances tolerances)
    {
        var (r, g, b) = HexToRgb255(namedColour.Hex!);
        var unicolour = Unicolour.FromRgb(r, g, b);
        var otherLibColour = toOtherLibColour(namedColour.Name!);
        AssertOtherColour(unicolour, otherLibColour, tolerances);
    }
    
    private static void AssertFromRgb255(int r, int g, int b, ToOtherLibFromRgb255 toOtherLibColour, Tolerances tolerances)
    {
        var unicolour = Unicolour.FromRgb(r, g, b);
        var otherLibColour = toOtherLibColour(r, g, b);
        AssertOtherColour(unicolour, otherLibColour, tolerances);
    }
    
    private static void AssertFromRgb(double r, double g, double b, ToOtherLibFromRgb toOtherLibColour, Tolerances tolerances)
    {
        var unicolour = Unicolour.FromRgb(r, g, b);
        var otherLibColour = toOtherLibColour(r, g, b);
        AssertOtherColour(unicolour, otherLibColour, tolerances);
    }
    
    private static void AssertFromHsb(double h, double s, double b, ToOtherLibFromHsb toOtherLibColour, Tolerances tolerances)
    {
        var unicolour = Unicolour.FromHsb(h, s, b);
        var otherLibColour = toOtherLibColour(h, s, b);
        AssertOtherColour(unicolour, otherLibColour, tolerances);
    }

    private static void AssertOtherColour(Unicolour unicolour, TestColour otherColour, Tolerances tolerances)
    {
        string FailMessage() => $"colour:{otherColour.Name}";
        
        void AssertColourSpace((double, double, double) unicolourSpace, (double, double, double)? otherSpace, double tolerance)
        {
            if (!otherSpace.HasValue) return;
            Assert.That(unicolourSpace.Item1, Is.EqualTo(otherSpace.Value.Item1).Within(tolerance), FailMessage);
            Assert.That(unicolourSpace.Item2, Is.EqualTo(otherSpace.Value.Item2).Within(tolerance), FailMessage);
            Assert.That(unicolourSpace.Item3, Is.EqualTo(otherSpace.Value.Item3).Within(tolerance), FailMessage);
        }

        AssertColourSpace(unicolour.Rgb.Tuple, otherColour.Rgb, tolerances.Rgb);
        AssertColourSpace(unicolour.Rgb.TupleLinear, otherColour.RgbLinear, tolerances.RgbLinear);
        AssertColourSpace(unicolour.Hsb.Tuple, otherColour.Hsb, tolerances.Hsb);
        AssertColourSpace(unicolour.Xyz.Tuple, otherColour.Xyz, tolerances.Xyz);
        AssertColourSpace(unicolour.Lab.Tuple, otherColour.Lab, tolerances.Lab);
    }
    
    private static (byte r, byte g, byte b) HexToRgb255(string hex)
    {
        var systemColour = ColorTranslator.FromHtml(hex);
        return (systemColour.R, systemColour.G, systemColour.B);
    }

    private class Tolerances {
        public double Rgb, RgbLinear, Hsb, Xyz, Lab;
    }
}