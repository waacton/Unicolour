namespace Wacton.Unicolour.Experimental;

public partial record Munsell
{
    public double HueNumber { get; private set; }
    public double? Value { get; private set; }
    public double? Chroma { get; private set; }
    public Letter? HueLetter { get; private set; }

    public Munsell(double hueNumber, double value, double chroma, Letter hueLetter)
    {
        HueNumber = hueNumber;
        Value = value;
        Chroma = chroma;
        HueLetter = hueLetter;
    }

    public Munsell(double greyNumber)
    {
        HueNumber = greyNumber;
        Value = null;
        Chroma = null;
        HueLetter = null;
    }

    // TODO: handle grey ("N" hue, or 0 chroma)
    public Xyy ToXyy()
    {
        if (!Value.HasValue) throw new NotImplementedException();
        
        var v = (double)Value;
        
        // following ASTM standard practice https://doi.org/10.1520/D1535-14R18
        var luminance = (1.1914 * v) - (0.22533 * Math.Pow(v, 2)) + (0.23352 * Math.Pow(v, 3)) - (0.020484 * Math.Pow(v, 4)) + (0.00081939 * Math.Pow(v, 5));

        return new Xyy(double.NaN, double.NaN, luminance);
    }
}

