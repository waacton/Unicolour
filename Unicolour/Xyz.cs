namespace Wacton.Unicolour;

public record Xyz : ColourRepresentation
{
    protected override int? HueIndex => null;
    public double X => First;
    public double Y => Second;
    public double Z => Third;
    
    // could compare whitepoint against config.XyzWhitePoint
    // but requires making assumptions about floating-point comparison, which I don't want to do
    internal override bool IsGreyscale => false;

    public Xyz(double x, double y, double z) : this(x, y, z, ColourMode.Unset) {}
    internal Xyz(ColourTriplet triplet, ColourMode colourMode) : this(triplet.First, triplet.Second, triplet.Third, colourMode) {}
    internal Xyz(double x, double y, double z, ColourMode colourMode) : base(x, y, z, colourMode) {}

    protected override string FirstString => $"{X:F4}";
    protected override string SecondString => $"{Y:F4}";
    protected override string ThirdString => $"{Z:F4}";
    public override string ToString() => base.ToString();
    
    /*
     * XYZ is considered the root colour representation (in terms of Unicolour implementation)
     * so does not contain any forward (from another space) or reverse (back to original space) functions
     */
}