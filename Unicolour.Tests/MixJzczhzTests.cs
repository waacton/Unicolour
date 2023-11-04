namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixJzczhzTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = Unicolour.FromJzczhz(0.08, 0.08, 180, 0.5);
        var unicolour2 = Unicolour.FromJzczhz(0.08, 0.08, 180, 0.5);
        var mixed1 = unicolour1.MixJzczhz(unicolour2, 0.25, false);
        var mixed2 = unicolour2.MixJzczhz(unicolour1, 0.75, false);
        var mixed3 = unicolour1.MixJzczhz(unicolour2, 0.75, false);
        var mixed4 = unicolour2.MixJzczhz(unicolour1, 0.25, false);
        
        AssertMixed(mixed1, (0.08, 0.08, 180, 0.5));
        AssertMixed(mixed2, (0.08, 0.08, 180, 0.5));
        AssertMixed(mixed3, (0.08, 0.08, 180, 0.5));
        AssertMixed(mixed4, (0.08, 0.08, 180, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = Unicolour.FromJzczhz(0, 0, 0, 0.0);
        var unicolour2 = Unicolour.FromJzczhz(0.08, 0.08, 180);
        var mixed1 = unicolour1.MixJzczhz(unicolour2, 0.5, false);
        var mixed2 = unicolour2.MixJzczhz(unicolour1, 0.5, false);
        
        AssertMixed(mixed1, (0.04, 0.04, 90, 0.5));
        AssertMixed(mixed2, (0.04, 0.04, 90, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = Unicolour.FromJzczhz(0, 0, 0, 0);
        var unicolour2 = Unicolour.FromJzczhz(0.12, 0.04, 340, 0.2);
        var mixed1 = unicolour1.MixJzczhz(unicolour2, 0.5, false);
        var mixed2 = unicolour2.MixJzczhz(unicolour1, 0.5, false);
        
        AssertMixed(mixed1, (0.06, 0.02, 350, 0.1));
        AssertMixed(mixed2, (0.06, 0.02, 350, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = Unicolour.FromJzczhz(0, 0.08, 0);
        var unicolour2 = Unicolour.FromJzczhz(0.12, 0, 180, 0.5);
        var mixed1 = unicolour1.MixJzczhz(unicolour2, 0.75, false);
        var mixed2 = unicolour2.MixJzczhz(unicolour1, 0.75, false);

        AssertMixed(mixed1, (0.09, 0.02, 135, 0.625));
        AssertMixed(mixed2, (0.03, 0.06, 45, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = Unicolour.FromJzczhz(0, 0.08, 300);
        var unicolour2 = Unicolour.FromJzczhz(0.12, 0, 60, 0.5);
        var mixed1 = unicolour1.MixJzczhz(unicolour2, 0.75, false);
        var mixed2 = unicolour2.MixJzczhz(unicolour1, 0.75, false);

        AssertMixed(mixed1, (0.09, 0.02, 30, 0.625));
        AssertMixed(mixed2, (0.03, 0.06, 330, 0.875));
    }

    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = Unicolour.FromJzczhz(0, 0.08, 0);
        var unicolour2 = Unicolour.FromJzczhz(0.12, 0, 180, 0.5);
        var mixed1 = unicolour1.MixJzczhz(unicolour2, 0.25, false);
        var mixed2 = unicolour2.MixJzczhz(unicolour1, 0.25, false);
        
        AssertMixed(mixed1, (0.03, 0.06, 45, 0.875));
        AssertMixed(mixed2, (0.09, 0.02, 135, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = Unicolour.FromJzczhz(0, 0.08, 300);
        var unicolour2 = Unicolour.FromJzczhz(0.12, 0, 60, 0.5);
        var mixed1 = unicolour1.MixJzczhz(unicolour2, 0.25, false);
        var mixed2 = unicolour2.MixJzczhz(unicolour1, 0.25, false);

        AssertMixed(mixed1, (0.03, 0.06, 330, 0.875));
        AssertMixed(mixed2, (0.09, 0.02, 30, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = Unicolour.FromJzczhz(0.02, 0.02, 0, 0.8);
        var unicolour2 = Unicolour.FromJzczhz(0.03, 0.03, 90, 0.9);
        var mixed1 = unicolour1.MixJzczhz(unicolour2, 1.5, false);
        var mixed2 = unicolour2.MixJzczhz(unicolour1, 1.5, false);

        AssertMixed(mixed1, (0.035, 0.035, 135, 0.95));
        AssertMixed(mixed2, (0.015, 0.015, 315, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = Unicolour.FromJzczhz(0.02, 0.02, 0, 0.8);
        var unicolour2 = Unicolour.FromJzczhz(0.03, 0.03, 90, 0.9);
        var mixed1 = unicolour1.MixJzczhz(unicolour2, -0.5, false);
        var mixed2 = unicolour2.MixJzczhz(unicolour1, -0.5, false);

        AssertMixed(mixed1, (0.015, 0.015, 315, 0.75));
        AssertMixed(mixed2, (0.035, 0.035, 135, 0.95));
    }
    
    [Test]
    public void BeyondMaxAlpha()
    {
        var unicolour1 = Unicolour.FromJzczhz(0, 0, 0, 0.5);
        var unicolour2 = Unicolour.FromJzczhz(0, 0, 0, 1.5);
        var mixed = unicolour1.MixJzczhz(unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(2.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(1.0));
    }
    
    [Test]
    public void BeyondMinAlpha()
    {
        var unicolour1 = Unicolour.FromJzczhz(0, 0, 0, 0.5);
        var unicolour2 = Unicolour.FromJzczhz(0, 0, 0, -0.5);
        var mixed = unicolour1.MixJzczhz(unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(-1.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(0.0));
    }
    
    [TestCaseSource(typeof(AlphaInterpolationUtils), nameof(AlphaInterpolationUtils.PremultipliedHueIndex2))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        var unicolour1 = Unicolour.FromJzczhz(start.Triplet.Tuple, start.Alpha);
        var unicolour2 = Unicolour.FromJzczhz(end.Triplet.Tuple, end.Alpha);
        var mixed = unicolour1.MixJzczhz(unicolour2, amount, premultiplyAlpha: true);
        AssertMixed(mixed, expected.Tuple);
    }

    private static void AssertMixed(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        AssertUtils.AssertMixed(unicolour.Jzczhz.Triplet, unicolour.Alpha.A, expected);
    }
}