namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripLchabTests
{
    private const double Tolerance = 0.00000000001;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchabTriplets))]
    public void ViaLab(ColourTriplet triplet)
    {
        var original = new Lchab(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Lchab.FromLab(Lchab.ToLab(original));
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}