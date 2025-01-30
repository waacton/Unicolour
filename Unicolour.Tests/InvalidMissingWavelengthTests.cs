using System;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class InvalidMissingWavelengthTests
{
    private const MissingWavelength BadMissingWavelength = (MissingWavelength)int.MaxValue;
    
    [Test]
    public void InvalidParameter()
    {
        var spectralCoefficients = new SpectralCoefficients(start: 360, interval: 1, coefficients: [0.1, 0.2, 0.3, 0.4, 0.5]);
        Assert.Throws<ArgumentOutOfRangeException>(() => spectralCoefficients.Get(370, BadMissingWavelength));
    }
}