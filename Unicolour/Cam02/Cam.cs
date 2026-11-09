using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Cam02;

public record Cam
{
    private readonly ColourTriplet? source;
    private readonly ViewingConditions view;
    
    public Jch Jch => field ??= source as Jch ?? new Jch(Model.J, Model.C, Model.H, Model.Limitation);
    public Jmh Jmh => field ??= source as Jmh ?? new Jmh(Model.J, Model.M, Model.H, Model.Limitation);
    public Jsh Jsh => field ??= source as Jsh ?? new Jsh(Model.J, Model.S, Model.H, Model.Limitation);
    public Qch Qch => field ??= source as Qch ?? new Qch(Model.Q, Model.C, Model.H, Model.Limitation);
    public Qmh Qmh => field ??= source as Qmh ?? new Qmh(Model.Q, Model.M, Model.H, Model.Limitation);
    public Qsh Qsh => field ??= source as Qsh ?? new Qsh(Model.Q, Model.S, Model.H, Model.Limitation);
    public Model Model => field ??= GetModel();
    
    public Cam(Jch jch, ViewingConditions view) : this((ColourTriplet)jch, view) {}
    public Cam(Jmh jmh, ViewingConditions view) : this((ColourTriplet)jmh, view) {}
    public Cam(Jsh jsh, ViewingConditions view) : this((ColourTriplet)jsh, view) {}
    public Cam(Qch qch, ViewingConditions view) : this((ColourTriplet)qch, view) {}
    public Cam(Qmh qmh, ViewingConditions view) : this((ColourTriplet)qmh, view) {}
    public Cam(Qsh qsh, ViewingConditions view) : this((ColourTriplet)qsh, view) {}
    internal Cam(ColourTriplet source, ViewingConditions view)
    {
        this.source = source;
        this.view = view;
    }
    
    internal Cam(Model model, ViewingConditions view)
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
            case Cam02.Jch:
                (j, c, h) = Jch;
                q = NegativeReflectQ(j, view);
                m = NegativeReflectM(c, view);
                s = NegativeReflectS(m, q);
                break;
            case Cam02.Jmh:
                (j, m, h) = Jmh;
                q = NegativeReflectQ(j, view);
                c = NegativeReflectC(m, view);
                s = NegativeReflectS(m, q);
                break;
            case Cam02.Jsh:
                (j, s, h) = Jsh;
                q = NegativeReflectQ(j, view);
                c = NegativeReflectC(s, q, view);
                m = NegativeReflectM(c, view);
                break;
            case Cam02.Qch:
                (q, c, h) = Qch;
                j = NegativeReflectJ(q, view);
                m = NegativeReflectM(c, view);
                s = NegativeReflectS(m, q);
                break;
            case Cam02.Qmh:
                (q, m, h) = Qmh;
                j = NegativeReflectJ(q, view);
                c = NegativeReflectC(m, view);
                s = NegativeReflectS(m, q);
                break;
            case Cam02.Qsh:
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

    internal static double NegativeReflectJ(double q, ViewingConditions view) => double.IsNaN(q) ? double.NaN : Math.Sign(q) * J(Math.Abs(q), view);
    internal static double NegativeReflectC(double m, ViewingConditions view) => double.IsNaN(m) ? double.NaN : Math.Sign(m) * C(Math.Abs(m), view);
    internal static double NegativeReflectQ(double j, ViewingConditions view) => double.IsNaN(j) ? double.NaN : Math.Sign(j) * Q(Math.Abs(j), view);
    internal static double NegativeReflectC(double s, double q, ViewingConditions view) => double.IsNaN(s) || double.IsNaN(q) ? double.NaN : Math.Sign(s) * Math.Sign(q) * C(Math.Abs(s), Math.Abs(q), view);
    internal static double NegativeReflectS(double m, double q) => double.IsNaN(m) || double.IsNaN(q) ? double.NaN : Math.Sign(m) * Math.Sign(q) * S(Math.Abs(m), Math.Abs(q));
    internal static double NegativeReflectM(double c, ViewingConditions view) => double.IsNaN(c) ? double.NaN : Math.Sign(c) * M(Math.Abs(c), view);
    
    private static double J(double q, ViewingConditions view) => 6.25 * Math.Pow(view.C * q / ((view.Aw + 4) * Math.Pow(view.Fl, 0.25)), 2);
    private static double C(double m, ViewingConditions view) => m / Math.Pow(view.Fl, 0.25);
    private static double Q(double j, ViewingConditions view) => 4 / view.C * Math.Pow(j / 100.0, 0.5) * (view.Aw + 4) * Math.Pow(view.Fl, 0.25);
    private static double C(double s, double q, ViewingConditions view) => Math.Pow(s / 100.0, 2) * (q / Math.Pow(view.Fl, 0.25));
    private static double S(double m, double q) => q == 0 ? 0 : 100 * Math.Pow(m / q, 0.5);
    private static double M(double c, ViewingConditions view) => c * Math.Pow(view.Fl, 0.25);

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
    
    internal static Cam FromXyz(Xyz xyz, WhitePoint whitePoint, ViewingConditions view, ChromaticAdaptor chromaticAdaptor)
    {
        // step 1
        var adaptedXyz = chromaticAdaptor.AdaptTo(xyz, whitePoint);
        var xyzMatrix = Matrix.From(adaptedXyz).Select(x => x * 100);
        var rgb = MCAT02.Multiply(xyzMatrix).ToTriplet();

        // step 2
        var cMatrix = Matrix.From(view.Dr * rgb.First, view.Dg * rgb.Second, view.Db * rgb.Third);
        
        // step 3
        var (rPrime, gPrime, bPrime) = MHPE.Multiply(MCAT02.Inverse()).Multiply(cMatrix).ToTriplet();

        // step 4
        var (ra, ga, ba) = (RgbA(rPrime), RgbA(gPrime), RgbA(bPrime));
        double RgbA(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            var power = Math.Pow(view.Fl * Math.Abs(input) / 100.0, 0.42);
            return Math.Sign(input) * 400 * (power / (power + 27.13));
        }

        // step 5
        var aMatrix = Matrix.From(ra, ga, ba);
        var components = ForwardStep4.Multiply(aMatrix);
        var (p2, a, b, u) = (components[0, 0], components[1, 0], components[2, 0], components[3, 0]);
        var h = ToDegrees(Math.Atan2(b, a)).WithHueModulo();

        // step 6
        var et = Hue.Cam.GetEccentricity(h);

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

        return new Cam(new Model(j, c, h, m, s, q, xyz.Limitation), view);
    }
    
    internal static Xyz ToXyz(Cam cam, WhitePoint whitePoint, ChromaticAdaptor chromaticAdaptor)
    {
        var view = cam.view;
        
        // step 1
        var (j, c, h) = cam.Jch;
        var alpha = j == 0 ? 0 : c / Math.Sqrt(j / 100.0);
        var t = Math.Pow(alpha / Math.Pow(1.64 - Math.Pow(0.29, view.N), 0.73), 1 / 0.9);
        
        // step 2
        var hRad = ToRadians(h);
        var et = 0.25 * (Math.Cos(hRad + 2) + 3.8);
        var achromatic = view.Aw * Math.Pow(j / 100.0, 1 / (view.C * view.Z));
        var p1 = et * (50000 / 13.0) * view.Nc * view.Ncb;
        var p2 = achromatic / view.Nbb;
        
        // step 3
        var gamma = 23 * (p2 + 0.305) * t / 
                    (23 * p1 + 11 * t * Math.Cos(hRad) + 108 * t * Math.Sin(hRad));
        var a = gamma * Math.Cos(hRad);
        var b = gamma * Math.Sin(hRad);
        
        // step 4
        var components = Matrix.From(p2, a, b);
        var (ra, ga, ba) = ReverseStep4.Multiply(components).ToTriplet();
        
        // step 5
        var primeMatrix = Matrix.From(RgbPrime(ra), RgbPrime(ga), RgbPrime(ba));
        double RgbPrime(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            return Math.Sign(input) * (100 / view.Fl) * Math.Pow(27.13 * Math.Abs(input) / (400 - Math.Abs(input)), 1 / 0.42);
        }
        
        var (rc, gc, bc) = MCAT02.Multiply(MHPE.Inverse()).Multiply(primeMatrix).ToTriplet();
        
        // step 6
        var rgbMatrix = Matrix.From(rc / view.Dr, gc / view.Dg, bc / view.Db);
        
        // step 7
        var xyzMatrix = MCAT02.Inverse().Multiply(rgbMatrix).Select(x => x / 100.0);
        var xyz = new Xyz(xyzMatrix.ToTriplet(), whitePoint, cam.Jch.Limitation);
        return chromaticAdaptor.AdaptFrom(xyz);
    }
    
    // unlikely for the CAM wrapper to be tested for equivalence
    // but the lazy-loaded field-backed properties cause complexity for the record
    // so explicitly check compare the actual model for equality
    public virtual bool Equals(Cam? other)
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