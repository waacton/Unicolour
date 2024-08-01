using Wacton.Unicolour.Icc;

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
        source = $"{nameof(Hex)} {hex}";
    }

    public Unicolour(Configuration config, string hex, double alphaOverride) :
        this(config, ColourSpace.Rgb, Parse(hex) with { a = alphaOverride })
    {
        source = $"{nameof(Hex)} {hex}";
    }
    
    /* construction from chromaticity */
    public Unicolour(Chromaticity chromaticity, double luminance = 1.0) :
        this(Configuration.Default, chromaticity, luminance)
    {
    }
    
    public Unicolour(Configuration config, Chromaticity chromaticity, double luminance = 1.0) :
        this(config, ColourSpace.Xyy, chromaticity.X, chromaticity.Y, luminance)
    {
        source = $"{nameof(Chromaticity)} {chromaticity}";
    }

    /* construction from temperature */
    public Unicolour(double cct, Locus locus = Locus.Blackbody, double luminance = 1.0) : 
        this(Configuration.Default, cct, locus, luminance)
    {
    }
    
    public Unicolour(Configuration config, double cct, Locus locus = Locus.Blackbody, double luminance = 1.0) : 
        this(config, Temperature.FromCct(cct, locus, config.Xyz.Planckian), luminance)
    {
    }
    
    public Unicolour(Temperature temperature, double luminance = 1.0) : 
        this(Configuration.Default, temperature, luminance)
    {
    }
    
    public Unicolour(Configuration config, Temperature temperature, double luminance = 1.0) :
        this(config, ColourSpace.Xyy, TemperatureToXyyTuple(temperature, config.Xyz.Observer, luminance))
    {
        this.temperature = new Lazy<Temperature>(() => temperature);
        source = $"{nameof(Temperature)} {temperature}";
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
        source = $"{nameof(Spd)} {spd}";
    }
    
    private static (double x, double y, double z, double alpha) SpdToXyzTuple(Spd spd, Observer observer)
    {
        var xyz = Xyz.FromSpd(spd, observer);
        return (xyz.X, xyz.Y, xyz.Z, 1.0);
    }
    
    /* construction from ICC channels */
    public Unicolour(Channels channels, double alpha = 1.0) : 
        this(Configuration.Default, channels, alpha)
    {
    }
    
    public Unicolour(Configuration config, Channels channels, double alpha = 1.0) : 
        this(config, config.Icc.ConnectingSpace, IccToTuple(channels, config.Icc, config.Xyz), alpha)
    {
        icc = new Lazy<Channels>(() => channels);
        source = $"{nameof(Icc)} {channels}";
    }
    
    private static (double x, double y, double z) IccToTuple(Channels channels, IccConfiguration iccConfig, XyzConfiguration xyzConfig)
    {
        ColourRepresentation connectingSpaceRepresentation = iccConfig.HasSupportedProfile
            ? Channels.ToXyz(channels, iccConfig, xyzConfig)
            : Channels.UncalibratedToRgb(channels);
        return connectingSpaceRepresentation.Triplet.Tuple;
    }
}