using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Wacton.Unicolour;

namespace Benchmark;

[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net80)]
public class ConvertFromRgb
{
    private const ColourSpace initialColourSpace = ColourSpace.Rgb;
    private readonly Random random = new();
    private (double, double, double) triplet;
    
    [GlobalSetup]
    public void Setup()
    {
        _ = Configuration.Default; // avoids the static initialisation being triggered by the first Unicolour constructor
        var r = random.NextDouble();
        var g = random.NextDouble();
        var b = random.NextDouble();
        triplet = new(r, g, b);
        
        Console.WriteLine($"Benchmark initial colour: {initialColourSpace} {triplet}");
    }
    
    [Params(
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
    )]
    public ColourSpace TargetColourSpace { get; set; }
    
    [Benchmark]
    public ColourRepresentation Convert()
    {
        return new Unicolour(initialColourSpace, triplet).GetRepresentation(TargetColourSpace);
    }
}