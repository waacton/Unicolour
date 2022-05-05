namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public class InterpolateOklchTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromOklch(0.5, 0.25, 180, 0.5);
        var unicolour2 = Unicolour.FromOklch(0.5, 0.25, 180, 0.5);
        var interpolated1 = unicolour1.InterpolateOklch(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateOklch(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateOklch(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateOklch(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (0.5, 0.25, 180, 0.5));
        AssertInterpolated(interpolated2, (0.5, 0.25, 180, 0.5));
        AssertInterpolated(interpolated3, (0.5, 0.25, 180, 0.5));
        AssertInterpolated(interpolated4, (0.5, 0.25, 180, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromOklch(0, 0, 0, 0.0);
        var unicolour2 = Unicolour.FromOklch(0.5, 0.5, 180);
        var interpolated1 = unicolour1.InterpolateOklch(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateOklch(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (0.25, 0.25, 90, 0.5));
        AssertInterpolated(interpolated2, (0.25, 0.25, 90, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromOklch(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromOklch(0.8, 0.25, 340, 0.2);
        var interpolated1 = unicolour1.InterpolateOklch(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateOklch(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (0.4, 0.125, 350, 0.1));
        AssertInterpolated(interpolated2, (0.4, 0.125, 350, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromOklch(0, 0.5, 0);
        var unicolour2 = Unicolour.FromOklch(0.8, 0, 180, 0.5);
        var interpolated1 = unicolour1.InterpolateOklch(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateOklch(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (0.6, 0.125, 135, 0.625));
        AssertInterpolated(interpolated2, (0.2, 0.375, 45, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromOklch(0, 0.5, 300);
        var unicolour2 = Unicolour.FromOklch(0.8, 0, 60, 0.5);
        var interpolated1 = unicolour1.InterpolateOklch(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateOklch(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (0.6, 0.125, 30, 0.625));
        AssertInterpolated(interpolated2, (0.2, 0.375, 330, 0.875));
    }

    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromOklch(0, 0.5, 0);
        var unicolour2 = Unicolour.FromOklch(0.8, 0, 180, 0.5);
        var interpolated1 = unicolour1.InterpolateOklch(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateOklch(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (0.2, 0.375, 45, 0.875));
        AssertInterpolated(interpolated2, (0.6, 0.125, 135, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromOklch(0, 0.5, 300);
        var unicolour2 = Unicolour.FromOklch(0.8, 0, 60, 0.5);
        var interpolated1 = unicolour1.InterpolateOklch(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateOklch(unicolour1, 0.25);

        AssertInterpolated(interpolated1, (0.2, 0.375, 330, 0.875));
        AssertInterpolated(interpolated2, (0.6, 0.125, 30, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromOklch(0.2, 0.2, 0, 0.8);
        var unicolour2 = Unicolour.FromOklch(0.3, 0.3, 90, 0.9);
        var interpolated1 = unicolour1.InterpolateOklch(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateOklch(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (0.35, 0.35, 135, 0.95));
        AssertInterpolated(interpolated2, (0.15, 0.15, 315, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromOklch(0.2, 0.2, 0, 0.8);
        var unicolour2 = Unicolour.FromOklch(0.3, 0.3, 90, 0.9);
        var interpolated1 = unicolour1.InterpolateOklch(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateOklch(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (0.15, 0.15, 315, 0.75));
        AssertInterpolated(interpolated2, (0.35, 0.35, 135, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double l, double c, double h, double alpha) expected)
    {
        var actualOklch = unicolour.Oklch;
        var actualAlpha = unicolour.Alpha;
        
        Assert.That(actualOklch.L, Is.EqualTo(expected.l).Within(0.00000000005));
        Assert.That(actualOklch.C, Is.EqualTo(expected.c).Within(0.00000000005));
        Assert.That(actualOklch.H, Is.EqualTo(expected.h).Within(0.00000000005));
        Assert.That(actualAlpha.A, Is.EqualTo(expected.alpha).Within(0.00000000005));
    }
}