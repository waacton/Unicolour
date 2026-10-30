using static Wacton.Unicolour.Cam;
using static Wacton.Unicolour.Utils;
using Jch = Wacton.Unicolour.Cam02Jch;
using Jmh = Wacton.Unicolour.Cam02Jmh;
using Jsh = Wacton.Unicolour.Cam02Jsh;
using Qch = Wacton.Unicolour.Cam02Qch;
using Qmh = Wacton.Unicolour.Cam02Qmh;
using Qsh = Wacton.Unicolour.Cam02Qsh;

namespace Wacton.Unicolour;

public record Cam02
{
    private readonly ColourRepresentation? source;
    private readonly ViewingConditions view;
    
    public Model Model => field ??= GetModel();
    public Jch Jch => field ??= source as Jch ?? new Jch(Model.J, Model.C, Model.H, Model.Limitation);
    public Jmh Jmh => field ??= source as Jmh ?? new Jmh(Model.J, Model.M, Model.H, Model.Limitation);
    public Jsh Jsh => field ??= source as Jsh ?? new Jsh(Model.J, Model.S, Model.H, Model.Limitation);
    public Qch Qch => field ??= source as Qch ?? new Qch(Model.Q, Model.C, Model.H, Model.Limitation);
    public Qmh Qmh => field ??= source as Qmh ?? new Qmh(Model.Q, Model.M, Model.H, Model.Limitation);
    public Qsh Qsh => field ??= source as Qsh ?? new Qsh(Model.Q, Model.S, Model.H, Model.Limitation);
    
    internal Cam02(Model model, ViewingConditions viewingConditions)
    {
        // Console.WriteLine($">>> INITIALISING FULL MODEL FROM XYZ CONVERSION ({model})");
        Model = model;
        view = viewingConditions;
    }
    
    internal Cam02(ColourRepresentation source, ViewingConditions viewingConditions)
    {
        this.source = source;
        view = viewingConditions;
    }
    
    private Model GetModel()
    {
        // Console.WriteLine($">>> INITIALISING MODEL FROM {source.GetType()} ({source})");
        double j, c, h, m, s, q;

        // steps 8 - 10 (from XYZ)
        switch (source)
        {
            case Cam02Jch:
                (j, c, h) = Jch;
                q = Q(j);
                m = M(c);
                s = S(m, q);
                break;
            case Cam02Jmh:
                (j, m, h) = Jmh;
                q = Q(j);
                c = C(m);
                s = S(m, q);
                break;
            case Cam02Jsh:
                (j, s, h) = Jsh;
                q = Q(j);
                c = C(s, q);
                m = M(c);
                break;
            case Cam02Qch:
                (q, c, h) = Qch;
                j = J(q);
                m = M(c);
                s = S(m, q);
                break;
            case Cam02Qmh:
                (q, m, h) = Qmh;
                j = J(q);
                c = C(m);
                s = S(m, q);
                break;
            case Cam02Qsh:
                (q, s, h) = Qsh;
                j = J(q);
                c = C(s, q);
                m = M(c);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return new Model(j, c, h, m, s, q, source.Limitation);
    }
    
    private double Q(double j) => 4 / view.C * Math.Pow(j / 100.0, 0.5) * (view.Aw + 4) * Math.Pow(view.Fl, 0.25);
    private double J(double q) => Math.Pow(q / Math.Pow(view.Fl, 0.25) / (view.Aw + 4) / (4 / view.C), 1 / 0.5) * 100;
    private double M(double c) => c * Math.Pow(view.Fl, 0.25);
    private double S(double m, double q) => q == 0 ? 0 : 100 * Math.Pow(m / q, 0.5);
    private double C(double m) => m / Math.Pow(view.Fl, 0.25);
    private double C(double s, double q) => Math.Pow(s / 100.0, 2) * (q / Math.Pow(view.Fl, 0.25));

    internal static readonly Matrix MCAT02 = new(new[,]
    {
        { +0.7328, +0.4296, -0.1624 },
        { -0.7036, +1.6975, +0.0061 },
        { +0.0030, +0.0136, +0.9834 }
    });
    
    internal static readonly Matrix MHPE = new(new[,]
    {
        { +0.38971, +0.68898, -0.07868 },
        { -0.22981, +1.18340, +0.04641 },
        { 0.00000, 0.00000, +1.00000 }
    });
    
    internal static readonly Matrix ForwardStep4 = new(new[,]
    {
        { 2, 1, 1 / 20.0 },
        { 1, -12 / 11.0, 1 / 11.0 },
        { 1 / 9.0, 1 / 9.0, -2 / 9.0 },
        { 1, 1, 21 / 20.0 }
    });

    internal static readonly Matrix ReverseStep4 = new Matrix(new double[,]
    {
        { 460, 451, 288 },
        { 460, -891, -261 },
        { 460, -220, -6300 }
    }).Scale(1 / 1403.0);
    
    internal static Cam02 FromXyz(Xyz xyz, WhitePoint whitePoint, ViewingConditions view, ChromaticAdaptor chromaticAdaptor)
    {
        // step 1
        var adaptedXyz = chromaticAdaptor.AdaptTo(xyz, whitePoint);
        var xyzMatrix = Matrix.From(adaptedXyz).Select(x => x * 100);
        var rgb = MCAT02.Multiply(xyzMatrix).ToTriplet();

        // step 2
        var cMatrix = Matrix.From(view.Dr * rgb.First, view.Dg * rgb.Second, view.Db * rgb.Third);
        
        // step 3, slightly different to CAM16
        var (rPrime, gPrime, bPrime) = MHPE.Multiply(MCAT02.Inverse()).Multiply(cMatrix).ToTriplet();

        // step 4
        var (ra, ga, ba) = (A(rPrime), A(gPrime), A(bPrime));
        double A(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            var power = Math.Pow(view.Fl * Math.Abs(input) / 100.0, 0.42);
            return 400 * Math.Sign(input) * (power / (power + 27.13));
        }

        // step 5
        var aMatrix = Matrix.From(ra, ga, ba);
        var components = ForwardStep4.Multiply(aMatrix);
        var (p2, a, b, u) = (components[0, 0], components[1, 0], components[2, 0], components[3, 0]);
        var h = ToDegrees(Math.Atan2(b, a)).WithHueModulo();

        // step 6
        var et = HueData.GetEccentricity(h);

        // step 7
        var achromatic = p2 * view.Nbb;

        // step 8
        var j = 100 * Math.Pow(achromatic / view.Aw, view.C * view.Z);

        // step 9
        var q = 4 / view.C * Math.Pow(j / 100.0, 0.5) * (view.Aw + 4) * Math.Pow(view.Fl, 0.25);

        // step 10
        var t = 50000 / 13.0 * view.Nc * view.Ncb * et * Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) / (u + 0.305);
        var alpha = Math.Pow(t, 0.9) * Math.Pow(1.64 - Math.Pow(0.29, view.N), 0.73);
        var c = alpha * Math.Sqrt(j / 100.0);
        var m = c * Math.Pow(view.Fl, 0.25);
        var s = 50 * Math.Sqrt(alpha * view.C / (view.Aw + 4));

        return new Cam02(new Model(j, c, h, m, s, q, xyz.Limitation), view);
    }
    
    internal static Xyz ToXyz(Cam02 cam, WhitePoint whitePoint, ChromaticAdaptor chromaticAdaptor)
    {
        var view = cam.view;
        
        var (j, c, h) = cam.Jch;
        
        // step 1
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
        
        // step 5, slightly different to CAM16
        var primeMatrix = Matrix.From(C(ra), C(ga), C(ba));
        double C(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            return Math.Sign(input) * (100 / view.Fl) * Math.Pow(27.13 * Math.Abs(input) / (400 - Math.Abs(input)), 1 / 0.42);
        }
        
        var (rc, gc, bc) = MCAT02.Multiply(MHPE.Inverse()).Multiply(primeMatrix).ToTriplet();
        
        // step 6
        var rgbMatrix = Matrix.From(rc / view.Dr, gc / view.Dg, bc / view.Db);
        
        // step 7
        var xyzMatrix = MCAT02.Inverse().Multiply(rgbMatrix).Select(x => x / 100.0);
        var xyz = new Xyz(xyzMatrix.ToTriplet(), whitePoint, cam.Jch.Limitation); // TODO: think carefully about where limitation comes from in CAM
        return chromaticAdaptor.AdaptFrom(xyz);
    }
    
    internal static ViewingConditions ViewingConditions(WhitePoint whitePoint, double la, double yb, double c, double f, double nc)
    {
        var (xw, yw, zw) = whitePoint;
        xw *= 100;
        yw *= 100;
        zw *= 100;
            
        // step 0
        var xyzWhitePointMatrix = Matrix.From(xw, yw, zw);
        var (rw, gw, bw) = MCAT02.Multiply(xyzWhitePointMatrix).ToTriplet();

        var d = (f * (1 - 1 / 3.6 * Math.Exp((-la - 42) / 92.0))).Clamp(0, 1);
        var (dr, dg, db) = (D(rw), D(gw), D(bw));
        double D(double input) => d * (yw / input) + 1 - d;

        var k = 1 / (5 * la + 1);
        var fl = 0.2 * Math.Pow(k, 4) * (5 * la) + 0.1 * Math.Pow(1 - Math.Pow(k, 4), 2) * CubeRoot(5 * la);
        var n = yb / yw;
        var z = 1.48 + Math.Sqrt(n);
        var nbb = 0.725 * Math.Pow(1 / n, 0.2);
        var ncb = nbb;
            
        // slightly different to CAM16
        var wcMatrix = Matrix.From(dr * rw, dg * gw, db * bw);
        var (rwPrime, gwPrime, bwPrime) = MHPE.Multiply(MCAT02.Inverse()).Multiply(wcMatrix).ToTriplet();
            
        var (raw, gaw, baw) = (Aw(rwPrime), Aw(gwPrime), Aw(bwPrime));
        double Aw(double input)
        {
            var power = Math.Pow(fl * input / 100.0, 0.42);
            return 400 * (power / (power + 27.13)) + 0.1;
        }
            
        var aw = (2 * raw + gaw + baw / 20.0 - 0.305) * nbb;
        return new ViewingConditions(c, nc, dr, dg, db, fl, n, z, nbb, ncb, aw);
    }
    
    // unlikely for the CAM wrapper to be tested for equivalence
    // but the lazy-loaded field-backed properties cause complexity for the record
    // so explicitly check compare the actual model for equality
    public virtual bool Equals(Cam02? other)
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