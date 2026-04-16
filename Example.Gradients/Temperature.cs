using SixLabors.ImageSharp;

namespace Wacton.Unicolour.Example.Gradients;

public static class Temperature
{
    private const int Width = 1200;
    private const int RowHeight = 120;

    private static readonly Unicolour Black = new("#000000");
    
    public static void Scaled()
    {
        List<Unicolour> scaledColours = [];
        for (var i = 1000; i <= 13000; i += 100)
        {
            var rgbComponents = new Unicolour(i).Rgb.ToArray();
            var max = rgbComponents.Max();
            var scaledRgb = rgbComponents.Select(x => x / max).ToArray();
            var scaledColour = new Unicolour(ColourSpace.Rgb, scaledRgb[0], scaledRgb[1], scaledRgb[2]);
            scaledColours.Add(scaledColour);
        }

        var row = Utils.Draw(("CCT (1,000 K - 13,000 K)", Black), Width, RowHeight, GetColour(scaledColours));
        var image = Utils.DrawRows([row], Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-temperature.png"));
    }

    private static Utils.GetColour GetColour(List<Unicolour> colours)
    {
        Utils.Mix mix = (start, end, amount) => start.Mix(end, ColourSpace.Rgb, amount);
        return column => Utils.GetColourFromStops(colours.ToArray(), column, Width, mix);
    }
}