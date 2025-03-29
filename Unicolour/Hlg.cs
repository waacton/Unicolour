namespace Wacton.Unicolour;

// unlike PQ, the nominal white luminance of 203 cd/m² is not baked into the transfer functions
// and needs to be taken into account when scaling for white luminance
internal static class Hlg
{
    private const double A = 0.17883277;
    private const double B = 0.28466892; // 1 - 4 * A
    private const double C = 0.55991073; // 0.5 - A * ln(4 * A)

    internal static double InverseOetf(double ePrime, DynamicRange dynamicRange)
    {
        var beta = BlackLift(dynamicRange.MaxLuminance, dynamicRange.MinLuminance);
        var e = InverseOetf(Math.Max(0, (1 - beta) * ePrime + beta));
        return e / dynamicRange.HlgScale;
    }
    
    internal static double InverseOetf(double ePrime)
    {
        return ePrime switch
        {
            <= 0.5 => Math.Pow(ePrime, 2) / 3.0,
            _ => (Math.Exp((ePrime - C) / A) + B) / 12.0
        };
    }
    
    internal static double Oetf(double e, DynamicRange dynamicRange)
    {
        // BT.2100 does not specify black lift correction for OETF (only referenced in EOTF)
        // but cannot roundtrip without it
        // assuming it is unspecified because HDR min luminance being close to zero results in almost zero
        // however, this should theoretically support SDR displays that are somehow HLG-compatible
        var beta = BlackLift(dynamicRange.MaxLuminance, dynamicRange.MinLuminance);
        e *= dynamicRange.HlgScale;
        return (Math.Max(beta, Oetf(e)) - beta) / (1 - beta);
    }

    internal static double Oetf(double e)
    {
        return e switch
        {
            <= 1 / 12.0 => Math.Sqrt(3 * e),
            _ => A * Math.Log(12 * e - B) + C
        };
    }

    internal static double BlackLift(double lw, double lb)
    {
        var gamma = lw switch
        {
            1000 => 1.2,
            >= 400 and <= 2000 => 1.2 + 0.42 * Math.Log10(lw / 1000.0),
            _ => 1.2 * Math.Pow(1.111, Math.Log(lw / 1000, 2))
        };
        
        return Math.Sqrt(3 * Math.Pow(lb / lw, 1 / gamma));
    }
}