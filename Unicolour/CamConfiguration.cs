﻿namespace Wacton.Unicolour;

// NOTE: "discounting" parameter which I've noticed in other implementations
// is not a parameter in the CAM papers (there's probably some other paper I've missed 🤷)
// therefore D is always calculated
public class CamConfiguration
{
    /*
     * hard to find guidance for default CAM settings; this is based on data in "Usage guidelines for CIECAM97s" (Moroney, 2000)
     * - sRGB standard ambient illumination level of 64 lux ~= 4
     * - La = E * R / PI / 5 where E = lux & R = 1 --> 64 / PI / 5
     * ----------
     * I don't know why Google's HCT luminance calculations don't match the above
     * they suggest 200 lux -> ~11.72 luminance, but the formula above gives ~12.73 luminance
     * and they appear to ignore the division by 5 and incorporate XYZ luminance (Y)
     */
    
    // https://www.researchgate.net/publication/220865484_Usage_guidelines_for_CIECAM97s · https://en.wikipedia.org/wiki/SRGB#Viewing_environment
    public static readonly CamConfiguration StandardRgb = new(Illuminant.D65.GetWhitePoint(Observer.Degree2), LuxToLuminance(64), 20, Surround.Average, "sRGB");
    
    // https://github.com/material-foundation/material-color-utilities/blob/624be32cc1a57cb803262150a9ad58f1a27c1b07/typescript/hct/viewing_conditions.ts#L60
    public static readonly CamConfiguration Hct = new(Illuminant.D65.GetWhitePoint(Observer.Degree2), LuxToLuminance(200) * 5 * DefaultHctY(), DefaultHctY() * 100, Surround.Average, "HCT");
    
    internal static double LuxToLuminance(double lux) => lux / Math.PI / 5.0;
    private const double DefaultHctLightness = 50; // just for HCT, use specific XYZ configuration
    private static double DefaultHctY() => Lab.ToXyz(new Lab(DefaultHctLightness, 0, 0), XyzConfiguration.D65).Y;
    
    public WhitePoint WhitePoint { get; }
    public double AdaptingLuminance { get; } // [L_A] Luminance of adapting field (brightness of the room where the colour is being viewed)
    public double BackgroundLuminance { get; } // [Y_b] Luminance of background (brightness of the area surrounding the colour)
    public Surround Surround { get; } // 0 = dark (movie theatre), 1 = dim (bright TV in dim room), 2 = average (surface colours)
    public string Name { get; }
    
    internal double F => Surround switch
    {
        Surround.Dark => 0.8,
        Surround.Dim => 0.9,
        Surround.Average => 1.0,
        _ => throw new ArgumentOutOfRangeException()
    };
    
    internal double C => Surround switch
    {
        Surround.Dark => 0.525,
        Surround.Dim => 0.59,
        Surround.Average => 0.69,
        _ => throw new ArgumentOutOfRangeException()
    };

    internal double Nc => Surround switch
    {
        Surround.Dark => 0.8,
        Surround.Dim => 0.9,
        Surround.Average => 1.0,
        _ => throw new ArgumentOutOfRangeException()
    };

    public CamConfiguration(WhitePoint whitePoint, 
        double adaptingLuminance, double backgroundLuminance,
        Surround surround, string name = Utils.Unnamed)
    {
        WhitePoint = whitePoint;
        AdaptingLuminance = adaptingLuminance;
        BackgroundLuminance = backgroundLuminance;
        Surround = surround;
        Name = name;
    }

    public override string ToString() => $"{Name} · {AdaptingLuminance:f0}, {BackgroundLuminance:f0}, {Surround}";
}

public enum Surround
{
    Dark = 0,
    Dim = 1,
    Average = 2
}