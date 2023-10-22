namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.OtherLibraries;
using Wacton.Unicolour.Tests.Utils;

/*
 * not testing from HWB / LCHuv / HSLuv / HPLuv / ICtCp / JzAzBz / JzCzHz / Oklab / Oklch / CAM02 / CAM16 / HCT --- does not support them
 * not testing from RGB [0-1] --- RGB only accepts 0-255
 * not testing from XYZ / xyY / LAB / LCHab / LUV --- does a terrible job
 */
public class LibraryColorMineTests : LibraryTestBase
{
    private static readonly ITestColourFactory ColorMineFactory = new ColorMineFactory();

    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void Named(TestColour namedColour) => AssertFromHex(namedColour.Hex!, ColorMineFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void Hex(string hex) => AssertFromHex(hex, ColorMineFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void Rgb255(ColourTriplet triplet) => AssertFromRgb255(triplet, ColorMineFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))] 
    public void Hsb(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromHsb, ColorMineFactory.FromHsb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HslTriplets))] 
    public void Hsl(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromHsl, ColorMineFactory.FromHsl);
}