using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// matches behaviour of https://www.waveformlighting.com/tech/spd-to-cie-xy-calculator
public class KnownSpdTests
{
    private const int StartWavelength = 380;
    private const int EndWavelength = 780;
    private const int WavelengthCount = EndWavelength - StartWavelength + 1;
    private static int[] Wavelengths => Enumerable.Range(StartWavelength, WavelengthCount).ToArray();
    
    [Test]
    public void Constant()
    {
        var spd = new Spd(StartWavelength, interval: 1, Wavelengths.Select(_ => 8.0).ToArray());
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.3333, 0.3333, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }

    [Test]
    public void LinearTowardsRed()
    {
        var spd = new Spd(StartWavelength, interval: 1, Wavelengths.Select(wavelength => (double)(wavelength - StartWavelength)).ToArray());
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.4299, 0.4040, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }
    
    [Test]
    public void LinearTowardsBlue()
    {
        var spd = new Spd(StartWavelength, interval: 1, Wavelengths.Select(wavelength => (double)(EndWavelength - wavelength)).ToArray());
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.2762, 0.2916, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }
    
    [Test]
    public void ExponentialTowardsRed()
    {
        var spd = new Spd(StartWavelength, interval: 1, Wavelengths.Select(wavelength => Math.Pow(wavelength - StartWavelength, 3)).ToArray());
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.5542, 0.4128, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }
    
    [Test]
    public void ExponentialTowardsBlue()
    {
        var spd = new Spd(StartWavelength, interval: 1, Wavelengths.Select(wavelength => Math.Pow(EndWavelength - wavelength, 3)).ToArray());
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.2013, 0.2004, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }

    [Test]
    public void YellowSpike()
    {
        var spd = Spd.Monochromatic(580);
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(0.5125, 0.4866, 1.0000), 0.00005);
        AssertWhitePoint(spd);
    }
    
    [Test]
    public void NoPower()
    {
        // an empty SPD is equivalent to all wavelengths having 0 power, and treated as not processable
        // as opposed to some kind of "black" illumination (illuminating with the absence of any light...)
        var spd = new Spd(start: 580, interval: 1);
        var unicolour = new Unicolour(spd);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(double.NaN, double.NaN, double.NaN), 0);
        TestUtils.AssertTriplet<Xyy>(unicolour, new(double.NaN, double.NaN, double.NaN), 0);
    }

    private static void AssertWhitePoint(Spd spd)
    {
        // regardless of the spectral power distribution
        // if the SPD is set as the white point, RGB will be white
        var xyzConfig = new XyzConfiguration(new Illuminant(spd), Observer.Degree2);
        var config = new Configuration(xyzConfig: xyzConfig);
        var unicolour = new Unicolour(config, spd);
        TestUtils.AssertTriplet<Rgb>(unicolour, new(1.0, 1.0, 1.0), 0.00000000001);
    }
}