namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

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
        var original = Unicolour.FromRgb(r, g, b);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.True);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
        AssertUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.Triplet, Tolerance);
    }
    
    [TestCase(-0.00000000001, 0, 0)]
    [TestCase(0, -0.00000000001, 0)]
    [TestCase(0, 0, -0.00000000001)]
    [TestCase(1, 1, 1.00000000001)]
    [TestCase(1, 1.00000000001, 1)]
    [TestCase(1.00000000001, 1, 1)]
    public void RgbOutOfGamut(double r, double g, double b)
    {
        var original = Unicolour.FromRgb(r, g, b);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.False);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
    }
    
    [TestCase(1.0, 0, 0)]
    [TestCase(1.00000000001, 0, 0)]
    public void MaxLightness(double l, double c, double h)
    {
        var white = Unicolour.FromOklch(l, c, h);
        var gamutMapped = white.MapToGamut();
        AssertUtils.AssertTriplet<Rgb>(gamutMapped, new(1, 1, 1), Tolerance);
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(-0.00000000001, 0, 0)]
    public void MinLightness(double l, double c, double h)
    {
        var white = Unicolour.FromOklch(l, c, h);
        var gamutMapped = white.MapToGamut();
        AssertUtils.AssertTriplet<Rgb>(gamutMapped, new(0, 0, 0), Tolerance);
    }
    
    [TestCase]
    public void NoChromaInGamut()
    {
        // OKLCH without chroma isn't processed by the gamut mapping algorithm (chroma is already considered to be converged, can't go lower)
        // OKLCH (0.5, 0, 0) corresponds to an in-gamut sRGB (0.39, 0.39, 0.39), so make sure that original RGB is returned
        var original = Unicolour.FromOklch(0.5, 0, 0);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.True);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
        AssertUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.Triplet, Tolerance);
    }
    
    [TestCase]
    public void NoChromaOutOfGamut()
    {
        // OKLCH without chroma isn't processed by the gamut mapping algorithm (chroma is already considered to be converged, can't go lower)
        // OKLCH (0.99999, 0, 0) corresponds to an out-of-gamut RGB (1.00010, 0.99998, 0.99974), so make sure RGB is brought into gamut anyway
        var original = Unicolour.FromOklch(0.99999, 0, 0);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.False);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
    }
    
    [Test]
    public void NegativeChroma()
    {
        var original = Unicolour.FromOklch(0.5, -0.5, 180);
        var gamutMapped = original.MapToGamut();
        Assert.That(gamutMapped.Rgb.IsInGamut, Is.True);
        AssertUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.ConstrainedTriplet, Tolerance);
    }

    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NaN)]
    public void ChromaCannotConverge(double chroma)
    {
        var original = Unicolour.FromOklch(0.5, chroma, 180);
        Assert.DoesNotThrow(() => original.MapToGamut());
    }
    
    [Test]
    public void NegativeHue()
    {
        var original = Unicolour.FromOklch(0.5, 0.1, -180);
        var gamutMapped = original.MapToGamut();
        Assert.That(gamutMapped.Rgb.IsInGamut, Is.True);
        AssertUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.ConstrainedTriplet, 0.0001);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void RandomRgb(ColourTriplet triplet)
    {
        var original = Unicolour.FromRgb(triplet.Tuple);
        var gamutMapped = original.MapToGamut();
        Assert.That(original.IsInDisplayGamut, Is.True);
        Assert.That(gamutMapped.IsInDisplayGamut, Is.True);
        AssertUtils.AssertTriplet(gamutMapped.Rgb.Triplet, original.Rgb.Triplet, Tolerance);
    }

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklchTriplets))]
    public void RandomOklch(ColourTriplet triplet)
    {
        var original = Unicolour.FromOklch(triplet.Tuple);
        var gamutMapped = original.MapToGamut();
        Assert.That(gamutMapped.Rgb.IsInGamut, Is.True);
        Assert.That(gamutMapped.Rgb.Triplet, Is.EqualTo(gamutMapped.Rgb.ConstrainedTriplet));
    }
    
    [Test]
    public void YellowOutOfGamut()
    {
        const double tolerance = 0.001;
        var yellowDisplayP3 = Unicolour.FromRgb(new Configuration(RgbConfiguration.DisplayP3), 1, 1, 0);
        var yellowStandardRgb = yellowDisplayP3.ConvertToConfiguration(new Configuration(RgbConfiguration.StandardRgb));
        AssertUtils.AssertTriplet<Rgb>(yellowDisplayP3, new(1.00000, 1.00000, 0.00000), tolerance);
        AssertUtils.AssertTriplet<Rgb>(yellowStandardRgb, new(1.00000, 1.00000, -0.34630), tolerance);
        AssertUtils.AssertTriplet<Oklch>(yellowDisplayP3, new(0.96476, 0.24503, 110.23), tolerance);
        AssertUtils.AssertTriplet<Oklch>(yellowStandardRgb, new(0.96476, 0.24503, 110.23), tolerance);

        var gamutMappedDisplayP3 = yellowDisplayP3.MapToGamut();
        Assert.That(gamutMappedDisplayP3, Is.EqualTo(yellowDisplayP3));
        Assert.That(yellowDisplayP3.IsInDisplayGamut, Is.True);
        Assert.That(gamutMappedDisplayP3.IsInDisplayGamut, Is.True);

        var gamutMappedStandardRgb = yellowStandardRgb.MapToGamut();
        Assert.That(yellowStandardRgb.Oklch.C, Is.EqualTo(0.245).Within(tolerance));
        Assert.That(yellowStandardRgb.IsInDisplayGamut, Is.False);
        Assert.That(gamutMappedStandardRgb.Oklch.C, Is.EqualTo(0.210).Within(tolerance));
        Assert.That(gamutMappedStandardRgb.IsInDisplayGamut, Is.True);
    }
}