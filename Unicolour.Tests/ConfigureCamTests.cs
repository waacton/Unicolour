namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ConfigureCamTests
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
    public void XyzWhitePointRoundTripViaCam02(Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(CamConfiguration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974);
        var expectedCam = Cam02.FromXyz(initialXyz, CamConfiguration.StandardRgb, initialXyzConfig);

        var xyzConfig = new XyzConfiguration(WhitePoint.From(xyzIlluminant));
        var xyz = Cam02.ToXyz(expectedCam, CamConfiguration.StandardRgb, xyzConfig);
        var cam = Cam02.FromXyz(xyz, CamConfiguration.StandardRgb, xyzConfig);
        AssertUtils.AssertTriplet(cam.Triplet, expectedCam.Triplet, 0.00000000001);
    }
    
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
    public void XyzWhitePointRoundTripViaCam16(Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(CamConfiguration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974);
        var expectedCam = Cam16.FromXyz(initialXyz, CamConfiguration.StandardRgb, initialXyzConfig);

        var xyzConfig = new XyzConfiguration(WhitePoint.From(xyzIlluminant));
        var xyz = Cam16.ToXyz(expectedCam, CamConfiguration.StandardRgb, xyzConfig);
        var cam = Cam16.FromXyz(xyz, CamConfiguration.StandardRgb, xyzConfig);
        AssertUtils.AssertTriplet(cam.Triplet, expectedCam.Triplet, 0.00000000001);
    }

    [Test]
    public void InvalidSurround()
    {
        const Surround badSurround = (Surround)int.MaxValue;
        var camConfig = new CamConfiguration(WhitePoint.StandardRgb, 0, 0, badSurround);
        Assert.Throws<ArgumentOutOfRangeException>(() => { _ = camConfig.F; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { _ = camConfig.C; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { _ = camConfig.Nc; });
    }
}