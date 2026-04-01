namespace Wacton.Unicolour;

internal class ChromaticAdaptor
{
    internal WhitePoint WhitePoint { get; }
    internal ChromaticAdaptation ChromaticAdaptation { get; }
    
    public ChromaticAdaptor(WhitePoint whitePoint, ChromaticAdaptation chromaticAdaptation)
    {
        WhitePoint = whitePoint;
        ChromaticAdaptation = chromaticAdaptation;
    }
    
    internal Xyz AdaptFrom(Xyz xyz) => ChromaticAdaptation.Transform(xyz, WhitePoint);
    internal Xyz AdaptTo(Xyz xyz, WhitePoint whitePoint) => ChromaticAdaptation.Transform(xyz, whitePoint);
    
    public override string ToString() => $"{ChromaticAdaptation} · {WhitePoint}";
}