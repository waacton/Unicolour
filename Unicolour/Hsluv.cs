namespace Wacton.Unicolour;

public record Hsluv : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Hsluv;
    protected override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double L => Third;
    public double ConstrainedH => ConstrainedFirst;
    public double ConstrainedS => ConstrainedSecond;
    public double ConstrainedL => ConstrainedThird;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => S.Clamp(0.0, 100.0);
    protected override double ConstrainedThird => L.Clamp(0.0, 100.0);
    internal override bool IsGreyscale => S <= 0.0 || L is <= 0.0 or >= 100.0;

    public Hsluv(double h, double s, double l) : this(h, s, l, ColourMode.Unset) {}
    internal Hsluv(double h, double s, double l, ColourMode colourMode) : base(h, s, l, colourMode) {}

    protected override string FirstString => IsEffectivelyHued ? $"{H:F1}°" : "—°";
    protected override string SecondString => $"{S:F1}%";
    protected override string ThirdString => $"{L:F1}%";
    public override string ToString() => base.ToString();
}