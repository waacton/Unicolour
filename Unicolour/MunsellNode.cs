namespace Wacton.Unicolour;

internal record Node(double HueNumber, string HueLetter, double Value, int Chroma, double X, double Y, double LuminanceMagnesiumOxide)
{
    // there are 10 letter bands, so each letter band represents 36° of 360° (YR = 0-36°, Y = 36-72°, ..., R = 324-360°)
    // there are 4 numbers in each letter band of 36°, so each number represents 9° of 36° (2.5 = 9°, 5 = 18°, 7.5 = 27°, 10 = 36°)
    internal const double DegreesPerHueLetter = 36;
    internal const double DegreesPerHueNumber = 9;
    
    internal readonly MunsellHue Hue = new(HueNumber, HueLetter);
    internal readonly double HueNumber = HueNumber;
    internal readonly string HueLetter = HueLetter;
    internal readonly double Value = Value;
    internal readonly int Chroma = Chroma;
    internal readonly double X = X;
    internal readonly double Y = Y;
    internal readonly double LuminanceMagnesiumOxide = LuminanceMagnesiumOxide; // actual Y = 0.975 * Ymgo

    internal Chromaticity Point => new(X, Y);
}