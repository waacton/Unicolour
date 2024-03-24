namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripJzczhzTests
{
    private const double Tolerance = 0.00000000001;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.JzczhzTriplets))]
    public void ViaJzazbz(ColourTriplet triplet)
    {
        var original = new Jzczhz(triplet.First, triplet.Second, triplet.Third);
        var jzazbz = Jzczhz.ToJzazbz(original);
        var roundtrip = Jzczhz.FromJzazbz(jzazbz);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}