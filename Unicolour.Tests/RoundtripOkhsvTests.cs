using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOkhsvTests
{
    private const double Tolerance = 0.0000075;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Okhsv, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Okhsv(triplet.First, triplet.Second, triplet.Third);
        var oklab = Okhsv.ToOklab(original, XyzConfig.ChromaticAdaptor, RgbConfig);
        var roundtrip = Okhsv.FromOklab(oklab, XyzConfig.ChromaticAdaptor, RgbConfig);
        
        // OKHSV <-> OKLAB is not robust when values are negative or too large
        // many instances of these out-of-gamut values do roundtrip
        // but this confirms that when they do not, it is related to these unusual values
        if (TestUtils.MaxDiff(roundtrip.Triplet, original.Triplet) > Tolerance)
        {
            Assert.That(original.S < 0 || original.V < 0 || original.S > 1.7 || original.V > 1.7);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOkhwb(ColourTriplet triplet)
    {
        var original = new Okhsv(triplet.First, triplet.Second, triplet.Third);
        var okhwb = Okhwb.FromOkhsv(original);
        var roundtrip = Okhwb.ToOkhsv(okhwb);
        
        var greyness = okhwb.W + okhwb.B;
        if (greyness > 1.0)
        {
            var greyLevel = okhwb.W / greyness;
            TestUtils.AssertTriplet(roundtrip.Triplet, new(original.H, 0, greyLevel, HueIndex: 0), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}