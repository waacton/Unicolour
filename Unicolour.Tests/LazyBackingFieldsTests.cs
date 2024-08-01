using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class LazyBackingFieldsTests
{
    // RGB255 is the only colour space that's not handled with its own backing field
    // (is a kind of sub-space behind RGB)
    private static readonly List<ColourSpace> ColourSpacesWithBackingFields =
        TestUtils.AllColourSpaces.Except(new [] { ColourSpace.Rgb255 }).ToList();
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void InitialUnicolour(ColourSpace colourSpace)
    {
        // no backing fields are evaluated when a unicolour is created
        // not even the backing field for the initial colour space
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        AssertBackingFieldsNotEvaluated(unicolour, ColourSpacesWithBackingFields);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterEquality(ColourSpace colourSpace)
    {
        // the initial colour space backing field is not required for equality
        // which uses the `InitialColourRepresentation` object
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        var other = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.Equals(other);
        AssertBackingFieldsNotEvaluated(unicolour, ColourSpacesWithBackingFields);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterIcc(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace, TestUtils.DefaultFogra39Config);
        _ = unicolour.Icc;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Xyz);
    }
        
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterIccUncalibrated(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.Icc;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Rgb);
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
    public void AfterDescription(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.Description;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Hsl);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterChromaticity(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.Chromaticity;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Xyy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterIsImaginary(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.IsImaginary;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Xyy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterRelativeLuminance(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.RelativeLuminance;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Xyz);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterTemperature(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.Temperature;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Xyy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterDominantWavelength(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.DominantWavelength;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Wxy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterExcitationPurity(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.ExcitationPurity;
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Wxy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterConfigurationConversion(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        _ = unicolour.ConvertToConfiguration(Configuration.Default);
        AssertBackingFieldEvaluated(unicolour, ColourSpace.Xyz);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterMix(ColourSpace colourSpace)
    {
        var unicolour = RandomColours.UnicolourFrom(colourSpace);
        var other = RandomColours.UnicolourFrom(colourSpace);
        var initialColourSpace = unicolour.InitialColourSpace;
        _ = Interpolation.Mix(unicolour, other, initialColourSpace, 0.5, HueSpan.Shorter, true);
        AssertBackingFieldsNotEvaluated(unicolour, ColourSpacesWithBackingFields.Except(new []{ colourSpace }).ToList());
        AssertBackingFieldEvaluated(unicolour, colourSpace);
    }
    
    private static void AssertBackingFieldsNotEvaluated(Unicolour unicolour, List<ColourSpace> colourSpaces)
    {
        foreach (var colourSpace in colourSpaces)
        {
            var backingFieldName = GetBackingFieldName(colourSpace);
            var isEvaluated = IsBackingFieldEvaluated(unicolour, backingFieldName);
            Assert.That(isEvaluated, Is.False);
        }
    }

    private static void AssertBackingFieldEvaluated(Unicolour unicolour, ColourSpace colourSpace)
    {
        var backingFieldName = GetBackingFieldName(colourSpace);
        var isEvaluated = IsBackingFieldEvaluated(unicolour, backingFieldName);
        Assert.That(isEvaluated, Is.True);
    }

    private static string GetBackingFieldName(ColourSpace colourSpace)
    {
        var colourSpaceName = colourSpace.ToString();
        return char.ToLower(colourSpaceName[0]) + colourSpaceName[1..];
    }

    private static bool IsBackingFieldEvaluated(Unicolour unicolour, string backingFieldName)
    {
        var lazyBackingField = GetPrivateField(backingFieldName).GetValue(unicolour)!;
        var isValueCreatedProperty = lazyBackingField.GetType().GetProperty("IsValueCreated")!;
        var isValueCreated = isValueCreatedProperty.GetValue(lazyBackingField, null);
        return (bool)isValueCreated!;
    }
    
    private static FieldInfo GetPrivateField(string name) => typeof(Unicolour).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
}

