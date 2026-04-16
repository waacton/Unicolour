using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Experimental;

namespace Wacton.Unicolour.Example.Gradients;

public static class ColourWheels
{
    private const int Segments = 12;
    private const int ArcDistance = 360 / Segments;
    private const int ArcThickness = 64;
    private const int ImageSize = ArcThickness * 10;
    private const int ImageCentre = ImageSize / 2;
    private const int InnerRadius = ImageCentre / 2 - 16;
    
    public static void Pigment()
    {
        var colourWheel = ColourWheel.From(
            red: ArtistPaint.QuinacridoneRed,
            yellow: ArtistPaint.BismuthVanadateYellow,
            blue: ArtistPaint.CeruleanBlueChromium,
            white: ArtistPaint.TitaniumWhite,
            black: ArtistPaint.BoneBlack
        );

        var image = Generate(colourWheel);
        image.Save(Utils.GetOutputPath("gradient-wheel-pigment.png"));
    }
    
    public static void Hue()
    {
        var red = new Unicolour("FF0000");
        var colourWheel = ColourWheel.From(ColourSpace.Munsell, red);

        var image = Generate(colourWheel);
        image.Save(Utils.GetOutputPath("gradient-wheel-hue.png"));
    }

    private static Image<Rgba32> Generate(ColourWheel colourWheel)
    {
        var hueData = new (int angle, Unicolour colour, int distance, int thickness)[Segments];
        var tintData = new (int angle, Unicolour colour, int distance, int thickness)[Segments];
        var shadeData = new (int angle, Unicolour colour, int distance, int thickness)[Segments];
        for (var i = 0; i < Segments; i++)
        {
            var hue = i * ArcDistance;
            var angle = hue - ArcDistance / 2 ;
            hueData[i] = (angle, colourWheel.Pure(hue), ArcDistance, ArcThickness);
            tintData[i] = (angle, colourWheel.Tint(hue, 1), ArcDistance, ArcThickness);
            shadeData[i] = (angle, colourWheel.Shade(hue, 1), ArcDistance, ArcThickness);
        }

        var image = new Image<Rgba32>(ImageSize, ImageSize);
        Utils.DrawArcs(image, radius: InnerRadius + ArcThickness * 2, tintData);
        Utils.DrawArcs(image, radius: InnerRadius + ArcThickness, hueData);
        Utils.DrawArcs(image, radius: InnerRadius, shadeData);
        return image;
    }
}