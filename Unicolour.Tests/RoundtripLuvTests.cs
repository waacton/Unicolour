using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripLuvTests
{
    private const double Tolerance = 0.00000001;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Luv, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Luv(triplet.First, triplet.Second, triplet.Third);
        var xyz = Luv.ToXyz(original, XyzConfig.WhitePoint);
        var roundtrip = Luv.FromXyz(xyz);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaLchuv(ColourTriplet triplet)
    {
        var original = new Luv(triplet.First, triplet.Second, triplet.Third);
        var lchuv = Lchuv.FromLuv(original);
        var roundtrip = Lchuv.ToLuv(lchuv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}