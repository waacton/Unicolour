namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public class InterpolateHsbTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromHsb(180, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromHsb(180, 0.25, 0.75, 0.5);
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHsb(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateHsb(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateHsb(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (180, 0.25, 0.75, 0.5));
        AssertInterpolated(interpolated2, (180, 0.25, 0.75, 0.5));
        AssertInterpolated(interpolated3, (180, 0.25, 0.75, 0.5));
        AssertInterpolated(interpolated4, (180, 0.25, 0.75, 0.5));
    }

    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHsb(180, 1, 1);
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateHsb(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (90, 0.5, 0.5, 0.5));
        AssertInterpolated(interpolated2, (90, 0.5, 0.5, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHsb(340, 0.5, 0.8, 0.2);
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateHsb(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (350, 0.25, 0.4, 0.1));
        AssertInterpolated(interpolated2, (350, 0.25, 0.4, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 1, 0);
        var unicolour2 = Unicolour.FromHsb(180, 0, 1, 0.5);
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateHsb(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (135, 0.25, 0.75, 0.625));
        AssertInterpolated(interpolated2, (45, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromHsb(300, 1, 0);
        var unicolour2 = Unicolour.FromHsb(60, 0, 1, 0.5);
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateHsb(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (30, 0.25, 0.75, 0.625));
        AssertInterpolated(interpolated2, (330, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 1, 0);
        var unicolour2 = Unicolour.FromHsb(180, 0, 1, 0.5);
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHsb(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (45, 0.75, 0.25, 0.875));
        AssertInterpolated(interpolated2, (135, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromHsb(300, 1, 0);
        var unicolour2 = Unicolour.FromHsb(60, 0, 1, 0.5);
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHsb(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (330, 0.75, 0.25, 0.875));
        AssertInterpolated(interpolated2, (30, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromHsb(90, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateHsb(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (135, 0.7, 0.3, 0.95));
        AssertInterpolated(interpolated2, (315, 0.3, 0.7, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromHsb(90, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateHsb(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (315, 0.3, 0.7, 0.75));
        AssertInterpolated(interpolated2, (135, 0.7, 0.3, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double h, double s, double b, double alpha) expected)
    {
        var actualHsb = unicolour.Hsb;
        var actualAlpha = unicolour.Alpha;
        
        Assert.That(actualHsb.H, Is.EqualTo(expected.h).Within(0.00000000005));
        Assert.That(actualHsb.S, Is.EqualTo(expected.s).Within(0.00000000005));
        Assert.That(actualHsb.B, Is.EqualTo(expected.b).Within(0.00000000005));
        Assert.That(actualAlpha.A, Is.EqualTo(expected.alpha).Within(0.00000000005));
    }
}