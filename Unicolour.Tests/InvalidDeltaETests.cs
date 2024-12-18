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
        var unicolour1 = RandomColours.UnicolourFrom(ColourSpace.Rgb);
        var unicolour2 = RandomColours.UnicolourFrom(ColourSpace.Rgb);
        Assert.Throws<ArgumentOutOfRangeException>(() => unicolour1.Difference(unicolour2, BadDeltaE));
    }
}