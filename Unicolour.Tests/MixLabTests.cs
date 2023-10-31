namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixLabTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromLab(50, -64, 64, 0.5);
        var unicolour2 = Unicolour.FromLab(50, -64, 64, 0.5);
        var mixed1 = unicolour1.MixLab(unicolour2, 0.25);
        var mixed2 = unicolour2.MixLab(unicolour1, 0.75);
        var mixed3 = unicolour1.MixLab(unicolour2, 0.75);
        var mixed4 = unicolour2.MixLab(unicolour1, 0.25);
        
        AssertMixed(mixed1, (50, -64, 64, 0.5));
        AssertMixed(mixed2, (50, -64, 64, 0.5));
        AssertMixed(mixed3, (50, -64, 64, 0.5));
        AssertMixed(mixed4, (50, -64, 64, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromLab(0, -128, -128, 0.0);
        var unicolour2 = Unicolour.FromLab(50, 128, 128);
        var mixed1 = unicolour1.MixLab(unicolour2, 0.5);
        var mixed2 = unicolour2.MixLab(unicolour1, 0.5);
        
        AssertMixed(mixed1, (25, 0, 0, 0.5));
        AssertMixed(mixed2, (25, 0, 0, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromLab(0, 128, -128);
        var unicolour2 = Unicolour.FromLab(80, -128, 128, 0.5);
        var mixed1 = unicolour1.MixLab(unicolour2, 0.75);
        var mixed2 = unicolour2.MixLab(unicolour1, 0.75);

        AssertMixed(mixed1, (60, -64, 64, 0.625));
        AssertMixed(mixed2, (20, 64, -64, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromLab(0, 128, -128);
        var unicolour2 = Unicolour.FromLab(80, -128, 128, 0.5);
        var mixed1 = unicolour1.MixLab(unicolour2, 0.25);
        var mixed2 = unicolour2.MixLab(unicolour1, 0.25);
        
        AssertMixed(mixed1, (20, 64, -64, 0.875));
        AssertMixed(mixed2, (60, -64, 64, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromLab(20, -25.6, 25.6, 0.8);
        var unicolour2 = Unicolour.FromLab(30, 25.6, -25.6, 0.9);
        var mixed1 = unicolour1.MixLab(unicolour2, 1.5);
        var mixed2 = unicolour2.MixLab(unicolour1, 1.5);

        AssertMixed(mixed1, (35, 51.2, -51.2, 0.95));
        AssertMixed(mixed2, (15, -51.2, 51.2, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromLab(20, -25.6, 25.6, 0.8);
        var unicolour2 = Unicolour.FromLab(30, 25.6, -25.6, 0.9);
        var mixed1 = unicolour1.MixLab(unicolour2, -0.5);
        var mixed2 = unicolour2.MixLab(unicolour1, -0.5);

        AssertMixed(mixed1, (15, -51.2, 51.2, 0.75));
        AssertMixed(mixed2, (35, 51.2, -51.2, 0.95));
    }

    private static void AssertMixed(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertMixed(unicolour.Lab.Triplet, unicolour.Alpha.A, expected);
    }
}