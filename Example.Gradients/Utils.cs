namespace Wacton.Unicolour.Example.Gradients;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

internal static class Utils
{
    private const int FontSize = 24;
    private static readonly FontCollection Collection = new();
    private static readonly FontFamily FontFamily = Collection.Add("Inconsolata-Regular.ttf");
    private static readonly Font Font = FontFamily.CreateFont(FontSize);
    
    private const bool RenderOutOfGamutAsTransparent = false;

    internal delegate Unicolour Mix(Unicolour start, Unicolour end, double amount);
    internal delegate Unicolour GetColour(int index);
    
    internal static Unicolour GetMixedColour(Unicolour[] colourPoints, int column, int width, Mix mix)
    {
        var segmentCount = colourPoints.Length - 1;
        var segmentWidth = width / segmentCount;
        var segmentIndex = column / segmentWidth;
        var segmentDistance = column % segmentWidth / (double) segmentWidth;
        var segmentStartColour = colourPoints[segmentIndex];
        var segmentEndColour = colourPoints[segmentIndex + 1];
        return mix(segmentStartColour, segmentEndColour, segmentDistance);
    }
    
    internal static Image<Rgba32> Draw((string text, Unicolour colour) label, int width, int height, GetColour getColour)
    {
        var image = new Image<Rgba32>(width, height);

        for (var column = 0; column < width; column++)
        {
            var colour = getColour(column);
            SetColumnPixels(image, column, height, colour);
        }
        
        SetLabel(image, label.text, label.colour);
        return image;
    }
    
    private static void SetColumnPixels(Image<Rgba32> image, int column, int height, Unicolour colour)
    {
        for (var row = 0; row < height; row++)
        {
            image[column, row] = AsRgba32(colour);
        }
    }
    
    private static void SetLabel(Image<Rgba32> image, string text, Unicolour colour)
    {
        var row = image.Height / 2.0 - FontSize / 2.0;
        var textLocation = new PointF(16, (int)row);
        image.Mutate(context => context.DrawText(text, Font, AsRgba32(colour), textLocation));
    }

    internal static Image<Rgba32> DrawRows(List<Image<Rgba32>> rows, int rowWidth, int rowHeight)
    {
        var rowIndex = 0;
        var image = new Image<Rgba32>(rowWidth, rowHeight * rows.Count);
        image.Mutate(context =>
        {
            foreach (var gradient in rows)
            {
                context.DrawImage(gradient, Location(rowIndex++), 1f);
            }
        });

        return image;

        Point Location(int index) => new(0, rowHeight * index);
    }
    
    internal static Image<Rgba32> DrawColumns(List<Image<Rgba32>> columns, int columnWidth, int columnHeight)
    {
        var columnIndex = 0;
        var image = new Image<Rgba32>(columnWidth * columns.Count, columnHeight);
        image.Mutate(context =>
        {
            foreach (var gradient in columns)
            {
                context.DrawImage(gradient, Location(columnIndex++), 1f);
            }
        });

        return image;

        Point Location(int index) => new(columnWidth * index, 0);
    }

    private static Rgba32 AsRgba32(Unicolour unicolour)
    {
        var (r, g, b) = unicolour.Rgb.Byte255.ConstrainedTriplet;
        var alpha = unicolour.Alpha.A255;
        var a = unicolour.IsInDisplayGamut || !RenderOutOfGamutAsTransparent ? alpha : 0;
        return new Rgba32((byte) r, (byte) g, (byte) b, (byte) a);
    }
}