namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class InterpolateHpluvTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromHpluv(180, 25, 75, 0.5);
        var unicolour2 = Unicolour.FromHpluv(180, 25, 75, 0.5);
        var interpolated1 = unicolour1.InterpolateHpluv(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHpluv(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateHpluv(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateHpluv(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (180, 25, 75, 0.5));
        AssertInterpolated(interpolated2, (180, 25, 75, 0.5));
        AssertInterpolated(interpolated3, (180, 25, 75, 0.5));
        AssertInterpolated(interpolated4, (180, 25, 75, 0.5));
    }

    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHpluv(180, 100, 100);
        var interpolated1 = unicolour1.InterpolateHpluv(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateHpluv(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (90, 50, 50, 0.5));
        AssertInterpolated(interpolated2, (90, 50, 50, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHpluv(340, 50, 80, 0.2);
        var interpolated1 = unicolour1.InterpolateHpluv(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateHpluv(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (350, 25, 40, 0.1));
        AssertInterpolated(interpolated2, (350, 25, 40, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 100, 0);
        var unicolour2 = Unicolour.FromHpluv(180, 0, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateHpluv(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateHpluv(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (135, 25, 75, 0.625));
        AssertInterpolated(interpolated2, (45, 75, 25, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromHpluv(300, 100, 0);
        var unicolour2 = Unicolour.FromHpluv(60, 0, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateHpluv(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateHpluv(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (30, 25, 75, 0.625));
        AssertInterpolated(interpolated2, (330, 75, 25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 100, 0);
        var unicolour2 = Unicolour.FromHpluv(180, 0, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateHpluv(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHpluv(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (45, 75, 25, 0.875));
        AssertInterpolated(interpolated2, (135, 25, 75, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromHpluv(300, 100, 0);
        var unicolour2 = Unicolour.FromHpluv(60, 0, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateHpluv(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHpluv(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (330, 75, 25, 0.875));
        AssertInterpolated(interpolated2, (30, 25, 75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 40, 60, 0.8);
        var unicolour2 = Unicolour.FromHpluv(90, 60, 40, 0.9);
        var interpolated1 = unicolour1.InterpolateHpluv(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateHpluv(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (135, 70, 30, 0.95));
        AssertInterpolated(interpolated2, (315, 30, 70, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 40, 60, 0.8);
        var unicolour2 = Unicolour.FromHpluv(90, 60, 40, 0.9);
        var interpolated1 = unicolour1.InterpolateHpluv(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateHpluv(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (315, 30, 70, 0.75));
        AssertInterpolated(interpolated2, (135, 70, 30, 0.95));
    }
    
    private static void AssertInterpolated(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertInterpolated(unicolour.Hpluv.Triplet, unicolour.Alpha.A, expected);
    }
}