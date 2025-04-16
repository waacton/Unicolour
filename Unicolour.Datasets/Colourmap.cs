namespace Wacton.Unicolour.Datasets;

public static class Colourmaps
{
    public static readonly Colourmap Viridis = new Viridis();
    public static readonly Colourmap Plasma = new Plasma();
    public static readonly Colourmap Inferno = new Inferno();
    public static readonly Colourmap Magma = new Magma();
    public static readonly Colourmap Cividis = new Cividis();
    public static readonly Colourmap Mako = new Mako();
    public static readonly Colourmap Rocket = new Rocket();
    public static readonly Colourmap Crest = new Crest();
    public static readonly Colourmap Flare = new Flare();
    public static readonly Colourmap Vlag = new Vlag();
    public static readonly Colourmap Icefire = new Icefire();
    public static readonly Colourmap Twilight = new Twilight();
    public static readonly Colourmap TwilightShifted = new TwilightShifted();
    public static readonly Colourmap Turbo = new Turbo();
    public static readonly Colourmap Cubehelix = new Cubehelix();
}

public abstract class Colourmap
{
    public static readonly Configuration Config = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65);
    public static readonly Unicolour Black = new(Config, ColourSpace.Rgb, 0, 0, 0); 
    public static readonly Unicolour White = new(Config, ColourSpace.Rgb, 1, 1, 1);

    public abstract Unicolour Map(double x);
    public Unicolour MapWithClipping(double x, Unicolour? lowerClipColour = null, Unicolour? upperClipColour = null)
    {
        return x switch
        {
            < 0 => lowerClipColour ?? Black,
            > 1 => upperClipColour ?? White,
            _ => Map(x)
        };
    }
    
    public IEnumerable<Unicolour> Palette(int count)
    {
        count = Math.Max(count, 0);
        
        var palette = new List<Unicolour>();
        for (var i = 0; i < count; i++)
        {
            var x = count == 1 ? 0.5 : i / (double)(count - 1);
            palette.Add(Map(x));
        }

        return palette;
    }
    
    protected static Unicolour InterpolateColourTable(Unicolour[] colourTable, double x)
    {
        var (lowerColour, upperColour, mixAmount) = Lut.Lookup(colourTable, x);
        return lowerColour.Mix(upperColour, ColourSpace.Rgb, mixAmount);
    }
}