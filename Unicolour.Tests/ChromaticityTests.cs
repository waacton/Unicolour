namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public class ChromaticityTests
{
    // https://en.wikipedia.org/wiki/Standard_illuminant#White_points_of_standard_illuminants
    [TestCase(0.44757, 0.40745, Illuminant.A, Observer.Standard2)]
    [TestCase(0.45117, 0.40594, Illuminant.A, Observer.Supplementary10)]
    [TestCase(0.31006, 0.31616, Illuminant.C, Observer.Standard2)]
    [TestCase(0.31039, 0.31905, Illuminant.C, Observer.Supplementary10)]
    [TestCase(0.34567, 0.35850, Illuminant.D50, Observer.Standard2)]
    [TestCase(0.34773, 0.35952, Illuminant.D50, Observer.Supplementary10)]
    [TestCase(0.33242, 0.34743, Illuminant.D55, Observer.Standard2)]
    [TestCase(0.33411, 0.34877, Illuminant.D55, Observer.Supplementary10)]
    [TestCase(0.31271, 0.32902, Illuminant.D65, Observer.Standard2)]
    [TestCase(0.31382, 0.33100, Illuminant.D65, Observer.Supplementary10)]
    [TestCase(0.29902, 0.31485, Illuminant.D75, Observer.Standard2)]
    [TestCase(0.29968, 0.31740, Illuminant.D75, Observer.Supplementary10)]
    [TestCase(0.33333, 0.33333, Illuminant.E, Observer.Standard2)]
    [TestCase(0.33333, 0.33333, Illuminant.E, Observer.Supplementary10)]
    [TestCase(0.37208, 0.37529, Illuminant.F2, Observer.Standard2)]
    [TestCase(0.37925, 0.36733, Illuminant.F2, Observer.Supplementary10)]
    [TestCase(0.31292, 0.32933, Illuminant.F7, Observer.Standard2)]
    [TestCase(0.31569, 0.32960, Illuminant.F7, Observer.Supplementary10)]
    [TestCase(0.38052, 0.37713, Illuminant.F11, Observer.Standard2)]
    [TestCase(0.38541, 0.37123, Illuminant.F11, Observer.Supplementary10)]
    public void MatchesIlluminant(double x, double y, Illuminant illuminant, Observer observer)
    {
        var chromaticityWhitePoint = WhitePoint.From(new(x, y));
        var illuminantWhitePoint = WhitePoint.From(illuminant, observer);
        
        const double tolerance = 0.125; // XYZ in approx. range [0, 100], and don't know the reliability of wikipedia values 
        Assert.That(chromaticityWhitePoint.X, Is.EqualTo(illuminantWhitePoint.X).Within(tolerance));
        Assert.That(chromaticityWhitePoint.Y, Is.EqualTo(illuminantWhitePoint.Y).Within(tolerance));
        Assert.That(chromaticityWhitePoint.Z, Is.EqualTo(illuminantWhitePoint.Z).Within(tolerance));
    }
}