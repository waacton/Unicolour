namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripHpluvTests
{
    private const double Tolerance = 0.00000000001;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HpluvTriplets))]
    public void ViaLchuv(ColourTriplet triplet)
    {
        var original = new Hpluv(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Hpluv.FromLchuv(Hpluv.ToLchuv(original));
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}