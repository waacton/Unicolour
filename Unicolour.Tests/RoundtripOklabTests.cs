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
        var roundtrip = Oklab.FromXyz(Oklab.ToXyz(original, XyzConfig), XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklabTriplets))]
    public void ViaOklch(ColourTriplet triplet)
    {
        var original = new Oklab(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Oklch.ToOklab(Oklch.FromOklab(original));
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}