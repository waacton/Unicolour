using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public static class Cam
{
    public record Model : ColourBase
    {
        public double J => this[0];
        public double C => this[1];
        public double H => this[2];
        public string Hc { get; } // technically not in CAM02 but 🤷
        public double M => this[3];
        public double S => this[4];
        public double Q => this[5];

        public double Lightness => J;
        public double Chroma => C;
        public double HueAngle => H;
        public string HueComposition => Hc;
        public double Colourfulness => M;
        public double Saturation => S;
        public double Brightness => Q;

        // CAM by definition has hue component, so not treated as achromatic (hue is technically present, just cannot be detected due to lack of chroma)
        protected override bool HasAchromaticValues => false;
        
        // TODO: should be able to delete this when reworked?
        internal Model(double j, double c, double h, double m, double s, double q) : this(j, c, h, m, s, q, Limitation.None) {}
        internal Model(double j, double c, double h, double m, double s, double q, Limitation limitation) : base([j, c, h, m, s, q], limitation)
        {
            Hc = HueData.GetHueComposition(h);
        }

        public override string ToString() => $"{J:F2} {C:F2} {H:F1} ({Hc}) {M:F2} {S:F2} {Q:F2}";
    }

    internal record ViewingConditions(
        double C, double Nc, double Dr, double Dg, double Db, 
        double Fl, double N, double Z, double Nbb, double Ncb, double Aw)
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
    }

    internal static class HueData
    {
        private const double Angle1 = 20.14;
        private const double Angle2 = 90.00;
        private const double Angle3 = 164.25;
        private const double Angle4 = 237.53;
        private const double Angle5 = 380.14;

        private static readonly string[] Names = ["R", "Y", "G", "B", "R"];
        private static readonly double[] Angles = [Angle1, Angle2, Angle3, Angle4, Angle5];
        private static readonly double[] Es = [0.8, 0.7, 1.0, 1.2, 0.8];
        private static readonly double[] Quads = [0.0, 100.0, 200.0, 300.0, 400.0];

        private static string Name(int i) => Get(Names, i);
        private static double Angle(int i) => Get(Angles, i);
        private static double E(int i) => Get(Es, i);
        private static double Quad(int i) => Get(Quads, i);
        private static T Get<T>(IReadOnlyList<T> array, int i) => array[i - 1];

        private static double GetHPrime(double h) => h < Angle1 ? h + 360 : h;
        internal static double GetEccentricity(double h) => 0.25 * (Math.Cos(ToRadians(GetHPrime(h)) + 2) + 3.8);
        internal static string GetHueComposition(double h)
        {
            if (double.IsNaN(h)) return "-";
            var hPrime = GetHPrime(h);
            int? index = hPrime switch
            {
                >= Angle1 and < Angle2 => 1,
                >= Angle2 and < Angle3 => 2,
                >= Angle3 and < Angle4 => 3,
                >= Angle4 and < Angle5 => 4,
                _ => null
            };

            if (index == null) return "-";
            var i = index.Value;
            var hQuad = Quad(i) +
                        100 * E(i + 1) * (hPrime - Angle(i)) /
                        (E(i + 1) * (hPrime - Angle(i)) + E(i) * (Angle(i + 1) - hPrime));
            var pl = Quad(i + 1) - hQuad;
            var pr = hQuad - Quad(i);
            var hc = $"{pl:f0}{Name(i)}{pr:f0}{Name(i + 1)}";
            return hc;
        }
    }
}