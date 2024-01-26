namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixConfigurationTests
{
    [Test]
    public void UndefinedConfig()
    {
        var unicolour1 = new Unicolour(ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        var unicolour2 = new Unicolour(ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        AssertConfig(unicolour1, unicolour2, expectSameId: true);
    }
    
    [Test]
    public void DefaultConfig()
    {
        var unicolour1 = new Unicolour(Configuration.Default, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        var unicolour2 = new Unicolour(Configuration.Default, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        AssertConfig(unicolour1, unicolour2, expectSameId: true);
    }
    
    [Test]
    public void SameConfig()
    {
        var config = GetConfig();
        var unicolour1 = new Unicolour(config, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        var unicolour2 = new Unicolour(config, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        AssertConfig(unicolour1, unicolour2, expectSameId: true);
    }
    
    [Test]
    public void DifferentConfigSameValues()
    {
        var config1 = GetConfig();
        var config2 = GetConfig();
        var unicolour1 = new Unicolour(config1, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        var unicolour2 = new Unicolour(config2, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        AssertConfig(unicolour1, unicolour2, expectSameId: false);
    }
    
    [Test]
    public void DifferentConfigDifferentValues()
    {
        var config1 = GetConfig();
        var config2 = GetConfig(defaultConfig: false);
        var unicolour1 = new Unicolour(config1, ColourSpace.Rgb, 0.0, 0.25, 0.75, 1.0);
        var unicolour2 = new Unicolour(config1, ColourSpace.Rgb, 1.0, 0.75, 0.25, 0.0);
        var defaultConfigHex = unicolour2.Hex;
        unicolour2 = unicolour2.ConvertToConfiguration(config2);
        Assert.That(unicolour2.Hex, Is.Not.EqualTo(defaultConfigHex));
        AssertConfig(unicolour1, unicolour2, expectSameId: false);

        // unicolour 2 should be converted back to config 1, therefore interpolating halfway between original values
        var mixed = unicolour1.Mix(unicolour2, ColourSpace.Rgb, premultiplyAlpha: false);
        TestUtils.AssertTriplet<Rgb>(mixed, new(0.5, 0.5, 0.5), 0.00000000005);
        Assert.That(mixed.Alpha.A, Is.EqualTo(0.5));
    }

    private static Configuration GetConfig(bool defaultConfig = true)
    {
        return defaultConfig
            ? new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D65, CamConfiguration.StandardRgb)
            : new Configuration(RgbConfiguration.DisplayP3, XyzConfiguration.D50, CamConfiguration.Hct);
    }

    private static void AssertConfig(Unicolour unicolour1, Unicolour unicolour2, bool expectSameId)
    {
        Assert.That(unicolour1.Config.Id, expectSameId ? Is.EqualTo(unicolour2.Config.Id) : Is.Not.EqualTo(unicolour2.Config.Id));
        
        var mix1 = unicolour1.Mix(unicolour2, ColourSpace.Rgb, premultiplyAlpha: false);
        var mix2 = unicolour2.Mix(unicolour1, ColourSpace.Rgb, premultiplyAlpha: false);
        var mix3 = unicolour1.Mix(unicolour2, ColourSpace.Hsb, premultiplyAlpha: false);
        var mix4 = unicolour2.Mix(unicolour1, ColourSpace.Hsb, premultiplyAlpha: false);
        
        Assert.That(mix1.Config, Is.EqualTo(unicolour1.Config));
        Assert.That(mix2.Config, Is.EqualTo(unicolour2.Config));
        Assert.That(mix3.Config, Is.EqualTo(unicolour1.Config));
        Assert.That(mix4.Config, Is.EqualTo(unicolour2.Config));
    }
}