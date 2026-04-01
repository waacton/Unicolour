using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class NotNumberTests
{
    private static readonly TestCaseData[] TestData =
    [
        new(Configuration.Default, double.NaN, 0, 0),
        new(Configuration.Default, 0, double.NaN, 0),
        new(Configuration.Default, 0, 0, double.NaN),
        new(Configuration.Default, double.NaN, double.NaN, 0),
        new(Configuration.Default, double.NaN, 0, double.NaN),
        new(Configuration.Default, 0, double.NaN, double.NaN),
        new(Configuration.Default, double.NaN, double.NaN, double.NaN),
        new(TestUtils.DefaultFogra39Config, double.NaN, 0, 0),
        new(TestUtils.DefaultFogra39Config, 0, double.NaN, 0),
        new(TestUtils.DefaultFogra39Config, 0, 0, double.NaN),
        new(TestUtils.DefaultFogra39Config, double.NaN, double.NaN, 0),
        new(TestUtils.DefaultFogra39Config, double.NaN, 0, double.NaN),
        new(TestUtils.DefaultFogra39Config, 0, double.NaN, double.NaN),
        new(TestUtils.DefaultFogra39Config, double.NaN, double.NaN, double.NaN)
    ];
    
    [TestCaseSource(nameof(TestData))]
    public void Rgb(Configuration config, double r, double g, double b) => AssertUnicolour(new(config, ColourSpace.Rgb, r, g, b));
    
    [TestCaseSource(nameof(TestData))]
    public void Rgb255(Configuration config, double r, double g, double b) => AssertUnicolour(new(config, ColourSpace.Rgb255, r, g, b));
    
    [TestCaseSource(nameof(TestData))]
    public void RgbLinear(Configuration config, double r, double g, double b) => AssertUnicolour(new(config, ColourSpace.RgbLinear, r, g, b));

    [TestCaseSource(nameof(TestData))]
    public void Hsb(Configuration config, double h, double s, double b) => AssertUnicolour(new(config, ColourSpace.Hsb, h, s, b));

    [TestCaseSource(nameof(TestData))]
    public void Hsl(Configuration config, double h, double s, double l) => AssertUnicolour(new(config, ColourSpace.Hsl, h, s, l));
    
    [TestCaseSource(nameof(TestData))]
    public void Hwb(Configuration config, double h, double w, double b) => AssertUnicolour(new(config, ColourSpace.Hwb, h, w, b));
    
    [TestCaseSource(nameof(TestData))]
    public void Hsi(Configuration config, double h, double s, double i) => AssertUnicolour(new(config, ColourSpace.Hsi, h, s, i));

    [TestCaseSource(nameof(TestData))]
    public void Xyz(Configuration config, double x, double y, double z) => AssertUnicolour(new(config, ColourSpace.Xyz, x, y, z));
    
    [TestCaseSource(nameof(TestData))]
    public void Xyy(Configuration config, double x, double y, double upperY) => AssertUnicolour(new(config, ColourSpace.Xyy, x, y, upperY));
    
    [TestCaseSource(nameof(TestData))]
    public void Wxy(Configuration config, double w, double x, double y) => AssertUnicolour(new(config, ColourSpace.Wxy, w, x, y));

    [TestCaseSource(nameof(TestData))]
    public void Lab(Configuration config, double l, double a, double b) => AssertUnicolour(new(config, ColourSpace.Lab, l, a, b));

    [TestCaseSource(nameof(TestData))]
    public void Lchab(Configuration config, double l, double c, double h) => AssertUnicolour(new(config, ColourSpace.Lchab, l, c, h));

    [TestCaseSource(nameof(TestData))]
    public void Luv(Configuration config, double l, double u, double v) => AssertUnicolour(new(config, ColourSpace.Luv, l, u, v));

    [TestCaseSource(nameof(TestData))]
    public void Lchuv(Configuration config, double l, double c, double h) => AssertUnicolour(new(config, ColourSpace.Lchuv, l, c, h));

    [TestCaseSource(nameof(TestData))]
    public void Hsluv(Configuration config, double h, double s, double l) => AssertUnicolour(new(config, ColourSpace.Hsluv, h, s, l));

    [TestCaseSource(nameof(TestData))]
    public void Hpluv(Configuration config, double h, double s, double l) => AssertUnicolour(new(config, ColourSpace.Hpluv, h, s, l));
    
    [TestCaseSource(nameof(TestData))]
    public void Ypbpr(Configuration config, double y, double pb, double pr) => AssertUnicolour(new(config, ColourSpace.Ypbpr, y, pb, pr));
    
    [TestCaseSource(nameof(TestData))]
    public void Ycbcr(Configuration config, double y, double cb, double cr) => AssertUnicolour(new(config, ColourSpace.Ycbcr, y, cb, cr));
    
    [TestCaseSource(nameof(TestData))]
    public void Ycgco(Configuration config, double y, double cg, double co) => AssertUnicolour(new(config, ColourSpace.Ycbcr, y, cg, co));
    
    [TestCaseSource(nameof(TestData))]
    public void Yuv(Configuration config, double y, double u, double v) => AssertUnicolour(new(config, ColourSpace.Yuv, y, u, v));
    
    [TestCaseSource(nameof(TestData))]
    public void Yiq(Configuration config, double y, double i, double q) => AssertUnicolour(new(config, ColourSpace.Yiq, y, i, q));
    
    [TestCaseSource(nameof(TestData))]
    public void Ydbdr(Configuration config, double y, double db, double dr) => AssertUnicolour(new(config, ColourSpace.Ydbdr, y, db, dr));
    
    [TestCaseSource(nameof(TestData))]
    public void Tsl(Configuration config, double t, double s, double l) => AssertUnicolour(new(config, ColourSpace.Tsl, t, s, l));
    
    [TestCaseSource(nameof(TestData))]
    public void Xyb(Configuration config, double x, double y, double b) => AssertUnicolour(new(config, ColourSpace.Xyb, x, y, b));
    
    [TestCaseSource(nameof(TestData))]
    public void Lms(Configuration config, double l, double m, double s) => AssertUnicolour(new(config, ColourSpace.Lms, l, m, s));
    
    [TestCaseSource(nameof(TestData))]
    public void Ipt(Configuration config, double i, double p, double t) => AssertUnicolour(new(config, ColourSpace.Ipt, i, p, t));

    [TestCaseSource(nameof(TestData))]
    public void Ictcp(Configuration config, double i, double ct, double cp) => AssertUnicolour(new(config, ColourSpace.Ictcp, i, ct, cp));

    [TestCaseSource(nameof(TestData))]
    public void Jzazbz(Configuration config, double jz, double az, double bz) => AssertUnicolour(new(config, ColourSpace.Jzazbz, jz, az, bz));

    [TestCaseSource(nameof(TestData))]
    public void Jzczhz(Configuration config, double jz, double cz, double hz) => AssertUnicolour(new(config, ColourSpace.Jzczhz, jz, cz, hz));

    [TestCaseSource(nameof(TestData))]
    public void Oklab(Configuration config, double l, double a, double b) => AssertUnicolour(new(config, ColourSpace.Oklab, l, a, b));

    [TestCaseSource(nameof(TestData))]
    public void Oklch(Configuration config, double l, double c, double h) => AssertUnicolour(new(config, ColourSpace.Oklch, l, c, h));
    
    [TestCaseSource(nameof(TestData))]
    public void Okhsv(Configuration config, double h, double s, double v) => AssertUnicolour(new(config, ColourSpace.Okhsv, h, s, v));

    [TestCaseSource(nameof(TestData))]
    public void Okhsl(Configuration config, double h, double s, double l) => AssertUnicolour(new(config, ColourSpace.Okhsl, h, s, l));
    
    [TestCaseSource(nameof(TestData))]
    public void Okhwb(Configuration config, double h, double w, double b) => AssertUnicolour(new(config, ColourSpace.Okhwb, h, w, b));
    
    [TestCaseSource(nameof(TestData))]
    public void Oklrab(Configuration config, double l, double a, double b) => AssertUnicolour(new(config, ColourSpace.Oklrab, l, a, b));

    [TestCaseSource(nameof(TestData))]
    public void Oklrch(Configuration config, double l, double c, double h) => AssertUnicolour(new(config, ColourSpace.Oklrch, l, c, h));
    
    [TestCaseSource(nameof(TestData))]
    public void Cam02(Configuration config, double j, double a, double b) => AssertUnicolour(new(config, ColourSpace.Cam02, j, a, b));
    
    [TestCaseSource(nameof(TestData))]
    public void Cam16(Configuration config, double j, double a, double b) => AssertUnicolour(new(config, ColourSpace.Cam16, j, a, b));
    
    [TestCaseSource(nameof(TestData))]
    public void Hct(Configuration config, double h, double c, double t) => AssertUnicolour(new(config, ColourSpace.Hct, h, c, t));
    
    [TestCaseSource(nameof(TestData))]
    public void Munsell(Configuration config, double h, double v, double c) => AssertUnicolour(new(config, ColourSpace.Munsell, h, v, c));

    [TestCaseSource(nameof(TestData))]
    public void Icc(Configuration config, double c, double m, double y) => AssertUnicolour(new(config, new Channels(c, m, y, 0.0)));
    
    // LUV -> XYZ converts NaNs to 0s
    // which results in downstream RGB / HSB / HSL containing real values but are used as NaN
    [TestCaseSource(nameof(TestData))]
    public void IsNumberButUseAsNotNumber(Configuration config, double l, double u, double v)
    {
        var colour = new Unicolour(ColourSpace.Luv, l, u, v);
        Assert.That(colour.Luv.Limitation, Is.EqualTo(Limitation.NaN));
        Assert.That(colour.Xyz.Limitation, Is.EqualTo(Limitation.NaN));
        Assert.That(colour.Rgb.Limitation, Is.EqualTo(Limitation.NaN));
        Assert.That(colour.Hsb.Limitation, Is.EqualTo(Limitation.NaN));
        Assert.That(colour.Hsl.Limitation, Is.EqualTo(Limitation.NaN));
    }

    private static void AssertUnicolour(Unicolour colour)
    {
        var initial = colour.SourceRepresentation;
        
        Assert.That(initial.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(initial.Limitation, Is.EqualTo(Limitation.NaN));
        Assert.That(initial.ToString().StartsWith("NaN"));
        Assert.That(colour.Hex, Is.EqualTo("-"));
        Assert.That(colour.Rgb.Byte255.Hex, Is.EqualTo("-"));
        Assert.That(colour.Chromaticity.Xy, Is.EqualTo((double.NaN, double.NaN)));
        Assert.That(colour.Chromaticity.Uv, Is.EqualTo((double.NaN, double.NaN)));
        Assert.That(colour.IsInRgbGamut, Is.False);
        Assert.That(colour.RelativeLuminance, Is.NaN);
        Assert.That(colour.Description, Is.EqualTo("-"));
        Assert.That(colour.Temperature.Cct, Is.NaN);
        Assert.That(colour.Temperature.Duv, Is.NaN);
        Assert.That(colour.Icc.ToString().StartsWith("NaN"));
        
        var downstreamSpaces = TestUtils.AllColourSpaces.Except([colour.SourceColourSpace]).ToArray();
        Assert.That(TestUtils.Limitations(colour, TestUtils.AllColourSpaces, baselines: false), Has.All.EqualTo(Limitation.NaN));
        Assert.That(TestUtils.Limitations(colour, downstreamSpaces, baselines: true), Has.All.EqualTo(Limitation.NaN));
    }
}