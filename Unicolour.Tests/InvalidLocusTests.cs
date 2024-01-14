namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class InvalidLocusTests
{
    private const Locus BadLocus = (Locus)int.MaxValue;
    
    [Test]
    public void InvalidParameter()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Temperature.FromCct(6500, BadLocus, new Planckian(Observer.Degree2)));
    }
}