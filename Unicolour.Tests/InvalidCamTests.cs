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
}