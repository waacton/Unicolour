namespace Wacton.Unicolour;

public class Configuration
{
    internal readonly Guid Id = Guid.NewGuid();
    
    public RgbConfiguration Rgb { get; }
    public XyzConfiguration Xyz { get; }
    public YbrConfiguration Ybr { get; }
    public CamConfiguration Cam { get; }
    public DynamicRange DynamicRange { get; }
    public IccConfiguration Icc { get; }
    
    public static readonly Configuration Default = new();

    public Configuration(
        RgbConfiguration? rgbConfig = null, 
        XyzConfiguration? xyzConfig = null, 
        YbrConfiguration? ybrConfig = null,
        CamConfiguration? camConfig = null,
        DynamicRange? dynamicRange = null,
        IccConfiguration? iccConfig = null)
    {
        Rgb = rgbConfig ?? RgbConfiguration.StandardRgb;
        Xyz = xyzConfig ?? XyzConfiguration.D65;
        Ybr = ybrConfig ?? YbrConfiguration.Rec601;
        Cam = camConfig ?? CamConfiguration.StandardRgb;
        DynamicRange = dynamicRange ?? DynamicRange.High;
        Icc = iccConfig ?? IccConfiguration.None;
    }
    
    public override string ToString() => $"RGB:[{Rgb.Name}] · XYZ:[{Xyz.Name}] · YBR:[{Ybr.Name}] · CAM:[{Cam.Name}] · ICC:[{Icc.Name}] · Id:[{Id}]";
}