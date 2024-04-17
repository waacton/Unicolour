namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripOklabTests
{
    private const double Tolerance = 0.000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklabTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var xyz = Oklab.ToXyz(original, XyzConfig, RgbConfig);
        var roundtrip = Oklab.FromXyz(xyz, XyzConfig, RgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklabTriplets))]
    public void ViaOklch(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var oklch = Oklch.FromOklab(original);
        var roundtrip = Oklch.ToOklab(oklch);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklabTriplets))]
    public void ViaOkhsv(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var okhsv = Okhsv.FromOklab(original, XyzConfig, RgbConfig);
        var roundtrip = Okhsv.ToOklab(okhsv, XyzConfig, RgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklabTriplets))]
    public void ViaOkhsl(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var okhsl = Okhsl.FromOklab(original, XyzConfig, RgbConfig);
        var roundtrip = Okhsl.ToOklab(okhsl, XyzConfig, RgbConfig);
        
        // note: cannot test round trip of all OKLAB values as OKLAB <-> OKHSL is not 1:1
        // as OKHSL does not handle OKLAB values that are out of the RGB gamut
        // and can result in OKHSL values that actually maps to a different OKLAB 
        var hasDifference = Math.Abs(original.A - roundtrip.A) > Tolerance || Math.Abs(original.B - roundtrip.B) > Tolerance;
        if (hasDifference)
        {
            var xyz = Oklab.ToXyz(original, XyzConfig, RgbConfig);
            var rgbLinear = RgbLinear.FromXyz(xyz, RgbConfig, XyzConfig);
            var rgb = Rgb.FromRgbLinear(rgbLinear, RgbConfig);
            Assert.That(roundtrip.L, Is.EqualTo(original.L).Within(Tolerance));
            Assert.That(rgb.IsInGamut, Is.False);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}