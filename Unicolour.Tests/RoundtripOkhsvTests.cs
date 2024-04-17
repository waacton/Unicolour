namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripOkhsvTests
{
    private const double Tolerance = 0.000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OkhsvTriplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Okhsv(triplet.First, triplet.Second, triplet.Third);
        var oklab = Okhsv.ToOklab(original, XyzConfig, RgbConfig);
        var roundtrip = Okhsv.FromOklab(oklab, XyzConfig, RgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OkhsvTriplets))]
    public void ViaOkhwb(ColourTriplet triplet)
    {
        var original = new Okhsv(triplet.First, triplet.Second, triplet.Third);
        var okhwb = Okhwb.FromOkhsv(original);
        var roundtrip = Okhwb.ToOkhsv(okhwb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}