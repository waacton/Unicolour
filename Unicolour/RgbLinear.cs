﻿namespace Wacton.Unicolour;

public record RgbLinear : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.RgbLinear;
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

    public RgbLinear(double r, double g, double b) : this(r, g, b, ColourMode.Unset) {}
    internal RgbLinear(double r, double g, double b, ColourMode colourMode) : base(r, g, b, colourMode) {}

    protected override string FirstString => $"{R:F2}";
    protected override string SecondString => $"{G:F2}";
    protected override string ThirdString => $"{B:F2}";
    public override string ToString() => base.ToString();
}