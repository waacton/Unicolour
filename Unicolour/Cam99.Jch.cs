using static Wacton.Unicolour.Cam;
using static Wacton.Unicolour.Utils;

namespace Wacton.Unicolour;

public partial record Cam99
{
    public record JCH : ColourRepresentation
    {
        protected internal override int? HueIndex => 2;
        public double J => First;
        public double C => Second;
        public double H => Third;
        
        protected override bool IsTripletAchromatic => C <= 0;
        
        public JCH(double j, double c, double h) : this(j, c, h, Limitation.None) {}
        public JCH(double j) : this(j, 0, 0, Limitation.Achromatic) {}
        internal JCH(double j, double c, double h, Limitation limitation) : base(j, c, h, limitation) {}

        protected override string String => Limitation != Limitation.Achromatic ? $"{J:F2} {C:F2} {H:F1}°" : $"{J:F2} {C:F2} {NoHue}°";
        public override string ToString() => base.ToString();
    } 
}
