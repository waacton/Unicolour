﻿namespace Wacton.Unicolour;

using static Cam;
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
    public Ucs Ucs { get; }
    public Model Model { get; }

    internal override bool IsGreyscale => Model.Chroma <= 0; // presumably also A.Equals(0.0) && B.Equals(0.0)

    public Cam16(double j, double a, double b, CamConfiguration camConfig) : this(new Ucs(j, a, b), camConfig, ColourMode.Unset) {}

    internal Cam16(Model model, CamConfiguration camConfig, ColourMode colourMode) : this(model.ToUcs(), camConfig, colourMode)
    {
        Model = model;
    }

    internal Cam16(Ucs ucs, CamConfiguration camConfig, ColourMode colourMode) : base(ucs.J, ucs.A, ucs.B, colourMode)
    {
        // Model will only be non-null if the constructor that takes Cam16Model is called (currently not possible from external code)
        Ucs = ucs;
        Model ??= ucs.ToModel(ViewingConditions(camConfig)); 
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
    }).Scale(1 / 1403.0);
    
    private static ViewingConditions ViewingConditions(CamConfiguration camConfig)
    {
        var (xw, yw, zw) = (camConfig.WhitePoint.X, camConfig.WhitePoint.Y, camConfig.WhitePoint.Z);
        var la = camConfig.AdaptingLuminance;
        var yb = camConfig.BackgroundLuminance;
        var c = camConfig.C;
        var f = camConfig.F;
        var nc = camConfig.Nc;
            
        // step 0
        var xyzWhitePointMatrix = Matrix.FromTriplet(xw, yw, zw);
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
    
    public static Cam16 FromXyz(Xyz xyz, CamConfiguration camConfig, XyzConfiguration xyzConfig)
    {
        var view = ViewingConditions(camConfig);

        // step 1
        var xyzMatrix = Matrix.FromTriplet(xyz.X, xyz.Y, xyz.Z);
        xyzMatrix = Adaptation.WhitePoint(xyzMatrix, xyzConfig.WhitePoint, camConfig.WhitePoint).Select(x => x * 100);
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
        var aMatrix = Matrix.FromTriplet(ra, ga, ba);
        var components = ForwardStep4.Multiply(aMatrix);
        var (p2, a, b, u) = (components[0, 0], components[1, 0], components[2, 0], components[3, 0]);
        var h = ToDegrees(Math.Atan2(b, a)).Modulo(360);

        // step 5
        var et = HueData.GetEccentricity(h);
        
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
        return new Cam16(new Model(j, c, h, m, s, q), camConfig, ColourMode.FromRepresentation(xyz));
    }
    
    public static Xyz ToXyz(Cam16 cam, CamConfiguration camConfig, XyzConfiguration xyzConfig)
    {
        var view = ViewingConditions(camConfig);
        
        var j = cam.Model.J;
        var c = cam.Model.C;
        var h = cam.Model.H;
        
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
        var components = Matrix.FromTriplet(p2, a, b);
        var (ra, ga, ba) = ReverseStep4.Multiply(components).ToTriplet();
        
        // step 5
        var (rc, gc, bc) = (C(ra), C(ga), C(ba));
        double C(double input)
        {
            if (double.IsNaN(input)) return double.NaN;
            return Math.Sign(input) * (100 / view.Fl) * Math.Pow(27.13 * Math.Abs(input) / (400 - Math.Abs(input)), 1 / 0.42);
        }
        
        // step 6
        var rgbMatrix = Matrix.FromTriplet(rc / view.Dr, gc / view.Dg, bc / view.Db);
        
        // step 7
        var xyzMatrix = M16.Inverse().Multiply(rgbMatrix);
        xyzMatrix = Adaptation.WhitePoint(xyzMatrix, camConfig.WhitePoint, xyzConfig.WhitePoint).Select(x => x / 100.0);
        return new Xyz(xyzMatrix.ToTriplet(), ColourMode.FromRepresentation(cam));
    }
}
