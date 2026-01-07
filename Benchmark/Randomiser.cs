extern alias LocalProject;
using LocalProject::Wacton.Unicolour;

namespace Benchmark;

public class Randomiser(int Seed)
{
    internal int Seed = Seed;
    private readonly Random random = new(Seed);
    private double Double() => random.NextDouble();
    private double Double(double min, double max) => random.NextDouble() * (max - min) + min;
    private int Int(int max) => random.Next(max);

    public Randomiser() : this(new Random().Next(int.MinValue, int.MaxValue)) {}
    
    internal Unicolour Unicolour(ColourSpace colourSpace, Configuration? configuration = null)
    {
        return new Unicolour(configuration ?? Configuration.Default, colourSpace, Triplet(colourSpace).Tuple, Alpha());
    }

    internal ColourTriplet Triplet(ColourSpace colourSpace)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => Rgb(),
            ColourSpace.Rgb255 => Rgb255(),
            ColourSpace.RgbLinear => RgbLinear(),
            ColourSpace.Hsb => Hsb(),
            ColourSpace.Hsl => Hsl(),
            ColourSpace.Hwb => Hwb(),
            ColourSpace.Hsi => Hsi(),
            ColourSpace.Xyz => Xyz(),
            ColourSpace.Xyy => Xyy(),
            ColourSpace.Wxy => Wxy(),
            ColourSpace.Lab => Lab(),
            ColourSpace.Lchab => Lchab(),
            ColourSpace.Luv => Luv(),
            ColourSpace.Lchuv => Lchuv(),
            ColourSpace.Hsluv => Hsluv(),
            ColourSpace.Hpluv => Hpluv(),
            ColourSpace.Ypbpr => Ypbpr(),
            ColourSpace.Ycbcr => Ycbcr(),
            ColourSpace.Ycgco => Ycgco(),
            ColourSpace.Yuv => Yuv(),
            ColourSpace.Yiq => Yiq(),
            ColourSpace.Ydbdr => Ydbdr(),
            ColourSpace.Tsl => Tsl(),
            ColourSpace.Xyb => Xyb(),
            ColourSpace.Lms => Lms(),
            ColourSpace.Ipt => Ipt(),
            ColourSpace.Ictcp => Ictcp(),
            ColourSpace.Jzazbz => Jzazbz(),
            ColourSpace.Jzczhz => Jzczhz(),
            ColourSpace.Oklab => Oklab(),
            ColourSpace.Oklch => Oklch(),
            ColourSpace.Okhsv => Okhsv(),
            ColourSpace.Okhsl => Okhsl(),
            ColourSpace.Okhwb => Okhwb(),
            ColourSpace.Oklrab => Oklrab(),
            ColourSpace.Oklrch => Oklrch(),
            ColourSpace.Cam02 => Cam02(),
            ColourSpace.Cam16 => Cam16(),
            ColourSpace.Hct => Hct(),
            ColourSpace.Munsell => Munsell(),
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }
    
    // ICC channels can be up to 15 dimensions
    internal double[] Channels() =>
    [
        Double(), Double(), Double(), Double(), Double(), Double(), Double(), Double(), Double(), Double(), Double(), Double(), Double(), Double(), Double()
    ];

    // W3C has useful information about the practical range of values (e.g. https://www.w3.org/TR/css-color-4/#serializing-oklab-oklch)
    private ColourTriplet Rgb255() => new(Int(256), Int(256), Int(256));
    private ColourTriplet Rgb() => new(Double(), Double(), Double());
    private ColourTriplet RgbLinear() => new(Double(), Double(), Double());
    private ColourTriplet Hsb() => new(Double(0, 360), Double(), Double());
    private ColourTriplet Hsl() => new(Double(0, 360), Double(), Double());
    private ColourTriplet Hwb() => new(Double(0, 360), Double(), Double());
    private ColourTriplet Hsi() => new(Double(0, 360), Double(), Double());
    private ColourTriplet Xyz() => new(Double(), Double(), Double());
    private ColourTriplet Xyy() => new(Double(), Double(), Double());
    private ColourTriplet Wxy() => new(Double() >= 0.5 ? Double(360, 700) : Double(-566, -493.5), Double(), Double());
    private ColourTriplet Lab() => new(Double(0, 100), Double(-128, 128), Double(-128, 128));
    private ColourTriplet Lchab() => new(Double(0, 100), Double(0, 230), Double(0, 360));
    private ColourTriplet Luv() => new(Double(0, 100), Double(-100, 100), Double(-100, 100));
    private ColourTriplet Lchuv() => new(Double(0, 100), Double(0, 230), Double(0, 360));
    private ColourTriplet Hsluv() => new(Double(0, 360), Double(0, 100), Double(0, 100));
    private ColourTriplet Hpluv() => new(Double(0, 360), Double(0, 100), Double(0, 100));
    private ColourTriplet Ypbpr() => new(Double(), Double(-0.5, 0.5), Double(-0.5, 0.5));
    private ColourTriplet Ycbcr() => new(Double(0, 255), Double(0, 255), Double(0, 255));
    private ColourTriplet Ycgco() => new(Double(), Double(-0.5, 0.5), Double(-0.5, 0.5));
    private ColourTriplet Yuv() => new(Double(), Double(-0.436, 0.436), Double(-0.614, 0.614));
    private ColourTriplet Yiq() => new(Double(), Double(-0.595, 0.595), Double(-0.522, 0.522));
    private ColourTriplet Ydbdr() => new(Double(), Double(-1.333, 1.333), Double(-1.333, 1.333));
    private ColourTriplet Tsl() => new(Double(0, 360), Double(), Double());
    private ColourTriplet Xyb() => new(Double(-0.03, 0.03), Double(), Double(-0.4, 0.4)); 
    private ColourTriplet Lms() => new(Double(), Double(), Double());
    private ColourTriplet Ipt() => new(Double(), Double(-0.75, 0.75), Double(-0.75, 0.75)); 
    private ColourTriplet Ictcp() => new(Double(), Double(-0.5, 0.5), Double(-0.5, 0.5)); 
    private ColourTriplet Jzazbz() => new(Double(0, 0.17), Double(-0.10, 0.11), Double(-0.16, 0.12)); // from own test values (SDR) - ranges suggested by paper (0->1, -0.5->0.5, -0.5->0.5) easily produce XYZ with NaNs [https://doi.org/10.1364/OE.25.015131]
    private ColourTriplet Jzczhz() => new(Double(0, 0.17), Double(0, 0.16), Double(0, 360)); // from own test values (SDR)
    private ColourTriplet Oklab() => new(Double(), Double(-0.5, 0.5), Double(-0.5, 0.5));
    private ColourTriplet Oklch() => new(Double(), Double(0, 0.5), Double(0, 360));
    private ColourTriplet Okhsv() => new(Double(0, 360), Double(), Double());
    private ColourTriplet Okhsl() => new(Double(0, 360), Double(), Double());
    private ColourTriplet Okhwb() => new(Double(0, 360), Double(), Double());
    private ColourTriplet Oklrab() => new(Double(), Double(-0.5, 0.5), Double(-0.5, 0.5));
    private ColourTriplet Oklrch() => new(Double(), Double(0, 0.5), Double(0, 360));
    private ColourTriplet Cam02() => new(Double(0, 100), Double(-50, 50), Double(-50, 50)); // from own test values 
    private ColourTriplet Cam16() => new(Double(0, 100), Double(-50, 50), Double(-50, 50)); // from own test values
    private ColourTriplet Hct() => new(Double(0, 360), Double(0, 120), Double(0, 100)); // from own test values 
    private ColourTriplet Munsell() => new(Double(0, 360), Double(0, 10), Double(0, 26)); // from data of "real" colours (smaller range than used in Unicolour)
    private double Alpha() => Double();
    
    private Temperature Temperature() => new(Double(1000, 20000), Double(-0.05, 0.05));
    private Chromaticity Chromaticity() => new(Double(0, 0.75), Double(0, 0.85));

    private string Hex()
    {
        const string hexChars = "0123456789abcdefABCDEF";
        var useHash = Int(2) == 0;
        var length = Int(2) == 0 ? 6 : 8;

        var hex = useHash ? "#" : string.Empty;
        for (var i = 0; i < length; i++)
        {
            hex += hexChars[Int(hexChars.Length)];
        }

        return hex;
    }

    public override string ToString() => $"seed {Seed}";
}