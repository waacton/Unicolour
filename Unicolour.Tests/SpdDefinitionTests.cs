using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class SpdDefinitionTests
{
    public static readonly List<TestCaseData> PredefinedTestData =
    [
        new TestCaseData(Spd.A).SetName(nameof(Spd.A)),
        new TestCaseData(Spd.C).SetName(nameof(Spd.C)),
        new TestCaseData(Spd.D50).SetName(nameof(Spd.D50)),
        new TestCaseData(Spd.D55).SetName(nameof(Spd.D55)),
        new TestCaseData(Spd.D65).SetName(nameof(Spd.D65)),
        new TestCaseData(Spd.D75).SetName(nameof(Spd.D75)),
        new TestCaseData(Spd.E).SetName(nameof(Spd.E)),
        new TestCaseData(Spd.F2).SetName(nameof(Spd.F2)),
        new TestCaseData(Spd.F7).SetName(nameof(Spd.F7)),
        new TestCaseData(Spd.F11).SetName(nameof(Spd.F11))
    ];
    
    [TestCaseSource(nameof(PredefinedTestData))]
    public void Predefined(Spd spd)
    {
        Assert.That(spd.IsValid, Is.True);
    }
    
    [TestCase(-5)]
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void IntervalValid(int interval)
    {
        double[] coefficients = interval == 0 ? [0.5] : [0, 0.2, 0.4, 0.6, 0.8, 1.0];
        var spd = new Spd(start: 360, interval, coefficients);
        Assert.That(spd.IsValid, Is.True);
    }
    
    [TestCase(-20)]
    [TestCase(-10)]
    [TestCase(-6)]
    [TestCase(-4)]
    [TestCase(-3)]
    [TestCase(-2)]
    [TestCase(0)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(6)]
    [TestCase(10)]
    [TestCase(20)]
    public void IntervalInvalid(int interval)
    {
        double[] coefficients = [0, 0.2, 0.4, 0.6, 0.8, 1.0];
        var spd = new Spd(start: 360, interval, coefficients);
        Assert.That(spd.IsValid, Is.False);
    }
    
    public static readonly List<TestCaseData> DaylightTestData =
    [
        new TestCaseData(5000, Spd.D50).SetName("5000 K"),
        new TestCaseData(5500, Spd.D55).SetName("5500 K"),
        new TestCaseData(6500, Spd.D65).SetName("6500 K"),
        new TestCaseData(7500, Spd.D75).SetName("7500 K")
    ];

    [TestCaseSource(nameof(DaylightTestData))]
    public void DaylightSpd(int kelvins, Spd expectedSpd)
    {
        // correct kelvin due to the revision of constants in Planck's law (e.g. https://en.wikipedia.org/wiki/Illuminant_D65#Color_temperature)
        // although using simplified value of 1.4388 for c2 as adopted in ITS-90
        var daylightSpd = Daylight.GetSpd(kelvins * 1.4388 / 1.4380);
        
        foreach (var wavelength in daylightSpd.Wavelengths)
        {
            // predefined D55 & D75 SPDs only go to 780 nm
            if (!expectedSpd.Wavelengths.Contains(wavelength))
            {
                continue;
            }

            var expectedSpectralPower = expectedSpd[wavelength];
            var actualSpectralPower = daylightSpd[wavelength];
            // how does the CIE spectral power calculation not match the tabular data provided by CIE more closely? 🤷
            Assert.That(actualSpectralPower, Is.EqualTo(expectedSpectralPower).Within(0.0175));
        }
    }
}