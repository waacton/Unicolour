namespace Wacton.Unicolour;

public record Lchuv : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Lchuv;
    protected override int? HueIndex => 2;
    public double L => First;
    public double C => Second;
    public double H => Third;
    public double ConstrainedH => ConstrainedThird;
    protected override double ConstrainedThird => H.Modulo(360.0);
    internal override bool IsGreyscale => C <= 0.0 || L is <= 0.0 or >= 100.0;
    
    public Lchuv(double l, double c, double h) : this(l, c, h, ColourMode.Unset) {}
    internal Lchuv(double l, double c, double h, ColourMode colourMode) : base(l, c, h, colourMode) {}

    protected override string FirstString => $"{L:F2}";
    protected override string SecondString => $"{C:F2}";
    protected override string ThirdString => IsEffectivelyHued ? $"{H:F1}°" : "—°";
    public override string ToString() => base.ToString();
}