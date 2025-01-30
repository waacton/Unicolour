using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class SpectralCoefficientsTests
{
    [Test]
    public void CoefficientsValidSingle()
    {
        var spd = new Spd(start: 580, interval: 1, coefficients: 0.5);
        Assert.That(spd.IsValid, Is.True);
        AssertWavelength(spd, int.MinValue, (0.0, 0.5));
        AssertWavelength(spd, 580, (0.5, 0.5));
        AssertWavelength(spd, int.MaxValue, (0.0, 0.5));
    }
    
    [Test]
    public void CoefficientsValidMultiple()
    {
        var coefficients = new SpectralCoefficients(start: 580, interval: 10, coefficients: [0.5, 1.0]);
        Assert.That(coefficients.IsValid, Is.True);
        AssertWavelength(coefficients, int.MinValue, (0.0, 0.5));
        AssertWavelength(coefficients, 580, (0.5, 0.5));
        AssertWavelength(coefficients, 585, (0.0, 0.75));
        AssertWavelength(coefficients, 590, (1.0, 1.0));
        AssertWavelength(coefficients, int.MaxValue, (0.0, 1.0));
    }
    
    [Test]
    public void CoefficientsInvalidNone()
    {
        var coefficients = new SpectralCoefficients(start: 580, interval: 1, coefficients: []);
        Assert.That(coefficients.IsValid, Is.False);
        AssertWavelength(coefficients, int.MinValue, (0.0, double.NaN));
        AssertWavelength(coefficients, 580, (0.0, double.NaN));
        AssertWavelength(coefficients, int.MaxValue, (0.0, double.NaN));
    }
    
    [Test]
    public void IntervalValidZero()
    {
        // 0 interval means all coefficients are for wavelength 580, assume first value is the intended value
        var coefficients = new SpectralCoefficients(start: 580, interval: 0, coefficients: [0.75]);
        Assert.That(coefficients.IsValid, Is.True);
        AssertWavelength(coefficients, int.MinValue, (0.0, 0.75));
        AssertWavelength(coefficients, 580, (0.75, 0.75));
        AssertWavelength(coefficients, int.MaxValue, (0.0, 0.75));
    }
    
    [Test]
    public void IntervalValidNegative()
    {
        var coefficients = new SpectralCoefficients(start: 580, interval: -10, coefficients: [0.5, 1.0]);
        Assert.That(coefficients.IsValid, Is.True);
        AssertWavelength(coefficients, int.MinValue, (0.0, 1.0));
        AssertWavelength(coefficients, 570, (1.0, 1.0));
        AssertWavelength(coefficients, 575, (0.0, 0.75));
        AssertWavelength(coefficients, 580, (0.5, 0.5));
        AssertWavelength(coefficients, 585, (0.0, 0.5));
        AssertWavelength(coefficients, 590, (0.0, 0.5));
        AssertWavelength(coefficients, int.MaxValue, (0.0, 0.5));
    }
    
    [Test]
    public void IntervalInvalidZeroAtLeastOneCoefficient()
    {
        // 0 interval means all coefficients are for wavelength 580, assume first value is the intended value
        var coefficients = new SpectralCoefficients(start: 580, interval: 0, coefficients: [0.25, 0.5, 0.75]);
        Assert.That(coefficients.IsValid, Is.False);
        AssertWavelength(coefficients, 580, (0.25, 0.25));
        AssertWavelength(coefficients, int.MinValue, (0.0, 0.25));
        AssertWavelength(coefficients, int.MaxValue, (0.0, 0.25));
    }
    
    [Test]
    public void IntervalInvalidZeroNoCoefficients()
    {
        // 0 interval means all coefficients are for wavelength 580, assume first value is the intended value - but there isn't one
        var coefficients = new SpectralCoefficients(start: 580, interval: 0, coefficients: []);
        Assert.That(coefficients.IsValid, Is.False);
        AssertWavelength(coefficients, 580, (0.0, double.NaN));
        AssertWavelength(coefficients, int.MinValue, (0.0, double.NaN));
        AssertWavelength(coefficients, int.MaxValue, (0.0, double.NaN));
    }
    
    [Test]
    public void WavelengthStartInvalid()
    {
        var coefficients = new SpectralCoefficients(start: 361, interval: 10, coefficients: [0.2, 0.4, 0.6, 0.8]);
        Assert.That(coefficients.IsValid, Is.False);
        AssertWavelength(coefficients, int.MinValue, (0.0, 0.2));
        AssertWavelength(coefficients, 360, (0.0, 0.2));
        AssertWavelength(coefficients, 361, (0.2, 0.2));
        AssertWavelength(coefficients, 366, (0.0, 0.3));
        AssertWavelength(coefficients, 371, (0.4, 0.4));
        AssertWavelength(coefficients, 376, (0.0, 0.5));
        AssertWavelength(coefficients, 381, (0.6, 0.6));
        AssertWavelength(coefficients, 386, (0.0, 0.7));
        AssertWavelength(coefficients, 391, (0.8, 0.8));
        AssertWavelength(coefficients, 396, (0.0, 0.8));
        AssertWavelength(coefficients, int.MaxValue, (0.0, 0.8));
    }
    
    private static void AssertWavelength(SpectralCoefficients coefficients, int wavelength, (double missingZero, double missingInterpolate) expected)
    {
        Assert.That(coefficients.Get(wavelength, MissingWavelength.Zero), Is.EqualTo(expected.missingZero));
        Assert.That(coefficients.Get(wavelength, MissingWavelength.Interpolate), Is.EqualTo(expected.missingInterpolate).Within(0.0000000000000001));
    }
}