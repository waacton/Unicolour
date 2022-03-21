using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Wacton.Unicolour;

var startColour = Unicolour.FromHsb(260, 1.0, 0.33);
var endColour = Unicolour.FromHsb(30, 0.66, 1.0);
var backgroundRgba32 = AsRgba32(Unicolour.FromHex("#404046"));
var textRgba32 = AsRgba32(Unicolour.FromHex("#E8E8FF"));

FontCollection collection = new();
var fontFamily = collection.Add("Inconsolata-Regular.ttf");
var font = fontFamily.CreateFont(32);

var gradientWidth = 1000;
var gradientHeight = 200;

var image = new Image<Rgba32>(gradientWidth, gradientHeight * 3);
image.Mutate(x => x.BackgroundColor(backgroundRgba32));

for (var x = 0; x < gradientWidth; x++)
{
    var distance = x / (double)(gradientWidth - 1);
    var interpolatedRgb = startColour.InterpolateRgb(endColour, distance);
    var interpolatedHsb = startColour.InterpolateHsb(endColour, distance);
    var interpolatedHsl = startColour.InterpolateHsl(endColour, distance);
    SetPixels(x, interpolatedRgb, interpolatedHsb, interpolatedHsl);
}

image.Save("gradients.png");

void SetPixels(int x, Unicolour viaRgb, Unicolour viaHsb, Unicolour viaHsl)
{
    for (var y = 0; y < gradientHeight; y++)
    {
        image[x, y] = AsRgba32(viaRgb);
        image[x, y + 200] = AsRgba32(viaHsb);
        image[x, y + 400] = AsRgba32(viaHsl);
    }

    PointF TextLocation(float targetY) => new(24, targetY + 24);
    image.Mutate(context => context.DrawText("RGB", font, textRgba32, TextLocation(0)));
    image.Mutate(context => context.DrawText("HSB", font, textRgba32, TextLocation(200)));
    image.Mutate(context => context.DrawText("HSL", font, textRgba32, TextLocation(400)));
}

Rgba32 AsRgba32(Unicolour unicolour)
{
    var rgb = unicolour.Rgb;
    return new Rgba32((byte) rgb.R255, (byte) rgb.G255, (byte) rgb.B255);
}

