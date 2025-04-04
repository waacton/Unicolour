using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripLchabTests
{
    private const double Tolerance = 0.00000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchabTriplets))]
    public void ViaLab(ColourTriplet triplet)
    {
        var original = new Lchab(triplet.First, triplet.Second, triplet.Third);
        var lab = Lchab.ToLab(original);
        var roundtrip = Lchab.FromLab(lab);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}