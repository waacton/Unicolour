using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using Wacton.Unicolour;

const int gradientWidth = 800;
const int gradientHeight = 100;
const bool constrainUndisplayableColours = true; // if false, undisplayable colours will render as transparent

FontCollection collection = new();
var fontFamily = collection.Add("Inconsolata-Regular.ttf");
var font = fontFamily.CreateFont(24);
var textRgba32 = AsRgba32(Unicolour.FromHex("#E8E8FF"));

var labels = new List<string>
{
    "RGB", "HSB", "HSL", "HWB", "XYZ", "xyY", "LAB", "LCHab", "LUV", "LCHuv", "HSLuv", "HPLuv", 
    "ICtCp", "JzAzBz", "JzCzHz", "OKLAB", "OKLCH", "CAM02", "CAM16"
};

var purple = Unicolour.FromHsb(260, 1.0, 0.33);
var orange = Unicolour.FromHsb(30, 0.66, 1.0);
var pink = Unicolour.FromHex("#FF1493");
var cyan = Unicolour.FromRgb255(0, 255, 255);
var black = Unicolour.FromRgb(0, 0, 0);
var green = Unicolour.FromRgb(0, 1, 0);

var image = new Image<Rgba32>(gradientWidth * 3, gradientHeight * labels.Count);
Draw(purple, orange, 0);
Draw(pink, cyan, 1);
Draw(black, green, 2);
image.Save("gradients.png");

void Draw(Unicolour start, Unicolour end, int column)
{
    for (var pixelIndex = 0; pixelIndex < gradientWidth; pixelIndex++)
    {
        var distance = pixelIndex / (double)(gradientWidth - 1);
        var unicolours = new List<Unicolour>
        {
            start.InterpolateRgb(end, distance),
            start.InterpolateHsb(end, distance),
            start.InterpolateHsl(end, distance),
            start.InterpolateHwb(end, distance),
            start.InterpolateXyz(end, distance),
            start.InterpolateXyy(end, distance),
            start.InterpolateLab(end, distance),
            start.InterpolateLchab(end, distance),
            start.InterpolateLuv(end, distance),
            start.InterpolateLchuv(end, distance),
            start.InterpolateHsluv(end, distance),
            start.InterpolateHpluv(end, distance),
            start.InterpolateIctcp(end, distance),
            start.InterpolateJzazbz(end, distance),
            start.InterpolateJzczhz(end, distance),
            start.InterpolateOklab(end, distance),
            start.InterpolateOklch(end, distance),
            start.InterpolateCam02(end, distance),
            start.InterpolateCam16(end, distance)
        };
    
        SetPixels(column, pixelIndex, unicolours);
    }
    
    for (var i = 0; i < labels.Count; i++)
    {
        var label = labels[i];
        var textLocation = TextLocation(column, i);
        image.Mutate(context => context.DrawText(label, font, textRgba32, textLocation));
    }
}

void SetPixels(int column, int pixelIndex, List<Unicolour> unicolours)
{
    for (var y = 0; y < gradientHeight; y++)
    {
        for (var i = 0; i < unicolours.Count; i++)
        {
            var x = gradientWidth * column + pixelIndex;
            image[x, y + gradientHeight * i] = AsRgba32(unicolours[i]);
        }
    }
}

PointF TextLocation(float column, float row) => new(gradientWidth * column + 16, gradientHeight * row + 16);

Rgba32 AsRgba32(Unicolour unicolour)
{
    var (r, g, b) = unicolour.Rgb.Byte255.ConstrainedTriplet;
    var a = constrainUndisplayableColours || unicolour.IsDisplayable ? 255 : 0;
    return new Rgba32((byte) r, (byte) g, (byte) b, (byte) a);
}