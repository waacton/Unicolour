namespace Wacton.Unicolour;

public class Illuminant
{
    public static readonly Illuminant A = new(Spd.A, $"Illuminant {nameof(A)}");
    public static readonly Illuminant C = new(Spd.C, $"Illuminant {nameof(C)}");
    public static readonly Illuminant D50 = new(Spd.D50, $"Illuminant {nameof(D50)}");
    public static readonly Illuminant D55 = new(Spd.D55, $"Illuminant {nameof(D55)}");
    public static readonly Illuminant D65 = new(Spd.D65, $"Illuminant {nameof(D65)}");
    public static readonly Illuminant D75 = new(Spd.D75, $"Illuminant {nameof(D75)}");
    public static readonly Illuminant E = new(Spd.E, $"Illuminant {nameof(E)}");
    public static readonly Illuminant F2 = new(Spd.F2, $"Illuminant {nameof(F2)}");
    public static readonly Illuminant F7 = new(Spd.F7, $"Illuminant {nameof(F7)}");
    public static readonly Illuminant F11 = new(Spd.F11, $"Illuminant {nameof(F11)}");
    
    /*
     * as far as I'm aware, this is ASTM standard practice https://doi.org/10.1520/E0308-18 (Tables 5, 10 nm)
     * and the 2 degree observers are an exact match with calculations on the calculator at http://www.brucelindbloom.com/
     * ----------
     * ⚠️ NOTE: some other colour libraries have chosen the four-digit chromaticity values to represent white points, i.e. D65 = 0.3127, 0.3290
     * since no one can agree on white points (see also https://ninedegreesbelow.com/photography/well-behaved-profiles-quest.html#white-point-values)
     * so if there was a desire to align even more closely with other libraries, instead of ASTM standards
     * would need to be make these non-static and configurable: { (D65, Observer.Degree2), new Chromaticity(0.3127, 0.3290).ToWhitePoint() }
     * ----------
     * more likely: users can just create custom XYZ and RGB configs using desired chromaticity-based white point
     * might want to consider allowing configuration of D65 illuminant used by other spaces (e.g. Ictcp, Jzazbz, Oklab, Hct)
     * which cannot currently be overriden (because they must be D65!)
     * but the impact is minimal, with no agreed correct answer, so unlikely to happen
     */
    private static readonly Dictionary<(Illuminant, Observer), WhitePoint> WhitePoints = new()
    {
        { (A, Observer.Degree2), new(109.850, 100.000, 35.585) },
        { (C, Observer.Degree2), new(98.074, 100.000, 118.232) },
        { (D50, Observer.Degree2), new(96.422, 100.000, 82.521) },
        { (D55, Observer.Degree2), new(95.682, 100.000, 92.149) },
        { (D65, Observer.Degree2), new(95.047, 100.000, 108.883) },
        { (D75, Observer.Degree2), new(94.972, 100.000, 122.638) },
        { (E, Observer.Degree2), new(100.000, 100.000, 100.000) },
        { (F2, Observer.Degree2), new(99.186, 100.000, 67.393) },
        { (F7, Observer.Degree2), new(95.041, 100.000, 108.747) },
        { (F11, Observer.Degree2), new(100.962, 100.000, 64.350) },
        
        { (A, Observer.Degree10), new(111.144, 100.000, 35.200) },
        { (C, Observer.Degree10), new(97.285, 100.000, 116.145) },
        { (D50, Observer.Degree10), new(96.720, 100.000, 81.427) },
        { (D55, Observer.Degree10), new(95.799, 100.000, 90.926) },
        { (D65, Observer.Degree10), new(94.811, 100.000, 107.304) },
        { (D75, Observer.Degree10), new(94.416, 100.000, 120.641) },
        { (E, Observer.Degree10), new(100.000, 100.000, 100.000) },
        { (F2, Observer.Degree10), new(103.279, 100.000, 69.027) },
        { (F7, Observer.Degree10), new(95.792, 100.000, 107.686) },
        { (F11, Observer.Degree10), new(103.863, 100.000, 65.607) }
    };
    
    public string Name { get; }

    internal Spd? Spd { get; }
    public Illuminant(Spd spd, string name = Utils.Unnamed)
    {
        Spd = spd;
        Name = name;
    }
    
    // allows white point to be defined explicitly, not relative to an observer
    private readonly WhitePoint? whitePoint;
    public Illuminant(WhitePoint whitePoint, string name = Utils.Unnamed)
    {
        this.whitePoint = whitePoint;
        Name = name;
    }
    
    public WhitePoint GetWhitePoint(Observer observer)
    {
        // if white point is defined explicitly, observer is not relevant
        if (whitePoint != null) return whitePoint;

        // if white point for illuminant/observer pair is predefined, use that, no need to calculate
        var lookupKey = (this, observer);
        if (WhitePoints.TryGetValue(lookupKey, out var value))
        {
            return value;
        }
        
        // if either illuminant or observer is not predefined, white point needs to be calculated
        var xyz = Xyz.FromSpd(Spd!, observer);
        return WhitePoint.FromXyz(xyz);
    }
    
    public override string ToString() => Name;
}