using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Path = System.IO.Path;

namespace Wacton.Unicolour.Example.Gradients;

internal static class Utils
{
    private const string OutputDirectory = "../../../../Unicolour.Readme/docs/";
    
    private const int FontSize = 24;
    private static readonly FontCollection Collection = new();
    private static readonly FontFamily FontFamily = Collection.Add("Inconsolata-Regular.ttf");
    private static readonly Font Font = FontFamily.CreateFont(FontSize);
    
    private const bool RenderOutOfGamutAsTransparent = false;

    internal delegate Unicolour Mix(Unicolour start, Unicolour end, double amount);
    internal delegate Unicolour GetColour(int index);
    
    internal static Unicolour GetColourFromStops(Unicolour[] stops, int column, int width, Mix mix)
    {
        var segmentCount = stops.Length - 1;
        var segmentWidth = width / segmentCount;
        var segmentIndex = column / segmentWidth;
        var segmentDistance = column % segmentWidth / (double) segmentWidth;
        var segmentStartColour = stops[segmentIndex];
        var segmentEndColour = stops[segmentIndex + 1];
        return mix(segmentStartColour, segmentEndColour, segmentDistance);
    }
    
    internal static Image<Rgba32> Draw(int width, int height, GetColour getColour)
    {
        var image = new Image<Rgba32>(width, height);

        for (var column = 0; column < width; column++)
        {
            var colour = getColour(column);
            SetColumnPixels(image, column, height, colour);
        }
        
        return image;
    }
    
    internal static Image<Rgba32> Draw((string text, Unicolour colour) label, int width, int height, GetColour getColour)
    {
        var image = Draw(width, height, getColour);
        SetLabel(image, label.text, label.colour);
        return image;
    }
    
    private static void SetColumnPixels(Image<Rgba32> image, int column, int height, Unicolour colour)
    {
        var rgba32 = AsRgba32(colour);
        for (var row = 0; row < height; row++)
        {
            image[column, row] = rgba32;
        }
    }
    
    private static void SetLabel(Image<Rgba32> image, string text, Unicolour colour)
    {
        var row = image.Height / 2.0 - FontSize / 2.0;
        var textLocation = new PointF(16, (int)row);
        image.Mutate(context => context.DrawText(text, Font, AsRgba32(colour), textLocation));
    }

    internal static Image<Rgba32> DrawRows(Image<Rgba32>[] rows, int rowWidth, int rowHeight)
    {
        var rowIndex = 0;
        var image = new Image<Rgba32>(rowWidth, rowHeight * rows.Length);
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
    
    internal static void DrawArcs(Image<Rgba32> image, int radius, (int angle, Unicolour colour, int distance, int thickness)[] segmentData)
    {
        foreach (var segment in segmentData)
        {
            DrawArc(image, radius, segment, addOverlap: true);
        }
        
        DrawArc(image, radius, segmentData[0], addOverlap: false);
    }

    private static void DrawArc(Image<Rgba32> image, int radius, (int angle, Unicolour colour, int distance, int thickness) segment, bool addOverlap)
    {
        var builder = new PathBuilder();

        builder.AddArc(
            center: new(image.Width / 2f, image.Height / 2f),
            radiusX: radius,
            radiusY: radius,
            rotation: -90f,
            startAngle: segment.angle,
            sweepAngle: segment.distance + (addOverlap ? 0.5f : 0f));

        var path = builder.Build();
        
        image.Mutate(ctx => ctx.Draw(AsRgba32(segment.colour), segment.thickness, path));
    }

    private static Rgba32 AsRgba32(Unicolour colour)
    {
        var (r, g, b) = colour.MapToRgbGamut(GamutMap.RgbClipping).Rgb.Byte255;
        var alpha = colour.Alpha.A255;
        var a = colour.IsInRgbGamut || !RenderOutOfGamutAsTransparent ? alpha : 0;
        return new Rgba32((byte) r, (byte) g, (byte) b, (byte) a);
    }

    internal static string GetOutputPath(string filename) => Path.Combine(OutputDirectory, filename);
}