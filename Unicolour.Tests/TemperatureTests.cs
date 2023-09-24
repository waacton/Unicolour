namespace Wacton.Unicolour.Tests;

using System.Globalization;
using NUnit.Framework;

public class TemperatureTests
{
    // using data from http://www.brucelindbloom.com/index.html?ColorCalculator.html
    [TestCase(Illuminant.A, 2855.6)]
    [TestCase(Illuminant.C, 6774.3)]
    [TestCase(Illuminant.D50, 5001.8)]
    [TestCase(Illuminant.D55, 5501.3)]
    [TestCase(Illuminant.D65, 6502.1)]
    [TestCase(Illuminant.D75, 7505.9)]
    [TestCase(Illuminant.E, 5454.0)]
    [TestCase(Illuminant.F2, 4223.7)]
    [TestCase(Illuminant.F7, 6494.0)]
    [TestCase(Illuminant.F11, 3998.6)]
    public void Illuminants(Illuminant illuminant, double kelvins)
    {
        var whitePoint = WhitePoint.From(illuminant);
        var unicolour = Unicolour.FromXyz(whitePoint.AsXyzMatrix().ToTriplet().Tuple);
        var temperature = unicolour.GetTemperature();
        Assert.That(temperature.Cct, Is.EqualTo(kelvins).Within(0.05));
        Assert.That(temperature.ToString().Contains(kelvins.ToString(CultureInfo.InvariantCulture)));
    }
    
    [Test]
    public void Red()
    {
        var unicolour = Unicolour.FromRgb(1, 0, 0);
        Assert.That(unicolour.Temperature.Cct, Is.NaN);
        Assert.That(unicolour.Temperature.ToString(), Is.EqualTo("-"));
    }
    
    [Test]
    public void Green()
    {
        var unicolour = Unicolour.FromRgb(0, 1, 0);
        Assert.That(unicolour.Temperature.Cct, Is.EqualTo(6064.4).Within(0.05));
        Assert.That(unicolour.Temperature.ToString().Contains("6064.4 K"));

    }
    
    [Test]
    public void Blue()
    {
        var unicolour = Unicolour.FromRgb(0, 0, 1);
        Assert.That(unicolour.Temperature.Cct, Is.NaN);
        Assert.That(unicolour.Temperature.ToString(), Is.EqualTo("-"));
    }
    
    [Test]
    public void Black()
    {
        var unicolour = Unicolour.FromXyz(0, 0, 0);
        Assert.That(unicolour.Temperature.Cct, Is.NaN);
        Assert.That(unicolour.Temperature.ToString(), Is.EqualTo("-"));
    }
    
    // lower temperatures (~red) correlate with greater (u, v) - (0.33724, 0.36051) is the uppermost defined (u, v) by Robertson
    [TestCase(0.33724, 0.36051, 1666.7)] 
    [TestCase(0.33724 + 0.000000000000001, 0.36051, double.NaN)] 
    [TestCase(0.33724, 0.36051 + 0.000000000000001, double.NaN)]
    [TestCase(0.33724 + 0.000000000000001, 0.36051 + 0.000000000000001, double.NaN)] 
    public void LowerBoundary(double u, double v, double? expected)
    {
        var temperature = Temperature.Get(u, v);
        Assert.That(temperature.Cct, Is.EqualTo(expected).Within(0.05));
    }
    
    // higher temperatures (~blue) correlate with lower (u, v) - (0.18006, 0.26352) is the uppermost defined (u, v) by Robertson
    [TestCase(0.18006, 0.26352, double.PositiveInfinity)] 
    [TestCase(0.18006 - 0.000000000000001, 0.26352, double.NaN)] 
    [TestCase(0.18006, 0.26352 - 0.000000000000001, double.NaN)]
    [TestCase(0.18006 - 0.000000000000001, 0.26352 - 0.000000000000001, double.NaN)] 
    public void UpperBoundary(double u, double v, double expected)
    {
        var temperature = Temperature.Get(u, v);
        Assert.That(temperature.Cct, Is.EqualTo(expected));
    }
    
    [Test] // matches the behaviour of python-based "colour-science/colour" (https://github.com/colour-science/colour/blob/d7d79c745b15b97e7e5b8ccf50e3f676c762c770/colour/temperature/robertson1968.py#L297)  
    public void KnownUv()
    {
        var temperature = Temperature.Get(0.193741375998230, 0.315221043940594);
        Assert.That(temperature.Cct, Is.EqualTo(6500.0162).Within(0.0005));
        Assert.That(temperature.Duv, Is.EqualTo(0.0083333289).Within(0.0005));
        Assert.That(temperature.ToString().Contains("6500.0 K"));
        Assert.That(temperature.ToString().Contains("Δuv 0.008"));
    }
}