using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ChromaticityTests
{
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void SameAsXyy(ColourTriplet triplet)
    {
        var chromaticity = new Chromaticity(triplet.First, triplet.Second);
        var luminance = triplet.Third;
        var fromChromaticity = new Unicolour(chromaticity, luminance);
        var fromXyy = new Unicolour(ColourSpace.Xyy, chromaticity.X, chromaticity.Y, luminance);
        TestUtils.AssertTriplet(fromChromaticity.Xyy.Triplet, fromXyy.Xyy.Triplet, 0.0);
        Assert.That(fromChromaticity.Chromaticity, Is.EqualTo(fromXyy.Chromaticity));
    }
    
    // https://en.wikipedia.org/wiki/Standard_illuminant#White_points_of_standard_illuminants
    [TestCase(0.44757, 0.40745, nameof(Illuminant.A), nameof(Observer.Degree2))]
    [TestCase(0.45117, 0.40594, nameof(Illuminant.A), nameof(Observer.Degree10))]
    [TestCase(0.31006, 0.31616, nameof(Illuminant.C), nameof(Observer.Degree2))]
    [TestCase(0.31039, 0.31905, nameof(Illuminant.C), nameof(Observer.Degree10))]
    [TestCase(0.34567, 0.35850, nameof(Illuminant.D50), nameof(Observer.Degree2))]
    [TestCase(0.34773, 0.35952, nameof(Illuminant.D50), nameof(Observer.Degree10))]
    [TestCase(0.33242, 0.34743, nameof(Illuminant.D55), nameof(Observer.Degree2))]
    [TestCase(0.33411, 0.34877, nameof(Illuminant.D55), nameof(Observer.Degree10))]
    [TestCase(0.31271, 0.32902, nameof(Illuminant.D65), nameof(Observer.Degree2))]
    [TestCase(0.31382, 0.33100, nameof(Illuminant.D65), nameof(Observer.Degree10))]
    [TestCase(0.29902, 0.31485, nameof(Illuminant.D75), nameof(Observer.Degree2))]
    [TestCase(0.29968, 0.31740, nameof(Illuminant.D75), nameof(Observer.Degree10))]
    [TestCase(0.33333, 0.33333, nameof(Illuminant.E), nameof(Observer.Degree2))]
    [TestCase(0.33333, 0.33333, nameof(Illuminant.E), nameof(Observer.Degree10))]
    [TestCase(0.37208, 0.37529, nameof(Illuminant.F2), nameof(Observer.Degree2))]
    [TestCase(0.37925, 0.36733, nameof(Illuminant.F2), nameof(Observer.Degree10))]
    [TestCase(0.31292, 0.32933, nameof(Illuminant.F7), nameof(Observer.Degree2))]
    [TestCase(0.31569, 0.32960, nameof(Illuminant.F7), nameof(Observer.Degree10))]
    [TestCase(0.38052, 0.37713, nameof(Illuminant.F11), nameof(Observer.Degree2))]
    [TestCase(0.38541, 0.37123, nameof(Illuminant.F11), nameof(Observer.Degree10))]
    public void MatchesIlluminant(double x, double y, string illuminantName, string observerName)
    {
        var illuminant = TestUtils.Illuminants[illuminantName];
        var observer = TestUtils.Observers[observerName];
        
        var chromaticityWhitePoint = new Chromaticity(x, y).ToWhitePoint(1.0);
        var illuminantWhitePoint = illuminant.GetWhitePoint(observer);
        
        const double tolerance = 0.125; // XYZ in approx. range [0, 100], and don't know the reliability of wikipedia values 
        Assert.That(chromaticityWhitePoint.X, Is.EqualTo(illuminantWhitePoint.X).Within(tolerance));
        Assert.That(chromaticityWhitePoint.Y, Is.EqualTo(illuminantWhitePoint.Y).Within(tolerance));
        Assert.That(chromaticityWhitePoint.Z, Is.EqualTo(illuminantWhitePoint.Z).Within(tolerance));
    }
    
    // chromaticities taken from http://www.brucelindbloom.com/index.html?ColorCalculator.html (using white with different illuminants)
    [TestCase(5000, nameof(Illuminant.D50), 0.345669, 0.358496)]
    [TestCase(5500, nameof(Illuminant.D55), 0.332424, 0.347426)]
    [TestCase(6500, nameof(Illuminant.D65), 0.312727, 0.329023)]
    [TestCase(7500, nameof(Illuminant.D75), 0.299021, 0.314852)]
    public void DaylightCct(double cct, string illuminantName, double x, double y)
    {
        var illuminant = TestUtils.Illuminants[illuminantName];
        var daylightChromaticity = Daylight.GetChromaticity(cct * 1.4388 / 1.4380); // adjust for change in c2 constant

        var config = new Configuration(xyzConfiguration: new XyzConfiguration(illuminant, Observer.Degree2));
        var white = new Unicolour(config, ColourSpace.Rgb, 1, 1, 1);
        var whiteChromaticity = white.Chromaticity;
        
        // not sure why "official" D-illuminant chromaticity calculations appear less accurate that from white points
        Assert.That(whiteChromaticity.X, Is.EqualTo(x).Within(0.0000005));
        Assert.That(whiteChromaticity.Y, Is.EqualTo(y).Within(0.0000005));
        Assert.That(daylightChromaticity.X, Is.EqualTo(x).Within(0.0005));
        Assert.That(daylightChromaticity.Y, Is.EqualTo(y).Within(0.0005));
    }
}