using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class BlendTests
{
    // for separable blend modes where channels are each channel is independent
    // these colours include channels that are greater than, less than, and equal to the other colour
    private static readonly Unicolour pink = new(ColourSpace.Rgb255, 230, 26, 128, 255 / 255.0);
    private static readonly Unicolour blue = new(ColourSpace.Rgb255, 51, 102, 128, 255 / 255.0);
    
    private static readonly Unicolour pinkAlpha = new(ColourSpace.Rgb255, 230, 26, 128, 153 / 255.0);
    private static readonly Unicolour blueAlpha = new(ColourSpace.Rgb255, 51, 102, 128, 191 / 255.0);

    private static readonly Unicolour redOutOfGamut = new(ColourSpace.Rgb, 1.5, 0, -0.5);
    private static readonly Unicolour blueOutOfGamut = new(ColourSpace.Rgb, -0.5, 0, 1.5);
    
    /*
     * for convenience, most test values are from https://www.sarasoueidan.com/demos/css-blender/
     * except where backdrop is not opaque, which are taken from Paint.NET layers (but doesn't support 7 of these blend modes)
     * ----------
     * expected alpha is always 1.0 (opaque) unless both source and backdrop are < 1.0 alpha
     */
    
    [TestCase(BlendMode.Normal, "#FFFFFF")]
    [TestCase(BlendMode.Multiply, "#000000")]
    [TestCase(BlendMode.Screen, "#FFFFFF")]
    [TestCase(BlendMode.Overlay, "#000000")]
    [TestCase(BlendMode.Darken, "#000000")]
    [TestCase(BlendMode.Lighten, "#FFFFFF")]
    [TestCase(BlendMode.ColourDodge, "#000000")]
    [TestCase(BlendMode.ColourBurn, "#000000")]
    [TestCase(BlendMode.HardLight, "#FFFFFF")]
    [TestCase(BlendMode.SoftLight, "#000000")]
    [TestCase(BlendMode.Difference, "#FFFFFF")]
    [TestCase(BlendMode.Exclusion, "#FFFFFF")]
    [TestCase(BlendMode.Hue, "#000000")]
    [TestCase(BlendMode.Saturation, "#000000")]
    [TestCase(BlendMode.Colour, "#000000")]
    [TestCase(BlendMode.Luminosity, "#FFFFFF")]
    public void WhiteOnBlack(BlendMode blendMode, string expectedHex) => AssertBlend(StandardRgb.White, StandardRgb.Black, blendMode, expectedHex, expectedAlpha: 1.0);
    
    [TestCase(BlendMode.Normal, "#000000")]
    [TestCase(BlendMode.Multiply, "#000000")]
    [TestCase(BlendMode.Screen, "#FFFFFF")]
    [TestCase(BlendMode.Overlay, "#FFFFFF")]
    [TestCase(BlendMode.Darken, "#000000")]
    [TestCase(BlendMode.Lighten, "#FFFFFF")]
    [TestCase(BlendMode.ColourDodge, "#FFFFFF")]
    [TestCase(BlendMode.ColourBurn, "#FFFFFF")]
    [TestCase(BlendMode.HardLight, "#000000")]
    [TestCase(BlendMode.SoftLight, "#FFFFFF")]
    [TestCase(BlendMode.Difference, "#FFFFFF")]
    [TestCase(BlendMode.Exclusion, "#FFFFFF")]
    [TestCase(BlendMode.Hue, "#FFFFFF")]
    [TestCase(BlendMode.Saturation, "#FFFFFF")]
    [TestCase(BlendMode.Colour, "#FFFFFF")]
    [TestCase(BlendMode.Luminosity, "#000000")]
    public void BlackOnWhite(BlendMode blendMode, string expectedHex) => AssertBlend(StandardRgb.Black, StandardRgb.White, blendMode, expectedHex, expectedAlpha: 1.0);
    
    [TestCase(BlendMode.Normal, "#FFFFFF")]
    [TestCase(BlendMode.Multiply, "#808080")]
    [TestCase(BlendMode.Screen, "#FFFFFF")]
    [TestCase(BlendMode.Overlay, "#FFFFFF")]
    [TestCase(BlendMode.Darken, "#808080")]
    [TestCase(BlendMode.Lighten, "#FFFFFF")]
    [TestCase(BlendMode.ColourDodge, "#FFFFFF")]
    [TestCase(BlendMode.ColourBurn, "#808080")]
    [TestCase(BlendMode.HardLight, "#FFFFFF")]
    [TestCase(BlendMode.SoftLight, "#B5B5B5")]
    [TestCase(BlendMode.Difference, "#808080")]
    [TestCase(BlendMode.Exclusion, "#808080")]
    [TestCase(BlendMode.Hue, "#808080")]
    [TestCase(BlendMode.Saturation, "#808080")]
    [TestCase(BlendMode.Colour, "#808080")]
    [TestCase(BlendMode.Luminosity, "#FFFFFF")]
    public void WhiteOnGrey(BlendMode blendMode, string expectedHex) => AssertBlend(StandardRgb.White, StandardRgb.Grey, blendMode, expectedHex, expectedAlpha: 1.0);
    
    [TestCase(BlendMode.Normal, "#000000")]
    [TestCase(BlendMode.Multiply, "#000000")]
    [TestCase(BlendMode.Screen, "#808080")]
    [TestCase(BlendMode.Overlay, "#000000")]
    [TestCase(BlendMode.Darken, "#000000")]
    [TestCase(BlendMode.Lighten, "#808080")]
    [TestCase(BlendMode.ColourDodge, "#808080")]
    [TestCase(BlendMode.ColourBurn, "#000000")]
    [TestCase(BlendMode.HardLight, "#000000")]
    [TestCase(BlendMode.SoftLight, "#404040")]
    [TestCase(BlendMode.Difference, "#808080")]
    [TestCase(BlendMode.Exclusion, "#808080")]
    [TestCase(BlendMode.Hue, "#808080")]
    [TestCase(BlendMode.Saturation, "#808080")]
    [TestCase(BlendMode.Colour, "#808080")]
    [TestCase(BlendMode.Luminosity, "#000000")]
    public void BlackOnGrey(BlendMode blendMode, string expectedHex) => AssertBlend(StandardRgb.Black, StandardRgb.Grey, blendMode, expectedHex, expectedAlpha: 1.0);
    
    [TestCase(BlendMode.Normal, "#E61A80")]
    [TestCase(BlendMode.Multiply, "#2E0B40")]
    [TestCase(BlendMode.Screen, "#EB76C0")]
    [TestCase(BlendMode.Overlay, "#5C1580")]
    [TestCase(BlendMode.Darken, "#331A80")]
    [TestCase(BlendMode.Lighten, "#E66680")]
    [TestCase(BlendMode.ColourDodge, "#FF72FF")]
    [TestCase(BlendMode.ColourBurn, "#1D0002")]
    [TestCase(BlendMode.HardLight, "#D71580")]
    [TestCase(BlendMode.SoftLight, "#663580")]
    [TestCase(BlendMode.Difference, "#B34C00")]
    [TestCase(BlendMode.Exclusion, "#BD6A80")]
    [TestCase(BlendMode.Hue, "#8B3E65")]
    [TestCase(BlendMode.Saturation, "#0076B3")]
    [TestCase(BlendMode.Colour, "#DD1177")]
    [TestCase(BlendMode.Luminosity, "#3C6F89")]
    public void PinkOnBlue(BlendMode blendMode, string expectedHex) => AssertBlend(pink, blue, blendMode, expectedHex, expectedAlpha: 1.0);
    
    [TestCase(BlendMode.Normal, "#9E3980")]
    [TestCase(BlendMode.Multiply, "#30305A")]
    [TestCase(BlendMode.Screen, "#A170A6")]
    [TestCase(BlendMode.Overlay, "#4C3680")]
    [TestCase(BlendMode.Darken, "#333980")]
    [TestCase(BlendMode.Lighten, "#9E6680")]
    [TestCase(BlendMode.ColourDodge, "#AD6dCC")]
    [TestCase(BlendMode.ColourBurn, "#262935")]
    [TestCase(BlendMode.HardLight, "#953680")]
    [TestCase(BlendMode.SoftLight, "#524980")]
    [TestCase(BlendMode.Difference, "#7F5633")]
    [TestCase(BlendMode.Exclusion, "#85687F")]
    [TestCase(BlendMode.Hue, "#684E70")]
    [TestCase(BlendMode.Saturation, "#14709F")]
    [TestCase(BlendMode.Colour, "#99337B")]
    [TestCase(BlendMode.Luminosity, "#396C86")]
    public void PinkAlphaOnBlue(BlendMode blendMode, string expectedHex) => AssertBlend(pinkAlpha, blue, blendMode, expectedHex, expectedAlpha: 1.0);
    
    [TestCase(BlendMode.Normal, "#336680")]
    [TestCase(BlendMode.Multiply, "#2E0B40")]
    [TestCase(BlendMode.Screen, "#EB76C0")]
    [TestCase(BlendMode.Overlay, "#D71580")]
    [TestCase(BlendMode.Darken, "#331A80")]
    [TestCase(BlendMode.Lighten, "#E66680")]
    [TestCase(BlendMode.ColourDodge, "#FF2BFF")]
    [TestCase(BlendMode.ColourBurn, "#820002")]
    [TestCase(BlendMode.HardLight, "#5C1580")]
    [TestCase(BlendMode.SoftLight, "#D81580")]
    [TestCase(BlendMode.Difference, "#B34C00")]
    [TestCase(BlendMode.Exclusion, "#BD6A80")]
    [TestCase(BlendMode.Hue, "#0082C5")]
    [TestCase(BlendMode.Saturation, "#94476E")]
    [TestCase(BlendMode.Colour, "#3C6F89")]
    [TestCase(BlendMode.Luminosity, "#DD1177")]
    public void BlueOnPink(BlendMode blendMode, string expectedHex) => AssertBlend(blue, pink, blendMode, expectedHex, expectedAlpha: 1.0);
    
    [TestCase(BlendMode.Normal, "#605280")]
    [TestCase(BlendMode.Multiply, "#5C0F50")]
    [TestCase(BlendMode.Screen, "#EA5EB0")]
    [TestCase(BlendMode.Overlay, "#DA1680")]
    [TestCase(BlendMode.Darken, "#601A80")]
    [TestCase(BlendMode.Lighten, "#E65280")]
    [TestCase(BlendMode.ColourDodge, "#F927DF")]
    [TestCase(BlendMode.ColourBurn, "#9B0722")]
    [TestCase(BlendMode.HardLight, "#7E1680")]
    [TestCase(BlendMode.SoftLight, "#DC1680")]
    [TestCase(BlendMode.Difference, "#C03E20")]
    [TestCase(BlendMode.Exclusion, "#C65680")]
    [TestCase(BlendMode.Hue, "#3A68B5")]
    [TestCase(BlendMode.Saturation, "#A93C72")]
    [TestCase(BlendMode.Colour, "#675987")]
    [TestCase(BlendMode.Luminosity, "#Df1379")]
    public void BlueAlphaOnPink(BlendMode blendMode, string expectedHex) => AssertBlend(blueAlpha, pink, blendMode, expectedHex, expectedAlpha: 1.0);
    
    // expected alpha 229 is the same regardless of blend mode = (0.6 + 0.75 * (1 - 0.6)) * 255
    [TestCase(BlendMode.Normal, "#AA3380")]
    [TestCase(BlendMode.Multiply, "#4E2B5F")]
    [TestCase(BlendMode.Screen, "#AD61A0")]
    [TestCase(BlendMode.Overlay, "#653080")]
    [TestCase(BlendMode.Darken, "#503380")]
    [TestCase(BlendMode.Lighten, "#AA5980")]
    [TestCase(BlendMode.ColourDodge, "#B75EBF")]
    [TestCase(BlendMode.ColourBurn, "#452640")]
    [TestCase(BlendMode.Difference, "#904C3F")]
    public void PinkAlphaOnBlueAlpha(BlendMode blendMode, string expectedHex) => AssertBlend(pinkAlpha, blueAlpha, blendMode, expectedHex, expectedAlpha: 229 / 255.0);
    
    // expected alpha 229 is the same regardless of blend mode = (0.75 + 0.6 * (1 - 0.75)) * 255
    [TestCase(BlendMode.Normal, "#505980")]
    [TestCase(BlendMode.Multiply, "#4E2B5F")]
    [TestCase(BlendMode.Screen, "#AD61A0")]
    [TestCase(BlendMode.Overlay, "#A33080")]
    [TestCase(BlendMode.Darken, "#503380")]
    [TestCase(BlendMode.Lighten, "#AA5980")]
    [TestCase(BlendMode.ColourDodge, "#B73BBF")]
    [TestCase(BlendMode.ColourBurn, "#782640")]
    [TestCase(BlendMode.Difference, "#904C3F")]
    public void BlueAlphaOnPinkAlpha(BlendMode blendMode, string expectedHex) => AssertBlend(blueAlpha, pinkAlpha, blendMode, expectedHex, expectedAlpha: 229 / 255.0);

    [TestCase(BlendMode.Normal, "#FF0000")]
    [TestCase(BlendMode.Multiply, "#000000")]
    [TestCase(BlendMode.Screen, "#FF00FF")]
    [TestCase(BlendMode.Overlay, "#0000FF")]
    [TestCase(BlendMode.Darken, "#000000")]
    [TestCase(BlendMode.Lighten, "#FF00FF")]
    [TestCase(BlendMode.ColourDodge, "#0000FF")]
    [TestCase(BlendMode.ColourBurn, "#0000FF")]
    [TestCase(BlendMode.HardLight, "#FF0000")]
    [TestCase(BlendMode.SoftLight, "#0000FF")]
    [TestCase(BlendMode.Difference, "#FF00FF")]
    [TestCase(BlendMode.Exclusion, "#FF00FF")]
    [TestCase(BlendMode.Hue, "#5D0000")]
    [TestCase(BlendMode.Saturation, "#0000FF")]
    [TestCase(BlendMode.Colour, "#5E0000")]
    [TestCase(BlendMode.Luminosity, "#3636FF")]
    public void OutOfGamutRedOnBlue(BlendMode blendMode, string expectedHex) => AssertBlend(redOutOfGamut, blueOutOfGamut, blendMode, expectedHex, expectedAlpha: 1.0);
    
    [TestCase(BlendMode.Normal, "#0000FF")]
    [TestCase(BlendMode.Multiply, "#000000")]
    [TestCase(BlendMode.Screen, "#FF00FF")]
    [TestCase(BlendMode.Overlay, "#FF0000")]
    [TestCase(BlendMode.Darken, "#000000")]
    [TestCase(BlendMode.Lighten, "#FF00FF")]
    [TestCase(BlendMode.ColourDodge, "#FF0000")]
    [TestCase(BlendMode.ColourBurn, "#FF0000")]
    [TestCase(BlendMode.HardLight, "#0000FF")]
    [TestCase(BlendMode.SoftLight, "#FF0000")]
    [TestCase(BlendMode.Difference, "#FF00FF")]
    [TestCase(BlendMode.Exclusion, "#FF00FF")]
    [TestCase(BlendMode.Hue, "#3636FF")]
    [TestCase(BlendMode.Saturation, "#FF0000")]
    [TestCase(BlendMode.Colour, "#3636FF")]
    [TestCase(BlendMode.Luminosity, "#5E0000")]
    public void OutOfGamutBlueOnRed(BlendMode blendMode, string expectedHex) => AssertBlend(blueOutOfGamut, redOutOfGamut, blendMode, expectedHex, expectedAlpha: 1.0);

    [Test]
    public void DifferentConfigSource()
    {
        var rec2020 = new Configuration(RgbConfiguration.Rec2020);
        var pinkRec2020 = pink.ConvertToConfiguration(rec2020);
        var blended = pinkRec2020.Blend(blue, BlendMode.HardLight);

        // blending is performed in source RGB config
        var blueRec2020 = blue.ConvertToConfiguration(rec2020);
        var expected = pinkRec2020.Blend(blueRec2020, BlendMode.HardLight);
        TestUtils.AssertTriplet(blended.Rgb.Triplet, expected.Rgb.Triplet, 1e-15);
    }
    
    [Test]
    public void DifferentConfigBackdrop()
    {
        var rec2020 = new Configuration(RgbConfiguration.Rec2020);
        var blueRec2020 = blue.ConvertToConfiguration(rec2020);
        var blended = pink.Blend(blueRec2020, BlendMode.HardLight);

        // blending is performed in source RGB config
        var expected = pink.Blend(blue, BlendMode.HardLight);
        TestUtils.AssertTriplet(blended.Rgb.Triplet, expected.Rgb.Triplet, 1e-15);
    }
    
    [Test]
    public void AlphaZero()
    {
        var pinkNoAlpha = new Unicolour(ColourSpace.Rgb, pink.Rgb.Tuple, 0);
        var blueNoAlpha = new Unicolour(ColourSpace.Rgb, blue.Rgb.Tuple, 0);
        var blended = pinkNoAlpha.Blend(blueNoAlpha, BlendMode.Normal);
        Assert.That(blended.Rgb.Triplet, Is.EqualTo(new ColourTriplet(0, 0, 0)));
        Assert.That(blended.Alpha.A, Is.EqualTo(0.0));
    }
    
    [Test]
    public void AlphaAboveOne()
    {
        var pinkBadAlpha = new Unicolour(ColourSpace.Rgb, pink.Rgb.Tuple, 1.5);
        var blended = pinkBadAlpha.Blend(blue, BlendMode.HardLight);
        var expected = pink.Blend(blue, BlendMode.HardLight);
        Assert.That(blended.Rgb.Triplet, Is.EqualTo(expected.Rgb.Triplet));
        Assert.That(blended.Alpha.A, Is.EqualTo(expected.Alpha.A));
    }
    
    [Test]
    public void AlphaBelowZero()
    {
        var pinkBadAlpha = new Unicolour(ColourSpace.Rgb, pink.Rgb.Tuple, -0.5);
        var blended = pinkBadAlpha.Blend(blue, BlendMode.HardLight);
        var expected = new Unicolour(ColourSpace.Rgb, pink.Rgb.Tuple, 0).Blend(blue, BlendMode.HardLight);
        Assert.That(blended.Rgb.Triplet, Is.EqualTo(expected.Rgb.Triplet));
        Assert.That(blended.Alpha.A, Is.EqualTo(expected.Alpha.A));
    }
    
    [Test]
    public void NotNumberSource()
    {
        var nan = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN, double.NaN);
        var blended = nan.Blend(blueAlpha, BlendMode.Normal);
        Assert.That(blended.Rgb.Triplet, Is.EqualTo(new ColourTriplet(double.NaN, double.NaN, double.NaN)));
        Assert.That(blended.Alpha.A, Is.EqualTo(blueAlpha.Alpha.A)); // alpha NaN is handled as 0; alpha compositing 0.0 with 0.75 = 0.75
    }
    
    [Test]
    public void NotNumberBackdrop()
    {
        var nan = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN, double.NaN);
        var blended = pinkAlpha.Blend(nan, BlendMode.Normal);
        Assert.That(blended.Rgb.Triplet, Is.EqualTo(new ColourTriplet(double.NaN, double.NaN, double.NaN)));
        Assert.That(blended.Alpha.A, Is.EqualTo(pinkAlpha.Alpha.A)); // alpha NaN is handled as 0; alpha compositing 0.6 with 0.0 = 0.6
    }
    
    [Test]
    public void NotNumberBoth()
    {
        var nan = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN, double.NaN);
        var blended = nan.Blend(nan, BlendMode.Normal);
        Assert.That(blended.Rgb.Triplet, Is.EqualTo(new ColourTriplet(double.NaN, double.NaN, double.NaN)));
        Assert.That(blended.Alpha.A, Is.EqualTo(0.0)); // alpha NaN is handled as 0; alpha compositing 0.0 with 0.0 = 0.0
    }
    
    private static void AssertBlend(Unicolour source, Unicolour backdrop, BlendMode blendMode, string expectedHex, double expectedAlpha)
    {
        var blended = source.Blend(backdrop, blendMode);
        var expected = new Unicolour(expectedHex, expectedAlpha);
        TestUtils.AssertTriplet(blended.Rgb.Triplet, expected.Rgb.Triplet, 0.025);
        Assert.That(blended.Alpha.A, Is.EqualTo(expectedAlpha).Within(0.01));
    }
}