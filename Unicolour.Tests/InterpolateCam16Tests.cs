namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class InterpolateCam16Tests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromCam16(50, -25, 25, 0.5);
        var unicolour2 = Unicolour.FromCam16(50, -25, 25, 0.5);
        var interpolated1 = unicolour1.InterpolateCam16(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateCam16(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateCam16(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateCam16(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (50, -25, 25, 0.5));
        AssertInterpolated(interpolated2, (50, -25, 25, 0.5));
        AssertInterpolated(interpolated3, (50, -25, 25, 0.5));
        AssertInterpolated(interpolated4, (50, -25, 25, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromCam16(0, -50, -50, 0.0);
        var unicolour2 = Unicolour.FromCam16(50, 50, 50);
        var interpolated1 = unicolour1.InterpolateCam16(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateCam16(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (25, 0, 0, 0.5));
        AssertInterpolated(interpolated2, (25, 0, 0, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromCam16(0, 50, -50);
        var unicolour2 = Unicolour.FromCam16(80, -50, 50, 0.5);
        var interpolated1 = unicolour1.InterpolateCam16(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateCam16(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (60, -25, 25, 0.625));
        AssertInterpolated(interpolated2, (20, 25, -25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromCam16(0, 50, -50);
        var unicolour2 = Unicolour.FromCam16(80, -50, 50, 0.5);
        var interpolated1 = unicolour1.InterpolateCam16(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateCam16(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (20, 25, -25, 0.875));
        AssertInterpolated(interpolated2, (60, -25, 25, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromCam16(20, -10, 10, 0.8);
        var unicolour2 = Unicolour.FromCam16(30, 10, -10, 0.9);
        var interpolated1 = unicolour1.InterpolateCam16(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateCam16(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (35, 20, -20, 0.95));
        AssertInterpolated(interpolated2, (15, -20, 20, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromCam16(20, -10, 10, 0.8);
        var unicolour2 = Unicolour.FromCam16(30, 10, -10, 0.9);
        var interpolated1 = unicolour1.InterpolateCam16(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateCam16(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (15, -20, 20, 0.75));
        AssertInterpolated(interpolated2, (35, 20, -20, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertInterpolated(unicolour.Cam16.Triplet, unicolour.Alpha.A, expected);
    }
}