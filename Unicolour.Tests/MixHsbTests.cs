namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixHsbTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromHsb(180, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromHsb(180, 0.25, 0.75, 0.5);
        var mixed1 = unicolour1.MixHsb(unicolour2, 0.25);
        var mixed2 = unicolour2.MixHsb(unicolour1, 0.75);
        var mixed3 = unicolour1.MixHsb(unicolour2, 0.75);
        var mixed4 = unicolour2.MixHsb(unicolour1, 0.25);
        
        AssertMixed(mixed1, (180, 0.25, 0.75, 0.5));
        AssertMixed(mixed2, (180, 0.25, 0.75, 0.5));
        AssertMixed(mixed3, (180, 0.25, 0.75, 0.5));
        AssertMixed(mixed4, (180, 0.25, 0.75, 0.5));
    }

    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHsb(180, 1, 1);
        var mixed1 = unicolour1.MixHsb(unicolour2, 0.5);
        var mixed2 = unicolour2.MixHsb(unicolour1, 0.5);
        
        AssertMixed(mixed1, (90, 0.5, 0.5, 0.5));
        AssertMixed(mixed2, (90, 0.5, 0.5, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHsb(340, 0.5, 0.8, 0.2);
        var mixed1 = unicolour1.MixHsb(unicolour2, 0.5);
        var mixed2 = unicolour2.MixHsb(unicolour1, 0.5);
        
        AssertMixed(mixed1, (350, 0.25, 0.4, 0.1));
        AssertMixed(mixed2, (350, 0.25, 0.4, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 1, 0);
        var unicolour2 = Unicolour.FromHsb(180, 0, 1, 0.5);
        var mixed1 = unicolour1.MixHsb(unicolour2, 0.75);
        var mixed2 = unicolour2.MixHsb(unicolour1, 0.75);

        AssertMixed(mixed1, (135, 0.25, 0.75, 0.625));
        AssertMixed(mixed2, (45, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromHsb(300, 1, 0);
        var unicolour2 = Unicolour.FromHsb(60, 0, 1, 0.5);
        var mixed1 = unicolour1.MixHsb(unicolour2, 0.75);
        var mixed2 = unicolour2.MixHsb(unicolour1, 0.75);

        AssertMixed(mixed1, (30, 0.25, 0.75, 0.625));
        AssertMixed(mixed2, (330, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 1, 0);
        var unicolour2 = Unicolour.FromHsb(180, 0, 1, 0.5);
        var mixed1 = unicolour1.MixHsb(unicolour2, 0.25);
        var mixed2 = unicolour2.MixHsb(unicolour1, 0.25);
        
        AssertMixed(mixed1, (45, 0.75, 0.25, 0.875));
        AssertMixed(mixed2, (135, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromHsb(300, 1, 0);
        var unicolour2 = Unicolour.FromHsb(60, 0, 1, 0.5);
        var mixed1 = unicolour1.MixHsb(unicolour2, 0.25);
        var mixed2 = unicolour2.MixHsb(unicolour1, 0.25);
        
        AssertMixed(mixed1, (330, 0.75, 0.25, 0.875));
        AssertMixed(mixed2, (30, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromHsb(90, 0.6, 0.4, 0.9);
        var mixed1 = unicolour1.MixHsb(unicolour2, 1.5);
        var mixed2 = unicolour2.MixHsb(unicolour1, 1.5);

        AssertMixed(mixed1, (135, 0.7, 0.3, 0.95));
        AssertMixed(mixed2, (315, 0.3, 0.7, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0.4, 0.6, 0.8);
        var unicolour2 = Unicolour.FromHsb(90, 0.6, 0.4, 0.9);
        var mixed1 = unicolour1.MixHsb(unicolour2, -0.5);
        var mixed2 = unicolour2.MixHsb(unicolour1, -0.5);

        AssertMixed(mixed1, (315, 0.3, 0.7, 0.75));
        AssertMixed(mixed2, (135, 0.7, 0.3, 0.95));
    }

    private static void AssertMixed(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertMixed(unicolour.Hsb.Triplet, unicolour.Alpha.A, expected);
    }
}