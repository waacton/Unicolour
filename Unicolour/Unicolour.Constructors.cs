namespace Wacton.Unicolour;

public partial class Unicolour
{
    /* construction from colour space values */
    public Unicolour(ColourSpace colourSpace, (double first, double second, double third) tuple, double alpha = 1.0) :
        this(colourSpace, tuple.first, tuple.second, tuple.third, alpha)
    {
    }
    
    public Unicolour(ColourSpace colourSpace, (double first, double second, double third, double alpha) tuple) :
        this(colourSpace, tuple.first, tuple.second, tuple.third, tuple.alpha)
    {
    }
    
    public Unicolour(ColourSpace colourSpace, double first, double second, double third, double alpha = 1.0) : 
        this(Configuration.Default, colourSpace, first, second, third, alpha)
    {
    }

    public Unicolour(Configuration config, ColourSpace colourSpace, (double first, double second, double third) tuple, double alpha = 1.0) :
        this(config, colourSpace, tuple.first, tuple.second, tuple.third, alpha)
    {
    }
    
    public Unicolour(Configuration config, ColourSpace colourSpace, (double first, double second, double third, double alpha) tuple) :
        this(config, colourSpace, tuple.first, tuple.second, tuple.third, tuple.alpha)
    {
    }

    public Unicolour(Configuration config, ColourSpace colourSpace, double first, double second, double third, double alpha = 1.0) :
        this(config, ColourHeritage.None, colourSpace, first, second, third, alpha)
    {
    }

    /* construction from hex representation */
    public Unicolour(string hex) : 
        this(Configuration.Default, hex)
    {
    }
    
    public Unicolour(string hex, double alphaOverride) : 
        this(Configuration.Default, hex, alphaOverride)
    {
    }
    
    public Unicolour(Configuration config, string hex) : 
        this(config, ColourSpace.Rgb, Parse(hex))
    {
    }

    public Unicolour(Configuration config, string hex, double alphaOverride) :
        this(config, ColourSpace.Rgb, Parse(hex) with { a = alphaOverride })
    {
    }

    /* construction from temperature */
    public Unicolour(double cct, double duv, double luminance = 1.0) : 
        this(Configuration.Default, cct, duv, luminance)
    {
    }
    
    public Unicolour(double cct, Locus locus = Locus.Blackbody, double luminance = 1.0) : 
        this(Configuration.Default, cct, locus, luminance)
    {
    }
    
    public Unicolour(Configuration config, double cct, double duv, double luminance = 1.0) : 
        this(config, new Temperature(cct, duv), luminance)
    {
    }
    
    public Unicolour(Configuration config, double cct, Locus locus = Locus.Blackbody, double luminance = 1.0) : 
        this(config, Temperature.FromCct(cct, locus, config.Xyz.Planckian), luminance)
    {
    }
    
    internal Unicolour(Configuration config, Temperature temperature, double luminance) :
        this(config, ColourSpace.Xyy, TemperatureToXyyTuple(temperature, config.Xyz.Observer, luminance))
    {
        this.temperature = new Lazy<Temperature>(() => temperature);
    }

    private static (double x, double y, double upperY, double alpha) TemperatureToXyyTuple(Temperature temperature, Observer observer, double luminance)
    {
        var chromaticity = Temperature.ToChromaticity(temperature, observer);
        return (chromaticity.X, chromaticity.Y, luminance, 1.0);
    }
    
    /* construction from SPD */
    public Unicolour(Spd spd) : 
        this(Configuration.Default, spd)
    {
    }
    
    public Unicolour(Configuration config, Spd spd) : 
        this(config, ColourSpace.Xyz, SpdToXyzTuple(spd, config.Xyz.Observer))
    {
    }
    
    private static (double x, double y, double z, double alpha) SpdToXyzTuple(Spd spd, Observer observer)
    {
        var xyz = Xyz.FromSpd(spd, observer);
        return (xyz.X, xyz.Y, xyz.Z, 1.0);
    }
}