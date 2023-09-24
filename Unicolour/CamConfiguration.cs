namespace Wacton.Unicolour;

// NOTE: "discounting" parameter which I've noticed in other implementations
// is not a parameter in the CAM papers (there's probably some other paper I've missed 🤷)
// therefore D is always calculated
public class CamConfiguration
{
    public WhitePoint WhitePoint { get; }
    public double AdaptingLuminance { get; } // [L_A] Luminance of adapting field (brightness of the room where the colour is being viewed)
    public double BackgroundLuminance { get; } // [Y_b] Luminance of background (brightness of the area surrounding the colour)
    
    internal readonly Surround Surround; // 0 = dark (movie theatre), 1 = dim (bright TV in dim room), 2 = average (surface colours)
    internal double F => Surround switch
    {
        Surround.Dark => 0.8,
        Surround.Dim => 0.9,
        Surround.Average => 1.0
    };
    
    internal double C => Surround switch
    {
        Surround.Dark => 0.525,
        Surround.Dim => 0.59,
        Surround.Average => 0.69
    };

    internal double Nc => Surround switch
    {
        Surround.Dark => 0.8,
        Surround.Dim => 0.9,
        Surround.Average => 1.0
    };

    /*
     * hard to find guidance for default CAM settings; this is based on data in "Usage guidelines for CIECAM97s" (Moroney, 2000)
     * - sRGB standard ambient illumination level of 64 lux ~= 4
     * - La = E * R / PI / 5 where E = lux & R = 1 --> 64 / PI / 5
     */
    public static readonly CamConfiguration StandardRgb = new(WhitePoint.From(Illuminant.D65), LuxToLuminance(64), 20, Surround.Average);
    internal static double LuxToLuminance(double lux) => lux / Math.PI / 5.0;
    
    public CamConfiguration(WhitePoint whitePoint, double adaptingLuminance, double backgroundLuminance, Surround surround)
    {
        WhitePoint = whitePoint;
        AdaptingLuminance = adaptingLuminance;
        BackgroundLuminance = backgroundLuminance;
        Surround = surround;
    }
    
    public override string ToString() => $"CAM {AdaptingLuminance:f0} {BackgroundLuminance:f0} {Surround}";
}

public enum Surround
{
    Dark = 0,
    Dim = 1,
    Average = 2
}