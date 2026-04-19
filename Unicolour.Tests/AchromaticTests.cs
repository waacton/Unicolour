using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class AchromaticTests
{
    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(-0.00000000001, 0.0, -0.0, false)]
    [TestCase(-0.0, -0.00000000001, 0.0, false)]
    [TestCase(0.0, -0.0, -0.00000000001, false)]
    [TestCase(0.00000000001, 0.0, 0.0, false)]
    [TestCase(0.0, 0.00000000001, 0.0, false)]
    [TestCase(0.0, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.5, 0.5, true)]
    [TestCase(0.50000000001, 0.5, 0.5, false)]
    [TestCase(0.5, 0.50000000001, 0.5, false)]
    [TestCase(0.5, 0.5, 0.50000000001, false)]
    [TestCase(0.49999999999, 0.5, 0.5, false)]
    [TestCase(0.5, 0.49999999999, 0.5, false)]
    [TestCase(0.5, 0.5, 0.49999999999, false)]
    [TestCase(1.0, 1.0, 1.0, true)]
    [TestCase(1.00000000001, 1.0, 1.0, false)]
    [TestCase(1.0, 1.00000000001, 1.0, false)]
    [TestCase(1.0, 1.0, 1.00000000001, false)]
    [TestCase(0.99999999999, 1.0, 1.0, false)]
    [TestCase(1.0, 0.99999999999, 1.0, false)]
    [TestCase(1.0, 1.0, 0.99999999999, false)]
    public void Rgb(double r, double g, double b, bool expected) => AssertUnicolour(new(ColourSpace.Rgb, r, g, b), expected);
    
    [TestCase(0, 0, 0, true)]
    [TestCase(-1, 0, -0, false)]
    [TestCase(-0, -1, 0, false)]
    [TestCase(0, -0, -1, false)]
    [TestCase(1, 0, 0, false)]
    [TestCase(0, 1, 0, false)]
    [TestCase(0, 0, 1, false)]
    [TestCase(128, 128, 128, true)]
    [TestCase(129, 128, 128, false)]
    [TestCase(128, 129, 128, false)]
    [TestCase(128, 128, 129, false)]
    [TestCase(127, 128, 128, false)]
    [TestCase(128, 127, 128, false)]
    [TestCase(128, 128, 127, false)]
    [TestCase(255, 255, 255, true)]
    [TestCase(256, 255, 255, false)]
    [TestCase(255, 256, 255, false)]
    [TestCase(255, 255, 256, false)]
    [TestCase(254, 255, 255, false)]
    [TestCase(255, 254, 255, false)]
    [TestCase(255, 255, 254, false)]
    public void Rgb255(double r, double g, double b, bool expected) => AssertUnicolour(new(ColourSpace.Rgb255, r, g, b), expected);
    
    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(-0.00000000001, 0.0, -0.0, false)]
    [TestCase(-0.0, -0.00000000001, 0.0, false)]
    [TestCase(0.0, -0.0, -0.00000000001, false)]
    [TestCase(0.00000000001, 0.0, 0.0, false)]
    [TestCase(0.0, 0.00000000001, 0.0, false)]
    [TestCase(0.0, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.5, 0.5, true)]
    [TestCase(0.50000000001, 0.5, 0.5, false)]
    [TestCase(0.5, 0.50000000001, 0.5, false)]
    [TestCase(0.5, 0.5, 0.50000000001, false)]
    [TestCase(0.49999999999, 0.5, 0.5, false)]
    [TestCase(0.5, 0.49999999999, 0.5, false)]
    [TestCase(0.5, 0.5, 0.49999999999, false)]
    [TestCase(1.0, 1.0, 1.0, true)]
    [TestCase(1.00000000001, 1.0, 1.0, false)]
    [TestCase(1.0, 1.00000000001, 1.0, false)]
    [TestCase(1.0, 1.0, 1.00000000001, false)]
    [TestCase(0.99999999999, 1.0, 1.0, false)]
    [TestCase(1.0, 0.99999999999, 1.0, false)]
    [TestCase(1.0, 1.0, 0.99999999999, false)]
    public void RgbLinear(double r, double g, double b, bool expected) => AssertUnicolour(new(ColourSpace.RgbLinear, r, g, b), expected);

    [TestCase(180.0, 0.0, 0.5, false)]
    [TestCase(180.0, -0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, false)]
    [TestCase(180.0, 0.5, -0.00000000001, false)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    public void Hsb(double h, double s, double b, bool expected) => AssertUnicolour(new(ColourSpace.Hsb, h, s, b), expected);

    [TestCase(180.0, 0.0, 0.5, false)]
    [TestCase(180.0, -0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, false)]
    [TestCase(180.0, 0.5, -0.00000000001, false)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    [TestCase(180.0, 0.5, 1.0, false)]
    [TestCase(180.0, 0.5, 1.00000000001, false)]
    [TestCase(180.0, 0.5, 0.99999999999, false)]
    public void Hsl(double h, double s, double l, bool expected) => AssertUnicolour(new(ColourSpace.Hsl, h, s, l), expected);
    
    [TestCase(180.0, 1.0, 0.0, false)]
    [TestCase(180.0, 1.00000000001, 0.0, false)]
    [TestCase(180.0, 0.99999999999, 0.0, false)]
    [TestCase(180.0, 0.0, 1.0, false)]
    [TestCase(180.0, 0.0, 1.00000000001, false)]
    [TestCase(180.0, 0.0, 0.99999999999, false)]
    [TestCase(180.0, 0.5, 0.5, false)]
    [TestCase(180.0, 0.50000000001, 0.5, false)]
    [TestCase(180.0, 0.49999999999, 0.5, false)]
    [TestCase(180.0, 0.5, 0.5, false)]
    [TestCase(180.0, 0.5, 0.50000000001, false)]
    [TestCase(180.0, 0.5, 0.49999999999, false)]
    public void Hwb(double h, double w, double b, bool expected) => AssertUnicolour(new(ColourSpace.Hwb, h, w, b), expected);

    [TestCase(180.0, 0.0, 0.5, false)]
    [TestCase(180.0, -0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, false)]
    [TestCase(180.0, 0.5, -0.00000000001, false)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    public void Hsi(double h, double s, double i, bool expected) => AssertUnicolour(new(ColourSpace.Hsi, h, s, i), expected);
    
    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(0.0, -0.00000000001, 0.0, false)]
    [TestCase(0.00000000001, 0.0, 0.00000000001, false)]
    [TestCase(0.0, 0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.5, 0.5, false)]
    [TestCase(0.25, 0.5, 0.75, false)]
    [TestCase(95.047 / 100, 100.000 / 100, 108.883 / 100, true)] // exact D65 default white from Unicolour
    [TestCase(95.047 / 200, 100.000 / 200, 108.883 / 200, true)] // exact D65 default white from Unicolour (50% luminance)
    [TestCase(95.047 / 400, 100.000 / 400, 108.883 / 400, true)] // exact D65 default white from Unicolour (25% luminance)
    public void Xyz(double x, double y, double z, bool expected) => AssertUnicolour(new(ColourSpace.Xyz, x, y, z), expected);
    
    [TestCase(0.0, 0.0, 0.0, false)]
    [TestCase(0.0, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.0, 0.00000000001, false)]
    [TestCase(1.0, 1.0, 0.0, false)]
    [TestCase(1.0, 1.0, -0.00000000001, false)]
    [TestCase(1.0, 1.0, 0.00000000001, false)]
    [TestCase(0.31272661468101204, 0.32902313032606195, 1.0, true)] // exact D65 default white from Unicolour
    [TestCase(0.31272661468101204, 0.32902313032606195, 0.5, true)] // exact D65 default white from Unicolour (50% luminance)
    [TestCase(0.31272661468101204, 0.32902313032606195, 0.25, true)] // exact D65 default white from Unicolour (25% luminance)
    public void Xyy(double x, double y, double upperY, bool expected) => AssertUnicolour(new(ColourSpace.Xyy, x, y, upperY), expected);
    
    [TestCase(360, 0.0, -0.00000000001, false)]
    [TestCase(360, 0.0, 0.00000000001, false)]
    [TestCase(360, 0.00000000001, 0.0, false)]
    [TestCase(360, -0.00000000001, 0.0, false)]
    [TestCase(360, 0.00000000001, 0.00000000001, false)]
    [TestCase(700, 0.0, -0.00000000001, false)]
    [TestCase(700, 0.0, 0.00000000001, false)]
    [TestCase(700, 0.00000000001, 0.0, false)]
    [TestCase(700, -0.00000000001, 0.0, false)]
    [TestCase(700, 0.00000000001, 0.00000000001, false)]
    [TestCase(-530, 0.0, -0.00000000001, false)]
    [TestCase(-530, 0.0, 0.00000000001, false)]
    [TestCase(-530, 0.00000000001, 0.0, false)]
    [TestCase(-530, -0.00000000001, 0.0, false)]
    [TestCase(-530, 0.00000000001, 0.00000000001, false)]
    public void Wxy(double w, double x, double y, bool expected) => AssertUnicolour(new(ColourSpace.Wxy, w, x, y), expected);

    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.00000000001, -0.00000000001, false)]
    [TestCase(-0.00000000001, 50, -50, false)]
    [TestCase(0.00000000001, 50, -50, false)]
    [TestCase(100.0, 50, -50, false)]
    [TestCase(100.00000000001, 50, -50, false)]
    [TestCase(99.99999999999, 50, -50, false)]
    public void Lab(double l, double a, double b, bool expected) => AssertUnicolour(new(ColourSpace.Lab, l, a, b), expected);

    [TestCase(50.0, 0.0, 180.0, false)]
    [TestCase(50.0, -0.00000000001, 180.0, false)]
    [TestCase(50.0, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 50.0, 180.0, false)]
    [TestCase(-0.00000000001, 50.0, 180.0, false)]
    [TestCase(0.00000000001, 50.0, 180.0, false)]
    [TestCase(100.0, 50.0, 180.0, false)]
    [TestCase(100.00000000001, 50.0, 180.0, false)]
    [TestCase(99.99999999999, 50.0, 180.0, false)]
    public void Lchab(double l, double c, double h, bool expected) => AssertUnicolour(new(ColourSpace.Lchab, l, c, h), expected);

    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.00000000001, -0.00000000001, false)]
    [TestCase(-0.00000000001, 50, -50, false)]
    [TestCase(0.00000000001, 50, -50, false)]
    [TestCase(100.0, 50, -50, false)]
    [TestCase(100.00000000001, 50, -50, false)]
    [TestCase(99.99999999999, 50, -50, false)]
    public void Luv(double l, double u, double v, bool expected) => AssertUnicolour(new(ColourSpace.Luv, l, u, v), expected);

    [TestCase(50.0, 0.0, 180.0, false)]
    [TestCase(50.0, -0.00000000001, 180.0, false)]
    [TestCase(50.0, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 50.0, 180.0, false)]
    [TestCase(-0.00000000001, 50.0, 180.0, false)]
    [TestCase(0.00000000001, 50.0, 180.0, false)]
    [TestCase(100.0, 50.0, 180.0, false)]
    [TestCase(100.00000000001, 50.0, 180.0, false)]
    [TestCase(99.99999999999, 50.0, 180.0, false)]
    public void Lchuv(double l, double c, double h, bool expected) => AssertUnicolour(new(ColourSpace.Lchuv, l, c, h), expected);

    [TestCase(180.0, 0.0, 50, false)]
    [TestCase(180.0, -0.00000000001, 50, false)]
    [TestCase(180.0, 0.00000000001, 50, false)]
    [TestCase(180.0, 50, 0.0, false)]
    [TestCase(180.0, 50, -0.00000000001, false)]
    [TestCase(180.0, 50, 0.00000000001, false)]
    [TestCase(180.0, 50, 100.0, false)]
    [TestCase(180.0, 50, 100.00000000001, false)]
    [TestCase(180.0, 50, 0.99999999999, false)]
    public void Hsluv(double h, double s, double l, bool expected) => AssertUnicolour(new(ColourSpace.Hsluv, h, s, l), expected);

    [TestCase(180.0, 0.0, 50, false)]
    [TestCase(180.0, -0.00000000001, 50, false)]
    [TestCase(180.0, 0.00000000001, 50, false)]
    [TestCase(180.0, 50, 0.0, false)]
    [TestCase(180.0, 50, -0.00000000001, false)]
    [TestCase(180.0, 50, 0.00000000001, false)]
    [TestCase(180.0, 50, 100.0, false)]
    [TestCase(180.0, 50, 100.00000000001, false)]
    [TestCase(180.0, 50, 0.99999999999, false)]
    public void Hpluv(double h, double s, double l, bool expected) => AssertUnicolour(new(ColourSpace.Hpluv, h, s, l), expected);
    
    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    [TestCase(1.00000000001, 0.1, -0.1, false)]
    public void Ypbpr(double y, double pb, double pr, bool expected) => AssertUnicolour(new(ColourSpace.Ypbpr, y, pb, pr), expected);
    
    [TestCase(0, 128, 128, true)]
    [TestCase(255, 128, 128, true)]
    [TestCase(0, 128.00000000001, 128, false)]
    [TestCase(0, 127.99999999999, 128, false)]
    [TestCase(0, 128, 128.00000000001, false)]
    [TestCase(0, 128, 127.99999999999, false)]
    [TestCase(-0.00000000001, 128.00000000001, 127.9999999999, false)]
    [TestCase(255.00000000001, 128.00000000001, 127.9999999999, false)]
    public void Ycbcr(double y, double cb, double cr, bool expected) => AssertUnicolour(new(ColourSpace.Ycbcr, y, cb, cr), expected);
    
    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    [TestCase(1.00000000001, 0.1, -0.1, false)]
    public void Ycgco(double y, double cg, double co, bool expected) => AssertUnicolour(new(ColourSpace.Ycgco, y, cg, co), expected);
    
    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    [TestCase(1.00000000001, 0.1, -0.1, false)]
    public void Yuv(double y, double u, double v, bool expected) => AssertUnicolour(new(ColourSpace.Yuv, y, u, v), expected);
    
    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    [TestCase(1.00000000001, 0.1, -0.1, false)]
    public void Yiq(double y, double i, double q, bool expected) => AssertUnicolour(new(ColourSpace.Yiq, y, i, q), expected);
    
    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    [TestCase(1.00000000001, 0.1, -0.1, false)]
    public void Ydbdr(double y, double db, double dr, bool expected) => AssertUnicolour(new(ColourSpace.Ydbdr, y, db, dr), expected);
    
    [TestCase(180.0, 0.0, 0.5, false)]
    [TestCase(180.0, -0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, false)]
    [TestCase(180.0, 0.5, -0.00000000001, false)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    public void Tsl(double t, double s, double l, bool expected) => AssertUnicolour(new(ColourSpace.Tsl, t, s, l), expected);
    
    [TestCase(0.0, 0.5, 0.0, true)]
    [TestCase(0.00000000001, 0.5, 0.0, false)]
    [TestCase(-0.00000000001, 0.5, 0.0, false)]
    [TestCase(0.0, 0.5, 0.00000000001, false)]
    [TestCase(0.0, 0.5, -0.00000000001, false)]
    [TestCase(0.1, 0.0, -0.1, false)]
    [TestCase(0.1, -0.00000000001, -0.1, false)]
    [TestCase(0.1, 0.00000000001, -0.1, false)]
    [TestCase(0.1, 1.0, -0.1, false)]
    public void Xyb(double x, double y, double b, bool expected) => AssertUnicolour(new(ColourSpace.Xyb, x, y, b), expected);
    
    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(-0.00000000001, 0.0, -0.0, false)]
    [TestCase(-0.0, -0.00000000001, 0.0, false)]
    [TestCase(0.0, -0.0, -0.00000000001, false)]
    [TestCase(0.00000000001, 0.0, 0.0, false)]
    [TestCase(0.0, 0.00000000001, 0.0, false)]
    [TestCase(0.0, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.5, 0.5, true)]
    [TestCase(0.50000000001, 0.5, 0.5, false)]
    [TestCase(0.5, 0.50000000001, 0.5, false)]
    [TestCase(0.5, 0.5, 0.50000000001, false)]
    [TestCase(0.49999999999, 0.5, 0.5, false)]
    [TestCase(0.5, 0.49999999999, 0.5, false)]
    [TestCase(0.5, 0.5, 0.49999999999, false)]
    [TestCase(1.0, 1.0, 1.0, true)]
    [TestCase(1.00000000001, 1.0, 1.0, false)]
    [TestCase(1.0, 1.00000000001, 1.0, false)]
    [TestCase(1.0, 1.0, 1.00000000001, false)]
    [TestCase(0.99999999999, 1.0, 1.0, false)]
    [TestCase(1.0, 0.99999999999, 1.0, false)]
    [TestCase(1.0, 1.0, 0.99999999999, false)]
    public void Lms(double l, double m, double s, bool expected) => AssertUnicolour(new(ColourSpace.Lms, l, m, s), expected);
    
    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    public void Ipt(double i, double p, double t, bool expected) => AssertUnicolour(new(ColourSpace.Ipt, i, p, t), expected);

    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    public void Ictcp(double i, double ct, double cp, bool expected) => AssertUnicolour(new(ColourSpace.Ictcp, i, ct, cp), expected);

    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    public void Jzazbz(double jz, double az, double bz, bool expected) => AssertUnicolour(new(ColourSpace.Jzazbz, jz, az, bz), expected);

    [TestCase(0.5, 0.0, 180.0, false)]
    [TestCase(0.5, -0.00000000001, 180.0, false)]
    [TestCase(0.5, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 0.1, 180.0, false)]
    [TestCase(-0.00000000001, 0.1, 180.0, false)]
    [TestCase(0.00000000001, 0.1, 180.0, false)]
    [TestCase(1.0, 0.1, 180.0, false)]
    public void Jzczhz(double jz, double cz, double hz, bool expected) => AssertUnicolour(new(ColourSpace.Jzczhz, jz, cz, hz), expected);

    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.00000000001, -0.00000000001, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    [TestCase(1.00000000001, 0.1, -0.1, false)]
    [TestCase(0.99999999999, 0.1, -0.1, false)]
    public void Oklab(double l, double a, double b, bool expected) => AssertUnicolour(new(ColourSpace.Oklab, l, a, b), expected);

    [TestCase(0.5, 0.0, 180.0, false)]
    [TestCase(0.5, -0.00000000001, 180.0, false)]
    [TestCase(0.5, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 0.25, 180.0, false)]
    [TestCase(-0.00000000001, 0.25, 180.0, false)]
    [TestCase(0.00000000001, 0.25, 180.0, false)]
    [TestCase(1.0, 0.25, 180.0, false)]
    [TestCase(1.00000000001, 0.25, 180.0, false)]
    [TestCase(0.99999999999, 0.25, 180.0, false)]
    public void Oklch(double l, double c, double h, bool expected) => AssertUnicolour(new(ColourSpace.Oklch, l, c, h), expected);
    
    [TestCase(180.0, 0.0, 0.5, false)]
    [TestCase(180.0, -0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, false)]
    [TestCase(180.0, 0.5, -0.00000000001, false)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    public void Okhsv(double h, double s, double v, bool expected) => AssertUnicolour(new(ColourSpace.Okhsv, h, s, v), expected);

    [TestCase(180.0, 0.0, 0.5, false)]
    [TestCase(180.0, -0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, false)]
    [TestCase(180.0, 0.5, -0.00000000001, false)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    [TestCase(180.0, 0.5, 1.0, false)]
    [TestCase(180.0, 0.5, 1.00000000001, false)]
    [TestCase(180.0, 0.5, 0.99999999999, false)]
    public void Okhsl(double h, double s, double l, bool expected) => AssertUnicolour(new(ColourSpace.Okhsl, h, s, l), expected);
    
    [TestCase(180.0, 1.0, 0.0, false)]
    [TestCase(180.0, 1.00000000001, 0.0, false)]
    [TestCase(180.0, 0.99999999999, 0.0, false)]
    [TestCase(180.0, 0.0, 1.0, false)]
    [TestCase(180.0, 0.0, 1.00000000001, false)]
    [TestCase(180.0, 0.0, 0.99999999999, false)]
    [TestCase(180.0, 0.5, 0.5, false)]
    [TestCase(180.0, 0.50000000001, 0.5, false)]
    [TestCase(180.0, 0.49999999999, 0.5, false)]
    [TestCase(180.0, 0.5, 0.5, false)]
    [TestCase(180.0, 0.5, 0.50000000001, false)]
    [TestCase(180.0, 0.5, 0.49999999999, false)]
    public void Okhwb(double h, double w, double b, bool expected) => AssertUnicolour(new(ColourSpace.Okhwb, h, w, b), expected);
    
    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.00000000001, -0.00000000001, false)]
    [TestCase(-0.00000000001, 0.1, -0.1, false)]
    [TestCase(0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    [TestCase(1.00000000001, 0.1, -0.1, false)]
    [TestCase(0.99999999999, 0.1, -0.1, false)]
    public void Oklrab(double l, double a, double b, bool expected) => AssertUnicolour(new(ColourSpace.Oklrab, l, a, b), expected);

    [TestCase(0.5, 0.0, 180.0, false)]
    [TestCase(0.5, -0.00000000001, 180.0, false)]
    [TestCase(0.5, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 0.25, 180.0, false)]
    [TestCase(-0.00000000001, 0.25, 180.0, false)]
    [TestCase(0.00000000001, 0.25, 180.0, false)]
    [TestCase(1.0, 0.25, 180.0, false)]
    [TestCase(1.00000000001, 0.25, 180.0, false)]
    [TestCase(0.99999999999, 0.25, 180.0, false)]
    public void Oklrch(double l, double c, double h, bool expected) => AssertUnicolour(new(ColourSpace.Oklrch, l, c, h), expected);
    
    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    public void Cam02(double j, double a, double b, bool expected) => AssertUnicolour(new(ColourSpace.Cam02, j, a, b), expected);
    
    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    public void Cam16(double j, double a, double b, bool expected) => AssertUnicolour(new(ColourSpace.Cam16, j, a, b), expected);
    
    [TestCase(180.0, 0.0, 50, false)]
    [TestCase(180.0, -0.00000000001, 50, false)]
    [TestCase(180.0, 0.00000000001, 50, false)]
    [TestCase(180.0, 50, 0.0, false)]
    [TestCase(180.0, 50, -0.00000000001, false)]
    [TestCase(180.0, 50, 0.00000000001, false)]
    [TestCase(180.0, 50, 100.0, false)]
    [TestCase(180.0, 50, 100.00000000001, false)]
    [TestCase(180.0, 50, 99.99999999999, false)]
    public void Hct(double h, double c, double t, bool expected) => AssertUnicolour(new(ColourSpace.Hct, h, c, t), expected);
    
    [TestCase(180.0, 5, 0.0, false)]
    [TestCase(180.0, 5, -0.00000000001, false)]
    [TestCase(180.0, 5, 0.00000000001, false)]
    [TestCase(180.0, 0.0, 10, false)]
    [TestCase(180.0, -0.00000000001, 10, false)]
    [TestCase(180.0, 0.00000000001, 10, false)]
    [TestCase(180.0, 10, 10, false)]
    public void Munsell(double h, double v, double c, bool expected) => AssertUnicolour(new(ColourSpace.Munsell, h, v, c), expected);

    private static void AssertUnicolour(Unicolour colour, bool expectAchromatic)
    {
        var expectedLimitation = expectAchromatic ? Limitation.Achromatic : Limitation.None;
        Assert.That(colour.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour.SourceRepresentation.Limitation, Is.EqualTo(expectedLimitation));

        if (colour.SourceRepresentation.Limitation == Limitation.Achromatic)
        {
            AssertAchromatic(colour);
        }
        else
        {
            AssertNotAchromatic(colour);
        }
    }
    
    private static readonly ColourSpace[] NaNProducingSpaces =
    [
        ColourSpace.Ictcp, ColourSpace.Jzazbz, ColourSpace.Jzczhz, ColourSpace.Cam02, ColourSpace.Cam16, ColourSpace.Hct
    ];
    
    private static void AssertAchromatic(Unicolour colour)
    {
        var spacesSafeFromNaN = TestUtils.AllColourSpaces.Except(NaNProducingSpaces).ToArray();
        if (spacesSafeFromNaN.Contains(colour.SourceColourSpace))
        {
            Assert.That(TestUtils.Limitations(colour, spacesSafeFromNaN, baselines: false), Has.All.EqualTo(Limitation.Achromatic));
            Assert.That(TestUtils.Limitations(colour, NaNProducingSpaces, baselines: false), Has.All.EqualTo(Limitation.Achromatic).Or.EqualTo(Limitation.NaN));
        }
        else
        {
            Assert.That(TestUtils.Limitations(colour, spacesSafeFromNaN, baselines: false), Has.All.EqualTo(Limitation.Achromatic).Or.EqualTo(Limitation.NaN));
        }
    }

    private static void AssertNotAchromatic(Unicolour colour)
    {
        // if initial representation has no limitation, at least one downstream representation should inherit no limitation
        // though it's still possible for their actual limitation to be achromatic or NaN, especially with outlier values like negatives
        var downstreamSpaces = TestUtils.AllColourSpaces.Except([colour.SourceColourSpace]).ToArray();
        Assert.That(TestUtils.Limitations(colour, downstreamSpaces, baselines: true), Has.Some.EqualTo(Limitation.None));
    }
}