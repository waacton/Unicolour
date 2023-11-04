namespace Wacton.Unicolour.Example;

using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

internal static class Gradient
{
    private const bool RenderOutOfGamutAsTransparent = false;

    internal delegate Unicolour Mix(Unicolour start, Unicolour end, double amount);

    internal static Image<Rgba32> Draw((string text, Unicolour colour) label, int width, int height, 
        Unicolour[] colourPoints, Mix mix)
    {
        var image = new Image<Rgba32>(width, height);

        var sectionCount = colourPoints.Length - 1;
        var sectionWidth = width / sectionCount;

        for (var sectionIndex = 0; sectionIndex < sectionCount; sectionIndex++)
        {
            var startColour = colourPoints[sectionIndex];
            var endColour = colourPoints[sectionIndex + 1];
            SetSectionPixels(image, sectionWidth, sectionIndex, height, startColour, endColour, mix);
        }
        
        SetLabel(image, label.text, label.colour);
        return image;
    }

    private static void SetSectionPixels(Image<Rgba32> image, int sectionWidth, int sectionIndex, int height,
        Unicolour startColour, Unicolour endColour, Mix mix)
    {
        for (var pixelIndex = 0; pixelIndex < sectionWidth; pixelIndex++)
        {
            var distance = pixelIndex / (double)(sectionWidth - 1);
            var colour = mix(startColour, endColour, distance);
            var column = sectionWidth * sectionIndex + pixelIndex;
            SetImageColumnPixels(image, column, height, colour);
        }
    }

    private static void SetImageColumnPixels(Image<Rgba32> image, int column, int height, Unicolour colour)
    {
        for (var row = 0; row < height; row++)
        {
            image[column, row] = AsRgba32(colour);
        }
    }
    
    private static void SetLabel(Image<Rgba32> image, string text, Unicolour colour)
    {
        FontCollection collection = new();
        var fontFamily = collection.Add("Inconsolata-Regular.ttf");
        var font = fontFamily.CreateFont(24);

        var textLocation = new PointF(16, 16);
        image.Mutate(context => context.DrawText(text, font, AsRgba32(colour), textLocation));
    }

    private static Rgba32 AsRgba32(Unicolour unicolour)
    {
        var (r, g, b) = unicolour.Rgb.Byte255.ConstrainedTriplet;
        var alpha = unicolour.Alpha.A255;
        var a = unicolour.IsInDisplayGamut || !RenderOutOfGamutAsTransparent ? alpha : 0;
        return new Rgba32((byte) r, (byte) g, (byte) b, (byte) a);
    }
}