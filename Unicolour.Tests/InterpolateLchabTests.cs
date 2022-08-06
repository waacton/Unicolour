namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class InterpolateLchabTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromLchab(50, 50, 180, 0.5);
        var unicolour2 = Unicolour.FromLchab(50, 50, 180, 0.5);
        var interpolated1 = unicolour1.InterpolateLchab(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLchab(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateLchab(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateLchab(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (50, 50, 180, 0.5));
        AssertInterpolated(interpolated2, (50, 50, 180, 0.5));
        AssertInterpolated(interpolated3, (50, 50, 180, 0.5));
        AssertInterpolated(interpolated4, (50, 50, 180, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromLchab(0, 0, 0, 0.0);
        var unicolour2 = Unicolour.FromLchab(50, 100, 180);
        var interpolated1 = unicolour1.InterpolateLchab(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateLchab(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (25, 50, 90, 0.5));
        AssertInterpolated(interpolated2, (25, 50, 90, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromLchab(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromLchab(80, 50, 340, 0.2);
        var interpolated1 = unicolour1.InterpolateLchab(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateLchab(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (40, 25, 350, 0.1));
        AssertInterpolated(interpolated2, (40, 25, 350, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromLchab(0, 100, 0);
        var unicolour2 = Unicolour.FromLchab(80, 0, 180, 0.5);
        var interpolated1 = unicolour1.InterpolateLchab(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateLchab(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (60, 25, 135, 0.625));
        AssertInterpolated(interpolated2, (20, 75, 45, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromLchab(0, 100, 300);
        var unicolour2 = Unicolour.FromLchab(80, 0, 60, 0.5);
        var interpolated1 = unicolour1.InterpolateLchab(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateLchab(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (60, 25, 30, 0.625));
        AssertInterpolated(interpolated2, (20, 75, 330, 0.875));
    }

    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromLchab(0, 100, 0);
        var unicolour2 = Unicolour.FromLchab(80, 0, 180, 0.5);
        var interpolated1 = unicolour1.InterpolateLchab(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLchab(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (20, 75, 45, 0.875));
        AssertInterpolated(interpolated2, (60, 25, 135, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromLchab(0, 100, 300);
        var unicolour2 = Unicolour.FromLchab(80, 0, 60, 0.5);
        var interpolated1 = unicolour1.InterpolateLchab(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateLchab(unicolour1, 0.25);

        AssertInterpolated(interpolated1, (20, 75, 330, 0.875));
        AssertInterpolated(interpolated2, (60, 25, 30, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromLchab(20, 40, 0, 0.8);
        var unicolour2 = Unicolour.FromLchab(30, 60, 90, 0.9);
        var interpolated1 = unicolour1.InterpolateLchab(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateLchab(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (35, 70, 135, 0.95));
        AssertInterpolated(interpolated2, (15, 30, 315, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromLchab(20, 40, 0, 0.8);
        var unicolour2 = Unicolour.FromLchab(30, 60, 90, 0.9);
        var interpolated1 = unicolour1.InterpolateLchab(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateLchab(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (15, 30, 315, 0.75));
        AssertInterpolated(interpolated2, (35, 70, 135, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertInterpolated(unicolour.Lchab.Triplet, unicolour.Alpha.A, expected);
    }
}