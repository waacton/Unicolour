using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownIccTests
{
    /*
     * I don't know why CSS Color 5 example values don't match what the ICC tools themselves generate
     * despite using the actual, exact ICC D50 XYZ (i.e. rounded to 4 decimal places)
     * even using the D50 value from color.js, a lot of tolerance is still required
     */
    private const double Tolerance = 0.02;
    
    [Test] // https://www.w3.org/TR/css-color-5/#ex-fogra
    public void Fogra39Rose()
    {
        var cmyk = new Channels(0.0, 0.7, 0.2, 0.0);
        var colour = new Unicolour(GetConfig(IccFile.Fogra39), cmyk);
        TestUtils.AssertTriplet<Lab>(colour, new(63.673303, 51.576902, 5.811058), Tolerance);
    }
    
    [Test] // https://www.w3.org/TR/css-color-5/#ex-swop5v2
    public void Swop2006Rose()
    {
        var cmyk = new Channels(0.0, 0.7, 0.2, 0.0);
        var colour = new Unicolour(GetConfig(IccFile.Swop2006), cmyk);
        TestUtils.AssertTriplet<Lab>(colour, new(64.965217, 52.119710, 5.406966), Tolerance);
    }
    
    [Test] // https://www.w3.org/TR/css-color-5/#ex-device-cmyk-naive
    public void NoProfileFirebrick()
    {
        var cmyk = new Channels(0.0, 0.81, 0.81, 0.3);
        var colour = new Unicolour(cmyk);
        TestUtils.AssertTriplet<Rgb255>(colour, new(178, 34, 34), 0);
    }
    
    [Test] // https://www.w3.org/TR/css-color-5/#ex-device-cmyk-colprof
    public void Fogra39Firebrick()
    {
        var cmyk = new Channels(0.0, 0.81, 0.81, 0.3);
        var colour = new Unicolour(GetConfig(IccFile.Fogra39), cmyk);
        TestUtils.AssertTriplet<Lab>(colour, new(45.060, 45.477, 35.459), Tolerance);
    }
    
    // NOTE: these values don't match
    // but Unicolour is heavily tested against ICC reference implementation which takes precedence over draft CSS examples
    // [Test] // https://www.w3.org/TR/css-color-5/#ex-fogra39-fallback-mq
    // public void Fogra39Green()
    // {
    //     var config = new Configuration(
    //         rgbConfiguration: RgbConfiguration.DisplayP3,
    //         xyzConfiguration: Profile.XyzD50,
    //         iccConfiguration: new IccConfiguration(IccFile.Fogra39.GetProfile(), Intent.RelativeColorimetric)
    //     );
    //     
    //     var cmyk = new Channels(0.9, 0.0, 9.0, 0.0);
    //     var colour = new Unicolour(config, cmyk);
    //     TestUtils.AssertTriplet<Lab>(colour, new(56.596645, -58.995875, 28.072154), Tolerance);
    // }
    
    // NOTE: these values don't match well enough to be good tests (perhaps non-beta version of profile produces different results?)
    // but Unicolour is heavily tested against ICC reference implementation which takes precedence over draft CSS examples
    // [Test] // https://www.w3.org/TR/css-color-5/#ex-fogra55beta-7color
    // public void Fogra55DarkSkin()
    // {
    //     var config = new Configuration(
    //         xyzConfiguration: Profile.XyzD50,
    //         iccConfiguration: new IccConfiguration(IccFile.Fogra55.GetProfile(), Intent.RelativeColorimetric)
    //     );
    //     
    //     var cmyk = new Channels(0.183596, 0.464444, 0.461729, 0.612490, 0.156903, 0.000000, 0.000000);
    //     var colour = new Unicolour(config, cmyk);
    //     TestUtils.AssertTriplet<Rgb>(colour, new(0.458702, 0.320071, 0.263813), Tolerance);
    // }
    //
    // [Test] // https://www.w3.org/TR/css-color-5/#ex-fogra55beta-7color
    // public void Fogra55LightSkin()
    // {
    //     var config = new Configuration(
    //         xyzConfiguration: Profile.XyzD50,
    //         iccConfiguration: new IccConfiguration(IccFile.Fogra55.GetProfile(), Intent.RelativeColorimetric)
    //     );
    //     
    //     var cmyk = new Channels(0.070804, 0.334971, 0.321802, 0.215606, 0.103107, 0.000000, 0.000000);
    //     var colour = new Unicolour(config, cmyk);
    //     TestUtils.AssertTriplet<Rgb>(colour, new(0.780170, 0.581957, 0.507737), Tolerance);
    // }
    //
    // [Test] // https://www.w3.org/TR/css-color-5/#ex-fogra55beta-7color
    // public void Fogra55BlueSky()
    // {
    //     var config = new Configuration(
    //         xyzConfiguration: Profile.XyzD50,
    //         iccConfiguration: new IccConfiguration(IccFile.Fogra55.GetProfile(), Intent.RelativeColorimetric)
    //     );
    //     
    //     var cmyk = new Channels(0.572088, 0.229346, 0.081708, 0.282044, 0.000000, 0.000000, 0.168260);
    //     var colour = new Unicolour(config, cmyk);
    //     TestUtils.AssertTriplet<Rgb>(colour, new(0.358614, 0.480665, 0.616556), Tolerance);
    // }
    //
    // [Test] // https://www.w3.org/TR/css-color-5/#ex-fogra55beta-7color
    // public void Fogra55Foliage()
    // {
    //     var config = new Configuration(
    //         xyzConfiguration: Profile.XyzD50,
    //         iccConfiguration: new IccConfiguration(IccFile.Fogra55.GetProfile(), Intent.RelativeColorimetric)
    //     );
    //     
    //     var cmyk = new Channels(0.314566, 0.145687, 0.661941, 0.582879, 0.000000, 0.234362, 0.000000);
    //     var colour = new Unicolour(config, cmyk);
    //     TestUtils.AssertTriplet<Rgb>(colour, new(0.349582, 0.423446, 0.254209), Tolerance);
    // }
    //
    // [Test] // https://www.w3.org/TR/css-color-5/#ex-fogra55beta-7color
    // public void Fogra55BlueFlower()
    // {
    //     var config = new Configuration(
    //         xyzConfiguration: Profile.XyzD50,
    //         iccConfiguration: new IccConfiguration(IccFile.Fogra55.GetProfile(), Intent.RelativeColorimetric)
    //     );
    //     
    //     var cmyk = new Channels(0.375515, 0.259934, 0.034849, 0.107161, 0.000000, 0.000000, 0.308200);
    //     var colour = new Unicolour(config, cmyk);
    //     TestUtils.AssertTriplet<Rgb>(colour, new(0.512952, 0.504131, 0.689186), Tolerance);
    // }
    //
    // [Test] // https://www.w3.org/TR/css-color-5/#ex-fogra55beta-7color
    // public void Fogra55BluishGreen()
    // {
    //     var config = new Configuration(
    //         xyzConfiguration: Profile.XyzD50,
    //         iccConfiguration: new IccConfiguration(IccFile.Fogra55.GetProfile(), Intent.RelativeColorimetric)
    //     );
    //     
    //     var cmyk = new Channels(0.397575, 0.010047, 0.223682, 0.031140, 0.000000, 0.317066, 0.000000);
    //     var colour = new Unicolour(config, cmyk);
    //     TestUtils.AssertTriplet<Rgb>(colour, new(0.368792, 0.743685, 0.674749), Tolerance);
    // }
    
    private static Configuration GetConfig(IccFile iccFile)
    {
        // CSS ICC profiles default to relative colorimetric
        return new Configuration(
            xyzConfig: Transform.XyzD50,
            iccConfig: new IccConfiguration(iccFile.GetProfile(), Intent.RelativeColorimetric)
        );
    }
}