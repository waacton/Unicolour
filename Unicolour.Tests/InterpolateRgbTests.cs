namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public class InterpolateRgbTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromRgb(0.5, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromRgb(0.5, 0.25, 0.75, 0.5);
        var interpolated1 = unicolour1.InterpolateRgb(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateRgb(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateRgb(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateRgb(unicolour1, 0.25);
        
        AssertRgba(interpolated1, (0.5, 0.25, 0.75, 0.5));
        AssertRgba(interpolated2, (0.5, 0.25, 0.75, 0.5));
        AssertRgba(interpolated3, (0.5, 0.25, 0.75, 0.5));
        AssertRgba(interpolated4, (0.5, 0.25, 0.75, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromRgb(0.0, 0.0, 0.0, 0.0);
        var unicolour2 = Unicolour.FromRgb(0.5, 1.0, 1.0);
        var interpolated1 = unicolour1.InterpolateRgb(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateRgb(unicolour1, 0.5);
        
        AssertRgba(interpolated1, (0.25, 0.5, 0.5, 0.5));
        AssertRgba(interpolated2, (0.25, 0.5, 0.5, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromRgb(0, 1, 0);
        var unicolour2 = Unicolour.FromRgb(0.8, 0.0, 1.0, 0.5);
        var interpolated1 = unicolour1.InterpolateRgb(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateRgb(unicolour1, 0.75);

        AssertRgba(interpolated1, (0.6, 0.25, 0.75, 0.625));
        AssertRgba(interpolated2, (0.2, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromRgb(0.0, 1.0, 0.0);
        var unicolour2 = Unicolour.FromRgb(0.8, 0.0, 1.0, 0.5);
        var interpolated1 = unicolour1.InterpolateRgb(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateRgb(unicolour1, 0.25);
        
        AssertRgba(interpolated1, (0.2, 0.75, 0.25, 0.875));
        AssertRgba(interpolated2, (0.6, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromRgb(0.2, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromRgb(0.3, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateRgb(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateRgb(unicolour1, 1.5);

        AssertRgba(interpolated1, (0.35, 0.7, 0.3, 0.95));
        AssertRgba(interpolated2, (0.15, 0.3, 0.7, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromRgb(0.2, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromRgb(0.3, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateRgb(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateRgb(unicolour1, -0.5);

        AssertRgba(interpolated1, (0.15, 0.3, 0.7, 0.75));
        AssertRgba(interpolated2, (0.35, 0.7, 0.3, 0.95));
    }

    private static void AssertRgba(Unicolour unicolour, (double r, double g, double b, double alpha) expected)
    {
        var actualRgb = unicolour.Rgb;
        var actualAlpha = unicolour.Alpha;
        
        Assert.That(actualRgb.R, Is.EqualTo(expected.r).Within(0.00000000005));
        Assert.That(actualRgb.G, Is.EqualTo(expected.g).Within(0.00000000005));
        Assert.That(actualRgb.B, Is.EqualTo(expected.b).Within(0.00000000005));
        Assert.That(actualAlpha.A, Is.EqualTo(expected.alpha).Within(0.00000000005));
    }
}