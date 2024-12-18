﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class NotNumberTests
{
    private static readonly List<TestCaseData> TestData =
    [
        new TestCaseData(Configuration.Default, double.NaN, 0, 0),
        new TestCaseData(Configuration.Default, 0, double.NaN, 0),
        new TestCaseData(Configuration.Default, 0, 0, double.NaN),
        new TestCaseData(Configuration.Default, double.NaN, double.NaN, 0),
        new TestCaseData(Configuration.Default, double.NaN, 0, double.NaN),
        new TestCaseData(Configuration.Default, 0, double.NaN, double.NaN),
        new TestCaseData(Configuration.Default, double.NaN, double.NaN, double.NaN),
        new TestCaseData(TestUtils.DefaultFogra39Config, double.NaN, 0, 0),
        new TestCaseData(TestUtils.DefaultFogra39Config, 0, double.NaN, 0),
        new TestCaseData(TestUtils.DefaultFogra39Config, 0, 0, double.NaN),
        new TestCaseData(TestUtils.DefaultFogra39Config, double.NaN, double.NaN, 0),
        new TestCaseData(TestUtils.DefaultFogra39Config, double.NaN, 0, double.NaN),
        new TestCaseData(TestUtils.DefaultFogra39Config, 0, double.NaN, double.NaN),
        new TestCaseData(TestUtils.DefaultFogra39Config, double.NaN, double.NaN, double.NaN)
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
    public void Cam02(Configuration config, double j, double a, double b) => AssertUnicolour(new(config, ColourSpace.Cam02, j, a, b));
    
    [TestCaseSource(nameof(TestData))]
    public void Cam16(Configuration config, double j, double a, double b) => AssertUnicolour(new(config, ColourSpace.Cam16, j, a, b));
    
    [TestCaseSource(nameof(TestData))]
    public void Hct(Configuration config, double h, double c, double t) => AssertUnicolour(new(config, ColourSpace.Hct, h, c, t));

    [TestCaseSource(nameof(TestData))]
    public void Icc(Configuration config, double c, double m, double y) => AssertUnicolour(new(config, new Channels(c, m, y, 0.0)));
    
    // LUV -> XYZ converts NaNs to 0s
    // which results in downstream RGB / HSB / HSL containing real values but are used as NaN
    [TestCaseSource(nameof(TestData))]
    public void IsNumberButUseAsNotNumber(Configuration config, double l, double u, double v)
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
        Assert.That(unicolour.Icc.ToString().StartsWith("NaN"));

        var spaces = TestUtils.AllColourSpaces.Except(new [] { unicolour.InitialColourSpace }).ToList();
        Assert.That(data.Heritages(spaces), Has.All.EqualTo(ColourHeritage.NaN));
        Assert.That(data.UseAsNaN(spaces), Has.All.True);
        Assert.That(data.UseAsGreyscale(spaces), Has.All.False);
        Assert.That(data.UseAsHued(spaces), Has.All.False);
    }
}