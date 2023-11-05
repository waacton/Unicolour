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
        var roundtrip = Lab.FromXyz(Lab.ToXyz(original, XyzConfig), XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LabTriplets))]
    public void ViaLchab(ColourTriplet triplet)
    {
        var original = new Lab(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Lchab.ToLab(Lchab.FromLab(original));
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}