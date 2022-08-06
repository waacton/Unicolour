namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class InterpolateJzazbzTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromJzazbz(0.08, -0.05, 0.05, 0.5);
        var unicolour2 = Unicolour.FromJzazbz(0.08, -0.05, 0.05, 0.5);
        var interpolated1 = unicolour1.InterpolateJzazbz(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateJzazbz(unicolour1, 0.75);
        var interpolated3 = unicolour1.InterpolateJzazbz(unicolour2, 0.75);
        var interpolated4 = unicolour2.InterpolateJzazbz(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (0.08, -0.05, 0.05, 0.5));
        AssertInterpolated(interpolated2, (0.08, -0.05, 0.05, 0.5));
        AssertInterpolated(interpolated3, (0.08, -0.05, 0.05, 0.5));
        AssertInterpolated(interpolated4, (0.08, -0.05, 0.05, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromJzazbz(0.0, -0.1, -0.1, 0.0);
        var unicolour2 = Unicolour.FromJzazbz(0.08, 0.1, 0.1);
        var interpolated1 = unicolour1.InterpolateJzazbz(unicolour2, 0.5);
        var interpolated2 = unicolour2.InterpolateJzazbz(unicolour1, 0.5);
        
        AssertInterpolated(interpolated1, (0.04, 0.0, 0.0, 0.5));
        AssertInterpolated(interpolated2, (0.04, 0.0, 0.0, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromJzazbz(0, 0.1, -0.1);
        var unicolour2 = Unicolour.FromJzazbz(0.12, -0.1, 0.1, 0.5);
        var interpolated1 = unicolour1.InterpolateJzazbz(unicolour2, 0.75);
        var interpolated2 = unicolour2.InterpolateJzazbz(unicolour1, 0.75);

        AssertInterpolated(interpolated1, (0.09, -0.05, 0.05, 0.625));
        AssertInterpolated(interpolated2, (0.03, 0.05, -0.05, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromJzazbz(0, 0.1, -0.1);
        var unicolour2 = Unicolour.FromJzazbz(0.12, -0.1, 0.1, 0.5);
        var interpolated1 = unicolour1.InterpolateJzazbz(unicolour2, 0.25);
        var interpolated2 = unicolour2.InterpolateJzazbz(unicolour1, 0.25);
        
        AssertInterpolated(interpolated1, (0.03, 0.05, -0.05, 0.875));
        AssertInterpolated(interpolated2, (0.09, -0.05, 0.05, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromJzazbz(0.02, -0.025, 0.025, 0.8);
        var unicolour2 = Unicolour.FromJzazbz(0.03, 0.025, -0.025, 0.9);
        var interpolated1 = unicolour1.InterpolateJzazbz(unicolour2, 1.5);
        var interpolated2 = unicolour2.InterpolateJzazbz(unicolour1, 1.5);

        AssertInterpolated(interpolated1, (0.035, 0.05, -0.05, 0.95));
        AssertInterpolated(interpolated2, (0.015, -0.05, 0.05, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromJzazbz(0.02, -0.025, 0.025, 0.8);
        var unicolour2 = Unicolour.FromJzazbz(0.03, 0.025, -0.025, 0.9);
        var interpolated1 = unicolour1.InterpolateJzazbz(unicolour2, -0.5);
        var interpolated2 = unicolour2.InterpolateJzazbz(unicolour1, -0.5);

        AssertInterpolated(interpolated1, (0.015, -0.05, 0.05, 0.75));
        AssertInterpolated(interpolated2, (0.035, 0.05, -0.05, 0.95));
    }

    private static void AssertInterpolated(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertInterpolated(unicolour.Jzazbz.Triplet, unicolour.Alpha.A, expected);
    }
}