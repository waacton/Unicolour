namespace Wacton.Unicolour;

// https://www.inf.ufrgs.br/~oliveira/pubs_files/CVD_Simulation/CVD_Simulation.html
// only using full severity matrices to simulate extreme cases
internal static class VisionDeficiency
{
    private static readonly Matrix Protanomaly = new(new[,]
    {
        { +0.152286, +1.052583, -0.204868 },
        { +0.114503, +0.786281, +0.099216 },
        { -0.003882, -0.048116, +1.051998 }
    });

    private static readonly Matrix Deuteranomaly = new(new[,]
    {
        { +0.367322, +0.860646, -0.227968 },
        { +0.280085, +0.672501, +0.047413 },
        { -0.011820, +0.042940, +0.968881 }
    });

    private static readonly Matrix Tritanomaly = new(new[,]
    {
        { +1.255528, -0.076749, -0.178779 },
        { -0.078411, +0.930809, +0.147602 },
        { +0.004733, +0.691367, +0.303900 }
    });

    // TODO: support standalone Linear RGB? would simplify this a little
    private static Unicolour SimulateCvd(Rgb rgb, Configuration config, Matrix cvdMatrix)
    {
        // since simulated RGB-Linear often results in values outwith 0 - 1, seems unnecessary to use constrained inputs
        var rgbLinearMatrix = Matrix.FromTriplet(rgb.Linear.Triplet);
        var simulatedRgbLinearMatrix = cvdMatrix.Multiply(rgbLinearMatrix);
        var simulatedRgbMatrix = simulatedRgbLinearMatrix.Select(config.Rgb.CompandFromLinear);
        return Unicolour.FromRgb(config, simulatedRgbMatrix.ToTriplet().Tuple);
    }
    
    internal static Unicolour SimulateProtanopia(Rgb rgb, Configuration config) => SimulateCvd(rgb, config, Protanomaly);
    internal static Unicolour SimulateDeuteranopia(Rgb rgb, Configuration config) => SimulateCvd(rgb, config, Deuteranomaly);
    internal static Unicolour SimulateTritanopia(Rgb rgb, Configuration config) => SimulateCvd(rgb, config, Tritanomaly);
    internal static Unicolour SimulateAchromatopsia(double luminance, Configuration config)
    {
        // luminance is based on Linear RGB, so needs to be companded back into chosen RGB space
        var rgbLuminance = config.Rgb.CompandFromLinear(luminance);
        return Unicolour.FromRgb(config, rgbLuminance, rgbLuminance, rgbLuminance);
    }
}