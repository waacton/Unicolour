namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class ExtremeValuesTests
{
    [Test, Combinatorial]
    public static void Rgb(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromRgb(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void RgbConfiguration(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double chromaticity, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double whitePoint,
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double linear)
    {
        var rgbConfig = new RgbConfiguration(
            new Chromaticity(chromaticity, chromaticity), new Chromaticity(chromaticity, chromaticity), new Chromaticity(chromaticity, chromaticity),
            new WhitePoint(whitePoint, whitePoint, whitePoint), 
            _ => linear, _ => linear);
        var config = new Configuration(rgbConfiguration: rgbConfig);
        AssertUtils.AssertNoPropertyError(Unicolour.FromRgb(config, 1.00, 0.08, 0.58));
    }
    
    [Test, Combinatorial]
    public static void Hsb(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromHsb(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Hsl(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromHsl(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Hwb(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromHwb(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Xyz(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromXyz(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void XyzConfiguration(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double whitePoint)
    {
        var xyzConfig = new XyzConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint));
        var config = new Configuration(xyzConfiguration: xyzConfig);
        AssertUtils.AssertNoPropertyError(Unicolour.FromXyz(config, 0.4676, 0.2387, 0.2974));
    }
    
    [Test, Combinatorial]
    public static void Xyy(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromXyy(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Lab(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromLab(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Lchab(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromLchab(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Luv(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromLuv(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Lchuv(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromLchuv(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Hsluv(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromHsluv(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Hpluv(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromHpluv(first, second, third));
    }

    [Test, Combinatorial]
    public static void Ictcp(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromIctcp(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void IctcpConfiguration(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double scalar)
    {
        var config = new Configuration(ictcpScalar: scalar);
        AssertUtils.AssertNoPropertyError(Unicolour.FromIctcp(config, 0.38, 0.12, 0.19));
    }
    
    [Test, Combinatorial]
    public static void Jzazbz(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromJzazbz(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void JzazbzConfiguration(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double scalar)
    {
        var config = new Configuration(jzazbzScalar: scalar);
        AssertUtils.AssertNoPropertyError(Unicolour.FromJzazbz(config, 0.106, 0.107, 0.005));
    }
    
    [Test, Combinatorial]
    public static void Jzczhz(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromJzczhz(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Oklab(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromOklab(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Oklch(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromOklch(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Cam02(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromCam02(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Cam16(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromCam16(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Cam02Configuration(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double whitePoint, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double adaptingLuminance,
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double backgroundLuminance)
    {
        var camConfig = new CamConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint), adaptingLuminance, backgroundLuminance, Surround.Average);
        var config = new Configuration(camConfiguration: camConfig);
        AssertUtils.AssertNoPropertyError(Unicolour.FromCam02(config, 62.86, 40.81, -1.18));
    }
    
    [Test, Combinatorial]
    public static void Cam16Configuration(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double whitePoint, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double adaptingLuminance,
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double backgroundLuminance)
    {
        var camConfig = new CamConfiguration(new WhitePoint(whitePoint, whitePoint, whitePoint), adaptingLuminance, backgroundLuminance, Surround.Average);
        var config = new Configuration(camConfiguration: camConfig);
        AssertUtils.AssertNoPropertyError(Unicolour.FromCam16(config, 62.47, 42.60, -1.36));
    }
}