namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public static class SmokeTests
{
    [Test]
    public static void UnicolourHex()
    {
        AssertNoError(Unicolour.FromHex("#000000"));
        AssertNoError(Unicolour.FromHex("#FFFFFF"));
        AssertNoError(Unicolour.FromHex("#667788"));
    }

    [Test]
    public static void UnicolourRgb255()
    {
        AssertNoError(Unicolour.FromRgb255(0, 0, 0));
        AssertNoError(Unicolour.FromRgb255(255, 255, 255));
        AssertNoError(Unicolour.FromRgb255(124, 125, 126));
    }

    [Test]
    public static void UnicolourRgb()
    {
        AssertNoError(Unicolour.FromRgb(0, 0, 0));
        AssertNoError(Unicolour.FromRgb(1, 1, 1));
        AssertNoError(Unicolour.FromRgb(0.4, 0.5, 0.6));
    }

    [Test]
    public static void UnicolourHsb()
    {
        AssertNoError(Unicolour.FromHsb(0, 0, 0));
        AssertNoError(Unicolour.FromHsb(360, 1, 1));
        AssertNoError(Unicolour.FromHsb(180, 0.4, 0.6));
    }

    [Test]
    public static void UnicolourHsl()
    {
        AssertNoError(Unicolour.FromHsl(0, 0, 0));
        AssertNoError(Unicolour.FromHsl(360, 1, 1));
        AssertNoError(Unicolour.FromHsl(180, 0.4, 0.6));
    }

    [Test]
    public static void UnicolourXyz()
    {
        AssertNoError(Unicolour.FromXyz(0, 0, 0));
        AssertNoError(Unicolour.FromXyz(1, 1, 1));
        AssertNoError(Unicolour.FromXyz(0.4, 0.5, 0.6));
    }

    [Test]
    public static void UnicolourLab()
    {
        AssertNoError(Unicolour.FromLab(0, -128, -128));
        AssertNoError(Unicolour.FromLab(100, 128, 128));
        AssertNoError(Unicolour.FromLab(50, -1, 1));
    }
    
    private static void AssertNoError(Unicolour unicolour)
    {
        Assert.DoesNotThrow(() => _ = unicolour.Rgb);
        Assert.DoesNotThrow(() => _ = unicolour.Hsb);
        Assert.DoesNotThrow(() => _ = unicolour.Hsl);
        Assert.DoesNotThrow(() => _ = unicolour.Xyz);
        Assert.DoesNotThrow(() => _ = unicolour.Lab);
        Assert.DoesNotThrow(() => _ = unicolour.Alpha);
        Assert.DoesNotThrow(() => _ = unicolour.Config);
        Assert.DoesNotThrow(() => _ = unicolour.Luminance);
    }
}

