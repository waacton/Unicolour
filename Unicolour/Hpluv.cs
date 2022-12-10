namespace Wacton.Unicolour;

public record Hpluv : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Hpluv;
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

    public Hpluv(double h, double s, double l) : this(h, s, l, ColourMode.Unset) {}
    internal Hpluv(double h, double s, double l, ColourMode colourMode) : base(h, s, l, colourMode) {}

    protected override string FirstString => $"{(IsEffectivelyHued ? Math.Round(H, 1) : "—")}°";
    protected override string SecondString => $"{Math.Round(S, 1)}%";
    protected override string ThirdString => $"{Math.Round(L, 1)}%";
    public override string ToString() => base.ToString();
}