namespace Wacton.Unicolour;

public record Ydbdr : ColourRepresentation
{
    protected internal override int? HueIndex => null;
    public double Y => First;
    public double Db => Second;
    public double Dr => Third;
    public double ConstrainedY => ConstrainedFirst;
    public double ConstrainedDb => ConstrainedSecond;
    public double ConstrainedDr => ConstrainedThird;
    protected override double ConstrainedFirst => Y.Clamp(0.0, 1.0);
    protected override double ConstrainedSecond => Db.Clamp(-DbMax, DbMax);
    protected override double ConstrainedThird => Dr.Clamp(-DrMax, DrMax);
    internal override bool IsGreyscale => Db.Equals(0.0) && Dr.Equals(0.0); // Y = 0 does not imply black; Y = 1 does not imply white

    public Ydbdr(double y, double db, double dr) : this(y, db, dr, ColourHeritage.None) {}
    internal Ydbdr(double y, double db, double dr, ColourHeritage heritage) : base(y, db, dr, heritage) {}
    
    protected override string String => $"{Y:F3} {Db:+0.000;-0.000;0.000} {Dr:+0.000;-0.000;0.000}";
    public override string ToString() => base.ToString();
    
    /*
     * YDbDr is a transform of YUV
     * Forward: https://en.wikipedia.org/wiki/YDbDr#Formulas
     * Reverse: https://en.wikipedia.org/wiki/YDbDr#Formulas
     * (note: wikipedia values of 3.059 & 2.169 seem to be inaccurate, unless the range of Db & Dr is not exactly -1.333 to +1.333)
     */

    private const double DbMax = 1.333 / Yuv.UMax; // ~= 3.0573394495412844
    private const double DrMax = 1.333 / Yuv.VMax; // ~= 2.1710097719869705
    
    internal static Ydbdr FromYuv(Yuv yuv)
    {
        var (y, u, v) = yuv;
        var db = DbMax * u;
        var dr = -DrMax * v;
        return new Ydbdr(y, db, dr, ColourHeritage.From(yuv));
    }
    
    internal static Yuv ToYuv(Ydbdr ydbdr)
    {
        var (y, db, dr) = ydbdr;
        var u = db / DbMax;
        var v = dr / -DrMax;
        return new Yuv(y, u, v, ColourHeritage.From(ydbdr));
    }
}