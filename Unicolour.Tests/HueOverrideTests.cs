namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class HueOverrideTests
{
    [Test]
    public void NoHue()
    {
        var triplet = new ColourTriplet(7.7, 8.8, 9.9, null);
        Assert.Throws<ArgumentException>(() => triplet.HueValue());
        Assert.Throws<ArgumentException>(() => triplet.WithHueOverride(6.6));
    }
    
    [Test]
    public void FirstHue()
    {
        var triplet = new ColourTriplet(7.7, 8.8, 9.9, 0);
        Assert.That(triplet.HueValue(), Is.EqualTo(7.7));
        var hueOverrideTriplet = triplet.WithHueOverride(6.6);
        Assert.That(hueOverrideTriplet.HueValue(), Is.EqualTo(6.6));
        Assert.That(hueOverrideTriplet.Tuple, Is.EqualTo((6.6, 8.8, 9.9)));
    }
    
    [Test]
    public void SecondHue()
    {
        var triplet = new ColourTriplet(7.7, 8.8, 9.9, 1);
        Assert.Throws<ArgumentException>(() => triplet.HueValue());
        Assert.Throws<ArgumentException>(() => triplet.WithHueOverride(6.6));
    }
    
    [Test]
    public void ThirdHue()
    {
        var triplet = new ColourTriplet(7.7, 8.8, 9.9, 2);
        Assert.That(triplet.HueValue(), Is.EqualTo(9.9));
        var hueOverrideTriplet = triplet.WithHueOverride(6.6);
        Assert.That(hueOverrideTriplet.HueValue(), Is.EqualTo(6.6));
        Assert.That(hueOverrideTriplet.Tuple, Is.EqualTo((7.7, 8.8, 6.6)));
    }
}