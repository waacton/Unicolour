using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class MixConfigurationTests
{
    [Test]
    public void UndefinedConfig()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        AssertConfig(colour1, colour2, expectSameId: true);
    }
    
    [Test]
    public void DefaultConfig()
    {
        var colour1 = new Unicolour(Configuration.Default, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        var colour2 = new Unicolour(Configuration.Default, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        AssertConfig(colour1, colour2, expectSameId: true);
    }
    
    [Test]
    public void SameConfig()
    {
        var config = GetConfig();
        var colour1 = new Unicolour(config, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        var colour2 = new Unicolour(config, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        AssertConfig(colour1, colour2, expectSameId: true);
    }
    
    [Test]
    public void DifferentConfigSameValues()
    {
        var config1 = GetConfig();
        var config2 = GetConfig();
        var colour1 = new Unicolour(config1, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        var colour2 = new Unicolour(config2, ColourSpace.Rgb, 0.5, 0.25, 0.75, 0.5);
        AssertConfig(colour1, colour2, expectSameId: false);
    }
    
    [Test]
    public void DifferentConfigDifferentValues()
    {
        var config1 = GetConfig();
        var config2 = GetConfig(defaultConfig: false);
        var colour1 = new Unicolour(config1, ColourSpace.Rgb, 0.0, 0.25, 0.75, 1.0);
        var colour2 = new Unicolour(config1, ColourSpace.Rgb, 1.0, 0.75, 0.25, 0.0);
        var defaultConfigHex = colour2.Hex;
        colour2 = colour2.ConvertToConfiguration(config2);
        Assert.That(colour2.Hex, Is.Not.EqualTo(defaultConfigHex));
        AssertConfig(colour1, colour2, expectSameId: false);

        // unicolour 2 should be converted back to config 1, therefore interpolating halfway between original values
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        TestUtils.AssertTriplet<Rgb>(mixed, new(0.5, 0.5, 0.5), 0.00000000005);
        Assert.That(mixed.Alpha.A, Is.EqualTo(0.5));
    }

    private static Configuration GetConfig(bool defaultConfig = true)
    {
        return defaultConfig
            ? new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D65, YbrConfiguration.Rec601, CamConfiguration.StandardRgb)
            : new Configuration(RgbConfiguration.DisplayP3, XyzConfiguration.D50, YbrConfiguration.Rec709, CamConfiguration.Hct);
    }

    private static void AssertConfig(Unicolour colour1, Unicolour colour2, bool expectSameId)
    {
        Assert.That(colour1.Configuration.Id, expectSameId ? Is.EqualTo(colour2.Configuration.Id) : Is.Not.EqualTo(colour2.Configuration.Id));
        
        var mix1 = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        var mix2 = colour2.Mix(colour1, ColourSpace.Rgb, premultiplyAlpha: false);
        var mix3 = colour1.Mix(colour2, ColourSpace.Hsb, premultiplyAlpha: false);
        var mix4 = colour2.Mix(colour1, ColourSpace.Hsb, premultiplyAlpha: false);
        
        Assert.That(mix1.Configuration, Is.EqualTo(colour1.Configuration));
        Assert.That(mix2.Configuration, Is.EqualTo(colour2.Configuration));
        Assert.That(mix3.Configuration, Is.EqualTo(colour1.Configuration));
        Assert.That(mix4.Configuration, Is.EqualTo(colour2.Configuration));
    }
}