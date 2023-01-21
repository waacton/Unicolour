namespace Wacton.Unicolour;

public record Ictcp : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Ictcp;
    protected override int? HueIndex => null;
    public double I => First;
    public double Ct => Second;
    public double Cp => Third;
    internal override bool IsGreyscale => Ct.Equals(0.0) && Cp.Equals(0.0);
    
    public Ictcp(double i, double ct, double cp) : this(i, ct, cp, ColourMode.Unset) {}
    internal Ictcp(ColourTriplet triplet, ColourMode colourMode) : this(triplet.First, triplet.Second, triplet.Third, colourMode) {}
    internal Ictcp(double i, double ct, double cp, ColourMode colourMode) : base(i, ct, cp, colourMode) {}

    protected override string FirstString => $"{I:F2}";
    protected override string SecondString => $"{Ct:+0.00;-0.00;0.00}";
    protected override string ThirdString => $"{Cp:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
}