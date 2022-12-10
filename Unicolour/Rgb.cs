namespace Wacton.Unicolour;

public record Rgb : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Rgb;
    protected override int? HueIndex => null;
    public double R => First;
    public double G => Second;
    public double B => Third;
    public double ConstrainedR => ConstrainedFirst;
    public double ConstrainedG => ConstrainedSecond;
    public double ConstrainedB => ConstrainedThird;
    protected override double ConstrainedFirst => R.Clamp(0.0, 1.0);
    protected override double ConstrainedSecond => G.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => B.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => ConstrainedR.Equals(ConstrainedG) && ConstrainedG.Equals(ConstrainedB);
    
    public RgbLinear Linear { get; }
    public Rgb255 Byte255 { get; }

    public Rgb(double r, double g, double b, Configuration config) : this(r, g, b, config, ColourMode.Unset) {}
    internal Rgb(double r, double g, double b, Configuration config, ColourMode colourMode) : base(r, g, b, colourMode)
    {
        double ToLinear(double value) => config.InverseCompand(value);
        Linear = new RgbLinear(ToLinear(r), ToLinear(g), ToLinear(b), ColourMode.FromRepresentation(this));
        
        double To255(double value) => Math.Round(value * 255);
        Byte255 = new Rgb255(To255(r), To255(g), To255(b), ColourMode.FromRepresentation(this));
    }

    protected override string FirstString => $"{Math.Round(R, 2)}";
    protected override string SecondString => $"{Math.Round(G, 2)}";
    protected override string ThirdString => $"{Math.Round(B, 2)}";
    public override string ToString() => base.ToString();
}