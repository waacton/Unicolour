namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

// matches behaviour of https://www.waveformlighting.com/tech/spd-to-cie-xy-calculator
public class KnownSpdTests
{
    [Test]
    public void Constant()
    {
        var spd = new Spd(TestWavelengthRange.ToDictionary(wavelength => wavelength, _ => 8.0));
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.3333, 0.3333, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }

    [Test]
    public void LinearTowardsRed()
    {
        var spd = new Spd(TestWavelengthRange.ToDictionary(wavelength => wavelength, wavelength => wavelength - 380.0));
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.4299, 0.4040, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }
    
    [Test]
    public void LinearTowardsBlue()
    {
        var spd = new Spd(TestWavelengthRange.ToDictionary(wavelength => wavelength, wavelength => 780.0 - wavelength));
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.2762, 0.2916, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }
    
    [Test]
    public void ExponentialTowardsRed()
    {
        var spd = new Spd(TestWavelengthRange.ToDictionary(wavelength => wavelength, wavelength => Math.Pow(wavelength - 380.0, 3)));
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.5542, 0.4128, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }
    
    [Test]
    public void ExponentialTowardsBlue()
    {
        var spd = new Spd(TestWavelengthRange.ToDictionary(wavelength => wavelength, wavelength => Math.Pow(780.0 - wavelength, 3)));
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.2013, 0.2004, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }

    [Test]
    public void YellowSpike()
    {
        var spd = new Spd { { 580, 1.0 } };
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.5125, 0.4866, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }
    
    [Test]
    public void NoPower()
    {
        // an empty SPD is equivalent to all wavelengths having 0 power, and treated as not processable
        // as opposed to some kind of "black" illumination (illuminating with the absence of any light...)
        var spd = new Spd();
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(double.NaN, double.NaN, double.NaN), 0);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(double.NaN, double.NaN, double.NaN), 0);
    }

    private static void AssertWhitePoint(Spd spd)
    {
        // regardless of the spectral power distribution
        // if the SPD is set as the white point, RGB will be white
        var xyzConfig = new XyzConfiguration(new Illuminant(spd), Observer.Degree2);
        var config = new Configuration(xyzConfiguration: xyzConfig);
        var unicolour = new Unicolour(config, spd);
        TestUtils.AssertTriplet<Rgb>(unicolour, new(1.0, 1.0, 1.0), 0.00000000001);
    }

    private static List<int> TestWavelengthRange => Enumerable.Range(380, 780 - 380 + 1).ToList();
}