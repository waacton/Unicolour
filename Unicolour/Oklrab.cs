namespace Wacton.Unicolour;

public record Oklrab : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double L => First;
    public double A => Second;
    public double B => Third;
    
    protected override bool IsTripletAchromatic => A == 0.0 && B == 0.0;
    
    public Oklrab(double l, double a, double b) : this(l, a, b, Limitation.None) {}
    public Oklrab(double l) : this(l, 0, 0, Limitation.Achromatic) {}
    internal Oklrab(double l, double a, double b, Limitation limitation) : base(l, a, b, limitation) {}

    protected override string String => $"{L:F2} {A:+0.00;-0.00;0.00} {B:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * OKLrAB is a transform of OKLAB 
     * Forward: https://bottosson.github.io/posts/colorpicker/#intermission---a-new-lightness-estimate-for-oklab
     * Reverse: https://bottosson.github.io/posts/colorpicker/#intermission---a-new-lightness-estimate-for-oklab
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    internal static Oklrab FromOklab(Oklab oklab)
    {
        var (l, a, b) = oklab;
        var lr = Okhsv.Toe(l);
        return new Oklrab(lr, a, b, oklab.Limitation);
    }
    
    internal static Oklab ToOklab(Oklrab oklrab)
    {
        var (lr, a, b) = oklrab;
        var l = Okhsv.ToeInverse(lr);
        return new Oklab(l, a, b, oklrab.Limitation);
    }
}