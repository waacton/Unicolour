using System;

namespace Wacton.Unicolour.Tests.Utils;

public static class ConfigUtils
{
    private static readonly Configuration StandardRgbConfig = new(RgbConfiguration.StandardRgb); 
    private static readonly Configuration DisplayP3Config = new(RgbConfiguration.DisplayP3); 
    private static readonly Configuration Rec2020Config = new(RgbConfiguration.Rec2020);
    
    internal static Configuration GetConfigWithXyzD65(RgbType rgbType)
    {
        return rgbType switch
        {
            RgbType.Standard => StandardRgbConfig,
            RgbType.DisplayP3 => DisplayP3Config,
            RgbType.Rec2020 => Rec2020Config,
            _ => throw new ArgumentOutOfRangeException(nameof(rgbType), rgbType, null)
        };
    }
    
    internal static Configuration GetConfigWithStandardRgb(string illuminantName)
    {
        var illuminant = TestUtils.Illuminants[illuminantName];
        var xyzConfig = new XyzConfiguration(illuminant, Observer.Degree2);
        return new Configuration(RgbConfiguration.StandardRgb, xyzConfig);
    }
    
    public enum RgbType
    {
        Standard,
        DisplayP3,
        Rec2020
    }
}