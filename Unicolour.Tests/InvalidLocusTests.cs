using System;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class InvalidLocusTests
{
    private const Locus BadLocus = (Locus)int.MaxValue;
    
    [Test]
    public void InvalidParameter()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Temperature.FromCct(6500, BadLocus, new Planckian(Observer.Degree2)));
    }
}