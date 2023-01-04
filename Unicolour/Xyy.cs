namespace Wacton.Unicolour;

public record Xyy : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Xyy;
    protected override int? HueIndex => null;
    public Chromaticity Chromaticity => new(First, Second);
    public double Luminance => Third;
    public Chromaticity ConstrainedChromaticity => new(ConstrainedFirst, ConstrainedSecond);
    public double ConstrainedLuminance => ConstrainedThird;
    protected override double ConstrainedFirst => Math.Max(Chromaticity.X, 0);
    protected override double ConstrainedSecond => Math.Max(Chromaticity.Y, 0);
    protected override double ConstrainedThird => Math.Max(Luminance, 0);
    
    // could compare chromaticity against config.ChromaticityWhite
    // but requires making assumptions about floating-point comparison, which I don't want to do
    internal override bool IsGreyscale => Luminance <= 0.0;

    public Xyy(double x, double y, double upperY) : this(x, y, upperY, ColourMode.Unset) {}
    internal Xyy(double x, double y, double upperY, ColourMode colourMode) : base(x, y, upperY, colourMode) { }

    protected override string FirstString => $"{Chromaticity.X:F4}";
    protected override string SecondString => $"{Chromaticity.Y:F4}";
    protected override string ThirdString => $"{Luminance:F4}";
    public override string ToString() => base.ToString();
}