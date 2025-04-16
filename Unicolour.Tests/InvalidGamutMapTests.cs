using System;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class InvalidGamutMapTests
{
    private const GamutMap BadGamutMap = (GamutMap)int.MaxValue;
    
    [Test]
    public void InvalidParameter()
    {
        var colour = new Unicolour(ColourSpace.Rgb, 1.1, 2.2, 3.3);
        Assert.Throws<ArgumentOutOfRangeException>(() => GamutMapping.ToRgbGamut(colour, BadGamutMap));
    }
}