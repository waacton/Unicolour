using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOklrchTests
{
    private const double DefaultTolerance = 0.00000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklrchTriplets))]
    public void ViaOklrab(ColourTriplet triplet)
    {
        var original = new Oklrch(triplet.First, triplet.Second, triplet.Third);
        var oklrab = Oklrch.ToOklrab(original);
        var roundtrip = Oklrch.FromOklrab(oklrab);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, DefaultTolerance);
    }
}