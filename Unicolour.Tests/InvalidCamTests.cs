using System;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class InvalidCamTests
{
    [Test]
    public void InvalidCam02()
    {
        var cam = new Cam02.Cam(new Rgb(0.1, 0.2, 0.3), Configuration.Default.Cam.View02);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = cam.Model);
    }
    
    [Test]
    public void InvalidCam16()
    {
        var cam = new Cam16.Cam(new Rgb(0.1, 0.2, 0.3), Configuration.Default.Cam.View16);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = cam.Model);
    }
    
    [Test]
    public void InvalidHellwig()
    {
        var cam = new Hellwig.Cam(new Rgb(0.1, 0.2, 0.3), Configuration.Default.Cam.ViewHellwig);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = cam.Model);
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