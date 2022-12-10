namespace Wacton.Unicolour;

public record Lchab : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Lchab;
    protected override int? HueIndex => 2;
    public double L => First;
    public double C => Second;
    public double H => Third;
    public double ConstrainedH => ConstrainedThird;
    protected override double ConstrainedThird => H.Modulo(360.0);
    internal override bool IsGreyscale => C <= 0.0 || L is <= 0.0 or >= 100.0;
    
    public Lchab(double l, double c, double h) : this(l, c, h, ColourMode.Unset) {}
    internal Lchab(double l, double c, double h, ColourMode colourMode) : base(l, c, h, colourMode) {}

    protected override string FirstString => $"{Math.Round(L, 2)}";
    protected override string SecondString => $"{Math.Round(C, 2)}";
    protected override string ThirdString => $"{(IsEffectivelyHued ? Math.Round(H, 1) : "—")}°";
    public override string ToString() => base.ToString();
}