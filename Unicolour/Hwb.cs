namespace Wacton.Unicolour;

public record Hwb : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Hwb;
    protected override int? HueIndex => 0;
    public double H => First;
    public double W => Second;
    public double B => Third;
    public double ConstrainedH => ConstrainedFirst;
    public double ConstrainedW => ConstrainedSecond;
    public double ConstrainedB => ConstrainedThird;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => W.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => B.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => ConstrainedW + ConstrainedB >= 1.0;

    public Hwb(double h, double w, double b) : this(h, w, b, ColourMode.Unset) {}
    internal Hwb(double h, double w, double b, ColourMode colourMode) : base(h, w, b, colourMode) {}

    protected override string FirstString => IsEffectivelyHued ? $"{H:F1}°" : "—°";
    protected override string SecondString => $"{W * 100:F1}%";
    protected override string ThirdString => $"{B * 100:F1}%";
    public override string ToString() => base.ToString();
}