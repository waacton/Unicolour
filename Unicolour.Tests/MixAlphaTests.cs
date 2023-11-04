namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixAlphaTests
{
    [Test] // https://www.w3.org/TR/css-color-4/#ex-gradient-transition-premultiply
    public void RgbRedToTransparent()
    {
        var red = Unicolour.FromRgb(1, 0, 0, alpha: 1);
        var transparentBlack = Unicolour.FromRgb(0, 0, 0, alpha: 0);
        
        var notPremultipliedAlpha1 = red.MixRgb(transparentBlack, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = transparentBlack.MixRgb(red, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = red.MixRgb(transparentBlack);
        var withPremultipliedAlpha2 = transparentBlack.MixRgb(red);
        
        AssertUtils.AssertTriplet<Rgb>(notPremultipliedAlpha1, new(0.5, 0, 0), AssertUtils.MixTolerance);
        AssertUtils.AssertTriplet<Rgb>(notPremultipliedAlpha2, new(0.5, 0, 0), AssertUtils.MixTolerance);
        AssertUtils.AssertTriplet<Rgb>(withPremultipliedAlpha1, new(1.0, 0, 0), AssertUtils.MixTolerance);
        AssertUtils.AssertTriplet<Rgb>(withPremultipliedAlpha2, new(1.0, 0, 0), AssertUtils.MixTolerance);
    }
    
    [Test] // https://www.w3.org/TR/css-color-4/#ex-gradient-transition-premultiply
    public void RgbTransparentToBlue()
    {
        var transparentBlack = Unicolour.FromRgb(0, 0, 0, alpha: 0);
        var blue = Unicolour.FromRgb(0, 0, 1, alpha: 1);
        
        var notPremultipliedAlpha1 = transparentBlack.MixRgb(blue, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = blue.MixRgb(transparentBlack, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = transparentBlack.MixRgb(blue);
        var withPremultipliedAlpha2 = blue.MixRgb(transparentBlack);
        
        AssertUtils.AssertTriplet<Rgb>(notPremultipliedAlpha1, new(0, 0, 0.5), AssertUtils.MixTolerance);
        AssertUtils.AssertTriplet<Rgb>(notPremultipliedAlpha2, new(0, 0, 0.5), AssertUtils.MixTolerance);
        AssertUtils.AssertTriplet<Rgb>(withPremultipliedAlpha1, new(0, 0, 1.0), AssertUtils.MixTolerance);
        AssertUtils.AssertTriplet<Rgb>(withPremultipliedAlpha2, new(0, 0, 1.0), AssertUtils.MixTolerance);
    }
    
    [Test] // https://www.w3.org/TR/css-color-4/#ex-premultiplied-srgb
    public void RgbPurpleToPink()
    {
        var purple = Unicolour.FromRgb(0.24, 0.12, 0.98, alpha: 0.4);
        var pink = Unicolour.FromRgb(0.62, 0.26, 0.64, alpha: 0.6);
        
        var notPremultipliedAlpha1 = purple.MixRgb(pink, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = pink.MixRgb(purple, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = purple.MixRgb(pink);
        var withPremultipliedAlpha2 = pink.MixRgb(purple);
        
        AssertUtils.AssertTriplet<Rgb>(notPremultipliedAlpha1, new(0.43, 0.19, 0.81), AssertUtils.MixTolerance);
        AssertUtils.AssertTriplet<Rgb>(notPremultipliedAlpha2, new(0.43, 0.19, 0.81), AssertUtils.MixTolerance);
        AssertUtils.AssertTriplet<Rgb>(withPremultipliedAlpha1, new(0.468, 0.204, 0.776), AssertUtils.MixTolerance);
        AssertUtils.AssertTriplet<Rgb>(withPremultipliedAlpha2, new(0.468, 0.204, 0.776), AssertUtils.MixTolerance);
    }
    
    [Test] // https://www.w3.org/TR/css-color-4/#ex-premultiplied-lab
    public void LabYellowToPink()
    {
        var yellowConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D50);
        var pinkConfig = new Configuration(RgbConfiguration.DisplayP3, XyzConfiguration.D50);
        var yellow = Unicolour.FromRgb(yellowConfig, 0.76, 0.62, 0.03, alpha: 0.4);
        var pink = Unicolour.FromRgb(pinkConfig, 0.84, 0.19, 0.72, alpha: 0.6);
        AssertUtils.AssertTriplet<Lab>(yellow, new(66.93, 4.87, 68.62), 0.05);
        AssertUtils.AssertTriplet<Lab>(pink, new(53.50, 82.67, -33.90), 0.05);
        
        // unicolour does not support interpolation between colours of different configurations
        // (because not clear what the result colour's configuration should be)
        // so workaround: create new unicolours with same configurations with the LAB values

        yellow = Unicolour.FromLab(yellow.Lab.Triplet.Tuple, yellow.Alpha.A);
        pink = Unicolour.FromLab(pink.Lab.Triplet.Tuple, pink.Alpha.A);
        var notPremultipliedAlpha1 = yellow.MixLab(pink, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = pink.MixLab(yellow, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = yellow.MixLab(pink);
        var withPremultipliedAlpha2 = pink.MixLab(yellow);
        
        AssertUtils.AssertTriplet<Lab>(notPremultipliedAlpha1, new(60.22, 43.77, 17.36), 0.05);
        AssertUtils.AssertTriplet<Lab>(notPremultipliedAlpha2, new(60.22, 43.77, 17.36), 0.05);
        AssertUtils.AssertTriplet<Lab>(withPremultipliedAlpha1, new(58.87, 51.55, 7.11), 0.05);
        AssertUtils.AssertTriplet<Lab>(withPremultipliedAlpha2, new(58.87, 51.55, 7.11), 0.05);
    }
    
    [Test] // https://www.w3.org/TR/css-color-4/#ex-premultiplied-lch
    public void LchabYellowToPink()
    {
        var yellowConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D50);
        var pinkConfig = new Configuration(RgbConfiguration.DisplayP3, XyzConfiguration.D50);
        var yellow = Unicolour.FromRgb(yellowConfig, 0.76, 0.62, 0.03, alpha: 0.4);
        var pink = Unicolour.FromRgb(pinkConfig, 0.84, 0.19, 0.72, alpha: 0.6);
        AssertUtils.AssertTriplet<Lchab>(yellow, new(66.93, 68.79, 85.94), 0.05);
        AssertUtils.AssertTriplet<Lchab>(pink, new(53.50, 89.35, 337.7), 0.05);
        
        // unicolour does not support interpolation between colours of different configurations
        // (because not clear what the result colour's configuration should be)
        // so workaround: create new unicolours with same configurations with the LCHAB values

        yellow = Unicolour.FromLchab(yellow.Lchab.Triplet.Tuple, yellow.Alpha.A);
        pink = Unicolour.FromLchab(pink.Lchab.Triplet.Tuple, pink.Alpha.A);
        var notPremultipliedAlpha1 = yellow.MixLchab(pink, premultiplyAlpha: false);
        var notPremultipliedAlpha2 = pink.MixLchab(yellow, premultiplyAlpha: false);
        var withPremultipliedAlpha1 = yellow.MixLchab(pink);
        var withPremultipliedAlpha2 = pink.MixLchab(yellow);
        
        AssertUtils.AssertTriplet<Lchab>(notPremultipliedAlpha1, new(60.22, 79.07, 31.82), 0.05);
        AssertUtils.AssertTriplet<Lchab>(notPremultipliedAlpha2, new(60.22, 79.07, 31.82), 0.05);
        AssertUtils.AssertTriplet<Lchab>(withPremultipliedAlpha1, new(58.87, 81.13, 31.82), 0.05);
        AssertUtils.AssertTriplet<Lchab>(withPremultipliedAlpha2, new(58.87, 81.13, 31.82), 0.05);
    }
}