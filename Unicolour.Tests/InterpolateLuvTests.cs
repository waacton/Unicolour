namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class InterpolateLuvTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromLuv(50, -50, 50, 0.5);
        var unicolour2 = Unicolour.FromLuv(50, -50, 50, 0.5);
        var interpolated1 = unicolour1.InterpolateLuv(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLuv(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateLuv(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateLuv(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (50, -50, 50, 0.5));
        AssertInterpolated(interpolated2, (50, -50, 50, 0.5));
        AssertInterpolated(interpolated3, (50, -50, 50, 0.5));
        AssertInterpolated(interpolated4, (50, -50, 50, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromLuv(0, -100, -100, 0.0);
        var unicolour2 = Unicolour.FromLuv(50, 100, 100);
        var interpolated1 = unicolour1.InterpolateLuv(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateLuv(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (25, 0, 0, 0.5));
        AssertInterpolated(interpolated2, (25, 0, 0, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromLuv(0, 100, -100);
        var unicolour2 = Unicolour.FromLuv(80, -100, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateLuv(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateLuv(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (60, -50, 50, 0.625));
        AssertInterpolated(interpolated2, (20, 50, -50, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromLuv(0, 100, -100);
        var unicolour2 = Unicolour.FromLuv(80, -100, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateLuv(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLuv(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (20, 50, -50, 0.875));
        AssertInterpolated(interpolated2, (60, -50, 50, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromLuv(20, -25.6, 25.6, 0.8);
        var unicolour2 = Unicolour.FromLuv(30, 25.6, -25.6, 0.9);
        var interpolated1 = unicolour1.InterpolateLuv(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateLuv(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (35, 51.2, -51.2, 0.95));
        AssertInterpolated(interpolated2, (15, -51.2, 51.2, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromLuv(20, -25.6, 25.6, 0.8);
        var unicolour2 = Unicolour.FromLuv(30, 25.6, -25.6, 0.9);
        var interpolated1 = unicolour1.InterpolateLuv(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateLuv(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (15, -51.2, 51.2, 0.75));
        AssertInterpolated(interpolated2, (35, 51.2, -51.2, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertInterpolated(unicolour.Luv.Triplet, unicolour.Alpha.A, expected);
    }
}