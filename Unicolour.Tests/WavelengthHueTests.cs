using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class WavelengthHueTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    // ReSharper disable CollectionNeverQueried.Local - used in test case sources by name
    private static readonly List<double> Wavelengths = [];
    private static readonly List<double> Hues = [];
    // ReSharper restore CollectionNeverQueried.Local
    
    static WavelengthHueTests()
    {
        for (var i = 0; i < 1000; i++)
        {
            Wavelengths.Add(Rng.Bool() ? Rng.Between(360, 700) : Rng.Between(-566, -493.5));
            Hues.Add(Rng.Between(0, 360));
        }
        
        Wavelengths.Add(double.NaN);
        Hues.Add(double.NaN);
    }
    
    [TestCaseSource(nameof(Wavelengths))]
    public void WavelengthToHue(double wavelength)
    {
        var hue = Hue.FromWavelength(wavelength, XyzConfig);
        var roundtrip = Hue.ToWavelength(hue, XyzConfig);
        Assert.That(roundtrip, Is.EqualTo(wavelength).Within(Tolerance));
    }
    
    [TestCaseSource(nameof(Hues))]
    public void HueToWavelength(double hue)
    {
        var wavelength = Hue.ToWavelength(hue, XyzConfig);
        var roundtrip = Hue.FromWavelength(wavelength, XyzConfig);
        Assert.That(roundtrip, Is.EqualTo(hue).Within(Tolerance));
    }
}