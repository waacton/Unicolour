using static Wacton.Unicolour.Experimental.MunsellUtils;

namespace Wacton.Unicolour.Experimental;

// TODO: handle grey ("N" hue e.g. N 5/ ... or 0 chroma e.g. 10YR 5/0)
// TODO: clamp hue between 0 - 10, clamp value to 10, handle extreme chroma values
public partial record Munsell
{
    // TODO: expose a constrained hue, modulo'd to 360, and use in conversions
    internal MunsellHue Hue { get; }
    public (double number, string letter) H => (Hue.Number, Hue.Letter);
    public double V { get; }
    public double C { get; }

    private readonly Lazy<MunsellBounds> bounds;
    internal MunsellBounds Bounds => bounds.Value;
    
    public bool IsGreyscale => V <= 0.0 || C <= 0.0; // TODO: what about V > 1? probably
    
    // TODO: remove when Munsell becomes a subclass of ColourRepresentation
    public ColourTriplet Triplet => new(Hue.Degrees, V, C, 0);

    public Munsell(double h1, string h2, double v, double c) : this(new MunsellHue(h1, h2), v, c) { }
    internal Munsell(double h, double v, double c) : this(new MunsellHue(h), v, c) { }
    private Munsell(MunsellHue h, double v, double c)
    {
        Hue = h;
        V = v;
        C = c;
        bounds = new Lazy<MunsellBounds>(GetBounds);
    }

    public Munsell(double greyNumber)
    {
        // HueDegrees = 0;
        // Value = null;
        // Chroma = null;
    }
    
    public static Munsell FromXyy(Xyy xyy)
    {
        var v = MunsellFuncs.GetValue(xyy.Luminance);
        var (h, c) = GetHueAndChroma(xyy.Chromaticity, v);
        return new Munsell(h, v, c);
    }

    private MunsellBounds GetBounds()
    {
        // these are the naive bounds, and will be adjusted if not available in the dataset
        // e.g. the chroma must exist for both hue/value/lowerChroma and hue/value/upperChroma to be used for interpolation
        //      if it doesn't, a different chroma that exists for both will be used
        var hueIntervals = MunsellFuncs.ToIntervals(Hue.Degrees, DegreesPerHueNumber);
        var (lowerH, upperH) = (new MunsellHue(hueIntervals.lower), new MunsellHue(hueIntervals.upper));
        var (lowerV, upperV) = MunsellFuncs.ToIntervals(V, V < 1.0 ? 0.2 : 1);
        var (lowerC, upperC) = MunsellFuncs.ToIntervals(C, 2);
        return new MunsellBounds(lowerH, upperH, lowerV, upperV, lowerC, upperC);
    }
    
    internal record MunsellHue
    {
        internal double Number { get; }
        internal string Letter { get; }
        internal double Degrees { get; }

        internal MunsellHue(double number, string letter)
        {
            Degrees = ToDegrees(number, letter);
            Number = number;
            Letter = letter;
        }

        internal MunsellHue(double degrees)
        {
            Degrees = degrees.Modulo(360);
            var (number, letter) = FromDegrees(Degrees);
            Number = number;
            Letter = letter;
        }
        
        // this is only used by the table of radially interpolated hue segments
        // which are defined anti-clockwise as red -> blue is depicted on a chromaticity diagram
        // in terms of traditional hue degrees, this means the end is defined first
        internal bool IsBetween((double number, string letter) end, (double number, string letter) start)
        {
            return IsBetween(new MunsellHue(end.number, end.letter), new MunsellHue(start.number, start.letter));
        }
        
        private bool IsBetween(MunsellHue end, MunsellHue start)
        {
            var adapted = Wacton.Unicolour.Hue.Unwrap(end.Degrees, start.Degrees);
            var min = Math.Min(adapted.start, adapted.end);
            var max = Math.Max(adapted.start, adapted.end);
            return Degrees >= min && Degrees <= max || Degrees + 360 >= min && Degrees + 360 <= max;
        }
        
        internal static double ToDegrees(double hueNumber, string hueLetter)
        {
            var bandIndex = Array.IndexOf(NodeHueLetters, hueLetter);

            var minDegrees = bandIndex * DegreesPerHueLetter;
            var maxDegrees = (bandIndex + 1) * DegreesPerHueLetter;
            var distance = hueNumber / 10.0; // maps 0 - 10 to 0 - 1
            var baseDegrees = Interpolation.Linear(minDegrees, maxDegrees, distance);
            var degrees = baseDegrees - 2 * DegreesPerHueNumber; // shifts degrees so 5R is 0 instead of 0R / 10RP
            return baseDegrees.Modulo(360);
        }

        internal static (double number, string letter) FromDegrees(double degrees)
        {
            var baseDegrees = degrees + 2 * DegreesPerHueNumber; // shifts degrees so 0R is 0 instead of 5R
            var bandLocation = degrees.Modulo(360) / DegreesPerHueLetter;
            var bandIndex = (int)Math.Truncate(bandLocation);
            var hueLetter = NodeHueLetters[bandIndex];
            var hueNumber = (bandLocation - bandIndex) * 10;
            if (hueNumber != 0) return (hueNumber, hueLetter);
        
            bandIndex = bandIndex == 0 ? NodeHueLetters.Length - 1 : bandIndex - 1;
            hueLetter = NodeHueLetters[bandIndex];
            hueNumber = 10;
            return (hueNumber, hueLetter);
        }
        
        public override string ToString() => $"{Number}{Letter} ({Degrees}°)";
    }
    
    // only for potential debugging or diagnostics
    internal MunsellFuncs.XyyToMunsellSearchResult? XyyToMunsellSearchResult;
    
    public override string ToString() => IsGreyscale ? $"N{V:0.##}" : $"{H.number:0.##}{H.letter} {V:0.##}/{C:0.##}";
}