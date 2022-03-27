namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class InterpolateLabTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromLab(50, -64, 64, 0.5);
        var unicolour2 = Unicolour.FromLab(50, -64, 64, 0.5);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateLab(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateLab(unicolour1, 0.25);
        
        AssertLaba(interpolated1, (50, -64, 64, 0.5));
        AssertLaba(interpolated2, (50, -64, 64, 0.5));
        AssertLaba(interpolated3, (50, -64, 64, 0.5));
        AssertLaba(interpolated4, (50, -64, 64, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromLab(0, -128, -128, 0.0);
        var unicolour2 = Unicolour.FromLab(50, 128, 128);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, 0.5);
        
        AssertLaba(interpolated1, (25, 0, 0, 0.5));
        AssertLaba(interpolated2, (25, 0, 0, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromLab(0, 128, -128);
        var unicolour2 = Unicolour.FromLab(80, -128, 128, 0.5);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, 0.75);

        AssertLaba(interpolated1, (60, -64, 64, 0.625));
        AssertLaba(interpolated2, (20, 64, -64, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromLab(0, 128, -128);
        var unicolour2 = Unicolour.FromLab(80, -128, 128, 0.5);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, 0.25);
        
        AssertLaba(interpolated1, (20, 64, -64, 0.875));
        AssertLaba(interpolated2, (60, -64, 64, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromLab(20, -25.6, 25.6, 0.8);
        var unicolour2 = Unicolour.FromLab(30, 25.6, -25.6, 0.9);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, 1.5);

        AssertLaba(interpolated1, (35, 51.2, -51.2, 0.95));
        AssertLaba(interpolated2, (15, -51.2, 51.2, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromLab(20, -25.6, 25.6, 0.8);
        var unicolour2 = Unicolour.FromLab(30, 25.6, -25.6, 0.9);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, -0.5);

        AssertLaba(interpolated1, (15, -51.2, 51.2, 0.75));
        AssertLaba(interpolated2, (35, 51.2, -51.2, 0.95));
    }

    private static void AssertLaba(Unicolour unicolour, (double l, double a, double b, double alpha) expected)
    {
        var actualLab = unicolour.Lab;
        var actualAlpha = unicolour.Alpha;
        
        Assert.That(actualLab.L, Is.EqualTo(expected.l).Within(0.00000000005));
        Assert.That(actualLab.A, Is.EqualTo(expected.a).Within(0.00000000005));
        Assert.That(actualLab.B, Is.EqualTo(expected.b).Within(0.00000000005));
        Assert.That(actualAlpha.A, Is.EqualTo(expected.alpha).Within(0.00000000005));
    }
}