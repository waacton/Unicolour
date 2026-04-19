using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOkhslTests
{
    private const double Tolerance = 0.000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Okhsl, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Okhsl(triplet.First, triplet.Second, triplet.Third);
        var oklab = Okhsl.ToOklab(original, XyzConfig.ChromaticAdaptor, RgbConfig);
        var roundtrip = Okhsl.FromOklab(oklab, XyzConfig.ChromaticAdaptor, RgbConfig);
        
        // OKHSV <-> OKLAB is not robust when values are outwith the usual 0 - 1 range
        // many instances of these out-of-gamut values do roundtrip
        // but this confirms that when they do not, it is related to these unusual values
        if (TestUtils.MaxDiff(roundtrip.Triplet, original.Triplet) > Tolerance)
        {
            Assert.That(original.S < 0 ||  original.L < 0 || original.S > 1 || original.L > 1);
            
            // still expect positive L values to roundtrip
            if (original.L > 0)
            {
                Assert.That(roundtrip.L, Is.EqualTo(original.L).Within(Tolerance));
            }
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}