using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOkhwbTests
{
    private const double Tolerance = 0.00000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Okhwb, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaOkhsv(ColourTriplet triplet)
    {
        // note: cannot test round trip of all OKHWB values as OKHWB <-> OKHSV is not 1:1
        // since when OKHWB W + B > 100%, it is the same as another OKHWB where W + B = 100%
        // (e.g. W 100 B 50 == W 66.666 B 33.333)
        // and OKHSV -> OKHWB will always produce OKHWB that results in W + B <= 100%
        var original = new Okhwb(triplet.First, triplet.Second, triplet.Third);
        var scale = original.W + original.B;
        var scaled = new Okhwb(original.H, original.W / scale, original.B / scale);

        var needsScaling = scale > 1.0;
        if (needsScaling)
        {
            var okhsvFromOriginal = Okhwb.ToOkhsv(original);
            var okhsvFromScaled = Okhwb.ToOkhsv(scaled);
            TestUtils.AssertTriplet(okhsvFromOriginal.Triplet, okhsvFromScaled.Triplet, Tolerance);
        }

        var okhsv = Okhwb.ToOkhsv(original);
        var roundtrip = Okhwb.FromOkhsv(okhsv);
        var expected = needsScaling ? scaled.Triplet : original.Triplet;
        TestUtils.AssertTriplet(roundtrip.Triplet, expected, Tolerance);
    }
}