namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixHpluvTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromHpluv(180, 25, 75, 0.5);
        var unicolour2 = Unicolour.FromHpluv(180, 25, 75, 0.5);
        var mixed1 = unicolour1.MixHpluv(unicolour2, 0.25, false);
        var mixed2 = unicolour2.MixHpluv(unicolour1, 0.75, false);
        var mixed3 = unicolour1.MixHpluv(unicolour2, 0.75, false);
        var mixed4 = unicolour2.MixHpluv(unicolour1, 0.25, false);
        
        AssertMixed(mixed1, (180, 25, 75, 0.5));
        AssertMixed(mixed2, (180, 25, 75, 0.5));
        AssertMixed(mixed3, (180, 25, 75, 0.5));
        AssertMixed(mixed4, (180, 25, 75, 0.5));
    }

    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHpluv(180, 100, 100);
        var mixed1 = unicolour1.MixHpluv(unicolour2, 0.5, false);
        var mixed2 = unicolour2.MixHpluv(unicolour1, 0.5, false);
        
        AssertMixed(mixed1, (90, 50, 50, 0.5));
        AssertMixed(mixed2, (90, 50, 50, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromHpluv(340, 50, 80, 0.2);
        var mixed1 = unicolour1.MixHpluv(unicolour2, 0.5, false);
        var mixed2 = unicolour2.MixHpluv(unicolour1, 0.5, false);
        
        AssertMixed(mixed1, (350, 25, 40, 0.1));
        AssertMixed(mixed2, (350, 25, 40, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 100, 0);
        var unicolour2 = Unicolour.FromHpluv(180, 0, 100, 0.5);
        var mixed1 = unicolour1.MixHpluv(unicolour2, 0.75, false);
        var mixed2 = unicolour2.MixHpluv(unicolour1, 0.75, false);

        AssertMixed(mixed1, (135, 25, 75, 0.625));
        AssertMixed(mixed2, (45, 75, 25, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromHpluv(300, 100, 0);
        var unicolour2 = Unicolour.FromHpluv(60, 0, 100, 0.5);
        var mixed1 = unicolour1.MixHpluv(unicolour2, 0.75, false);
        var mixed2 = unicolour2.MixHpluv(unicolour1, 0.75, false);

        AssertMixed(mixed1, (30, 25, 75, 0.625));
        AssertMixed(mixed2, (330, 75, 25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 100, 0);
        var unicolour2 = Unicolour.FromHpluv(180, 0, 100, 0.5);
        var mixed1 = unicolour1.MixHpluv(unicolour2, 0.25, false);
        var mixed2 = unicolour2.MixHpluv(unicolour1, 0.25, false);
        
        AssertMixed(mixed1, (45, 75, 25, 0.875));
        AssertMixed(mixed2, (135, 25, 75, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromHpluv(300, 100, 0);
        var unicolour2 = Unicolour.FromHpluv(60, 0, 100, 0.5);
        var mixed1 = unicolour1.MixHpluv(unicolour2, 0.25, false);
        var mixed2 = unicolour2.MixHpluv(unicolour1, 0.25, false);
        
        AssertMixed(mixed1, (330, 75, 25, 0.875));
        AssertMixed(mixed2, (30, 25, 75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 40, 60, 0.8);
        var unicolour2 = Unicolour.FromHpluv(90, 60, 40, 0.9);
        var mixed1 = unicolour1.MixHpluv(unicolour2, 1.5, false);
        var mixed2 = unicolour2.MixHpluv(unicolour1, 1.5, false);

        AssertMixed(mixed1, (135, 70, 30, 0.95));
        AssertMixed(mixed2, (315, 30, 70, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 40, 60, 0.8);
        var unicolour2 = Unicolour.FromHpluv(90, 60, 40, 0.9);
        var mixed1 = unicolour1.MixHpluv(unicolour2, -0.5, false);
        var mixed2 = unicolour2.MixHpluv(unicolour1, -0.5, false);

        AssertMixed(mixed1, (315, 30, 70, 0.75));
        AssertMixed(mixed2, (135, 70, 30, 0.95));
    }
    
    [Test]
    public void BeyondMaxAlpha()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 0, 0, 0.5);
        var unicolour2 = Unicolour.FromHpluv(0, 0, 0, 1.5);
        var mixed = unicolour1.MixHpluv(unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(2.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(1.0));
    }
    
    [Test]
    public void BeyondMinAlpha()
    {
        var unicolour1 = Unicolour.FromHpluv(0, 0, 0, 0.5);
        var unicolour2 = Unicolour.FromHpluv(0, 0, 0, -0.5);
        var mixed = unicolour1.MixHpluv(unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(-1.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(0.0));
    }
    
    [TestCaseSource(typeof(AlphaInterpolationUtils), nameof(AlphaInterpolationUtils.PremultipliedHueIndex0))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        var unicolour1 = Unicolour.FromHpluv(start.Triplet.Tuple, start.Alpha);
        var unicolour2 = Unicolour.FromHpluv(end.Triplet.Tuple, end.Alpha);
        var mixed = unicolour1.MixHpluv(unicolour2, amount, premultiplyAlpha: true);
        AssertMixed(mixed, expected.Tuple);
    }
    
    private static void AssertMixed(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertMixed(unicolour.Hpluv.Triplet, unicolour.Alpha.A, expected);
    }
}