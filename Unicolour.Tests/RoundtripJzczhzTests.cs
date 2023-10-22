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
        var roundtrip = Jzczhz.FromJzazbz(Jzczhz.ToJzazbz(original));
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}