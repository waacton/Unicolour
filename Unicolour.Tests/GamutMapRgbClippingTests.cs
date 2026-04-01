using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class GamutMapRgbClippingTests
{
    private static readonly TestCaseData[] TestData =
    [
        new(new Rgb(-0.00001, 0.0, 0.0), new Rgb(0.0, 0.0, 0.0)),
        new(new Rgb(0.0, -0.00001, 0.0), new Rgb(0.0, 0.0, 0.0)),
        new(new Rgb(0.0, 0.0, -0.00001), new Rgb(0.0, 0.0, 0.0)),
        new(new Rgb(1.00001, 1.0, 1.0), new Rgb(1.0, 1.0, 1.0)),
        new(new Rgb(1.0, 1.00001, 1.0), new Rgb(1.0, 1.0, 1.0)),
        new(new Rgb(1.0, 1.0, 1.00001), new Rgb(1.0, 1.0, 1.0)),
        new(new Rgb(double.MaxValue, 0.5, 0.5), new Rgb(1.0, 0.5, 0.5)),
        new(new Rgb(0.5, double.MaxValue, 0.5), new Rgb(0.5, 1.0, 0.5)),
        new(new Rgb(0.5, 0.5, double.MaxValue), new Rgb(0.5, 0.5, 1.0)),
        new(new Rgb(double.MinValue, 0.5, 0.5), new Rgb(0.0, 0.5, 0.5)),
        new(new Rgb(0.5, double.MinValue, 0.5), new Rgb(0.5, 0.0, 0.5)),
        new(new Rgb(0.5, 0.5, double.MinValue), new Rgb(0.5, 0.5, 0.0)),
        new(new Rgb(double.PositiveInfinity, 0.5, 0.5), new Rgb(1.0, 0.5, 0.5)),
        new(new Rgb(0.5, double.PositiveInfinity, 0.5), new Rgb(0.5, 1.0, 0.5)),
        new(new Rgb(0.5, 0.5, double.PositiveInfinity), new Rgb(0.5, 0.5, 1.0)),
        new(new Rgb(double.NegativeInfinity, 0.5, 0.5), new Rgb(0.0, 0.5, 0.5)),
        new(new Rgb(0.5, double.NegativeInfinity, 0.5), new Rgb(0.5, 0.0, 0.5)),
        new(new Rgb(0.5, 0.5, double.NegativeInfinity), new Rgb(0.5, 0.5, 0.0))
    ];
    
    [TestCaseSource(nameof(TestData))]
    public void Clip(Rgb outOfGamut, Rgb expected)
    {
        var colour = new Unicolour(ColourSpace.Rgb, outOfGamut.Tuple);
        var gamutMapped = colour.MapToRgbGamut(GamutMap.RgbClipping);
        Assert.That(gamutMapped.IsInRgbGamut, Is.True);
        TestUtils.AssertColour(gamutMapped, expected, 0);
    }
}