namespace Wacton.Unicolour;

public record Rgb
{
    public double R { get; }
    public double G { get; }
    public double B { get; }
    public ColourTriplet Triplet => new(R, G, B);

    public double ConstrainedR => R.Clamp(0.0, 1.0);
    public double ConstrainedG => G.Clamp(0.0, 1.0);
    public double ConstrainedB => B.Clamp(0.0, 1.0);
    public ColourTriplet ConstrainedTriplet => new(ConstrainedR, ConstrainedG, ConstrainedB);

    public int R255 => (int) Math.Round(ConstrainedR * 255);
    public int G255 => (int) Math.Round(ConstrainedG * 255);
    public int B255 => (int) Math.Round(ConstrainedB * 255);
    public ColourTriplet Triplet255 => new(R255, G255, B255);

    private readonly Func<double, double> inverseCompand;
    public double RLinear => inverseCompand(R);
    public double GLinear => inverseCompand(G);
    public double BLinear => inverseCompand(B);
    public ColourTriplet TripletLinear => new(RLinear, GLinear, BLinear);
    
    public double ConstrainedRLinear => RLinear.Clamp(0.0, 1.0);
    public double ConstrainedGLinear => GLinear.Clamp(0.0, 1.0);
    public double ConstrainedBLinear => BLinear.Clamp(0.0, 1.0);
    public ColourTriplet ConstrainedTripletLinear => new(ConstrainedRLinear, ConstrainedGLinear, ConstrainedBLinear);

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