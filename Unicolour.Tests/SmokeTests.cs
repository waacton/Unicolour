namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class SmokeTests
{
    [TestCase("000000")]
    [TestCase("FFFFFF")]
    [TestCase("798081")]
    [TestCase("#000000")]
    [TestCase("#FFFFFF")]
    [TestCase("#798081")]
    public void Hex(string hex) => AssertHex(hex);
    
    [TestCase("00000000")]
    [TestCase("FFFFFFFF")]
    [TestCase("79808180")]
    [TestCase("#00000000")]
    [TestCase("#FFFFFFFF")]
    [TestCase("#79808180")]
    public void HexAlpha(string hex) => AssertHex(hex);

    [TestCase(0, 0, 0)]
    [TestCase(255, 255, 255)]
    [TestCase(127, 128, 129)]
    public void Rgb255(int r, int g, int b) => AssertRgb255(r, g, b);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(255, 255, 255, 255)]
    [TestCase(127, 128, 129, 128)]
    public void Rgb255Alpha(int r, int g, int b, int a) => AssertRgb255(r, g, b, a);

    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void Rgb(double r, double g, double b) => AssertInit(r, g, b, Unicolour.FromRgb, Unicolour.FromRgb, Unicolour.FromRgb, Unicolour.FromRgb);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6, 0.5)]
    public void RgbAlpha(double r, double g, double b, double alpha) => AssertInit(r, g, b, alpha, Unicolour.FromRgb, Unicolour.FromRgb, Unicolour.FromRgb, Unicolour.FromRgb);

    [TestCase(0, 0, 0)]
    [TestCase(360, 1, 1)]
    [TestCase(180, 0.4, 0.6)]
    public void Hsb(double h, double s, double b) => AssertInit(h, s, b, Unicolour.FromHsb, Unicolour.FromHsb, Unicolour.FromHsb, Unicolour.FromHsb);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 1, 1, 1)]
    [TestCase(180, 0.4, 0.6, 0.5)]
    public void HsbAlpha(double h, double s, double b, double alpha) => AssertInit(h, s, b, alpha, Unicolour.FromHsb, Unicolour.FromHsb, Unicolour.FromHsb, Unicolour.FromHsb);

    [TestCase(0, 0, 0)]
    [TestCase(360, 1, 1)]
    [TestCase(180, 0.4, 0.6)]
    public void Hsl(double h, double s, double l) => AssertInit(h, s, l, Unicolour.FromHsl, Unicolour.FromHsl, Unicolour.FromHsl, Unicolour.FromHsl);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 1, 1, 1)]
    [TestCase(180, 0.4, 0.6, 0.5)]
    public void HslAlpha(double h, double s, double l, double alpha) => AssertInit(h, s, l, alpha, Unicolour.FromHsl, Unicolour.FromHsl, Unicolour.FromHsl, Unicolour.FromHsl);
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 1, 1)]
    [TestCase(180, 0.4, 0.6)]
    public void Hwb(double h, double w, double b) => AssertInit(h, w, b, Unicolour.FromHwb, Unicolour.FromHwb, Unicolour.FromHwb, Unicolour.FromHwb);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 1, 1, 1)]
    [TestCase(180, 0.4, 0.6, 0.5)]
    public void HwbAlpha(double h, double w, double b, double alpha) => AssertInit(h, w, b, alpha, Unicolour.FromHwb, Unicolour.FromHwb, Unicolour.FromHwb, Unicolour.FromHwb);

    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void Xyz(double x, double y, double z) => AssertInit(x, y, z, Unicolour.FromXyz, Unicolour.FromXyz, Unicolour.FromXyz, Unicolour.FromXyz);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6, 0.5)]
    public void XyzAlpha(double x, double y, double z, double alpha) => AssertInit(x, y, z, alpha, Unicolour.FromXyz, Unicolour.FromXyz, Unicolour.FromXyz, Unicolour.FromXyz);
    
    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void Xyy(double x, double y, double upperY) => AssertInit(x, y, upperY, Unicolour.FromXyy, Unicolour.FromXyy, Unicolour.FromXyy, Unicolour.FromXyy);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6, 0.5)]
    public void XyyAlpha(double x, double y, double upperY, double alpha) => AssertInit(x, y, upperY, alpha, Unicolour.FromXyy, Unicolour.FromXyy, Unicolour.FromXyy, Unicolour.FromXyy);
    
    [TestCase(0, -128, -128)]
    [TestCase(100, 128, 128)]
    [TestCase(50, -1, 1)]
    public void Lab(double l, double a, double b) => AssertInit(l, a, b, Unicolour.FromLab, Unicolour.FromLab, Unicolour.FromLab, Unicolour.FromLab);
    
    [TestCase(0, -128, -128, 0)]
    [TestCase(100, 128, 128, 1)]
    [TestCase(50, -1, 1, 0.5)]
    public void LabAlpha(double l, double a, double b, double alpha) => AssertInit(l, a, b, alpha, Unicolour.FromLab, Unicolour.FromLab, Unicolour.FromLab, Unicolour.FromLab);
    
    [TestCase(0, 0, 0)]
    [TestCase(100, 230, 360)]
    [TestCase(50, 115, 180)]
    public void Lchab(double l, double c, double h) => AssertInit(l, c, h, Unicolour.FromLchab, Unicolour.FromLchab, Unicolour.FromLchab, Unicolour.FromLchab);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(100, 230, 360, 1)]
    [TestCase(50, 115, 180, 0.5)]
    public void LchabAlpha(double l, double c, double h, double alpha) => AssertInit(l, c, h, alpha, Unicolour.FromLchab, Unicolour.FromLchab, Unicolour.FromLchab, Unicolour.FromLchab);

    [TestCase(0, -100, -100)]
    [TestCase(100, 100, 100)]
    [TestCase(50, -1, 1)]
    public void Luv(double l, double u, double v) => AssertInit(l, u, v, Unicolour.FromLuv, Unicolour.FromLuv, Unicolour.FromLuv, Unicolour.FromLuv);
    
    [TestCase(0, -100, -100, 0)]
    [TestCase(100, 100, 100, 1)]
    [TestCase(50, -1, 1, 0.5)]
    public void LuvAlpha(double l, double u, double v, double alpha) => AssertInit(l, u, v, alpha, Unicolour.FromLuv, Unicolour.FromLuv, Unicolour.FromLuv, Unicolour.FromLuv);
    
    [TestCase(0, 0, 0)]
    [TestCase(100, 230, 360)]
    [TestCase(50, 115, 180)]
    public void Lchuv(double l, double c, double h) => AssertInit(l, c, h, Unicolour.FromLchuv, Unicolour.FromLchuv, Unicolour.FromLchuv, Unicolour.FromLchuv);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(100, 230, 360, 1)]
    [TestCase(50, 115, 180, 0.5)]
    public void LchuvAlpha(double l, double c, double h, double alpha) => AssertInit(l, c, h, alpha, Unicolour.FromLchuv, Unicolour.FromLchuv, Unicolour.FromLchuv, Unicolour.FromLchuv);
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 100, 100)]
    [TestCase(180, 50, 50)]
    public void Hsluv(double h, double s, double l) => AssertInit(h, s, l, Unicolour.FromHsluv, Unicolour.FromHsluv, Unicolour.FromHsluv, Unicolour.FromHsluv);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 100, 100, 1)]
    [TestCase(180, 50, 50, 0.5)]
    public void HsluvAlpha(double h, double s, double l, double alpha) => AssertInit(h, s, l, alpha, Unicolour.FromHsluv, Unicolour.FromHsluv, Unicolour.FromHsluv, Unicolour.FromHsluv);
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 100, 100)]
    [TestCase(180, 50, 50)]
    public void Hpluv(double h, double s, double l) => AssertInit(h, s, l, Unicolour.FromHpluv, Unicolour.FromHpluv, Unicolour.FromHpluv, Unicolour.FromHpluv);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 100, 100, 1)]
    [TestCase(180, 50, 50, 0.5)]
    public void HpluvAlpha(double h, double s, double l, double alpha) => AssertInit(h, s, l, alpha, Unicolour.FromHpluv, Unicolour.FromHpluv, Unicolour.FromHpluv, Unicolour.FromHpluv);

    [TestCase(0, -0.5, -0.5)]
    [TestCase(1, 0.5, 0.5)]
    [TestCase(0.5, -0.01, 0.01)]
    public void Ictcp(double i, double ct, double cp) => AssertInit(i, ct, cp, Unicolour.FromIctcp, Unicolour.FromIctcp, Unicolour.FromIctcp, Unicolour.FromIctcp);
    
    [TestCase(0, -0.5, -0.5, 0)]
    [TestCase(1, 0.5, 0.5, 1)]
    [TestCase(0.5, -0.01, 0.01, 0.5)]
    public void IctcpAlpha(double i, double ct, double cp, double alpha) => AssertInit(i, ct, cp, alpha, Unicolour.FromIctcp, Unicolour.FromIctcp, Unicolour.FromIctcp, Unicolour.FromIctcp);
    
    [TestCase(0, -0.10, -0.16)]
    [TestCase(0.17, 0.11, 0.12)]
    [TestCase(0.085, -0.0001, 0.0001)]
    public void Jzazbz(double j, double a, double b) => AssertInit(j, a, b, Unicolour.FromJzazbz, Unicolour.FromJzazbz, Unicolour.FromJzazbz, Unicolour.FromJzazbz);
    
    [TestCase(0, -0.10, -0.16, 0)]
    [TestCase(0.17, 0.11, 0.12, 1)]
    [TestCase(0.085, -0.0001, 0.0001, 0.5)]
    public void JzazbzAlpha(double j, double a, double b, double alpha) => AssertInit(j, a, b, alpha, Unicolour.FromJzazbz, Unicolour.FromJzazbz, Unicolour.FromJzazbz, Unicolour.FromJzazbz);
    
    [TestCase(0, 0, 0)]
    [TestCase(0.17, 0.16, 360)]
    [TestCase(0.085, 0.08, 180)]
    public void Jzczhz(double j, double c, double h) => AssertInit(j, c, h, Unicolour.FromJzczhz, Unicolour.FromJzczhz, Unicolour.FromJzczhz, Unicolour.FromJzczhz);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(0.17, 0.16, 360, 1)]
    [TestCase(0.085, 0.08, 180, 0.5)]
    public void JzczhzAlpha(double j, double c, double h, double alpha) => AssertInit(j, c, h, alpha, Unicolour.FromJzczhz, Unicolour.FromJzczhz, Unicolour.FromJzczhz, Unicolour.FromJzczhz);
    
    [TestCase(0, -0.5, -0.5)]
    [TestCase(1, 0.5, 0.5)]
    [TestCase(0.5, -0.001, 0.001)]
    public void Oklab(double l, double a, double b) => AssertInit(l, a, b, Unicolour.FromOklab, Unicolour.FromOklab, Unicolour.FromOklab, Unicolour.FromOklab);
    
    [TestCase(0, -0.5, -0.5, 0)]
    [TestCase(1, 0.5, 0.5, 1)]
    [TestCase(0.5, -0.001, 0.001, 0.5)]
    public void OklabAlpha(double l, double a, double b, double alpha) => AssertInit(l, a, b, alpha, Unicolour.FromOklab, Unicolour.FromOklab, Unicolour.FromOklab, Unicolour.FromOklab);
    
    [TestCase(0, 0, 0)]
    [TestCase(1, 0.5, 360)]
    [TestCase(0.5, 0.25, 180)]
    public void Oklch(double l, double c, double h) => AssertInit(l, c, h, Unicolour.FromOklch, Unicolour.FromOklch, Unicolour.FromOklch, Unicolour.FromOklch);

    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 0.5, 360, 1)]
    [TestCase(0.5, 0.25, 180, 0.5)]
    public void OklchAlpha(double l, double c, double h, double alpha) => AssertInit(l, c, h, alpha, Unicolour.FromOklch, Unicolour.FromOklch, Unicolour.FromOklch, Unicolour.FromOklch);
    
    [TestCase(0, -50, -50)]
    [TestCase(100, 50, 50)]
    [TestCase(50, -1, 1)]
    public void Cam02(double j, double a, double b) => AssertInit(j, a, b, Unicolour.FromCam02, Unicolour.FromCam02, Unicolour.FromCam02, Unicolour.FromCam02);
    
    [TestCase(0, -50, -50, 0)]
    [TestCase(100, 50, 50, 1)]
    [TestCase(50, -1, 1, 0.5)]
    public void Cam02Alpha(double j, double a, double b, double alpha) => AssertInit(j, a, b, alpha, Unicolour.FromCam02, Unicolour.FromCam02, Unicolour.FromCam02, Unicolour.FromCam02);
    
    [TestCase(0, -50, -50)]
    [TestCase(100, 50, 50)]
    [TestCase(50, -1, 1)]
    public void Cam16(double j, double a, double b) => AssertInit(j, a, b, Unicolour.FromCam16, Unicolour.FromCam16, Unicolour.FromCam16, Unicolour.FromCam16);
        
    [TestCase(0, -50, -50, 0)]
    [TestCase(100, 50, 50, 1)]
    [TestCase(50, -1, 1, 0.5)]
    public void Cam16Alpha(double j, double a, double b, double alpha) => AssertInit(j, a, b, alpha, Unicolour.FromCam16, Unicolour.FromCam16, Unicolour.FromCam16, Unicolour.FromCam16);
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 120, 100)]
    [TestCase(180, 60, 50)]
    public void Hct(double h, double c, double t) => AssertInit(h, c, t, Unicolour.FromHct, Unicolour.FromHct, Unicolour.FromHct, Unicolour.FromHct);
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 120, 100, 1)]
    [TestCase(180, 60, 50, 0.5)]
    public void HctAlpha(double h, double c, double t, double alpha) => AssertInit(h, c, t, alpha, Unicolour.FromHct, Unicolour.FromHct, Unicolour.FromHct, Unicolour.FromHct);

    // TODO: remove delegates and replace with explicit calls
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
        AssertNoError(expected, fromValuesWithConfig(Configuration.Default, first, second, third));
        AssertNoError(expected, fromTuple(tuple));
        AssertNoError(expected, fromTupleWithConfig(Configuration.Default, tuple));
    }
    
    private static void AssertInit(double first, double second, double third, double alpha,
        FromValues fromValues, FromValuesWithConfig fromValuesWithConfig,
        FromTuple fromTuple, FromTupleWithConfig fromTupleWithConfig)
    {
        var tuple = (first, second, third);
        var expected = fromValues(first, second, third, alpha);
        AssertNoError(expected, fromValues(first, second, third, alpha));
        AssertNoError(expected, fromValuesWithConfig(Configuration.Default, first, second, third, alpha));
        AssertNoError(expected, fromTuple(tuple, alpha));
        AssertNoError(expected, fromTupleWithConfig(Configuration.Default, tuple, alpha));
    }
    
    // dedicated method due to using ints instead of doubles
    private static void AssertRgb255(int first, int second, int third)
    {
        var tuple = (first, second, third);
        var expected = Unicolour.FromRgb255(first, second, third);
        AssertNoError(expected, Unicolour.FromRgb255(first, second, third));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, first, second, third));
        AssertNoError(expected, Unicolour.FromRgb255(tuple));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, tuple));
    }
    
    // dedicated method due to using ints instead of doubles
    private static void AssertRgb255(int first, int second, int third, int alpha)
    {
        var tuple = (first, second, third);
        var expected = Unicolour.FromRgb255(first, second, third, alpha);
        AssertNoError(expected, Unicolour.FromRgb255(first, second, third, alpha));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, first, second, third, alpha));
        AssertNoError(expected, Unicolour.FromRgb255(tuple, alpha));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, tuple, alpha));
    }
    
    private static void AssertHex(string hex)
    {
        var expected = Unicolour.FromHex(hex);
        AssertNoError(expected, Unicolour.FromHex(hex));
    }

    private static void AssertNoError(Unicolour expected, Unicolour unicolour)
    {
        AssertUtils.AssertNoPropertyError(unicolour);
        Assert.That(unicolour, Is.EqualTo(expected));
    }
}

