using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOklchTests
{
    private const double DefaultTolerance = 0.00000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklchTriplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Oklch(triplet.First, triplet.Second, triplet.Third);
        var oklab = Oklch.ToOklab(original);
        var roundtrip = Oklch.FromOklab(oklab);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, DefaultTolerance);
    }
}