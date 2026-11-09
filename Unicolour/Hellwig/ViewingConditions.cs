using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour.Hellwig;

public record ViewingConditions
{
    public double C { get; }
    public double Nc { get; }
    public double Dr { get; }
    public double Dg { get; }
    public double Db { get; }
    public double Fl { get; }
    public double N { get; }
    public double Z { get; }
    public double Aw { get; }
    
    public ViewingConditions(WhitePoint whitePoint, double la, double yb, double c, double f, double nc)
    {
        var (xw, yw, zw) = whitePoint;
        xw *= 100;
        yw *= 100;
        zw *= 100;
            
        // step 0
        var xyzWhitePointMatrix = Matrix.From(xw, yw, zw);
        var (rw, gw, bw) = Cam.M16.Multiply(xyzWhitePointMatrix).ToTriplet();

        var d = (f * (1 - 1 / 3.6 * Math.Exp((-la - 42) / 92.0))).Clamp(0, 1);
        var (dr, dg, db) = (D(rw), D(gw), D(bw));
        double D(double input) => d * (yw / input) + 1 - d;

        var k = 1 / (5 * la + 1);
        var fl = 0.2 * Math.Pow(k, 4) * (5 * la) + 0.1 * Math.Pow(1 - Math.Pow(k, 4), 2) * CubeRoot(5 * la);
        var n = yb / yw;
        var z = 1.48 + Math.Sqrt(n);
        
        var (rwc, gwc, bwc) = (dr * rw, dg * gw, db * bw);
        var (raw, gaw, baw) = (RgbAw(rwc), RgbAw(gwc), RgbAw(bwc));
        double RgbAw(double input)
        {
            var power = Math.Pow(fl * input / 100.0, 0.42);
            return 400 * (power / (power + 27.13)) + 0.1;
        }

        var aw = 2 * raw + gaw + 0.05 * baw - 0.305;

        C = c;
        Nc = nc;
        Dr = dr;
        Dg = dg;
        Db = db;
        Fl = fl;    
        N = n;    
        Z = z;
        Aw = aw;
    }
}