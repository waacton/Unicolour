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

    public Rgb(double r, double g, double b, RgbConfiguration rgbConfig) : this(r, g, b, rgbConfig, ColourMode.Unset) {}
    internal Rgb(ColourTriplet triplet, RgbConfiguration rgbConfig, ColourMode colourMode) : this(triplet.First, triplet.Second, triplet.Third, rgbConfig, colourMode) {}
    internal Rgb(double r, double g, double b, RgbConfiguration rgbConfig, ColourMode colourMode) : base(r, g, b, colourMode)
    {
        double ToLinear(double value) => rgbConfig.InverseCompandToLinear(value);
        Linear = new RgbLinear(ToLinear(r), ToLinear(g), ToLinear(b), ColourMode.FromRepresentation(this));
        
        double To255(double value) => Math.Round(value * 255);
        Byte255 = new Rgb255(To255(r), To255(g), To255(b), ColourMode.FromRepresentation(this));
    }

    protected override string FirstString => $"{R:F2}";
    protected override string SecondString => $"{G:F2}";
    protected override string ThirdString => $"{B:F2}";
    public override string ToString() => base.ToString();
}