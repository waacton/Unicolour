using static Wacton.Unicolour.Experimental.MunsellUtils;

namespace Wacton.Unicolour.Experimental;

// TODO: handle grey ("N" hue e.g. N 5/ ... or 0 chroma e.g. 10YR 5/0)
// TODO: clamp hue between 0 - 10, clamp value to 10, handle extreme chroma values
// TODO: handle boundary cases
//       - input xy is beyond all x-values in the dataset (in either direction; need to extrapolate horizontals from 2x closest points in other direction)
//       - input xy is beyond all y-values in the dataset (in either direction; need to extrapolate verticals from 2x closest points in other direction)
//       - input xy is beyond all x- and y-values in the dataset (form 2 segments from the 4 points in the same direction (how to choose pairing?) and extrapolate)
public partial record Munsell
{
    internal MunsellHue Hue { get; }
    public (double number, string letter) H => (Hue.Number, Hue.Letter);
    public double V { get; }
    public double C { get; }
    
    // TODO: remove when Munsell becomes a subclass of ColourRepresentation
    public ColourTriplet Triplet => new(Hue.Degrees, V, C, 0);

    public Munsell(double h1, string h2, double v, double c)
    {
        Hue = new MunsellHue(h1, h2);
        V = v;
        C = c;
    }
    
    internal Munsell(double h, double v, double c)
    {
        Hue = new MunsellHue(h);
        V = v;
        C = c;
    }

    public Munsell(double greyNumber)
    {
        // HueDegrees = 0;
        // Value = null;
        // Chroma = null;
    }
    
    /*
     * following ASTM standard practice https://doi.org/10.1520/D1535-14R18
     */

    public static Xyy ToXyy(Munsell munsell)
    {
        var value = munsell.V;
        // if (!value.HasValue) throw new NotImplementedException();
        
        var luminance = GetLuminance(value);
        var (x, y) = GetXy(munsell);
        return new Xyy(x, y, luminance);
    }
    
    public static Munsell FromXyy(Xyy xyy)
    {
        var v = GetValue(xyy.Luminance);
        var (h, c) = GetHueAndChroma(xyy.Chromaticity, v);
        return new Munsell(h, v, c);
    }

    internal static double GetLuminance(double v)
    {
        var y = 1.1914 * v - 0.22533 * Math.Pow(v, 2) + 0.23352 * Math.Pow(v, 3) - 0.020484 * Math.Pow(v, 4) + 0.00081939 * Math.Pow(v, 5);
        return y / 100.0;
    }

    /*
     * the maximum error of the core Y -> V function is 0.0035 (https://doi.org/10.1002/col.5080170308)
     * which isn't bad but compounds during roundtrip conversions and causes algorithm of H and C to interpolate away from the actual V by a small amount
     * however, this error provides a range of potential V from a given Y (e.g. if result V = 5, actual V is between 4.9965 to 5.0035)
     * which can be interpolated using exact Y calculations for a more accurate result (error of 0.000005, obtained from testing)
     * and this process can be repeated for this new max error for even greater accuracy
     * though at a depth of 3 iterations the max error is 5e-15, and further iteration yields no improvement
     */
    internal static readonly double[] IterationDepthError = { 0.0035, 0.000005, 0.00000000005, 0.000000000000005 };
    internal static double GetValue(double y, int iterationDepth = 3)
    {
        if (iterationDepth == 0)
        {
            y *= 100;
            return y <= 0.9
                ? 0.87445 * Math.Pow(y, 0.9967)
                : 2.49268 * Math.Pow(y, 1 / 3.0) - 1.5614 - 0.985 / (Math.Pow(0.1073 * y - 3.084, 2) + 7.54)
                  + 0.0133 / Math.Pow(y, 2.3) + 0.0084 * Math.Sin(4.1 * Math.Pow(y, 1 / 3.0) + 1)
                  + 0.0221 / y * Math.Sin(0.39 * (y - 2))
                  - 0.0037 / (0.44 * y) * Math.Sin(1.28 * (y - 0.53));
        }

        iterationDepth--;
        var vEstimate = GetValue(y, iterationDepth);
        var error = IterationDepthError[iterationDepth];
        var vLower = vEstimate - error;
        var vUpper = vEstimate + error;
        
        var yLower = GetLuminance(vLower);
        var yUpper = GetLuminance(vUpper);
        var distance = (y - yLower) / (yUpper - yLower);
        return Interpolation.Interpolate(vLower, vUpper, distance);
    }
    
    internal record MunsellHue
    {
        internal double Number { get; }
        internal string Letter { get; }
        internal double Degrees { get; }

        internal MunsellHue(double number, string letter)
        {
            Number = number;
            Letter = letter;
            Degrees = ToDegrees(number, letter);
        }

        internal MunsellHue(double degrees)
        {
            var (number, letter) = FromDegrees(degrees);
            Number = number;
            Letter = letter;
            Degrees = degrees;
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
            var adapted = Wacton.Unicolour.Hue.Adapt(end.Degrees, start.Degrees, HueSpan.Shorter);
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
            return Interpolation.Interpolate(minDegrees, maxDegrees, distance);
        }

        internal static (double number, string letter) FromDegrees(double degrees)
        {
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
    
    public override string ToString() => $"{H.number:0.##}{H.letter} {V:0.##}/{C:0.##}";
}