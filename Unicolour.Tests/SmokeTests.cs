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
    public void Hex(string hex)
    {
        var expected = Unicolour.FromHex(hex);
        AssertNoError(expected, Unicolour.FromHex(hex));
    }
    
    [TestCase("00000000")]
    [TestCase("FFFFFFFF")]
    [TestCase("79808180")]
    [TestCase("#00000000")]
    [TestCase("#FFFFFFFF")]
    [TestCase("#79808180")]
    public void HexAlpha(string hex)
    {
        var expected = Unicolour.FromHex(hex);
        AssertNoError(expected, Unicolour.FromHex(hex));
    }

    [TestCase(0, 0, 0)]
    [TestCase(255, 255, 255)]
    [TestCase(127, 128, 129)]
    public void Rgb255(int r255, int g255, int b255)
    {
        var tuple = (r255, g255, b255);
        var expected = Unicolour.FromRgb255(r255, g255, b255);
        AssertNoError(expected, Unicolour.FromRgb255(r255, g255, b255));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, r255, g255, b255));
        AssertNoError(expected, Unicolour.FromRgb255(tuple));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(255, 255, 255, 255)]
    [TestCase(127, 128, 129, 128)]
    public void Rgb255Alpha(int r255, int g255, int b255, int alpha)
    {
        var tuple = (r255, g255, b255);
        var expected = Unicolour.FromRgb255(r255, g255, b255, alpha);
        AssertNoError(expected, Unicolour.FromRgb255(r255, g255, b255, alpha));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, r255, g255, b255, alpha));
        AssertNoError(expected, Unicolour.FromRgb255(tuple, alpha));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, tuple, alpha));
    }

    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void Rgb(double r, double g, double b)
    {
        var tuple = (r, g, b);
        var expected = Unicolour.FromRgb(r, g, b);
        AssertNoError(expected, Unicolour.FromRgb(r, g, b));
        AssertNoError(expected, Unicolour.FromRgb(Configuration.Default, r, g, b));
        AssertNoError(expected, Unicolour.FromRgb(tuple));
        AssertNoError(expected, Unicolour.FromRgb(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6, 0.5)]
    public void RgbAlpha(double r, double g, double b, double alpha)
    {
        var tuple = (r, g, b);
        var expected = Unicolour.FromRgb(r, g, b, alpha);
        AssertNoError(expected, Unicolour.FromRgb(r, g, b, alpha));
        AssertNoError(expected, Unicolour.FromRgb(Configuration.Default, r, g, b, alpha));
        AssertNoError(expected, Unicolour.FromRgb(tuple, alpha));
        AssertNoError(expected, Unicolour.FromRgb(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void RgbLinear(double r, double g, double b)
    {
        var tuple = (r, g, b);
        var expected = Unicolour.FromRgbLinear(r, g, b);
        AssertNoError(expected, Unicolour.FromRgbLinear(r, g, b));
        AssertNoError(expected, Unicolour.FromRgbLinear(Configuration.Default, r, g, b));
        AssertNoError(expected, Unicolour.FromRgbLinear(tuple));
        AssertNoError(expected, Unicolour.FromRgbLinear(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6, 0.5)]
    public void RgbLinearAlpha(double r, double g, double b, double alpha)
    {
        var tuple = (r, g, b);
        var expected = Unicolour.FromRgbLinear(r, g, b, alpha);
        AssertNoError(expected, Unicolour.FromRgbLinear(r, g, b, alpha));
        AssertNoError(expected, Unicolour.FromRgbLinear(Configuration.Default, r, g, b, alpha));
        AssertNoError(expected, Unicolour.FromRgbLinear(tuple, alpha));
        AssertNoError(expected, Unicolour.FromRgbLinear(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 1, 1)]
    [TestCase(180, 0.4, 0.6)]
    public void Hsb(double h, double s, double b)
    {
        var tuple = (h, s, b);
        var expected = Unicolour.FromHsb(h, s, b);
        AssertNoError(expected, Unicolour.FromHsb(h, s, b));
        AssertNoError(expected, Unicolour.FromHsb(Configuration.Default, h, s, b));
        AssertNoError(expected, Unicolour.FromHsb(tuple));
        AssertNoError(expected, Unicolour.FromHsb(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 1, 1, 1)]
    [TestCase(180, 0.4, 0.6, 0.5)]
    public void HsbAlpha(double h, double s, double b, double alpha)
    {
        var tuple = (h, s, b);
        var expected = Unicolour.FromHsb(h, s, b, alpha);
        AssertNoError(expected, Unicolour.FromHsb(h, s, b, alpha));
        AssertNoError(expected, Unicolour.FromHsb(Configuration.Default, h, s, b, alpha));
        AssertNoError(expected, Unicolour.FromHsb(tuple, alpha));
        AssertNoError(expected, Unicolour.FromHsb(Configuration.Default, tuple, alpha));
    }

    [TestCase(0, 0, 0)]
    [TestCase(360, 1, 1)]
    [TestCase(180, 0.4, 0.6)]
    public void Hsl(double h, double s, double l)
    {
        var tuple = (h, s, l);
        var expected = Unicolour.FromHsl(h, s, l);
        AssertNoError(expected, Unicolour.FromHsl(h, s, l));
        AssertNoError(expected, Unicolour.FromHsl(Configuration.Default, h, s, l));
        AssertNoError(expected, Unicolour.FromHsl(tuple));
        AssertNoError(expected, Unicolour.FromHsl(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 1, 1, 1)]
    [TestCase(180, 0.4, 0.6, 0.5)]
    public void HslAlpha(double h, double s, double l, double alpha)
    {
        var tuple = (h, s, l);
        var expected = Unicolour.FromHsl(h, s, l, alpha);
        AssertNoError(expected, Unicolour.FromHsl(h, s, l, alpha));
        AssertNoError(expected, Unicolour.FromHsl(Configuration.Default, h, s, l, alpha));
        AssertNoError(expected, Unicolour.FromHsl(tuple, alpha));
        AssertNoError(expected, Unicolour.FromHsl(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 1, 1)]
    [TestCase(180, 0.4, 0.6)]
    public void Hwb(double h, double w, double b)
    {
        var tuple = (h, w, b);
        var expected = Unicolour.FromHwb(h, w, b);
        AssertNoError(expected, Unicolour.FromHwb(h, w, b));
        AssertNoError(expected, Unicolour.FromHwb(Configuration.Default, h, w, b));
        AssertNoError(expected, Unicolour.FromHwb(tuple));
        AssertNoError(expected, Unicolour.FromHwb(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 1, 1, 1)]
    [TestCase(180, 0.4, 0.6, 0.5)]
    public void HwbAlpha(double h, double w, double b, double alpha)
    {
        var tuple = (h, w, b);
        var expected = Unicolour.FromHwb(h, w, b, alpha);
        AssertNoError(expected, Unicolour.FromHwb(h, w, b, alpha));
        AssertNoError(expected, Unicolour.FromHwb(Configuration.Default, h, w, b, alpha));
        AssertNoError(expected, Unicolour.FromHwb(tuple, alpha));
        AssertNoError(expected, Unicolour.FromHwb(Configuration.Default, tuple, alpha));
    }

    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void Xyz(double x, double y, double z)
    {
        var tuple = (x, y, z);
        var expected = Unicolour.FromXyz(x, y, z);
        AssertNoError(expected, Unicolour.FromXyz(x, y, z));
        AssertNoError(expected, Unicolour.FromXyz(Configuration.Default, x, y, z));
        AssertNoError(expected, Unicolour.FromXyz(tuple));
        AssertNoError(expected, Unicolour.FromXyz(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6, 0.5)]
    public void XyzAlpha(double x, double y, double z, double alpha)
    {
        var tuple = (x, y, z);
        var expected = Unicolour.FromXyz(x, y, z, alpha);
        AssertNoError(expected, Unicolour.FromXyz(x, y, z, alpha));
        AssertNoError(expected, Unicolour.FromXyz(Configuration.Default, x, y, z, alpha));
        AssertNoError(expected, Unicolour.FromXyz(tuple, alpha));
        AssertNoError(expected, Unicolour.FromXyz(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6)]
    public void Xyy(double x, double y, double upperY)
    {
        var tuple = (x, y, upperY);
        var expected = Unicolour.FromXyy(x, y, upperY);
        AssertNoError(expected, Unicolour.FromXyy(x, y, upperY));
        AssertNoError(expected, Unicolour.FromXyy(Configuration.Default, x, y, upperY));
        AssertNoError(expected, Unicolour.FromXyy(tuple));
        AssertNoError(expected, Unicolour.FromXyy(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 1, 1, 1)]
    [TestCase(0.4, 0.5, 0.6, 0.5)]
    public void XyyAlpha(double x, double y, double upperY, double alpha)
    {
        var tuple = (x, y, upperY);
        var expected = Unicolour.FromXyy(x, y, upperY, alpha);
        AssertNoError(expected, Unicolour.FromXyy(x, y, upperY, alpha));
        AssertNoError(expected, Unicolour.FromXyy(Configuration.Default, x, y, upperY, alpha));
        AssertNoError(expected, Unicolour.FromXyy(tuple, alpha));
        AssertNoError(expected, Unicolour.FromXyy(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, -128, -128)]
    [TestCase(100, 128, 128)]
    [TestCase(50, -1, 1)]
    public void Lab(double l, double a, double b)
    {
        var tuple = (l, a, b);
        var expected = Unicolour.FromLab(l, a, b);
        AssertNoError(expected, Unicolour.FromLab(l, a, b));
        AssertNoError(expected, Unicolour.FromLab(Configuration.Default, l, a, b));
        AssertNoError(expected, Unicolour.FromLab(tuple));
        AssertNoError(expected, Unicolour.FromLab(Configuration.Default, tuple));
    }
    
    [TestCase(0, -128, -128, 0)]
    [TestCase(100, 128, 128, 1)]
    [TestCase(50, -1, 1, 0.5)]
    public void LabAlpha(double l, double a, double b, double alpha)
    {
        var tuple = (l, a, b);
        var expected = Unicolour.FromLab(l, a, b, alpha);
        AssertNoError(expected, Unicolour.FromLab(l, a, b, alpha));
        AssertNoError(expected, Unicolour.FromLab(Configuration.Default, l, a, b, alpha));
        AssertNoError(expected, Unicolour.FromLab(tuple, alpha));
        AssertNoError(expected, Unicolour.FromLab(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(100, 230, 360)]
    [TestCase(50, 115, 180)]
    public void Lchab(double l, double c, double h)
    {
        var tuple = (l, c, h);
        var expected = Unicolour.FromLchab(l, c, h);
        AssertNoError(expected, Unicolour.FromLchab(l, c, h));
        AssertNoError(expected, Unicolour.FromLchab(Configuration.Default, l, c, h));
        AssertNoError(expected, Unicolour.FromLchab(tuple));
        AssertNoError(expected, Unicolour.FromLchab(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(100, 230, 360, 1)]
    [TestCase(50, 115, 180, 0.5)]
    public void LchabAlpha(double l, double c, double h, double alpha)
    {
        var tuple = (l, c, h);
        var expected = Unicolour.FromLchab(l, c, h, alpha);
        AssertNoError(expected, Unicolour.FromLchab(l, c, h, alpha));
        AssertNoError(expected, Unicolour.FromLchab(Configuration.Default, l, c, h, alpha));
        AssertNoError(expected, Unicolour.FromLchab(tuple, alpha));
        AssertNoError(expected, Unicolour.FromLchab(Configuration.Default, tuple, alpha));
    }

    [TestCase(0, -100, -100)]
    [TestCase(100, 100, 100)]
    [TestCase(50, -1, 1)]
    public void Luv(double l, double u, double v)
    {
        var tuple = (l, u, v);
        var expected = Unicolour.FromLuv(l, u, v);
        AssertNoError(expected, Unicolour.FromLuv(l, u, v));
        AssertNoError(expected, Unicolour.FromLuv(Configuration.Default, l, u, v));
        AssertNoError(expected, Unicolour.FromLuv(tuple));
        AssertNoError(expected, Unicolour.FromLuv(Configuration.Default, tuple));
    }
    
    [TestCase(0, -100, -100, 0)]
    [TestCase(100, 100, 100, 1)]
    [TestCase(50, -1, 1, 0.5)]
    public void LuvAlpha(double l, double u, double v, double alpha)
    {
        var tuple = (l, u, v);
        var expected = Unicolour.FromLuv(l, u, v, alpha);
        AssertNoError(expected, Unicolour.FromLuv(l, u, v, alpha));
        AssertNoError(expected, Unicolour.FromLuv(Configuration.Default, l, u, v, alpha));
        AssertNoError(expected, Unicolour.FromLuv(tuple, alpha));
        AssertNoError(expected, Unicolour.FromLuv(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(100, 230, 360)]
    [TestCase(50, 115, 180)]
    public void Lchuv(double l, double c, double h)    
    {
        var tuple = (l, c, h);
        var expected = Unicolour.FromLchuv(l, c, h);
        AssertNoError(expected, Unicolour.FromLchuv(l, c, h));
        AssertNoError(expected, Unicolour.FromLchuv(Configuration.Default, l, c, h));
        AssertNoError(expected, Unicolour.FromLchuv(tuple));
        AssertNoError(expected, Unicolour.FromLchuv(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(100, 230, 360, 1)]
    [TestCase(50, 115, 180, 0.5)]
    public void LchuvAlpha(double l, double c, double h, double alpha)
    {
        var tuple = (l, c, h);
        var expected = Unicolour.FromLchuv(l, c, h, alpha);
        AssertNoError(expected, Unicolour.FromLchuv(l, c, h, alpha));
        AssertNoError(expected, Unicolour.FromLchuv(Configuration.Default, l, c, h, alpha));
        AssertNoError(expected, Unicolour.FromLchuv(tuple, alpha));
        AssertNoError(expected, Unicolour.FromLchuv(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 100, 100)]
    [TestCase(180, 50, 50)]
    public void Hsluv(double h, double s, double l)
    {
        var tuple = (h, s, l);
        var expected = Unicolour.FromHsluv(h, s, l);
        AssertNoError(expected, Unicolour.FromHsluv(h, s, l));
        AssertNoError(expected, Unicolour.FromHsluv(Configuration.Default, h, s, l));
        AssertNoError(expected, Unicolour.FromHsluv(tuple));
        AssertNoError(expected, Unicolour.FromHsluv(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 100, 100, 1)]
    [TestCase(180, 50, 50, 0.5)]
    public void HsluvAlpha(double h, double s, double l, double alpha)
    {
        var tuple = (h, s, l);
        var expected = Unicolour.FromHsluv(h, s, l, alpha);
        AssertNoError(expected, Unicolour.FromHsluv(h, s, l, alpha));
        AssertNoError(expected, Unicolour.FromHsluv(Configuration.Default, h, s, l, alpha));
        AssertNoError(expected, Unicolour.FromHsluv(tuple, alpha));
        AssertNoError(expected, Unicolour.FromHsluv(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 100, 100)]
    [TestCase(180, 50, 50)]
    public void Hpluv(double h, double s, double l)
    {
        var tuple = (h, s, l);
        var expected = Unicolour.FromHpluv(h, s, l);
        AssertNoError(expected, Unicolour.FromHpluv(h, s, l));
        AssertNoError(expected, Unicolour.FromHpluv(Configuration.Default, h, s, l));
        AssertNoError(expected, Unicolour.FromHpluv(tuple));
        AssertNoError(expected, Unicolour.FromHpluv(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 100, 100, 1)]
    [TestCase(180, 50, 50, 0.5)]
    public void HpluvAlpha(double h, double s, double l, double alpha)
    {
        var tuple = (h, s, l);
        var expected = Unicolour.FromHpluv(h, s, l, alpha);
        AssertNoError(expected, Unicolour.FromHpluv(h, s, l, alpha));
        AssertNoError(expected, Unicolour.FromHpluv(Configuration.Default, h, s, l, alpha));
        AssertNoError(expected, Unicolour.FromHpluv(tuple, alpha));
        AssertNoError(expected, Unicolour.FromHpluv(Configuration.Default, tuple, alpha));
    }

    [TestCase(0, -0.5, -0.5)]
    [TestCase(1, 0.5, 0.5)]
    [TestCase(0.5, -0.01, 0.01)]
    public void Ictcp(double i, double ct, double cp)
    {
        var tuple = (i, ct, cp);
        var expected = Unicolour.FromIctcp(i, ct, cp);
        AssertNoError(expected, Unicolour.FromIctcp(i, ct, cp));
        AssertNoError(expected, Unicolour.FromIctcp(Configuration.Default, i, ct, cp));
        AssertNoError(expected, Unicolour.FromIctcp(tuple));
        AssertNoError(expected, Unicolour.FromIctcp(Configuration.Default, tuple));
    }
    
    [TestCase(0, -0.5, -0.5, 0)]
    [TestCase(1, 0.5, 0.5, 1)]
    [TestCase(0.5, -0.01, 0.01, 0.5)]
    public void IctcpAlpha(double i, double ct, double cp, double alpha)
    {
        var tuple = (i, ct, cp);
        var expected = Unicolour.FromIctcp(i, ct, cp, alpha);
        AssertNoError(expected, Unicolour.FromIctcp(i, ct, cp, alpha));
        AssertNoError(expected, Unicolour.FromIctcp(Configuration.Default, i, ct, cp, alpha));
        AssertNoError(expected, Unicolour.FromIctcp(tuple, alpha));
        AssertNoError(expected, Unicolour.FromIctcp(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, -0.10, -0.16)]
    [TestCase(0.17, 0.11, 0.12)]
    [TestCase(0.085, -0.0001, 0.0001)]
    public void Jzazbz(double jz, double az, double bz)
    {
        var tuple = (jz, az, bz);
        var expected = Unicolour.FromJzazbz(jz, az, bz);
        AssertNoError(expected, Unicolour.FromJzazbz(jz, az, bz));
        AssertNoError(expected, Unicolour.FromJzazbz(Configuration.Default, jz, az, bz));
        AssertNoError(expected, Unicolour.FromJzazbz(tuple));
        AssertNoError(expected, Unicolour.FromJzazbz(Configuration.Default, tuple));
    }
    
    [TestCase(0, -0.10, -0.16, 0)]
    [TestCase(0.17, 0.11, 0.12, 1)]
    [TestCase(0.085, -0.0001, 0.0001, 0.5)]
    public void JzazbzAlpha(double jz, double az, double bz, double alpha)
    {
        var tuple = (jz, az, bz);
        var expected = Unicolour.FromJzazbz(jz, az, bz, alpha);
        AssertNoError(expected, Unicolour.FromJzazbz(jz, az, bz, alpha));
        AssertNoError(expected, Unicolour.FromJzazbz(Configuration.Default, jz, az, bz, alpha));
        AssertNoError(expected, Unicolour.FromJzazbz(tuple, alpha));
        AssertNoError(expected, Unicolour.FromJzazbz(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(0.17, 0.16, 360)]
    [TestCase(0.085, 0.08, 180)]
    public void Jzczhz(double jz, double cz, double hz)
    {
        var tuple = (jz, cz, hz);
        var expected = Unicolour.FromJzczhz(jz, cz, hz);
        AssertNoError(expected, Unicolour.FromJzczhz(jz, cz, hz));
        AssertNoError(expected, Unicolour.FromJzczhz(Configuration.Default, jz, cz, hz));
        AssertNoError(expected, Unicolour.FromJzczhz(tuple));
        AssertNoError(expected, Unicolour.FromJzczhz(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(0.17, 0.16, 360, 1)]
    [TestCase(0.085, 0.08, 180, 0.5)]
    public void JzczhzAlpha(double jz, double cz, double hz, double alpha)
    {
        var tuple = (jz, cz, hz);
        var expected = Unicolour.FromJzczhz(jz, cz, hz, alpha);
        AssertNoError(expected, Unicolour.FromJzczhz(jz, cz, hz, alpha));
        AssertNoError(expected, Unicolour.FromJzczhz(Configuration.Default, jz, cz, hz, alpha));
        AssertNoError(expected, Unicolour.FromJzczhz(tuple, alpha));
        AssertNoError(expected, Unicolour.FromJzczhz(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, -0.5, -0.5)]
    [TestCase(1, 0.5, 0.5)]
    [TestCase(0.5, -0.001, 0.001)]
    public void Oklab(double l, double a, double b)
    {
        var tuple = (l, a, b);
        var expected = Unicolour.FromOklab(l, a, b);
        AssertNoError(expected, Unicolour.FromOklab(l, a, b));
        AssertNoError(expected, Unicolour.FromOklab(Configuration.Default, l, a, b));
        AssertNoError(expected, Unicolour.FromOklab(tuple));
        AssertNoError(expected, Unicolour.FromOklab(Configuration.Default, tuple));
    }
    
    [TestCase(0, -0.5, -0.5, 0)]
    [TestCase(1, 0.5, 0.5, 1)]
    [TestCase(0.5, -0.001, 0.001, 0.5)]
    public void OklabAlpha(double l, double a, double b, double alpha)
    {
        var tuple = (l, a, b);
        var expected = Unicolour.FromOklab(l, a, b, alpha);
        AssertNoError(expected, Unicolour.FromOklab(l, a, b, alpha));
        AssertNoError(expected, Unicolour.FromOklab(Configuration.Default, l, a, b, alpha));
        AssertNoError(expected, Unicolour.FromOklab(tuple, alpha));
        AssertNoError(expected, Unicolour.FromOklab(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(1, 0.5, 360)]
    [TestCase(0.5, 0.25, 180)]
    public void Oklch(double l, double c, double h)
    {
        var tuple = (l, c, h);
        var expected = Unicolour.FromOklch(l, c, h);
        AssertNoError(expected, Unicolour.FromOklch(l, c, h));
        AssertNoError(expected, Unicolour.FromOklch(Configuration.Default, l, c, h));
        AssertNoError(expected, Unicolour.FromOklch(tuple));
        AssertNoError(expected, Unicolour.FromOklch(Configuration.Default, tuple));
    }

    [TestCase(0, 0, 0, 0)]
    [TestCase(1, 0.5, 360, 1)]
    [TestCase(0.5, 0.25, 180, 0.5)]
    public void OklchAlpha(double l, double c, double h, double alpha)
    {
        var tuple = (l, c, h);
        var expected = Unicolour.FromOklch(l, c, h, alpha);
        AssertNoError(expected, Unicolour.FromOklch(l, c, h, alpha));
        AssertNoError(expected, Unicolour.FromOklch(Configuration.Default, l, c, h, alpha));
        AssertNoError(expected, Unicolour.FromOklch(tuple, alpha));
        AssertNoError(expected, Unicolour.FromOklch(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, -50, -50)]
    [TestCase(100, 50, 50)]
    [TestCase(50, -1, 1)]
    public void Cam02(double j, double a, double b)
    {
        var tuple = (j, a, b);
        var expected = Unicolour.FromCam02(j, a, b);
        AssertNoError(expected, Unicolour.FromCam02(j, a, b));
        AssertNoError(expected, Unicolour.FromCam02(Configuration.Default, j, a, b));
        AssertNoError(expected, Unicolour.FromCam02(tuple));
        AssertNoError(expected, Unicolour.FromCam02(Configuration.Default, tuple));
    }
    
    [TestCase(0, -50, -50, 0)]
    [TestCase(100, 50, 50, 1)]
    [TestCase(50, -1, 1, 0.5)]
    public void Cam02Alpha(double j, double a, double b, double alpha)
    {
        var tuple = (j, a, b);
        var expected = Unicolour.FromCam02(j, a, b, alpha);
        AssertNoError(expected, Unicolour.FromCam02(j, a, b, alpha));
        AssertNoError(expected, Unicolour.FromCam02(Configuration.Default, j, a, b, alpha));
        AssertNoError(expected, Unicolour.FromCam02(tuple, alpha));
        AssertNoError(expected, Unicolour.FromCam02(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, -50, -50)]
    [TestCase(100, 50, 50)]
    [TestCase(50, -1, 1)]
    public void Cam16(double j, double a, double b)
    {
        var tuple = (j, a, b);
        var expected = Unicolour.FromCam16(j, a, b);
        AssertNoError(expected, Unicolour.FromCam16(j, a, b));
        AssertNoError(expected, Unicolour.FromCam16(Configuration.Default, j, a, b));
        AssertNoError(expected, Unicolour.FromCam16(tuple));
        AssertNoError(expected, Unicolour.FromCam16(Configuration.Default, tuple));
    }
        
    [TestCase(0, -50, -50, 0)]
    [TestCase(100, 50, 50, 1)]
    [TestCase(50, -1, 1, 0.5)]
    public void Cam16Alpha(double j, double a, double b, double alpha)
    {
        var tuple = (j, a, b);
        var expected = Unicolour.FromCam16(j, a, b, alpha);
        AssertNoError(expected, Unicolour.FromCam16(j, a, b, alpha));
        AssertNoError(expected, Unicolour.FromCam16(Configuration.Default, j, a, b, alpha));
        AssertNoError(expected, Unicolour.FromCam16(tuple, alpha));
        AssertNoError(expected, Unicolour.FromCam16(Configuration.Default, tuple, alpha));
    }
    
    [TestCase(0, 0, 0)]
    [TestCase(360, 120, 100)]
    [TestCase(180, 60, 50)]
    public void Hct(double h, double c, double t)
    {
        var tuple = (h, c, t);
        var expected = Unicolour.FromHct(h, c, t);
        AssertNoError(expected, Unicolour.FromHct(h, c, t));
        AssertNoError(expected, Unicolour.FromHct(Configuration.Default, h, c, t));
        AssertNoError(expected, Unicolour.FromHct(tuple));
        AssertNoError(expected, Unicolour.FromHct(Configuration.Default, tuple));
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(360, 120, 100, 1)]
    [TestCase(180, 60, 50, 0.5)]
    public void HctAlpha(double h, double c, double t, double alpha)
    {
        var tuple = (h, c, t);
        var expected = Unicolour.FromHct(h, c, t, alpha);
        AssertNoError(expected, Unicolour.FromHct(h, c, t, alpha));
        AssertNoError(expected, Unicolour.FromHct(Configuration.Default, h, c, t, alpha));
        AssertNoError(expected, Unicolour.FromHct(tuple, alpha));
        AssertNoError(expected, Unicolour.FromHct(Configuration.Default, tuple, alpha));
    }

    private static void AssertNoError(Unicolour expected, Unicolour unicolour)
    {
        AssertUtils.AssertNoPropertyError(unicolour);
        Assert.That(unicolour, Is.EqualTo(expected));
    }
}

