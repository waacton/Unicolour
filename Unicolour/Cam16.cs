using static Wacton.Unicolour.Utils;
using Model = Wacton.Unicolour.Cam16Model;
using View = Wacton.Unicolour.Cam16ViewingConditions;
using Jch = Wacton.Unicolour.Cam16Jch;
using Jmh = Wacton.Unicolour.Cam16Jmh;
using Jsh = Wacton.Unicolour.Cam16Jsh;
using Qch = Wacton.Unicolour.Cam16Qch;
using Qmh = Wacton.Unicolour.Cam16Qmh;
using Qsh = Wacton.Unicolour.Cam16Qsh;

namespace Wacton.Unicolour;

public record Cam16
{
    private readonly ColourTriplet? source;
    private readonly View view;
    
    public Jch Jch => field ??= source as Jch ?? new Jch(Model.J, Model.C, Model.H, Model.Limitation);
    public Jmh Jmh => field ??= source as Jmh ?? new Jmh(Model.J, Model.M, Model.H, Model.Limitation);
    public Jsh Jsh => field ??= source as Jsh ?? new Jsh(Model.J, Model.S, Model.H, Model.Limitation);
    public Qch Qch => field ??= source as Qch ?? new Qch(Model.Q, Model.C, Model.H, Model.Limitation);
    public Qmh Qmh => field ??= source as Qmh ?? new Qmh(Model.Q, Model.M, Model.H, Model.Limitation);
    public Qsh Qsh => field ??= source as Qsh ?? new Qsh(Model.Q, Model.S, Model.H, Model.Limitation);
    public Model Model => field ??= GetModel();
    
    public Cam16(Jch jch, View view) : this((ColourTriplet)jch, view) {}
    public Cam16(Jmh jmh, View view) : this((ColourTriplet)jmh, view) {}
    public Cam16(Jsh jsh, View view) : this((ColourTriplet)jsh, view) {}
    public Cam16(Qch qch, View view) : this((ColourTriplet)qch, view) {}
    public Cam16(Qmh qmh, View view) : this((ColourTriplet)qmh, view) {}
    public Cam16(Qsh qsh, View view) : this((ColourTriplet)qsh, view) {}
    internal Cam16(ColourTriplet source, View view)
    {
        this.source = source;
        this.view = view;
    }
    
    public Cam16(Model model, View view)
    {
        Model = model;
        this.view = view;
    }
    
    private Model GetModel()
    {
        double j, c, h, m, s, q;
        
        // steps 8 - 10 (from XYZ)
        switch (source)
        {
            case Cam16Jch:
                (j, c, h) = Jch;
                q = NegativeReflectQ(j, view);
                m = NegativeReflectM(c, view);
                s = NegativeReflectS(m, q);
                break;
            case Cam16Jmh:
                (j, m, h) = Jmh;
                q = NegativeReflectQ(j, view);
                c = NegativeReflectC(m, view);
                s = NegativeReflectS(m, q);
                break;
            case Cam16Jsh:
                (j, s, h) = Jsh;
                q = NegativeReflectQ(j, view);
                c = NegativeReflectC(s, q, view);
                m = NegativeReflectM(c, view);
                break;
            case Cam16Qch:
                (q, c, h) = Qch;
                j = NegativeReflectJ(q, view);
                m = NegativeReflectM(c, view);
                s = NegativeReflectS(m, q);
                break;
            case Cam16Qmh:
                (q, m, h) = Qmh;
                j = NegativeReflectJ(q, view);
                c = NegativeReflectC(m, view);
                s = NegativeReflectS(m, q);
                break;
            case Cam16Qsh:
                (q, s, h) = Qsh;
                j = NegativeReflectJ(q, view);
                c = NegativeReflectC(s, q, view);
                m = NegativeReflectM(c, view);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return new Model(j, c, h, m, s, q, source.Limitation);
    }
    
    internal static double NegativeReflectJ(double q, View view) => double.IsNaN(q) ? double.NaN : Math.Sign(q) * J(Math.Abs(q), view);
    internal static double NegativeReflectC(double m, View view) => double.IsNaN(m) ? double.NaN : Math.Sign(m) * C(Math.Abs(m), view);
    internal static double NegativeReflectQ(double j, View view) => double.IsNaN(j) ? double.NaN : Math.Sign(j) * Q(Math.Abs(j), view);
    internal static double NegativeReflectC(double s, double q, View view) => double.IsNaN(s) || double.IsNaN(q) ? double.NaN : Math.Sign(s) * Math.Sign(q) * C(Math.Abs(s), Math.Abs(q), view);
    internal static double NegativeReflectM(double c, View view) => double.IsNaN(c) ? double.NaN : Math.Sign(c) * M(Math.Abs(c), view);
    internal static double NegativeReflectS(double m, double q) => double.IsNaN(m) || double.IsNaN(q) ? double.NaN : Math.Sign(m) * Math.Sign(q) * S(Math.Abs(m), Math.Abs(q));
    
    private static double J(double q, View view) => 6.25 * Math.Pow(view.C * q / ((view.Aw + 4) * Math.Pow(view.Fl, 0.25)), 2);
    private static double C(double m, View view) => m / Math.Pow(view.Fl, 0.25);
    private static double Q(double j, View view) => 4 / view.C * Math.Pow(j / 100.0, 0.5) * (view.Aw + 4) * Math.Pow(view.Fl, 0.25);
    private static double C(double s, double q, View view) => Math.Pow(s / 100.0, 2) * (q / Math.Pow(view.Fl, 0.25));
    private static double M(double c, View view) => c * Math.Pow(view.Fl, 0.25);
    private static double S(double m, double q) => q == 0 ? 0 : 100 * Math.Pow(m / q, 0.5);

    private static readonly Matrix M16 = new(new[,]
    {
        { +0.401288, +0.650173, -0.051461 },
        { -0.250268, +1.204414, +0.045854 },
        { -0.002079, +0.048952, +0.953127 }
    });
    
    private static readonly Matrix ForwardStep4 = new(new[,]
    {
        { 2, 1, 1 / 20.0 },
        { 1, -12 / 11.0, 1 / 11.0 },
        { 1 / 9.0, 1 / 9.0, -2 / 9.0 },
        { 1, 1, 21 / 20.0 }
    });

    private static readonly Matrix ReverseStep4 = new Matrix(new double[,]
    {
        { 460, 451, 288 },
        { 460, -891, -261 },
        { 460, -220, -6300 }
    }).Scale(1 / 1403.0);
    
    internal static Cam16 FromXyz(Xyz xyz, WhitePoint whitePoint, View view, ChromaticAdaptor chromaticAdaptor)
    {
        // step 1
        var adaptedXyz = chromaticAdaptor.AdaptTo(xyz, whitePoint);
        var xyzMatrix = Matrix.From(adaptedXyz).Select(x => x * 100);
        var rgb = M16.Multiply(xyzMatrix).ToTriplet();

        // step 2
        var (rc, gc, bc) = (view.Dr * rgb.First, view.Dg * rgb.Second, view.Db * rgb.Third);

        // step 3
        var (ra, ga, ba) = (A(rc), A(gc), A(bc));
        double A(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            var power = Math.Pow(view.Fl * Math.Abs(input) / 100.0, 0.42);
            return 400 * Math.Sign(input) * (power / (power + 27.13));
        }

        // step 4
        var aMatrix = Matrix.From(ra, ga, ba);
        var components = ForwardStep4.Multiply(aMatrix);
        var (p2, a, b, u) = (components[0, 0], components[1, 0], components[2, 0], components[3, 0]);
        var h = ToDegrees(Math.Atan2(b, a)).WithHueModulo();

        // step 5
        var et = Hue.Cam.GetEccentricity(h);
        
        // step 6
        var achromatic = p2 * view.Nbb;

        // step 7
        var j = 100 * Math.Pow(achromatic / view.Aw, view.C * view.Z);

        // step 8
        var q = 4 / view.C * Math.Pow(j / 100.0, 0.5) * (view.Aw + 4) * Math.Pow(view.Fl, 0.25);

        // step 9
        var t = 50000 / 13.0 * view.Nc * view.Ncb * et * Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) / (u + 0.305);
        var alpha = Math.Pow(t, 0.9) * Math.Pow(1.64 - Math.Pow(0.29, view.N), 0.73);
        var c = alpha * Math.Sqrt(j / 100.0);
        var m = c * Math.Pow(view.Fl, 0.25);
        var s = 50 * Math.Sqrt(alpha * view.C / (view.Aw + 4));
        return new Cam16(new Model(j, c, h, m, s, q, xyz.Limitation), view);
    }
    
    internal static Xyz ToXyz(Cam16 cam, WhitePoint whitePoint, ChromaticAdaptor chromaticAdaptor)
    {
        var view = cam.view;
        
        var (j, c, h) = cam.Jch;
        
        // step 1 (NOTE: currently not supporting partial models - all model values will be present)
        var alpha = j == 0 ? 0 : c / Math.Sqrt(j / 100.0);
        var t = Math.Pow(alpha / Math.Pow(1.64 - Math.Pow(0.29, view.N), 0.73), 1 / 0.9);
        
        // step 2
        var et = 0.25 * (Math.Cos(ToRadians(h) + 2) + 3.8);
        var achromatic = view.Aw * Math.Pow(j / 100.0, 1 / (view.C * view.Z));
        var p1 = et * (50000 / 13.0) * view.Nc * view.Ncb;
        var p2 = achromatic / view.Nbb;
        
        // step 3
        var gamma = 23 * (p2 + 0.305) * t / 
                    (23 * p1 + 11 * t * Math.Cos(ToRadians(h)) + 108 * t * Math.Sin(ToRadians(h)));
        var a = gamma * Math.Cos(ToRadians(h));
        var b = gamma * Math.Sin(ToRadians(h));
        
        // step 4
        var components = Matrix.From(p2, a, b);
        var (ra, ga, ba) = ReverseStep4.Multiply(components).ToTriplet();
        
        // step 5
        var (rc, gc, bc) = (RgbC(ra), RgbC(ga), RgbC(ba));
        double RgbC(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            return Math.Sign(input) * (100 / view.Fl) * Math.Pow(27.13 * Math.Abs(input) / (400 - Math.Abs(input)), 1 / 0.42);
        }
        
        // step 6
        var rgbMatrix = Matrix.From(rc / view.Dr, gc / view.Dg, bc / view.Db);
        
        // step 7
        var xyzMatrix = M16.Inverse().Multiply(rgbMatrix).Select(x => x / 100.0);
        var xyz = new Xyz(xyzMatrix.ToTriplet(), whitePoint, cam.Jch.Limitation);
        return chromaticAdaptor.AdaptFrom(xyz);
    }
    
    internal static View View(WhitePoint whitePoint, double la, double yb, double c, double f, double nc)
    {
        var (xw, yw, zw) = whitePoint;
        xw *= 100;
        yw *= 100;
        zw *= 100;
            
        // step 0
        var xyzWhitePointMatrix = Matrix.From(xw, yw, zw);
        var (rw, gw, bw) = M16.Multiply(xyzWhitePointMatrix).ToTriplet();

        var d = (f * (1 - 1 / 3.6 * Math.Exp((-la - 42) / 92.0))).Clamp(0, 1);
        var (dr, dg, db) = (D(rw), D(gw), D(bw));
        double D(double input) => d * (yw / input) + 1 - d;

        var k = 1 / (5 * la + 1);
        var fl = 0.2 * Math.Pow(k, 4) * (5 * la) + 0.1 * Math.Pow(1 - Math.Pow(k, 4), 2) * CubeRoot(5 * la);
        var n = yb / yw;
        var z = 1.48 + Math.Sqrt(n);
        var nbb = 0.725 * Math.Pow(1 / n, 0.2);
        var ncb = nbb;
            
        var (rwc, gwc, bwc) = (dr * rw, dg * gw, db * bw);
        var (raw, gaw, baw) = (Aw(rwc), Aw(gwc), Aw(bwc));
        double Aw(double input)
        {
            var power = Math.Pow(fl * input / 100.0, 0.42);
            return 400 * (power / (power + 27.13)) + 0.1;
        }
            
        var aw = (2 * raw + gaw + baw / 20.0 - 0.305) * nbb;
        return new View(c, nc, dr, dg, db, fl, n, z, nbb, ncb, aw);
    }
    
    // unlikely for the CAM wrapper to be tested for equivalence
    // but the lazy-loaded field-backed properties cause complexity for the record
    // so explicitly check compare the actual model for equality
    public virtual bool Equals(Cam16? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Model.Equals(other.Model);
    }
    
    public override int GetHashCode()
    {
        return Model.GetHashCode();
    }
}