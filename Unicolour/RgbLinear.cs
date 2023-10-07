namespace Wacton.Unicolour;

public record RgbLinear : ColourRepresentation
{
    protected override int? HueIndex => null;
    public double R => First;
    public double G => Second;
    public double B => Third;
    public double ConstrainedR => ConstrainedFirst;
    public double ConstrainedG => ConstrainedSecond;
    public double ConstrainedB => ConstrainedThird;
    protected override double ConstrainedFirst => R.Clamp(0.0, 1.0);
    protected override double ConstrainedSecond => G.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => B.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => ConstrainedR.Equals(ConstrainedG) && ConstrainedG.Equals(ConstrainedB);
    
    // https://www.w3.org/TR/WCAG21/#dfn-relative-luminance - effectively an approximation of Y from XYZ, but will stick to the specification
    internal double RelativeLuminance => UseAsNaN ? double.NaN : 0.2126 * R + 0.7152 * G + 0.0722 * B;

    public RgbLinear(double r, double g, double b) : this(r, g, b, ColourHeritage.None) {}
    internal RgbLinear(double r, double g, double b, ColourHeritage heritage) : base(r, g, b, heritage) {}
    
    protected override string FirstString => $"{R:F2}";
    protected override string SecondString => $"{G:F2}";
    protected override string ThirdString => $"{B:F2}";
    public override string ToString() => base.ToString();
    
    /*
     * if I implement Unicolour.FromRgbLinear(), will need to rethink the workflow a bit, where:
     * RGB-LINEAR is a transform of XYZ
     * RGB is a transform of RGB-LINEAR
     * (though with the aim of keeping RgbLinear a 'subspace' under Rgb)
     */
}