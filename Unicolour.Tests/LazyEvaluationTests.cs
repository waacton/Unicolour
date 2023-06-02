namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class LazyEvaluationTests
{
    // RgbLinear & Rgb255 are currently calculated during Rgb construction
    // instead of separately like the other colour spaces
    // but this might change in future if Unicolour.FromRgbLinear() becomes supported
    private static readonly List<string> BackingFields = 
        AssertUtils.AllColourSpaces
            .Except(new [] { ColourSpace.RgbLinear, ColourSpace.Rgb255 })
            .Select(x => x.ToString().ToLower())
            .ToList();

    private static readonly List<TestCaseData> TestCases = new()
    {
        new TestCaseData(RandomColours.UnicolourFromRgb).SetName("Rgb"),
        new TestCaseData(RandomColours.UnicolourFromHsb).SetName("Hsb"),
        new TestCaseData(RandomColours.UnicolourFromHsl).SetName("Hsl"),
        new TestCaseData(RandomColours.UnicolourFromHwb).SetName("Hwb"),
        new TestCaseData(RandomColours.UnicolourFromXyz).SetName("Xyz"),
        new TestCaseData(RandomColours.UnicolourFromXyy).SetName("Xyy"),
        new TestCaseData(RandomColours.UnicolourFromLab).SetName("Lab"),
        new TestCaseData(RandomColours.UnicolourFromLchab).SetName("Lchab"),
        new TestCaseData(RandomColours.UnicolourFromLuv).SetName("Luv"),
        new TestCaseData(RandomColours.UnicolourFromLchuv).SetName("Lchuv"),
        new TestCaseData(RandomColours.UnicolourFromHsluv).SetName("Hsluv"),
        new TestCaseData(RandomColours.UnicolourFromHpluv).SetName("Hpluv"),
        new TestCaseData(RandomColours.UnicolourFromIctcp).SetName("Ictcp"),
        new TestCaseData(RandomColours.UnicolourFromJzazbz).SetName("Jzazbz"),
        new TestCaseData(RandomColours.UnicolourFromJzczhz).SetName("Jzczhz"),
        new TestCaseData(RandomColours.UnicolourFromOklab).SetName("Oklab"),
        new TestCaseData(RandomColours.UnicolourFromOklch).SetName("Oklch"),
        new TestCaseData(RandomColours.UnicolourFromCam02).SetName("Cam02"),
        new TestCaseData(RandomColours.UnicolourFromCam16).SetName("Cam16")
    };
    
    [TestCaseSource(nameof(TestCases))]
    public static void InitialUnicolour(Func<Unicolour> unicolourFunction)
    {
        var unicolour = unicolourFunction();
        AssertBackingFields(unicolour);
    }
    
    [TestCaseSource(nameof(TestCases))]
    public static void AfterEquality(Func<Unicolour> unicolourFunction)
    {
        var unicolour = unicolourFunction();
        var other = unicolourFunction();
        var _ = unicolour.Equals(other);
        AssertBackingFields(unicolour);
    }
    
    [TestCaseSource(nameof(TestCases))]
    public static void AfterInterpolation(Func<Unicolour> unicolourFunction)
    {
        var unicolour = unicolourFunction();
        var other = unicolourFunction();
        var initialColourSpace = unicolour.InitialColourSpace;
        var _ = Interpolation.Interpolate(initialColourSpace, unicolour, other, 0.5);
        AssertBackingFields(unicolour);
    }
    
    [TestCaseSource(nameof(TestCases))]
    public static void AfterHex(Func<Unicolour> unicolourFunction)
    {
        var unicolour = unicolourFunction();
        var _ = unicolour.Hex;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Rgb);
    }
    
    [TestCaseSource(nameof(TestCases))]
    public static void AfterIsDisplayable(Func<Unicolour> unicolourFunction)
    {
        var unicolour = unicolourFunction();
        var _ = unicolour.IsDisplayable;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Rgb);
    }
    
    [TestCaseSource(nameof(TestCases))]
    public static void AfterRelativeLuminance(Func<Unicolour> unicolourFunction)
    {
        var unicolour = unicolourFunction();
        var _ = unicolour.RelativeLuminance;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Rgb);
    }
    
    [TestCaseSource(nameof(TestCases))]
    public static void AfterDescription(Func<Unicolour> unicolourFunction)
    {
        var unicolour = unicolourFunction();
        var _ = unicolour.Description;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Hsl);
    }
    
    [TestCaseSource(nameof(TestCases))]
    public static void AfterConfigurationConversion(Func<Unicolour> unicolourFunction)
    {
        var unicolour = unicolourFunction();
        var _ = unicolour.ConvertToConfiguration(Configuration.Default);
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Xyz);
    }
    
    private static void AssertBackingFields(Unicolour unicolour)
    {
        var initialField = unicolour.InitialColourSpace.ToString().ToLower();
        foreach (var backingField in BackingFields)
        {
            var value = GetPrivateField(backingField).GetValue(unicolour);
            if (backingField == initialField)
            {
                Assert.NotNull(value);
            }
            else
            {
                Assert.Null(value);
            }
        }
    }

    private static void AssertBackingFieldEvaluated(Unicolour unicolour, ColourSpace colourSpace)
    {
        var field = colourSpace.ToString().ToLower();
        var value = GetPrivateField(field).GetValue(unicolour);
        Assert.NotNull(value);
    }
    
    private static FieldInfo GetPrivateField(string name) => typeof(Unicolour).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
}

