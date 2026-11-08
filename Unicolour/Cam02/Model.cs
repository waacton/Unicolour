namespace Wacton.Unicolour.Cam02;

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

    public Model(double j, double c, double h, double m, double s, double q) : this(j, c, h, m, s, q, Limitation.None) {}
    internal Model(double j, double c, double h, double m, double s, double q, Limitation limitation = Limitation.None) : base([j, c, h, m, s, q], limitation)
    {
        Hc = Hue.Cam.GetHueComposition(h);
    }

    public override string ToString() => $"{J:F2} {C:F2} {H:F1} ({Hc}) {M:F2} {S:F2} {Q:F2}";
}