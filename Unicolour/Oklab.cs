namespace Wacton.Unicolour;

public record Oklab : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Oklab;
    protected override int? HueIndex => null;
    public double L => First;
    public double A => Second;
    public double B => Third;
    internal override bool IsGreyscale => A.Equals(0.0) && B.Equals(0.0);

    public Oklab(double l, double a, double b) : this(l, a, b, ColourMode.Unset) {}
    internal Oklab(double l, double a, double b, ColourMode colourMode) : base(l, a, b, colourMode) {}

    protected override string FirstString => $"{Math.Round(L, 2)}";
    protected override string SecondString => $"{Math.Round(A, 2)}";
    protected override string ThirdString => $"{Math.Round(B, 2)}";
    public override string ToString() => base.ToString();
}