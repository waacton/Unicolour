namespace Wacton.Unicolour;

public class XyzConfiguration
{
    public static readonly XyzConfiguration D65 = new(Illuminant.D65, Observer.Degree2, nameof(D65));
    public static readonly XyzConfiguration D50 = new(Illuminant.D50, Observer.Degree2, nameof(D50));
    
    public WhitePoint WhitePoint { get; }
    public Chromaticity WhiteChromaticity => WhitePoint.ToChromaticity();
    public Observer Observer { get; }
    internal Illuminant? Illuminant { get; }
    internal SpectralBoundary SpectralBoundary { get; }
    internal Planckian Planckian { get; }
    internal Matrix AdaptationMatrix { get; }
    public string Name { get; }

    // even if white point has been hardcoded, still need observer to calculate CCT
    // should be safe to assume 2 degree observer
    public XyzConfiguration(WhitePoint whitePoint, string name = Utils.Unnamed) : 
        this(whitePoint, Observer.Degree2, Adaptation.Bradford, name)
    {
    }
    
    public XyzConfiguration(Illuminant illuminant, Observer observer, string name = Utils.Unnamed) : 
        this(illuminant, observer, Adaptation.Bradford, name)
    {
        Illuminant = illuminant;
    }
    
    public XyzConfiguration(Illuminant illuminant, Observer observer, double[,] adaptation, string name = Utils.Unnamed) : 
        this(illuminant.GetWhitePoint(observer), observer, adaptation, name)
    {
        Illuminant = illuminant;
    }
    
    public XyzConfiguration(WhitePoint whitePoint, Observer observer, double[,] adaptation, string name = Utils.Unnamed)
    {
        WhitePoint = whitePoint;
        Observer = observer;
        SpectralBoundary = new SpectralBoundary(observer, WhiteChromaticity);
        Planckian = new Planckian(observer);
        AdaptationMatrix = new Matrix(adaptation);
        Name = name;
    }
    
    public override string ToString() => $"{Name} · white point {WhitePoint}";
}