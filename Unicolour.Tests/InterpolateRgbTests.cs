namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour;

public class InterpolateRgbTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromRgb(0.5, 0.25, 0.75);
        var unicolour2 = Unicolour.FromRgb(0.5, 0.25, 0.75);
        var rgb1 = unicolour1.InterpolateRgb(unicolour2, 0.25).Rgb;
        var rgb2 = unicolour2.InterpolateRgb(unicolour1, 0.75).Rgb;
        var rgb3 = unicolour1.InterpolateRgb(unicolour2, 0.75).Rgb;
        var rgb4 = unicolour2.InterpolateRgb(unicolour1, 0.25).Rgb;
        
        AssertRgb(rgb1, (0.5, 0.25, 0.75));
        AssertRgb(rgb2, (0.5, 0.25, 0.75));
        AssertRgb(rgb3, (0.5, 0.25, 0.75));
        AssertRgb(rgb4, (0.5, 0.25, 0.75));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromRgb(0.0, 0.0, 0.0);
        var unicolour2 = Unicolour.FromRgb(0.5, 1.0, 1.0);
        var rgb1 = unicolour1.InterpolateRgb(unicolour2, 0.5).Rgb;
        var rgb2 = unicolour2.InterpolateRgb(unicolour1, 0.5).Rgb;
        
        AssertRgb(rgb1, (0.25, 0.5, 0.5));
        AssertRgb(rgb2, (0.25, 0.5, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromRgb(0.0, 1.0, 0.0);
        var unicolour2 = Unicolour.FromRgb(0.8, 0.0, 1.0);
        var rgb1 = unicolour1.InterpolateRgb(unicolour2, 0.75).Rgb;
        var rgb2 = unicolour2.InterpolateRgb(unicolour1, 0.75).Rgb;

        AssertRgb(rgb1, (0.6, 0.25, 0.75));
        AssertRgb(rgb2, (0.2, 0.75, 0.25));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromRgb(0.0, 1.0, 0.0);
        var unicolour2 = Unicolour.FromRgb(0.8, 0.0, 1.0);
        var rgb1 = unicolour1.InterpolateRgb(unicolour2, 0.25).Rgb;
        var rgb2 = unicolour2.InterpolateRgb(unicolour1, 0.25).Rgb;
        
        AssertRgb(rgb1, (0.2, 0.75, 0.25));
        AssertRgb(rgb2, (0.6, 0.25, 0.75));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromRgb(0.2, 0.4, 0.6);
        var unicolour2 = Unicolour.FromRgb(0.3, 0.6, 0.4);
        var rgb1 = unicolour1.InterpolateRgb(unicolour2, 1.5).Rgb;
        var rgb2 = unicolour2.InterpolateRgb(unicolour1, 1.5).Rgb;

        AssertRgb(rgb1, (0.35, 0.7, 0.3));
        AssertRgb(rgb2, (0.15, 0.3, 0.7));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromRgb(0.2, 0.4, 0.6);
        var unicolour2 = Unicolour.FromRgb(0.3, 0.6, 0.4);
        var rgb1 = unicolour1.InterpolateRgb(unicolour2, -0.5).Rgb;
        var rgb2 = unicolour2.InterpolateRgb(unicolour1, -0.5).Rgb;

        AssertRgb(rgb1, (0.15, 0.3, 0.7));
        AssertRgb(rgb2, (0.35, 0.7, 0.3));
    }
    
    [Test]
    public void BeyondColourToInvalidRed()
    {
        var unicolour1 = Unicolour.FromRgb(0.0, 0.4, 0.6);
        var unicolour2 = Unicolour.FromRgb(1.0, 0.6, 0.4);
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateRgb(unicolour2, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateRgb(unicolour1, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateRgb(unicolour2, -0.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateRgb(unicolour1, -0.0001));
    }
    
    [Test]
    public void BeyondColourToInvalidGreen()
    {
        var unicolour1 = Unicolour.FromRgb(0.4, 0.0, 0.6);
        var unicolour2 = Unicolour.FromRgb(0.6, 1.0, 0.4);
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateRgb(unicolour2, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateRgb(unicolour1, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateRgb(unicolour2, -0.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateRgb(unicolour1, -0.0001));
    }
    
    [Test]
    public void BeyondColourToInvalidBlue()
    {
        var unicolour1 = Unicolour.FromRgb(0.4, 0.6, 0.0);
        var unicolour2 = Unicolour.FromRgb(0.6, 0.4, 1.0);
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateRgb(unicolour2, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateRgb(unicolour1, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateRgb(unicolour2, -0.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateRgb(unicolour1, -0.0001));
    }

    private static void AssertRgb(Rgb actualRgb, (double r, double g, double b) expectedRgb)
    {
        Assert.That(actualRgb.R, Is.EqualTo(expectedRgb.r).Within(0.00000000005));
        Assert.That(actualRgb.G, Is.EqualTo(expectedRgb.g).Within(0.00000000005));
        Assert.That(actualRgb.B, Is.EqualTo(expectedRgb.b).Within(0.00000000005));
    }
}