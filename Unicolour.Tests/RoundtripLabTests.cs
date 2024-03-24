namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripLabTests
{
    private const double Tolerance = 0.00000000001;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LabTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Lab(triplet.First, triplet.Second, triplet.Third);
        var xyz = Lab.ToXyz(original, XyzConfig);
        var roundtrip = Lab.FromXyz(xyz, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LabTriplets))]
    public void ViaLchab(ColourTriplet triplet)
    {
        var original = new Lab(triplet.First, triplet.Second, triplet.Third);
        var lchab = Lchab.FromLab(original);
        var roundtrip = Lchab.ToLab(lchab);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}