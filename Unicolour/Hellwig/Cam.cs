using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Hellwig;

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
    
    public Cam(Model model, ViewingConditions view)
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
            case Hellwig.Jch:
                (j, c, h) = Jch;
                q = NegativeReflectQ(j, view);
                m = NegativeReflectM(c, view);
                s = NegativeReflectS(m, q);
                break;
            case Hellwig.Jmh:
                (j, m, h) = Jmh;
                q = NegativeReflectQ(j, view);
                c = NegativeReflectC(m, view);
                s = NegativeReflectS(m, q);
                break;
            case Hellwig.Jsh:
                (j, s, h) = Jsh;
                q = NegativeReflectQ(j, view);
                m = NegativeReflectM(s, q);
                c = NegativeReflectC(m, view);
                break;
            case Hellwig.Qch:
                (q, c, h) = Qch;
                j = NegativeReflectJ(q, view);
                m = NegativeReflectM(c, view);
                s = NegativeReflectS(m, q);
                break;
            case Hellwig.Qmh:
                (q, m, h) = Qmh;
                j = NegativeReflectJ(q, view);
                c = NegativeReflectC(m, view);
                s = NegativeReflectS(m, q);
                break;
            case Hellwig.Qsh:
                (q, s, h) = Qsh;
                j = NegativeReflectJ(q, view);
                m = NegativeReflectM(s, q);
                c = NegativeReflectC(m, view);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return new Model(j, c, h, m, s, q, source.Limitation);
    }
    
    internal static double NegativeReflectJ(double q, ViewingConditions view) => double.IsNaN(q) ? double.NaN : Math.Sign(q) * J(Math.Abs(q), view);
    internal static double NegativeReflectC(double m, ViewingConditions view) => double.IsNaN(m) ? double.NaN : Math.Sign(m) * C(Math.Abs(m), view);
    internal static double NegativeReflectQ(double j, ViewingConditions view) => double.IsNaN(j) ? double.NaN : Math.Sign(j) * Q(Math.Abs(j), view);
    internal static double NegativeReflectM(double c, ViewingConditions view) => double.IsNaN(c) ? double.NaN : Math.Sign(c) * M(Math.Abs(c), view);
    internal static double NegativeReflectM(double s, double q) => double.IsNaN(s) || double.IsNaN(q) ? double.NaN : Math.Sign(s) * Math.Sign(q) * M(Math.Abs(s), Math.Abs(q));
    internal static double NegativeReflectS(double m, double q) => double.IsNaN(m) || double.IsNaN(q) ? double.NaN : Math.Sign(m) * Math.Sign(q) * S(Math.Abs(m), Math.Abs(q));
    
    private static double J(double q, ViewingConditions view) => 50 * view.C * q / view.Aw;
    private static double C(double m, ViewingConditions view) => 35 * (m / view.Aw);
    private static double Q(double j, ViewingConditions view) => 2 / view.C * (j / 100.0) * view.Aw;
    private static double M(double c, ViewingConditions view) => c * view.Aw / 35.0;
    private static double M(double s, double q) => s * q / 100.0;
    private static double S(double m, double q) => q == 0 ? 0 : 100 * m / q;

    internal static readonly Matrix M16 = new(new[,]
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
    
    internal static Cam FromXyz(Xyz xyz, WhitePoint whitePoint, ViewingConditions view, ChromaticAdaptor chromaticAdaptor)
    {
        // step 1
        var adaptedXyz = chromaticAdaptor.AdaptTo(xyz, whitePoint);
        var xyzMatrix = Matrix.From(adaptedXyz).Select(x => x * 100);
        var rgb = M16.Multiply(xyzMatrix).ToTriplet();

        // step 2
        var (rc, gc, bc) = (view.Dr * rgb.First, view.Dg * rgb.Second, view.Db * rgb.Third);

        // step 3
        var (ra, ga, ba) = (RgbA(rc), RgbA(gc), RgbA(bc));
        double RgbA(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            var power = Math.Pow(view.Fl * Math.Abs(input) / 100.0, 0.42);
            return 400 * Math.Sign(input) * (power / (power + 27.13));
        }

        // step 4
        var aMatrix = Matrix.From(ra, ga, ba);
        var components = ForwardStep4.Multiply(aMatrix);
        var (p2, a, b, _) = (components[0, 0], components[1, 0], components[2, 0], components[3, 0]);
        var hRad = Math.Atan2(b, a);
        var h = ToDegrees(hRad).WithHueModulo();

        // step 5 (different to CAM16)
        var et = -0.0582 * Math.Cos(hRad) - 0.0258 * Math.Cos(2 * hRad) - 0.1347 * Math.Cos(3 * hRad)
                 + 0.0289 * Math.Cos(4 * hRad) - 0.1475 * Math.Sin(hRad) - 0.0308 * Math.Sin(2 * hRad)
                 + 0.0385 * Math.Sin(3 * hRad) + 0.0096 * Math.Sin(4 * hRad) + 1;
        
        // step 6 (different to CAM16)
        var achromatic = p2;
        
        // step 7
        var j = 100 * Math.Pow(achromatic / view.Aw, view.C * view.Z);
        
        // step 8 (different to CAM16)
        var q = 2 / view.C * (j / 100.0) * view.Aw;

        // step 9 (different to CAM16)
        var m = 43 * view.Nc * et * Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        var c = 35 * (m / view.Aw);
        var s = 100 * (m / q);
        return new Cam(new Model(j, c, h, m, s, q, xyz.Limitation), view);
    }
    
    internal static Xyz ToXyz(Cam cam, WhitePoint whitePoint, ChromaticAdaptor chromaticAdaptor)
    {
        var view = cam.view;
        
        // step 1
        var (j, m, h) = cam.Jmh;
        
        // step 2 (different to CAM16)
        var hRad = ToRadians(h);
        var et = -0.0582 * Math.Cos(hRad) - 0.0258 * Math.Cos(2 * hRad) - 0.1347 * Math.Cos(3 * hRad)
                 + 0.0289 * Math.Cos(4 * hRad) - 0.1475 * Math.Sin(hRad) - 0.0308 * Math.Sin(2 * hRad)
                 + 0.0385 * Math.Sin(3 * hRad) + 0.0096 * Math.Sin(4 * hRad) + 1;
        
        var achromatic = view.Aw * Math.Pow(j / 100.0, 1 / (view.C * view.Z));
        var p1 = 43 * view.Nc * et;
        
        // step 3 (different to CAM16)
        var gamma = m / p1;
        var a = gamma * Math.Cos(hRad);
        var b = gamma * Math.Sin(hRad);
        
        // step 4
        var components = Matrix.From(achromatic, a, b);
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