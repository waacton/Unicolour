namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public class InterpolateOklabTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromOklab(0.5, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromOklab(0.5, 0.25, 0.75, 0.5);
        var interpolated1 = unicolour1.InterpolateOklab(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateOklab(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateOklab(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateOklab(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (0.5, 0.25, 0.75, 0.5));
        AssertInterpolated(interpolated2, (0.5, 0.25, 0.75, 0.5));
        AssertInterpolated(interpolated3, (0.5, 0.25, 0.75, 0.5));
        AssertInterpolated(interpolated4, (0.5, 0.25, 0.75, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromOklab(0.0, 0.0, 0.0, 0.0);
        var unicolour2 = Unicolour.FromOklab(0.5, 1.0, 1.0);
        var interpolated1 = unicolour1.InterpolateOklab(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateOklab(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (0.25, 0.5, 0.5, 0.5));
        AssertInterpolated(interpolated2, (0.25, 0.5, 0.5, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromOklab(0, 1, 0);
        var unicolour2 = Unicolour.FromOklab(0.8, 0.0, 1.0, 0.5);
        var interpolated1 = unicolour1.InterpolateOklab(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateOklab(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (0.6, 0.25, 0.75, 0.625));
        AssertInterpolated(interpolated2, (0.2, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromOklab(0.0, 1.0, 0.0);
        var unicolour2 = Unicolour.FromOklab(0.8, 0.0, 1.0, 0.5);
        var interpolated1 = unicolour1.InterpolateOklab(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateOklab(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (0.2, 0.75, 0.25, 0.875));
        AssertInterpolated(interpolated2, (0.6, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromOklab(0.2, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromOklab(0.3, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateOklab(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateOklab(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (0.35, 0.7, 0.3, 0.95));
        AssertInterpolated(interpolated2, (0.15, 0.3, 0.7, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromOklab(0.2, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromOklab(0.3, 0.6, 0.4, 0.9);
        var interpolated1 = unicolour1.InterpolateOklab(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateOklab(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (0.15, 0.3, 0.7, 0.75));
        AssertInterpolated(interpolated2, (0.35, 0.7, 0.3, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double x, double y, double z, double alpha) expected)
    {
        var actualOklab = unicolour.Oklab;
        var actualAlpha = unicolour.Alpha;
        
        Assert.That(actualOklab.L, Is.EqualTo(expected.x).Within(0.00000000005));
        Assert.That(actualOklab.A, Is.EqualTo(expected.y).Within(0.00000000005));
        Assert.That(actualOklab.B, Is.EqualTo(expected.z).Within(0.00000000005));
        Assert.That(actualAlpha.A, Is.EqualTo(expected.alpha).Within(0.00000000005));
    }
}