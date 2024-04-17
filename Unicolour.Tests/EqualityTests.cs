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
    public void SameReferenceNotNumber(ColourSpace colourSpace)
    {
        var unicolour1 = new Unicolour(colourSpace, double.NaN, double.NaN, double.NaN);
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
            RgbModels.StandardRgb.R,
            new Chromaticity(0.25, 0.75),
            new Chromaticity(0.5, 0.5),
            new WhitePoint(0.9, 1.0, 1.1),
            RgbModels.StandardRgb.FromLinear,
            RgbModels.StandardRgb.ToLinear,
            "RGB 1");
        var xyzConfig1 = new XyzConfiguration(new WhitePoint(0.95, 1.0, 1.05), "XYZ 1");
        var ybrConfig1 = new YbrConfiguration(0.299, 0.114, (16, 235), (16, 240), "YBR 1");
        var camConfig1 = new CamConfiguration(new WhitePoint(0.9, 1.0, 1.1), 4, 20, Surround.Dark, "CAM 1");
        var config1 = new Configuration(rgbConfig1, xyzConfig1, ybrConfig1, camConfig1);
        
        var rgbConfig2 = new RgbConfiguration(
            RgbModels.StandardRgb.R,
            new Chromaticity(0.75, 0.25),
            new Chromaticity(0.5, 0.5), 
            new WhitePoint(0.9, 1.0, 1.1),
            RgbModels.StandardRgb.FromLinear,
            RgbModels.StandardRgb.ToLinear,
            "RGB 2");
        var xyzConfig2 = new XyzConfiguration(new WhitePoint(0.95001, 1.0001, 1.05001), "XYZ 2");
        var ybrConfig2 = new YbrConfiguration(0.300, 0.15, (0, 255), (0, 255), "YBR 2");
        var camConfig2 = new CamConfiguration(new WhitePoint(0.9, 1.0, 1.1), 4, 20, Surround.Dim, "CAM 2");
        var config2 = new Configuration(rgbConfig2, xyzConfig2, ybrConfig2, camConfig2);

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
        AssertNotEqual(config1.Ybr, config2.Ybr);
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
        AssertEqual(unicolour1.Alpha, unicolour2.Alpha);
        AssertEqual(unicolour1.Cam02, unicolour2.Cam02);
        AssertEqual(unicolour1.Cam16, unicolour2.Cam16);
        AssertEqual(unicolour1.Chromaticity, unicolour2.Chromaticity);
        AssertEqual(unicolour1.Description, unicolour2.Description);
        AssertEqual(unicolour1.DominantWavelength, unicolour2.DominantWavelength);
        AssertEqual(unicolour1.ExcitationPurity, unicolour2.ExcitationPurity);
        AssertEqual(unicolour1.Hct, unicolour2.Hct);
        AssertEqual(unicolour1.Hex, unicolour2.Hex);
        AssertEqual(unicolour1.Hpluv, unicolour2.Hpluv);
        AssertEqual(unicolour1.Hsb, unicolour2.Hsb);
        AssertEqual(unicolour1.Hsl, unicolour2.Hsl);
        AssertEqual(unicolour1.Hsluv, unicolour2.Hsluv);
        AssertEqual(unicolour1.Hwb, unicolour2.Hwb);
        AssertEqual(unicolour1.Ipt, unicolour2.Ipt);
        AssertEqual(unicolour1.Ictcp, unicolour2.Ictcp);
        AssertEqual(unicolour1.IsImaginary, unicolour2.IsImaginary);
        AssertEqual(unicolour1.IsInDisplayGamut, unicolour2.IsInDisplayGamut);
        AssertEqual(unicolour1.Jzazbz, unicolour2.Jzazbz);
        AssertEqual(unicolour1.Jzczhz, unicolour2.Jzczhz);
        AssertEqual(unicolour1.Lab, unicolour2.Lab);
        AssertEqual(unicolour1.Lchab, unicolour2.Lchab);
        AssertEqual(unicolour1.Luv, unicolour2.Luv);
        AssertEqual(unicolour1.Lchuv, unicolour2.Lchuv);
        AssertEqual(unicolour1.Oklab, unicolour2.Oklab);
        AssertEqual(unicolour1.Oklch, unicolour2.Oklch);
        AssertEqual(unicolour1.Okhsl, unicolour2.Okhsl);
        AssertEqual(unicolour1.Okhsv, unicolour2.Okhsv);
        AssertEqual(unicolour1.Okhwb, unicolour2.Okhwb);
        AssertEqual(unicolour1.RelativeLuminance, unicolour2.RelativeLuminance);
        AssertEqual(unicolour1.Rgb, unicolour2.Rgb);
        AssertEqual(unicolour1.Rgb.Byte255, unicolour2.Rgb.Byte255);
        AssertEqual(unicolour1.RgbLinear, unicolour2.RgbLinear);
        AssertEqual(unicolour1.Temperature, unicolour2.Temperature);
        AssertEqual(unicolour1.Xyz, unicolour2.Xyz);
        AssertEqual(unicolour1.Xyy, unicolour2.Xyy);
        AssertEqual(unicolour1.Ycbcr, unicolour2.Ycbcr);
        AssertEqual(unicolour1.Ycbcr, unicolour2.Ycbcr);
        AssertEqual(unicolour1.Ycgco, unicolour2.Ycgco);
        AssertEqual(unicolour1.Yiq, unicolour2.Yiq);
        AssertEqual(unicolour1.Ypbpr, unicolour2.Ypbpr);
        AssertEqual(unicolour1.Yuv, unicolour2.Yuv);

        if (unicolour1.Xyz.HctToXyzSearchResult != null)
        {
            AssertEqual(unicolour1.Xyz.HctToXyzSearchResult, unicolour2.Xyz.HctToXyzSearchResult);
        }

        AssertConfigurationEqual(unicolour1.Config, unicolour2.Config);
        AssertEqual(unicolour1, unicolour2);
    }
    
    private static void AssertConfigurationEqual(Configuration config1, Configuration config2)
    {
        AssertEqual(config1, config2);
        AssertEqual(config1.Rgb, config2.Rgb);
        AssertEqual(config1.Rgb.ChromaticityR, config2.Rgb.ChromaticityR);
        AssertEqual(config1.Rgb.ChromaticityG, config2.Rgb.ChromaticityG);
        AssertEqual(config1.Rgb.ChromaticityB, config2.Rgb.ChromaticityB);
        AssertEqual(config1.Rgb.WhitePoint, config2.Rgb.WhitePoint);
        AssertEqual(config1.Rgb.CompandFromLinear, config2.Rgb.CompandFromLinear);
        AssertEqual(config1.Rgb.InverseCompandToLinear, config2.Rgb.InverseCompandToLinear);
        AssertEqual(config1.Xyz.WhitePoint, config2.Xyz.WhitePoint);
        AssertEqual(config1.Xyz.Observer, config2.Xyz.Observer);
        AssertEqual(config1.Xyz.Spectral, config2.Xyz.Spectral);
        AssertEqual(config1.Xyz.Planckian, config2.Xyz.Planckian);
        AssertEqual(config1.Ybr.Kr, config2.Ybr.Kr);
        AssertEqual(config1.Ybr.Kg, config2.Ybr.Kg);
        AssertEqual(config1.Ybr.Kb, config2.Ybr.Kb);
        AssertEqual(config1.Ybr.RangeY, config2.Ybr.RangeY);
        AssertEqual(config1.Ybr.RangeC, config2.Ybr.RangeC);
        AssertEqual(config1.Cam.WhitePoint, config2.Cam.WhitePoint);
        AssertEqual(config1.Cam.AdaptingLuminance, config2.Cam.AdaptingLuminance);
        AssertEqual(config1.Cam.BackgroundLuminance, config2.Cam.BackgroundLuminance);
        AssertEqual(config1.IctcpScalar, config2.IctcpScalar);
        AssertEqual(config1.JzazbzScalar, config2.JzazbzScalar);
    }

    private static void AssertUnicoloursNotEqual(Unicolour unicolour1, Unicolour unicolour2, Func<Unicolour, ColourTriplet> getTriplet)
    {
        AssertNotEqual(getTriplet(unicolour1), getTriplet(unicolour2));
        AssertNotEqual(unicolour1.Alpha, unicolour2.Alpha);
        AssertNotEqual(unicolour1, unicolour2);
    }

    private static void AssertEqual<T>(T object1, T object2) => TestUtils.AssertEqual(object1, object2);
    private static void AssertNotEqual<T>(T object1, T object2) => TestUtils.AssertNotEqual(object1, object2);
}