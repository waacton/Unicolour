namespace Wacton.Unicolour;

public record Hsb : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Hsb;
    protected override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double B => Third;
    public double ConstrainedH => ConstrainedFirst;
    public double ConstrainedS => ConstrainedSecond;
    public double ConstrainedB => ConstrainedThird;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => S.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => B.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => S <= 0.0 || B <= 0.0;

    public Hsb(double h, double s, double b) : this(h, s, b, ColourMode.Unset) {}
    internal Hsb(double h, double s, double b, ColourMode colourMode) : base(h, s, b, colourMode) {}

    protected override string FirstString => IsEffectivelyHued ? $"{H:F1}°" : "—°";
    protected override string SecondString => $"{S * 100:F1}%";
    protected override string ThirdString => $"{B * 100:F1}%";
    public override string ToString() => base.ToString();
}