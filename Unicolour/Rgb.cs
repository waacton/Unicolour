namespace Wacton.Unicolour;

public record Rgb
{
    public double R { get; }
    public double G { get; }
    public double B { get; }
    public ColourTuple Tuple => new(R, G, B);

    public int R255 => (int) Math.Round(R * 255);
    public int G255 => (int) Math.Round(G * 255);
    public int B255 => (int) Math.Round(B * 255);
    public ColourTuple Tuple255 => new(R255, G255, B255);

    private readonly Func<double, double> inverseCompanding;
    public double RLinear => inverseCompanding(R);
    public double GLinear => inverseCompanding(G);
    public double BLinear => inverseCompanding(B);
    public ColourTuple TupleLinear => new(RLinear, GLinear, BLinear);

    public string Hex => $"#{R255:X2}{G255:X2}{B255:X2}";

    public Rgb(double r, double g, double b, Configuration config)
    {
        r.Guard(0.0, 1.0, "Red");
        g.Guard(0.0, 1.0, "Green");
        b.Guard(0.0, 1.0, "Blue");

        R = r;
        G = g;
        B = b;
        inverseCompanding = config.InverseCompanding;
    }
    
    public override string ToString() => $"{R255} {G255} {B255}";
}