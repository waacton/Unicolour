namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class CmfDefinitionTests
{
    public static readonly List<TestCaseData> PredefinedTestData = new()
    {
        new TestCaseData(Cmf.Degree2).SetName("2°"),
        new TestCaseData(Cmf.Degree10).SetName("10°")
    };
    
    [TestCaseSource(nameof(PredefinedTestData))]
    public void PredefinedValid(Cmf cmf)
    {
        Assert.That(cmf.IsValid, Is.True);
    }

    [Test]
    public void CustomValid()
    {
        var cmf = new Cmf(Cmf.RequiredWavelengths.ToDictionary(wavelength => wavelength, _ => (1.0, 1.0, 1.0)));
        Assert.That(cmf.IsValid, Is.True);
    }

    [Test]
    public void CustomInvalidEmpty()
    {
        var cmf = new Cmf();
        Assert.That(cmf.IsValid, Is.False);
    }
    
    [Test]
    public void CustomInvalidMin()
    {
        var cmf = Cmf.Degree2;
        cmf[Cmf.StartWavelength - 1] = (0, 0, 0);
        Assert.That(cmf.IsValid, Is.False);

        cmf.Remove(Cmf.EndWavelength);
        Assert.That(cmf.IsValid, Is.False);
    }
    
    [Test]
    public void CustomInvalidMax()
    {
        var cmf = Cmf.Degree10;
        cmf[Cmf.EndWavelength + 1] = (0, 0, 0);
        Assert.That(cmf.IsValid, Is.False);

        cmf.Remove(Cmf.StartWavelength);
        Assert.That(cmf.IsValid, Is.False);
    }
}