namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.OtherLibraries;
using Wacton.Unicolour.Tests.Utils;

/*
 * not testing from HWB / LCHab / HSLuv / HPLuv / ICtCp / JzAzBz / JzCzHz / Oklab / Oklch / CAM02 / CAM16 / HCT --- does not support them
 * not testing from LAB / LUV / LCHuv --- appears to give wrong values (XYZ clamping?)
 */
public class LibrarySixLaborsTests : LibraryTestBase
{
    private static readonly ITestColourFactory SixLaborsFactory = new SixLaborsFactory();

    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void Named(TestColour namedColour) => AssertFromHex(namedColour.Hex!, SixLaborsFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void Hex(string hex) => AssertFromHex(hex, SixLaborsFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void Rgb255(ColourTriplet triplet) => AssertFromRgb255(triplet, SixLaborsFactory);

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void Rgb(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromRgb, SixLaborsFactory.FromRgb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))]
    public void Hsb(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromHsb, SixLaborsFactory.FromHsb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HslTriplets))]
    public void Hsl(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromHsl, SixLaborsFactory.FromHsl);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void Xyz(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromXyz, SixLaborsFactory.FromXyz);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void Xyy(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromXyy, SixLaborsFactory.FromXyy);
}