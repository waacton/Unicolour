extern alias LocalProject;
extern alias NuGetPackage;

using Local = LocalProject::Wacton.Unicolour;
using NuGet = NuGetPackage::Wacton.Unicolour;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Benchmark;

[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[Config(typeof(Config))]
public class ConvertFromIcc
{
    private const string Fogra39 = "Coated_Fogra39L_VIGC_300.icc";
    private const string Fogra55 = "Ref-ECG-CMYKOGV_FOGRA55_TAC300.icc";
    
    private static readonly string IccFile;
    private static readonly ColourSpaceWrapper ColourSpace;
    private static readonly NuGet.Configuration NuGetConfig;
    private static readonly Local.Configuration LocalConfig;
    private static readonly int Seed;
    private static readonly double[] ChannelValues;
    
    static ConvertFromIcc()
    {
        /*
         * choose the ICC file to generate a benchmark for
         * as well as the colour space to end at (XYZ D50 would be closest to benchmarking only the ICC transform code)
         */
        IccFile = Fogra39;
        ColourSpace = ColourSpaceWrapper.Rgb;
        
        // not strictly necessary, but being explicit ensures the default configuration objects are initialised
        // otherwise they are initialised on-demand when a config-less Unicolour is created, which is during the benchmark itself, skewing results
        // note: can also force initialisation using `_ = Local.Configuration.Default;` but will continue to use these objects here for clarity
        var profilePath = Directory.GetFiles(".", IccFile, SearchOption.AllDirectories).First();
        NuGetConfig = new(iccConfig: new(profilePath, NuGet.Icc.Intent.Unspecified));
        LocalConfig = new(iccConfig: new(profilePath, Local.Icc.Intent.Unspecified));
        
        // if control of the seed is needed, easiest to edit in Program.cs
        // this class is statically initialised per benchmark, i.e. once for Old() once for New()
        // so using serialised seed ensures a new randomiser instance with the same seed each time, and therefore the same input values
        Seed = Serialisation.ReadSeed();
        ChannelValues = new Randomiser(Seed).Channels();
    }
    
    // this is only used to log some data in a convenient place...
    // ... a bit overkill but seems to be the easiest way to get useful text alongside the summary 🤷
    private class Config : ManualConfig
    {
        // just for a friendly colour description in the summary
        private static readonly string Description = new Local.Unicolour(LocalConfig, new Local.Icc.Channels(ChannelValues)).ToString();
        
        public Config()
        {
            AddColumn(new TagColumn("Seed", _ => Seed.ToString()));
            AddColumn(new TagColumn("Colour", _ => LocalConfig.Icc.Error ?? Description));
        }
    }
    
    /*
     * cannot use NuGet.* or Local.* in function signatures because auto-generated BenchmarkDotNet cannot distinguish between them
     * so resort to either `object` as mentioned at https://github.com/dotnet/BenchmarkDotNet/issues/2289 or something more meaningful
     */
    
    [Benchmark(Baseline = true)]
    public (double, double, double) Old()
    {
        var colour = new NuGet.Unicolour(NuGetConfig, new NuGet.Icc.Channels(ChannelValues));
        return colour.GetRepresentation(ColourSpace.ToNuGet()).Tuple;
    }
    
    [Benchmark]
    public (double, double, double) New()
    {
        var colour = new Local.Unicolour(LocalConfig, new Local.Icc.Channels(ChannelValues));
        return colour.GetRepresentation(ColourSpace.ToLocal()).Tuple;
    }
}