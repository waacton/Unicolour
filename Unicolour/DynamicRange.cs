namespace Wacton.Unicolour;

public class DynamicRange
{
    // https://www.itu.int/pub/R-REP-BT.2408
    // reference level · HDR reference white · diffuse white · graphics white
    internal const int NominalWhiteLuminance = 203;
    
    /*
     * HLG displays need to decide what luminance to use for diffuse ("100%") white
     * nominally a value of 0.75 is used (https://www.itu.int/pub/R-REP-BT.2408)
     * e.g. if white luminance is intended to be 203 cd/m², that would correspond to a HLG signal of 0.75
     * so a scale is calculated based on this number and applied to linear light values
     */
    internal const double HlgDefaultWhiteLevel = 0.75;
    
    public static readonly DynamicRange Standard = new(100, 100, 0.1, HlgDefaultWhiteLevel, "SDR");
    public static readonly DynamicRange High = new(NominalWhiteLuminance, 1000, 0, HlgDefaultWhiteLevel, "HDR");
    
    public double WhiteLuminance { get; }
    public double MaxLuminance { get; }
    public double MinLuminance { get; }
    public double HlgWhiteLevel { get; }
    public string Name { get; }
    
    /*
     * used to scale linear values so that white = HlgWhiteLevel (0.75 by default)
     * there's a lot of inconsistency online regarding this value, which always seems to be hardcoded with little explanation
     * but it's usually ~0.265:
     * --- 0.26496256042100724 (inverse OETF of 0.75) [https://lists.w3.org/Archives/Public/public-colorweb/2021Sep/0008.html · https://github.com/w3c/ColorWeb-CG/blob/7d550030e6547439cae73fb7008ee890e6fd0b35/hdr_html_canvas_element.md#conversions-via-color-connection-space] 
     * --- 0.26472534745201853 (80 / 302.2; sRGB peak luminance / derived HLG nominal peak luminance) [https://github.com/w3c/ColorWeb-CG/blob/955eead439475987e5f284c7fd1834b0f308d917/hdr_html_canvas_element.md]
     * --- 0.264949 (actually 1 / 3.7743; no explanation given) [https://github.com/color-js/color.js/blob/d88c6a79c0f222fbbb8f69f8c4489e0181c87588/src/spaces/rec2100-hlg.js#L9]
     * Unicolour takes white luminance into account and generates the following values for predefined configs:
     * --- HDR: 0.26496255978640015 (InverseOETF(0.75) * 203/203)
     * --- SDR: 0.13052342846620699 (InverseOETF(0.75) * 203/100)
     */
    internal double HlgScale { get; }
    
    // linear values below this are clipped to this value in the HLG OETF; it is the smallest value that Inverse OETF can generate (i.e. when input = 0)
    // = Hlg.InverseOetf(Hlg.BlackLift(MaxLuminance, MinLuminance)) / HlgScale
    internal double HlgMinLinear => Hlg.InverseOetf(0, dynamicRange: this);
    
    public DynamicRange(double whiteLuminance, double maxLuminance, double minLuminance, double hlgWhiteLevel = HlgDefaultWhiteLevel, string name = Utils.Unnamed)
    {
        WhiteLuminance = whiteLuminance;
        MaxLuminance = maxLuminance;
        MinLuminance = minLuminance;
        HlgWhiteLevel = hlgWhiteLevel;
        HlgScale = Hlg.InverseOetf(hlgWhiteLevel) * (WhiteLuminance / NominalWhiteLuminance);
        Name = name;
    }

    public override string ToString() => $"{Name} · white {WhiteLuminance} · max {MaxLuminance} · min {MinLuminance} · HLG white level {HlgWhiteLevel * 100}%";
}