using System;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class InvalidHueSpanTests
{
    private const HueSpan BadHueSpan = (HueSpan)int.MaxValue;
    
    [Test]
    public void InvalidParameter()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, 0.1, 0.2, 0.3);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0.7, 0.8, 0.9);
        Assert.Throws<ArgumentOutOfRangeException>(() => Interpolation.Mix(colour1, colour2, ColourSpace.Hsb, 0.5, BadHueSpan, true));
    }
}