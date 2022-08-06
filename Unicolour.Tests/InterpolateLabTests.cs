namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

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
        
        AssertInterpolated(interpolated1, (50, -64, 64, 0.5));
        AssertInterpolated(interpolated2, (50, -64, 64, 0.5));
        AssertInterpolated(interpolated3, (50, -64, 64, 0.5));
        AssertInterpolated(interpolated4, (50, -64, 64, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromLab(0, -128, -128, 0.0);
        var unicolour2 = Unicolour.FromLab(50, 128, 128);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (25, 0, 0, 0.5));
        AssertInterpolated(interpolated2, (25, 0, 0, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromLab(0, 128, -128);
        var unicolour2 = Unicolour.FromLab(80, -128, 128, 0.5);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (60, -64, 64, 0.625));
        AssertInterpolated(interpolated2, (20, 64, -64, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromLab(0, 128, -128);
        var unicolour2 = Unicolour.FromLab(80, -128, 128, 0.5);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (20, 64, -64, 0.875));
        AssertInterpolated(interpolated2, (60, -64, 64, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromLab(20, -25.6, 25.6, 0.8);
        var unicolour2 = Unicolour.FromLab(30, 25.6, -25.6, 0.9);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (35, 51.2, -51.2, 0.95));
        AssertInterpolated(interpolated2, (15, -51.2, 51.2, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromLab(20, -25.6, 25.6, 0.8);
        var unicolour2 = Unicolour.FromLab(30, 25.6, -25.6, 0.9);
        var interpolated1 = unicolour1.InterpolateLab(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateLab(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (15, -51.2, 51.2, 0.75));
        AssertInterpolated(interpolated2, (35, 51.2, -51.2, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertInterpolated(unicolour.Lab.Triplet, unicolour.Alpha.A, expected);
    }
}