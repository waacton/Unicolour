namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class InterpolateHctTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromHct(180, 30, 75, 0.5);
        var unicolour2 = Unicolour.FromHct(180, 30, 75, 0.5);
        var interpolated1 = unicolour1.InterpolateHct(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHct(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateHct(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateHct(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (180, 30, 75, 0.5));
        AssertInterpolated(interpolated2, (180, 30, 75, 0.5));
        AssertInterpolated(interpolated3, (180, 30, 75, 0.5));
        AssertInterpolated(interpolated4, (180, 30, 75, 0.5));
    }

    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromHct(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHct(180, 120, 100);
        var interpolated1 = unicolour1.InterpolateHct(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateHct(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (90, 60, 50, 0.5));
        AssertInterpolated(interpolated2, (90, 60, 50, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromHct(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHct(340, 60, 80, 0.2);
        var interpolated1 = unicolour1.InterpolateHct(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateHct(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (350, 30, 40, 0.1));
        AssertInterpolated(interpolated2, (350, 30, 40, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromHct(0, 120, 0);
        var unicolour2 = Unicolour.FromHct(180, 0, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateHct(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateHct(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (135, 30, 75, 0.625));
        AssertInterpolated(interpolated2, (45, 90, 25, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromHct(300, 120, 0);
        var unicolour2 = Unicolour.FromHct(60, 0, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateHct(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateHct(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (30, 30, 75, 0.625));
        AssertInterpolated(interpolated2, (330, 90, 25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromHct(0, 120, 0);
        var unicolour2 = Unicolour.FromHct(180, 0, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateHct(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHct(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (45, 90, 25, 0.875));
        AssertInterpolated(interpolated2, (135, 30, 75, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromHct(300, 120, 0);
        var unicolour2 = Unicolour.FromHct(60, 0, 100, 0.5);
        var interpolated1 = unicolour1.InterpolateHct(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateHct(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (330, 90, 25, 0.875));
        AssertInterpolated(interpolated2, (30, 30, 75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromHct(0, 48, 60, 0.8);
        var unicolour2 = Unicolour.FromHct(90, 72, 40, 0.9);
        var interpolated1 = unicolour1.InterpolateHct(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateHct(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (135, 84, 30, 0.95));
        AssertInterpolated(interpolated2, (315, 36, 70, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromHct(0, 48, 60, 0.8);
        var unicolour2 = Unicolour.FromHct(90, 72, 40, 0.9);
        var interpolated1 = unicolour1.InterpolateHct(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateHct(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (315, 36, 70, 0.75));
        AssertInterpolated(interpolated2, (135, 84, 30, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertInterpolated(unicolour.Hct.Triplet, unicolour.Alpha.A, expected);
    }
}