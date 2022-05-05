namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;

public static class MonochromeTests
{
    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(-0.00000000001, 0.0, -0.0, true)]
    [TestCase(-0.0, -0.00000000001, 0.0, true)]
    [TestCase(0.0, -0.0, -0.00000000001, true)]
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
    [TestCase(1.00000000001, 1.0, 1.0, true)]
    [TestCase(1.0, 1.00000000001, 1.0, true)]
    [TestCase(1.0, 1.0, 1.00000000001, true)]
    [TestCase(0.99999999999, 1.0, 1.0, false)]
    [TestCase(1.0, 0.99999999999, 1.0, false)]
    [TestCase(1.0, 1.0, 0.99999999999, false)]
    public static void MonochromeRgb(double r, double g, double b, bool expected)
    {
        var rgb = new Rgb(r, g, b, Configuration.Default);
        Assert.That(rgb.IsMonochrome, Is.EqualTo(expected));
        Assert.False(rgb.ConvertedFromMonochrome);
        AssertUnicolour(Unicolour.FromRgb(r, g, b), expected);
    }

    [TestCase(180.0, 0.0, 0.5, true)]
    [TestCase(180.0, -0.00000000001, 0.5, true)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, true)]
    [TestCase(180.0, 0.5, -0.00000000001, true)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    public static void MonochromeHsb(double h, double s, double b, bool expected)
    {
        var hsb = new Hsb(h, s, b);
        Assert.That(hsb.IsMonochrome, Is.EqualTo(expected));
        Assert.False(hsb.ConvertedFromMonochrome);
        AssertUnicolour(Unicolour.FromHsb(h, s, b), expected);
    }
    
    [TestCase(180.0, 0.0, 0.5, true)]
    [TestCase(180.0, -0.00000000001, 0.5, true)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, true)]
    [TestCase(180.0, 0.5, -0.00000000001, true)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    [TestCase(180.0, 0.5, 1.0, true)]
    [TestCase(180.0, 0.5, 1.00000000001, true)]
    [TestCase(180.0, 0.5, 0.99999999999, false)]
    public static void MonochromeHsl(double h, double s, double l, bool expected)
    {
        var hsl = new Hsl(h, s, l);
        Assert.That(hsl.IsMonochrome, Is.EqualTo(expected));
        Assert.False(hsl.ConvertedFromMonochrome);
        AssertUnicolour(Unicolour.FromHsl(h, s, l), expected);
    }

    // XYZ does not currently attempt to determine monochrome status from XYZ triplet, too much room for error
    // subsequent colour spaces may later report to be monochrome based on their own triplet values
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(0.5, 0.5, 0.5)]
    [TestCase(0.25, 0.5, 0.75)]
    [TestCase(0.95047, 1.0, 1.08883)]
    public static void MonochromeXyz(double x, double y, double z)
    {
        var xyz = new Xyz(x, y, z);
        Assert.False(xyz.ConvertedFromMonochrome);
    }
    
    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    public static void MonochromeLab(double l, double a, double b, bool expected)
    {
        var lab = new Lab(l, a, b);
        Assert.That(lab.IsMonochrome, Is.EqualTo(expected));
        Assert.False(lab.ConvertedFromMonochrome);
        AssertUnicolour(Unicolour.FromLab(l, a, b), expected);
    }
    
    [TestCase(50.0, 0.0, 180.0, true)]
    [TestCase(50.0, -0.00000000001, 180.0, true)]
    [TestCase(50.0, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 50.0, 180.0, true)]
    [TestCase(-0.00000000001, 50.0, 180.0, true)]
    [TestCase(0.00000000001, 50.0, 180.0, false)]
    [TestCase(100.0, 50.0, 180.0, true)]
    [TestCase(100.00000000001, 50.0, 180.0, true)]
    [TestCase(99.99999999999, 50.0, 180.0, false)]
    public static void MonochromeLchab(double l, double c, double h, bool expected)
    {
        var lchab = new Lchab(l, c, h);
        Assert.That(lchab.IsMonochrome, Is.EqualTo(expected));
        Assert.False(lchab.ConvertedFromMonochrome);
        AssertUnicolour(Unicolour.FromLchab(l, c, h), expected);
    }
    
    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    public static void MonochromeLuv(double l, double u, double v, bool expected)
    {
        var luv = new Luv(l, u, v);
        Assert.That(luv.IsMonochrome, Is.EqualTo(expected));
        Assert.False(luv.ConvertedFromMonochrome);
        AssertUnicolour(Unicolour.FromLuv(l, u, v), expected);
    }
    
    [TestCase(50.0, 0.0, 180.0, true)]
    [TestCase(50.0, -0.00000000001, 180.0, true)]
    [TestCase(50.0, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 50.0, 180.0, true)]
    [TestCase(-0.00000000001, 50.0, 180.0, true)]
    [TestCase(0.00000000001, 50.0, 180.0, false)]
    [TestCase(100.0, 50.0, 180.0, true)]
    [TestCase(100.00000000001, 50.0, 180.0, true)]
    [TestCase(99.99999999999, 50.0, 180.0, false)]
    public static void MonochromeLchuv(double l, double c, double h, bool expected)
    {
        var lchuv = new Lchuv(l, c, h);
        Assert.That(lchuv.IsMonochrome, Is.EqualTo(expected));
        Assert.False(lchuv.ConvertedFromMonochrome);
        AssertUnicolour(Unicolour.FromLchuv(l, c, h), expected);
    }
    
    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    public static void MonochromeOklab(double l, double a, double b, bool expected)
    {
        var oklab = new Oklab(l, a, b);
        Assert.That(oklab.IsMonochrome, Is.EqualTo(expected));
        Assert.False(oklab.ConvertedFromMonochrome);
        AssertUnicolour(Unicolour.FromOklab(l, a, b), expected);
    }
    
    [TestCase(0.5, 0.0, 180.0, true)]
    [TestCase(0.5, -0.00000000001, 180.0, true)]
    [TestCase(0.5, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 0.25, 180.0, true)]
    [TestCase(-0.00000000001, 0.25, 180.0, true)]
    [TestCase(0.00000000001, 0.25, 180.0, false)]
    [TestCase(1.0, 0.25, 180.0, true)]
    [TestCase(1.00000000001, 0.25, 180.0, true)]
    [TestCase(0.99999999999, 0.25, 180.0, false)]
    public static void MonochromeOklch(double l, double c, double h, bool expected)
    {
        var oklch = new Oklch(l, c, h);
        Assert.That(oklch.IsMonochrome, Is.EqualTo(expected));
        Assert.False(oklch.ConvertedFromMonochrome);
        AssertUnicolour(Unicolour.FromOklch(l, c, h), expected);
    }

    private static void AssertUnicolour(Unicolour unicolour, bool isMonochrome)
    {
        var isMonochromeList = GetIsMonochromeList(unicolour);
        var convertedFromMonochromeList = GetConvertedFromMonochromeList(unicolour);
        
        Assert.That(isMonochromeList, isMonochrome ? Has.All.EqualTo(isMonochrome) : Has.Some.EqualTo(false));
        Assert.That(convertedFromMonochromeList, isMonochrome ? Has.One.EqualTo(false) : Has.All.False);
    }
    
    private static List<bool> GetIsMonochromeList(Unicolour unicolour)
    {
        return new List<bool>
        {
            unicolour.Rgb.IsMonochrome,
            unicolour.Hsb.IsMonochrome,
            unicolour.Hsl.IsMonochrome,
            // unicolour.Xyz.IsMonochrome - XYZ only exposes ConvertedFromMonochrome (see Xyz.cs for details)
            unicolour.Lab.IsMonochrome,
            unicolour.Lchab.IsMonochrome,
            unicolour.Luv.IsMonochrome,
            unicolour.Lchuv.IsMonochrome,
            unicolour.Oklab.IsMonochrome,
            unicolour.Oklch.IsMonochrome
        };
    }

    private static List<bool> GetConvertedFromMonochromeList(Unicolour unicolour)
    {
        return new List<bool>
        {
            unicolour.Rgb.ConvertedFromMonochrome,
            unicolour.Hsb.ConvertedFromMonochrome,
            unicolour.Hsl.ConvertedFromMonochrome,
            unicolour.Xyz.ConvertedFromMonochrome,
            unicolour.Lab.ConvertedFromMonochrome,
            unicolour.Lchab.ConvertedFromMonochrome,
            unicolour.Luv.ConvertedFromMonochrome,
            unicolour.Lchuv.ConvertedFromMonochrome,
            unicolour.Oklab.ConvertedFromMonochrome,
            unicolour.Oklch.ConvertedFromMonochrome
        };
    }
}