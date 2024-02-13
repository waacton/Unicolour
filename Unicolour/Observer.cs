namespace Wacton.Unicolour;

public class Observer
{
    public static readonly Observer Degree2 = new(Cmf.Degree2, "1931 2\u00b0 Observer");
    public static readonly Observer Degree10 = new(Cmf.Degree10, "1964 10\u00b0 Observer");
    
    public string Name { get; }

    private readonly Cmf cmf;
    public Observer(Cmf cmf, string name = Utils.Unnamed)
    {
        this.cmf = cmf;
        Name = name;
    }
    
    internal double ColourMatchX(int wavelength) => cmf[wavelength].x;
    internal double ColourMatchY(int wavelength) => cmf[wavelength].y;
    internal double ColourMatchZ(int wavelength) => cmf[wavelength].z;
    
    public override string ToString() => Name;
}