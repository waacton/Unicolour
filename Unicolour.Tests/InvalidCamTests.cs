using System;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class InvalidCamTests
{
    [Test]
    public void InvalidCam02()
    {
        var cam02 = new Cam02(new Rgb(0.1, 0.2, 0.3), Configuration.Default.Cam.View02);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = cam02.Model);
    }
    
    [Test]
    public void InvalidCam16()
    {
        var cam16 = new Cam16(new Rgb(0.1, 0.2, 0.3), Configuration.Default.Cam.View16);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = cam16.Model);
    }
}