using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripIptTests
{
    private const double Tolerance = 0.00000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Ipt, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Ipt(triplet.First, triplet.Second, triplet.Third);
        var xyz = Ipt.ToXyz(original, XyzConfig.ChromaticAdaptor);
        var roundtrip = Ipt.FromXyz(xyz, XyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}