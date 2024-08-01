using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHpluvTests
{
    private const double Tolerance = 0.00000000001;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HpluvTriplets))]
    public void ViaLchuv(ColourTriplet triplet)
    {
        var original = new Hpluv(triplet.First, triplet.Second, triplet.Third);
        var lchuv = Hpluv.ToLchuv(original);
        var roundtrip = Hpluv.FromLchuv(lchuv);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}