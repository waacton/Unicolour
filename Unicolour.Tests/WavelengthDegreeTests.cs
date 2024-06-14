namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class WavelengthDegreeTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    // ReSharper disable CollectionNeverQueried.Local - used in test case sources by name
    private static readonly List<double> Wavelengths = new();
    private static readonly List<double> Degrees = new();
    
    static WavelengthDegreeTests()
    {
        for (var i = 0; i < 1000; i++)
        {
            Wavelengths.Add(RandomColours.Rng() >= 0.5 ? RandomColours.Rng(360, 700) : RandomColours.Rng(-566, -493.5));
            Degrees.Add(RandomColours.Rng(0, 360));
        }
        
        Wavelengths.Add(double.NaN);
        Degrees.Add(double.NaN);
    }
    
    [TestCaseSource(nameof(Wavelengths))]
    public void WavelengthToDegree(double wavelength)
    {
        var degree = Wxy.WavelengthToDegree(wavelength, XyzConfig);
        var roundtrip = Wxy.DegreeToWavelength(degree, XyzConfig);
        Assert.That(roundtrip, Is.EqualTo(wavelength).Within(Tolerance));
    }
    
    [TestCaseSource(nameof(Degrees))]
    public void DegreeToWavelength(double degree)
    {
        var wavelength = Wxy.DegreeToWavelength(degree, XyzConfig);
        var roundtrip = Wxy.WavelengthToDegree(wavelength, XyzConfig);
        Assert.That(roundtrip, Is.EqualTo(degree).Within(Tolerance));
    }
}