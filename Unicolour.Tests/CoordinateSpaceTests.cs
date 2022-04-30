namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

/*
 * Conversions that are just translations to/from cylindrical coordinate spaces
 * should have input values clamped to the range of the source coordinate space
 * otherwise they will be mapped to incorrect values
 * e.g. RGB with negative -> HSB should produce the same value as RGB with zero -> HSB
 */
public class CoordinateSpaceTests
{
    private const double HueUpperOutRange = 360.00001;
    private const double HueUpperInRange = 0.00001;
    private const double HueLowerOutRange = -0.00001;
    private const double HueLowerInRange = 359.99999;
    
    private readonly ColourTriplet hsxUpperOutRange = new(HueUpperOutRange, 100, double.PositiveInfinity);
    private readonly ColourTriplet hsxUpperInRange = new(HueUpperInRange, 1, 1);
    private readonly ColourTriplet hsxLowerOutRange = new(HueLowerOutRange, -100, double.NegativeInfinity);
    private readonly ColourTriplet hsxLowerInRange = new(HueLowerInRange, 0, 0);
    
    private readonly ColourTriplet rgbUpperOutRange = new(1.00001, 100, double.PositiveInfinity);
    private readonly ColourTriplet rgbUpperInRange = new(1, 1, 1);
    private readonly ColourTriplet rgbLowerOutRange = new(-0.00001, -100, double.NegativeInfinity);
    private readonly ColourTriplet rgbLowerInRange = new(0, 0, 0);
    
    [Test]
    public void CartesianRgbToCylindricalHsb()
    {
        AssertRgbToHsb(rgbUpperInRange, rgbUpperOutRange);
        AssertRgbToHsb(rgbLowerInRange, rgbLowerOutRange);
    }

    [Test]
    public void CylindricalHsbToCartesianRgb()
    {
        AssertHsbToRgb(hsxUpperInRange, hsxUpperOutRange);
        AssertHsbToRgb(hsxLowerInRange, hsxLowerOutRange);
    }
    
    [Test]
    public void CylindricalHsbToCylindricalHsl()
    {
        AssertHsbToHsl(hsxUpperInRange, hsxUpperOutRange);
        AssertHsbToHsl(hsxLowerInRange, hsxLowerOutRange);
    }
    
    [Test]
    public void CylindricalHslToCylindricalHsb()
    {
        AssertHslToHsb(hsxUpperInRange, hsxUpperOutRange);
        AssertHslToHsb(hsxLowerInRange, hsxLowerOutRange);
    }

    [Test]
    public void CylindricalLchabCartesianLab()
    {
        AssertLchabToLab(HueUpperInRange, HueUpperOutRange);
        AssertLchabToLab(HueLowerInRange, HueLowerOutRange);
    }
    
    [Test]
    public void CylindricalLchuvCartesianLuv()
    {
        AssertLchuvToLuv(HueUpperInRange, HueUpperOutRange);
        AssertLchuvToLuv(HueLowerInRange, HueLowerOutRange);
    }
    
    [Test]
    public void CylindricalOklchCartesianOklab()
    {
        AssertOklchToOklab(HueUpperInRange, HueUpperOutRange);
        AssertOklchToOklab(HueLowerInRange, HueLowerOutRange);
    }
    
    private static void AssertRgbToHsb(ColourTriplet inRange, ColourTriplet outRange)
    {
        var rgbInRange = new Rgb(inRange.First, inRange.Second, inRange.Third, Configuration.Default);
        var rgbOutRange = new Rgb(outRange.First, outRange.Second, outRange.Third, Configuration.Default);
        var hsbFromInRange = Conversion.RgbToHsb(rgbInRange);
        var hsbFromOutRange = Conversion.RgbToHsb(rgbOutRange);
        AssertSourceTriplets(AsTriplets(rgbInRange), AsTriplets(rgbOutRange));
        AssertDestinationTriplets(AsTriplets(hsbFromInRange), AsTriplets(hsbFromOutRange));
    }

    private static void AssertHsbToRgb(ColourTriplet inRange, ColourTriplet outRange)
    {
        var hsbInRange = new Hsb(inRange.First, inRange.Second, inRange.Third);
        var hsbOutRange = new Hsb(outRange.First, outRange.Second, outRange.Third);
        var rgbFromInRange = Conversion.HsbToRgb(hsbInRange, Configuration.Default);
        var rgbFromOutRange = Conversion.HsbToRgb(hsbOutRange, Configuration.Default);
        AssertSourceTriplets(AsTriplets(hsbInRange), AsTriplets(hsbOutRange));
        AssertDestinationTriplets(AsTriplets(rgbFromInRange), AsTriplets(rgbFromOutRange));
    }
    
    private static void AssertHsbToHsl(ColourTriplet inRange, ColourTriplet outRange)
    {
        var hsbInRange = new Hsb(inRange.First, inRange.Second, inRange.Third);
        var hsbOutRange = new Hsb(outRange.First, outRange.Second, outRange.Third);
        var hslFromInRange = Conversion.HsbToHsl(hsbInRange);
        var hslFromOutRange = Conversion.HsbToHsl(hsbOutRange);
        AssertSourceTriplets(AsTriplets(hsbInRange), AsTriplets(hsbOutRange));
        AssertDestinationTriplets(AsTriplets(hslFromInRange), AsTriplets(hslFromOutRange));
    }

    private static void AssertHslToHsb(ColourTriplet inRange, ColourTriplet outRange)
    {
        var hslInRange = new Hsl(inRange.First, inRange.Second, inRange.Third);
        var hslOutRange = new Hsl(outRange.First, outRange.Second, outRange.Third);
        var hsbFromInRange = Conversion.HslToHsb(hslInRange);
        var hsbFromOutRange = Conversion.HslToHsb(hslOutRange);
        AssertSourceTriplets(AsTriplets(hslInRange), AsTriplets(hslOutRange));
        AssertDestinationTriplets(AsTriplets(hsbFromInRange), AsTriplets(hsbFromOutRange));
    }
    
    private static void AssertLchabToLab(double hueInRange, double hueOutRange)
    {
        var lchabInRange = new Lchab(50, 100, hueInRange);
        var lchabOutRange = new Lchab(50, 100, hueOutRange);
        var labFromInRange = Conversion.LchabToLab(lchabInRange);
        var labFromOutRange = Conversion.LchabToLab(lchabOutRange);
        AssertSourceTriplets(AsTriplets(lchabInRange), AsTriplets(lchabOutRange));
        AssertTriplet(labFromOutRange.Triplet, labFromInRange.Triplet);
    }
    
    private static void AssertLchuvToLuv(double hueInRange, double hueOutRange)
    {
        var lchuvInRange = new Lchuv(50, 100, hueInRange);
        var lchuvOutRange = new Lchuv(50, 100, hueOutRange);
        var luvFromInRange = Conversion.LchuvToLuv(lchuvInRange);
        var luvFromOutRange = Conversion.LchuvToLuv(lchuvOutRange);
        AssertSourceTriplets(AsTriplets(lchuvInRange), AsTriplets(lchuvOutRange));
        AssertTriplet(luvFromOutRange.Triplet, luvFromInRange.Triplet);
    }
    
    private static void AssertOklchToOklab(double hueInRange, double hueOutRange)
    {
        var oklchInRange = new Oklch(0.5, 1, hueInRange);
        var oklchOutRange = new Oklch(0.5, 1, hueOutRange);
        var oklabFromInRange = Conversion.OklchToOklab(oklchInRange);
        var oklabFromOutRange = Conversion.OklchToOklab(oklchOutRange);
        AssertSourceTriplets(AsTriplets(oklchInRange), AsTriplets(oklchOutRange));
        AssertTriplet(oklabFromOutRange.Triplet, oklabFromInRange.Triplet);
    }

    private static void AssertSourceTriplets(Triplets sourceInRange, Triplets sourceOutRange)
    {
        AssertTriplet(sourceOutRange.Constrained, sourceInRange.Unconstrained);
    }
    
    private static void AssertDestinationTriplets(Triplets destinationFromInRange, Triplets destinationFromOutRange)
    {
        AssertTriplet(destinationFromOutRange.Unconstrained, destinationFromInRange.Unconstrained);
        AssertTriplet(destinationFromOutRange.Constrained, destinationFromInRange.Unconstrained);
    }

    private static void AssertTriplet(ColourTriplet actual, ColourTriplet expected)
    {
        AssertUtils.AssertColourTriplet(actual, expected, 0.00001);
    }

    private static Triplets AsTriplets(Rgb rgb) => new(rgb.Triplet, rgb.ConstrainedTriplet);
    private static Triplets AsTriplets(Hsb hsb) => new(hsb.Triplet, hsb.ConstrainedTriplet);
    private static Triplets AsTriplets(Hsl hsl) => new(hsl.Triplet, hsl.ConstrainedTriplet);
    private static Triplets AsTriplets(Lchab lchab) => new(lchab.Triplet, lchab.ConstrainedTriplet);
    private static Triplets AsTriplets(Lchuv lchuv) => new(lchuv.Triplet, lchuv.ConstrainedTriplet);
    private static Triplets AsTriplets(Oklch oklch) => new(oklch.Triplet, oklch.ConstrainedTriplet);

    private record Triplets(ColourTriplet Unconstrained, ColourTriplet Constrained);
}