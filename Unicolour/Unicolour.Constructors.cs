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
        source = new Lazy<string>(() => $"{nameof(Hex)} {hex}");
    }

    public Unicolour(Configuration config, string hex, double alphaOverride) :
        this(config, ColourSpace.Rgb, Parse(hex) with { a = alphaOverride })
    {
        source = new Lazy<string>(() => $"{nameof(Hex)} {hex}");
    }
    
    /* construction from chromaticity */
    public Unicolour(Chromaticity chromaticity, double luminance = 1.0) :
        this(Configuration.Default, chromaticity, luminance)
    {
    }
    
    public Unicolour(Configuration config, Chromaticity chromaticity, double luminance = 1.0) :
        this(config, ColourSpace.Xyy, chromaticity.X, chromaticity.Y, luminance)
    {
        source = new Lazy<string>(() => $"{nameof(Chromaticity)} {chromaticity}");
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
        source = new Lazy<string>(() => $"{nameof(Temperature)} {temperature}");
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
        source = new Lazy<string>(() => $"{nameof(Spd)} {spd}");
    }
    
    private static (double x, double y, double z, double alpha) SpdToXyzTuple(Spd spd, Observer observer)
    {
        var xyz = Xyz.FromSpd(spd, observer);
        return (xyz.X, xyz.Y, xyz.Z, 1.0);
    }
    
    /* construction from pigments */
    public Unicolour(Pigment[] pigments, double[] weights) :
        this(Configuration.Default, pigments, weights)
    {
    }
    
    public Unicolour(Configuration config, Pigment[] pigments, double[] weights) :
        this(config, ColourSpace.Xyz, PigmentsToXyzTuple(pigments, weights, config.Xyz))
    {
        source = new Lazy<string>(() => $"{nameof(Pigment)} [{string.Join(", ", pigments.Select(x => x.ToString()))}] [{string.Join(", ", weights)}]");
    }
    
    private static (double x, double y, double z, double alpha) PigmentsToXyzTuple(Pigment[] pigments, double[] weights, XyzConfiguration xyzConfig)
    {
        var reflectance = Pigment.GetReflectance(pigments, weights);
        if (reflectance == null) return (double.NaN, double.NaN, double.NaN, 1.0);

        /*
         * reflectance is a material property and should be the same regardless of illuminant used when measuring it
         * so if the requested XYZ config doesn't contain an illuminant with an SPD (e.g. only has a hardcoded white point)
         * calculate XYZ using a different configuration, and then adapt to the target white point
         * ----------
         * NOTE: although reflectance is independent of illuminant, SPDs are just a sample of continuous data
         * and different illuminant SPDs result in slightly different linear RGB values
         * so for most accurate results it is best to use the illuminant used when measuring the pigment
         */
        var xyzConfigWithSpd = xyzConfig.Illuminant is { Spd: not null }
            ? xyzConfig
            : Configuration.Default.Xyz;

        var illuminantSpd = xyzConfigWithSpd.Illuminant!.Spd!;
        var observer = xyzConfigWithSpd.Observer;
        var (x, y, z) = Xyz.FromSpd(illuminantSpd, observer, reflectance).Triplet;
        if (xyzConfigWithSpd != xyzConfig)
        {
            (x, y, z) = Adaptation.WhitePoint(new ColourTriplet(x, y, z), xyzConfigWithSpd.WhitePoint, xyzConfig.WhitePoint).Tuple;
        }
        
        return (x, y, z, 1.0);
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
        source = new Lazy<string>(() => $"{nameof(Icc)} {channels}");
    }
    
    private static (double x, double y, double z) IccToTuple(Channels channels, IccConfiguration iccConfig, XyzConfiguration xyzConfig)
    {
        ColourRepresentation connectingSpaceRepresentation = iccConfig.HasSupportedProfile
            ? Channels.ToXyz(channels, iccConfig, xyzConfig)
            : Channels.UncalibratedToRgb(channels);
        return connectingSpaceRepresentation.Triplet.Tuple;
    }
}