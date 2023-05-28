namespace Wacton.Unicolour;

public class Configuration
{
    internal readonly Guid Id = Guid.NewGuid();
    
    public RgbConfiguration Rgb { get; }
    public XyzConfiguration Xyz { get; }
    public Cam16Configuration Cam16 { get; }
    public double IctcpScalar { get; }
    public double JzazbzScalar { get; }
    
    public static readonly Configuration Default = new();

    public Configuration(
        RgbConfiguration? rgbConfiguration = null, 
        XyzConfiguration? xyzConfiguration = null, 
        Cam16Configuration? cam16Configuration = null,
        double ictcpScalar = 100, 
        double jzazbzScalar = 100)
    {
        Rgb = rgbConfiguration ?? RgbConfiguration.StandardRgb;
        Xyz = xyzConfiguration ?? XyzConfiguration.D65;
        Cam16 = cam16Configuration ?? Cam16Configuration.StandardRgb;
        IctcpScalar = ictcpScalar;
        JzazbzScalar = jzazbzScalar;
    }
    
    public override string ToString() => $"{Id}";
}