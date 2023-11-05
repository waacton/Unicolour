namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixCam02Tests
{
    [Test]
    public void SameColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Cam02, 50, -25, 25, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Cam02, 50, -25, 25, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Cam02, unicolour2, 0.25, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Cam02, unicolour1, 0.75, false);
        var mixed3 = unicolour1.Mix(ColourSpace.Cam02, unicolour2, 0.75, false);
        var mixed4 = unicolour2.Mix(ColourSpace.Cam02, unicolour1, 0.25, false);
        
        AssertMix(mixed1, (50, -25, 25, 0.5));
        AssertMix(mixed2, (50, -25, 25, 0.5));
        AssertMix(mixed3, (50, -25, 25, 0.5));
        AssertMix(mixed4, (50, -25, 25, 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = new Unicolour(ColourSpace.Cam02, 0, -50, -50, 0.0);
        var unicolour2 = new Unicolour(ColourSpace.Cam02, 50, 50, 50);
        var mixed1 = unicolour1.Mix(ColourSpace.Cam02, unicolour2, 0.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Cam02, unicolour1, 0.5, false);
        
        AssertMix(mixed1, (25, 0, 0, 0.5));
        AssertMix(mixed2, (25, 0, 0, 0.5));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Cam02, 0, 50, -50);
        var unicolour2 = new Unicolour(ColourSpace.Cam02, 80, -50, 50, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Cam02, unicolour2, 0.75, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Cam02, unicolour1, 0.75, false);

        AssertMix(mixed1, (60, -25, 25, 0.625));
        AssertMix(mixed2, (20, 25, -25, 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Cam02, 0, 50, -50);
        var unicolour2 = new Unicolour(ColourSpace.Cam02, 80, -50, 50, 0.5);
        var mixed1 = unicolour1.Mix(ColourSpace.Cam02, unicolour2, 0.25, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Cam02, unicolour1, 0.25, false);
        
        AssertMix(mixed1, (20, 25, -25, 0.875));
        AssertMix(mixed2, (60, -25, 25, 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Cam02, 20, -10, 10, 0.8);
        var unicolour2 = new Unicolour(ColourSpace.Cam02, 30, 10, -10, 0.9);
        var mixed1 = unicolour1.Mix(ColourSpace.Cam02, unicolour2, 1.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Cam02, unicolour1, 1.5, false);

        AssertMix(mixed1, (35, 20, -20, 0.95));
        AssertMix(mixed2, (15, -20, 20, 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = new Unicolour(ColourSpace.Cam02, 20, -10, 10, 0.8);
        var unicolour2 = new Unicolour(ColourSpace.Cam02, 30, 10, -10, 0.9);
        var mixed1 = unicolour1.Mix(ColourSpace.Cam02, unicolour2, -0.5, false);
        var mixed2 = unicolour2.Mix(ColourSpace.Cam02, unicolour1, -0.5, false);

        AssertMix(mixed1, (15, -20, 20, 0.75));
        AssertMix(mixed2, (35, 20, -20, 0.95));
    }
    
    [Test]
    public void BeyondMaxAlpha()
    {
        var unicolour1 = new Unicolour(ColourSpace.Cam02, 0, 0, 0, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Cam02, 0, 0, 0, 1.5);
        var mixed = unicolour1.Mix(ColourSpace.Cam02, unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(2.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(1.0));
    }
    
    [Test]
    public void BeyondMinAlpha()
    {
        var unicolour1 = new Unicolour(ColourSpace.Cam02, 0, 0, 0, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Cam02, 0, 0, 0, -0.5);
        var mixed = unicolour1.Mix(ColourSpace.Cam02, unicolour2, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(-1.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(0.0));
    }
    
    [TestCaseSource(typeof(AlphaInterpolationUtils), nameof(AlphaInterpolationUtils.PremultipliedNoHueComponent))]
    public void PremultiplyAlpha(AlphaTriplet start, AlphaTriplet end, double amount, AlphaTriplet expected)
    {
        var unicolour1 = new Unicolour(ColourSpace.Cam02, start.Triplet.Tuple, start.Alpha);
        var unicolour2 = new Unicolour(ColourSpace.Cam02, end.Triplet.Tuple, end.Alpha);
        var mixed = unicolour1.Mix(ColourSpace.Cam02, unicolour2, amount, premultiplyAlpha: true);
        AssertMix(mixed, expected.Tuple);
    }

    private static void AssertMix(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        TestUtils.AssertMixed(unicolour.Cam02.Triplet, unicolour.Alpha.A, expected);
    }
}