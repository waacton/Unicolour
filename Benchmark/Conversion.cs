using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Wacton.Unicolour;

namespace Benchmark;

[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net80)]
[Config(typeof(Config))]
public class Conversion
{
    private static readonly ColourSpace InitialSpace;
    private static readonly Randomiser Randomiser;
    private static readonly Unicolour Colour;
    private static readonly (double first, double second, double third) Tuple;
    
    static Conversion()
    {
        // forces the static initialisation of the default configuration object
        // so that it doesn't occur during the first unicolour constructor
        _ = Configuration.Default;
        
        InitialSpace = ColourSpace.Rgb;                 // choose which colour space to generate benchmark for
        Randomiser = new();                             // provide an explicit seed for deterministic values
        Colour = Randomiser.Unicolour(InitialSpace);    // just for a friendly colour string in the summary
        Tuple = Colour.GetRepresentation(InitialSpace).Tuple;
    }
    
    // this is only used to log some data in a convenient place...
    // ... a bit overkill but don't know if there's another way to get text alongside the summary 🤷
    private class Config : ManualConfig
    {
        public Config()
        {
            AddColumn(new TagColumn("Seed", _ => Randomiser.Seed.ToString()));
            AddColumn(new TagColumn("Colour", _ => Colour.ToString()));
        }
    }
    
    [Benchmark]
    public Unicolour Construct()
    {
        Utils.UseOldPower = true;
        return new Unicolour(InitialSpace, Tuple);
    }
    
    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(TargetSpaces))]
    public ColourRepresentation ConvertOld(ColourSpace colourSpace)
    {
        Utils.UseOldPower = true;
        
        // need to construct a new colour to avoid reading the cached result
        // but the Construct benchmark shows us what that extra time is
        var colour = new Unicolour(InitialSpace, Tuple);
        return colour.GetRepresentation(colourSpace); 
    }
    
    [Benchmark]
    [ArgumentsSource(nameof(TargetSpaces))]
    public ColourRepresentation ConvertNew(ColourSpace colourSpace)
    {
        Utils.UseOldPower = false;

        // need to construct a new colour to avoid reading the cached result
        // but the Construct benchmark shows us what that extra time is
        var colour = new Unicolour(InitialSpace, Tuple);
        return colour.GetRepresentation(colourSpace); 
    }

    public ColourSpace[] TargetSpaces =>
    [
        ColourSpace.Rgb, ColourSpace.Rgb255, ColourSpace.RgbLinear,
        ColourSpace.Hsb, ColourSpace.Hsl, ColourSpace.Hwb, ColourSpace.Hsi,
        ColourSpace.Xyz, ColourSpace.Xyy, ColourSpace.Wxy,
        ColourSpace.Lab, ColourSpace.Lchab, ColourSpace.Luv, ColourSpace.Lchuv, ColourSpace.Hsluv, ColourSpace.Hpluv,
        ColourSpace.Ypbpr, ColourSpace.Ycbcr, ColourSpace.Ycgco, ColourSpace.Yuv, ColourSpace.Yiq, ColourSpace.Ydbdr,
        ColourSpace.Tsl, ColourSpace.Xyb,
        ColourSpace.Ipt, ColourSpace.Ictcp, ColourSpace.Jzazbz, ColourSpace.Jzczhz,
        ColourSpace.Oklab, ColourSpace.Oklch, ColourSpace.Okhsv, ColourSpace.Okhsl, ColourSpace.Okhwb,
        ColourSpace.Cam02, ColourSpace.Cam16,
        ColourSpace.Hct
    ];
}