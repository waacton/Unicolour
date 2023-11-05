namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

internal static class TestUtils
{
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
        Assert.That(triplet.First, Is.EqualTo(expected.first).Within(MixTolerance));
        Assert.That(triplet.Second, Is.EqualTo(expected.second).Within(MixTolerance));
        Assert.That(triplet.Third, Is.EqualTo(expected.third).Within(MixTolerance));
        Assert.That(alpha, Is.EqualTo(expected.alpha).Within(MixTolerance));
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