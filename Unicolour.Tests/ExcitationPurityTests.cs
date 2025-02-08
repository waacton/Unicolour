using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ExcitationPurityTests
{
    [Test]
    public void RgbGamut([Values(0, 255)] int r, [Values(0, 255)] int g, [Values(0, 255)] int b)
    {
        var colour = new Unicolour(ColourSpace.Rgb255, r, g, b);
        var greyscale = r == g && g == b;
        Assert.That(colour.ExcitationPurity, greyscale ? Is.NaN : Is.LessThan(1.0));
    }

    [Test]
    public void Greyscale([Range(0, 1, 0.1)] double value)
    {
        var colour = new Unicolour(ColourSpace.Rgb, value, value, value);
        Assert.That(colour.ExcitationPurity, Is.NaN);
    }
    
    private static readonly Dictionary<(Illuminant illuminant, Observer observer), Configuration> Configurations = new()
    {
        { (Illuminant.D65, Observer.Degree2), new(xyzConfig: new(Illuminant.D65, Observer.Degree2)) },
        { (Illuminant.D65, Observer.Degree10), new(xyzConfig: new(Illuminant.D65, Observer.Degree10)) },
        { (Illuminant.E, Observer.Degree2), new(xyzConfig: new(Illuminant.E, Observer.Degree2)) },
        { (Illuminant.E, Observer.Degree10), new(xyzConfig: new(Illuminant.E, Observer.Degree10)) }
    };

    [Test]
    public void Monochromatic(
        [Range(360, 700)] int wavelength,
        [Values(nameof(Observer.Degree2), nameof(Observer.Degree10))] string observerName,
        [Values(nameof(Illuminant.D65), nameof(Illuminant.E))] string illuminantName)
    {
        var illuminant = TestUtils.Illuminants[illuminantName];
        var observer = TestUtils.Observers[observerName];
        var config = Configurations[(illuminant, observer)];
        
        var colour = new Unicolour(config, Spd.Monochromatic(wavelength));
        Assert.That(colour.ExcitationPurity, Is.EqualTo(1.0).Within(0.00000000000005));
    }
    
    [TestCase(0, 0)]
    [TestCase(0, 1)]
    [TestCase(1, 0)]
    [TestCase(1, 1)]
    public void Imaginary(double x, double y)
    {
        var colour = new Unicolour(new Chromaticity(x, y));
        Assert.That(colour.ExcitationPurity, Is.GreaterThan(1.0));
    }
}