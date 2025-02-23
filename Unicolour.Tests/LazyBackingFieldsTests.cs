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
        TestUtils.AllColourSpaces.Except([ColourSpace.Rgb255]).ToList();
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void InitialUnicolour(ColourSpace colourSpace)
    {
        // no backing fields are evaluated when a unicolour is created
        // not even the backing field for the initial colour space
        var colour = RandomColours.UnicolourFrom(colourSpace);
        AssertBackingFieldsNotEvaluated(colour, ColourSpacesWithBackingFields);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterEquality(ColourSpace colourSpace)
    {
        // the initial colour space backing field is not required for equality
        // which uses the `InitialColourRepresentation` object
        var colour = RandomColours.UnicolourFrom(colourSpace);
        var other = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Equals(other);
        AssertBackingFieldsNotEvaluated(colour, ColourSpacesWithBackingFields);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterIcc(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace, TestUtils.DefaultFogra39Config);
        _ = colour.Icc;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyz);
    }
        
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterIccUncalibrated(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Icc;
        AssertBackingFieldEvaluated(colour, ColourSpace.Rgb);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterHex(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Hex;
        AssertBackingFieldEvaluated(colour, ColourSpace.Rgb);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterIsInRgbGamut(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.IsInRgbGamut;
        AssertBackingFieldEvaluated(colour, ColourSpace.Rgb);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterDescription(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Description;
        AssertBackingFieldEvaluated(colour, ColourSpace.Hsl);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterChromaticity(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Chromaticity;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterIsImaginary(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.IsImaginary;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterRelativeLuminance(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.RelativeLuminance;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyz);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterTemperature(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Temperature;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterDominantWavelength(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.DominantWavelength;
        AssertBackingFieldEvaluated(colour, ColourSpace.Wxy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterExcitationPurity(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.ExcitationPurity;
        AssertBackingFieldEvaluated(colour, ColourSpace.Wxy);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterConfigurationConversion(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.ConvertToConfiguration(Configuration.Default);
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyz);
    }
    
    [TestCaseSource(typeof(TestUtils), nameof(TestUtils.AllColourSpacesTestCases))]
    public void AfterMix(ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        var other = RandomColours.UnicolourFrom(colourSpace);
        var sourceColourSpace = colour.SourceColourSpace;
        _ = Interpolation.Mix(colour, other, sourceColourSpace, 0.5, HueSpan.Shorter, true);
        AssertBackingFieldsNotEvaluated(colour, ColourSpacesWithBackingFields.Except([colourSpace]).ToList());
        AssertBackingFieldEvaluated(colour, colourSpace);
    }
    
    private static void AssertBackingFieldsNotEvaluated(Unicolour colour, List<ColourSpace> colourSpaces)
    {
        foreach (var colourSpace in colourSpaces)
        {
            var backingFieldName = GetBackingFieldName(colourSpace);
            var isEvaluated = IsBackingFieldEvaluated(colour, backingFieldName);
            Assert.That(isEvaluated, Is.False);
        }
    }

    private static void AssertBackingFieldEvaluated(Unicolour colour, ColourSpace colourSpace)
    {
        var backingFieldName = GetBackingFieldName(colourSpace);
        var isEvaluated = IsBackingFieldEvaluated(colour, backingFieldName);
        Assert.That(isEvaluated, Is.True);
    }

    private static string GetBackingFieldName(ColourSpace colourSpace)
    {
        var colourSpaceName = colourSpace.ToString();
        return char.ToLower(colourSpaceName[0]) + colourSpaceName[1..];
    }

    private static bool IsBackingFieldEvaluated(Unicolour colour, string backingFieldName)
    {
        var lazyBackingField = GetPrivateField(backingFieldName).GetValue(colour)!;
        var isValueCreatedProperty = lazyBackingField.GetType().GetProperty("IsValueCreated")!;
        var isValueCreated = isValueCreatedProperty.GetValue(lazyBackingField, null);
        return (bool)isValueCreated!;
    }
    
    private static FieldInfo GetPrivateField(string name) => typeof(Unicolour).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
}

