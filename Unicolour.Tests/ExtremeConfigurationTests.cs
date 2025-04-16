using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ExtremeConfigurationTests
{
    [Test, Combinatorial]
    public void Rgb(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double chromaticity, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whitePoint,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double linear)
    {
        var rgbConfig = new RgbConfiguration(
            new Chromaticity(chromaticity, chromaticity), new Chromaticity(chromaticity, chromaticity), new Chromaticity(chromaticity, chromaticity),
            new WhitePoint(whitePoint, whitePoint, whitePoint), _ => linear, _ => linear);
        var config = new Configuration(rgbConfig: rgbConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Rgb, 1.00, 0.08, 0.58));
    }
    
    [Test, Combinatorial]
    public void RgbLinear(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double chromaticity, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whitePoint,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double linear)
    {
        var rgbConfig = new RgbConfiguration(
            new Chromaticity(chromaticity, chromaticity), new Chromaticity(chromaticity, chromaticity), new Chromaticity(chromaticity, chromaticity),
            new WhitePoint(whitePoint, whitePoint, whitePoint), _ => linear, _ => linear);
        var config = new Configuration(rgbConfig: rgbConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.RgbLinear, 1.00, 0.01, 0.29));
    }
    
    [Test, Combinatorial]
    public void Xyz(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whitePoint)
    {
        var xyzConfig = new XyzConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint));
        var config = new Configuration(xyzConfig: xyzConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Xyz, 0.4676, 0.2387, 0.2974));
    }
    
    [Test, Combinatorial]
    public void Xyy(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whitePoint)
    {
        var xyzConfig = new XyzConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint));
        var config = new Configuration(xyzConfig: xyzConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Xyy, 0.4658, 0.2378, 0.2387));
    }
    
    [Test, Combinatorial]
    public void Ypbpr(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double constant, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double range)
    {
        var ybrConfig = new YbrConfiguration(constant, constant, (range, range), (range, range));
        var config = new Configuration(ybrConfig: ybrConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Ypbpr, 0.411, 0.094, 0.420));
    }
    
    [Test, Combinatorial]
    public void Ycbcr(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double constant, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double range)
    {
        var ybrConfig = new YbrConfiguration(constant, constant, (range, range), (range, range));
        var config = new Configuration(ybrConfig: ybrConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Ycbcr, 106, 149, 222));
    }
    
    [Test, Combinatorial]
    public void Ictcp(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whiteLuminance)
    {
        var config = new Configuration(dynamicRange: new(whiteLuminance, whiteLuminance, whiteLuminance, whiteLuminance));
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Ictcp, 0.38, 0.12, 0.19));
    }
    
    [Test, Combinatorial]
    public void Jzazbz(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whiteLuminance)
    {
        var config = new Configuration(dynamicRange: new(whiteLuminance, whiteLuminance, whiteLuminance, whiteLuminance));
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Jzazbz, 0.106, 0.107, 0.005));
    }
    
    [Test, Combinatorial]
    public void Cam02(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whitePoint, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double adaptingLuminance,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double backgroundLuminance)
    {
        var camConfig = new CamConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint), adaptingLuminance, backgroundLuminance, Surround.Average);
        var config = new Configuration(camConfig: camConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Cam02, 62.86, 40.81, -1.18));
    }
    
    [Test, Combinatorial]
    public void Cam16(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whitePoint, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double adaptingLuminance,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double backgroundLuminance)
    {
        var camConfig = new CamConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint), adaptingLuminance, backgroundLuminance, Surround.Average);
        var config = new Configuration(camConfig: camConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Cam16, 62.47, 42.60, -1.36));
    }
}