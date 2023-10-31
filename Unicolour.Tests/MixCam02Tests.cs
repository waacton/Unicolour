namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixCam02Tests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromCam02(50, -25, 25, 0.5);
        var unicolour2 = Unicolour.FromCam02(50, -25, 25, 0.5);
        var mixed1 = unicolour1.MixCam02(unicolour2, 0.25);
        var mixed2 = unicolour2.MixCam02(unicolour1, 0.75);
        var mixed3 = unicolour1.MixCam02(unicolour2, 0.75);
        var mixed4 = unicolour2.MixCam02(unicolour1, 0.25);
        
        AssertMixed(mixed1, (50, -25, 25, 0.5));
        AssertMixed(mixed2, (50, -25, 25, 0.5));
        AssertMixed(mixed3, (50, -25, 25, 0.5));
        AssertMixed(mixed4, (50, -25, 25, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromCam02(0, -50, -50, 0.0);
        var unicolour2 = Unicolour.FromCam02(50, 50, 50);
        var mixed1 = unicolour1.MixCam02(unicolour2, 0.5);
        var mixed2 = unicolour2.MixCam02(unicolour1, 0.5);
        
        AssertMixed(mixed1, (25, 0, 0, 0.5));
        AssertMixed(mixed2, (25, 0, 0, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromCam02(0, 50, -50);
        var unicolour2 = Unicolour.FromCam02(80, -50, 50, 0.5);
        var mixed1 = unicolour1.MixCam02(unicolour2, 0.75);
        var mixed2 = unicolour2.MixCam02(unicolour1, 0.75);

        AssertMixed(mixed1, (60, -25, 25, 0.625));
        AssertMixed(mixed2, (20, 25, -25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromCam02(0, 50, -50);
        var unicolour2 = Unicolour.FromCam02(80, -50, 50, 0.5);
        var mixed1 = unicolour1.MixCam02(unicolour2, 0.25);
        var mixed2 = unicolour2.MixCam02(unicolour1, 0.25);
        
        AssertMixed(mixed1, (20, 25, -25, 0.875));
        AssertMixed(mixed2, (60, -25, 25, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromCam02(20, -10, 10, 0.8);
        var unicolour2 = Unicolour.FromCam02(30, 10, -10, 0.9);
        var mixed1 = unicolour1.MixCam02(unicolour2, 1.5);
        var mixed2 = unicolour2.MixCam02(unicolour1, 1.5);

        AssertMixed(mixed1, (35, 20, -20, 0.95));
        AssertMixed(mixed2, (15, -20, 20, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromCam02(20, -10, 10, 0.8);
        var unicolour2 = Unicolour.FromCam02(30, 10, -10, 0.9);
        var mixed1 = unicolour1.MixCam02(unicolour2, -0.5);
        var mixed2 = unicolour2.MixCam02(unicolour1, -0.5);

        AssertMixed(mixed1, (15, -20, 20, 0.75));
        AssertMixed(mixed2, (35, 20, -20, 0.95));
    }

    private static void AssertMixed(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertMixed(unicolour.Cam02.Triplet, unicolour.Alpha.A, expected);
    }
}