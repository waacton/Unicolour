namespace Wacton.Unicolour;

public class Configuration
{
    internal readonly Guid Id = Guid.NewGuid();
    
    public RgbConfiguration Rgb { get; }
    public XyzConfiguration Xyz { get; }
    public YbrConfiguration Ybr { get; }
    public CamConfiguration Cam { get; }
    public IccConfiguration Icc { get; }
    public double IctcpScalar { get; }
    public double JzazbzScalar { get; }
    
    public static readonly Configuration Default = new();

    public Configuration(
        RgbConfiguration? rgbConfiguration = null, 
        XyzConfiguration? xyzConfiguration = null, 
        YbrConfiguration? ybrConfiguration = null,
        CamConfiguration? camConfiguration = null,
        IccConfiguration? iccConfiguration = null,
        double ictcpScalar = 100, 
        double jzazbzScalar = 100)
    {
        Rgb = rgbConfiguration ?? RgbConfiguration.StandardRgb;
        Xyz = xyzConfiguration ?? XyzConfiguration.D65;
        Ybr = ybrConfiguration ?? YbrConfiguration.Rec601;
        Cam = camConfiguration ?? CamConfiguration.StandardRgb;
        Icc = iccConfiguration ?? IccConfiguration.None;
        IctcpScalar = ictcpScalar;
        JzazbzScalar = jzazbzScalar;
    }
    
    public override string ToString() => $"RGB:[{Rgb.Name}] · XYZ:[{Xyz.Name}] · YBR:[{Ybr.Name}] · CAM:[{Cam.Name}] · ICC:[{Icc.Name}] · Id:[{Id}]";
}