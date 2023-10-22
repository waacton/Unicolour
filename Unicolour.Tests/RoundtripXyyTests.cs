namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripXyyTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void XyyRoundTrip(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Xyy.FromXyz(Xyy.ToXyz(original), XyzConfig);
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}