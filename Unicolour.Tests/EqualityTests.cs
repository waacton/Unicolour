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
        var iccConfigWithProfile = new IccConfiguration(IccFile.Fogra39.Path, Intent.Perceptual);
        var iccConfigWithoutProfile = new IccConfiguration(null);
        IccConfiguration[] iccConfigs = [iccConfigWithProfile, iccConfigWithoutProfile];

        foreach (var iccConfig in iccConfigs)
        {
            var rgbConfig1 = GetNew(Configuration.Default.Rgb, "RGB 1");
            var xyzConfig1 = GetNew(Configuration.Default.Xyz, "XYZ 1");
            var ybrConfig1 = GetNew(Configuration.Default.Ybr, "YBR 1");
            var camConfig1 = GetNew(Configuration.Default.Cam, "CAM 1");
            var dynamicRange1 = GetNew(Configuration.Default.DynamicRange, "DR 1");
            var iccConfig1 = GetNew(iccConfig, "ICC 1");
            var config1 = new Configuration(rgbConfig1, xyzConfig1, ybrConfig1, camConfig1, dynamicRange1, iccConfig1);
        
            var rgbConfig2 = GetNew(Configuration.Default.Rgb, "RGB 2");
            var xyzConfig2 = GetNew(Configuration.Default.Xyz, "XYZ 2");
            var ybrConfig2 = GetNew(Configuration.Default.Ybr, "YBR 2");
            var camConfig2 = GetNew(Configuration.Default.Cam, "CAM 2");
            var dynamicRange2 = GetNew(Configuration.Default.DynamicRange, "DR 2");
            var iccConfig2 = GetNew(iccConfig, "ICC 2");
            var config2 = new Configuration(rgbConfig2, xyzConfig2, ybrConfig2, camConfig2, dynamicRange2, iccConfig2);
        
            // configs have same values but are different references
            AssertNotEqual(config1, config2);
            AssertNotEqual(config1.Rgb, config2.Rgb);
            AssertNotEqual(config1.Xyz, config2.Xyz);
            AssertNotEqual(config1.Ybr, config2.Ybr);
            AssertNotEqual(config1.Cam, config2.Cam);
            AssertNotEqual(config1.DynamicRange, config2.DynamicRange);
            AssertNotEqual(config1.Icc, config2.Icc);
        }
    }

    private static RgbConfiguration GetNew(RgbConfiguration config, string name) => new(config.ChromaticityR, config.ChromaticityG, config.ChromaticityB, config.WhitePoint, config.FromLinear, config.ToLinear, name);
    private static XyzConfiguration GetNew(XyzConfiguration config, string name) => new(config.Illuminant!, config.Observer, name);
    private static YbrConfiguration GetNew(YbrConfiguration config, string name) => new(config.Kr, config.Kb, config.RangeY, config.RangeC, name);
    private static CamConfiguration GetNew(CamConfiguration config, string name) => new(config.WhitePoint, config.AdaptingLuminance, config.BackgroundLuminance, config.Surround, name);
    private static DynamicRange GetNew(DynamicRange config, string name) => new(config.WhiteLuminance, config.MaxLuminance, config.MinLuminance, config.HlgWhiteLevel, name);
    private static IccConfiguration GetNew(IccConfiguration config, string name) => config.Profile == null ? new(config.Profile, name) : new(config.Profile, config.Intent, name);
    
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
        AssertEqual(colour1.IsInMacAdamLimits, colour2.IsInMacAdamLimits);
        AssertEqual(colour1.IsInPointerGamut, colour2.IsInPointerGamut);
        AssertEqual(colour1.IsInRgbGamut, colour2.IsInRgbGamut);
        AssertEqual(colour1.Jzazbz, colour2.Jzazbz);
        AssertEqual(colour1.Jzczhz, colour2.Jzczhz);
        AssertEqual(colour1.Lab, colour2.Lab);
        AssertEqual(colour1.Lchab, colour2.Lchab);
        AssertEqual(colour1.Lchuv, colour2.Lchuv);
        AssertEqual(colour1.Lms, colour2.Lms);
        AssertEqual(colour1.Luv, colour2.Luv);
        AssertEqual(colour1.Munsell, colour2.Munsell);
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
        
        if (colour1.Munsell.XyyToMunsellSearchResult != null)
        {
            AssertEqual(colour1.Munsell.XyyToMunsellSearchResult, colour2.Munsell.XyyToMunsellSearchResult);
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
        AssertEqual(config1.Rgb.FromLinear, config2.Rgb.FromLinear);
        AssertEqual(config1.Rgb.ToLinear, config2.Rgb.ToLinear);
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
        AssertEqual(config1.DynamicRange.WhiteLuminance, config2.DynamicRange.WhiteLuminance);
        AssertEqual(config1.DynamicRange.MaxLuminance, config2.DynamicRange.MaxLuminance);
        AssertEqual(config1.DynamicRange.MinLuminance, config2.DynamicRange.MinLuminance);
        AssertEqual(config1.Icc.Profile, config2.Icc.Profile);
        AssertEqual(config1.Icc.Intent, config2.Icc.Intent);
        AssertEqual(config1.Icc.Error, config2.Icc.Error);
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