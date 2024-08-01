using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class GamutMappingTests
{
    private const double Tolerance = 0.00000000001;
    
    [TestCase(0, 0, 0)]
    [TestCase(0.00000000001, 0, 0)]
    [TestCase(0, 0.00000000001, 0)]
    [TestCase(0, 0, 0.00000000001)]
    [TestCase(1, 1, 0.99999999999)]
    [TestCase(1, 0.99999999999, 1)]
    [TestCase(0.99999999999, 1, 1)]
    [TestCase(1, 1, 1)]
    public void RgbInGamut(double r, double g, double b)
    {
        var original = new Unicolour(ColourSpace.Rgb, r, g, b);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.True);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
        TestUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.Triplet, Tolerance);
    }
    
    [TestCase(-0.00000000001, 0, 0)]
    [TestCase(0, -0.00000000001, 0)]
    [TestCase(0, 0, -0.00000000001)]
    [TestCase(1, 1, 1.00000000001)]
    [TestCase(1, 1.00000000001, 1)]
    [TestCase(1.00000000001, 1, 1)]
    public void RgbOutOfGamut(double r, double g, double b)
    {
        var original = new Unicolour(ColourSpace.Rgb, r, g, b);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.False);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
    }
    
    [TestCase(1.0, 0, 0)]
    [TestCase(1.00000000001, 0, 0)]
    public void MaxLightness(double l, double c, double h)
    {
        var white = new Unicolour(ColourSpace.Oklch, l, c, h);
        var gamutMapped = white.MapToGamut();
        TestUtils.AssertTriplet<Rgb>(gamutMapped, new(1, 1, 1), Tolerance);
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(-0.00000000001, 0, 0)]
    public void MinLightness(double l, double c, double h)
    {
        var white = new Unicolour(ColourSpace.Oklch, l, c, h);
        var gamutMapped = white.MapToGamut();
        TestUtils.AssertTriplet<Rgb>(gamutMapped, new(0, 0, 0), Tolerance);
    }
    
    [TestCase]
    public void NoChromaInGamut()
    {
        // OKLCH without chroma isn't processed by the gamut mapping algorithm (chroma is already considered to be converged, can't go lower)
        // OKLCH (0.5, 0, 0) corresponds to an in-gamut sRGB (0.39, 0.39, 0.39), so make sure that original RGB is returned
        var original = new Unicolour(ColourSpace.Oklch, 0.5, 0, 0);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.True);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
        TestUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.Triplet, Tolerance);
    }
    
    [TestCase]
    public void NoChromaOutOfGamut()
    {
        // OKLCH without chroma isn't processed by the gamut mapping algorithm (chroma is already considered to be converged, can't go lower)
        // OKLCH (0.99999999, 0, 0) corresponds to an out-of-gamut RGB (0.999999956, 0.999999995, 1.000000102)
        // so make sure RGB is brought into gamut anyway
        var original = new Unicolour(ColourSpace.Oklch, 0.99999999, 0, 0);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.False);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
    }
    
    [Test]
    public void NegativeChroma()
    {
        var original = new Unicolour(ColourSpace.Oklch, 0.5, -0.5, 180);
        var gamutMapped = original.MapToGamut();
        Assert.That(gamutMapped.Rgb.IsInGamut, Is.True);
        TestUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.ConstrainedTriplet, Tolerance);
    }

    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NaN)]
    public void ChromaCannotConverge(double chroma)
    {
        var original = new Unicolour(ColourSpace.Oklch, 0.5, chroma, 180);
        Assert.DoesNotThrow(() => original.MapToGamut());
    }
    
    [Test]
    public void NegativeHue()
    {
        var original = new Unicolour(ColourSpace.Oklch, 0.5, 0.1, -180);
        var gamutMapped = original.MapToGamut();
        Assert.That(gamutMapped.Rgb.IsInGamut, Is.True);
        TestUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.ConstrainedTriplet, 0.0001);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void RandomRgb(ColourTriplet triplet)
    {
        var original = new Unicolour(ColourSpace.Rgb, triplet.Tuple);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.True);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
        TestUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.Triplet, Tolerance);
    }

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklchTriplets))]
    public void RandomOklch(ColourTriplet triplet)
    {
        var original = new Unicolour(ColourSpace.Oklch, triplet.Tuple);
        var gamutMapped = original.MapToGamut();
        Assert.That(gamutMapped.Rgb.IsInGamut, Is.True);
        Assert.That(gamutMapped.Rgb.Triplet, Is.EqualTo(gamutMapped.Rgb.ConstrainedTriplet));
    }
    
    [Test] // https://www.w3.org/TR/css-color-4/#GM-chroma
    public void YellowOutOfGamut()
    {
        const double tripletTolerance = 0.00005;
        var yellowDisplayP3 = new Unicolour(new Configuration(RgbConfiguration.DisplayP3), ColourSpace.Rgb, 1, 1, 0);
        var yellowStandardRgb = yellowDisplayP3.ConvertToConfiguration(new Configuration(RgbConfiguration.StandardRgb));
        TestUtils.AssertTriplet<Rgb>(yellowDisplayP3, new(1.00000, 1.00000, 0.00000), tripletTolerance);
        TestUtils.AssertTriplet<Rgb>(yellowStandardRgb, new(1.00000, 1.00000, -0.34630), tripletTolerance);
        
        // different because Unicolour doesn't limit Oklab / Oklch to sRGB
        TestUtils.AssertTriplet<Oklch>(yellowStandardRgb, new(0.96476, 0.24503, 110.23), tripletTolerance);
        TestUtils.AssertTriplet<Oklch>(yellowDisplayP3, new(0.96798, 0.21101, 109.77), tripletTolerance);

        var gamutMappedDisplayP3 = yellowDisplayP3.MapToGamut();
        Assert.That(gamutMappedDisplayP3, Is.EqualTo(yellowDisplayP3));
        Assert.That(yellowDisplayP3.IsInDisplayGamut, Is.True);
        Assert.That(gamutMappedDisplayP3.IsInDisplayGamut, Is.True);

        const double gamutMapTolerance = 0.0025;
        var gamutMappedStandardRgb = yellowStandardRgb.MapToGamut();
        Assert.That(yellowStandardRgb.Oklch.C, Is.EqualTo(0.245).Within(gamutMapTolerance));
        Assert.That(yellowStandardRgb.IsInDisplayGamut, Is.False);
        Assert.That(gamutMappedStandardRgb.Oklch.C, Is.EqualTo(0.210).Within(gamutMapTolerance));
        Assert.That(gamutMappedStandardRgb.IsInDisplayGamut, Is.True);
    }
    
    /* search result that is in gamut before being clipped is hard to come by */
    [TestCase(ColourSpace.Oklch, 0.495275659305237, 0.23728554321306156, 320.367400932029)]
    [TestCase(ColourSpace.Rgb, 0.5981, -0.0003, 0.6961)]
    public void ResultInGamutNotClipped(ColourSpace colourSpace, double first, double second, double third)
    {
        var original = new Unicolour(colourSpace, first, second, third);
        var gamutMapped = original.MapToGamut();
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
    }
}