using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Wacton.Unicolour;
using Wacton.Unicolour.Datasets;
using Path = System.IO.Path;

var white = new Unicolour(ColourSpace.Rgb, 1, 1, 1);
var black = new Unicolour(ColourSpace.Rgb, 0, 0, 0);

const int fontSize = 48;
FontCollection collection = new();
var fontFamily = collection.Add("Inconsolata-Regular.ttf");
var font = fontFamily.CreateFont(fontSize);

const string outputDirectory = "../../../../Unicolour.Readme/docs/";

Generate("sun.jpg");
Generate("moon.jpg");
return;

void Generate(string filename)
{
    var originalImage = Image.Load<Rgba32>(filename);
    var pixels = new Rgba32[originalImage.Width * originalImage.Height];
    originalImage.CopyPixelDataTo(pixels);

    var unicolours = pixels.Select(pixel => new Unicolour(ColourSpace.Rgb255, pixel.R, pixel.G, pixel.B, pixel.A));
    var luminances = unicolours.Select(colour => colour.RelativeLuminance).ToArray();
    var maxLuminance = luminances.Max();
    var minLuminance = luminances.Min();
    var normalisedLuminances = luminances.Select(luminance => Normalise(luminance, minLuminance, maxLuminance)).ToArray();

    AddLabel(originalImage, "Original", white);

    var outputImages = new List<Image>
    {
        originalImage,
        GetHeatmap(Colourmaps.Turbo, "Turbo"),
        GetHeatmap(Colourmaps.Cubehelix, "Cubehelix"),
        GetHeatmap(Colourmaps.Mako, "Mako"),
        GetHeatmap(Colourmaps.Icefire, "Icefire")
    };

    var result = new Image<Rgba32>(originalImage.Width * outputImages.Count, originalImage.Height);
    for (var i = 0; i < outputImages.Count; i++)
    {
        var image = outputImages[i];
        var location = new Point(originalImage.Width * i, 0);
        result.Mutate(x => x.DrawImage(image, location, 1.0f));
    }

    result.Save(Path.Combine(outputDirectory, $"heatmaps-{Path.GetFileNameWithoutExtension(filename)}.png"));
    return;
    
    // ensures colourmap spans from 0 = darkest pixel to 1 = brightest pixel
    double Normalise(double value, double min, double max) => (value - min) / (max - min);

    Image<Rgba32> GetHeatmap(Colourmap colourmap, string name)
    {
        var mappedColours = normalisedLuminances.Select(colourmap.Map);
        var mappedPixels = mappedColours.Select(AsRgba32).ToArray();
        var heatmap = Image.LoadPixelData<Rgba32>(mappedPixels, originalImage.Width, originalImage.Height);

        var darkestColour = colourmap.Map(0);
        var labelColour = darkestColour.Contrast(white) > darkestColour.Contrast(black) ? white : black;
        AddLabel(heatmap, name, labelColour);
    
        return heatmap;
    }
}

void AddLabel(Image image, string text, Unicolour colour)
{
    var textOptions = new RichTextOptions(font)
    {
        HorizontalAlignment = HorizontalAlignment.Center, 
        VerticalAlignment = VerticalAlignment.Center,
        TextAlignment = TextAlignment.Center,
        Origin = new Vector2(image.Width / 2.0f, image.Height - fontSize)
    };
    
    image.Mutate(context => context.DrawText(textOptions, text, AsColor(colour)));
}

Rgba32 AsRgba32(Unicolour unicolour)
{
    var (r255, g255, b255) = unicolour.Rgb.Byte255.Triplet;
    var a255 = unicolour.Alpha.A255;
    return new((byte)r255, (byte)g255, (byte)b255, (byte)a255);
}

Color AsColor(Unicolour unicolour) => new(AsRgba32(unicolour));
