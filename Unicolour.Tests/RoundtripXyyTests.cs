using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripXyyTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var xyz = Xyy.ToXyz(original);
        var roundtrip = Xyy.FromXyz(xyz, XyzConfig.WhiteChromaticity);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaWxy(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var wxy = Wxy.FromXyy(original, XyzConfig);
        var roundtrip = Wxy.ToXyy(wxy, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaMunsell(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var munsell = Munsell.FromXyy(original);
        var roundtrip = Munsell.ToXyy(munsell);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, 0.01); // TODO:
    }
}