using static Wacton.Unicolour.Experimental.MunsellUtils;

namespace Wacton.Unicolour.Experimental;

// TODO: double check luminance equations, not particularly accurate roundtrip
// TODO: handle grey ("N" hue e.g. N 5/ ... or 0 chroma e.g. 10YR 5/0)
// TODO: clamp hue between 0 - 10, clamp value to 10, handle extreme chroma values
// TODO: handle boundary cases
//       - input xy is beyond all x-values in the dataset (in either direction; need to extrapolate horizontals from 2x closest points in other direction)
//       - input xy is beyond all y-values in the dataset (in either direction; need to extrapolate verticals from 2x closest points in other direction)
//       - input xy is beyond all x- and y-values in the dataset (form 2 segments from the 4 points in the same direction (how to choose pairing?) and extrapolate)
// TODO: construct using string (accounting for both standard and grey variants, allowing decimals)
public partial record Munsell
{
    public double HueDegrees { get; }
    public (double number, string letter) Hue { get; }
    public double Value { get; }
    public double Chroma { get; }

    public Munsell(double hueNumber, string hueLetter, double value, double chroma) : this(ToDegrees(hueNumber, hueLetter), value, chroma) { }
    public Munsell(double hueDegrees, double value, double chroma)
    {
        HueDegrees = hueDegrees;
        Hue = FromDegrees(HueDegrees);
        Value = value;
        Chroma = chroma;
    }

    public Munsell(double greyNumber)
    {
        HueDegrees = 0;
        // Value = null;
        // Chroma = null;
    }

    public static Xyy ToXyy(Munsell munsell)
    {
        var value = munsell.Value;
        // if (!value.HasValue) throw new NotImplementedException();
        
        var v = value;
        var luminance = GetLuminance(v);
        var (x, y) = GetXy(munsell);
        return new Xyy(x, y, luminance / 100.0);
    }
    
    public static Munsell FromXyy(Xyy xyy)
    {
        var v = GetValue(xyy.Luminance * 100);
        var (h, c) = GetHueAndChroma(xyy.Chromaticity, v);
        return new Munsell(h, v, c);
    }

    // following ASTM standard practice https://doi.org/10.1520/D1535-14R18
    private static double GetLuminance(double v)
    {
        return 1.1914 * v - 0.22533 * Math.Pow(v, 2) + 0.23352 * Math.Pow(v, 3) - 0.020484 * Math.Pow(v, 4) + 0.00081939 * Math.Pow(v, 5);
    }
    
    // following ASTM standard practice https://doi.org/10.1520/D1535-14R18
    private static double GetValue(double y)
    {
        return y <= 0.9
            ? 0.87445 * Math.Pow(y, 0.9967)
            : 2.49268 * Math.Pow(y, 1 / 3.0) - 1.5614 - 0.985 / (Math.Pow(0.1073 * y - 3.084, 2) + 7.54)
              + 0.0133 / Math.Pow(y, 2.3) + 0.0084 * Math.Sin(4.1 * Math.Pow(y, 1 / 3.0) + 1)
              + 0.0221 / y * Math.Sin(0.39 * (y - 2))
              - 0.0037 / (0.44 * y) * Math.Sin(1.28 * (y - 0.53));
    }

    public override string ToString()
    {
        var roundedDegrees = Math.Round(HueDegrees);
        var roundedHue = FromDegrees(roundedDegrees);
        return $"{roundedHue.number:F1}{roundedHue.letter} {Value:F1}/{Chroma:F1}";
    } 
}

