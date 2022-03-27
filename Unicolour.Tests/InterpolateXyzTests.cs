namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class InterpolateXyzTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromXyz(0.5, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromXyz(0.5, 0.25, 0.75, 0.5);
        var interpolated1 = unicolour1.InterpolateXyz(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateXyz(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateXyz(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateXyz(unicolour1, 0.25);
        
        AssertXyza(interpolated1, (0.5, 0.25, 0.75, 0.5));
        AssertXyza(interpolated2, (0.5, 0.25, 0.75, 0.5));
        AssertXyza(interpolated3, (0.5, 0.25, 0.75, 0.5));
        AssertXyza(interpolated4, (0.5, 0.25, 0.75, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromXyz(0.0, 0.0, 0.0, 0.0);
        var unicolour2 = Unicolour.FromXyz(0.5, 1.0, 1.0);
        var interpolated1 = unicolour1.InterpolateXyz(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateXyz(unicolour1, 0.5);
        
        AssertXyza(interpolated1, (0.25, 0.5, 0.5, 0.5));
        AssertXyza(interpolated2, (0.25, 0.5, 0.5, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromXyz(0, 1, 0);
        var unicolour2 = Unicolour.FromXyz(0.8, 0.0, 1.0, 0.5);
        var interpolated1 = unicolour1.InterpolateXyz(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateXyz(unicolour1, 0.75);

        AssertXyza(interpolated1, (0.6, 0.25, 0.75, 0.625));
        AssertXyza(interpolated2, (0.2, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromXyz(0.0, 1.0, 0.0);
        var unicolour2 = Unicolour.FromXyz(0.8, 0.0, 1.0, 0.5);
        var interpolated1 = unicolour1.InterpolateXyz(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateXyz(unicolour1, 0.25);
        
        AssertXyza(interpolated1, (0.2, 0.75, 0.25, 0.875));
        AssertXyza(interpolated2, (0.6, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromXyz(0.2, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromXyz(0.3, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateXyz(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateXyz(unicolour1, 1.5);

        AssertXyza(interpolated1, (0.35, 0.7, 0.3, 0.95));
        AssertXyza(interpolated2, (0.15, 0.3, 0.7, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromXyz(0.2, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromXyz(0.3, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateXyz(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateXyz(unicolour1, -0.5);

        AssertXyza(interpolated1, (0.15, 0.3, 0.7, 0.75));
        AssertXyza(interpolated2, (0.35, 0.7, 0.3, 0.95));
    }

    private static void AssertXyza(Unicolour unicolour, (double x, double y, double z, double alpha) expected)
    {
        var actualXyz = unicolour.Xyz;
        var actualAlpha = unicolour.Alpha;
        
        Assert.That(actualXyz.X, Is.EqualTo(expected.x).Within(0.00000000005));
        Assert.That(actualXyz.Y, Is.EqualTo(expected.y).Within(0.00000000005));
        Assert.That(actualXyz.Z, Is.EqualTo(expected.z).Within(0.00000000005));
        Assert.That(actualAlpha.A, Is.EqualTo(expected.alpha).Within(0.00000000005));
    }
}