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
    public void Rgb(double r, double g, double b) => AssertUnicolour(Unicolour.FromRgb(r, g, b));

    [TestCaseSource(nameof(testCases))]
    public void Hsb(double h, double s, double b) => AssertUnicolour(Unicolour.FromHsb(h, s, b));

    [TestCaseSource(nameof(testCases))]
    public void Hsl(double h, double s, double l) => AssertUnicolour(Unicolour.FromHsl(h, s, l));
    
    [TestCaseSource(nameof(testCases))]
    public void Hwb(double h, double w, double b) => AssertUnicolour(Unicolour.FromHwb(h, w, b));

    [TestCaseSource(nameof(testCases))]
    public void Xyz(double x, double y, double z) => AssertUnicolour(Unicolour.FromXyz(x, y, z));
    
    [TestCaseSource(nameof(testCases))]
    public void Xyy(double x, double y, double upperY) => AssertUnicolour(Unicolour.FromXyy(x, y, upperY));

    [TestCaseSource(nameof(testCases))]
    public void Lab(double l, double a, double b) => AssertUnicolour(Unicolour.FromLab(l, a, b));

    [TestCaseSource(nameof(testCases))]
    public void Lchab(double l, double c, double h) => AssertUnicolour(Unicolour.FromLchab(l, c, h));

    [TestCaseSource(nameof(testCases))]
    public void Luv(double l, double u, double v) => AssertUnicolour(Unicolour.FromLuv(l, u, v));

    [TestCaseSource(nameof(testCases))]
    public void Lchuv(double l, double c, double h) => AssertUnicolour(Unicolour.FromLchuv(l, c, h));

    [TestCaseSource(nameof(testCases))]
    public void Hsluv(double h, double s, double l) => AssertUnicolour(Unicolour.FromHsluv(h, s, l));

    [TestCaseSource(nameof(testCases))]
    public void Hpluv(double h, double s, double l) => AssertUnicolour(Unicolour.FromHpluv(h, s, l));

    [TestCaseSource(nameof(testCases))]
    public void Ictcp(double i, double ct, double cp) => AssertUnicolour(Unicolour.FromIctcp(i, ct, cp));

    [TestCaseSource(nameof(testCases))]
    public void Jzazbz(double jz, double az, double bz) => AssertUnicolour(Unicolour.FromJzazbz(jz, az, bz));

    [TestCaseSource(nameof(testCases))]
    public void Jzczhz(double jz, double cz, double hz) => AssertUnicolour(Unicolour.FromJzczhz(jz, cz, hz));

    [TestCaseSource(nameof(testCases))]
    public void Oklab(double l, double a, double b) => AssertUnicolour(Unicolour.FromOklab(l, a, b));

    [TestCaseSource(nameof(testCases))]
    public void Oklch(double l, double c, double h) => AssertUnicolour(Unicolour.FromOklch(l, c, h));
    
    [TestCaseSource(nameof(testCases))]
    public void Cam02(double j, double a, double b) => AssertUnicolour(Unicolour.FromCam02(j, a, b));
    
    [TestCaseSource(nameof(testCases))]
    public void Cam16(double j, double a, double b) => AssertUnicolour(Unicolour.FromCam16(j, a, b));
    
    [TestCaseSource(nameof(testCases))]
    public void Hct(double h, double c, double t) => AssertUnicolour(Unicolour.FromHct(h, c, t));
    
    // LUV -> XYZ converts NaNs to 0s
    // which results in downstream RGB / HSB / HSL containing real values but are used as NaN
    [TestCaseSource(nameof(testCases))]
    public static void IsNumberButUseAsNotNumber(double l, double u, double v)
    {
        var unicolour = Unicolour.FromLuv(l, u, v);
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
        Assert.That(unicolour.IsDisplayable, Is.False);
        Assert.That(unicolour.RelativeLuminance, Is.NaN);
        Assert.That(unicolour.Description, Is.EqualTo("-"));

        var spaces = AssertUtils.AllColourSpaces.Except(new [] { unicolour.InitialColourSpace }).ToList();
        Assert.That(data.Heritages(spaces), Has.All.EqualTo(ColourHeritage.NaN));
        Assert.That(data.UseAsNaN(spaces), Has.All.True);
        Assert.That(data.UseAsGreyscale(spaces), Has.All.False);
        Assert.That(data.UseAsHued(spaces), Has.All.False);
    }
}