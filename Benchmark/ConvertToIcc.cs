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
public class ConvertToIcc
{
    private const string Fogra39 = "Coated_Fogra39L_VIGC_300.icc";
    private const string Fogra55 = "Ref-ECG-CMYKOGV_FOGRA55_TAC300.icc";
    
    private static readonly string IccFile;
    private static readonly ColourSpaceWrapper ColourSpace;
    private static readonly NuGet.Configuration NuGetConfig;
    private static readonly Local.Configuration LocalConfig;
    private static readonly int Seed;
    private static readonly (double, double, double) Tuple;
    
    static ConvertToIcc()
    {
        /*
         * choose the ICC file to generate a benchmark for
         * as well as the colour space to start from (XYZ D50 would be closest to benchmarking only the ICC transform code)
         */
        IccFile = Fogra39;
        ColourSpace = ColourSpaceWrapper.Rgb;
        
        var profilePath = Directory.GetFiles(".", IccFile, SearchOption.AllDirectories).First();
        NuGetConfig = new(iccConfig: new(profilePath, NuGet.Icc.Intent.Unspecified));
        LocalConfig = new(iccConfig: new(profilePath, Local.Icc.Intent.Unspecified));
        
        // if control of the seed is needed, easiest to edit in Program.cs
        // this class is statically initialised per benchmark, i.e. once for Old() once for New()
        // so using serialised seed ensures a new randomiser instance with the same seed each time, and therefore the same input values
        Seed = Serialisation.ReadSeed();
        Tuple = new Randomiser(Seed).Triplet(ColourSpace.ToLocal()).Tuple;
    }
    
    // this is only used to log some data in a convenient place...
    // ... a bit overkill but seems to be the easiest way to get useful text alongside the summary 🤷
    private class Config : ManualConfig
    {
        // just for a friendly colour description in the summary
        private static readonly string Description = new Local.Unicolour(LocalConfig, ColourSpace.ToLocal(), Tuple).ToString();
        
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
    public double[] Old()
    {
        var colour = new NuGet.Unicolour(NuGetConfig, ColourSpace.ToNuGet(), Tuple);
        return colour.Icc.Values;
    }
    
    [Benchmark]
    public double[] New()
    {
        var colour = new Local.Unicolour(LocalConfig, ColourSpace.ToLocal(), Tuple);
        return colour.Icc.Values;
    }
}