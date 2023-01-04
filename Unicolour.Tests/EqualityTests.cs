namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class EqualityTests
{
    [Test]
    public void EqualRgbGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromRgb();
        var unicolour2 = Unicolour.FromRgb(unicolour1.Rgb.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualHsbGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromHsb();
        var unicolour2 = Unicolour.FromHsb(unicolour1.Hsb.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualHslGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromHsl();
        var unicolour2 = Unicolour.FromHsl(unicolour1.Hsl.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualXyzGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromXyz();
        var unicolour2 = Unicolour.FromXyz(unicolour1.Xyz.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualXyyGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromXyy();
        var unicolour2 = Unicolour.FromXyy(unicolour1.Xyy.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualLabGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromLab();
        var unicolour2 = Unicolour.FromLab(unicolour1.Lab.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualLchabGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromLchab();
        var unicolour2 = Unicolour.FromLchab(unicolour1.Lchab.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualLuvGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromLuv();
        var unicolour2 = Unicolour.FromLuv(unicolour1.Luv.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualLchuvGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromLchuv();
        var unicolour2 = Unicolour.FromLchuv(unicolour1.Lchuv.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualHsluvGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromHsluv();
        var unicolour2 = Unicolour.FromHsluv(unicolour1.Hsluv.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
        
    [Test]
    public void EqualHpluvGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromHpluv();
        var unicolour2 = Unicolour.FromHpluv(unicolour1.Hpluv.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualJzazbzGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromJzazbz();
        var unicolour2 = Unicolour.FromJzazbz(unicolour1.Jzazbz.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualJzczhzGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromJzczhz();
        var unicolour2 = Unicolour.FromJzczhz(unicolour1.Jzczhz.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualOklabGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromOklab();
        var unicolour2 = Unicolour.FromOklab(unicolour1.Oklab.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [Test]
    public void EqualOklchGivesEqualObjects()
    {
        var unicolour1 = UnicolourFromOklch();
        var unicolour2 = Unicolour.FromOklch(unicolour1.Oklch.Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }

    [Test]
    public void NotEqualRgbGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromRgb();
        var differentTuple = GetDifferent(unicolour1.Rgb.Triplet).Tuple;
        var unicolour2 = Unicolour.FromRgb(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Rgb.Triplet);
    }
    
    [Test]
    public void NotEqualHsbGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromHsb();
        var differentTuple = GetDifferent(unicolour1.Hsb.Triplet).Tuple;
        var unicolour2 = Unicolour.FromHsb(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Hsb.Triplet);
    }
    
    [Test]
    public void NotEqualHslGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromHsl();
        var differentTuple = GetDifferent(unicolour1.Hsl.Triplet).Tuple;
        var unicolour2 = Unicolour.FromHsl(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Hsl.Triplet);
    }
    
    [Test]
    public void NotEqualXyzGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromXyz();
        var differentTuple = GetDifferent(unicolour1.Xyz.Triplet).Tuple;
        var unicolour2 = Unicolour.FromXyz(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Xyz.Triplet);
    }
    
    [Test]
    public void NotEqualXyyGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromXyy();
        var differentTuple = GetDifferent(unicolour1.Xyy.Triplet).Tuple;
        var unicolour2 = Unicolour.FromXyy(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Xyy.Triplet);
    }
    
    [Test]
    public void NotEqualLabGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromLab();
        var differentTuple = GetDifferent(unicolour1.Lab.Triplet, 1.0).Tuple;
        var unicolour2 = Unicolour.FromLab(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Lab.Triplet);
    }
    
    [Test]
    public void NotEqualLchabGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromLchab();
        var differentTuple = GetDifferent(unicolour1.Lchab.Triplet, 1.0).Tuple;
        var unicolour2 = Unicolour.FromLchab(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Lchab.Triplet);
    }
    
    [Test]
    public void NotEqualLuvGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromLuv();
        var differentTuple = GetDifferent(unicolour1.Luv.Triplet, 1.0).Tuple;
        var unicolour2 = Unicolour.FromLuv(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Luv.Triplet);
    }
    
    [Test]
    public void NotEqualLchuvGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromLchuv();
        var differentTuple = GetDifferent(unicolour1.Lchuv.Triplet, 1.0).Tuple;
        var unicolour2 = Unicolour.FromLchuv(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Lchuv.Triplet);
    }
    
    [Test]
    public void NotEqualHsluvGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromHsluv();
        var differentTuple = GetDifferent(unicolour1.Hsluv.Triplet).Tuple;
        var unicolour2 = Unicolour.FromHsluv(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Hsluv.Triplet);
    }
    
    [Test]
    public void NotEqualHpluvGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromHpluv();
        var differentTuple = GetDifferent(unicolour1.Hsluv.Triplet).Tuple;
        var unicolour2 = Unicolour.FromHpluv(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Hpluv.Triplet);
    }
    
    [Test]
    public void NotEqualJzazbzGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromJzazbz();
        var differentTuple = GetDifferent(unicolour1.Jzazbz.Triplet, 0.001).Tuple;
        var unicolour2 = Unicolour.FromJzazbz(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Jzazbz.Triplet);
    }
    
    [Test]
    public void NotEqualJzczhzGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromJzczhz();
        var differentTuple = GetDifferent(unicolour1.Jzczhz.Triplet, 0.001).Tuple;
        var unicolour2 = Unicolour.FromJzczhz(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Jzczhz.Triplet);
    }
    
    [Test]
    public void NotEqualOklabGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromOklab();
        var differentTuple = GetDifferent(unicolour1.Oklab.Triplet, 0.01).Tuple;
        var unicolour2 = Unicolour.FromOklab(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Oklab.Triplet);
    }
    
    [Test]
    public void NotEqualOklchGivesNotEqualObjects()
    {
        var unicolour1 = UnicolourFromOklch();
        var differentTuple = GetDifferent(unicolour1.Oklch.Triplet, 0.01).Tuple;
        var unicolour2 = Unicolour.FromOklch(differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.Oklch.Triplet);
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
    
    [Test]
    public void DifferentColourModeObjects()
    {
        var colourModes = new List<ColourMode>
        {
            ColourMode.Unset, ColourMode.NoExplicitBehaviour, 
            ColourMode.ExplicitNaN, ColourMode.ExplicitHue, ColourMode.ExplicitGreyscale
        };
        
        foreach (var mode in colourModes)
        {
            foreach (var otherMode in colourModes.Except(new[] {mode}))
            {
                AssertNotEqual(mode, otherMode);
            }
        }
    }
    
    private static Unicolour UnicolourFromRgb() => Unicolour.FromRgb(RandomColours.Rgb().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromHsb() => Unicolour.FromHsb(RandomColours.Hsb().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromHsl() => Unicolour.FromHsl(RandomColours.Hsl().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromXyz() => Unicolour.FromXyz(RandomColours.Xyz().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromXyy() => Unicolour.FromXyy(RandomColours.Xyy().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromLab() => Unicolour.FromLab(RandomColours.Lab().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromLchab() => Unicolour.FromLchab(RandomColours.Lchab().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromLuv() => Unicolour.FromLuv(RandomColours.Luv().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromLchuv() => Unicolour.FromLchuv(RandomColours.Lchuv().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromHsluv() => Unicolour.FromHsluv(RandomColours.Hsluv().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromHpluv() => Unicolour.FromHpluv(RandomColours.Hpluv().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromJzazbz() => Unicolour.FromJzazbz(RandomColours.Jzazbz().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromJzczhz() => Unicolour.FromJzczhz(RandomColours.Jzczhz().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromOklab() => Unicolour.FromOklab(RandomColours.Oklab().Tuple, RandomColours.Alpha());
    private static Unicolour UnicolourFromOklch() => Unicolour.FromOklch(RandomColours.Oklch().Tuple, RandomColours.Alpha());
    private static ColourTriplet GetDifferent(ColourTriplet triplet, double diff = 0.1) => new(triplet.First + diff, triplet.Second + diff, triplet.Third + diff);

    private static void AssertUnicoloursEqual(Unicolour unicolour1, Unicolour unicolour2)
    {
        AssertEqual(unicolour1.Rgb, unicolour2.Rgb);
        AssertEqual(unicolour1.Rgb.Byte255, unicolour2.Rgb.Byte255);
        AssertEqual(unicolour1.Rgb.Linear, unicolour2.Rgb.Linear);
        AssertEqual(unicolour1.Hsb, unicolour2.Hsb);
        AssertEqual(unicolour1.Hsl, unicolour2.Hsl);
        AssertEqual(unicolour1.Xyz, unicolour2.Xyz);
        AssertEqual(unicolour1.Xyy, unicolour2.Xyy);
        AssertEqual(unicolour1.Lab, unicolour2.Lab);
        AssertEqual(unicolour1.Lchab, unicolour2.Lchab);
        AssertEqual(unicolour1.Luv, unicolour2.Luv);
        AssertEqual(unicolour1.Lchuv, unicolour2.Lchuv);
        AssertEqual(unicolour1.Hsluv, unicolour2.Hsluv);
        AssertEqual(unicolour1.Hpluv, unicolour2.Hpluv);
        AssertEqual(unicolour1.Jzazbz, unicolour2.Jzazbz);
        AssertEqual(unicolour1.Jzczhz, unicolour2.Jzczhz);
        AssertEqual(unicolour1.Oklab, unicolour2.Oklab);
        AssertEqual(unicolour1.Oklch, unicolour2.Oklch);
        AssertEqual(unicolour1.Alpha, unicolour2.Alpha);
        AssertEqual(unicolour1.Hex, unicolour2.Hex);
        AssertEqual(unicolour1.IsDisplayable, unicolour2.IsDisplayable);
        AssertEqual(unicolour1.RelativeLuminance, unicolour2.RelativeLuminance);
        AssertEqual(unicolour1.Description, unicolour2.Description);
        AssertEqual(unicolour1, unicolour2);
    }

    private static void AssertUnicoloursNotEqual(Unicolour unicolour1, Unicolour unicolour2, Func<Unicolour, ColourTriplet> getTriplet)
    {
        AssertNotEqual(getTriplet(unicolour1), getTriplet(unicolour2));
        AssertNotEqual(unicolour1.Alpha, unicolour2.Alpha);
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