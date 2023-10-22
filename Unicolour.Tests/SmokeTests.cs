namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class SmokeTests
{
    [TestCase("000000")]
    [TestCase("FFFFFF")]
    [TestCase("667788")]
    public void UnicolourHex(string hex) => AssertHex(hex);

    [TestCase(0, 0, 0)]
    [TestCase(255, 255, 255)]
    [TestCase(124, 125, 126)]
    public void UnicolourRgb255(int r, int g, int b) => AssertRgb255(r, g, b);

    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void UnicolourRgb(double r, double g, double b) => AssertInit(r, g, b, Unicolour.FromRgb, Unicolour.FromRgb, Unicolour.FromRgb, Unicolour.FromRgb);

    [TestCase(0, 0, 0)]
    [TestCase(360, 1, 1)]
    [TestCase(180, 0.4, 0.6)]
    public void UnicolourHsb(double h, double s, double b) => AssertInit(h, s, b, Unicolour.FromHsb, Unicolour.FromHsb, Unicolour.FromHsb, Unicolour.FromHsb);

    [TestCase(0, 0, 0)]
    [TestCase(360, 1, 1)]
    [TestCase(180, 0.4, 0.6)]
    public void UnicolourHsl(double h, double s, double l) => AssertInit(h, s, l, Unicolour.FromHsl, Unicolour.FromHsl, Unicolour.FromHsl, Unicolour.FromHsl);
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 1, 1)]
    [TestCase(180, 0.4, 0.6)]
    public void UnicolourHwb(double h, double w, double b) => AssertInit(h, w, b, Unicolour.FromHwb, Unicolour.FromHwb, Unicolour.FromHwb, Unicolour.FromHwb);

    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void UnicolourXyz(double x, double y, double z) => AssertInit(x, y, z, Unicolour.FromXyz, Unicolour.FromXyz, Unicolour.FromXyz, Unicolour.FromXyz);
    
    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void UnicolourXyy(double x, double y, double upperY) => AssertInit(x, y, upperY, Unicolour.FromXyy, Unicolour.FromXyy, Unicolour.FromXyy, Unicolour.FromXyy);
    
    [TestCase(0, -128, -128)]
    [TestCase(100, 128, 128)]
    [TestCase(50, -1, 1)]
    public void UnicolourLab(double l, double a, double b) => AssertInit(l, a, b, Unicolour.FromLab, Unicolour.FromLab, Unicolour.FromLab, Unicolour.FromLab);
    
    [TestCase(0, 0, 0)]
    [TestCase(100, 230, 360)]
    [TestCase(50, 115, 180)]
    public void UnicolourLchab(double l, double c, double h) => AssertInit(l, c, h, Unicolour.FromLchab, Unicolour.FromLchab, Unicolour.FromLchab, Unicolour.FromLchab);

    [TestCase(0, -100, -100)]
    [TestCase(100, 100, 100)]
    [TestCase(50, -1, 1)]
    public void UnicolourLuv(double l, double u, double v) => AssertInit(l, u, v, Unicolour.FromLuv, Unicolour.FromLuv, Unicolour.FromLuv, Unicolour.FromLuv);
    
    [TestCase(0, 0, 0)]
    [TestCase(100, 230, 360)]
    [TestCase(50, 115, 180)]
    public void UnicolourLchuv(double l, double c, double h) => AssertInit(l, c, h, Unicolour.FromLchuv, Unicolour.FromLchuv, Unicolour.FromLchuv, Unicolour.FromLchuv);
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 100, 100)]
    [TestCase(180, 50, 50)]
    public void UnicolourHsluv(double h, double s, double l) => AssertInit(h, s, l, Unicolour.FromHsluv, Unicolour.FromHsluv, Unicolour.FromHsluv, Unicolour.FromHsluv);
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 100, 100)]
    [TestCase(180, 50, 50)]
    public void UnicolourHpluv(double h, double s, double l) => AssertInit(h, s, l, Unicolour.FromHpluv, Unicolour.FromHpluv, Unicolour.FromHpluv, Unicolour.FromHpluv);

    [TestCase(0, -0.5, -0.5)]
    [TestCase(1, 0.5, 0.5)]
    [TestCase(0.5, -0.01, 0.01)]
    public void UnicolourIctcp(double i, double ct, double cp) => AssertInit(i, ct, cp, Unicolour.FromIctcp, Unicolour.FromIctcp, Unicolour.FromIctcp, Unicolour.FromIctcp);
    
    [TestCase(0, -0.10, -0.16)]
    [TestCase(0.17, 0.11, 0.12)]
    [TestCase(0.085, -0.0001, 0.0001)]
    public void UnicolourJzazbz(double j, double a, double b) => AssertInit(j, a, b, Unicolour.FromJzazbz, Unicolour.FromJzazbz, Unicolour.FromJzazbz, Unicolour.FromJzazbz);
    
    [TestCase(0, 0, 0)]
    [TestCase(0.17, 0.16, 360)]
    [TestCase(0.085, 0.08, 180)]
    public void UnicolourJzczhz(double j, double c, double h) => AssertInit(j, c, h, Unicolour.FromJzczhz, Unicolour.FromJzczhz, Unicolour.FromJzczhz, Unicolour.FromJzczhz);
    
    [TestCase(0, -0.5, -0.5)]
    [TestCase(1, 0.5, 0.5)]
    [TestCase(0.5, -0.001, 0.001)]
    public void UnicolourOklab(double l, double a, double b) => AssertInit(l, a, b, Unicolour.FromOklab, Unicolour.FromOklab, Unicolour.FromOklab, Unicolour.FromOklab);
    
    [TestCase(0, 0, 0)]
    [TestCase(1, 0.5, 360)]
    [TestCase(0.5, 0.25, 180)]
    public void UnicolourOklch(double l, double c, double h) => AssertInit(l, c, h, Unicolour.FromOklch, Unicolour.FromOklch, Unicolour.FromOklch, Unicolour.FromOklch);

    [TestCase(0, -50, -50)]
    [TestCase(100, 50, 50)]
    [TestCase(50, -1, 1)]
    public void UnicolourCam02(double j, double a, double b) => AssertInit(j, a, b, Unicolour.FromCam02, Unicolour.FromCam02, Unicolour.FromCam02, Unicolour.FromCam02);
    
    [TestCase(0, -50, -50)]
    [TestCase(100, 50, 50)]
    [TestCase(50, -1, 1)]
    public void UnicolourCam16(double j, double a, double b) => AssertInit(j, a, b, Unicolour.FromCam16, Unicolour.FromCam16, Unicolour.FromCam16, Unicolour.FromCam16);
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 120, 100)]
    [TestCase(180, 60, 50)]
    public void UnicolourHct(double h, double c, double t) => AssertInit(h, c, t, Unicolour.FromHct, Unicolour.FromHct, Unicolour.FromHct, Unicolour.FromHct);

    private delegate Unicolour FromValues(double first, double second, double third, double alpha = 1.0);
    private delegate Unicolour FromValuesWithConfig(Configuration config, double first, double second, double third, double alpha = 1.0);
    private delegate Unicolour FromTuple((double first, double second, double third) tuple, double alpha = 1.0);
    private delegate Unicolour FromTupleWithConfig(Configuration config, (double first, double second, double third) tuple, double alpha = 1.0);
    
    private static void AssertInit(double first, double second, double third, 
        FromValues fromValues, FromValuesWithConfig fromValuesWithConfig,
        FromTuple fromTuple, FromTupleWithConfig fromTupleWithConfig)
    {
        var tuple = (first, second, third);
        var expected = fromValues(first, second, third);
        AssertNoError(expected, fromValues(first, second, third));
        AssertNoError(expected, fromValues(first, second, third, 1.0));
        AssertNoError(expected, fromValuesWithConfig(Configuration.Default, first, second, third));
        AssertNoError(expected, fromValuesWithConfig(Configuration.Default, first, second, third, 1.0));
        AssertNoError(expected, fromTuple(tuple));
        AssertNoError(expected, fromTuple(tuple, 1.0));
        AssertNoError(expected, fromTupleWithConfig(Configuration.Default, tuple));
        AssertNoError(expected, fromTupleWithConfig(Configuration.Default, tuple, 1.0));
    }
    
    // dedicated method due to using ints instead of doubles
    private static void AssertRgb255(int first, int second, int third)
    {
        var tuple = (first, second, third);
        var expected = Unicolour.FromRgb255(first, second, third);
        AssertNoError(expected, Unicolour.FromRgb255(first, second, third));
        AssertNoError(expected, Unicolour.FromRgb255(first, second, third, 255));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, first, second, third));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, first, second, third, 255));
        AssertNoError(expected, Unicolour.FromRgb255(tuple));
        AssertNoError(expected, Unicolour.FromRgb255(tuple, 255));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, tuple));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, tuple, 255));
    }
    
    private static void AssertHex(string hex)
    {
        var withHash = $"#{hex}";
        var withHashAndAlpha = $"#{hex}FF";
        var withoutHash = $"{hex}";
        var withoutHashAndAlpha = $"{hex}FF";
        
        var expected = Unicolour.FromHex(withHash);
        AssertNoError(expected, Unicolour.FromHex(withHash));
        AssertNoError(expected, Unicolour.FromHex(withHashAndAlpha));
        AssertNoError(expected, Unicolour.FromHex(withoutHash));
        AssertNoError(expected, Unicolour.FromHex(withoutHashAndAlpha));
    }

    private static void AssertNoError(Unicolour expected, Unicolour unicolour)
    {
        AssertUtils.AssertNoPropertyError(unicolour);
        Assert.That(unicolour, Is.EqualTo(expected));
    }
}

