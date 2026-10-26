using static Wacton.Unicolour.Cam;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public partial record Cam99
{
    public record QMH : ColourRepresentation
    {
        protected internal override int? HueIndex => 2;
        public double J => First;
        public double Q => Second;
        public double H => Third;
        
        protected override bool IsTripletAchromatic => Q <= 0;
        
        public QMH(double q, double m, double h) : this(q, m, h, Limitation.None) {}
        public QMH(double q) : this(q, 0, 0, Limitation.Achromatic) {}
        internal QMH(double q, double m, double h, Limitation limitation) : base(q, m, h, limitation) {}

        protected override string String => Limitation != Limitation.Achromatic ? $"{J:F2} {Q:F2} {H:F1}°" : $"{J:F2} {Q:F2} {NoHue}°";
        public override string ToString() => base.ToString();
    } 
}
