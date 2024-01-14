namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

internal static class TestUtils
{
    // generating planckian tables is expensive, but this is the set of tables needed for most temperature tests
    internal static readonly Planckian PlanckianObserverDegree2 = new(Observer.Degree2);

    public static List<ColourSpace> AllColourSpaces => Enum.GetValues<ColourSpace>().ToList();
    public static readonly List<TestCaseData> AllColourSpacesTestCases = new()
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
        new TestCaseData(ColourSpace.Ictcp),
        new TestCaseData(ColourSpace.Jzazbz),
        new TestCaseData(ColourSpace.Jzczhz),
        new TestCaseData(ColourSpace.Oklab),
        new TestCaseData(ColourSpace.Oklch),
        new TestCaseData(ColourSpace.Cam02),
        new TestCaseData(ColourSpace.Cam16),
        new TestCaseData(ColourSpace.Hct)
    };
    
    public static readonly List<TestCaseData> AllIlluminantsTestCases = new()
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
    
    public static List<double> ExtremeDoubles = new() { double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN };
        
    public const double MixTolerance = 0.00000000005;

    public static void AssertTriplet(ColourTriplet actual, ColourTriplet expected, double tolerance, string? info = null)
    {
        var details = $"Expected --- {expected}\nActual ----- {actual}";
        string FailMessage(string channel) => $"{(info == null ? string.Empty : $"{info} · ")}{channel}\n{details}";
        AssertTripletValue(actual.First, expected.First, tolerance, FailMessage("Channel 1"), actual.HueIndex == 0);
        AssertTripletValue(actual.Second, expected.Second, tolerance, FailMessage("Channel 2"));
        AssertTripletValue(actual.Third, expected.Third, tolerance, FailMessage("Channel 3"), actual.HueIndex == 2);
    }

    public static void AssertTriplet<T>(Unicolour unicolour, ColourTriplet expected, double tolerance) where T : ColourRepresentation
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
    
    public static void AssertMixed(ColourTriplet triplet, double alpha, (double first, double second, double third, double alpha) expected)
    {
        Assert.That(triplet.First, Is.EqualTo(expected.first).Within(MixTolerance), "First");
        Assert.That(triplet.Second, Is.EqualTo(expected.second).Within(MixTolerance), "Second");
        Assert.That(triplet.Third, Is.EqualTo(expected.third).Within(MixTolerance), "Third");
        Assert.That(alpha, Is.EqualTo(expected.alpha).Within(MixTolerance), "Alpha");
    }

    public static void AssertNoPropertyError(Unicolour unicolour)
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
            AccessProperty(() => unicolour.Hct);
            AccessProperty(() => unicolour.Hex);
            AccessProperty(() => unicolour.Hpluv);
            AccessProperty(() => unicolour.Hsb);
            AccessProperty(() => unicolour.Hsl);
            AccessProperty(() => unicolour.Hsluv);
            AccessProperty(() => unicolour.Hwb);
            AccessProperty(() => unicolour.Ictcp);
            AccessProperty(() => unicolour.IsInDisplayGamut);
            AccessProperty(() => unicolour.Jzazbz);
            AccessProperty(() => unicolour.Jzczhz);
            AccessProperty(() => unicolour.Lab);
            AccessProperty(() => unicolour.Lchab);
            AccessProperty(() => unicolour.Lchuv);
            AccessProperty(() => unicolour.Luv);
            AccessProperty(() => unicolour.Oklab);
            AccessProperty(() => unicolour.Oklch);
            AccessProperty(() => unicolour.RelativeLuminance);
            AccessProperty(() => unicolour.Rgb);
            AccessProperty(() => unicolour.Rgb.Byte255);
            AccessProperty(() => unicolour.RgbLinear);
            AccessProperty(() => unicolour.Temperature);
            AccessProperty(() => unicolour.Xyy);
            AccessProperty(() => unicolour.Xyz);
        }
        
        void AccessProperty(Func<object> getProperty)
        {
            _ = getProperty();
        }
    }
    
    public static void AssertEqual<T>(T object1, T object2)
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

    public static void AssertNotEqual<T>(T object1, T object2)
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
        { typeof(Ictcp), ColourSpace.Ictcp },
        { typeof(Jzazbz), ColourSpace.Jzazbz },
        { typeof(Jzczhz), ColourSpace.Jzczhz },
        { typeof(Oklab), ColourSpace.Oklab },
        { typeof(Oklch), ColourSpace.Oklch },
        { typeof(Cam02), ColourSpace.Cam02 },
        { typeof(Cam16), ColourSpace.Cam16 },
        { typeof(Hct), ColourSpace.Hct }
    };
}