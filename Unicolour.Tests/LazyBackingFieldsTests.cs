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
    private static readonly List<ColourSpace> ColourSpacesWithBackingFields = TestUtils.AllColourSpaces.Except([ColourSpace.Rgb255]).ToList();
    
    [Test]
    public void InitialUnicolour([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        // no backing fields are evaluated when a unicolour is created
        // not even the backing field for the initial colour space
        var colour = RandomColours.UnicolourFrom(colourSpace);
        AssertBackingFieldsNotEvaluated(colour, ColourSpacesWithBackingFields);
    }
    
    [Test]
    public void AfterEquality([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        // the initial colour space backing field is not required for equality
        // which uses the `InitialColourRepresentation` object
        var colour = RandomColours.UnicolourFrom(colourSpace);
        var other = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Equals(other);
        AssertBackingFieldsNotEvaluated(colour, ColourSpacesWithBackingFields);
    }
    
    [Test]
    public void AfterIcc([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace, TestUtils.DefaultFogra39Config);
        _ = colour.Icc;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyz);
    }
        
    [Test]
    public void AfterIccUncalibrated([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Icc;
        AssertBackingFieldEvaluated(colour, ColourSpace.Rgb);
    }
    
    [Test]
    public void AfterHex([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Hex;
        AssertBackingFieldEvaluated(colour, ColourSpace.Rgb);
    }
    
    [Test]
    public void AfterIsInRgbGamut([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.IsInRgbGamut;
        AssertBackingFieldEvaluated(colour, ColourSpace.Rgb);
    }
    
    [Test]
    public void AfterDescription([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Description;
        AssertBackingFieldEvaluated(colour, ColourSpace.Hsl);
    }
    
    [Test]
    public void AfterChromaticity([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Chromaticity;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyy);
    }
    
    [Test]
    public void AfterIsImaginary([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.IsImaginary;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyy);
    }
    
    [Test]
    public void AfterRelativeLuminance([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.RelativeLuminance;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyz);
    }
    
    [Test]
    public void AfterTemperature([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.Temperature;
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyy);
    }
    
    [Test]
    public void AfterDominantWavelength([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.DominantWavelength;
        AssertBackingFieldEvaluated(colour, ColourSpace.Wxy);
    }
    
    [Test]
    public void AfterExcitationPurity([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.ExcitationPurity;
        AssertBackingFieldEvaluated(colour, ColourSpace.Wxy);
    }
    
    [Test]
    public void AfterConfigurationConversion([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
    {
        var colour = RandomColours.UnicolourFrom(colourSpace);
        _ = colour.ConvertToConfiguration(TestUtils.D50Config);
        AssertBackingFieldEvaluated(colour, ColourSpace.Xyz);
    }
    
    [Test]
    public void AfterMix([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
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

    private static string GetBackingFieldName([ValueSource(nameof(ColourSpacesWithBackingFields))] ColourSpace colourSpace)
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

