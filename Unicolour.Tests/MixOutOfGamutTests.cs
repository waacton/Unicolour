using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class MixOutOfGamutTests
{
    [Test]
    public void AchromaticWhite()
    {
        var white = new Unicolour(ColourSpace.Rgb, 1, 1, 1); // HSB     —°   0.0% 100.0%
        var green = new Unicolour(ColourSpace.Rgb, 0, 1, 0); // HSB 120.0° 100.0% 100.0%
        var mixed = white.Mix(green, ColourSpace.Hsb);
        
        TestUtils.AssertColour(mixed, new Hsb(120, 0.5, 1.0), 0);
        
        Assert.That(white.Description.Contains("white"));
        Assert.That(green.Description.Contains("green"));
        Assert.That(mixed.Description.Contains("green"));
    }
    
    [Test]
    public void BlueWhite()
    {
        var white = new Unicolour(ColourSpace.Rgb, 1, 1, 2); // HSB 240.0° 50.0% 200.0%
        var green = new Unicolour(ColourSpace.Rgb, 0, 1, 0); // HSB 120.0° 100.0% 100.0%
        var mixed = white.Mix(green, ColourSpace.Hsb);
        
        TestUtils.AssertColour(white, new Hsb(240, 0.5, 2), 0);
        TestUtils.AssertColour(white.MapToRgbGamut(GamutMap.RgbClipping), new Rgb(1, 1, 1), 0);
        TestUtils.AssertColour(mixed, new Hsb(180, 0.75, 1.5), 0);
        
        Assert.That(white.Description.Contains("white"));
        Assert.That(green.Description.Contains("green"));
        Assert.That(mixed.Description.Contains("cyan")); // hue has shifted, despite one of the colours appearing to be white
    }
}