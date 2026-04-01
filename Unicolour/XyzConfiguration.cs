namespace Wacton.Unicolour;

public class XyzConfiguration
{
    public static readonly XyzConfiguration D65 = new(Illuminant.D65, Observer.Degree2, nameof(D65));
    public static readonly XyzConfiguration D50 = new(Illuminant.D50, Observer.Degree2, nameof(D50));
    
    public WhitePoint WhitePoint { get; }
    internal Illuminant? Illuminant { get; }
    public Observer Observer { get; }
    internal ChromaticAdaptation ChromaticAdaptation { get; }
    internal ChromaticAdaptor ChromaticAdaptor { get; }
    internal SpectralBoundary SpectralBoundary { get; }
    internal Planckian Planckian { get; }
    public string Name { get; }

    // even if white point has been hardcoded, still need observer to calculate CCT
    // should be safe to assume 2 degree observer
    public XyzConfiguration(WhitePoint whitePoint, string name = Utils.Unnamed) : 
        this(whitePoint, Observer.Degree2, ChromaticAdaptation.Bradford, name)
    {
    }
    
    public XyzConfiguration(Illuminant illuminant, Observer observer, string name = Utils.Unnamed) : 
        this(illuminant, observer, ChromaticAdaptation.Bradford, name)
    {
        Illuminant = illuminant;
    }
    
    public XyzConfiguration(Illuminant illuminant, Observer observer, ChromaticAdaptation chromaticAdaptation, string name = Utils.Unnamed) : 
        this(illuminant.GetWhitePoint(observer), observer, chromaticAdaptation, name)
    {
        Illuminant = illuminant;
    }
    
    public XyzConfiguration(WhitePoint whitePoint, Observer observer, ChromaticAdaptation chromaticAdaptation, string name = Utils.Unnamed)
    {
        WhitePoint = whitePoint;
        Observer = observer;
        ChromaticAdaptation = chromaticAdaptation;
        ChromaticAdaptor = new ChromaticAdaptor(WhitePoint, ChromaticAdaptation);
        SpectralBoundary = new SpectralBoundary(observer, WhitePoint);
        Planckian = new Planckian(observer);
        Name = name;
    }

    public override string ToString() => $"{Name} · white point {WhitePoint}";
}