namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

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
            new WhitePoint(whitePoint, whitePoint, whitePoint), 
            _ => linear, _ => linear);
        var config = new Configuration(rgbConfiguration: rgbConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Rgb, 1.00, 0.08, 0.58));
    }
    
    [Test, Combinatorial]
    public void Xyz(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whitePoint)
    {
        var xyzConfig = new XyzConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint));
        var config = new Configuration(xyzConfiguration: xyzConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Xyz, 0.4676, 0.2387, 0.2974));
    }
    
    [Test, Combinatorial]
    public void Ictcp(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double scalar)
    {
        var config = new Configuration(ictcpScalar: scalar);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Ictcp, 0.38, 0.12, 0.19));
    }
    
    [Test, Combinatorial]
    public void Jzazbz(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double scalar)
    {
        var config = new Configuration(jzazbzScalar: scalar);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Jzazbz, 0.106, 0.107, 0.005));
    }
    
    [Test, Combinatorial]
    public void Cam02(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whitePoint, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double adaptingLuminance,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double backgroundLuminance)
    {
        var camConfig = new CamConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint), adaptingLuminance, backgroundLuminance, Surround.Average);
        var config = new Configuration(camConfiguration: camConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Cam02, 62.86, 40.81, -1.18));
    }
    
    [Test, Combinatorial]
    public void Cam16(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double whitePoint, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double adaptingLuminance,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double backgroundLuminance)
    {
        var camConfig = new CamConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint), adaptingLuminance, backgroundLuminance, Surround.Average);
        var config = new Configuration(camConfiguration: camConfig);
        TestUtils.AssertNoPropertyError(new Unicolour(config, ColourSpace.Cam16, 62.47, 42.60, -1.36));
    }
}