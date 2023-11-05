namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixXyyTests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Xyy, 0.5, 0.25, 0.75, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Xyy, 0.5, 0.25, 0.75, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Xyy, unicolour2, 0.25, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Xyy, unicolour1, 0.75, false);
        var mixed3 = unicolour1.Mix(ColourSpace.Xyy, unicolour2, 0.75, false);
        var mixed4 = unicolour2.Mix(ColourSpace.Xyy, unicolour1, 0.25, false);
        
        AssertMix(mixed1, (0.5, 0.25, 0.75, 0.5));
        AssertMix(mixed2, (0.5, 0.25, 0.75, 0.5));
        AssertMix(mixed3, (0.5, 0.25, 0.75, 0.5));
        AssertMix(mixed4, (0.5, 0.25, 0.75, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = new Unicolour(ColourSpace.Xyy, 0.0, 0.0, 0.0, 0.0);
        var unicolour2 = new Unicolour(ColourSpace.Xyy, 0.5, 1.0, 1.0);
        var mixed1 = unicolour1.Mix(ColourSpace.Xyy, unicolour2, 0.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Xyy, unicolour1, 0.5, false);
        
        AssertMix(mixed1, (0.25, 0.5, 0.5, 0.5));
        AssertMix(mixed2, (0.25, 0.5, 0.5, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Xyy, 0, 1, 0);
        var unicolour2 = new Unicolour(ColourSpace.Xyy, 0.8, 0.0, 1.0, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Xyy, unicolour2, 0.75, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Xyy, unicolour1, 0.75, false);

        AssertMix(mixed1, (0.6, 0.25, 0.75, 0.625));
        AssertMix(mixed2, (0.2, 0.75, 0.25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Xyy, 0.0, 1.0, 0.0);
        var unicolour2 = new Unicolour(ColourSpace.Xyy, 0.8, 0.0, 1.0, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Xyy, unicolour2, 0.25, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Xyy, unicolour1, 0.25, false);
        
        AssertMix(mixed1, (0.2, 0.75, 0.25, 0.875));
        AssertMix(mixed2, (0.6, 0.25, 0.75, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Xyy, 0.2, 0.4, 0.6, 0.8);
        var unicolour2 = new Unicolour(ColourSpace.Xyy, 0.3, 0.6, 0.4, 0.9);
        var mixed1 = unicolour1.Mix(ColourSpace.Xyy, unicolour2, 1.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Xyy, unicolour1, 1.5, false);

        AssertMix(mixed1, (0.35, 0.7, 0.3, 0.95));
        AssertMix(mixed2, (0.15, 0.3, 0.7, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Xyy, 0.2, 0.4, 0.6, 0.8);
        var unicolour2 = new Unicolour(ColourSpace.Xyy, 0.3, 0.6, 0.4, 0.9);
        var mixed1 = unicolour1.Mix(ColourSpace.Xyy, unicolour2, -0.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Xyy, unicolour1, -0.5, false);

        AssertMix(mixed1, (0.15, 0.3, 0.7, 0.75));
        AssertMix(mixed2, (0.35, 0.7, 0.3, 0.95));
    }
    
    [Test]
    public void BeyondMaxAlpha()
    {
        var unicolour1 = new Unicolour(ColourSpace.Xyy, 0, 0, 0, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Xyy, 0, 0, 0, 1.5);
        var mixed = unicolour1.Mix(ColourSpace.Xyy, unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(2.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(1.0));
    }
    
    [Test]
    public void BeyondMinAlpha()
    {
        var unicolour1 = new Unicolour(ColourSpace.Xyy, 0, 0, 0, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Xyy, 0, 0, 0, -0.5);
        var mixed = unicolour1.Mix(ColourSpace.Xyy, unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(-1.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(0.0));
    }
    
    [TestCaseSource(typeof(AlphaInterpolationUtils), nameof(AlphaInterpolationUtils.PremultipliedNoHueComponent))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        var unicolour1 = new Unicolour(ColourSpace.Xyy, start.Triplet.Tuple, start.Alpha);
        var unicolour2 = new Unicolour(ColourSpace.Xyy, end.Triplet.Tuple, end.Alpha);
        var mixed = unicolour1.Mix(ColourSpace.Xyy, unicolour2, amount, premultiplyAlpha: true);
        AssertMix(mixed, expected.Tuple);
    }

    private static void AssertMix(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        TestUtils.AssertMixed(unicolour.Xyy.Triplet, unicolour.Alpha.A, expected);
    }
}