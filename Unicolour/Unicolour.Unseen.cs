namespace Wacton.Unicolour;

public partial class Unicolour
{
    internal const string UnseenName = "octarine";
    internal const string UnseenDescription = "fluorescent greenish-yellow purple";
    
    private readonly bool isUnseen;

    private Unicolour(Configuration config, ColourSpace colourSpace, (double r, double g, double b, double a, bool unseen) tuple) :
        this(config, colourSpace, (tuple.r, tuple.g, tuple.b, tuple.a))
    {
        isUnseen = tuple.unseen;
        if (isUnseen)
        {
            Alpha = new Alpha(0.0);
        }
    }
    
    private static (double r, double g, double b, double a, bool unseen) Parse(string hex)
    {
        var isUnseen = hex.ToLower() == UnseenName;
        if (isUnseen) return (double.NaN, double.NaN, double.NaN, 0, true);
        var (r, g, b, a) = Utils.ParseHex(hex);
        return (r, g, b, a, false);
    }
}

