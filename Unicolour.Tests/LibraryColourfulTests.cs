namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.OtherLibraries;
using Wacton.Unicolour.Tests.Utils;

/*
 * not testing from HSB / HSL / HWB / HSLuv / HPLuv / ICtCp / Oklab / Oklch / CAM02 / CAM16 / HCT --- does not support them
 * not testing from LUV / LCHuv --- appears to give wrong values (XYZ clamping?)
 * not testing JzAzBz / JzCzHz --- generates different values, due to multiplying XYZ by different values
 * (Jzazbz paper is ambiguous about XYZ input, more details here https://github.com/nschloe/colorio/issues/41 - Unicolour aims to match plots of colour datasets like Colorio)
 */
public class LibraryColourfulTests : LibraryTestBase
{
    private static readonly ITestColourFactory ColourfulFactory = new ColourfulFactory();
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void Named(TestColour namedColour) => AssertFromHex(namedColour.Hex!, ColourfulFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void Hex(string hex) => AssertFromHex(hex, ColourfulFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void Rgb255(ColourTriplet triplet) => AssertFromRgb255(triplet, ColourfulFactory);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void Rgb(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromRgb, ColourfulFactory.FromRgb);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void Xyz(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromXyz, ColourfulFactory.FromXyz);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void Xyy(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromXyy, ColourfulFactory.FromXyy);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LabTriplets))]
    public void Lab(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromLab, ColourfulFactory.FromLab);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchabTriplets))]
    public void Lchab(ColourTriplet triplet) => AssertTriplet(triplet, Unicolour.FromLchab, ColourfulFactory.FromLchab);
}