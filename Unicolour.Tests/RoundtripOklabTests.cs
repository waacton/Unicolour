using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOklabTests
{
    private const double Tolerance = 0.000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Oklab, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var xyz = Oklab.ToXyz(original, XyzConfig.ChromaticAdaptor, RgbConfig);
        var roundtrip = Oklab.FromXyz(xyz, XyzConfig.ChromaticAdaptor, RgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOklch(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var oklch = Oklch.FromOklab(original);
        var roundtrip = Oklch.ToOklab(oklch);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOkhsv(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var okhsv = Okhsv.FromOklab(original, XyzConfig.ChromaticAdaptor, RgbConfig);
        var roundtrip = Okhsv.ToOklab(okhsv, XyzConfig.ChromaticAdaptor, RgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOkhsl(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var okhsl = Okhsl.FromOklab(original, XyzConfig.ChromaticAdaptor, RgbConfig);
        var roundtrip = Okhsl.ToOklab(okhsl, XyzConfig.ChromaticAdaptor, RgbConfig);
        
        // OKLAB <-> OKHSL is not 1:1
        // as OKHSL does not handle OKLAB values that are out of the RGB gamut
        // and can result in OKHSL values that actually maps to a different OKLAB 
        if (TestUtils.MaxDiff(roundtrip.Triplet, original.Triplet) > Tolerance)
        {
            var xyz = Oklab.ToXyz(original, XyzConfig.ChromaticAdaptor, RgbConfig);
            var rgbLinear = RgbLinear.FromXyz(xyz, RgbConfig, XyzConfig.ChromaticAdaptor);
            var rgb = Rgb.FromRgbLinear(rgbLinear, RgbConfig, DynamicRange.High);
            Assert.That(roundtrip.L, Is.EqualTo(original.L).Within(Tolerance));
            Assert.That(rgb.IsInGamut, Is.False);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOklrab(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var oklrab = Oklrab.FromOklab(original);
        var roundtrip = Oklrab.ToOklab(oklrab);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}