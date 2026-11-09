using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ConfigureCamTests
{
    [Test]
    public void XyzWhitePointRoundTripViaCam02([ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(CamConfiguration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974, initialXyzConfig.WhitePoint);
        var expectedCam = Cam02.Cam.FromXyz(initialXyz, CamConfiguration.StandardRgb.WhitePoint, CamConfiguration.StandardRgb.View02, initialXyzConfig.ChromaticAdaptor);
        var expectedCamUcs = Cam02.Ucs.FromCam(expectedCam);

        var xyzConfig = new XyzConfiguration(xyzIlluminant, Observer.Degree2);
        var xyz = Cam02.Cam.ToXyz(expectedCam, CamConfiguration.StandardRgb.WhitePoint, xyzConfig.ChromaticAdaptor);
        var cam = Cam02.Cam.FromXyz(xyz, CamConfiguration.StandardRgb.WhitePoint, CamConfiguration.StandardRgb.View02, xyzConfig.ChromaticAdaptor);
        var camUcs = Cam02.Ucs.FromCam(cam);
        TestUtils.AssertTriplet(camUcs, expectedCamUcs, 0.00000000001);
    }
    
    [Test] // TODO: Hellwig variant
    public void XyzWhitePointRoundTripViaCam16([ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(CamConfiguration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974, initialXyzConfig.WhitePoint);
        var expectedCam = Cam16.Cam.FromXyz(initialXyz, CamConfiguration.StandardRgb.WhitePoint, CamConfiguration.StandardRgb.View16, initialXyzConfig.ChromaticAdaptor);
        var expectedCamUcs = Cam16.Ucs.FromCam(expectedCam);

        var xyzConfig = new XyzConfiguration(xyzIlluminant, Observer.Degree2);
        var xyz = Cam16.Cam.ToXyz(expectedCam, CamConfiguration.StandardRgb.WhitePoint, xyzConfig.ChromaticAdaptor);
        var cam = Cam16.Cam.FromXyz(xyz, CamConfiguration.StandardRgb.WhitePoint, CamConfiguration.StandardRgb.View16, xyzConfig.ChromaticAdaptor);
        var camUcs = Cam16.Ucs.FromCam(cam);
        TestUtils.AssertTriplet(camUcs, expectedCamUcs, 0.00000000001);
    }

    [Test]
    public void InvalidSurround()
    {
        const Surround badSurround = (Surround)int.MaxValue;
        var camConfig = new CamConfiguration(RgbModels.StandardRgb.WhitePoint, 0, 0, badSurround);
        Assert.Throws<ArgumentOutOfRangeException>(() => { _ = camConfig.F; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { _ = camConfig.C; });
        Assert.Throws<ArgumentOutOfRangeException>(() => { _ = camConfig.Nc; });
    }
}