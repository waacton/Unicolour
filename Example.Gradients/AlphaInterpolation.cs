using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Example.Gradients;

public static class AlphaInterpolation
{
    private const int Width = 1000;
    private const int RowHeight = 100;

    private static readonly Unicolour Black = new("#000000");
    
    public static void Premultiplied()
    {
        Unicolour[] colourStops = [Css.Red, Css.Transparent, Css.Blue];
        Image<Rgba32>[] rows =
        [
            Utils.Draw(("With premultiplied alpha", Black), Width, RowHeight, GetColour(true, colourStops)),
            Utils.Draw(("Without premultiplied alpha", Black), Width, RowHeight, GetColour(false, colourStops))
        ];
    
        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-alpha-interpolation.png"));
    }

    private static Utils.GetColour GetColour(bool premultiplyAlpha, Unicolour[] colourStops)
    {
        Utils.Mix mix = (start, end, amount) => start.Mix(end, ColourSpace.Rgb, amount, premultiplyAlpha: premultiplyAlpha);
        return column => Utils.GetColourFromStops(colourStops, column, Width, mix);
    }
}