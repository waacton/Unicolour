using Wacton.Unicolour.Experimental;

namespace Wacton.Unicolour.Tests.Utils;

public record MunsellTestData(double HueNumber, string HueLetter, double Value, int Chroma, double X, double Y, double LuminanceMagnesiumOxide)
{
    internal readonly double HueNumber = HueNumber;
    internal readonly string HueLetter = HueLetter;
    internal readonly double Value = Value;
    internal readonly int Chroma = Chroma;
    internal readonly double X = X;
    internal readonly double Y = Y;
    internal readonly double LuminanceMagnesiumOxide = LuminanceMagnesiumOxide; // Y value for smoked magnesium dioxide reference white

    internal static MunsellTestData FromNode(Munsell.Node node)
    {
        return new MunsellTestData(node.HueNumber, node.HueLetter, node.Value, node.Chroma, node.X, node.Y, node.LuminanceMagnesiumOxide);
    }
    
    public override string ToString() => $"{HueNumber}{HueLetter} {Value}/{Chroma} = ({X}, {Y})";
}