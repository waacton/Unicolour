using static Wacton.Unicolour.Cam;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public partial record Cam99
{
    public record JMH : ColourRepresentation
    {
        protected internal override int? HueIndex => 2;
        public double J => First;
        public double M => Second;
        public double H => Third;
        
        protected override bool IsTripletAchromatic => M <= 0;
        
        public JMH(double j, double m, double h) : this(j, m, h, Limitation.None) {}
        public JMH(double j) : this(j, 0, 0, Limitation.Achromatic) {}
        internal JMH(double j, double m, double h, Limitation limitation) : base(j, m, h, limitation) {}

        protected override string String => Limitation != Limitation.Achromatic ? $"{J:F2} {M:F2} {H:F1}°" : $"{J:F2} {M:F2} {NoHue}°";
        public override string ToString() => base.ToString();
    } 
}
