using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class EqualityTests
{
    [Test]
    public void SameReference([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = RandomColours.UnicolourFrom(colourSpace, TestUtils.DefaultFogra39Config);
        var colour2 = colour1;
        AssertUnicoloursEqual(colour1, colour2);
    }
    
    [Test]
    public void SameReferenceNotNumber([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = new Unicolour(TestUtils.DefaultFogra39Config, colourSpace, double.NaN, double.NaN, double.NaN);
        var colour2 = colour1;
        AssertUnicoloursEqual(colour1, colour2);
    }
    
    [Test]
    public void DifferentType([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        var notUnicolour = new object();
        AssertNotEqual(colour, notUnicolour);
    }
    
    [Test]
    public void NullUnicolour([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        Assert.That(colour.Equals(null), Is.False);
    }
    
    [Test]
    public void NullObject([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        Assert.That(colour.Equals(null as object), Is.False);
    }
    
    [Test]
    public void EqualObjects([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = RandomColours.UnicolourFrom(colourSpace, TestUtils.DefaultFogra39Config);
        var colour2 = new Unicolour(TestUtils.DefaultFogra39Config, colourSpace, colour1.GetRepresentation(colourSpace).Tuple, colour1.Alpha.A);
        AssertUnicoloursEqual(colour1, colour2);
    }
    
    [Test]
    public void NotEqualObjects([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = RandomColours.UnicolourFrom(colourSpace, TestUtils.DefaultFogra39Config);
        var difference = colourSpace == ColourSpace.Rgb255 ? 1 : 0.1;
        var differentTuple = GetDifferent(colour1.GetRepresentation(colourSpace).Triplet, difference).Tuple;
        var colour2 = new Unicolour(TestUtils.DefaultFogra39Config, colourSpace, differentTuple, colour1.Alpha.A + 0.1);
        AssertUnicoloursNotEqual(colour1, colour2, unicolour => unicolour.GetRepresentation(colourSpace).Triplet);
    }

    [Test]
    public void DifferentIccChannels()
    {
        var channels1 = new Channels(0.25, 0.5, 0.5, 0.75);
        var channels2 = new Channels(0.75, 0.5, 0.5, 0.25);
        AssertNotEqual(channels1, channels2);
    }
    
    [Test]
    public void NullIccChannels()
    {
        var channels = new Channels(0.25, 0.5, 0.5, 0.75);
        Assert.That(channels.Equals(null), Is.False);
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
        var iccConfig1 = new IccConfiguration(IccFile.Fogra39.Path, Intent.Perceptual, "ICC 1");
        var config1 = new Configuration(rgbConfig1, xyzConfig1, ybrConfig1, camConfig1, iccConfig1);
        
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
        var iccConfig2 = new IccConfiguration(IccFile.Swop2006.Path, Intent.Saturation, "ICC 2");
        var config2 = new Configuration(rgbConfig2, xyzConfig2, ybrConfig2, camConfig2, iccConfig2);

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
        AssertNotEqual(config1.Icc, config2.Icc);
    }
    
    [Test]
    public void DifferentColourHeritageObjects()
    {
        var heritages = new List<ColourHeritage> { ColourHeritage.None, ColourHeritage.NaN, ColourHeritage.Greyscale };
        foreach (var heritage in heritages)
        {
            foreach (var otherHeritage in heritages.Except([heritage]))
            {
                AssertNotEqual(heritage, otherHeritage);
            }
        }
    }
    
    private static ColourTriplet GetDifferent(ColourTriplet triplet, double diff = 0.1) => new(triplet.First + diff, triplet.Second + diff, triplet.Third + diff);

    private static void AssertUnicoloursEqual(Unicolour colour1, Unicolour colour2)
    {
        AssertEqual(colour1.Alpha, colour2.Alpha);
        AssertEqual(colour1.Cam02, colour2.Cam02);
        AssertEqual(colour1.Cam16, colour2.Cam16);
        AssertEqual(colour1.Chromaticity, colour2.Chromaticity);
        AssertEqual(colour1.Description, colour2.Description);
        AssertEqual(colour1.DominantWavelength, colour2.DominantWavelength);
        AssertEqual(colour1.ExcitationPurity, colour2.ExcitationPurity);
        AssertEqual(colour1.Hct, colour2.Hct);
        AssertEqual(colour1.Hex, colour2.Hex);
        AssertEqual(colour1.Hpluv, colour2.Hpluv);
        AssertEqual(colour1.Hsb, colour2.Hsb);
        AssertEqual(colour1.Hsi, colour2.Hsi);
        AssertEqual(colour1.Hsl, colour2.Hsl);
        AssertEqual(colour1.Hsluv, colour2.Hsluv);
        AssertEqual(colour1.Hwb, colour2.Hwb);
        AssertEqual(colour1.Icc, colour2.Icc);
        AssertEqual(colour1.Ictcp, colour2.Ictcp);
        AssertEqual(colour1.Ipt, colour2.Ipt);
        AssertEqual(colour1.IsImaginary, colour2.IsImaginary);
        AssertEqual(colour1.IsInRgbGamut, colour2.IsInRgbGamut);
        AssertEqual(colour1.Jzazbz, colour2.Jzazbz);
        AssertEqual(colour1.Jzczhz, colour2.Jzczhz);
        AssertEqual(colour1.Lab, colour2.Lab);
        AssertEqual(colour1.Lchab, colour2.Lchab);
        AssertEqual(colour1.Luv, colour2.Luv);
        AssertEqual(colour1.Lchuv, colour2.Lchuv);
        AssertEqual(colour1.Oklab, colour2.Oklab);
        AssertEqual(colour1.Oklch, colour2.Oklch);
        AssertEqual(colour1.Okhsl, colour2.Okhsl);
        AssertEqual(colour1.Okhsv, colour2.Okhsv);
        AssertEqual(colour1.Okhwb, colour2.Okhwb);
        AssertEqual(colour1.RelativeLuminance, colour2.RelativeLuminance);
        AssertEqual(colour1.Rgb, colour2.Rgb);
        AssertEqual(colour1.Rgb.Byte255, colour2.Rgb.Byte255);
        AssertEqual(colour1.RgbLinear, colour2.RgbLinear);
        AssertEqual(colour1.Temperature, colour2.Temperature);
        AssertEqual(colour1.Tsl, colour2.Tsl);
        AssertEqual(colour1.Wxy, colour2.Wxy);
        AssertEqual(colour1.Xyb, colour2.Xyb);
        AssertEqual(colour1.Xyy, colour2.Xyy);
        AssertEqual(colour1.Xyz, colour2.Xyz);
        AssertEqual(colour1.Ycbcr, colour2.Ycbcr);
        AssertEqual(colour1.Ycbcr, colour2.Ycbcr);
        AssertEqual(colour1.Ycgco, colour2.Ycgco);
        AssertEqual(colour1.Yiq, colour2.Yiq);
        AssertEqual(colour1.Ypbpr, colour2.Ypbpr);
        AssertEqual(colour1.Yuv, colour2.Yuv);

        if (colour1.Xyz.HctToXyzSearchResult != null)
        {
            AssertEqual(colour1.Xyz.HctToXyzSearchResult, colour2.Xyz.HctToXyzSearchResult);
        }

        AssertConfigurationEqual(colour1.Configuration, colour2.Configuration);
        AssertEqual(colour1, colour2);
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
        AssertEqual(config1.Xyz.SpectralBoundary, config2.Xyz.SpectralBoundary);
        AssertEqual(config1.Xyz.Planckian, config2.Xyz.Planckian);
        AssertEqual(config1.Ybr.Kr, config2.Ybr.Kr);
        AssertEqual(config1.Ybr.Kg, config2.Ybr.Kg);
        AssertEqual(config1.Ybr.Kb, config2.Ybr.Kb);
        AssertEqual(config1.Ybr.RangeY, config2.Ybr.RangeY);
        AssertEqual(config1.Ybr.RangeC, config2.Ybr.RangeC);
        AssertEqual(config1.Cam.WhitePoint, config2.Cam.WhitePoint);
        AssertEqual(config1.Cam.AdaptingLuminance, config2.Cam.AdaptingLuminance);
        AssertEqual(config1.Cam.BackgroundLuminance, config2.Cam.BackgroundLuminance);
        AssertEqual(config1.Cam.Surround, config2.Cam.Surround);
        AssertEqual(config1.Icc.Profile, config2.Icc.Profile);
        AssertEqual(config1.Icc.Intent, config2.Icc.Intent);
        AssertEqual(config1.Icc.Error, config2.Icc.Error);
        AssertEqual(config1.IctcpScalar, config2.IctcpScalar);
        AssertEqual(config1.JzazbzScalar, config2.JzazbzScalar);
    }

    private static void AssertUnicoloursNotEqual(Unicolour colour1, Unicolour colour2, Func<Unicolour, ColourTriplet> getTriplet)
    {
        AssertNotEqual(getTriplet(colour1), getTriplet(colour2));
        AssertNotEqual(colour1.Alpha, colour2.Alpha);
        AssertNotEqual(colour1, colour2);
    }

    private static void AssertEqual<T>(T object1, T object2) => TestUtils.AssertEqual(object1, object2);
    private static void AssertNotEqual<T>(T object1, T object2) => TestUtils.AssertNotEqual(object1, object2);
}