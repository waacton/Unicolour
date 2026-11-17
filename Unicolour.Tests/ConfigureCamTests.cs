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
        
        var xyzConfig = new XyzConfiguration(xyzIlluminant, Observer.Degree2);
        var xyz = Cam02.Cam.ToXyz(expectedCam, CamConfiguration.StandardRgb.WhitePoint, xyzConfig.ChromaticAdaptor);
        var cam = Cam02.Cam.FromXyz(xyz, CamConfiguration.StandardRgb.WhitePoint, CamConfiguration.StandardRgb.View02, xyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(cam.Jch, expectedCam.Jch, 0.00000000001);
    }
    
    [Test]
    public void XyzWhitePointRoundTripViaCam16([ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(CamConfiguration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974, initialXyzConfig.WhitePoint);
        var expectedCam = Cam16.Cam.FromXyz(initialXyz, CamConfiguration.StandardRgb.WhitePoint, CamConfiguration.StandardRgb.View16, initialXyzConfig.ChromaticAdaptor);
        
        var xyzConfig = new XyzConfiguration(xyzIlluminant, Observer.Degree2);
        var xyz = Cam16.Cam.ToXyz(expectedCam, CamConfiguration.StandardRgb.WhitePoint, xyzConfig.ChromaticAdaptor);
        var cam = Cam16.Cam.FromXyz(xyz, CamConfiguration.StandardRgb.WhitePoint, CamConfiguration.StandardRgb.View16, xyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(cam.Jch, expectedCam.Jch, 0.00000000001);
    }
    
    [Test]
    public void XyzWhitePointRoundTripViaHellwig([ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant xyzIlluminant)
    {
        var initialXyzConfig = new XyzConfiguration(CamConfiguration.StandardRgb.WhitePoint);
        var initialXyz = new Xyz(0.4676, 0.2387, 0.2974, initialXyzConfig.WhitePoint);
        var expectedCam = Hellwig.Cam.FromXyz(initialXyz, CamConfiguration.StandardRgb.WhitePoint, CamConfiguration.StandardRgb.ViewHellwig, initialXyzConfig.ChromaticAdaptor);
        
        var xyzConfig = new XyzConfiguration(xyzIlluminant, Observer.Degree2);
        var xyz = Hellwig.Cam.ToXyz(expectedCam, CamConfiguration.StandardRgb.WhitePoint, xyzConfig.ChromaticAdaptor);
        var cam = Hellwig.Cam.FromXyz(xyz, CamConfiguration.StandardRgb.WhitePoint, CamConfiguration.StandardRgb.ViewHellwig, xyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(cam.Jmh, expectedCam.Jmh, 0.00000000001);
    }
}