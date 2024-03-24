namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripOklabTests
{
    private const double Tolerance = 0.000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklabTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var xyz = Oklab.ToXyz(original, XyzConfig);
        var roundtrip = Oklab.FromXyz(xyz, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklabTriplets))]
    public void ViaOklch(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var oklch = Oklch.FromOklab(original);
        var roundtrip = Oklch.ToOklab(oklch);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}