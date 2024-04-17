namespace Wacton.Unicolour.Tests;

using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class NotNumberTests
{
    private static double[][] testCases =
    {
        new[] { double.NaN, 0, 0 },
        new[] { 0, double.NaN, 0 },
        new[] { 0, 0, double.NaN },
        new[] { double.NaN, double.NaN, 0 },
        new[] { double.NaN, 0, double.NaN },
        new[] { 0, double.NaN, double.NaN },
        new[] { double.NaN, double.NaN, double.NaN }
    };
    
    [TestCaseSource(nameof(testCases))]
    public void Rgb(double r, double g, double b) => AssertUnicolour(new(ColourSpace.Rgb, r, g, b));
    
    [TestCaseSource(nameof(testCases))]
    public void Rgb255(double r, double g, double b) => AssertUnicolour(new(ColourSpace.Rgb255, r, g, b));
    
    [TestCaseSource(nameof(testCases))]
    public void RgbLinear(double r, double g, double b) => AssertUnicolour(new(ColourSpace.RgbLinear, r, g, b));

    [TestCaseSource(nameof(testCases))]
    public void Hsb(double h, double s, double b) => AssertUnicolour(new(ColourSpace.Hsb, h, s, b));

    [TestCaseSource(nameof(testCases))]
    public void Hsl(double h, double s, double l) => AssertUnicolour(new(ColourSpace.Hsl, h, s, l));
    
    [TestCaseSource(nameof(testCases))]
    public void Hwb(double h, double w, double b) => AssertUnicolour(new(ColourSpace.Hwb, h, w, b));

    [TestCaseSource(nameof(testCases))]
    public void Xyz(double x, double y, double z) => AssertUnicolour(new(ColourSpace.Xyz, x, y, z));
    
    [TestCaseSource(nameof(testCases))]
    public void Xyy(double x, double y, double upperY) => AssertUnicolour(new(ColourSpace.Xyy, x, y, upperY));

    [TestCaseSource(nameof(testCases))]
    public void Lab(double l, double a, double b) => AssertUnicolour(new(ColourSpace.Lab, l, a, b));

    [TestCaseSource(nameof(testCases))]
    public void Lchab(double l, double c, double h) => AssertUnicolour(new(ColourSpace.Lchab, l, c, h));

    [TestCaseSource(nameof(testCases))]
    public void Luv(double l, double u, double v) => AssertUnicolour(new(ColourSpace.Luv, l, u, v));

    [TestCaseSource(nameof(testCases))]
    public void Lchuv(double l, double c, double h) => AssertUnicolour(new(ColourSpace.Lchuv, l, c, h));

    [TestCaseSource(nameof(testCases))]
    public void Hsluv(double h, double s, double l) => AssertUnicolour(new(ColourSpace.Hsluv, h, s, l));

    [TestCaseSource(nameof(testCases))]
    public void Hpluv(double h, double s, double l) => AssertUnicolour(new(ColourSpace.Hpluv, h, s, l));
    
    [TestCaseSource(nameof(testCases))]
    public void Ypbpr(double y, double pb, double pr) => AssertUnicolour(new(ColourSpace.Ypbpr, y, pb, pr));
    
    [TestCaseSource(nameof(testCases))]
    public void Ycbcr(double y, double cb, double cr) => AssertUnicolour(new(ColourSpace.Ycbcr, y, cb, cr));
    
    [TestCaseSource(nameof(testCases))]
    public void Ycgco(double y, double cg, double co) => AssertUnicolour(new(ColourSpace.Ycbcr, y, cg, co));
    
    [TestCaseSource(nameof(testCases))]
    public void Yuv(double y, double u, double v) => AssertUnicolour(new(ColourSpace.Yuv, y, u, v));
    
    [TestCaseSource(nameof(testCases))]
    public void Yiq(double y, double i, double q) => AssertUnicolour(new(ColourSpace.Yiq, y, i, q));
    
    [TestCaseSource(nameof(testCases))]
    public void Ydbdr(double y, double db, double dr) => AssertUnicolour(new(ColourSpace.Ydbdr, y, db, dr));
    
    [TestCaseSource(nameof(testCases))]
    public void Ipt(double i, double p, double t) => AssertUnicolour(new(ColourSpace.Ipt, i, p, t));

    [TestCaseSource(nameof(testCases))]
    public void Ictcp(double i, double ct, double cp) => AssertUnicolour(new(ColourSpace.Ictcp, i, ct, cp));

    [TestCaseSource(nameof(testCases))]
    public void Jzazbz(double jz, double az, double bz) => AssertUnicolour(new(ColourSpace.Jzazbz, jz, az, bz));

    [TestCaseSource(nameof(testCases))]
    public void Jzczhz(double jz, double cz, double hz) => AssertUnicolour(new(ColourSpace.Jzczhz, jz, cz, hz));

    [TestCaseSource(nameof(testCases))]
    public void Oklab(double l, double a, double b) => AssertUnicolour(new(ColourSpace.Oklab, l, a, b));

    [TestCaseSource(nameof(testCases))]
    public void Oklch(double l, double c, double h) => AssertUnicolour(new(ColourSpace.Oklch, l, c, h));
    
    [TestCaseSource(nameof(testCases))]
    public void Okhsv(double h, double s, double v) => AssertUnicolour(new(ColourSpace.Okhsv, h, s, v));

    [TestCaseSource(nameof(testCases))]
    public void Okhsl(double h, double s, double l) => AssertUnicolour(new(ColourSpace.Okhsl, h, s, l));
    
    [TestCaseSource(nameof(testCases))]
    public void Okhwb(double h, double w, double b) => AssertUnicolour(new(ColourSpace.Okhwb, h, w, b));
    
    [TestCaseSource(nameof(testCases))]
    public void Cam02(double j, double a, double b) => AssertUnicolour(new(ColourSpace.Cam02, j, a, b));
    
    [TestCaseSource(nameof(testCases))]
    public void Cam16(double j, double a, double b) => AssertUnicolour(new(ColourSpace.Cam16, j, a, b));
    
    [TestCaseSource(nameof(testCases))]
    public void Hct(double h, double c, double t) => AssertUnicolour(new(ColourSpace.Hct, h, c, t));
    
    // LUV -> XYZ converts NaNs to 0s
    // which results in downstream RGB / HSB / HSL containing real values but are used as NaN
    [TestCaseSource(nameof(testCases))]
    public void IsNumberButUseAsNotNumber(double l, double u, double v)
    {
        var unicolour = new Unicolour(ColourSpace.Luv, l, u, v);
        Assert.That(unicolour.Luv.IsNaN, Is.True);
        Assert.That(unicolour.Xyz.IsNaN, Is.False);
        Assert.That(unicolour.Rgb.IsNaN, Is.False);
        Assert.That(unicolour.Hsb.IsNaN, Is.False);
        Assert.That(unicolour.Hsl.IsNaN, Is.False);
        
        Assert.That(unicolour.Luv.UseAsNaN, Is.True);
        Assert.That(unicolour.Xyz.UseAsNaN, Is.True);
        Assert.That(unicolour.Rgb.UseAsNaN, Is.True);
        Assert.That(unicolour.Hsb.UseAsNaN, Is.True);
        Assert.That(unicolour.Hsl.UseAsNaN, Is.True);
    }

    private static void AssertUnicolour(Unicolour unicolour)
    {
        var data = new ColourHeritageData(unicolour);
        var initial = unicolour.InitialRepresentation;
        
        Assert.That(initial.Heritage, Is.EqualTo(ColourHeritage.None));
        Assert.That(initial.IsNaN, Is.True);
        Assert.That(initial.UseAsNaN, Is.True);
        Assert.That(initial.UseAsGreyscale, Is.False);
        Assert.That(initial.UseAsHued, Is.False);
        Assert.That(initial.ToString().StartsWith("NaN"));
        Assert.That(unicolour.Hex, Is.EqualTo("-"));
        Assert.That(unicolour.Rgb.Byte255.ConstrainedHex, Is.EqualTo("-"));
        Assert.That(unicolour.Chromaticity.Xy, Is.EqualTo((double.NaN, double.NaN)));
        Assert.That(unicolour.Chromaticity.Uv, Is.EqualTo((double.NaN, double.NaN)));
        Assert.That(unicolour.IsInDisplayGamut, Is.False);
        Assert.That(unicolour.RelativeLuminance, Is.NaN);
        Assert.That(unicolour.Description, Is.EqualTo("-"));
        Assert.That(unicolour.Temperature.Cct, Is.NaN);
        Assert.That(unicolour.Temperature.Duv, Is.NaN);

        var spaces = TestUtils.AllColourSpaces.Except(new [] { unicolour.InitialColourSpace }).ToList();
        Assert.That(data.Heritages(spaces), Has.All.EqualTo(ColourHeritage.NaN));
        Assert.That(data.UseAsNaN(spaces), Has.All.True);
        Assert.That(data.UseAsGreyscale(spaces), Has.All.False);
        Assert.That(data.UseAsHued(spaces), Has.All.False);
    }
}