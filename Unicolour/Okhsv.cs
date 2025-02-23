using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Okhsv : ColourRepresentation
{
    protected override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double V => Third;
    public double ConstrainedH => ConstrainedFirst;
    public double ConstrainedS => ConstrainedSecond;
    public double ConstrainedV => ConstrainedThird;
    protected override double ConstrainedFirst => H.Modulo(360.0);
    protected override double ConstrainedSecond => S.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => V.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => S <= 0.0 || V <= 0.0;
    
    public Okhsv(double h, double s, double v) : this(h, s, v, ColourHeritage.None) { }
    internal Okhsv(double h, double s, double v, ColourHeritage heritage) : base(h, s, v, heritage) { }
    
    protected override string FirstString => UseAsHued ? $"{H:F1}°" : "—°";
    protected override string SecondString => $"{S * 100:F1}%";
    protected override string ThirdString => $"{V * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * OKHSV is a transform of OKLAB
     * Forward: https://bottosson.github.io/posts/colorpicker/#hsv-2
     * Reverse: https://bottosson.github.io/posts/colorpicker/#hsv-2
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    internal static Okhsv FromOklab(Oklab oklab, XyzConfiguration xyzConfig, RgbConfiguration rgbConfig)
    {
        var (l, a, b) = oklab;
        var (_, c, h) = ToLchTriplet(oklab.L, oklab.A, oklab.B);
        var aPrime = a / c;
        var bPrime = b / c;
        
        double s, v;
        if (oklab.IsGreyscale)
        {
            // minor deviation from original algorithm, which doesn't provide guidance on handling edge cases that result in NaN
            // which is when L <= 0 || L >= 1 || C == 0, i.e. greyscale 
            // when greyscale assume no S, since greyscale has no saturation
            // and want V to match Lr of OKLrCH from https://bottosson.github.io/misc/colorpicker/ (see also "a new lightness estimate" https://bottosson.github.io/posts/colorpicker/#summary)
            // which appears to be just Toe(l) (https://github.com/bottosson/bottosson.github.io/blob/f6f08b7fde9436be1f20f66cebbc739d660898fd/misc/colorpicker/main.js#L134)
            (s, v) = (0.0, Toe(l));
        }
        else
        {
            var cusp = FindCusp(aPrime, bPrime, xyzConfig, rgbConfig);
            var (sMax, tMax) = (cusp.S, cusp.T);
            const double s0 = 0.5;
            var k = 1 - s0 / sMax;
            
            var t = tMax / (c + l * tMax);
            var lv = t * l;
            var cv = t * c;
            
            var lvt = ToeInverse(lv);
            var cvt = cv * lvt / lv;
            
            var rgbScale = ToLinearRgb(new Oklab(lvt, aPrime * cvt, bPrime * cvt), xyzConfig, rgbConfig);
            var scaleL = CubeRoot(1.0 / new[] { rgbScale.R, rgbScale.G, rgbScale.B, 0.0 }.Max());
            
            l /= scaleL;
            l = Toe(l);
            
            v = l / lv;
            s = (s0 + tMax) * cv / (tMax * s0 + tMax * k * cv);
        }
        
        return new Okhsv(h.Modulo(360.0), s, v, ColourHeritage.From(oklab));
    }
    
    internal static Oklab ToOklab(Okhsv okhsv, XyzConfiguration xyzConfig, RgbConfiguration rgbConfig)
    {
        var (h, s, v) = okhsv;
        double l, a, b;
        if (v == 0.0)
        {
            // minor deviation from original algorithm, which doesn't provide guidance on handling edge cases that result in NaN
            // when no V:
            // - assume no L, since L tends towards 0 as V decreases
            // - assume no A an B, since A and B tend towards 0 as V decreases
            (l, a, b) = (0, 0, 0);
        }
        else
        {
            var (_, aPrime, bPrime) = FromLchTriplet(new(double.NaN, 1, h));
            
            var cusp = FindCusp(aPrime, bPrime, xyzConfig, rgbConfig);
            var (sMax, tMax) = (cusp.S, cusp.T);
            const float s0 = 0.5f;
            var k = 1 - s0 / sMax;
            
            var lv = 1 - s * s0 / (s0 + tMax - tMax * k * s);
            var cv = s * tMax * s0 / (s0 + tMax - tMax * k * s);
            
            l = v * lv;
            var c = v * cv;
            
            var lvt = ToeInverse(lv);
            var cvt = cv * lvt / lv;
            
            var lNew = ToeInverse(l);
            c = c * lNew / l;
            l = lNew;
            
            var rgbScale = ToLinearRgb(new Oklab(lvt, aPrime * cvt, bPrime * cvt), xyzConfig, rgbConfig);
            var scaleL = CubeRoot(1.0 / new[] { rgbScale.R, rgbScale.G, rgbScale.B, 0.0 }.Max());
            
            l *= scaleL;
            c *= scaleL;

            a = c * aPrime;
            b = c * bPrime;
        }

        return new Oklab(l, a, b, ColourHeritage.From(okhsv));
    }

    internal static Cusp FindCusp(double a, double b, XyzConfiguration xyzConfig, RgbConfiguration rgbConfig)
    {
        var sCusp = ComputeMaxSaturation(a, b);
        var rgbAtMax = ToLinearRgb(new Oklab(1, sCusp * a, sCusp * b), xyzConfig, rgbConfig);
        var lCusp = CubeRoot(1.0 / new[] { rgbAtMax.R, rgbAtMax.G, rgbAtMax.B }.Max());
        var cCusp = lCusp * sCusp;
        return new(lCusp, cCusp);
    }

    private static readonly List<(double k0, double k1, double k2, double k3, double k4)> ConstantsK = new()
    {
        (+1.19086277, +1.76576728, +0.59662641, +0.75515197, +0.56771245),
        (+0.73956515, -0.45954404, +0.08285427, +0.12541070, +0.14503204),
        (+1.35733652, -0.00915799, -1.15130210, -0.50559606, 0.00692167)
    };
    
    private static readonly List<(double wl, double wm, double ws)> ConstantsW = new()
    {
        (+4.0767416621, -3.3077115913, +0.2309699292),
        (-1.2684380046, +2.6097574011, -0.3413193965),
        (-0.0041960863, -0.7034186147, +1.7076147010)
    };
    
    private static double ComputeMaxSaturation(double a, double b)
    {
        int constantsIndex;
        if (-1.88170328 * a - 0.80936493 * b > 1)
        {
            constantsIndex = 0;
        }
        else if (1.81444104 * a - 1.19445276 * b > 1)
        {
            constantsIndex = 1;
        }
        else
        {
            constantsIndex = 2;
        }
        
        var (k0, k1, k2, k3, k4) = ConstantsK[constantsIndex];
        var (wl, wm, ws) = ConstantsW[constantsIndex];
        
        var saturation = k0 + k1 * a + k2 * b + k3 * Math.Pow(a, 2) + k4 * a * b;
        
        var kl = +0.3963377774 * a + 0.2158037573 * b;
        var km = -0.1055613458 * a - 0.0638541728 * b;
        var ks = -0.0894841775 * a - 1.2914855480 * b;
        
        var lPrime = 1 + saturation * kl;
        var mPrime = 1 + saturation * km;
        var sPrime = 1 + saturation * ks;
        
        var l = Math.Pow(lPrime, 3);
        var m = Math.Pow(mPrime, 3);
        var s = Math.Pow(sPrime, 3);
        
        var lds = 3 * kl * Math.Pow(lPrime, 2);
        var mds = 3 * km * Math.Pow(mPrime, 2);
        var sds = 3 * ks * Math.Pow(sPrime, 2);
        
        var lds2 = 6 * Math.Pow(kl, 2) * lPrime;
        var mds2 = 6 * Math.Pow(km, 2) * mPrime;
        var sds2 = 6 * Math.Pow(ks, 2) * sPrime;
        
        var f = wl * l + wm * m + ws * s;
        var f1 = wl * lds + wm * mds + ws * sds;
        var f2 = wl * lds2 + wm * mds2 + ws * sds2;
        
        return saturation - f * f1 / (Math.Pow(f1, 2) - 0.5 * f * f2);
    }

    private static RgbLinear ToLinearRgb(Oklab oklab, XyzConfiguration xyzConfig, RgbConfiguration rgbConfig)
    {
        var xyz = Oklab.ToXyz(oklab, xyzConfig, rgbConfig);
        var rgbLinear = RgbLinear.FromXyz(xyz, rgbConfig, xyzConfig);
        return rgbLinear;
    }
    
    private const double K1 = 0.206;
    private const double K2 = 0.03;
    private const double K3 = (1 + K1) / (1 + K2);
    
    internal static double Toe(double x) => 0.5 * (K3 * x - K1 + Math.Sqrt((K3 * x - K1) * (K3 * x - K1) + 4 * K2 * K3 * x));
    
    internal static double ToeInverse(double x) => (Math.Pow(x, 2) + K1 * x) / (K3 * (x + K2));
    
    internal record Cusp(double L, double C)
    {
        internal double L { get; } = L;
        internal double C { get; } = C;
        internal double S => C / L;
        internal double T => C / (1 - L);
    }
}