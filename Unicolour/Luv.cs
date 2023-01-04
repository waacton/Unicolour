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

    protected override string FirstString => $"{L:F2}";
    protected override string SecondString => $"{U:+0.00;-0.00;0.00}";
    protected override string ThirdString => $"{V:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
}