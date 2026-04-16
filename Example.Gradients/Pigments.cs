using SixLabors.ImageSharp;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Example.Gradients;

public static class Pigments
{
    private const int Width = 800;
    private const int RowHeight = 100;

    private const int Blocks = 8;
    private const int ColumnsPerBlock = Width / Blocks;
    
    private static readonly Unicolour Black = new("#000000");
    private static readonly Unicolour White = new("#FFFFFF");
    
    private static readonly Pigment WhitePigment = ArtistPaint.TitaniumWhite;
    private static readonly Pigment[] NonWhitePigments =
    [
        ArtistPaint.BoneBlack, 
        ArtistPaint.BismuthVanadateYellow, ArtistPaint.HansaYellowOpaque, ArtistPaint.DiarylideYellow,
        ArtistPaint.CadmiumOrange, ArtistPaint.PyrroleOrange,
        ArtistPaint.CadmiumRedLight, ArtistPaint.PyrroleRed, ArtistPaint.QuinacridoneRed,
        ArtistPaint.QuinacridoneMagenta, ArtistPaint.DioxazinePurple,
        ArtistPaint.PhthaloBlueRedShade, ArtistPaint.PhthaloBlueGreenShade,
        ArtistPaint.UltramarineBlue, ArtistPaint.CobaltBlue, ArtistPaint.CeruleanBlueChromium,
        ArtistPaint.PhthaloGreenBlueShade, ArtistPaint.PhthaloGreenYellowShade
    ];
    
    public static void Mix()
    {
        var rows = NonWhitePigments
            .Select(pigment => Utils.Draw((pigment.Name, GetLabelColour(pigment)), Width, RowHeight, GetMixColour(pigment, WhitePigment)))
            .ToArray();

        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-pigments-mix.png"));
    }

    private static Utils.GetColour GetMixColour(Pigment start, Pigment end)
    {
        return column =>
        {
            var distance = column / (double)(Width - 1);
            return new Unicolour([start, end], [1 - distance, distance]);
        };
    }

    public static void Palette()
    {
        var weights = Enumerable.Range(0, Blocks).Select(i => new[] { 1 - Weight(i), Weight(i) }).ToArray();
        var pigmentToPalette = NonWhitePigments.ToDictionary(
            pigment => pigment, 
            pigment => weights.Select(weight => new Unicolour([pigment, WhitePigment], weight)).ToArray());
    
        var rows = pigmentToPalette
            .Select(kvp =>
            {
                var (pigment, palette) = kvp;
                return Utils.Draw((pigment.Name, GetLabelColour(pigment)), Width, RowHeight, GetPaletteColour(palette));
            })
            .ToArray();
    
        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-pigments-palette.png"));
    }

    private static double Weight(int i) => i / (double)(Blocks - 1);

    private static Utils.GetColour GetPaletteColour(Unicolour[] palette)
    {
        return column => palette[column / ColumnsPerBlock];
    }

    private static Unicolour GetLabelColour(Pigment pigment)
    {
        Pigment[] light = [ArtistPaint.BismuthVanadateYellow, ArtistPaint.HansaYellowOpaque, ArtistPaint.DiarylideYellow];
        return light.Contains(pigment) ? Black : White;
    }
}