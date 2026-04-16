using SixLabors.ImageSharp;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Experimental;

namespace Wacton.Unicolour.Example.Gradients;

public static class Harmonies
{
    private const int Width = 1200;
    private const int RowHeight = 120;
    
    private static readonly Harmony[] AllHarmonies =
    [
        Harmony.MonochromaticTint,
        Harmony.MonochromaticShade,
        Harmony.MonochromaticTone,
        Harmony.Monochromatic,
        Harmony.Analogous,
        Harmony.Complementary,
        Harmony.SplitComplementary,
        Harmony.Triadic,
        Harmony.TetradicRectangle,
        Harmony.TetradicSquare
    ];
    
    private static readonly Unicolour White = new("#FFFFFF");
    
    public static void Pigment()
    {
        var colourWheel = ColourWheel.From(
            red: ArtistPaint.QuinacridoneRed,
            yellow: ArtistPaint.BismuthVanadateYellow,
            blue: ArtistPaint.CeruleanBlueChromium,
            white: ArtistPaint.TitaniumWhite,
            black: ArtistPaint.BoneBlack
        );

        const int tealHue = 238;
        var palettes = AllHarmonies.ToDictionary(harmony => harmony, harmony => colourWheel.Harmony(tealHue, harmony));
        
        var rows = AllHarmonies
            .Select(harmony => Utils.Draw((harmony.ToString(), White), Width, RowHeight, GetColour(palettes[harmony])))
            .ToArray();
    
        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-harmonies-pigment.png"));
    }
    
    public static void Hue()
    {
        var teal = new Unicolour("005E72"); // matches pigment wheel @ 238°
        var colourWheel = ColourWheel.From(ColourSpace.Hsb, teal);
        
        // matches behaviour of #005E72 seen on https://www.canva.com/colors/color-wheel/ (which uses RGB / HSB space)
        var palettes = AllHarmonies.ToDictionary(harmony => harmony, harmony => colourWheel.Harmony(teal.Hsb.H, harmony));
        
        var rows = AllHarmonies
            .Select(harmony => Utils.Draw((harmony.ToString(), White), Width, RowHeight, GetColour(palettes[harmony])))
            .ToArray();
    
        var image = Utils.DrawRows(rows, Width, RowHeight);
        image.Save(Utils.GetOutputPath("gradient-harmonies-hue.png"));
    }
    
    private static Utils.GetColour GetColour(Unicolour[] palette)
    {
        return column =>
        {
            var columnsPerBlock = Width / palette.Length;
            var block = column / columnsPerBlock;
            return palette[block];
        };
    }
}