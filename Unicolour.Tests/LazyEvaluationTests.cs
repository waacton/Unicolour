namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class LazyEvaluationTests
{
    // RGB255 is the only colour space that's not handled with its own backing field
    // (is a kind of sub-space behind RGB)
    private static readonly List<ColourSpace> ColourSpacesWithBackingFields =
        TestUtils.AllColourSpaces.Except(new [] { ColourSpace.Rgb255 }).ToList();
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void InitialUnicolour(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        AssertBackingFields(unicolour);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterEquality(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        var other = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.Equals(other);
        AssertBackingFields(unicolour);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterMix(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        var other = RandomColours.UnicolourFrom(colourSpace);
        var initialColourSpace = unicolour.InitialColourSpace;
        _ = Interpolation.Mix(initialColourSpace, unicolour, other, 0.5, true);
        AssertBackingFields(unicolour);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterHex(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.Hex;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Rgb);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterIsInDisplayGamut(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.IsInDisplayGamut;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Rgb);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterRelativeLuminance(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.RelativeLuminance;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.RgbLinear);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterDescription(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.Description;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Hsl);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterTemperature(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.Temperature;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Xyz);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterConfigurationConversion(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.ConvertToConfiguration(Configuration.Default);
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Xyz);
    }
    
    private static void AssertBackingFields(Unicolour unicolour)
    {
        var initialField = GetBackingFieldName(unicolour.InitialColourSpace);
        foreach (var colourSpace in ColourSpacesWithBackingFields)
        {
            var backingField = GetBackingFieldName(colourSpace);
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
        var field = GetBackingFieldName(colourSpace);
        var value = GetPrivateField(field).GetValue(unicolour);
        Assert.NotNull(value);
    }

    private static string GetBackingFieldName(ColourSpace colourSpace)
    {
        var colourSpaceName = colourSpace.ToString();
        return char.ToLower(colourSpaceName[0]) + colourSpaceName[1..];
    }
    
    private static FieldInfo GetPrivateField(string name) => typeof(Unicolour).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
}

