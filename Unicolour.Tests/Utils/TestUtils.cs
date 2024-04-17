namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

internal static class TestUtils
{
    // generating planckian tables is expensive, but this is the set of tables needed for most temperature tests
    internal static readonly Planckian PlanckianObserverDegree2 = new(Observer.Degree2);

    internal static List<ColourSpace> AllColourSpaces => Enum.GetValues<ColourSpace>().ToList();
    internal static readonly List<TestCaseData> AllColourSpacesTestCases = new()
    {
        new TestCaseData(ColourSpace.Rgb),
        new TestCaseData(ColourSpace.RgbLinear),
        new TestCaseData(ColourSpace.Hsb),
        new TestCaseData(ColourSpace.Hsl),
        new TestCaseData(ColourSpace.Hwb),
        new TestCaseData(ColourSpace.Xyz),
        new TestCaseData(ColourSpace.Xyy),
        new TestCaseData(ColourSpace.Lab),
        new TestCaseData(ColourSpace.Lchab),
        new TestCaseData(ColourSpace.Luv),
        new TestCaseData(ColourSpace.Lchuv),
        new TestCaseData(ColourSpace.Hsluv),
        new TestCaseData(ColourSpace.Hpluv),
        new TestCaseData(ColourSpace.Ypbpr),
        new TestCaseData(ColourSpace.Ycbcr),
        new TestCaseData(ColourSpace.Ycgco),
        new TestCaseData(ColourSpace.Yuv),
        new TestCaseData(ColourSpace.Yiq),
        new TestCaseData(ColourSpace.Ydbdr),
        new TestCaseData(ColourSpace.Ipt),
        new TestCaseData(ColourSpace.Ictcp),
        new TestCaseData(ColourSpace.Jzazbz),
        new TestCaseData(ColourSpace.Jzczhz),
        new TestCaseData(ColourSpace.Oklab),
        new TestCaseData(ColourSpace.Oklch),
        new TestCaseData(ColourSpace.Okhsl),
        new TestCaseData(ColourSpace.Okhsv),
        new TestCaseData(ColourSpace.Okhwb),
        new TestCaseData(ColourSpace.Cam02),
        new TestCaseData(ColourSpace.Cam16),
        new TestCaseData(ColourSpace.Hct)
    };
    
    internal static readonly List<TestCaseData> AllIlluminantsTestCases = new()
    {
        new TestCaseData(Illuminant.A),
        new TestCaseData(Illuminant.C),
        new TestCaseData(Illuminant.D50),
        new TestCaseData(Illuminant.D55),
        new TestCaseData(Illuminant.D65),
        new TestCaseData(Illuminant.D75),
        new TestCaseData(Illuminant.E),
        new TestCaseData(Illuminant.F2),
        new TestCaseData(Illuminant.F7),
        new TestCaseData(Illuminant.F11)
    };

    internal static readonly Dictionary<string, Illuminant> Illuminants = new()
    {
        { nameof(Illuminant.A), Illuminant.A },
        { nameof(Illuminant.C), Illuminant.C },
        { nameof(Illuminant.D50), Illuminant.D50 },
        { nameof(Illuminant.D55), Illuminant.D55 },
        { nameof(Illuminant.D65), Illuminant.D65 },
        { nameof(Illuminant.D75), Illuminant.D75 },
        { nameof(Illuminant.E), Illuminant.E },
        { nameof(Illuminant.F2), Illuminant.F2 },
        { nameof(Illuminant.F7), Illuminant.F7 },
        { nameof(Illuminant.F11), Illuminant.F11 }
    };
    
    internal static readonly Dictionary<string, Observer> Observers = new()
    {
        { nameof(Observer.Degree2), Observer.Degree2 },
        { nameof(Observer.Degree10), Observer.Degree10 }
    };
    
    internal static readonly List<RgbConfiguration> NonDefaultRgbConfigs = new()
    {
        RgbConfiguration.DisplayP3,
        RgbConfiguration.Rec2020,
        RgbConfiguration.A98,
        RgbConfiguration.ProPhoto,
        RgbConfiguration.XvYcc,
        RgbConfiguration.Aces20651,
        RgbConfiguration.Acescg,
        RgbConfiguration.Acescct,
        RgbConfiguration.Acescc,
        RgbConfiguration.Rec601Line625,
        RgbConfiguration.Rec601Line525,
        RgbConfiguration.Rec709,
        RgbConfiguration.Pal,
        RgbConfiguration.PalM,
        RgbConfiguration.Pal625,
        RgbConfiguration.Pal525,
        RgbConfiguration.Ntsc,
        RgbConfiguration.NtscSmpteC,
        RgbConfiguration.Ntsc525,
        RgbConfiguration.Secam,
        RgbConfiguration.Secam625
    };
    
    internal static readonly List<YbrConfiguration> NonDefaultYbrConfigs = new()
    {
        YbrConfiguration.Rec709,
        YbrConfiguration.Rec2020,
        YbrConfiguration.Jpeg
    };
    
    internal static List<double> ExtremeDoubles = new() { double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN };
        
    internal const double MixTolerance = 0.00000000005;

    internal static void AssertTriplet(ColourTriplet actual, ColourTriplet expected, double tolerance, string? info = null)
    {
        var details = $"Expected --- {expected}\nActual ----- {actual}";
        string FailMessage(string channel) => $"{(info == null ? string.Empty : $"{info} · ")}{channel}\n{details}";
        AssertTripletValue(actual.First, expected.First, tolerance, FailMessage("Channel 1"), actual.HueIndex == 0);
        AssertTripletValue(actual.Second, expected.Second, tolerance, FailMessage("Channel 2"));
        AssertTripletValue(actual.Third, expected.Third, tolerance, FailMessage("Channel 3"), actual.HueIndex == 2);
    }

    internal static void AssertTriplet<T>(Unicolour unicolour, ColourTriplet expected, double tolerance) where T : ColourRepresentation
    {
        var colourSpace = RepresentationTypeToColourSpace[typeof(T)];
        var colourRepresentation = unicolour.GetRepresentation(colourSpace);
        AssertTriplet(colourRepresentation.Triplet, expected, tolerance);
    }

    private static void AssertTripletValue(double actual, double expected, double tolerance, string failMessage, bool isHue = false)
    {
        if (!isHue) Assert.That(actual, Is.EqualTo(expected).Within(tolerance), failMessage);
        else AssertNormalisedForHue(actual, expected, tolerance, failMessage);
    }

    private static void AssertNormalisedForHue(double actualHue, double expectedHue, double tolerance, string failMessage)
    {
        double Normalise(double value) => value / 360.0;
        var actual = Normalise(actualHue);
        var expected = Normalise(expectedHue);
        var expectedPlus360 = Normalise(expectedHue + 360);
        var expectedMinus360 = Normalise(expectedHue - 360);
        Assert.That(actual, 
            Is.EqualTo(expected).Within(tolerance)
            .Or.EqualTo(expectedPlus360).Within(tolerance)
            .Or.EqualTo(expectedMinus360).Within(tolerance), 
            failMessage);
    }
    
    internal static void AssertMixed(ColourTriplet triplet, double alpha, (double first, double second, double third, double alpha) expected)
    {
        Assert.That(triplet.First, Is.EqualTo(expected.first).Within(MixTolerance), "First");
        Assert.That(triplet.Second, Is.EqualTo(expected.second).Within(MixTolerance), "Second");
        Assert.That(triplet.Third, Is.EqualTo(expected.third).Within(MixTolerance), "Third");
        Assert.That(alpha, Is.EqualTo(expected.alpha).Within(MixTolerance), "Alpha");
    }

    internal static void AssertNoPropertyError(Unicolour unicolour)
    {
        Assert.DoesNotThrow(AccessProperties);
        return;

        void AccessProperties()
        {
            AccessProperty(() => unicolour.Alpha);
            AccessProperty(() => unicolour.Cam02);
            AccessProperty(() => unicolour.Cam16);
            AccessProperty(() => unicolour.Chromaticity);
            AccessProperty(() => unicolour.Config);
            AccessProperty(() => unicolour.Description);
            AccessProperty(() => unicolour.DominantWavelength);
            AccessProperty(() => unicolour.ExcitationPurity);
            AccessProperty(() => unicolour.Hct);
            AccessProperty(() => unicolour.Hex);
            AccessProperty(() => unicolour.Hpluv);
            AccessProperty(() => unicolour.Hsb);
            AccessProperty(() => unicolour.Hsl);
            AccessProperty(() => unicolour.Hsluv);
            AccessProperty(() => unicolour.Hwb);
            AccessProperty(() => unicolour.Ictcp);
            AccessProperty(() => unicolour.IsImaginary);
            AccessProperty(() => unicolour.IsInDisplayGamut);
            AccessProperty(() => unicolour.Ipt);
            AccessProperty(() => unicolour.Jzazbz);
            AccessProperty(() => unicolour.Jzczhz);
            AccessProperty(() => unicolour.Lab);
            AccessProperty(() => unicolour.Lchab);
            AccessProperty(() => unicolour.Lchuv);
            AccessProperty(() => unicolour.Luv);
            AccessProperty(() => unicolour.Oklab);
            AccessProperty(() => unicolour.Oklch);
            AccessProperty(() => unicolour.Okhsl);
            AccessProperty(() => unicolour.Okhsv);
            AccessProperty(() => unicolour.Okhwb);
            AccessProperty(() => unicolour.RelativeLuminance);
            AccessProperty(() => unicolour.Rgb);
            AccessProperty(() => unicolour.Rgb.Byte255);
            AccessProperty(() => unicolour.RgbLinear);
            AccessProperty(() => unicolour.Temperature);
            AccessProperty(() => unicolour.Xyy);
            AccessProperty(() => unicolour.Xyz);
            AccessProperty(() => unicolour.Ypbpr);
            AccessProperty(() => unicolour.Ycbcr);
            AccessProperty(() => unicolour.Ycgco);
            AccessProperty(() => unicolour.Yuv);
            AccessProperty(() => unicolour.Yiq);
            AccessProperty(() => unicolour.Ydbdr);
        }
        
        void AccessProperty(Func<object> getProperty)
        {
            _ = getProperty();
        }
    }
    
    internal static void AssertEqual<T>(T object1, T object2)
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

    internal static void AssertNotEqual<T>(T object1, T object2)
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
    
    private static readonly Dictionary<Type, ColourSpace> RepresentationTypeToColourSpace = new()
    {
        { typeof(Rgb), ColourSpace.Rgb },
        { typeof(Rgb255), ColourSpace.Rgb255 },
        { typeof(RgbLinear), ColourSpace.RgbLinear },
        { typeof(Hsb), ColourSpace.Hsb },
        { typeof(Hsl), ColourSpace.Hsl },
        { typeof(Hwb), ColourSpace.Hwb },
        { typeof(Xyz), ColourSpace.Xyz },
        { typeof(Xyy), ColourSpace.Xyy },
        { typeof(Lab), ColourSpace.Lab },
        { typeof(Lchab), ColourSpace.Lchab },
        { typeof(Luv), ColourSpace.Luv },
        { typeof(Lchuv), ColourSpace.Lchuv },
        { typeof(Hsluv), ColourSpace.Hsluv },
        { typeof(Hpluv), ColourSpace.Hpluv },
        { typeof(Ypbpr), ColourSpace.Ypbpr },
        { typeof(Ycbcr), ColourSpace.Ycbcr },
        { typeof(Ycgco), ColourSpace.Ycgco },
        { typeof(Yuv), ColourSpace.Yuv },
        { typeof(Yiq), ColourSpace.Yiq },
        { typeof(Ydbdr), ColourSpace.Ydbdr },
        { typeof(Ipt), ColourSpace.Ipt },
        { typeof(Ictcp), ColourSpace.Ictcp },
        { typeof(Jzazbz), ColourSpace.Jzazbz },
        { typeof(Jzczhz), ColourSpace.Jzczhz },
        { typeof(Oklab), ColourSpace.Oklab },
        { typeof(Oklch), ColourSpace.Oklch },
        { typeof(Okhsv), ColourSpace.Okhsv },
        { typeof(Okhsl), ColourSpace.Okhsl },
        { typeof(Okhwb), ColourSpace.Okhwb },
        { typeof(Cam02), ColourSpace.Cam02 },
        { typeof(Cam16), ColourSpace.Cam16 },
        { typeof(Hct), ColourSpace.Hct }
    };
}