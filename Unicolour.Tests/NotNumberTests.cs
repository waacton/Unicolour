namespace Wacton.Unicolour.Tests;

using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class NotNumberTests
{
    private static double[][] notNumberTestCases =
    {
        new[] {double.NaN, 0, 0},
        new[] {0, double.NaN, 0},
        new[] {0, 0, double.NaN},
        new[] {double.NaN, double.NaN, 0},
        new[] {double.NaN, 0, double.NaN},
        new[] {0, double.NaN, double.NaN},
        new[] {double.NaN, double.NaN, double.NaN}
    };
    
    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberRgb(double r, double g, double b) => AssertUnicolour(Unicolour.FromRgb(r, g, b));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberHsb(double h, double s, double b) => AssertUnicolour(Unicolour.FromHsb(h, s, b));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberHsl(double h, double s, double l) => AssertUnicolour(Unicolour.FromHsl(h, s, l));
    
    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberHwb(double h, double w, double b) => AssertUnicolour(Unicolour.FromHwb(h, w, b));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberXyz(double x, double y, double z) => AssertUnicolour(Unicolour.FromXyz(x, y, z));
    
    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberXyy(double x, double y, double upperY) => AssertUnicolour(Unicolour.FromXyy(x, y, upperY));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberLab(double l, double a, double b) => AssertUnicolour(Unicolour.FromLab(l, a, b));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberLchab(double l, double c, double h) => AssertUnicolour(Unicolour.FromLchab(l, c, h));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberLuv(double l, double u, double v) => AssertUnicolour(Unicolour.FromLuv(l, u, v));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberLchuv(double l, double c, double h) => AssertUnicolour(Unicolour.FromLchuv(l, c, h));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberHsluv(double h, double s, double l) => AssertUnicolour(Unicolour.FromHsluv(h, s, l));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberHpluv(double h, double s, double l) => AssertUnicolour(Unicolour.FromHpluv(h, s, l));
    
    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberIctcp(double i, double ct, double cp) => AssertUnicolour(Unicolour.FromIctcp(i, ct, cp));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberJzazbz(double jz, double az, double bz) => AssertUnicolour(Unicolour.FromJzazbz(jz, az, bz));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberJzczhz(double jz, double cz, double hz) => AssertUnicolour(Unicolour.FromJzczhz(jz, cz, hz));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberOklab(double l, double a, double b) => AssertUnicolour(Unicolour.FromOklab(l, a, b));

    [TestCaseSource(nameof(notNumberTestCases))]
    public static void NotNumberOklch(double l, double c, double h) => AssertUnicolour(Unicolour.FromOklch(l, c, h));
    
    // LUV -> XYZ converts NaNs to 0s
    // which results in downstream RGB / HSB / HSL containing real values but treated effectively as NaN
    [TestCaseSource(nameof(notNumberTestCases))]
    public static void OnlyEffectivelyNotNumber(double l, double u, double v)
    {
        var unicolour = Unicolour.FromLuv(l, u, v);
        Assert.That(unicolour.Luv.IsNaN, Is.True);
        Assert.That(unicolour.Xyz.IsNaN, Is.False);
        Assert.That(unicolour.Rgb.IsNaN, Is.False);
        Assert.That(unicolour.Hsb.IsNaN, Is.False);
        Assert.That(unicolour.Hsl.IsNaN, Is.False);
        
        Assert.That(unicolour.Luv.IsEffectivelyNaN, Is.True);
        Assert.That(unicolour.Xyz.IsEffectivelyNaN, Is.True);
        Assert.That(unicolour.Rgb.IsEffectivelyNaN, Is.True);
        Assert.That(unicolour.Hsb.IsEffectivelyNaN, Is.True);
        Assert.That(unicolour.Hsl.IsEffectivelyNaN, Is.True);
    }

    private static void AssertUnicolour(Unicolour unicolour)
    {
        var data = new ColourModeData(unicolour);
        var initial = unicolour.InitialRepresentation();
        
        Assert.That(initial.ColourMode, Is.EqualTo(ColourMode.ExplicitNaN));
        Assert.That(initial.IsNaN, Is.True);
        Assert.That(initial.IsEffectivelyNaN, Is.True);
        Assert.That(initial.IsEffectivelyGreyscale, Is.False);
        Assert.That(initial.IsEffectivelyHued, Is.False);
        Assert.That(initial.ToString().StartsWith("NaN"));
        Assert.That(unicolour.Hex, Is.EqualTo("-"));
        Assert.That(unicolour.IsDisplayable, Is.False);
        Assert.That(unicolour.RelativeLuminance, Is.NaN);
        Assert.That(unicolour.Description, Is.EqualTo("-"));

        var spaces = Enum.GetValues<ColourSpace>().ToList();
        Assert.That(data.Modes(spaces), Has.All.EqualTo(ColourMode.ExplicitNaN));
        Assert.That(data.NaN(spaces), Has.All.True);
        Assert.That(data.Greyscale(spaces), Has.All.False);
        Assert.That(data.Hued(spaces), Has.All.False);
    }
}