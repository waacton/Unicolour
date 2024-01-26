namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class InvalidHueSpanTests
{
    private const HueSpan BadHueSpan = (HueSpan)int.MaxValue;
    
    [Test]
    public void InvalidParameter()
    {
        var unicolour1 = new Unicolour(ColourSpace.Rgb, 0.1, 0.2, 0.3);
        var unicolour2 = new Unicolour(ColourSpace.Rgb, 0.7, 0.8, 0.9);
        Assert.Throws<ArgumentOutOfRangeException>(() => Interpolation.Mix(unicolour1, unicolour2, ColourSpace.Hsb, 0.5, BadHueSpan, true));
    }
}