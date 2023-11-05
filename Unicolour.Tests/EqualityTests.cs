namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class EqualityTests
{
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void SameReference(ColourSpace colourSpace)
    {
        var unicolour1 = RandomColours.UnicolourFrom(colourSpace);
        var unicolour2 = unicolour1;
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void DifferentType(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        var notUnicolour = new object();
        AssertNotEqual(unicolour, notUnicolour);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void NullUnicolour(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        Assert.That(unicolour.Equals(null), Is.False);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void NullObject(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        Assert.That(unicolour.Equals(null as object), Is.False);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void EqualObjects(ColourSpace colourSpace)
    {
        var unicolour1 = RandomColours.UnicolourFrom(colourSpace);
        var unicolour2 = new Unicolour(colourSpace, unicolour1.GetRepresentation(colourSpace).Triplet.Tuple, unicolour1.Alpha.A);
        AssertUnicoloursEqual(unicolour1, unicolour2);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void NotEqualObjects(ColourSpace colourSpace)
    {
        var unicolour1 = RandomColours.UnicolourFrom(colourSpace);
        var differentTuple = GetDifferent(unicolour1.GetRepresentation(colourSpace).Triplet).Tuple;
        var unicolour2 = new Unicolour(colourSpace, differentTuple, unicolour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(unicolour1, unicolour2, unicolour => unicolour.GetRepresentation(colourSpace).Triplet);
    }
    
    [Test]
    public void DifferentConfigurationObjects()
    {
        var rgbConfig1 = new RgbConfiguration(
            Chromaticity.StandardRgb.R,
            new Chromaticity(0.25, 0.75),
            new Chromaticity(0.5, 0.5),
            new WhitePoint(0.9, 1.0, 1.1),
            Companding.StandardRgb.FromLinear,
            Companding.StandardRgb.ToLinear);
        var xyzConfig1 = new XyzConfiguration(new WhitePoint(0.95, 1.0, 1.05));
        var camConfig1 = new CamConfiguration(new WhitePoint(0.9, 1.0, 1.1), 4, 20, Surround.Dark);
        var config1 = new Configuration(rgbConfig1, xyzConfig1, camConfig1);
        
        var rgbConfig2 = new RgbConfiguration(
            Chromaticity.StandardRgb.R,
            new Chromaticity(0.75, 0.25),
            new Chromaticity(0.5, 0.5), 
            new WhitePoint(0.9, 1.0, 1.1),
            Companding.StandardRgb.FromLinear,
            Companding.StandardRgb.ToLinear);
        var xyzConfig2 = new XyzConfiguration(new WhitePoint(0.95001, 1.0001, 1.05001));
        var camConfig2 = new CamConfiguration(new WhitePoint(0.9, 1.0, 1.1), 4, 20, Surround.Dim);
        var config2 = new Configuration(rgbConfig2, xyzConfig2, camConfig2);

        AssertEqual(config1.Rgb.ChromaticityR, config2.Rgb.ChromaticityR);
        AssertNotEqual(config1.Rgb.ChromaticityG, config2.Rgb.ChromaticityG);
        AssertEqual(config1.Rgb.ChromaticityB, config2.Rgb.ChromaticityB);
        AssertEqual(config1.Rgb.WhitePoint, config2.Rgb.WhitePoint);
        AssertEqual(config1.Rgb.CompandFromLinear, config2.Rgb.CompandFromLinear);
        AssertEqual(config1.Rgb.InverseCompandToLinear, config2.Rgb.InverseCompandToLinear);
        AssertNotEqual(config1.Xyz.WhitePoint, config2.Xyz.WhitePoint);
        AssertNotEqual(config1, config2);
        AssertNotEqual(config1.Rgb, config2.Rgb);
        AssertNotEqual(config1.Xyz, config2.Xyz);
        AssertNotEqual(config1.Cam, config2.Cam);
    }
    
    [Test]
    public void DifferentColourHeritageObjects()
    {
        var heritages = new List<ColourHeritage> { ColourHeritage.None, ColourHeritage.NaN, ColourHeritage.Greyscale };
        foreach (var heritage in heritages)
        {
            foreach (var otherHeritage in heritages.Except(new[] { heritage }))
            {
                AssertNotEqual(heritage, otherHeritage);
            }
        }
    }
    
    private static ColourTriplet GetDifferent(ColourTriplet triplet, double diff = 0.1) => new(triplet.First + diff, triplet.Second + diff, triplet.Third + diff);

    private static void AssertUnicoloursEqual(Unicolour unicolour1, Unicolour unicolour2)
    {
        AssertEqual(unicolour1.Rgb, unicolour2.Rgb);
        AssertEqual(unicolour1.Rgb.Byte255, unicolour2.Rgb.Byte255);
        AssertEqual(unicolour1.RgbLinear, unicolour2.RgbLinear);
        AssertEqual(unicolour1.Hsb, unicolour2.Hsb);
        AssertEqual(unicolour1.Hsl, unicolour2.Hsl);
        AssertEqual(unicolour1.Hwb, unicolour2.Hwb);
        AssertEqual(unicolour1.Xyz, unicolour2.Xyz);
        AssertEqual(unicolour1.Xyy, unicolour2.Xyy);
        AssertEqual(unicolour1.Lab, unicolour2.Lab);
        AssertEqual(unicolour1.Lchab, unicolour2.Lchab);
        AssertEqual(unicolour1.Luv, unicolour2.Luv);
        AssertEqual(unicolour1.Lchuv, unicolour2.Lchuv);
        AssertEqual(unicolour1.Hsluv, unicolour2.Hsluv);
        AssertEqual(unicolour1.Hpluv, unicolour2.Hpluv);
        AssertEqual(unicolour1.Ictcp, unicolour2.Ictcp);
        AssertEqual(unicolour1.Jzazbz, unicolour2.Jzazbz);
        AssertEqual(unicolour1.Jzczhz, unicolour2.Jzczhz);
        AssertEqual(unicolour1.Oklab, unicolour2.Oklab);
        AssertEqual(unicolour1.Oklch, unicolour2.Oklch);
        AssertEqual(unicolour1.Cam02, unicolour2.Cam02);
        AssertEqual(unicolour1.Cam16, unicolour2.Cam16);
        AssertEqual(unicolour1.Hct, unicolour2.Hct);
        AssertEqual(unicolour1.Alpha, unicolour2.Alpha);
        AssertEqual(unicolour1.Hex, unicolour2.Hex);
        AssertEqual(unicolour1.IsInDisplayGamut, unicolour2.IsInDisplayGamut);
        AssertEqual(unicolour1.RelativeLuminance, unicolour2.RelativeLuminance);
        AssertEqual(unicolour1.Description, unicolour2.Description);
        AssertEqual(unicolour1.Temperature, unicolour2.Temperature);
        AssertEqual(unicolour1, unicolour2);

        if (unicolour1.Xyz.HctToXyzSearchResult != null)
        {
            AssertEqual(unicolour1.Xyz.HctToXyzSearchResult, unicolour2.Xyz.HctToXyzSearchResult);
        }
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