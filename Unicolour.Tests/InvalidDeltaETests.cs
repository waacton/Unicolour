using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class InvalidDeltaETests
{
    private const DeltaE BadDeltaE = (DeltaE)int.MaxValue;
    
    [Test]
    public void InvalidConstructor()
    {
        var colour1 = RandomColours.UnicolourFrom(ColourSpace.Rgb);
        var colour2 = RandomColours.UnicolourFrom(ColourSpace.Rgb);
        Assert.Throws<ArgumentOutOfRangeException>(() => colour1.Difference(colour2, BadDeltaE));
    }
}