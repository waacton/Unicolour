namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

internal static class AssertUtils
{
    public static List<ColourSpace> AllColourSpaces => Enum.GetValues<ColourSpace>().ToList();
        
    public const double InterpolationTolerance = 0.00000000005;

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
        var colourRepresentation = unicolour.GetRepresentations(AllColourSpaces).Single(x => x is T);
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

    public static void AssertInterpolated(ColourTriplet triplet, double alpha, (double first, double second, double third, double alpha) expected)
    {
        Assert.That(triplet.First, Is.EqualTo(expected.first).Within(InterpolationTolerance));
        Assert.That(triplet.Second, Is.EqualTo(expected.second).Within(InterpolationTolerance));
        Assert.That(triplet.Third, Is.EqualTo(expected.third).Within(InterpolationTolerance));
        Assert.That(alpha, Is.EqualTo(expected.alpha).Within(InterpolationTolerance));
    }

    public static void AssertNoPropertyError(Unicolour unicolour)
    {
        void AccessProperty(Func<object> getProperty)
        {
            var _ = getProperty();
        }

        void AccessProperties()
        {
            AccessProperty(() => unicolour.Alpha);
            AccessProperty(() => unicolour.Config);
            AccessProperty(() => unicolour.Description);
            AccessProperty(() => unicolour.Hex);
            AccessProperty(() => unicolour.Hpluv);
            AccessProperty(() => unicolour.Hsb);
            AccessProperty(() => unicolour.Hsl);
            AccessProperty(() => unicolour.Hsluv);
            AccessProperty(() => unicolour.Hwb);
            AccessProperty(() => unicolour.IsDisplayable);
            AccessProperty(() => unicolour.Ictcp);
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
            AccessProperty(() => unicolour.Xyy);
            AccessProperty(() => unicolour.Xyz);
        }
        
        Assert.DoesNotThrow(AccessProperties);
    }
}