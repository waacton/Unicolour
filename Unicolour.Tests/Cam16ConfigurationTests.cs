namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class Cam16ConfigurationTests
{
    [TestCase(Illuminant.A)]
    [TestCase(Illuminant.C)]
    [TestCase(Illuminant.D50)]
    [TestCase(Illuminant.D55)]
    [TestCase(Illuminant.D65)]
    [TestCase(Illuminant.D75)]
    [TestCase(Illuminant.E)]
    [TestCase(Illuminant.F2)]
    [TestCase(Illuminant.F7)]
    [TestCase(Illuminant.F11)]
    public static void XyzWhitePointRoundTrip(Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(Cam16Configuration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974);
        var expectedCam16 = Cam16.FromXyz(initialXyz, Cam16Configuration.StandardRgb, initialXyzConfig);

        var xyzConfig = new XyzConfiguration(WhitePoint.From(xyzIlluminant));
        var xyz = Cam16.ToXyz(expectedCam16, Cam16Configuration.StandardRgb, xyzConfig);
        var cam16 = Cam16.FromXyz(xyz, Cam16Configuration.StandardRgb, xyzConfig);
        AssertUtils.AssertTriplet(cam16.Triplet, expectedCam16.Triplet, 0.00000000001);
    }
}