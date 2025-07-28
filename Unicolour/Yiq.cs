using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Yiq : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double Y => First;
    public double I => Second;
    public double Q => Third;
    public double ConstrainedY => ConstrainedFirst;
    public double ConstrainedI => ConstrainedSecond;
    public double ConstrainedQ => ConstrainedThird;
    protected override double ConstrainedFirst => Y.Clamp(0.0, 1.0);
    protected override double ConstrainedSecond => I.Clamp(-IMax, IMax);
    protected override double ConstrainedThird => Q.Clamp(QMin, -QMin);
    internal override bool IsGreyscale => I.Equals(0.0) && Q.Equals(0.0); // Y = 0 does not imply black; Y = 1 does not imply white

    public Yiq(double y, double i, double q) : this(y, i, q, ColourHeritage.None) {}
    internal Yiq(ColourTriplet triplet, ColourHeritage heritage) : this(triplet.First, triplet.Second, triplet.Third, heritage) {}
    internal Yiq(double y, double i, double q, ColourHeritage heritage) : base(y, i, q, heritage) {}
    
    protected override string String => $"{Y:F3} {I:+0.000;-0.000;0.000} {Q:+0.000;-0.000;0.000}";
    public override string ToString() => base.ToString();
    
    /*
     * YIQ is a transform of YUV
     * Forward: https://en.wikipedia.org/wiki/YIQ#From_YUV_to_YIQ_and_vice_versa
     * Reverse: https://en.wikipedia.org/wiki/YIQ#From_YUV_to_YIQ_and_vice_versa
     */

    // could also rotate [I Q] by [cos(33) sin(33), -sin(33) cos(33)] 
    private static readonly double Sin33 = Math.Sin(ToRadians(33));
    private static readonly double Cos33 = Math.Cos(ToRadians(33));
    private static readonly Matrix Rotation = new(new[,]
    {
        { 1, 0, 0 },
        { 0, -Sin33, Cos33 },
        { 0, Cos33, Sin33 }
    });

    // IMax derived from RGB Red -> YUV -> 33° rotation ~= 0.59508066230844403
    private static readonly double IMax = -Sin33 * (Yuv.UMax * ((0 - Yuv.Wr) / (1 - Yuv.Wb))) 
                                          + Cos33 * (Yuv.VMax * ((1 - Yuv.Wr) / (1 - Yuv.Wr)));
    
    // QMin derived from RGB Green -> YUV -> 33° rotation ~= -0.52228557764675743
    private static readonly double QMin = Cos33 * (Yuv.UMax * ((0 - Yuv.Wg) / (1 - Yuv.Wb))) 
                                          + Sin33 * (Yuv.VMax * ((0 - Yuv.Wg) / (1 - Yuv.Wr)));
    
    internal static Yiq FromYuv(Yuv yuv)
    {
        // effectively the following approximations from https://www.itu.int/rec/R-REC-BT.1700 (SMPTE 170M-2004):
        // i = -0.2746...G - 0.3213...B + 0.5959...R;
        // q = -0.5227...G + 0.3112...B + 0.2115...R;
        var yuvMatrix = Matrix.From(yuv);
        var yiqMatrix = Rotation.Multiply(yuvMatrix);
        return new Yiq(yiqMatrix.ToTriplet(), ColourHeritage.From(yuv));
    }
    
    internal static Yuv ToYuv(Yiq yiq)
    {
        // no need to invert rotation matrix
        // (the inverse of a rotation is its transpose, and the transpose of a symmetric matrix is itself)
        var yiqMatrix = Matrix.From(yiq);
        var yuvMatrix = Rotation.Multiply(yiqMatrix);
        return new Yuv(yuvMatrix.ToTriplet(), ColourHeritage.From(yiq));
    }
}