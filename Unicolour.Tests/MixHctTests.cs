namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixHctTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromHct(180, 30, 75, 0.5);
        var unicolour2 = Unicolour.FromHct(180, 30, 75, 0.5);
        var mixed1 = unicolour1.MixHct(unicolour2, 0.25, false);
        var mixed2 = unicolour2.MixHct(unicolour1, 0.75, false);
        var mixed3 = unicolour1.MixHct(unicolour2, 0.75, false);
        var mixed4 = unicolour2.MixHct(unicolour1, 0.25, false);
        
        AssertMixed(mixed1, (180, 30, 75, 0.5));
        AssertMixed(mixed2, (180, 30, 75, 0.5));
        AssertMixed(mixed3, (180, 30, 75, 0.5));
        AssertMixed(mixed4, (180, 30, 75, 0.5));
    }

    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromHct(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHct(180, 120, 100);
        var mixed1 = unicolour1.MixHct(unicolour2, 0.5, false);
        var mixed2 = unicolour2.MixHct(unicolour1, 0.5, false);
        
        AssertMixed(mixed1, (90, 60, 50, 0.5));
        AssertMixed(mixed2, (90, 60, 50, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromHct(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHct(340, 60, 80, 0.2);
        var mixed1 = unicolour1.MixHct(unicolour2, 0.5, false);
        var mixed2 = unicolour2.MixHct(unicolour1, 0.5, false);
        
        AssertMixed(mixed1, (350, 30, 40, 0.1));
        AssertMixed(mixed2, (350, 30, 40, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromHct(0, 120, 0);
        var unicolour2 = Unicolour.FromHct(180, 0, 100, 0.5);
        var mixed1 = unicolour1.MixHct(unicolour2, 0.75, false);
        var mixed2 = unicolour2.MixHct(unicolour1, 0.75, false);

        AssertMixed(mixed1, (135, 30, 75, 0.625));
        AssertMixed(mixed2, (45, 90, 25, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromHct(300, 120, 0);
        var unicolour2 = Unicolour.FromHct(60, 0, 100, 0.5);
        var mixed1 = unicolour1.MixHct(unicolour2, 0.75, false);
        var mixed2 = unicolour2.MixHct(unicolour1, 0.75, false);

        AssertMixed(mixed1, (30, 30, 75, 0.625));
        AssertMixed(mixed2, (330, 90, 25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromHct(0, 120, 0);
        var unicolour2 = Unicolour.FromHct(180, 0, 100, 0.5);
        var mixed1 = unicolour1.MixHct(unicolour2, 0.25, false);
        var mixed2 = unicolour2.MixHct(unicolour1, 0.25, false);
        
        AssertMixed(mixed1, (45, 90, 25, 0.875));
        AssertMixed(mixed2, (135, 30, 75, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromHct(300, 120, 0);
        var unicolour2 = Unicolour.FromHct(60, 0, 100, 0.5);
        var mixed1 = unicolour1.MixHct(unicolour2, 0.25, false);
        var mixed2 = unicolour2.MixHct(unicolour1, 0.25, false);
        
        AssertMixed(mixed1, (330, 90, 25, 0.875));
        AssertMixed(mixed2, (30, 30, 75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromHct(0, 48, 60, 0.8);
        var unicolour2 = Unicolour.FromHct(90, 72, 40, 0.9);
        var mixed1 = unicolour1.MixHct(unicolour2, 1.5, false);
        var mixed2 = unicolour2.MixHct(unicolour1, 1.5, false);

        AssertMixed(mixed1, (135, 84, 30, 0.95));
        AssertMixed(mixed2, (315, 36, 70, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromHct(0, 48, 60, 0.8);
        var unicolour2 = Unicolour.FromHct(90, 72, 40, 0.9);
        var mixed1 = unicolour1.MixHct(unicolour2, -0.5, false);
        var mixed2 = unicolour2.MixHct(unicolour1, -0.5, false);

        AssertMixed(mixed1, (315, 36, 70, 0.75));
        AssertMixed(mixed2, (135, 84, 30, 0.95));
    }
    
    [Test]
    public void BeyondMaxAlpha()
    {
        var unicolour1 = Unicolour.FromHct(0, 0, 0, 0.5);
        var unicolour2 = Unicolour.FromHct(0, 0, 0, 1.5);
        var mixed = unicolour1.MixHct(unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(2.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(1.0));
    }
    
    [Test]
    public void BeyondMinAlpha()
    {
        var unicolour1 = Unicolour.FromHct(0, 0, 0, 0.5);
        var unicolour2 = Unicolour.FromHct(0, 0, 0, -0.5);
        var mixed = unicolour1.MixHct(unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(-1.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(0.0));
    }
    
    [TestCaseSource(typeof(AlphaInterpolationUtils), nameof(AlphaInterpolationUtils.PremultipliedHueIndex0))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        var unicolour1 = Unicolour.FromHct(start.Triplet.Tuple, start.Alpha);
        var unicolour2 = Unicolour.FromHct(end.Triplet.Tuple, end.Alpha);
        var mixed = unicolour1.MixHct(unicolour2, amount, premultiplyAlpha: true);
        AssertMixed(mixed, expected.Tuple);
    }

    private static void AssertMixed(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertMixed(unicolour.Hct.Triplet, unicolour.Alpha.A, expected);
    }
}