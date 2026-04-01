using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public record Okhsl : ColourRepresentation
{
    protected internal override int? HueIndex => 0;
    public double H => First;
    public double S => Second;
    public double L => Third;
    
    // a colour defined using all 3 coordinates of a hue-based system by definition has hue and chroma (even if it cannot be detected)
    protected override bool IsAchromatic => false;
    
    public Okhsl(double h, double s, double l) : this(h, s, l, Limitation.None) {}
    internal Okhsl(double h, double s, double l, Limitation limitation) : base(h, s, l, limitation) {}

    protected override string String => Limitation != Limitation.Achromatic ? $"{H:F1}° {S * 100:F1}% {L * 100:F1}%" : $"{NoHue}° {S * 100:F1}% {L * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * OKHSL is a transform of OKLAB
     * Forward: https://bottosson.github.io/posts/colorpicker/#hsl-2
     * Reverse: https://bottosson.github.io/posts/colorpicker/#hsl-2
     *
     * ⚠️
     * this colour space is potentially defined relative to sRGB, but Unicolour does not currently enforce sRGB
     * (using other RGB configs may lead to unexpected results, though it may be desirable to explore non-sRGB behaviour)
     */
    
    internal static Okhsl FromOklab(Oklab oklab, ChromaticAdaptor chromaticAdaptor, RgbConfiguration rgbConfig)
    {
        var (l, a, b) = oklab;
        
        if (oklab.Limitation == Limitation.Achromatic)
        {
            // minor deviation from original algorithm, which doesn't provide guidance on handling edge cases that result in NaN
            // which is when Oklab has no chroma information, so fall back to 0 saturation
            // and set V to match Lr of OKLrCH from https://bottosson.github.io/misc/colorpicker/ (see also "a new lightness estimate" https://bottosson.github.io/posts/colorpicker/#summary)
            // which appears to be just Toe(l) (https://github.com/bottosson/bottosson.github.io/blob/f6f08b7fde9436be1f20f66cebbc739d660898fd/misc/colorpicker/main.js#L134)
            return new Okhsl(0, 0, Okhsv.Toe(l), oklab.Limitation);
        }
        
        var (_, c, h) = ToLchTriplet(oklab.Triplet);
        var aPrime = a / c;
        var bPrime = b / c;
        
        var lightness = Okhsv.Toe(l);
        var (c0, cMid, cMax) = GetCs(l, aPrime, bPrime, chromaticAdaptor, rgbConfig);
        
        const double mid = 0.8;
        const double midInv = 1.25;
        
        double s;
        if (c < cMid)
        {
            var k1 = mid * c0;
            var k2 = 1 - k1 / cMid;
            
            var t = c / (k1 + k2 * c);
            s = t * mid;
        }
        else
        {
            var k0 = cMid;
            var k1 = (1 - mid) * Math.Pow(cMid, 2) * midInv * midInv / c0;
            var k2 = 1 - k1 / (cMax - cMid);
            
            var t = (c - k0) / (k1 + k2 * (c - k0));
            s = mid + (1 - mid) * t;
        }
        
        return new Okhsl(h, s, lightness, oklab.Limitation);
    }

    internal static Oklab ToOklab(Okhsl okhsl, ChromaticAdaptor chromaticAdaptor, RgbConfiguration rgbConfig)
    {
        var (h, s, lightness) = okhsl.WithHueModulo();
        
        if (lightness is 0.0 or 1.0)
        {
            return new Oklab(lightness, 0, 0, okhsl.Limitation);
        }
        
        var l = Okhsv.ToeInverse(lightness);

        var (_, aPrime, bPrime) = FromLchTriplet(new(double.NaN, 1, h));
        var (c0, cMid, cMax) = GetCs(l, aPrime, bPrime, chromaticAdaptor, rgbConfig);
        
        const double mid = 0.8;
        const double midInv = 1.25;

        double c;
        if (s < mid)
        {
            var t = midInv * s;
            
            var k1 = mid * c0;
            var k2 = 1 - k1 / cMid;
            
            c = t * k1 / (1 - k2 * t);
        }
        else
        {
            var t = (s - mid) / (1 - mid);
            
            var k0 = cMid;
            var k1 = (1 - mid) * cMid * cMid * midInv * midInv / c0;
            var k2 = 1 - k1 / (cMax - cMid);
            
            c = k0 + t * k1 / (1 - k2 * t);
        }
        
        var a = c * aPrime;
        var b = c * bPrime;
        
        return new Oklab(l, a, b, okhsl.Limitation);
    }

    private static (double c0, double cMid, double cMax) GetCs(double l, double aPrime, double bPrime, ChromaticAdaptor chromaticAdaptor, RgbConfiguration rgbConfig)
    {
        var cusp = Okhsv.FindCusp(aPrime, bPrime, chromaticAdaptor, rgbConfig);
        var cMax = FindGamutIntersection(aPrime, bPrime, l, 1, l, cusp);
        var (sMax, tMax) = (cusp.S, cusp.T);
        var k = cMax / Math.Min(l * sMax, (1 - l) * tMax);
        
        var (sMid, tMid) = GetMidSt(aPrime, bPrime);
        var caMid = l * sMid;
        var cbMid = (1 - l) * tMid;
        var cMid = 0.9 * k * Math.Sqrt(Math.Sqrt(1 / (1 / Math.Pow(caMid, 4) + 1 / Math.Pow(cbMid, 4))));
        
        var ca0 = l * 0.4;
        var cb0 = (1 - l) * 0.8;
        var c0 = Math.Sqrt(1 / (1 / Math.Pow(ca0, 2) + 1 / Math.Pow(cb0, 2)));
        
        return (c0, cMid, cMax);
    }
    
    private static double FindGamutIntersection(double a, double b, double l1, double c1, double l0, Okhsv.Cusp cusp)
    {
        double t;
        if ((l1 - l0) * cusp.C - (cusp.L - l0) * c1 <= 0)
        {
            t = cusp.C * l0 / (c1 * cusp.L + cusp.C * (l0 - l1));
        }
        else
        {
            t = cusp.C * (l0 - 1) / (c1 * (cusp.L - 1) + cusp.C * (l0 - l1));
            
            var dL = l1 - l0;
            var dC = c1;
            
            var kl = +0.3963377774 * a + 0.2158037573 * b;
            var km = -0.1055613458 * a - 0.0638541728 * b;
            var ks = -0.0894841775 * a - 1.2914855480 * b;
            
            var ldt = dL + dC * kl;
            var mdt = dL + dC * km;
            var sdt = dL + dC * ks;
            
            var upperL = l0 * (1 - t) + t * l1;
            var c = t * c1;
            
            var lPrime = upperL + c * kl;
            var mPrime = upperL + c * km;
            var sPrime = upperL + c * ks;
            
            var l = Math.Pow(lPrime, 3);
            var m = Math.Pow(mPrime, 3);
            var s = Math.Pow(sPrime, 3);
            
            var ldt1 = 3 * ldt * Math.Pow(lPrime, 2);
            var mdt1 = 3 * mdt * Math.Pow(mPrime, 2);
            var sdt1 = 3 * sdt * Math.Pow(sPrime, 2);
            
            var ldt2 = 6 * Math.Pow(ldt, 2) * lPrime;
            var mdt2 = 6 * Math.Pow(mdt, 2) * mPrime;
            var sdt2 = 6 * Math.Pow(sdt, 2) * sPrime;
            
            var f0 = 4.0767416621 * l - 3.3077115913 * m + 0.2309699292 * s - 1;
            var r1 = 4.0767416621 * ldt1 - 3.3077115913 * mdt1 + 0.2309699292 * sdt1;
            var r2 = 4.0767416621 * ldt2 - 3.3077115913 * mdt2 + 0.2309699292 * sdt2;
            
            var ur = r1 / (r1 * r1 - 0.5 * f0 * r2);
            var tr = -f0 * ur;
            
            var g0 = -1.2684380046 * l + 2.6097574011 * m - 0.3413193965 * s - 1;
            var g1 = -1.2684380046 * ldt1 + 2.6097574011 * mdt1 - 0.3413193965 * sdt1;
            var g2 = -1.2684380046 * ldt2 + 2.6097574011 * mdt2 - 0.3413193965 * sdt2;
            
            var ug = g1 / (g1 * g1 - 0.5 * g0 * g2);
            var tg = -g0 * ug;
            
            var b0 = -0.0041960863 * l - 0.7034186147 * m + 1.7076147010 * s - 1;
            var b1 = -0.0041960863 * ldt1 - 0.7034186147 * mdt1 + 1.7076147010 * sdt1;
            var b2 = -0.0041960863 * ldt2 - 0.7034186147 * mdt2 + 1.7076147010 * sdt2;
            
            var ub = b1 / (b1 * b1 - 0.5 * b0 * b2);
            var tb = -b0 * ub;
            
            tr = ur >= 0 ? tr : float.MaxValue;
            tg = ug >= 0 ? tg : float.MaxValue;
            tb = ub >= 0 ? tb : float.MaxValue;
            
            t += new[] { tr, tg, tb }.Min();
        }
        
        return t;
    }
    
    private static (double s, double t) GetMidSt(double aPrime, double bPrime)
    {
        var s = 0.11516993 + 1 /
            (+7.44778970 + 4.15901240 * bPrime + aPrime *
                (-2.19557347 + 1.75198401 * bPrime + aPrime *
                    (-2.13704948 - 10.02301043 * bPrime + aPrime *
                        (-4.24894561 + 5.38770819 * bPrime + 4.69891013 * aPrime))));
        
        var t = 0.11239642 + 1 /
            (+1.61320320 - 0.68124379 * bPrime + aPrime *
                (+0.40370612 + 0.90148123 * bPrime + aPrime *
                    (-0.27087943 + 0.61223990 * bPrime + aPrime *
                        (+0.00299215 - 0.45399568 * bPrime - 0.14661872 * aPrime))));
        
        return (s, t);
    }
}