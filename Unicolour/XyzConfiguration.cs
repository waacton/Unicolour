namespace Wacton.Unicolour;

public class XyzConfiguration
{
    public WhitePoint WhitePoint { get; }
    public Chromaticity ChromaticityWhite => GetChromaticity(WhitePoint);
    public Observer Observer { get; }
    internal Planckian Planckian { get; }
    
    public static readonly XyzConfiguration D65 = new(Illuminant.D65, Observer.Degree2);
    public static readonly XyzConfiguration D50 = new(Illuminant.D50, Observer.Degree2);

    // even if white point has been hardcoded, still need observer to calculate CCT
    public XyzConfiguration(WhitePoint whitePoint) : 
        this(whitePoint, Observer.Degree2)
    {
    }
    
    public XyzConfiguration(Illuminant illuminant, Observer observer) : 
        this(illuminant.GetWhitePoint(observer), observer)
    {
    }
    
    public XyzConfiguration(WhitePoint whitePoint, Observer observer)
    {
        WhitePoint = whitePoint;
        Observer = observer;
        Planckian = new Planckian(observer);
    }

    private static Chromaticity GetChromaticity(WhitePoint whitePoint)
    {
        var x = whitePoint.X / 100.0;
        var y = whitePoint.Y / 100.0;
        var z = whitePoint.Z / 100.0;
        var normalisation = x + y + z;
        return new(x / normalisation, y / normalisation);
    }

    public override string ToString() => $"XYZ {WhitePoint}";
}