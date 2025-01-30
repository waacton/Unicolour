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
        RgbConfiguration? rgbConfig = null, 
        XyzConfiguration? xyzConfig = null, 
        YbrConfiguration? ybrConfig = null,
        CamConfiguration? camConfig = null,
        IccConfiguration? iccConfig = null,
        double ictcpScalar = 100, 
        double jzazbzScalar = 100)
    {
        Rgb = rgbConfig ?? RgbConfiguration.StandardRgb;
        Xyz = xyzConfig ?? XyzConfiguration.D65;
        Ybr = ybrConfig ?? YbrConfiguration.Rec601;
        Cam = camConfig ?? CamConfiguration.StandardRgb;
        Icc = iccConfig ?? IccConfiguration.None;
        IctcpScalar = ictcpScalar;
        JzazbzScalar = jzazbzScalar;
    }
    
    public override string ToString() => $"RGB:[{Rgb.Name}] · XYZ:[{Xyz.Name}] · YBR:[{Ybr.Name}] · CAM:[{Cam.Name}] · ICC:[{Icc.Name}] · Id:[{Id}]";
}