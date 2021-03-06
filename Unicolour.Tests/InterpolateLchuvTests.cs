namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public class InterpolateLchuvTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromLchuv(50, 50, 180, 0.5);
        var unicolour2 = Unicolour.FromLchuv(50, 50, 180, 0.5);
        var interpolated1 = unicolour1.InterpolateLchuv(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLchuv(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateLchuv(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateLchuv(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (50, 50, 180, 0.5));
        AssertInterpolated(interpolated2, (50, 50, 180, 0.5));
        AssertInterpolated(interpolated3, (50, 50, 180, 0.5));
        AssertInterpolated(interpolated4, (50, 50, 180, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromLchuv(0, 0, 0, 0.0);
        var unicolour2 = Unicolour.FromLchuv(50, 100, 180);
        var interpolated1 = unicolour1.InterpolateLchuv(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateLchuv(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (25, 50, 90, 0.5));
        AssertInterpolated(interpolated2, (25, 50, 90, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromLchuv(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromLchuv(80, 50, 340, 0.2);
        var interpolated1 = unicolour1.InterpolateLchuv(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateLchuv(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (40, 25, 350, 0.1));
        AssertInterpolated(interpolated2, (40, 25, 350, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromLchuv(0, 100, 0);
        var unicolour2 = Unicolour.FromLchuv(80, 0, 180, 0.5);
        var interpolated1 = unicolour1.InterpolateLchuv(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateLchuv(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (60, 25, 135, 0.625));
        AssertInterpolated(interpolated2, (20, 75, 45, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromLchuv(0, 100, 300);
        var unicolour2 = Unicolour.FromLchuv(80, 0, 60, 0.5);
        var interpolated1 = unicolour1.InterpolateLchuv(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateLchuv(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (60, 25, 30, 0.625));
        AssertInterpolated(interpolated2, (20, 75, 330, 0.875));
    }

    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromLchuv(0, 100, 0);
        var unicolour2 = Unicolour.FromLchuv(80, 0, 180, 0.5);
        var interpolated1 = unicolour1.InterpolateLchuv(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLchuv(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (20, 75, 45, 0.875));
        AssertInterpolated(interpolated2, (60, 25, 135, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromLchuv(0, 100, 300);
        var unicolour2 = Unicolour.FromLchuv(80, 0, 60, 0.5);
        var interpolated1 = unicolour1.InterpolateLchuv(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLchuv(unicolour1, 0.25);

        AssertInterpolated(interpolated1, (20, 75, 330, 0.875));
        AssertInterpolated(interpolated2, (60, 25, 30, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromLchuv(20, 40, 0, 0.8);
        var unicolour2 = Unicolour.FromLchuv(30, 60, 90, 0.9);
        var interpolated1 = unicolour1.InterpolateLchuv(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateLchuv(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (35, 70, 135, 0.95));
        AssertInterpolated(interpolated2, (15, 30, 315, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromLchuv(20, 40, 0, 0.8);
        var unicolour2 = Unicolour.FromLchuv(30, 60, 90, 0.9);
        var interpolated1 = unicolour1.InterpolateLchuv(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateLchuv(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (15, 30, 315, 0.75));
        AssertInterpolated(interpolated2, (35, 70, 135, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double l, double c, double h, double alpha) expected)
    {
        var actualLchuv = unicolour.Lchuv;
        var actualAlpha = unicolour.Alpha;
        
        Assert.That(actualLchuv.L, Is.EqualTo(expected.l).Within(0.00000000005));
        Assert.That(actualLchuv.C, Is.EqualTo(expected.c).Within(0.00000000005));
        Assert.That(actualLchuv.H, Is.EqualTo(expected.h).Within(0.00000000005));
        Assert.That(actualAlpha.A, Is.EqualTo(expected.alpha).Within(0.00000000005));
    }
}