namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripOklchTests
{
    private const double DefaultTolerance = 0.00000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklchTriplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Oklch(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Oklch.FromOklab(Oklch.ToOklab(original));
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, DefaultTolerance);
    }
}