namespace Wacton.Unicolour.Datasets;

// https://people.phy.cam.ac.uk/dag9/CUBEHELIX/
public class Cubehelix : Colourmap
{
    internal Cubehelix()
    {
    }
    
    // corresponds to the 'default' scheme
    public override Unicolour Map(double x) => Map(x, start: 0.5, rotations: -1.5, hue: 1.0, gamma: 1.0);
    public override string ToString() => nameof(Cubehelix);
    
    public static Unicolour Map(double x, double start, double rotations, double hue, double gamma)
    {
        var angle = 2 * Math.PI * (start / 3.0 + 1.0 + rotations * x);
        x = Math.Pow(x, gamma);
        var amp = hue * x * (1 - x) / 2.0;
        var r = x + amp * (-0.14861 * Math.Cos(angle) + 1.78277 * Math.Sin(angle));
        var g = x + amp * (-0.29227 * Math.Cos(angle) - 0.90649 * Math.Sin(angle));
        var b = x + amp * (1.97294 * Math.Cos(angle));
        return new Unicolour(ColourSpace.Rgb, r.Clamp(0, 1), g.Clamp(0, 1), b.Clamp(0, 1));
    }
}