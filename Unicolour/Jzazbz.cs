namespace Wacton.Unicolour;

using static Utils;

public record Jzazbz : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Jzazbz;
    protected override int? HueIndex => null;
    public double J => First;
    public double A => Second;
    public double B => Third;
    
    // based on the figures from the paper, greyscale behaviour is the same as LAB
    // i.e. non-lightness axes are zero
    internal override bool IsGreyscale => A.Equals(0.0) && B.Equals(0.0);
    
    public Jzazbz(double j, double a, double b) : this(j, a, b, ColourMode.Unset) {}
    internal Jzazbz(double j, double a, double b, ColourMode colourMode) : base(j, a, b, colourMode) {}

    protected override string FirstString => $"{Math.Round(J, 3)}";
    protected override string SecondString => $"{Signed(Math.Round(A, 3))}";
    protected override string ThirdString => $"{Signed(Math.Round(B, 3))}";
    public override string ToString() => base.ToString();
}