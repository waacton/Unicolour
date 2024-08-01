using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

/*
 * this base class handles all core mixing tests that are common to every colour space
 * i.e. ones that do not need to consider cyclical hue values
 * hue-based mixing tests are handled in subclasses
 */
public abstract class MixHueAgnosticTests
{
    protected ColourSpace ColourSpace { get; }
    protected Range First { get; }
    protected Range Second { get; }
    protected Range Third { get; }
    
    protected MixHueAgnosticTests(ColourSpace colourSpace, Range first, Range second, Range third)
    {
        ColourSpace = colourSpace;
        First = first;
        Second = second;
        Third = third;
    }
    
    [Test]
    public void SameColour()
    {
        var unicolour1 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.25), Third.At(0.75), 0.5);
        var unicolour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.25), Third.At(0.75), 0.5);
        var mixed1 = unicolour1.Mix(unicolour2, ColourSpace, 0.25, premultiplyAlpha: false);
        var mixed2 = unicolour2.Mix(unicolour1, ColourSpace, 0.75, premultiplyAlpha: false);
        var mixed3 = unicolour1.Mix(unicolour2, ColourSpace, 0.75, premultiplyAlpha: false);
        var mixed4 = unicolour2.Mix(unicolour1, ColourSpace, 0.25, premultiplyAlpha: false);
        
        AssertMix(mixed1, (First.At(0.5), Second.At(0.25), Third.At(0.75), 0.5));
        AssertMix(mixed2, (First.At(0.5), Second.At(0.25), Third.At(0.75), 0.5));
        AssertMix(mixed3, (First.At(0.5), Second.At(0.25), Third.At(0.75), 0.5));
        AssertMix(mixed4, (First.At(0.5), Second.At(0.25), Third.At(0.75), 0.5));
    }
    
    [Test]
    public void Equidistant()
    {
        var unicolour1 = new Unicolour(ColourSpace, First.At(0.0), Second.At(0.0), Third.At(0.0), 0.0);
        var unicolour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(1.0), Third.At(0.5), 0.2);
        var mixed1 = unicolour1.Mix(unicolour2, ColourSpace, premultiplyAlpha: false);
        var mixed2 = unicolour2.Mix(unicolour1, ColourSpace, premultiplyAlpha: false);
        
        AssertMix(mixed1, (First.At(0.25), Second.At(0.5), Third.At(0.25), 0.1));
        AssertMix(mixed2, (First.At(0.25), Second.At(0.5), Third.At(0.25), 0.1));
    }
    
    [Test]
    public void CloserToEndColour()
    {
        var unicolour1 = new Unicolour(ColourSpace, First.At(0.0), Second.At(1.0), Third.At(0.5));
        var unicolour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.0), Third.At(0.0), 0.5);
        var mixed1 = unicolour1.Mix(unicolour2, ColourSpace, 0.75, premultiplyAlpha: false);
        var mixed2 = unicolour2.Mix(unicolour1, ColourSpace, 0.75, premultiplyAlpha: false);

        AssertMix(mixed1, (First.At(0.375), Second.At(0.25), Third.At(0.125), 0.625));
        AssertMix(mixed2, (First.At(0.125), Second.At(0.75), Third.At(0.375), 0.875));
    }
    
    [Test]
    public void CloserToStartColour()
    {
        var unicolour1 = new Unicolour(ColourSpace, First.At(0.0), Second.At(1.0), Third.At(0.5));
        var unicolour2 = new Unicolour(ColourSpace, First.At(0.5), Second.At(0.0), Third.At(0.0), 0.5);
        var mixed1 = unicolour1.Mix(unicolour2, ColourSpace, 0.25, premultiplyAlpha: false);
        var mixed2 = unicolour2.Mix(unicolour1, ColourSpace, 0.25, premultiplyAlpha: false);
        
        AssertMix(mixed1, (First.At(0.125), Second.At(0.75), Third.At(0.375), 0.875));
        AssertMix(mixed2, (First.At(0.375), Second.At(0.25), Third.At(0.125), 0.625));
    }
    
    [Test]
    public void BeyondEndColour()
    {
        var unicolour1 = new Unicolour(ColourSpace, First.At(0.2), Second.At(0.4), Third.At(0.6), 0.8);
        var unicolour2 = new Unicolour(ColourSpace, First.At(0.3), Second.At(0.6), Third.At(0.4), 0.9);
        var mixed1 = unicolour1.Mix(unicolour2, ColourSpace, 1.5, premultiplyAlpha: false);
        var mixed2 = unicolour2.Mix(unicolour1, ColourSpace, 1.5, premultiplyAlpha: false);

        AssertMix(mixed1, (First.At(0.35), Second.At(0.7), Third.At(0.3), 0.95));
        AssertMix(mixed2, (First.At(0.15), Second.At(0.3), Third.At(0.7), 0.75));
    }
    
    [Test]
    public void BeyondStartColour()
    {
        var unicolour1 = new Unicolour(ColourSpace, First.At(0.2), Second.At(0.4), Third.At(0.6), 0.8);
        var unicolour2 = new Unicolour(ColourSpace, First.At(0.3), Second.At(0.6), Third.At(0.4), 0.9);
        var mixed1 = unicolour1.Mix(unicolour2, ColourSpace, -0.5, premultiplyAlpha: false);
        var mixed2 = unicolour2.Mix(unicolour1, ColourSpace, -0.5, premultiplyAlpha: false);

        AssertMix(mixed1, (First.At(0.15), Second.At(0.3), Third.At(0.7), 0.75));
        AssertMix(mixed2, (First.At(0.35), Second.At(0.7), Third.At(0.3), 0.95));
    }
    
    [Test]
    public void BeyondMaxAlpha()
    {
        var unicolour1 = new Unicolour(ColourSpace, 0, 0, 0, 0.5);
        var unicolour2 = new Unicolour(ColourSpace, 0, 0, 0, 1.5);
        var mixed = unicolour1.Mix(unicolour2, ColourSpace, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(2.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(1.0));
    }
    
    [Test]
    public void BeyondMinAlpha()
    {
        var unicolour1 = new Unicolour(ColourSpace, 0, 0, 0, 0.5);
        var unicolour2 = new Unicolour(ColourSpace, 0, 0, 0, -0.5);
        var mixed = unicolour1.Mix(unicolour2, ColourSpace, 3);
        Assert.That(mixed.Alpha.A, Is.EqualTo(-1.0));
        Assert.That(mixed.Alpha.ConstrainedA, Is.EqualTo(0.0));
    }
    
    protected void AssertMix(Unicolour unicolour, (double first, double second, double third, double alpha) expected)
    {
        TestUtils.AssertMixed(unicolour.GetRepresentation(ColourSpace).Triplet, unicolour.Alpha.A, expected);
    }
}

