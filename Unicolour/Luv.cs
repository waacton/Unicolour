namespace Wacton.Unicolour;

using static Utils;

public record Luv : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Luv;
    protected override int? HueIndex => null;
    public double L => First;
    public double U => Second;
    public double V => Third;
    internal override bool IsGreyscale => U.Equals(0.0) && V.Equals(0.0);
    
    public Luv(double l, double u, double v) : this(l, u, v, ColourMode.Unset) {}
    internal Luv(double l, double u, double v, ColourMode colourMode) : base(l, u, v, colourMode) {}

    protected override string FirstString => $"{Math.Round(L, 2)}";
    protected override string SecondString => $"{Signed(Math.Round(U, 2))}";
    protected override string ThirdString => $"{Signed(Math.Round(V, 2))}";
    public override string ToString() => base.ToString();
}