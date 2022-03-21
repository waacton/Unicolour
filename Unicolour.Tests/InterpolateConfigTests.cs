namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class InterpolateConfigTests
{
    [Test]
    public void UndefinedConfig()
    {
        var unicolour1 = Unicolour.FromRgb(0.5, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromRgb(0.5, 0.25, 0.75, 0.5);
        AssertNoError(unicolour1, unicolour2);
    }
    
    [Test]
    public void DefaultConfig()
    {
        var unicolour1 = Unicolour.FromRgb(Configuration.Default, 0.5, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromRgb(Configuration.Default, 0.5, 0.25, 0.75, 0.5);
        AssertNoError(unicolour1, unicolour2);
    }
    
    [Test]
    public void SameConfig()
    {
        var config = GetConfig();
        var unicolour1 = Unicolour.FromRgb(config, 0.5, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromRgb(config, 0.5, 0.25, 0.75, 0.5);
        AssertNoError(unicolour1, unicolour2);
    }
    
    [Test]
    public void DifferentConfig()
    {
        var config1 = GetConfig();
        var config2 = GetConfig();
        var unicolour1 = Unicolour.FromRgb(config1, 0.5, 0.25, 0.75, 0.5);
        var unicolour2 = Unicolour.FromRgb(config2, 0.5, 0.25, 0.75, 0.5);
        AssertError(unicolour1, unicolour2);
    }

    private static Configuration GetConfig()
    {
        return new Configuration(
            Chromaticity.StandardRgbR,
            Chromaticity.StandardRgbG,
            Chromaticity.StandardRgbB,
            Companding.InverseStandardRgb, 
            WhitePoint.From(Illuminant.D65), 
            WhitePoint.From(Illuminant.D65));
    }

    private static void AssertNoError(Unicolour unicolour1, Unicolour unicolour2)
    {
        Assert.DoesNotThrow(() => unicolour1.InterpolateRgb(unicolour2, 0.5));
        Assert.DoesNotThrow(() => unicolour2.InterpolateRgb(unicolour1, 0.5));
        Assert.DoesNotThrow(() => unicolour1.InterpolateHsb(unicolour2, 0.5));
        Assert.DoesNotThrow(() => unicolour2.InterpolateHsb(unicolour1, 0.5));
    }
    
    private static void AssertError(Unicolour unicolour1, Unicolour unicolour2)
    {
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateRgb(unicolour2, 0.5));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateRgb(unicolour1, 0.5));
        Assert.Catch<InvalidOperationException>(() => unicolour1.InterpolateHsb(unicolour2, 0.5));
        Assert.Catch<InvalidOperationException>(() => unicolour2.InterpolateHsb(unicolour1, 0.5));
    }
}