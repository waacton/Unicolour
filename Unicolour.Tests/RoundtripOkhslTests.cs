namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripOkhslTests
{
    private const double Tolerance = 0.000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OkhslTriplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Okhsl(triplet.First, triplet.Second, triplet.Third);
        var oklab = Okhsl.ToOklab(original, XyzConfig, RgbConfig);
        var roundtrip = Okhsl.FromOklab(oklab, XyzConfig, RgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}