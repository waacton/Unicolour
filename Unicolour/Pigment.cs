namespace Wacton.Unicolour;

public class Pigment
{
    private KubelkaMunk KubelkaMunk { get; }
    private int StartWavelength { get; }
    private int WavelengthInterval { get; }
    private int[] Wavelengths { get; }
    
    public SpectralCoefficients? R { get; } // reflectance
    public SpectralCoefficients? K { get; } // absorption
    public SpectralCoefficients? S { get; } // scattering
    public double? K1 { get; }
    public double? K2 { get; }
    public string Name { get; }
    
    public Pigment(int startWavelength, int wavelengthInterval, double[] r, string name = Utils.Unnamed)
    {
        KubelkaMunk = KubelkaMunk.SingleConstant;
        StartWavelength = startWavelength;
        WavelengthInterval = wavelengthInterval;
        R = new SpectralCoefficients(startWavelength, wavelengthInterval, r);
        Wavelengths = R.Wavelengths;
        Name = name;
    }
    
    public Pigment(int startWavelength, int wavelengthInterval, double[] k, double[] s, double? k1 = null, double? k2 = null, string name = Utils.Unnamed)
    {
        KubelkaMunk = KubelkaMunk.TwoConstant;
        StartWavelength = startWavelength;
        WavelengthInterval = wavelengthInterval;
        K = new SpectralCoefficients(startWavelength, wavelengthInterval, k);
        S = new SpectralCoefficients(startWavelength, wavelengthInterval, s);
        Wavelengths = K.Wavelengths.Length <= S.Wavelengths.Length ? K.Wavelengths : S.Wavelengths;
        K1 = k1;
        K2 = k2;
        Name = name;
    }

    internal static SpectralCoefficients? GetReflectance(Pigment[] pigments, double[] weights)
    {
        if (!pigments.Any())
        {
            return null;
        }
        
        var examplePigment = pigments.First();
        var kubelkaMunk = examplePigment.KubelkaMunk;
        var wavelengths = examplePigment.Wavelengths;
        var k1 = examplePigment.K1;
        var k2 = examplePigment.K2;

        /*
         * exit early to avoid unnecessary calculations
         * these could probably be handled by making big assumptions, e.g.
         * - assume 0 coefficient for a missing wavelength, or interpolate, or only process wavelengths common to all pigments
         * - convert two-constant pigments to single-constant if any other pigment is single-constant (if feasible?)
         * - fall back to uncorrected when different K1 or K2
         * but it's a lot of unclear extra complexity for a niche feature (and can be revisited in future)
         */
        var otherPigments = pigments.Skip(1).ToArray();
        var matchingWavelengths = otherPigments.All(x => x.Wavelengths.SequenceEqual(wavelengths));
        var matchingKubelkaMunk = otherPigments.All(x => x.KubelkaMunk == kubelkaMunk);
        var matchingCorrection = otherPigments.All(x => x.K1 == k1 && x.K2 == k2);
        if (!matchingWavelengths || !matchingKubelkaMunk || !matchingCorrection)
        {
            return null;
        }

        // assume negative weights means no weight
        // and exit early if there is no positive weight (i.e. no amount of pigment is mixed)
        weights = weights.Select(x => Math.Max(x, 0.0)).ToArray();
        var totalWeight = weights.Sum();
        if (totalWeight == 0.0)
        {
            return null;
        }
        
        var concentrations = weights.Select(x => x / totalWeight).ToArray();

        var mixedReflectance = new double[wavelengths.Length];
        for (var i = 0; i < wavelengths.Length; i++)
        {
            var wavelength = wavelengths[i];
            var r = kubelkaMunk == KubelkaMunk.SingleConstant 
                ? SingleConstantR(wavelength, pigments, concentrations) 
                : TwoConstantR(wavelength, pigments, concentrations, k1, k2);

            mixedReflectance[i] = r;
        }

        var startWavelength = examplePigment.StartWavelength;
        var wavelengthDelta = examplePigment.WavelengthInterval;
        return new SpectralCoefficients(startWavelength, wavelengthDelta, mixedReflectance);
    }

    private static double SingleConstantR(int wavelength, Pigment[] pigments, double[] concentrations)
    {
        var ks = pigments.Select((pigment, i) => GetKOverS(pigment.R![wavelength]) * concentrations[i]).Sum();
        return GetR(ks);
    }

    private static double TwoConstantR(int wavelength, Pigment[] pigments, double[] concentrations, double? k1, double? k2)
    {
        var k = pigments.Select((pigment, i) => pigment.K![wavelength] * concentrations[i]).Sum();
        var s = pigments.Select((pigment, i) => pigment.S![wavelength] * concentrations[i]).Sum();
        var r = GetR(k, s);
        r = k1.HasValue && k2.HasValue ? CorrectR(r, k1.Value, k2.Value) : r;
        return r;
    }

    private static double GetKOverS(double r) => r == 0.0 ? double.NaN : Math.Pow(1 - r, 2) / (2 * r);
    private static double GetR(double k, double s) => s == 0.0 ? double.NaN : GetR(k / s);
    private static double GetR(double ks) => 1 + ks - Math.Sqrt(Math.Pow(ks, 2) + 2 * ks);

    // Saunderson correction, assumes data measured in SPEX mode (initial "k1 + ..." term not required)
    private static double CorrectR(double r, double k1, double k2) => (1 - k1) * (1 - k2) * r / (1 - k2 * r);
    
    public override string ToString() => Name;
}

internal enum KubelkaMunk
{
    SingleConstant,
    TwoConstant
}

