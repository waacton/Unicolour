namespace Wacton.Unicolour;

public record Rgb
{
    public double R { get; }
    public double G { get; }
    public double B { get; }
    public ColourTuple Tuple => new(R, G, B);

    public double ClampedR => R.Clamp(0.0, 1.0);
    public double ClampedG => G.Clamp(0.0, 1.0);
    public double ClampedB => B.Clamp(0.0, 1.0);
    public ColourTuple ClampedTuple => new(ClampedR, ClampedG, ClampedB);

    public int R255 => (int) Math.Round(ClampedR * 255);
    public int G255 => (int) Math.Round(ClampedG * 255);
    public int B255 => (int) Math.Round(ClampedB * 255);
    public ColourTuple Tuple255 => new(R255, G255, B255);

    private readonly Func<double, double> inverseCompand;
    public double RLinear => inverseCompand(R);
    public double GLinear => inverseCompand(G);
    public double BLinear => inverseCompand(B);
    public ColourTuple TupleLinear => new(RLinear, GLinear, BLinear);

    public string Hex => $"#{R255:X2}{G255:X2}{B255:X2}";

    public Rgb(double r, double g, double b, Configuration config) 
    {
        R = r;
        G = g;
        B = b;
        inverseCompand = config.InverseCompand;
    }
    
    public override string ToString() => $"{R255} {G255} {B255}";
}