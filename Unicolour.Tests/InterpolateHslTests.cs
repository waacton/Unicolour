namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class InterpolateHslTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromHsl(180, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromHsl(180, 0.25, 0.75, 0.5);
        var interpolated1 = unicolour1.InterpolateHsl(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHsl(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateHsl(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateHsl(unicolour1, 0.25);
        
        AssertHsla(interpolated1, (180, 0.25, 0.75, 0.5));
        AssertHsla(interpolated2, (180, 0.25, 0.75, 0.5));
        AssertHsla(interpolated3, (180, 0.25, 0.75, 0.5));
        AssertHsla(interpolated4, (180, 0.25, 0.75, 0.5));
    }

    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromHsl(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHsl(180, 1, 1);
        var interpolated1 = unicolour1.InterpolateHsl(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateHsl(unicolour1, 0.5);
        
        AssertHsla(interpolated1, (90, 0.5, 0.5, 0.5));
        AssertHsla(interpolated2, (90, 0.5, 0.5, 0.5));
    }
    
    [Test]
    public void EquidistantViaRed()
    {
        var unicolour1 = Unicolour.FromHsl(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHsl(340, 0.5, 0.8, 0.2);
        var interpolated1 = unicolour1.InterpolateHsl(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateHsl(unicolour1, 0.5);
        
        AssertHsla(interpolated1, (350, 0.25, 0.4, 0.1));
        AssertHsla(interpolated2, (350, 0.25, 0.4, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromHsl(0, 1, 0);
        var unicolour2 = Unicolour.FromHsl(180, 0, 1, 0.5);
        var interpolated1 = unicolour1.InterpolateHsl(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateHsl(unicolour1, 0.75);

        AssertHsla(interpolated1, (135, 0.25, 0.75, 0.625));
        AssertHsla(interpolated2, (45, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaRed()
    {
        var unicolour1 = Unicolour.FromHsl(300, 1, 0);
        var unicolour2 = Unicolour.FromHsl(60, 0, 1, 0.5);
        var interpolated1 = unicolour1.InterpolateHsl(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateHsl(unicolour1, 0.75);

        AssertHsla(interpolated1, (30, 0.25, 0.75, 0.625));
        AssertHsla(interpolated2, (330, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromHsl(0, 1, 0);
        var unicolour2 = Unicolour.FromHsl(180, 0, 1, 0.5);
        var interpolated1 = unicolour1.InterpolateHsl(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHsl(unicolour1, 0.25);
        
        AssertHsla(interpolated1, (45, 0.75, 0.25, 0.875));
        AssertHsla(interpolated2, (135, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaRed()
    {
        var unicolour1 = Unicolour.FromHsl(300, 1, 0);
        var unicolour2 = Unicolour.FromHsl(60, 0, 1, 0.5);
        var interpolated1 = unicolour1.InterpolateHsl(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHsl(unicolour1, 0.25);
        
        AssertHsla(interpolated1, (330, 0.75, 0.25, 0.875));
        AssertHsla(interpolated2, (30, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromHsl(0, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromHsl(90, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateHsl(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateHsl(unicolour1, 1.5);

        AssertHsla(interpolated1, (135, 0.7, 0.3, 0.95));
        AssertHsla(interpolated2, (315, 0.3, 0.7, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromHsl(0, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromHsl(90, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateHsl(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateHsl(unicolour1, -0.5);

        AssertHsla(interpolated1, (315, 0.3, 0.7, 0.75));
        AssertHsla(interpolated2, (135, 0.7, 0.3, 0.95));
    }
    
    private static void AssertHsla(Unicolour unicolour, (double h, double s, double l, double alpha) expected)
    {
        var actualHsl = unicolour.Hsl;
        var actualAlpha = unicolour.Alpha;
        
        Assert.That(actualHsl.H, Is.EqualTo(expected.h).Within(0.00000000005));
        Assert.That(actualHsl.S, Is.EqualTo(expected.s).Within(0.00000000005));
        Assert.That(actualHsl.L, Is.EqualTo(expected.l).Within(0.00000000005));
        Assert.That(actualAlpha.A, Is.EqualTo(expected.alpha).Within(0.00000000005));
    }
}