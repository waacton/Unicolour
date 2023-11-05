namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixHsbTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 180, 0.25, 0.75, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 180, 0.25, 0.75, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 0.25, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, 0.75, false);
        var mixed3 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 0.75, false);
        var mixed4 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, 0.25, false);
        
        AssertMix(mixed1, (180, 0.25, 0.75, 0.5));
        AssertMix(mixed2, (180, 0.25, 0.75, 0.5));
        AssertMix(mixed3, (180, 0.25, 0.75, 0.5));
        AssertMix(mixed4, (180, 0.25, 0.75, 0.5));
    }

    [Test]
    public void Equidistant()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 0, 0, 0, 0);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 180, 1, 1);
        var mixed1 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 0.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, 0.5, false);
        
        AssertMix(mixed1, (90, 0.5, 0.5, 0.5));
        AssertMix(mixed2, (90, 0.5, 0.5, 0.5));
    }
    
    [Test]
    public void EquidistantViaZero()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 0, 0, 0, 0);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 340, 0.5, 0.8, 0.2);
        var mixed1 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 0.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, 0.5, false);
        
        AssertMix(mixed1, (350, 0.25, 0.4, 0.1));
        AssertMix(mixed2, (350, 0.25, 0.4, 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 0, 1, 0);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 180, 0, 1, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 0.75, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, 0.75, false);

        AssertMix(mixed1, (135, 0.25, 0.75, 0.625));
        AssertMix(mixed2, (45, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToEndColourViaZero()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 300, 1, 0);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 60, 0, 1, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 0.75, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, 0.75, false);

        AssertMix(mixed1, (30, 0.25, 0.75, 0.625));
        AssertMix(mixed2, (330, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 0, 1, 0);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 180, 0, 1, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 0.25, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, 0.25, false);
        
        AssertMix(mixed1, (45, 0.75, 0.25, 0.875));
        AssertMix(mixed2, (135, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void CloserToStartColourViaZero()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 300, 1, 0);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 60, 0, 1, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 0.25, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, 0.25, false);
        
        AssertMix(mixed1, (330, 0.75, 0.25, 0.875));
        AssertMix(mixed2, (30, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 0, 0.4, 0.6, 0.8);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 90, 0.6, 0.4, 0.9);
        var mixed1 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 1.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, 1.5, false);

        AssertMix(mixed1, (135, 0.7, 0.3, 0.95));
        AssertMix(mixed2, (315, 0.3, 0.7, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 0, 0.4, 0.6, 0.8);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 90, 0.6, 0.4, 0.9);
        var mixed1 = unicolour1.Mix(ColourSpace.Hsb, unicolour2, -0.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Hsb, unicolour1, -0.5, false);

        AssertMix(mixed1, (315, 0.3, 0.7, 0.75));
        AssertMix(mixed2, (135, 0.7, 0.3, 0.95));
    }
    
    [Test]
    public void BeyondMaxAlpha()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 0, 0, 0, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 0, 0, 0, 1.5);
        var mixed = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(2.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(1.0));
    }
    
    [Test]
    public void BeyondMinAlpha()
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, 0, 0, 0, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, 0, 0, 0, -0.5);
        var mixed = unicolour1.Mix(ColourSpace.Hsb, unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(-1.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(0.0));
    }
    
    [TestCaseSource(typeof(AlphaInterpolationUtils), nameof(AlphaInterpolationUtils.PremultipliedHueIndex0))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        var unicolour1 = new Unicolour(ColourSpace.Hsb, start.Triplet.Tuple, start.Alpha);
        var unicolour2 = new Unicolour(ColourSpace.Hsb, end.Triplet.Tuple, end.Alpha);
        var mixed = unicolour1.Mix(ColourSpace.Hsb, unicolour2, amount, premultiplyAlpha: true);
        AssertMix(mixed, expected.Tuple);
    }

    private static void AssertMix(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        TestUtils.AssertMixed(unicolour.Hsb.Triplet, unicolour.Alpha.A, expected);
    }
}