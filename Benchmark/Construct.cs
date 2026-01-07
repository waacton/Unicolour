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
public class Construct
{
    private static readonly ColourSpaceWrapper InitialColourSpace;
    private static readonly NuGet.Configuration NuGetConfig;
    private static readonly Local.Configuration LocalConfig;
    private static readonly int Seed;
    private static readonly (double, double, double) Tuple;
    
    static Construct()
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
    public object Old()
    {
        return new NuGet.Unicolour(NuGetConfig, InitialColourSpace.ToNuGet(), Tuple);
    }
    
    [Benchmark]
    public object New()
    {
        return new Local.Unicolour(LocalConfig, InitialColourSpace.ToLocal(), Tuple);
    }
}