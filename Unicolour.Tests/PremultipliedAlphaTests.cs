namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class PremultipliedAlphaTests
{
    [Test] // https://www.w3.org/TR/css-color-4/#ex-gradient-transition-premultiply
    public void RgbRedToTransparent()
    {
        var red = new Unicolour(ColourSpace.Rgb, 1, 0, 0, alpha: 1);
        var transparentBlack = new Unicolour(ColourSpace.Rgb, 0, 0, 0, alpha: 0);
        
        var notPremultipliedAlpha1 = red.Mix(ColourSpace.Rgb, transparentBlack, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = transparentBlack.Mix(ColourSpace.Rgb, red, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = red.Mix(ColourSpace.Rgb, transparentBlack);
        var withPremultipliedAlpha2 = transparentBlack.Mix(ColourSpace.Rgb, red);
        
        TestUtils.AssertTriplet<Rgb>(notPremultipliedAlpha1, new(0.5, 0, 0), TestUtils.MixTolerance);
        TestUtils.AssertTriplet<Rgb>(notPremultipliedAlpha2, new(0.5, 0, 0), TestUtils.MixTolerance);
        TestUtils.AssertTriplet<Rgb>(withPremultipliedAlpha1, new(1.0, 0, 0), TestUtils.MixTolerance);
        TestUtils.AssertTriplet<Rgb>(withPremultipliedAlpha2, new(1.0, 0, 0), TestUtils.MixTolerance);
    }
    
    [Test] // https://www.w3.org/TR/css-color-4/#ex-gradient-transition-premultiply
    public void RgbTransparentToBlue()
    {
        var transparentBlack = new Unicolour(ColourSpace.Rgb, 0, 0, 0, alpha: 0);
        var blue = new Unicolour(ColourSpace.Rgb, 0, 0, 1, alpha: 1);
        
        var notPremultipliedAlpha1 = transparentBlack.Mix(ColourSpace.Rgb, blue, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = blue.Mix(ColourSpace.Rgb, transparentBlack, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = transparentBlack.Mix(ColourSpace.Rgb, blue);
        var withPremultipliedAlpha2 = blue.Mix(ColourSpace.Rgb, transparentBlack);
        
        TestUtils.AssertTriplet<Rgb>(notPremultipliedAlpha1, new(0, 0, 0.5), TestUtils.MixTolerance);
        TestUtils.AssertTriplet<Rgb>(notPremultipliedAlpha2, new(0, 0, 0.5), TestUtils.MixTolerance);
        TestUtils.AssertTriplet<Rgb>(withPremultipliedAlpha1, new(0, 0, 1.0), TestUtils.MixTolerance);
        TestUtils.AssertTriplet<Rgb>(withPremultipliedAlpha2, new(0, 0, 1.0), TestUtils.MixTolerance);
    }
    
    [Test] // https://www.w3.org/TR/css-color-4/#ex-premultiplied-srgb
    public void RgbPurpleToPink()
    {
        var purple = new Unicolour(ColourSpace.Rgb, 0.24, 0.12, 0.98, alpha: 0.4);
        var pink = new Unicolour(ColourSpace.Rgb, 0.62, 0.26, 0.64, alpha: 0.6);
        
        var notPremultipliedAlpha1 = purple.Mix(ColourSpace.Rgb, pink, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = pink.Mix(ColourSpace.Rgb, purple, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = purple.Mix(ColourSpace.Rgb, pink);
        var withPremultipliedAlpha2 = pink.Mix(ColourSpace.Rgb, purple);
        
        TestUtils.AssertTriplet<Rgb>(notPremultipliedAlpha1, new(0.43, 0.19, 0.81), TestUtils.MixTolerance);
        TestUtils.AssertTriplet<Rgb>(notPremultipliedAlpha2, new(0.43, 0.19, 0.81), TestUtils.MixTolerance);
        TestUtils.AssertTriplet<Rgb>(withPremultipliedAlpha1, new(0.468, 0.204, 0.776), TestUtils.MixTolerance);
        TestUtils.AssertTriplet<Rgb>(withPremultipliedAlpha2, new(0.468, 0.204, 0.776), TestUtils.MixTolerance);
    }
    
    [Test] // https://www.w3.org/TR/css-color-4/#ex-premultiplied-lab
    public void LabYellowToPink()
    {
        var yellowConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D50);
        var pinkConfig = new Configuration(RgbConfiguration.DisplayP3, XyzConfiguration.D50);
        var yellow = new Unicolour(ColourSpace.Rgb, yellowConfig, 0.76, 0.62, 0.03, alpha: 0.4);
        var pink = new Unicolour(ColourSpace.Rgb, pinkConfig, 0.84, 0.19, 0.72, alpha: 0.6);
        TestUtils.AssertTriplet<Lab>(yellow, new(66.93, 4.87, 68.62), 0.05);
        TestUtils.AssertTriplet<Lab>(pink, new(53.50, 82.67, -33.90), 0.05);
        
        // unicolour does not support interpolation between colours of different configurations
        // (because not clear what the result colour's configuration should be)
        // so workaround: create new unicolours with same configurations with the LAB values

        yellow = new Unicolour(ColourSpace.Lab, yellow.Lab.Triplet.Tuple, yellow.Alpha.A);
        pink = new Unicolour(ColourSpace.Lab, pink.Lab.Triplet.Tuple, pink.Alpha.A);
        var notPremultipliedAlpha1 = yellow.Mix(ColourSpace.Lab, pink, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = pink.Mix(ColourSpace.Lab, yellow, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = yellow.Mix(ColourSpace.Lab, pink);
        var withPremultipliedAlpha2 = pink.Mix(ColourSpace.Lab, yellow);
        
        TestUtils.AssertTriplet<Lab>(notPremultipliedAlpha1, new(60.22, 43.77, 17.36), 0.05);
        TestUtils.AssertTriplet<Lab>(notPremultipliedAlpha2, new(60.22, 43.77, 17.36), 0.05);
        TestUtils.AssertTriplet<Lab>(withPremultipliedAlpha1, new(58.87, 51.55, 7.11), 0.05);
        TestUtils.AssertTriplet<Lab>(withPremultipliedAlpha2, new(58.87, 51.55, 7.11), 0.05);
    }
    
    [Test] // https://www.w3.org/TR/css-color-4/#ex-premultiplied-lch
    public void LchabYellowToPink()
    {
        var yellowConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D50);
        var pinkConfig = new Configuration(RgbConfiguration.DisplayP3, XyzConfiguration.D50);
        var yellow = new Unicolour(ColourSpace.Rgb, yellowConfig, 0.76, 0.62, 0.03, alpha: 0.4);
        var pink = new Unicolour(ColourSpace.Rgb, pinkConfig, 0.84, 0.19, 0.72, alpha: 0.6);
        TestUtils.AssertTriplet<Lchab>(yellow, new(66.93, 68.79, 85.94), 0.05);
        TestUtils.AssertTriplet<Lchab>(pink, new(53.50, 89.35, 337.7), 0.05);
        
        // unicolour does not support interpolation between colours of different configurations
        // (because not clear what the result colour's configuration should be)
        // so workaround: create new unicolours with same configurations with the LCHAB values

        yellow = new Unicolour(ColourSpace.Lchab, yellow.Lchab.Triplet.Tuple, yellow.Alpha.A);
        pink = new Unicolour(ColourSpace.Lchab, pink.Lchab.Triplet.Tuple, pink.Alpha.A);
        var notPremultipliedAlpha1 = yellow.Mix(ColourSpace.Lchab, pink, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = pink.Mix(ColourSpace.Lchab, yellow, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = yellow.Mix(ColourSpace.Lchab, pink);
        var withPremultipliedAlpha2 = pink.Mix(ColourSpace.Lchab, yellow);
        
        TestUtils.AssertTriplet<Lchab>(notPremultipliedAlpha1, new(60.22, 79.07, 31.82), 0.05);
        TestUtils.AssertTriplet<Lchab>(notPremultipliedAlpha2, new(60.22, 79.07, 31.82), 0.05);
        TestUtils.AssertTriplet<Lchab>(withPremultipliedAlpha1, new(58.87, 81.13, 31.82), 0.05);
        TestUtils.AssertTriplet<Lchab>(withPremultipliedAlpha2, new(58.87, 81.13, 31.82), 0.05);
    }
}