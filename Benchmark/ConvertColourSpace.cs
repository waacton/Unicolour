extern alias LocalProject;
extern alias NuGetPackage;

using Local = LocalProject::Wacton.Unicolour;
using NuGet = NuGetPackage::Wacton.Unicolour;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Benchmark;

[SimpleJob(RuntimeMoniker.Net481)]
[SimpleJob(RuntimeMoniker.Net80)]
[Config(typeof(Config))]
public class ConvertColourSpace
{
    private static readonly ColourSpaceWrapper InitialColourSpace;
    private static readonly NuGet.Configuration NuGetConfig;
    private static readonly Local.Configuration LocalConfig;
    private static readonly int Seed;
    private static readonly (double, double, double) Tuple;
    
    static ConvertColourSpace()
    {
        /*
         * choose the colour space to generate a benchmark for
         */
        InitialColourSpace = ColourSpaceWrapper.Rgb;
        
        // not strictly necessary, but being explicit ensures the default configuration objects are initialised
        // otherwise they are initialised on-demand when a config-less Unicolour is created, which is during the benchmark itself, skewing results
        // note: can also force initialisation using `_ = Local.Configuration.Default;` but will continue to use these objects here for clarity
        NuGetConfig = new();
        LocalConfig = new();
        
        // if control of the seed is needed, easiest to edit in Program.cs
        // this class is statically initialised per benchmark, i.e. once for Old() once for New()
        // so using serialised seed ensures a new randomiser instance with the same seed each time, and therefore the same input values
        Seed = Serialisation.ReadSeed();
        Tuple = new Randomiser(Seed).Triplet(InitialColourSpace.ToLocal()).Tuple;
    }
    
    // this is only used to log some data in a convenient place...
    // ... a bit overkill but seems to be the easiest way to get useful text alongside the summary 🤷
    private class Config : ManualConfig
    {
        // just for a friendly colour description in the summary
        private static readonly string Description = new Local.Unicolour(LocalConfig, InitialColourSpace.ToLocal(), Tuple).ToString();
        
        public Config()
        {
            AddColumn(new TagColumn("Seed", _ => Seed.ToString()));
            AddColumn(new TagColumn("Colour", _ => Description));
        }
    }
    
    /*
     * cannot use NuGet.* or Local.* in function signatures because auto-generated BenchmarkDotNet cannot distinguish between them
     * so resort to either `object` as mentioned at https://github.com/dotnet/BenchmarkDotNet/issues/2289 or something more meaningful
     */
    
    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(TargetColourSpaces))]
    public (double, double, double) Old(ColourSpaceWrapper targetColourSpace)
    {
        var colour = new NuGet.Unicolour(NuGetConfig, InitialColourSpace.ToNuGet(), Tuple); // Construct benchmark can isolate the duration of this action
        return colour.GetRepresentation(targetColourSpace.ToNuGet()).Tuple; 
    }
    
    [Benchmark]
    [ArgumentsSource(nameof(TargetColourSpaces))]
    public (double, double, double) New(ColourSpaceWrapper targetColourSpace)
    {
        var colour = new Local.Unicolour(LocalConfig, InitialColourSpace.ToLocal(), Tuple); // Construct benchmark can isolate the duration of this action
        return colour.GetRepresentation(targetColourSpace.ToLocal()).Tuple; 
    }
    
    public ColourSpaceWrapper[] TargetColourSpaces =>
    [
        ColourSpaceWrapper.Rgb, ColourSpaceWrapper.Rgb255, ColourSpaceWrapper.RgbLinear,
        ColourSpaceWrapper.Hsb, ColourSpaceWrapper.Hsl, ColourSpaceWrapper.Hwb, ColourSpaceWrapper.Hsi,
        ColourSpaceWrapper.Xyz, ColourSpaceWrapper.Xyy, ColourSpaceWrapper.Wxy,
        ColourSpaceWrapper.Lab, ColourSpaceWrapper.Lchab, ColourSpaceWrapper.Luv, ColourSpaceWrapper.Lchuv, ColourSpaceWrapper.Hsluv, ColourSpaceWrapper.Hpluv,
        ColourSpaceWrapper.Ypbpr, ColourSpaceWrapper.Ycbcr, ColourSpaceWrapper.Ycgco, ColourSpaceWrapper.Yuv, ColourSpaceWrapper.Yiq, ColourSpaceWrapper.Ydbdr,
        ColourSpaceWrapper.Tsl, ColourSpaceWrapper.Xyb,
        ColourSpaceWrapper.Ipt, ColourSpaceWrapper.Ictcp, ColourSpaceWrapper.Jzazbz, ColourSpaceWrapper.Jzczhz,
        ColourSpaceWrapper.Oklab, ColourSpaceWrapper.Oklch, ColourSpaceWrapper.Okhsv, ColourSpaceWrapper.Okhsl, ColourSpaceWrapper.Okhwb,
        ColourSpaceWrapper.Cam02, ColourSpaceWrapper.Cam16,
        ColourSpaceWrapper.Hct
    ];
}