namespace Wacton.Unicolour.Experimental;

public partial record Munsell
{
    public double HueNumber { get; private set; }
    public double? Value { get; private set; }
    public double? Chroma { get; private set; }
    public string? HueLetter { get; private set; }

    public Munsell(double hueNumber, double value, double chroma, string hueLetter)
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
        var luminance = 1.1914 * v - 0.22533 * Math.Pow(v, 2) + 0.23352 * Math.Pow(v, 3) - 0.020484 * Math.Pow(v, 4) + 0.00081939 * Math.Pow(v, 5);

        var lowerH = HueNumbers.Last(x => x <= HueNumber);
        var upperH = HueNumbers.First(x => x >= HueNumber);
        
        // TODO: if H <= 2.5, set to 10 of previous band
        // TODO: backing hue likely to be 0 - 360, so need a conversion to/from
        
        var lowerV = Values.Last(x => x <= Value);
        var upperV = Values.First(x => x >= Value);
        
        var lowerC = Chromas.Last(x => x <= Chroma);
        var upperC = Chromas.First(x => x >= Chroma);
        
        // TODO: interpolate
        var lower = Data.Value.Single(x => x.h == lowerH && x.v == lowerV && x.c == lowerC && x.letter == HueLetter);
        var upper = Data.Value.Single(x => x.h == upperH && x.v == upperV && x.c == upperC && x.letter == HueLetter);
        
        return new Xyy(lower.x, lower.y, luminance);
    }
    
    public static Munsell FromXyy(Xyy xyy)
    {
        var y = xyy.Luminance;

        double v;
        if (y <= 0.9)
        {
            v = 0.87445 * Math.Pow(y, 0.9967);
        }
        else
        {
            v = 2.49268 * Math.Pow(y, 1 / 3.0) - 1.5614 - 0.985 / (Math.Pow(0.1073 * y - 3.084, 2) + 7.54)
                + 0.0133 / Math.Pow(y, 2.3) + 0.0084 * Math.Sin(4.1 * Math.Pow(y, 1 / 3.0) + 1)
                + 0.0221 / y * Math.Sin(0.39 * (y - 2))
                - 0.0037 / (0.44 * y) * Math.Sin(1.28 * (y - 0.53));
        }
        
        return new Munsell(0, v, 0, "X");
    }
}

