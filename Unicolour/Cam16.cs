namespace Wacton.Unicolour;

using static Utils;

// TODO: CAM16 colour difference
// - Euclidean distance between JAB --> ΔE'
// - ΔE = 1.41 * Math.Pow(ΔE', 0.63)
public record Cam16 : ColourRepresentation
{
    protected override int? HueIndex => null;
    public double J => First;
    public double A => Second;
    public double B => Third;
    public Cam16Ucs Ucs { get; }
    public Cam16Model Model { get; }

    internal override bool IsGreyscale => Model.Chroma <= 0; // presumably also A.Equals(0.0) && B.Equals(0.0)

    public Cam16(double j, double a, double b, Cam16Configuration cam16Config) : this(new Cam16Ucs(j, a, b), cam16Config, ColourMode.Unset) {}

    internal Cam16(Cam16Model model, Cam16Configuration cam16Config, ColourMode colourMode) : this(model.ToUcs(), cam16Config, colourMode)
    {
        Model = model;
    }

    internal Cam16(Cam16Ucs ucs, Cam16Configuration cam16Config, ColourMode colourMode) : base(ucs.J, ucs.A, ucs.B, colourMode)
    {
        // Model will only be non-null if the constructor that takes Cam16Model is called (currently not possible from external code)
        Ucs = ucs;
        Model ??= ucs.ToModel(ViewingConditions.Create(cam16Config)); 
    }

    protected override string FirstString => $"{J:F2}";
    protected override string SecondString => $"{A:+0.00;-0.00;0.00}";
    protected override string ThirdString => $"{B:+0.00;-0.00;0.00}";
    public override string ToString() => base.ToString();
    
    /*
     * CAM16 is a transform of XYZ 
     * Forward: https://doi.org/10.1002/col.22131 · https://doi.org/10.48550/arXiv.1802.06067
     * Reverse: https://doi.org/10.1002/col.22131 · https://doi.org/10.48550/arXiv.1802.06067
     */
    
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
    }).Scalar(x => x / 1403.0);
    
    public static Cam16 FromXyz(Xyz xyz, Cam16Configuration cam16Config, XyzConfiguration xyzConfig)
    {
        var view = ViewingConditions.Create(cam16Config);

        // Step 1: Calculate 'cone' responses
        var xyzMatrix = Matrix.FromTriplet(new(xyz.X, xyz.Y, xyz.Z));
        xyzMatrix = Adaptation.WhitePoint(xyzMatrix, xyzConfig.WhitePoint, cam16Config.WhitePoint).Scalar(x => x * 100);
        var rgb = M16.Multiply(xyzMatrix).ToTriplet();

        // Step 2: Complete the color adaptation of the illuminant in the corresponding cone response space
        var (rc, gc, bc) = (view.Dr * rgb.First, view.Dg * rgb.Second, view.Db * rgb.Third);

        // Step 3: Calculate the postadaptation cone response
        var (ra, ga, ba) = (A(rc), A(gc), A(bc));
        double A(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            var power = Math.Pow(view.Fl * Math.Abs(input) / 100.0, 0.42);
            return 400 * Math.Sign(input) * (power / (power + 27.13));
        }

        // Step 4: Calculate Redness – Greenness (a), Yellowness – Blueness (b) components, and hue angle (h):
        var aMatrix = Matrix.FromTriplet(new(ra, ga, ba));
        var components = ForwardStep4.Multiply(aMatrix);
        var (p2, a, b, u) = (components[0, 0], components[1, 0], components[2, 0], components[3, 0]);
        var h = ToDegrees(Math.Atan2(b, a)).Modulo(360);

        // Step 5: Calculate eccentricity [et, hue quadrature composition (H) and hue composition (Hc)]
        var et = HueData.GetEccentricity(h);
        
        // Step 6: Calculate achromatic response A
        var achromatic = p2 * view.Nbb;

        // Step 7: Calculate the correlate of lightness J
        var j = 100 * Math.Pow(achromatic / view.Aw, view.C * view.Z);

        // Step 8: Calculate the correlate of brightness Q
        var q = 4 / view.C * Math.Pow(j / 100.0, 0.5) * (view.Aw + 4) * Math.Pow(view.Fl, 0.25);

        // Step 9: Calculate the correlates of chroma (C), colorfulness (M), and saturation (s)
        var t = 50000 / 13.0 * view.Nc * view.Ncb * et * Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) / (u + 0.305);
        var alpha = Math.Pow(t, 0.9) * Math.Pow(1.64 - Math.Pow(0.29, view.N), 0.73);
        var c = alpha * Math.Sqrt(j / 100.0);
        var m = c * Math.Pow(view.Fl, 0.25);
        var s = 50 * Math.Sqrt(alpha * view.C / (view.Aw + 4));
        return new Cam16(new Cam16Model(j, c, h, m, s, q), cam16Config, ColourMode.FromRepresentation(xyz));
    }
    
    public static Xyz ToXyz(Cam16 cam, Cam16Configuration cam16Config, XyzConfiguration xyzConfig)
    {
        var view = ViewingConditions.Create(cam16Config);
        
        var j = cam.Model.J;
        var c = cam.Model.C;
        var h = cam.Model.H;
        
        // Step 1: Obtain J, t and h from H, Q, C, M, s
        // NOTE: currently not supporting partial models - all model values will be present
        var alpha = j == 0 ? 0 : c / Math.Sqrt(j / 100.0);
        var t = Math.Pow(alpha / Math.Pow(1.64 - Math.Pow(0.29, view.N), 0.73), 1 / 0.9);
        
        // Step 2: Calculate t, et, A, p1, and p2
        var et = 0.25 * (Math.Cos(ToRadians(h) + 2) + 3.8);
        var achromatic = view.Aw * Math.Pow(j / 100.0, 1 / (view.C * view.Z));
        var p1 = et * (50000 / 13.0) * view.Nc * view.Ncb;
        var p2 = achromatic / view.Nbb;
        
        // Step 3: Calculate a and b
        var gamma = 23 * (p2 + 0.305) * t / 
                    (23 * p1 + 11 * t * Math.Cos(ToRadians(h)) + 108 * t * Math.Sin(ToRadians(h)));
        var a = gamma * Math.Cos(ToRadians(h));
        var b = gamma * Math.Sin(ToRadians(h));
        
        // Step 4: Calculate Ra, Ga, and Ba
        var components = Matrix.FromTriplet(new(p2, a, b));
        var (ra, ga, ba) = ReverseStep4.Multiply(components).ToTriplet();
        
        // Step 5: Calculate Rc, Gc, and Bc
        var (rc, gc, bc) = (C(ra), C(ga), C(ba));
        double C(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            return Math.Sign(input) * (100 / view.Fl) * Math.Pow(27.13 * Math.Abs(input) / (400 - Math.Abs(input)), 1 / 0.42);
        }
        
        // Step 6: Calculate R, G, and B from Rc, Gc, and Bc
        var rgbMatrix = Matrix.FromTriplet(new(rc / view.Dr, gc / view.Dg, bc / view.Db));
        
        // Step 7:  Calculate X, Y, and Z
        var xyzMatrix = M16.Inverse().Multiply(rgbMatrix);
        xyzMatrix = Adaptation.WhitePoint(xyzMatrix, cam16Config.WhitePoint, xyzConfig.WhitePoint).Scalar(x => x / 100.0);
        return new Xyz(xyzMatrix.ToTriplet(), ColourMode.FromRepresentation(cam));
    }

    internal record ViewingConditions(double C, double Nc, double Dr, double Dg, double Db, double Fl, double N, double Z, double Nbb, double Ncb, double Aw)
    {
        public double C { get; } = C;
        public double Nc { get; } = Nc;
        public double Dr { get; } = Dr;
        public double Dg { get; } = Dg;
        public double Db { get; } = Db;
        public double Fl { get; } = Fl;
        public double N { get; } = N;
        public double Z { get; } = Z;
        public double Nbb { get; } = Nbb;
        public double Ncb { get; } = Ncb;
        public double Aw { get; } = Aw;
        
        public static ViewingConditions Create(Cam16Configuration cam16Config)
        {
            var (xw, yw, zw) = (cam16Config.WhitePoint.X, cam16Config.WhitePoint.Y, cam16Config.WhitePoint.Z);
            var la = cam16Config.AdaptingLuminance;
            var yb = cam16Config.BackgroundLuminance;
            var c = cam16Config.C;
            var f = cam16Config.F;
            var nc = cam16Config.Nc;
            
            // Step 0: Calculate all values/parameters which are independent of the input sample
            var xyzWhitePointMatrix = Matrix.FromTriplet(new(xw, yw, zw));
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
            return new ViewingConditions(c, nc, dr, dg, db, fl, n, z, nbb, ncb, aw);
        }
    }
}

public record Cam16Model(double J, double C, double H, double M, double S, double Q)
{
    public double J { get; } = J;
    public double C { get; } = C;
    public double H { get; } = H;
    public string Hc { get; } = HueData.GetHueComposition(H);
    public double M { get; } = M;
    public double S { get; } = S;
    public double Q { get; } = Q;

    public double Lightness => J;
    public double Chroma => C;
    public double HueAngle => H;
    public string HueComposition => Hc;
    public double Colourfulness => M;
    public double Saturation => S;
    public double Brightness => Q;
    
    internal Cam16Ucs ToUcs()
    {
        var j = 1.7 * J / (1 + 0.007 * J);
        var m = Math.Log(1 + 0.0228 * M) / 0.0228;
        (j, var a, var b) = FromLchTriplet(new(j, m, H));
        return new Cam16Ucs(j, a, b);
    }
}

public record Cam16Ucs(double J, double A, double B)
{
    public double J { get; } = J;
    public double A { get; } = A;
    public double B { get; } = B;

    internal Cam16Model ToModel(Cam16.ViewingConditions view)
    {
        var j = J / (1.7 - 0.007 * J);
        (j, var m, var h) = ToLchTriplet(j, A, B);
        m = (Math.Exp(0.0228 * m) - 1) / 0.0228;
        
        var q = 4 / view.C * Math.Pow(j / 100.0, 0.5) * (view.Aw + 4) * Math.Pow(view.Fl, 0.25);
        var c = m / Math.Pow(view.Fl, 0.25);
        var s = 100 * Math.Pow(m / q, 0.5);
        return new Cam16Model(j, c, h, m, s, q);
    }
}

internal static class HueData
{
    private const double Angle1 = 20.14;
    private const double Angle2 = 90.00;
    private const double Angle3 = 164.25;
    private const double Angle4 = 237.53;
    private const double Angle5 = 380.14;
        
    private static readonly string[] Names = { "R", "Y", "G", "B", "R" };
    private static readonly double[] Angles = {  Angle1, Angle2, Angle3, Angle4, Angle5 };
    private static readonly double[] Es = {  0.8, 0.7, 1.0, 1.2, 0.8 };
    private static readonly double[] Quads = {  0.0, 100.0, 200.0, 300.0, 400.0 };

    private static string Name(int i) => Get(Names, i);
    private static double Angle(int i) => Get(Angles, i);
    private static double E(int i) => Get(Es, i);
    private static double Quad(int i) => Get(Quads, i);
    private static T Get<T>(IReadOnlyList<T> array, int i) => array[i - 1];

    private static double GetHPrime(double h) => h < Angle1 ? h + 360 : h;
    public static double GetEccentricity(double h) => 0.25 * (Math.Cos(ToRadians(GetHPrime(h)) + 2) + 3.8);
    public static string GetHueComposition(double h)
    {
        if (double.IsNaN(h)) return "-";
        var hPrime = GetHPrime(h);
        var i = hPrime switch
        {
            >= Angle1 and < Angle2 => 1,
            >= Angle2 and < Angle3 => 2,
            >= Angle3 and < Angle4 => 3,
            >= Angle4 and < Angle5 => 4,
            _ => throw new Exception("hPrime out of range")
        };

        var hQuad = Quad(i) +
                    100 * E(i + 1) * (hPrime - Angle(i)) /
                    (E(i + 1) * (hPrime - Angle(i)) + E(i) * (Angle(i + 1) - hPrime));
        var pl = Quad(i + 1) - hQuad;
        var pr = hQuad - Quad(i);
        var hc = $"{pl:f0}{Name(i)}{pr:f0}{Name(i + 1)}";
        return hc;
    }
}
