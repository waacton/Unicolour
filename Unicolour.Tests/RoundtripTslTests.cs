using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripTslTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly YbrConfiguration YbrConfig = YbrConfiguration.Rec601;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.TslTriplets))]
    public void ViaRgb(ColourTriplet triplet)
    {
        var original = new Tsl(triplet.First, triplet.Second, triplet.Third);
        var rgb = Tsl.ToRgb(original, YbrConfig);
        var roundtrip = Tsl.FromRgb(rgb, YbrConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}