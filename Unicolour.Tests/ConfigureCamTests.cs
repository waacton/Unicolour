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
        var expectedCam = Cam02.FromXyz(initialXyz, CamConfiguration.StandardRgb, initialXyzConfig.ChromaticAdaptor);

        var xyzConfig = new XyzConfiguration(xyzIlluminant, Observer.Degree2);
        var xyz = Cam02.ToXyz(expectedCam, CamConfiguration.StandardRgb, xyzConfig.ChromaticAdaptor);
        var cam = Cam02.FromXyz(xyz, CamConfiguration.StandardRgb, xyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(cam.Triplet, expectedCam.Triplet, 0.00000000001);
    }
    
    [Test]
    public void XyzWhitePointRoundTripViaCam16([ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(CamConfiguration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974, initialXyzConfig.WhitePoint);
        var expectedCam = Cam16.FromXyz(initialXyz, CamConfiguration.StandardRgb, initialXyzConfig.ChromaticAdaptor);

        var xyzConfig = new XyzConfiguration(xyzIlluminant, Observer.Degree2);
        var xyz = Cam16.ToXyz(expectedCam, CamConfiguration.StandardRgb, xyzConfig.ChromaticAdaptor);
        var cam = Cam16.FromXyz(xyz, CamConfiguration.StandardRgb, xyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(cam.Triplet, expectedCam.Triplet, 0.00000000001);
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