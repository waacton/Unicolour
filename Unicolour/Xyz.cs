namespace Wacton.Unicolour;

/*
 * I don't know of a way to accurately report whether an XYZ triplet is greyscale
 * one option is to check that ratios match (i.e. X / WhitePoint.X == Y / WhitePoint.Y etc.)
 * but this means making assumptions about floating-point comparison tolerances
 * so for now, XYZ colour is only labelled as greyscale when converted to from a known greyscale colour in a different space
 */
public record Xyz : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Xyz;
    protected override int? HueIndex => null;
    public double X => First;
    public double Y => Second;
    public double Z => Third;
    internal override bool IsGreyscale => false;

    public Xyz(double x, double y, double z) : this(x, y, z, ColourMode.Unset) {}
    internal Xyz(double x, double y, double z, ColourMode colourMode) : base(x, y, z, colourMode) {}

    protected override string FirstString => $"{Math.Round(X, 2)}";
    protected override string SecondString => $"{Math.Round(Y, 2)}";
    protected override string ThirdString => $"{Math.Round(Z, 2)}";
    public override string ToString() => base.ToString();
}