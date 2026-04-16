using SixLabors.ImageSharp;

namespace Wacton.Unicolour.Example.Gradients;

public static class Simple
{
    private const int Width = 800;
    private const int RowHeight = 100;
    private const int Blocks = 8;
    private const int ColumnsPerBlock = Width / Blocks;
    
    private static readonly Unicolour DeepPink = new("FF1493");     // same as Css.DeepPink
    private static readonly Unicolour Aquamarine = new("7FFFD4");   // same as Css.Aquamarine
    
    public static void Mix()
    {
        var row = Utils.Draw(Width, RowHeight, GetMixedColour);
        var image = Utils.DrawRows([row], Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-simple-mixing.png"));
        return;

        // calculate each column by mixing on demand at every distance between start and end
        // NOTE: can achieve same result by doing the palette approach below with 1024 colours
        Unicolour GetMixedColour(int column) => DeepPink.Mix(Aquamarine, ColourSpace.Oklch, column / (double)Width, HueSpan.Decreasing);
    }
    
    public static void Palette()
    {
        var palette = DeepPink.Palette(Aquamarine, ColourSpace.Oklch, Blocks, HueSpan.Decreasing).ToArray();
        var row = Utils.Draw(Width, RowHeight, GetPaletteColour);
        var image = Utils.DrawRows([row], Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-simple-palette.png"));
        return;
        
        // calculate each column according to a pre-generated palette between start and end
        Unicolour GetPaletteColour(int column) => palette[column / ColumnsPerBlock];
    }
}