namespace Wacton.Unicolour;

public record Tsl : ColourRepresentation
{
    protected internal override int? HueIndex => 0; 
    public double T => First;
    public double S => Second;
    public double L => Third;
    public double ConstrainedT => ConstrainedFirst;
    public double ConstrainedS => ConstrainedSecond;
    public double ConstrainedL => ConstrainedThird;
    protected override double ConstrainedFirst => T.Modulo(360.0);
    protected override double ConstrainedSecond => S.Clamp(0.0, 1.0);
    protected override double ConstrainedThird => L.Clamp(0.0, 1.0);
    internal override bool IsGreyscale => S <= 0.0 || L <= 0.0;

    public Tsl(double t, double s, double l) : this(t, s, l, ColourHeritage.None) {}
    internal Tsl(double t, double s, double l, ColourHeritage heritage) : base(t, s, l, heritage) {}
    
    protected override string String => UseAsHued ? $"{T:F1}° {S * 100:F1}% {L * 100:F1}%" : $"—° {S * 100:F1}% {L * 100:F1}%";
    public override string ToString() => base.ToString();
    
    /*
     * TSL is a transform of RGB
     * Forward: https://doi.org/10.1109/AFGR.2000.840612
     * Reverse: n/a - no published reverse transform so implemented my own
     */
    
    /*
     * NOTE: this colour space is unusual - tint is a weird form hue, RGB context is unknown, and no reverse transform is provided
     * hence the mini-essay of comments... 🫠
     * --------------------
     * T ranges from 0.0 (azure) -> 0.25 (chartreuse) -> 0.5 (orange) -> 0.75 (violet) -> 1.0 (azure)
     * despite being hue-like, T stops making sense outwith 0 - 1 (although it is still reversible mathematically)
     * presumably due to the use arctan, instead of being fully cyclical:
     * - T < 0 cycles the [0 - 0.5] region (azure -> chartreuse -> orange)
     * - T > 1 cycles the [0.5 - 1] region (orange -> violet -> azure)
     * RGB -> TSL can only produce 0 - 1, so outliers are likely due to user input or extrapolation
     * assumption is that because 0 - 1 looks cyclical, any consumer of TSL expects T 1.25 == T 0.25 (which mathematically is actually T 1.25 == T 0.75)
     * and there is no benefit to preserving the unintuitive behaviour beyond 0 - 1
     * --------------------
     * RGB -> TSL deviates from original algorithm: instead of g' = 0 -> T = 0 ... now T = 0 or T = 0.5 depending on the sign of r'
     * but g' can be 0 when RGB channels are weighted towards orange as well as azure, e.g.
     * - RGB [0.0, 0.5, 1.0] (azure)  -> g-chromaticity = 1/3 so g' = 0 and T = 0 -> azure
     * - RGB [1.0, 0.5, 0.0] (orange) -> g-chromaticity = 1/3 so g' = 0 and T = 0 -> also azure
     * we can detect when RGB channels are weighted towards azure or orange by using r', e.g.
     * - RGB [0.0, 0.5, 1.0] (azure)   -> g' = 0; r-chromaticity = 0   so r' = -1/3; T = 0.0 -> azure
     * - RGB [1.0, 0.5, 0.0] (orange)  -> g' = 0; r-chromaticity = 2/3 so r' = +1/3; T = 0.5 -> orange
     * outcome is that this will produce T of 0.5 instead of 0.0 in specific circumstances, which is a slight deviation but:
     * - A) behaves more intuitively (orange colours map to orange tint instead of azure tint)
     * - B) makes RGB <-> TSL a 1:1 roundtrip conversion
     * - C) doesn't appear to conflict with the intention of the algorithm
     * --------------------
     * despite being hue-like, T stops making sense outwith 0 - 1 (although it is still reversible mathematically)
     * presumably due to the arctan, instead of being cyclical:
     * - T < 0 cycles the [0 - 0.5] region (azure -> chartreuse -> orange)
     * - T > 1 cycles the [0.5 - 1] region (orange -> violet -> azure)
     * RGB -> TSL can only produce 0 - 1, so outliers are likely due to user input or extrapolation
     * assumption is that because 0 - 1 looks cyclical, any consumer of TSL expects T 1.25 == T 0.25 (which mathematically is actually T 1.25 == T 0.75)
     * and there is no benefit to preserving the unintuitive behaviour beyond 0 - 1
     * --------------------
     * the equation for luma in the paper is given as L = 0.299R + 0.587G + 0114B
     * (where luma is similar to relative luminance, but calculated on gamma-encoded RGB instead of linear RGB / Y of XYZ)
     * which is specific to Rec. 601; other specifications like Rec. 709 and Rec. 2020 that came after this paper use different luma coefficients
     * so making the assumption that the coefficients used by TSL should be adaptable to newer specifications
     * --------------------
     * it also raises the question: if Rec. 601 is used for the luma, what is the intended RGB configuration?
     * when the paper was written, Rec. 601 did not define RGB primaries, and sRGB was only recently created
     * so it is unclear which RgbConfig should be used alongside Rec. 601 luma to obtain the intended results
     * --------------------
     * this repurposes the luma coefficients used in YbrConfig, which is not so obvious, but is the primary usage of luma in Unicolour
     * if this became problematic, could be refactored into either
     * - dedicated TslConfig (seems overkill for such a niche colour space)
     * - separate YbrConfig into LumaConfig (just luma coefficients) and YbrConfig (range of Y and C)
     * --------------------
     * would be nice if luma coefficients could be derived from the RgbConfig, like relative luminance can, i.e. middle row of RGB -> XYZ matrix
     * however while luma coefficients for Rec. 709 and Rec. 2020 are the same coefficients as relative luminance
     * it seems to be more out of convenience than colorimetric necessity
     * and luma coefficients for Rec. 601 are based on the original NTSC RGB primaries (!)
     * which are not used by either Rec. 601 625-line (PAL & SECAM primaries) or Rec. 601 525-line (NTSC SMPTE-C primaries)
     * --------------------
     * 😵‍💫
     */
    internal static Tsl FromRgb(Rgb rgb, YbrConfiguration ybrConfig)
    {
        var kr = ybrConfig.Kr;
        var kb = ybrConfig.Kb;
        var kg = ybrConfig.Kg;
                
        var (r, g, b) = rgb;

        var sum = r + g + b;
        var rChromaticity = r / sum;
        var gChromaticity = g / sum;
        
        var rPrime = rChromaticity - 1 / 3.0;
        var gPrime = gChromaticity - 1 / 3.0;
        var tangent = rPrime / gPrime;

        const double tAzure = 0.0;
        const double tOrange = 0.5;
        var t = double.IsNaN(gPrime) ? tAzure : gPrime switch
        {
            > 0 => Math.Atan(tangent) / (2 * Math.PI) + 0.25,
            < 0 => Math.Atan(tangent) / (2 * Math.PI) + 0.75,
            _ => rPrime <= 0 ? tAzure : tOrange
        };

        var s = double.IsNaN(gPrime) 
            ? 0.0 
            : Math.Pow(9.0 / 5.0 * (Math.Pow(rPrime, 2) + Math.Pow(gPrime, 2)), 0.5);
        
        var l = kr * r + kg * g + kb * b;
        return new Tsl(t * 360, s, l, ColourHeritage.From(rgb));
    }
    
    internal static Rgb ToRgb(Tsl tsl, YbrConfiguration ybrConfig)
    {
        var kr = ybrConfig.Kr;
        var kb = ybrConfig.Kb;
        var kg = ybrConfig.Kg;
        
        var t = tsl.ConstrainedT / 360.0;
        var (_, s, l) = tsl;

        const double infinityTangent = 16331239353195370; // Math.Tan(Math.PI / 2)
        double tangent;
        bool gPrimeNegation;
        switch (t)
        {
            // technically the default case "just works" because tan(π/2) returns a very big number (1.63e+16) instead of infinity or undefined
            // however, that feels too incidental, so handling these cases explicitly for clarity, and better resembles the forward transform
            // 1.63e+16 is big enough to represent "a lot" but small enough that when used with S to calculate g', g' is very small but non-zero
            // and although g' = 0 is the most correct reverse transform, it makes calculating r' impossible - but a big number gives us a close approximation
            case 0.0:
                gPrimeNegation = false;
                tangent = -infinityTangent;
                break;
            case 0.5:
                gPrimeNegation = false;
                tangent = infinityTangent;
                break;
            default:
            {
                gPrimeNegation = t > 0.5;
                var offset = gPrimeNegation ? 0.75 : 0.25;
                var angle = (t - offset) * 2 * Math.PI;
                tangent = Math.Tan(angle);
                break;
            }
        }
        
        var gPrime = Math.Sqrt(5.0 / 9.0 * Math.Pow(s, 2) / (Math.Pow(tangent, 2) + 1));
        gPrime = gPrimeNegation ? -gPrime : gPrime;
        var rPrime = gPrime * tangent;
        
        var rChromaticity = rPrime + 1 / 3.0;
        var gChromaticity = gPrime + 1 / 3.0;
        var bChromaticity = 1 - rChromaticity - gChromaticity;
        var sum = l / (kr * rChromaticity + kg * gChromaticity + kb * bChromaticity);
        var (r, g, b) = (rChromaticity * sum, gChromaticity * sum, bChromaticity * sum);
        
        return new Rgb(r, g, b, ColourHeritage.From(tsl));
    }
}