using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOklrabTests
{
    private const double Tolerance = 0.00000000001;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklrabTriplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Oklrab(triplet.First, triplet.Second, triplet.Third);
        var oklab = Oklrab.ToOklab(original);
        var roundtrip = Oklrab.FromOklab(oklab);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklrabTriplets))]
    public void ViaOklrch(ColourTriplet triplet)
    {
        var original = new Oklrab(triplet.First, triplet.Second, triplet.Third);
        var oklrch = Oklrch.FromOklrab(original);
        var roundtrip = Oklrch.ToOklrab(oklrch);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}