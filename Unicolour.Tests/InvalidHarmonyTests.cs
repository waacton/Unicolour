using System;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class InvalidHarmonyTests
{
    private const Harmony BadHarmony = (Harmony)int.MaxValue;
    
    [Test]
    public void InvalidParameter()
    {
        var colourWheel = ColourWheel.From(ColourSpace.Hsb, StandardRgb.Grey);
        Assert.Throws<ArgumentOutOfRangeException>(() => colourWheel.Harmony(0, BadHarmony));
    }
}