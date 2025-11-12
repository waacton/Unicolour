using System;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class InvalidCvdTests
{
    private const Cvd BadCvd = (Cvd)int.MaxValue;
    
    [Test]
    public void InvalidParameter()
    {
        var colour = new Unicolour(ColourSpace.Rgb, 0.1, 0.2, 0.3);
        Assert.Throws<ArgumentOutOfRangeException>(() => VisionDeficiency.Simulate(BadCvd, 1.0, colour));
    }
}