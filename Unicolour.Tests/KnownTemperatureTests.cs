using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownTemperatureTests
{
    [Test] // matches the behaviour of an example in https://cormusa.org/wp-content/uploads/2018/04/CORM_2011_Calculation_of_CCT_and_Duv_and_Practical_Conversion_Formulae.pdf
    public void CascadeExpansionAccuracy()
    {
        var temperature = new Temperature(2900, 0.0200);
        var chromaticity = Temperature.ToChromaticity(temperature, Observer.Degree2);
        Assert.That(chromaticity.X, Is.EqualTo(0.478420).Within(0.0000005));
        Assert.That(chromaticity.Y, Is.EqualTo(0.473737).Within(0.0000005));
        Assert.That(chromaticity.U, Is.EqualTo(0.247629).Within(0.0000005));
        Assert.That(chromaticity.V, Is.EqualTo(0.367808).Within(0.0000005));
        
        var roundtrip = Temperature.FromChromaticity(chromaticity, TestUtils.PlanckianObserverDegree2);
        Assert.That(roundtrip.Cct, Is.EqualTo(2900).Within(0.001));
        Assert.That(roundtrip.Duv, Is.EqualTo(0.0200).Within(0.000000005));
        Assert.That(roundtrip.ToString().Contains("2900.0 K"));
        Assert.That(roundtrip.ToString().Contains("Δuv 0.0200"));
    }

    [Test] // matches the behaviour of python-based "coloraide" (https://facelessuser.github.io/coloraide/temperature/#duv)
    public void Yellow()
    {
        var yellow = new Unicolour(ColourSpace.RgbLinear, 1, 1, 0);
        Assert.That(yellow.Temperature.Cct, Is.EqualTo(3934.7).Within(0.75));
        Assert.That(yellow.Temperature.Duv, Is.EqualTo(0.0403).Within(0.0005));
    }

    [Test] // matches the behaviour of python-based "coloraide" (https://facelessuser.github.io/coloraide/temperature/#duv)
    public void DisplayP3()
    {
        var unicolour = new Unicolour(new Configuration(rgbConfig: RgbConfiguration.DisplayP3), 1200);
        TestUtils.AssertTriplet<Rgb>(unicolour, new(1.6804, 0.62798, 0.05495), 0.005);
    }
    
    [Test] // matches the behaviour of python-based "colour-science/colour" (https://github.com/colour-science/colour/blob/d7d79c745b15b97e7e5b8ccf50e3f676c762c770/colour/temperature/ohno2013.py#L144)  
    public void KnownUv()
    {
        var chromaticity = Chromaticity.FromUv(0.1978, 0.3122);
        var temperature = Temperature.FromChromaticity(chromaticity, TestUtils.PlanckianObserverDegree2);
        Assert.That(temperature.Cct, Is.EqualTo(6507.47).Within(0.05));
        Assert.That(temperature.Duv, Is.EqualTo(0.00322334).Within(0.0005));
        Assert.That(temperature.ToString().Contains("6507.5 K"));
        Assert.That(temperature.ToString().Contains("Δuv 0.0032"));
    }
    
    // using data from http://www.brucelindbloom.com/index.html?ColorCalculator.html
    // however, tolerance of 1.5 K because Unicolour uses a more modern algorithm
    [TestCase(nameof(Illuminant.A), 2855.6)]
    [TestCase(nameof(Illuminant.C), 6774.3)]
    [TestCase(nameof(Illuminant.D50), 5001.8)]
    [TestCase(nameof(Illuminant.D55), 5501.3)]
    [TestCase(nameof(Illuminant.D65), 6502.1)]
    [TestCase(nameof(Illuminant.D75), 7505.9)]
    [TestCase(nameof(Illuminant.E), 5454.0)]
    [TestCase(nameof(Illuminant.F2), 4223.7)]
    [TestCase(nameof(Illuminant.F7), 6494.0)]
    [TestCase(nameof(Illuminant.F11), 3998.6)]
    public void IlluminantCct(string illuminantName, double kelvins)
    {
        var illuminant = TestUtils.Illuminants[illuminantName];
        var whitePoint = illuminant.GetWhitePoint(Observer.Degree2);
        var unicolour = new Unicolour(ColourSpace.Xyz, whitePoint.AsXyzMatrix().ToTriplet().Tuple);
        var temperature = unicolour.Temperature;
        Assert.That(temperature.Cct, Is.EqualTo(kelvins).Within(1.5), temperature.ToString);
    }
    
    // chromaticities taken from http://www.brucelindbloom.com/index.html?ColorCalculator.html (using white with different illuminants)
    [TestCase(nameof(Illuminant.D50), 5000, 0.345669, 0.358496)]
    [TestCase(nameof(Illuminant.D55), 5500, 0.332424, 0.347426)]
    [TestCase(nameof(Illuminant.D65), 6500, 0.312727, 0.329023)]
    [TestCase(nameof(Illuminant.D75), 7500, 0.299021, 0.314852)]
    public void DaylightCct(string illuminantName, double cct, double x, double y)
    {
        var daylightCct = cct * 1.4388 / 1.4380;
        
        var illuminant = TestUtils.Illuminants[illuminantName];
        var config = new Configuration(xyzConfig: new XyzConfiguration(illuminant, Observer.Degree2));
        var fromDaylightCct = new Unicolour(config, daylightCct, Locus.Daylight);
        var fromChromaticity = new Unicolour(config, ColourSpace.Xyy, x, y, 1);
        var fromColour = new Unicolour(config, ColourSpace.Rgb, 1, 1, 1);
        
        // there seems to be some inconsistency between blackbody vs daylight CCT
        // which results in blackbody CCT being a little different than the desired daylight CCT
        // (which is at least present on http://www.brucelindbloom.com/index.html?ColorCalculator.html, so it's not just me)
        Assert.That(fromDaylightCct.Temperature.Cct, Is.EqualTo(daylightCct).Within(1.75));
        Assert.That(fromChromaticity.Temperature.Cct, Is.EqualTo(daylightCct).Within(2.25));
        Assert.That(fromColour.Temperature.Cct, Is.EqualTo(daylightCct).Within(2.25));
        
        // however, the blackbody CCT should be similar no matter how they are constructed
        var blackbodyTemperature = Temperature.FromChromaticity(fromChromaticity.Chromaticity, TestUtils.PlanckianObserverDegree2);
        var fromBlackbodyCct = new Unicolour(config, blackbodyTemperature);
        Assert.That(fromBlackbodyCct.Temperature.Cct, Is.EqualTo(fromChromaticity.Temperature.Cct).Within(0.75));
        Assert.That(fromBlackbodyCct.Temperature.Duv, Is.EqualTo(fromChromaticity.Temperature.Duv).Within(0.0005));
        Assert.That(fromChromaticity.Temperature.Cct, Is.EqualTo(fromColour.Temperature.Cct).Within(0.75));
        Assert.That(fromChromaticity.Temperature.Duv, Is.EqualTo(fromColour.Temperature.Duv).Within(0.0005));
        Assert.That(fromColour.Temperature.Cct, Is.EqualTo(fromBlackbodyCct.Temperature.Cct).Within(0.75));
        Assert.That(fromColour.Temperature.Duv, Is.EqualTo(fromBlackbodyCct.Temperature.Duv).Within(0.0005));
    }

    [TestCase(1.0)]
    [TestCase(0.5)]
    [TestCase(0.0)]
    public void Luminance(double luminance)
    {
        var temperatureD65 = new Temperature(6500 * 1.4388 / 1.4380, 0.0032);
        
        var unicolour = new Unicolour(Configuration.Default, temperatureD65, luminance);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.3127, 0.3290, luminance), 0.00005);
        TestUtils.AssertTriplet<RgbLinear>(unicolour, new(luminance, luminance, luminance), 0.0005);
        Assert.That(unicolour.Xyy.Luminance, Is.EqualTo(luminance));
        Assert.That(unicolour.Temperature, Is.EqualTo(temperatureD65));
    }
}