using SixLabors.ImageSharp;

namespace Wacton.Unicolour.Example.Gradients;

public static class SpectralJs
{
    private const int Width = 540;
    private const int RowHeight = 60;

    private const int Blocks = 9;
    private const int ColumnsPerBlock = Width / Blocks;
    
    private static readonly (Unicolour start, Unicolour end)[] ColourPairs =
    [
        // default colours on https://onedayofcrypto.art/
        (new Unicolour("#002185"), new Unicolour("#FCD200")),

        // colours taken from github example https://github.com/rvanwijnen/spectral.js#usage
        (new Unicolour("#005E72"), new Unicolour("#EAD9A7")),
        (new Unicolour("#FF8A3E"), new Unicolour("#FF006D")),
        (new Unicolour("#002185"), new Unicolour("#F0F0F0")),
        (new Unicolour("#DFE800"), new Unicolour("#CC3536"))
    ];

    public static void Mix()
    {
        var rows = ColourPairs
            .Select(pair => Utils.Draw(Width, RowHeight, GetMixColour(pair.start, pair.end)))
            .ToArray();

        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-spectraljs-mix.png"));
    }

    private static Utils.GetColour GetMixColour(Unicolour start, Unicolour end)
    {
        return column =>
        {
            var distance = column / (double)(Width - 1);
            return Experimental.SpectralJs.Mix([start, end], [1 - distance, distance]);
        };
    }

    public static void Palette()
    {
        var palettes = ColourPairs.Select(x => Experimental.SpectralJs.Palette(x.start, x.end, Blocks).ToArray()).ToList();
        var rows = palettes
            .Select(palette => Utils.Draw(Width, RowHeight, GetPaletteColour(palette)))
            .ToArray();
        
        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-spectraljs-palette.png"));
    }
    
    private static Utils.GetColour GetPaletteColour(Unicolour[] palette)
    {
        return column => palette[column / ColumnsPerBlock];
    }
}