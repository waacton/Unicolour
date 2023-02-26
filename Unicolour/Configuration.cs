namespace Wacton.Unicolour;

public class Configuration
{
    public RgbConfiguration Rgb { get; }
    public XyzConfiguration Xyz { get; }
    public double IctcpScalar { get; }
    public double JzazbzScalar { get; }
    
    public static readonly Configuration Default = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65);

    public Configuration(RgbConfiguration rgbConfiguration, XyzConfiguration xyzConfiguration, 
        double ictcpScalar = 100, double jzazbzScalar = 100)
    {
        Rgb = rgbConfiguration;
        Xyz = xyzConfiguration;
        IctcpScalar = ictcpScalar;
        JzazbzScalar = jzazbzScalar;
    }

    public override string ToString() => $"{Rgb} <-> {Xyz}";
}