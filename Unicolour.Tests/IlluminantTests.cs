namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class IlluminantTests
{
    public static readonly List<TestCaseData> PredefinedWhitePointTestData = new()
    {
        new TestCaseData(Spd.A, Illuminant.A, Observer.Degree2).SetName("A/2°"),
        new TestCaseData(Spd.C, Illuminant.C, Observer.Degree2).SetName("C/2°"),
        new TestCaseData(Spd.D50, Illuminant.D50, Observer.Degree2).SetName("D50/2°"),
        new TestCaseData(Spd.D55, Illuminant.D55, Observer.Degree2).SetName("D55/2°"),
        new TestCaseData(Spd.D65, Illuminant.D65, Observer.Degree2).SetName("D65/2°"),
        new TestCaseData(Spd.D75, Illuminant.D75, Observer.Degree2).SetName("D75/2°"),
        new TestCaseData(Spd.E, Illuminant.E, Observer.Degree2).SetName("E/2°"),
        new TestCaseData(Spd.F2, Illuminant.F2, Observer.Degree2).SetName("F2/2°"),
        new TestCaseData(Spd.F7, Illuminant.F7, Observer.Degree2).SetName("F7/2°"),
        new TestCaseData(Spd.F11, Illuminant.F11, Observer.Degree2).SetName("F11/2°"),
        
        new TestCaseData(Spd.A, Illuminant.A, Observer.Degree10).SetName("A/10°"),
        new TestCaseData(Spd.C, Illuminant.C, Observer.Degree10).SetName("C/10°"),
        new TestCaseData(Spd.D50, Illuminant.D50, Observer.Degree10).SetName("D50/10°"),
        new TestCaseData(Spd.D55, Illuminant.D55, Observer.Degree10).SetName("D55/10°"),
        new TestCaseData(Spd.D65, Illuminant.D65, Observer.Degree10).SetName("D65/10°"),
        new TestCaseData(Spd.D75, Illuminant.D75, Observer.Degree10).SetName("D75/10°"),
        new TestCaseData(Spd.E, Illuminant.E, Observer.Degree10).SetName("E/10°"),
        new TestCaseData(Spd.F2, Illuminant.F2, Observer.Degree10).SetName("F2/10°"),
        new TestCaseData(Spd.F7, Illuminant.F7, Observer.Degree10).SetName("F7/10°"),
        new TestCaseData(Spd.F11, Illuminant.F11, Observer.Degree10).SetName("F11/10°")
    };
    
    [TestCaseSource(nameof(PredefinedWhitePointTestData))]
    public void PredefinedWhitePoints(Spd spd, Illuminant illuminant, Observer observer)
    {
        // illuminant E presumably ignores observer colour matching functions
        // (since observer CMFs are not equal, even a constant SPD would not result in equal XYZ values)
        // so need to be slightly more tolerant in that case
        var illuminantFromSpd = new Illuminant(spd);
        var spdWhitePoint = illuminantFromSpd.GetWhitePoint(observer);
        var predefinedWhitePoint = illuminant.GetWhitePoint(observer);
        Assert.That(spdWhitePoint.X, Is.EqualTo(predefinedWhitePoint.X).Within(illuminant == Illuminant.E ? 0.05 : 0.0125));
        Assert.That(spdWhitePoint.Y, Is.EqualTo(predefinedWhitePoint.Y).Within(illuminant == Illuminant.E ? 0.05 : 0.0125));
        Assert.That(spdWhitePoint.Z, Is.EqualTo(predefinedWhitePoint.Z).Within(illuminant == Illuminant.E ? 0.05 : 0.0125));
        
        var config = new Configuration(xyzConfiguration: new XyzConfiguration(illuminant, observer));
        var fromSpdWhite = new Unicolour(config, ColourSpace.Xyz, spdWhitePoint.AsXyzMatrix().ToTriplet().Tuple);
        var fromRgbWhite = new Unicolour(config, ColourSpace.Rgb, 1, 1, 1);
        Assert.That(fromSpdWhite.Chromaticity.X, Is.EqualTo(fromRgbWhite.Chromaticity.X).Within(0.00005));
        Assert.That(fromSpdWhite.Chromaticity.Y, Is.EqualTo(fromRgbWhite.Chromaticity.Y).Within(0.00005));
        Assert.That(fromSpdWhite.Chromaticity.U, Is.EqualTo(fromRgbWhite.Chromaticity.U).Within(0.00005));
        Assert.That(fromSpdWhite.Chromaticity.V, Is.EqualTo(fromRgbWhite.Chromaticity.V).Within(0.00005));
        Assert.That(fromSpdWhite.Temperature.Cct, Is.EqualTo(fromRgbWhite.Temperature.Cct).Within(illuminant == Illuminant.E ? 2.0 : 0.75));
        Assert.That(fromSpdWhite.Temperature.Duv, Is.EqualTo(fromRgbWhite.Temperature.Duv).Within(illuminant == Illuminant.E ? 0.005 : 0.000005));
    }

    private static readonly Observer NotObserved = new(new Cmf(Cmf.RequiredWavelengths.ToDictionary(wavelength => wavelength, _ => (0.0, 0.0, 0.0))));
    public static readonly List<TestCaseData> CustomWhitePointTestData = new()
    {
        new TestCaseData(Observer.Degree2).SetName("2°"),
        new TestCaseData(Observer.Degree10).SetName("10°"),
        new TestCaseData(NotObserved).SetName("Not Observed")
    };
    
    [TestCaseSource(nameof(CustomWhitePointTestData))]
    public void CustomWhitePoint(Observer observer)
    {
        var expectedWhitePoint = new WhitePoint(50, 50, 50);
        var illuminant = new Illuminant(expectedWhitePoint);
        
        // regardless of the observer, the white point should always be the same as provided
        var actualWhitePoint = illuminant.GetWhitePoint(observer);
        Assert.That(actualWhitePoint, Is.EqualTo(expectedWhitePoint));

        var xyzConfig = new XyzConfiguration(illuminant, observer);
        var config = new Configuration(xyzConfiguration: xyzConfig);
        var unicolour = new Unicolour(config, "#FFFFFF");
        TestUtils.AssertTriplet<Xyz>(unicolour, new(0.5, 0.5, 0.5), 0.00000000001);
    }
}