namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripIptTests
{
    private const double DefaultTolerance = 0.00000000001;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.IptTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Ipt(triplet.First, triplet.Second, triplet.Third);
        var xyz = Ipt.ToXyz(original, XyzConfig);
        var roundtrip = Ipt.FromXyz(xyz, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, DefaultTolerance);
    }
}