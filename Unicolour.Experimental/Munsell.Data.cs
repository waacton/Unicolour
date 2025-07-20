namespace Wacton.Unicolour.Experimental;

public partial record Munsell
{
    internal static readonly string[] NodeHueLetters = { "R", "YR", "Y", "GY", "G", "BG", "B", "PB", "P", "RP" };
    internal static readonly double[] NodeValues = { 0.0, 0.2, 0.4, 0.6, 0.8, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    internal static readonly int[] NodeChromas = Enumerable.Range(0, 25).Select(x => x * 2).ToArray();
    
    // there are 10 letter bands, so each letter band represents 36° of 360° (YR = 0-36°, Y = 36-72°, ..., R = 324-360°)
    // there are 4 numbers in each letter band of 36°, so each number represents 9° of 36° (2.5 = 9°, 5 = 18°, 7.5 = 27°, 10 = 36°)
    internal const double DegreesPerHueLetter = 36;
    internal const double DegreesPerHueNumber = 9;

    internal record Node(double HueNumber, string HueLetter, double Value, int Chroma, double X, double Y, double LuminanceMagnesiumOxide)
    {
        internal readonly MunsellHue Hue = new(HueNumber, HueLetter);
        internal readonly double HueNumber = HueNumber;
        internal readonly string HueLetter = HueLetter;
        internal readonly double Value = Value;
        internal readonly int Chroma = Chroma;
        internal readonly double X = X;
        internal readonly double Y = Y;
        internal readonly double LuminanceMagnesiumOxide = LuminanceMagnesiumOxide; // actual Y = 0.975 * Ymgo

        internal readonly double HueDegrees = MunsellHue.ToDegrees(HueNumber, HueLetter);
        internal Chromaticity Point => new(X, Y);
        
        internal bool IsMatch(MunsellHue hue, double value, double chroma)
        {
            return hue.Number == Hue.Number && hue.Letter == Hue.Letter && value == Value && chroma == Chroma;
        }

        internal bool IsMatch(double hueNumber, string hueLetter, double value, double chroma)
        {
            return hueNumber == Hue.Number && hueLetter == Hue.Letter && value == Value && chroma == Chroma;
        }

        public override string ToString() => $"{HueNumber}{HueLetter} {Value}/{Chroma} = ({X}, {Y})";
    }
}