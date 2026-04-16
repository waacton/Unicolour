using SixLabors.ImageSharp;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Example.Gradients;

public static class Colourmaps
{
    private const int Width = 1000;
    private const int RowHeight = 100;
    
    private const int Blocks = 10;
    private const int ColumnsPerBlock = Width / Blocks;
    
    private static readonly Unicolour Black = new("#000000");
    private static readonly Unicolour White = new("#FFFFFF");

    private static readonly Colourmap[] AllColourmaps =
    [
        Datasets.Colourmaps.Viridis, Datasets.Colourmaps.Plasma, Datasets.Colourmaps.Inferno, Datasets.Colourmaps.Magma, Datasets.Colourmaps.Cividis,
        Datasets.Colourmaps.Mako, Datasets.Colourmaps.Rocket, Datasets.Colourmaps.Crest, Datasets.Colourmaps.Flare,
        Datasets.Colourmaps.Vlag, Datasets.Colourmaps.Icefire, Datasets.Colourmaps.Twilight, Datasets.Colourmaps.TwilightShifted,
        Datasets.Colourmaps.Turbo, Datasets.Colourmaps.Cubehelix
    ];
    
    public static void Map()
    {
        var rows = AllColourmaps
            .Select(map => Utils.Draw((map.ToString()!, GetLabelColour(map)), Width, RowHeight, GetMapColour(map)))
            .ToArray();

        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-maps.png"));
    }

    private static Utils.GetColour GetMapColour(Colourmap colourmap)
    {
        return column => colourmap.Map(column / (double)(Width - 1));
    }

    public static void Palette()
    {
        var colourmapToPalette = AllColourmaps.ToDictionary(
            map => map, 
            map => map.Palette(Blocks).ToArray());
        
        var rows = colourmapToPalette
            .Select(kvp =>
            {
                var (map, palette) = kvp;
                return Utils.Draw((map.ToString()!, GetLabelColour(map)), Width, RowHeight, GetPaletteColour(palette));
            })
            .ToArray();

        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-maps-palette.png"));
    }

    private static Utils.GetColour GetPaletteColour(Unicolour[] palette)
    {
        return column => palette[column / ColumnsPerBlock];
    }

    private static Unicolour GetLabelColour(Colourmap colourmap)
    {
        Colourmap[] light = [Datasets.Colourmaps.Crest, Datasets.Colourmaps.Flare, Datasets.Colourmaps.Vlag, Datasets.Colourmaps.Icefire, Datasets.Colourmaps.Twilight];
        return light.Contains(colourmap) ? Black : White;
    }
}