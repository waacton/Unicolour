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
    public void PredefinedValid(Spd spd)
    {
        Assert.That(spd.IsValid, Is.True);
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public void CustomValidInterval(int interval)
    {
        var spd = new Spd(Spd.ExpectedWavelengths(interval).ToDictionary(wavelength => wavelength, _ => 1.0));
        Assert.That(spd.WavelengthDelta, Is.EqualTo(interval));
        Assert.That(spd.IsValid, Is.True);
    }

    [Test]
    public void CustomValidSingleWavelength()
    {
        var spd = new Spd { { 580, 1.0 } };
        Assert.That(spd.WavelengthDelta, Is.EqualTo(1));
        Assert.That(spd.IsValid, Is.True);
    }
    
    [Test]
    public void CustomInvalidNoWavelengths()
    {
        var spd = new Spd();
        Assert.That(spd.WavelengthDelta, Is.EqualTo(1));
        Assert.That(spd.IsValid, Is.False);
    }
    
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(6)]
    [TestCase(10)]
    [TestCase(20)]
    public void CustomInvalidInterval(int interval)
    {
        var spd = new Spd(Spd.ExpectedWavelengths(interval).ToDictionary(wavelength => wavelength, _ => 1.0));
        Assert.That(spd.WavelengthDelta, Is.EqualTo(interval));
        Assert.That(spd.IsValid, Is.False);
    }
    
    [Test]
    public void CustomInvalidMultipleIntervals()
    {
        var spd = new Spd
        {
            { 360, 1.0 },
            { 361, 1.0 },
            { 362, 1.0 },
            { 363, 1.0 },
            { 364, 1.0 },
            { 365, 1.0 },
            { 370, 1.0 },
            { 371, 1.0 },
            { 372, 1.0 },
            { 373, 1.0 },
            { 374, 1.0 },
            { 375, 1.0 }
        };
        
        Assert.That(spd.WavelengthDelta, Is.EqualTo(1)); // delta assumed to be item[1] - item[0]
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
        var spd = Daylight.GetSpd(kelvins * 1.4388 / 1.4380);
        
        foreach (var (wavelength, spectralPower) in spd)
        {
            // predefined D55 & D75 SPDs only go to 780 nm
            if (!expectedSpd.TryGetValue(wavelength, out var value))
            {
                continue;
            }
            
            // how does the CIE spectral power calculation not match the tabular data provided by CIE more closely? 🤷
            Assert.That(spectralPower, Is.EqualTo(value).Within(0.0175));
        }
    }
}