namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

/*
 * Conversions that are just translations to/from cylindrical coordinate spaces
 * should have input values clamped to the range of the source coordinate space
 * otherwise they will be mapped to incorrect values
 * e.g. RGB with negative -> HSB should produce the same value as RGB with zero -> HSB
 */
public class CoordinateSpaceTests
{
    [Test]
    public void CartesianRgbToCylindricalHsb()
    {
        ColourTriplet upperOutRange = new(1.00001, 100, double.PositiveInfinity);
        ColourTriplet upperInRange = new(1, 1, 1);
        ColourTriplet lowerOutRange = new(-0.00001, -100, double.NegativeInfinity);
        ColourTriplet lowerInRange = new(0, 0, 0);
        AssertRgbToHsb(upperInRange, upperOutRange);
        AssertRgbToHsb(lowerInRange, lowerOutRange);
    }

    [Test]
    public void CylindricalHsbToCartesianRgb()
    {
        ColourTriplet upperOutRange = new(360.00001, 100, double.PositiveInfinity);
        ColourTriplet upperInRange = new(0.00001, 1, 1);
        ColourTriplet lowerOutRange = new(-0.00001, -100, double.NegativeInfinity);
        ColourTriplet lowerInRange = new(359.99999, 0, 0);
        AssertHsbToRgb(upperInRange, upperOutRange);
        AssertHsbToRgb(lowerInRange, lowerOutRange);
    }
    
    [Test]
    public void CylindricalHsbToCylindricalHsl()
    {
        ColourTriplet upperOutRange = new(360.00001, 100, double.PositiveInfinity);
        ColourTriplet upperInRange = new(0.00001, 1, 1);
        ColourTriplet lowerOutRange = new(-0.00001, -100, double.NegativeInfinity);
        ColourTriplet lowerInRange = new(359.99999, 0, 0);
        AssertHsbToHsl(upperInRange, upperOutRange);
        AssertHsbToHsl(lowerInRange, lowerOutRange);
    }
    
    [Test]
    public void CylindricalHslToCylindricalHsb()
    {
        ColourTriplet upperOutRange = new(360.00001, 100, double.PositiveInfinity);
        ColourTriplet upperInRange = new(0.00001, 1, 1);
        ColourTriplet lowerOutRange = new(-0.00001, -100, double.NegativeInfinity);
        ColourTriplet lowerInRange = new(359.99999, 0, 0);
        AssertHslToHsb(upperInRange, upperOutRange);
        AssertHslToHsb(lowerInRange, lowerOutRange);
    }
    
    private static void AssertRgbToHsb(ColourTriplet inRange, ColourTriplet outRange)
    {
        Rgb GetInput(ColourTriplet triplet) => new(triplet.First, triplet.Second, triplet.Third, Configuration.Default);
        var inRangeInput = GetInput(inRange);
        var outRangeInput = GetInput(outRange);
        
        Hsb GetOutput(Rgb rgb) => Conversion.RgbToHsb(rgb);
        var inRangeOutput = GetOutput(inRangeInput);
        var outRangeOutput = GetOutput(outRangeInput);
        
        AssertTriplets(
            AsTriplets(inRangeInput), AsTriplets(outRangeInput), 
            AsTriplets(inRangeOutput), AsTriplets(outRangeOutput));
    }
    
    private static void AssertHsbToRgb(ColourTriplet inRange, ColourTriplet outRange)
    {
        Hsb GetInput(ColourTriplet triplet) => new(triplet.First, triplet.Second, triplet.Third);
        var inRangeInput = GetInput(inRange);
        var outRangeInput = GetInput(outRange);
        
        Rgb GetOutput(Hsb hsb) => Conversion.HsbToRgb(hsb, Configuration.Default);
        var inRangeOutput = GetOutput(inRangeInput);
        var outRangeOutput = GetOutput(outRangeInput);
        
        AssertTriplets(
            AsTriplets(inRangeInput), AsTriplets(outRangeInput), 
            AsTriplets(inRangeOutput), AsTriplets(outRangeOutput));
    }
    
    private static void AssertHsbToHsl(ColourTriplet inRange, ColourTriplet outRange)
    {
        Hsb GetInput(ColourTriplet triplet) => new(triplet.First, triplet.Second, triplet.Third);
        var inRangeInput = GetInput(inRange);
        var outRangeInput = GetInput(outRange);
        
        Hsl GetOutput(Hsb hsb) => Conversion.HsbToHsl(hsb);
        var inRangeOutput = GetOutput(inRangeInput);
        var outRangeOutput = GetOutput(outRangeInput);
        
        AssertTriplets(
            AsTriplets(inRangeInput), AsTriplets(outRangeInput), 
            AsTriplets(inRangeOutput), AsTriplets(outRangeOutput));
    }
    
    private static void AssertHslToHsb(ColourTriplet inRange, ColourTriplet outRange)
    {
        Hsl GetInput(ColourTriplet triplet) => new(triplet.First, triplet.Second, triplet.Third);
        var inRangeInput = GetInput(inRange);
        var outRangeInput = GetInput(outRange);
        
        Hsb GetOutput(Hsl hsl) => Conversion.HslToHsb(hsl);
        var inRangeOutput = GetOutput(inRangeInput);
        var outRangeOutput = GetOutput(outRangeInput);

        AssertTriplets(
            AsTriplets(inRangeInput), AsTriplets(outRangeInput), 
            AsTriplets(inRangeOutput), AsTriplets(outRangeOutput));
    }
    
    private static void AssertTriplets(Triplets inRangeInput, Triplets outRangeInput, Triplets inRangeOutput, Triplets outRangeOutput)
    {
        AssertUtils.AssertColourTriplet(outRangeInput.Constrained, inRangeInput.Unconstrained, 0.00001);
        AssertUtils.AssertColourTriplet(outRangeOutput.Unconstrained, inRangeOutput.Unconstrained, 0.00001);
        AssertUtils.AssertColourTriplet(outRangeOutput.Constrained, inRangeOutput.Unconstrained, 0.00001);
    }

    private static Triplets AsTriplets(Rgb rgb) => new(rgb.Triplet, rgb.ConstrainedTriplet);
    private static Triplets AsTriplets(Hsb hsb) => new(hsb.Triplet, hsb.ConstrainedTriplet);
    private static Triplets AsTriplets(Hsl hsl) => new(hsl.Triplet, hsl.ConstrainedTriplet);

    private record Triplets(ColourTriplet Unconstrained, ColourTriplet Constrained);
}