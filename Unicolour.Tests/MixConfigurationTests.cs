namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class MixConfigurationTests
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
        return new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D65, CamConfiguration.StandardRgb);
    }

    private static void AssertNoError(Unicolour unicolour1, Unicolour unicolour2)
    {
        Assert.That(unicolour1.Config.Id, Is.EqualTo(unicolour2.Config.Id));
        Assert.DoesNotThrow(() => unicolour1.MixRgb(unicolour2, 0.5, false));
        Assert.DoesNotThrow(() => unicolour2.MixRgb(unicolour1, 0.5, false));
        Assert.DoesNotThrow(() => unicolour1.MixHsb(unicolour2, 0.5, false));
        Assert.DoesNotThrow(() => unicolour2.MixHsb(unicolour1, 0.5, false));
    }
    
    private static void AssertError(Unicolour unicolour1, Unicolour unicolour2)
    {
        Assert.That(unicolour1.Config.Id, Is.Not.EqualTo(unicolour2.Config.Id));
        Assert.Throws<InvalidOperationException>(() => unicolour1.MixRgb(unicolour2, 0.5, false));
        Assert.Throws<InvalidOperationException>(() => unicolour2.MixRgb(unicolour1, 0.5, false));
        Assert.Throws<InvalidOperationException>(() => unicolour1.MixHsb(unicolour2, 0.5, false));
        Assert.Throws<InvalidOperationException>(() => unicolour2.MixHsb(unicolour1, 0.5, false));
    }
}