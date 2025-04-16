using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class GamutMapRgbClippingTests
{
    private static readonly TestCaseData[] TestData =
    [
        new(new ColourTriplet(-0.00001, 0.0, 0.0), new ColourTriplet(0.0, 0.0, 0.0)),
        new(new ColourTriplet(0.0, -0.00001, 0.0), new ColourTriplet(0.0, 0.0, 0.0)),
        new(new ColourTriplet(0.0, 0.0, -0.00001), new ColourTriplet(0.0, 0.0, 0.0)),
        new(new ColourTriplet(1.00001, 1.0, 1.0), new ColourTriplet(1.0, 1.0, 1.0)),
        new(new ColourTriplet(1.0, 1.00001, 1.0), new ColourTriplet(1.0, 1.0, 1.0)),
        new(new ColourTriplet(1.0, 1.0, 1.00001), new ColourTriplet(1.0, 1.0, 1.0)),
        new(new ColourTriplet(double.MaxValue, 0.5, 0.5), new ColourTriplet(1.0, 0.5, 0.5)),
        new(new ColourTriplet(0.5, double.MaxValue, 0.5), new ColourTriplet(0.5, 1.0, 0.5)),
        new(new ColourTriplet(0.5, 0.5, double.MaxValue), new ColourTriplet(0.5, 0.5, 1.0)),
        new(new ColourTriplet(double.MinValue, 0.5, 0.5), new ColourTriplet(0.0, 0.5, 0.5)),
        new(new ColourTriplet(0.5, double.MinValue, 0.5), new ColourTriplet(0.5, 0.0, 0.5)),
        new(new ColourTriplet(0.5, 0.5, double.MinValue), new ColourTriplet(0.5, 0.5, 0.0)),
        new(new ColourTriplet(double.PositiveInfinity, 0.5, 0.5), new ColourTriplet(1.0, 0.5, 0.5)),
        new(new ColourTriplet(0.5, double.PositiveInfinity, 0.5), new ColourTriplet(0.5, 1.0, 0.5)),
        new(new ColourTriplet(0.5, 0.5, double.PositiveInfinity), new ColourTriplet(0.5, 0.5, 1.0)),
        new(new ColourTriplet(double.NegativeInfinity, 0.5, 0.5), new ColourTriplet(0.0, 0.5, 0.5)),
        new(new ColourTriplet(0.5, double.NegativeInfinity, 0.5), new ColourTriplet(0.5, 0.0, 0.5)),
        new(new ColourTriplet(0.5, 0.5, double.NegativeInfinity), new ColourTriplet(0.5, 0.5, 0.0))
    ];
    
    [TestCaseSource(nameof(TestData))]
    public void Clip(ColourTriplet outOfGamut, ColourTriplet expected)
    {
        var colour = new Unicolour(ColourSpace.Rgb, outOfGamut.Tuple);
        var gamutMapped = colour.MapToRgbGamut(GamutMap.RgbClipping);
        Assert.That(gamutMapped.IsInRgbGamut, Is.True);
        TestUtils.AssertTriplet<Rgb>(gamutMapped, expected, 0);
    }
}