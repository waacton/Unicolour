using SixLabors.ImageSharp;

namespace Wacton.Unicolour.Example.Gradients;

public static class VisionDeficiency
{
    private const int Width = 1000;
    private const int RowHeight = 100;

    private static readonly Unicolour Hue0 = new(ColourSpace.Hsb, 0, 0.666, 1);
    private static readonly Unicolour Hue360 = new(ColourSpace.Hsb, 360, 0.666, 1);
    private static readonly Unicolour Black = new("#000000");
    
    private static readonly Cvd?[] AllDeficiencies =
    [
        null,
        Cvd.Protanomaly, Cvd.Protanopia,
        Cvd.Deuteranomaly, Cvd.Deuteranopia,
        Cvd.Tritanomaly, Cvd.Tritanopia,
        Cvd.BlueConeMonochromacy, Cvd.Achromatopsia
    ];
    
    public static void Spectrum()
    {
        var rows = AllDeficiencies
            .Select(cvd => Utils.Draw((cvd?.ToString() ?? "No deficiency", Black), Width, RowHeight, GetColour(cvd)))
            .ToArray();
    
        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-vision-deficiency.png"));
    }

    private static Utils.GetColour GetColour(Cvd? cvd)
    {
        return column =>
        {
            // not interpolating through OKLCH for the spectrum
            // because the uniform luminance results in flat gradient for achromatopsia
            var mixed = Hue0.Mix(Hue360, ColourSpace.Hsb, column / (double)Width, HueSpan.Increasing);
            return cvd.HasValue ? mixed.Simulate(cvd.Value, severity: 0.333) : mixed;
        };
    }
}