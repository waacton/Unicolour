namespace Wacton.Unicolour;

using static Utils;

public record Lab : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Lab;
    protected override int? HueIndex => null;
    public double L => First;
    public double A => Second;
    public double B => Third;
    internal override bool IsGreyscale => A.Equals(0.0) && B.Equals(0.0);
    
    public Lab(double l, double a, double b) : this(l, a, b, ColourMode.Unset) {}
    internal Lab(double l, double a, double b, ColourMode colourMode) : base(l, a, b, colourMode) {}

    protected override string FirstString => $"{L:F2}";
    protected override string SecondString => $"{A:+0.00;-0.00;0.00}";
    protected override string ThirdString => $"{B:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
}