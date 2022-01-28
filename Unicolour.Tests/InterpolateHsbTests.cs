namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour;

public class InterpolateHsbTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromHsb(180, 0.25, 0.75);
        var unicolour2 = Unicolour.FromHsb(180, 0.25, 0.75);
        var hsb1 = unicolour1.InterpolateHsb(unicolour2, 0.25).Hsb;
        var hsb2 = unicolour2.InterpolateHsb(unicolour1, 0.75).Hsb;
        var hsb3 = unicolour1.InterpolateHsb(unicolour2, 0.75).Hsb;
        var hsb4 = unicolour2.InterpolateHsb(unicolour1, 0.25).Hsb;
        
        AssertHsb(hsb1, (180, 0.25, 0.75));
        AssertHsb(hsb2, (180, 0.25, 0.75));
        AssertHsb(hsb3, (180, 0.25, 0.75));
        AssertHsb(hsb4, (180, 0.25, 0.75));
    }
    
    [Test]
    public void EquidistantForward()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0, 0);
        var unicolour2 = Unicolour.FromHsb(180, 1, 1);
        var hsb1 = unicolour1.InterpolateHsb(unicolour2, 0.5).Hsb;
        var hsb2 = unicolour2.InterpolateHsb(unicolour1, 0.5).Hsb;
        
        AssertHsb(hsb1, (90, 0.5, 0.5));
        AssertHsb(hsb2, (90, 0.5, 0.5));
    }
    
    [Test]
    public void EquidistantBackward()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0, 0);
        var unicolour2 = Unicolour.FromHsb(340, 0.5, 0.8);
        var hsb1 = unicolour1.InterpolateHsb(unicolour2, 0.5).Hsb;
        var hsb2 = unicolour2.InterpolateHsb(unicolour1, 0.5).Hsb;
        
        AssertHsb(hsb1, (350, 0.25, 0.4));
        AssertHsb(hsb2, (350, 0.25, 0.4));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 1, 0);
        var unicolour2 = Unicolour.FromHsb(180, 0, 1);
        var hsb1 = unicolour1.InterpolateHsb(unicolour2, 0.75).Hsb;
        var hsb2 = unicolour2.InterpolateHsb(unicolour1, 0.75).Hsb;

        AssertHsb(hsb1, (135, 0.25, 0.75));
        AssertHsb(hsb2, (45, 0.75, 0.25));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 1, 0);
        var unicolour2 = Unicolour.FromHsb(180, 0, 1);
        var hsb1 = unicolour1.InterpolateHsb(unicolour2, 0.25).Hsb;
        var hsb2 = unicolour2.InterpolateHsb(unicolour1, 0.25).Hsb;
        
        AssertHsb(hsb1, (45, 0.75, 0.25));
        AssertHsb(hsb2, (135, 0.25, 0.75));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0.4, 0.6);
        var unicolour2 = Unicolour.FromHsb(90, 0.6, 0.4);
        var hsb1 = unicolour1.InterpolateHsb(unicolour2, 1.5).Hsb;
        var hsb2 = unicolour2.InterpolateHsb(unicolour1, 1.5).Hsb;

        AssertHsb(hsb1, (135, 0.7, 0.3));
        AssertHsb(hsb2, (315, 0.3, 0.7));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0.4, 0.6);
        var unicolour2 = Unicolour.FromHsb(90, 0.6, 0.4);
        var hsb1 = unicolour1.InterpolateHsb(unicolour2, -0.5).Hsb;
        var hsb2 = unicolour2.InterpolateHsb(unicolour1, -0.5).Hsb;

        AssertHsb(hsb1, (315, 0.3, 0.7));
        AssertHsb(hsb2, (135, 0.7, 0.3));
    }
    
    [Test]
    public void BeyondColourToInvalidSaturation()
    {
        var unicolour1 = Unicolour.FromHsb(90, 0, 0.6);
        var unicolour2 = Unicolour.FromHsb(270, 1, 0.4);
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateHsb(unicolour2, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateHsb(unicolour1, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateHsb(unicolour2, -0.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateHsb(unicolour1, -0.0001));
    }
    
    [Test]
    public void BeyondColourToInvalidBrightness()
    {
        var unicolour1 = Unicolour.FromHsb(90, 0.6, 0);
        var unicolour2 = Unicolour.FromHsb(270, 0.4, 1);
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateHsb(unicolour2, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateHsb(unicolour1, 1.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateHsb(unicolour2, -0.0001));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateHsb(unicolour1, -0.0001));
    }

    private static void AssertHsb(Hsb actualHsb, (double h, double s, double b) expectedHsb)
    {
        Assert.That(actualHsb.H, Is.EqualTo(expectedHsb.h).Within(0.00000000005));
        Assert.That(actualHsb.S, Is.EqualTo(expectedHsb.s).Within(0.00000000005));
        Assert.That(actualHsb.B, Is.EqualTo(expectedHsb.b).Within(0.00000000005));
    }
}