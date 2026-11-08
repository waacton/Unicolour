using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class EqualityTests
{
    [Test]
    public void UnicolourVsSameRef([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = Rng.Unicolour(colourSpace, TestUtils.DefaultFogra39Config);
        var colour2 = colour1;
        AssertUnicoloursEqual(colour1, colour2);
    }
    
    [Test]
    public void UnicolourVsSameRefNotNumber([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = new Unicolour(TestUtils.DefaultFogra39Config, colourSpace, double.NaN, double.NaN, double.NaN);
        var colour2 = colour1;
        AssertUnicoloursEqual(colour1, colour2);
    }
    
    [Test]
    public void UnicolourVsSameVal([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = Rng.Unicolour(colourSpace, TestUtils.DefaultFogra39Config);
        var colour2 = new Unicolour(TestUtils.DefaultFogra39Config, colourSpace, colour1.GetTriplet(colourSpace).Tuple, colour1.Alpha.A);
        AssertUnicoloursEqual(colour1, colour2);
    }
    
    [Test]
    public void UnicolourVsWrongType([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour = Rng.Unicolour(colourSpace);
        var notUnicolour = new object();
        AssertNotEqual(colour, notUnicolour);
    }
    
    [Test]
    public void UnicolourVsNullValue([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour = Rng.Unicolour(colourSpace);
        Assert.That(colour.Equals(null), Is.False);
    }
    
    [Test]
    public void UnicolourVsNullObject([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour = Rng.Unicolour(colourSpace);
        Assert.That(colour.Equals(null as object), Is.False);
    }
    
    [Test]
    public void UnicolourVsDifferentTriplet([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = Rng.Unicolour(colourSpace, TestUtils.DefaultFogra39Config);
        var difference = colourSpace is ColourSpace.Rgb255 or ColourSpace.Ycbcr ? 3 : 0.1;
        var differentTuple = GetDifferent(colour1.GetTriplet(colourSpace), difference).Tuple;
        var colour2 = new Unicolour(TestUtils.DefaultFogra39Config, colourSpace, differentTuple, colour1.Alpha.A);
        AssertNotEqual(colour1.GetTriplet(colourSpace), colour2.GetTriplet(colourSpace));
        AssertEqual(colour1.Alpha, colour2.Alpha);
        AssertEqual(colour1.Configuration, colour2.Configuration);
        AssertNotEqual(colour1, colour2);
    }
    
    [Test]
    public void UnicolourVsDifferentAlpha([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = Rng.Unicolour(colourSpace, TestUtils.DefaultFogra39Config);
        var colour2 = new Unicolour(TestUtils.DefaultFogra39Config, colourSpace, colour1.GetTriplet(colourSpace).Tuple, colour1.Alpha.A + 0.1);
        AssertEqual(colour1.GetTriplet(colourSpace), colour2.GetTriplet(colourSpace));
        AssertNotEqual(colour1.Alpha, colour2.Alpha);
        AssertEqual(colour1.Configuration, colour2.Configuration);
        AssertNotEqual(colour1, colour2);
    }
    
    [Test]
    public void UnicolourVsDifferentConfig([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour1 = Rng.Unicolour(colourSpace, TestUtils.DefaultFogra39Config);
        var colour2 = new Unicolour(TestUtils.D65Config, colourSpace, colour1.GetTriplet(colourSpace).Tuple, colour1.Alpha.A);
        AssertEqual(colour1.GetTriplet(colourSpace), colour2.GetTriplet(colourSpace));
        AssertEqual(colour1.Alpha, colour2.Alpha);
        AssertNotEqual(colour1.Configuration, colour2.Configuration);
        AssertNotEqual(colour1, colour2, expectDifferentString: false); // configs are not shown in the string, and even if they were configs must be same reference to be considered equal
    }
    
    [Test]
    public void TripletVsDifferentDerivedType([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        var colour = new Unicolour(TestUtils.DefaultFogra39Config, colourSpace, 0, 0, 0);
        var otherSpaces = TestUtils.AllColourSpaces.Except([colourSpace]);
        foreach (var otherSpace in otherSpaces)
        {
            var sourceTriplet = colour.GetTriplet(colourSpace);
            var otherTriplet = colour.GetTriplet(otherSpace);
            AssertNotEqual(sourceTriplet, otherTriplet, expectDifferentString: false); // e.g. Rgb(0, 0, 0) and RgbLinear(0, 0, 0) - not equal but same hash code (1252698706) and string ("0.00 0.00 0.00")
        }
    }

    [Test]
    public void ColourBaseVsSameRef()
    {
        ColourBase colourBase1 = new ColourTriplet(0.1, 0.2, 0.3);
        var colourBase2 = colourBase1;
        AssertEqual(colourBase1, colourBase2);
        
        /*
         * an equality check on 2 abstract ColourBase objects that are the same reference never reaches the ColourBase.Equals() override
         * because Equals() is invoked on derived records (e.g. ColourTriplet), which detect reference equality and return true, either because
         * A) the record's compiler-generated Equals() performs a reference check
         * B) the Rider-generated Equal() override performs a reference check (these are mostly kept unchanged from generated)
         * so reflection is used to explicitly trigger ColourBase.Equals()
         */
        Assert.That(CallAbstractEqualsOverride(colourBase1, colourBase2), Is.True);
    }
    
    private static bool CallAbstractEqualsOverride(ColourBase colourBase1, ColourBase colourBase2)
    {
        // cannot simply use methodInfo.Invoke() because that will also just invoke the Equals on the derived record
        // instead, use a pointer to the ColourBase.Equals() method to create an instance of a Func<> delegate, which can indeed be invoked 🪄
        // (arguably overkill to test something that currently can't be hit but ensures equality behaviour won't break in future)
        var methodInfo = typeof(ColourBase).GetMethod("Equals", [typeof(ColourBase)])!;
        var methodPointer = methodInfo.MethodHandle.GetFunctionPointer();
        var colourBase1EqualsMethod = (Func<ColourBase?, bool>)Activator.CreateInstance(typeof(Func<ColourBase?, bool>), colourBase1, methodPointer)!;
        return colourBase1EqualsMethod(colourBase2);
    }
    
    [Test]
    public void ColourBaseVsSameVal()
    {
        ColourBase tripletA = new ColourTriplet(0.1, 0.2, 0.3);
        ColourBase tripletB = new ColourTriplet(0.1, 0.2, 0.3);
        AssertEqual(tripletA, tripletB);
    }
    
    [Test]
    public void ColourBaseVsDifferent()
    {
        ColourBase tripletA = new ColourTriplet(0.1, 0.2, 0.3);
        ColourBase tripletB = new ColourTriplet(0.3, 0.2, 0.1);
        AssertNotEqual(tripletA, tripletB);
    }
    
    [Test]
    public void ColourBaseVsNull()
    {
        ColourBase triplet = new ColourTriplet(0.1, 0.2, 0.3);
        Assert.That(triplet.Equals(null), Is.False);
    }
    
    [Test]
    public void Cam02VsSameRef()
    {
        var camA = new Cam02(new Cam02Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View02);
        var camB = camA;
        AssertEqual(camA, camB);
    }
    
    [Test]
    public void Cam02VsSameVal()
    {
        var camA = new Cam02(new Cam02Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View02);
        var camB = new Cam02(new Cam02Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View02);
        AssertEqual(camA, camB);
    }
    
    [Test]
    public void Cam02VsDifferent()
    {
        var camA = new Cam02(new Cam02Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View02);
        var camB = new Cam02(new Cam02Model(0.9, 0.8, 0.7, 0.6, 0.5, 0.4), TestUtils.DefaultFogra39Config.Cam.View02);
        AssertNotEqual(camA, camB);
    }
    
    [Test]
    public void Cam02VsNull()
    {
        var cam = new Cam02(new Cam02Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View02);
        Assert.That(cam.Equals(null), Is.False);
    }
    
    [Test]
    public void Cam16VsSameRef()
    {
        var camA = new Cam16(new Cam16Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View16);
        var camB = camA;
        AssertEqual(camA, camB);
    }
    
    [Test]
    public void Cam16VsSameVal()
    {
        var camA = new Cam16(new Cam16Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View16);
        var camB = new Cam16(new Cam16Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View16);
        AssertEqual(camA, camB);
    }
    
    [Test]
    public void Cam16VsDifferent()
    {
        var camA = new Cam16(new Cam16Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View16);
        var camB = new Cam16(new Cam16Model(0.9, 0.8, 0.7, 0.6, 0.5, 0.4), TestUtils.DefaultFogra39Config.Cam.View16);
        AssertNotEqual(camA, camB);
    }
    
    [Test]
    public void Cam16VsNull()
    {
        var cam = new Cam16(new Cam16Model(0.1, 0.2, 0.3, 0.4, 0.5, 0.6), TestUtils.DefaultFogra39Config.Cam.View16);
        Assert.That(cam.Equals(null), Is.False);
    }
    
    [Test]
    public void IccChannelsVsSameRef()
    {
        var channels1 = new Channels(0.25, 0.5, 0.5, 0.75);
        var channels2 = channels1;
        AssertEqual(channels1, channels2);
    }
    
    [Test]
    public void IccChannelsVsSameVal()
    {
        var channels1 = new Channels(0.25, 0.5, 0.5, 0.75);
        var channels2 = new Channels(0.25, 0.5, 0.5, 0.75);
        AssertEqual(channels1, channels2);
    }

    [Test]
    public void IccChannelsVsDifferent()
    {
        var channels1 = new Channels(0.25, 0.5, 0.5, 0.75);
        var channels2 = new Channels(0.75, 0.5, 0.5, 0.25);
        AssertNotEqual(channels1, channels2);
    }
    
    [Test]
    public void IccChannelsVsNull()
    {
        var channels = new Channels(0.25, 0.5, 0.5, 0.75);
        Assert.That(channels.Equals(null), Is.False);
    }
    
    [Test]
    public void ConfigVsSameRef()
    {
        var config1 = TestUtils.D65Config;
        var config2 = config1;
        AssertEqual(config1, config2);
    }
    
    [Test]
    public void ConfigVsSameVal()
    {
        // configs are considered too complex to attempt to test for equality by value
        // and by only checking reference equality, Unicolour may attempt to convert between 2 configs of the same value
        // but this is a conscious decision and far preferable to not converting between them when they should be
        var config1 = TestUtils.D65Config;
        var config2 = new Configuration(xyzConfig: XyzConfiguration.D65);
        AssertNotEqual(config1, config2);
    }
    
    [Test]
    public void ConfigVsDifferent()
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
    
    [Test]
    public void ConfigVsNull()
    {
        var config = TestUtils.D65Config;
        Assert.That(config.Equals(null), Is.False);
    }

    private static RgbConfiguration GetNew(RgbConfiguration config, string name) => new(config.ChromaticityR, config.ChromaticityG, config.ChromaticityB, config.WhitePoint, config.FromLinear, config.ToLinear, name);
    private static XyzConfiguration GetNew(XyzConfiguration config, string name) => new(config.Illuminant!, config.Observer, name);
    private static YbrConfiguration GetNew(YbrConfiguration config, string name) => new(config.Kr, config.Kb, config.RangeY, config.RangeC, name);
    private static CamConfiguration GetNew(CamConfiguration config, string name) => new(config.WhitePoint, config.AdaptingLuminance, config.BackgroundLuminance, config.Surround, name);
    private static DynamicRange GetNew(DynamicRange config, string name) => new(config.WhiteLuminance, config.MaxLuminance, config.MinLuminance, config.HlgWhiteLevel, name);
    private static IccConfiguration GetNew(IccConfiguration config, string name) => config.Profile == null ? new(config.Profile, name) : new(config.Profile, config.Intent, name);
    private static ColourTriplet GetDifferent(ColourTriplet triplet, double diff = 0.1) => new(triplet.First + diff, triplet.Second + diff, triplet.Third + diff);

    private static void AssertUnicoloursEqual(Unicolour colour1, Unicolour colour2)
    {
        AssertEqual(colour1.Alpha, colour2.Alpha);
        AssertEqual(colour1.Cam02, colour2.Cam02);
        AssertEqual(colour1.Cam02Jch, colour2.Cam02Jch);
        AssertEqual(colour1.Cam02Jmh, colour2.Cam02Jmh);
        AssertEqual(colour1.Cam02Jsh, colour2.Cam02Jsh);
        AssertEqual(colour1.Cam02Qch, colour2.Cam02Qch);
        AssertEqual(colour1.Cam02Qmh, colour2.Cam02Qmh);
        AssertEqual(colour1.Cam02Qsh, colour2.Cam02Qsh);
        AssertEqual(colour1.Cam02Ucs, colour2.Cam02Ucs);
        AssertEqual(colour1.Cam16, colour2.Cam16);
        AssertEqual(colour1.Cam16Jch, colour2.Cam16Jch);
        AssertEqual(colour1.Cam16Jmh, colour2.Cam16Jmh);
        AssertEqual(colour1.Cam16Jsh, colour2.Cam16Jsh);
        AssertEqual(colour1.Cam16Qch, colour2.Cam16Qch);
        AssertEqual(colour1.Cam16Qmh, colour2.Cam16Qmh);
        AssertEqual(colour1.Cam16Qsh, colour2.Cam16Qsh);
        AssertEqual(colour1.Cam16Ucs, colour2.Cam16Ucs);
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
        AssertEqual(colour1.Okhsl, colour2.Okhsl);
        AssertEqual(colour1.Okhsv, colour2.Okhsv);
        AssertEqual(colour1.Okhwb, colour2.Okhwb);
        AssertEqual(colour1.Oklab, colour2.Oklab);
        AssertEqual(colour1.Oklch, colour2.Oklch);
        AssertEqual(colour1.Oklrab, colour2.Oklrab);
        AssertEqual(colour1.Oklrch, colour2.Oklrch);
        AssertEqual(colour1.RelativeLuminance, colour2.RelativeLuminance);
        AssertEqual(colour1.Rgb, colour2.Rgb);
        AssertEqual(colour1.Rgb255, colour2.Rgb255);
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
        
        if (colour1.Munsell.XyyToHvcSearchResult != null)
        {
            AssertEqual(colour1.Munsell.XyyToHvcSearchResult, colour2.Munsell.XyyToHvcSearchResult);
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
        AssertEqual(config1.Xyz, config2.Xyz);
        AssertEqual(config1.Xyz.WhitePoint, config2.Xyz.WhitePoint);
        AssertEqual(config1.Xyz.Illuminant, config2.Xyz.Illuminant);
        AssertEqual(config1.Xyz.Observer, config2.Xyz.Observer);
        AssertEqual(config1.Xyz.ChromaticAdaptation, config2.Xyz.ChromaticAdaptation);
        AssertEqual(config1.Xyz.ChromaticAdaptor, config2.Xyz.ChromaticAdaptor);
        AssertEqual(config1.Xyz.SpectralBoundary, config2.Xyz.SpectralBoundary);
        AssertEqual(config1.Xyz.Planckian, config2.Xyz.Planckian);
        AssertEqual(config1.Ybr, config2.Ybr);
        AssertEqual(config1.Ybr.Kr, config2.Ybr.Kr);
        AssertEqual(config1.Ybr.Kg, config2.Ybr.Kg);
        AssertEqual(config1.Ybr.Kb, config2.Ybr.Kb);
        AssertEqual(config1.Ybr.RangeY, config2.Ybr.RangeY);
        AssertEqual(config1.Ybr.RangeC, config2.Ybr.RangeC);
        AssertEqual(config1.Cam, config2.Cam);
        AssertEqual(config1.Cam.WhitePoint, config2.Cam.WhitePoint);
        AssertEqual(config1.Cam.AdaptingLuminance, config2.Cam.AdaptingLuminance);
        AssertEqual(config1.Cam.BackgroundLuminance, config2.Cam.BackgroundLuminance);
        AssertEqual(config1.Cam.Surround, config2.Cam.Surround);
        AssertEqual(config1.Cam.View02, config2.Cam.View02);
        AssertEqual(config1.DynamicRange, config2.DynamicRange);
        AssertEqual(config1.DynamicRange.WhiteLuminance, config2.DynamicRange.WhiteLuminance);
        AssertEqual(config1.DynamicRange.MaxLuminance, config2.DynamicRange.MaxLuminance);
        AssertEqual(config1.DynamicRange.MinLuminance, config2.DynamicRange.MinLuminance);
        AssertEqual(config1.Icc, config2.Icc);
        AssertEqual(config1.Icc.Profile, config2.Icc.Profile);
        AssertEqual(config1.Icc.Intent, config2.Icc.Intent);
        AssertEqual(config1.Icc.Error, config2.Icc.Error);
    }

    private static void AssertEqual<T>(T object1, T object2) => TestUtils.AssertEqual(object1, object2);
    private static void AssertNotEqual<T>(T object1, T object2, bool expectDifferentString = true) => TestUtils.AssertNotEqual(object1, object2, expectDifferentString);
}