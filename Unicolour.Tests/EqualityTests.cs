namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class EqualityTests
{
    [Test]
    public void EqualRgbGivesEqualObjects()
    {
        var unicolour1 = GetRandomRgbUnicolour();
        var unicolour2 = Unicolour.FromRgb(unicolour1.Rgb.R, unicolour1.Rgb.G, unicolour1.Rgb.B, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualHsbGivesEqualObjects()
    {
        var unicolour1 = GetRandomHsbUnicolour();
        var unicolour2 = Unicolour.FromHsb(unicolour1.Hsb.H, unicolour1.Hsb.S, unicolour1.Hsb.B, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualHslGivesEqualObjects()
    {
        var unicolour1 = GetRandomHslUnicolour();
        var unicolour2 = Unicolour.FromHsl(unicolour1.Hsl.H, unicolour1.Hsl.S, unicolour1.Hsl.L, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualXyzGivesEqualObjects()
    {
        var unicolour1 = GetRandomXyzUnicolour();
        var unicolour2 = Unicolour.FromXyz(unicolour1.Xyz.X, unicolour1.Xyz.Y, unicolour1.Xyz.Z, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualLabGivesEqualObjects()
    {
        var unicolour1 = GetRandomLabUnicolour();
        var unicolour2 = Unicolour.FromLab(unicolour1.Lab.L, unicolour1.Lab.A, unicolour1.Lab.B, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualLchabGivesEqualObjects()
    {
        var unicolour1 = GetRandomLchabUnicolour();
        var unicolour2 = Unicolour.FromLchab(unicolour1.Lchab.L, unicolour1.Lchab.C, unicolour1.Lchab.H, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualLuvGivesEqualObjects()
    {
        var unicolour1 = GetRandomLuvUnicolour();
        var unicolour2 = Unicolour.FromLuv(unicolour1.Luv.L, unicolour1.Luv.U, unicolour1.Luv.V, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualLchuvGivesEqualObjects()
    {
        var unicolour1 = GetRandomLchuvUnicolour();
        var unicolour2 = Unicolour.FromLchuv(unicolour1.Lchuv.L, unicolour1.Lchuv.C, unicolour1.Lchuv.H, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualOklabGivesEqualObjects()
    {
        var unicolour1 = GetRandomOklabUnicolour();
        var unicolour2 = Unicolour.FromOklab(unicolour1.Oklab.L, unicolour1.Oklab.A, unicolour1.Oklab.B, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualOklchGivesEqualObjects()
    {
        var unicolour1 = GetRandomOklchUnicolour();
        var unicolour2 = Unicolour.FromOklch(unicolour1.Oklch.L, unicolour1.Oklch.C, unicolour1.Oklch.H, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }

    [Test]
    public void NotEqualRgbGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomRgbUnicolour();
        var differentTuple = GetDifferent(unicolour1.Rgb.Triplet).Tuple;
        var unicolour2 = Unicolour.FromRgb(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Rgb.Triplet);
    }
    
    [Test]
    public void NotEqualHsbGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomHsbUnicolour();
        var differentTuple = GetDifferent(unicolour1.Hsb.Triplet).Tuple;
        var unicolour2 = Unicolour.FromHsb(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Hsb.Triplet);
    }
    
    [Test]
    public void NotEqualHslGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomHslUnicolour();
        var differentTuple = GetDifferent(unicolour1.Hsl.Triplet).Tuple;
        var unicolour2 = Unicolour.FromHsl(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Hsl.Triplet);
    }
    
    [Test]
    public void NotEqualXyzGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomXyzUnicolour();
        var differentTuple = GetDifferent(unicolour1.Xyz.Triplet).Tuple;
        var unicolour2 = Unicolour.FromXyz(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Xyz.Triplet);
    }
    
    [Test]
    public void NotEqualLabGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomLabUnicolour();
        var differentTuple = GetDifferent(unicolour1.Lab.Triplet, 1.0).Tuple;
        var unicolour2 = Unicolour.FromLab(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Lab.Triplet);
    }
    
    [Test]
    public void NotEqualLchabGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomLchabUnicolour();
        var differentTuple = GetDifferent(unicolour1.Lchab.Triplet, 1.0).Tuple;
        var unicolour2 = Unicolour.FromLchab(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Luv.Triplet);
    }
    
    [Test]
    public void NotEqualLuvGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomLuvUnicolour();
        var differentTuple = GetDifferent(unicolour1.Luv.Triplet, 1.0).Tuple;
        var unicolour2 = Unicolour.FromLuv(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Luv.Triplet);
    }
    
    [Test]
    public void NotEqualLchuvGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomLchuvUnicolour();
        var differentTuple = GetDifferent(unicolour1.Lchuv.Triplet, 1.0).Tuple;
        var unicolour2 = Unicolour.FromLchuv(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Luv.Triplet);
    }
    
    [Test]
    public void NotEqualOklabGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomOklabUnicolour();
        var differentTuple = GetDifferent(unicolour1.Oklab.Triplet).Tuple;
        var unicolour2 = Unicolour.FromOklab(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Oklab.Triplet);
    }
    
    [Test]
    public void NotEqualOklchGivesNotEqualObjects()
    {
        var unicolour1 = GetRandomOklchUnicolour();
        var differentTuple = GetDifferent(unicolour1.Oklch.Triplet, 1.0).Tuple;
        var unicolour2 = Unicolour.FromOklch(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Luv.Triplet);
    }
    
    [Test]
    public void DifferentConfigurationObjects()
    {
        var config1 = new Configuration(
            Chromaticity.StandardRgbR,
            new Chromaticity(0.25, 0.75),
            new Chromaticity(0.5, 0.5),
            Companding.StandardRgb,
            Companding.InverseStandardRgb, 
            new WhitePoint(0.9, 1.0, 1.1), 
            new WhitePoint(0.95, 1.0, 1.05));

        var config2 = new Configuration(
            Chromaticity.StandardRgbR,
            new Chromaticity(0.75, 0.25),
            new Chromaticity(0.5, 0.5),
            Companding.StandardRgb,
            Companding.InverseStandardRgb, 
            new WhitePoint(0.9, 1.0, 1.1), 
            new WhitePoint(0.95001, 1.0001, 1.05001));
        
        AssertEqual(config1.ChromaticityR, config2.ChromaticityR);
        AssertNotEqual(config1.ChromaticityG, config2.ChromaticityG);
        AssertEqual(config1.ChromaticityB, config2.ChromaticityB);
        AssertEqual(config1.RgbWhitePoint, config2.RgbWhitePoint);
        AssertNotEqual(config1.XyzWhitePoint, config2.XyzWhitePoint);
        AssertEqual(config1.Compand, config2.Compand);
        AssertEqual(config1.InverseCompand, config2.InverseCompand);
        AssertNotEqual(config1, config2);
    }
    
    private static Unicolour GetRandomRgbUnicolour() => Unicolour.FromRgb(TestColours.GetRandomRgb().Tuple, TestColours.GetRandomAlpha());
    private static Unicolour GetRandomHsbUnicolour() => Unicolour.FromHsb(TestColours.GetRandomHsb().Tuple, TestColours.GetRandomAlpha());
    private static Unicolour GetRandomHslUnicolour() => Unicolour.FromHsl(TestColours.GetRandomHsl().Tuple, TestColours.GetRandomAlpha());
    private static Unicolour GetRandomXyzUnicolour() => Unicolour.FromXyz(TestColours.GetRandomXyz().Tuple, TestColours.GetRandomAlpha());
    private static Unicolour GetRandomLabUnicolour() => Unicolour.FromLab(TestColours.GetRandomLab().Tuple, TestColours.GetRandomAlpha());
    private static Unicolour GetRandomLchabUnicolour() => Unicolour.FromLchab(TestColours.GetRandomLchab().Tuple, TestColours.GetRandomAlpha());
    private static Unicolour GetRandomLuvUnicolour() => Unicolour.FromLuv(TestColours.GetRandomLuv().Tuple, TestColours.GetRandomAlpha());
    private static Unicolour GetRandomLchuvUnicolour() => Unicolour.FromLchuv(TestColours.GetRandomLchuv().Tuple, TestColours.GetRandomAlpha());
    private static Unicolour GetRandomOklabUnicolour() => Unicolour.FromOklab(TestColours.GetRandomOklab().Tuple, TestColours.GetRandomAlpha());
    private static Unicolour GetRandomOklchUnicolour() => Unicolour.FromOklch(TestColours.GetRandomOklch().Tuple, TestColours.GetRandomAlpha());
    private static ColourTriplet GetDifferent(ColourTriplet triplet, double diff = 0.1) => new(triplet.First + diff, triplet.Second + diff, triplet.Third + diff);

    private static void AssertUnicoloursEqual(Unicolour unicolour1, Unicolour unicolour2)
    {
        AssertEqual(unicolour1.Rgb, unicolour2.Rgb);
        AssertEqual(unicolour1.Hsb, unicolour2.Hsb);
        AssertEqual(unicolour1.Hsl, unicolour2.Hsl);
        AssertEqual(unicolour1.Xyz, unicolour2.Xyz);
        AssertEqual(unicolour1.Lab, unicolour2.Lab);
        AssertEqual(unicolour1.Lchab, unicolour2.Lchab);
        AssertEqual(unicolour1.Luv, unicolour2.Luv);
        AssertEqual(unicolour1.Lchuv, unicolour2.Lchuv);
        AssertEqual(unicolour1.Oklab, unicolour2.Oklab);
        AssertEqual(unicolour1.Oklch, unicolour2.Oklch);
        AssertEqual(unicolour1.Alpha, unicolour2.Alpha);
        AssertEqual(unicolour1.RelativeLuminance, unicolour2.RelativeLuminance);
        AssertEqual(unicolour1, unicolour2);
    }

    private static void AssertUnicoloursNotEqual(Unicolour unicolour1, Unicolour unicolour2, Func<Unicolour, ColourTriplet> getTriplet)
    {
        AssertNotEqual(getTriplet(unicolour1), getTriplet(unicolour2));
        AssertNotEqual(unicolour1.Alpha, unicolour2.Alpha);
        AssertNotEqual(unicolour1.RelativeLuminance, unicolour2.RelativeLuminance);
        AssertNotEqual(unicolour1, unicolour2);
    }
    
    private static void AssertEqual<T>(T object1, T object2)
    {
        if (object1 == null || object2 == null)
        {
            Assert.Fail();
            return;
        }
        
        Assert.That(object1, Is.EqualTo(object2));
        Assert.That(object1.Equals(object2));
        Assert.That(object1.GetHashCode(), Is.EqualTo(object2.GetHashCode()));
        Assert.That(object1.ToString(), Is.EqualTo(object2.ToString()));
    }

    private static void AssertNotEqual<T>(T object1, T object2)
    {
        if (object1 == null || object2 == null)
        {
            Assert.Fail();
            return;
        }
        
        Assert.That(object1, Is.Not.EqualTo(object2));
        Assert.That(object1.Equals(object2), Is.False);
        Assert.That(object1.GetHashCode(), Is.Not.EqualTo(object2.GetHashCode()));
        Assert.That(object1.ToString(), Is.Not.EqualTo(object2.ToString()));
    }
}