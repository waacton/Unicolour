using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests.Utils;

internal static class TestUtils
{
    private static readonly Random Random = new();
    internal static double RandomDouble() => Random.NextDouble();
    internal static double RandomDouble(double min, double max) => Random.NextDouble() * (max - min) + min;
    internal static int RandomInt(int max) => Random.Next(max);
    
    internal static List<ColourSpace> AllColourSpaces => Enum.GetValues<ColourSpace>().ToList();
    
    internal static readonly List<Illuminant> AllIlluminants =
    [
        Illuminant.A,
        Illuminant.C,
        Illuminant.D50, Illuminant.D55, Illuminant.D65, Illuminant.D75,
        Illuminant.E,
        Illuminant.F2, Illuminant.F7, Illuminant.F11
    ];

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
    
    internal static readonly List<RgbConfiguration> NonDefaultRgbConfigs =
    [
        RgbConfiguration.DisplayP3,
        RgbConfiguration.Rec2020,
        RgbConfiguration.Rec2100Pq,
        RgbConfiguration.Rec2100Hlg,
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
    ];
    
    internal static readonly List<YbrConfiguration> NonDefaultYbrConfigs =
    [
        YbrConfiguration.Rec709,
        YbrConfiguration.Rec2020,
        YbrConfiguration.Jpeg
    ];

    private static readonly IccConfiguration IccFogra39 = new(IccFile.Fogra39.GetProfile(), Intent.RelativeColorimetric, "Fogra39 relative");
    internal static readonly Configuration DefaultFogra39Config = new(iccConfig: IccFogra39);

    internal static readonly Configuration D65Config = new(xyzConfig: XyzConfiguration.D65); // same as Configuration.Default
    internal static readonly Configuration D50Config = new(xyzConfig: XyzConfiguration.D50);
    internal static readonly Configuration EqualEnergyConfig = new(xyzConfig: new(Illuminant.E, Observer.Degree2, "Equal Energy"));
    internal static readonly Configuration CConfig = new(xyzConfig: new(Illuminant.C, Observer.Degree2, "C"));
    
    // generating planckian tables is expensive, but this is the set of tables needed for most temperature tests
    internal static readonly Planckian PlanckianObserverDegree2 = new(Observer.Degree2);
    
    internal static List<double> ExtremeDoubles =
    [
        double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN
    ];
        
    internal const double MixTolerance = 0.00000000005;
    
    internal static void AssertTriplet<T>(Unicolour colour, ColourTriplet expected, double tolerance) where T : ColourRepresentation
    {
        var colourSpace = RepresentationTypeToColourSpace[typeof(T)];
        var colourRepresentation = colour.GetRepresentation(colourSpace);
        var tolerances = GetTolerances(tolerance, colourRepresentation.HueIndex);
        AssertTriplet(colourRepresentation.Triplet, expected, tolerances);
    }
    
    internal static void AssertTriplet<T>(Unicolour colour, ColourTriplet expected, double[] tolerances) where T : ColourRepresentation
    {
        var colourSpace = RepresentationTypeToColourSpace[typeof(T)];
        var colourRepresentation = colour.GetRepresentation(colourSpace);
        AssertTriplet(colourRepresentation.Triplet, expected, tolerances);
    }

    internal static void AssertTriplet(ColourTriplet actual, ColourTriplet expected, double tolerance, string? info = null)
    {
        var tolerances = GetTolerances(tolerance, expected.HueIndex);
        AssertTriplet(actual, expected, tolerances, info);
    }
    
    private static double[] GetTolerances(double tolerance, int? hueIndex)
    {
        double[] tolerances = [tolerance, tolerance, tolerance];
        if (hueIndex == null)
        {
            return tolerances;
        }
        
        tolerances[(int)hueIndex] *= 360;
        return tolerances;
    }
    
    internal static void AssertTriplet(ColourTriplet actual, ColourTriplet expected, double[] tolerances, string? info = null)
    {
        var details = $"Expected --- {expected}\nActual ----- {actual}";
        string FailMessage(string channel) => $"{(info == null ? string.Empty : $"{info} · ")}{channel}\n{details}";
        AssertTripletValue(actual.First, expected.First, tolerances[0], FailMessage("Channel 1"), actual.HueIndex == 0);
        AssertTripletValue(actual.Second, expected.Second, tolerances[1], FailMessage("Channel 2"));
        AssertTripletValue(actual.Third, expected.Third, tolerances[2], FailMessage("Channel 3"), actual.HueIndex == 2);
    }

    private static void AssertTripletValue(double actual, double expected, double tolerance, string failMessage, bool isHue = false)
    {
        if (isHue)
        {
            (actual, expected) = Hue.Unwrap(actual, expected);
        }
        
        Assert.That(actual, Is.EqualTo(expected).Within(tolerance), failMessage);
    }

    internal static void AssertMixed(ColourTriplet triplet, double alpha, (double first, double second, double third, double alpha) expected)
    {
        Assert.That(triplet.First, Is.EqualTo(expected.first).Within(MixTolerance), "First");
        Assert.That(triplet.Second, Is.EqualTo(expected.second).Within(MixTolerance), "Second");
        Assert.That(triplet.Third, Is.EqualTo(expected.third).Within(MixTolerance), "Third");
        Assert.That(alpha, Is.EqualTo(expected.alpha).Within(MixTolerance), "Alpha");
    }
    
    internal static void AssertNoPropertyError(Unicolour colour)
    {
        Assert.DoesNotThrow(AccessProperties);
        return;

        void AccessProperties()
        {
            AccessProperty(() => colour.Alpha);
            AccessProperty(() => colour.Cam02);
            AccessProperty(() => colour.Cam16);
            AccessProperty(() => colour.Chromaticity);
            AccessProperty(() => colour.Configuration);
            AccessProperty(() => colour.Description);
            AccessProperty(() => colour.DominantWavelength);
            AccessProperty(() => colour.ExcitationPurity);
            AccessProperty(() => colour.Hct);
            AccessProperty(() => colour.Hex);
            AccessProperty(() => colour.Hpluv);
            AccessProperty(() => colour.Hsb);
            AccessProperty(() => colour.Hsi);
            AccessProperty(() => colour.Hsl);
            AccessProperty(() => colour.Hsluv);
            AccessProperty(() => colour.Hwb);
            AccessProperty(() => colour.Icc);
            AccessProperty(() => colour.Ictcp);
            AccessProperty(() => colour.Ipt);
            AccessProperty(() => colour.IsImaginary);
            AccessProperty(() => colour.IsInMacAdamLimits);
            AccessProperty(() => colour.IsInPointerGamut);
            AccessProperty(() => colour.IsInRgbGamut);
            AccessProperty(() => colour.Jzazbz);
            AccessProperty(() => colour.Jzczhz);
            AccessProperty(() => colour.Lab);
            AccessProperty(() => colour.Lchab);
            AccessProperty(() => colour.Lchuv);
            AccessProperty(() => colour.Lms);
            AccessProperty(() => colour.Luv);
            AccessProperty(() => colour.Oklab);
            AccessProperty(() => colour.Oklch);
            AccessProperty(() => colour.Okhsl);
            AccessProperty(() => colour.Okhsv);
            AccessProperty(() => colour.Okhwb);
            AccessProperty(() => colour.RelativeLuminance);
            AccessProperty(() => colour.Rgb);
            AccessProperty(() => colour.Rgb.Byte255);
            AccessProperty(() => colour.RgbLinear);
            AccessProperty(() => colour.Temperature);
            AccessProperty(() => colour.Tsl);
            AccessProperty(() => colour.Wxy);
            AccessProperty(() => colour.Xyb);
            AccessProperty(() => colour.Xyy);
            AccessProperty(() => colour.Xyz);
            AccessProperty(() => colour.Ypbpr);
            AccessProperty(() => colour.Ycbcr);
            AccessProperty(() => colour.Ycgco);
            AccessProperty(() => colour.Yuv);
            AccessProperty(() => colour.Yiq);
            AccessProperty(() => colour.Ydbdr);
            AccessProperty(colour.ToString);
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
            Assert.That(object1, Is.EqualTo(object2));
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
        { typeof(Hsi), ColourSpace.Hsi },
        { typeof(Xyz), ColourSpace.Xyz },
        { typeof(Xyy), ColourSpace.Xyy },
        { typeof(Wxy), ColourSpace.Wxy },
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
        { typeof(Tsl), ColourSpace.Tsl },
        { typeof(Xyb), ColourSpace.Xyb },
        { typeof(Lms), ColourSpace.Lms },
        { typeof(Ipt), ColourSpace.Ipt },
        { typeof(Ictcp), ColourSpace.Ictcp },
        { typeof(Jzazbz), ColourSpace.Jzazbz },
        { typeof(Jzczhz), ColourSpace.Jzczhz },
        { typeof(Oklab), ColourSpace.Oklab },
        { typeof(Oklch), ColourSpace.Oklch },
        { typeof(Okhsv), ColourSpace.Okhsv },
        { typeof(Okhsl), ColourSpace.Okhsl },
        { typeof(Okhwb), ColourSpace.Okhwb },
        { typeof(Oklrab), ColourSpace.Oklrab },
        { typeof(Oklrch), ColourSpace.Oklrch },
        { typeof(Cam02), ColourSpace.Cam02 },
        { typeof(Cam16), ColourSpace.Cam16 },
        { typeof(Hct), ColourSpace.Hct },
        { typeof(Munsell), ColourSpace.Munsell }
    };
}