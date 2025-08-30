namespace Wacton.Unicolour.Icc;

internal abstract class Transform
{
    protected static readonly double[] RefWhite = { 0.9642, 1.0000, 0.8249 };
    private static readonly double[] RefBlack = { 0.00336, 0.0034731, 0.00287 };
    internal static readonly XyzConfiguration XyzD50 = new(WhitePoint.FromXyz(new Xyz(RefWhite[0], RefWhite[1], RefWhite[2])), "ICC XYZ D50");

    private readonly Header header;
    protected readonly Tags tags;
    private int ProfileVersion => header.ProfileVersion.Major;
    protected bool IsLabPcs => header.Pcs == Signatures.Lab;
    private double[] MediaWhite => tags.MediaWhite.Value!.ToArray();
    
    /*
     * note: not header.PcsIlluminant!
     * as my previous comments wondered about, the illuminant should be set to D50 (4dp) to meet ICC specifications
     * but for some reason many profiles do not adhere to the spec, so ignoring custom illuminant in line with the reference implementation
     * however v5+ / iccMAX will need to take it into account under certain conditions
     * ----------
     * see also the discussion I raised here: https://github.com/InternationalColorConsortium/DemoIccMAX/issues/164#issuecomment-3221521027
     * from Max Derhak:
     * - "It uses the XYZ specified by the ICC.1 specification if the version is less than 5"
     * - "If the spectral viewing conditions are set to use D50 with 2 degree observer, then the standard V4 XYZ illuminant values will be used.
     *    Otherwise the header illuminant XYZ values get used"
     */
    private double[] PcsIlluminant => RefWhite; 

    private readonly bool hasPerceptualHandling;
    
    protected Transform(Header header, Tags tags, bool hasPerceptualHandling)
    {
        this.header = header;
        this.tags = tags;
        this.hasPerceptualHandling = hasPerceptualHandling;
    }

    internal abstract double[] ToXyz(double[] deviceValues, Intent intent);
    internal abstract double[] FromXyz(double[] xyz, Intent intent);
    
    // A -> CLUT -> B (or input -> CLUT -> output)
    internal static double[] AB(double[] aCurveInputs, List<Curve> aCurves, Clut clut, List<Curve> bCurves)
    {
        var clutInputs = ApplyCurves(aCurves, aCurveInputs);
        var bCurveInputs = clut.Lookup(clutInputs);
        return ApplyCurves(bCurves, bCurveInputs);
    }
    
    // B -> CLUT -> A (or input -> CLUT -> output)
    internal static double[] BA(double[] bCurveInputs, List<Curve> bCurves, Clut clut, List<Curve> aCurves)
    {
        var clutInputs = ApplyCurves(bCurves, bCurveInputs);
        var aCurveInputs = clut.Lookup(clutInputs);
        return ApplyCurves(aCurves, aCurveInputs);
    }
    
    // A -> CLUT -> M -> Matrix -> B
    internal static double[] AMB(double[] aCurveInputs, List<Curve> aCurves, Clut clut, List<Curve> mCurves, Matrices matrices, List<Curve> bCurves)
    {
        var clutInputs = ApplyCurves(aCurves, aCurveInputs);
        var mCurveInputs = clut.Lookup(clutInputs);
        var matrixInputs = ApplyCurves(mCurves, mCurveInputs);
        var bCurveInputs = matrices.Apply(matrixInputs);
        return ApplyCurves(bCurves, bCurveInputs);
    }
    
    // B -> Matrix -> M -> CLUT -> A
    internal static double[] BMA(double[] bCurveInputs, List<Curve> bCurves, Matrices matrices, List<Curve> mCurves, Clut clut, List<Curve> aCurves)
    {
        var matrixInputs = ApplyCurves(bCurves, bCurveInputs);
        var mCurveInputs = matrices.Apply(matrixInputs);
        var clutInputs = ApplyCurves(mCurves, mCurveInputs);
        var aCurveInputs = clut.Lookup(clutInputs);
        return ApplyCurves(aCurves, aCurveInputs);
    }
    
    // M -> Matrix -> B
    internal static double[] MB(double[] mCurveInputs, List<Curve> mCurves, Matrices matrices, List<Curve> bCurves)
    {
        var matrixInputs = ApplyCurves(mCurves, mCurveInputs);
        var bCurveInputs = matrices.Apply(matrixInputs);
        return ApplyCurves(bCurves, bCurveInputs);
    }
    
    // B -> Matrix -> M
    internal static double[] BM(double[] bCurveInputs, List<Curve> bCurves, Matrices matrices, List<Curve> mCurves)
    {
        var matrixInputs = ApplyCurves(bCurves, bCurveInputs);
        var mCurveInputs = matrices.Apply(matrixInputs);
        return ApplyCurves(mCurves, mCurveInputs);
    }
    
    // B
    internal static double[] B(double[] bCurveInputs, List<Curve> bCurves)
    {
        return ApplyCurves(bCurves, bCurveInputs);
    }
    
    private static double[] ApplyCurves(List<Curve> curves, double[] inputs)
    {
        var outputs = new double[curves.Count];
        for (var i = 0; i < curves.Count; i++)
        {
            // default to 0.0 if not enough channels have been provided
            // (e.g. using 7-channel Fogra55 but only input 4 dimensions, last 3 dimensions default to 0.0)
            var input = i >= inputs.Length ? 0.0 : inputs[i];
            outputs[i] = curves[i].Lookup(input);
        }

        return outputs;
    }
    
    private enum PcsAdjustment
    {
        None,
        Perceptual,
        AbsoluteColorimetric
    }
    
    protected double[] AdjustXyz(double[] xyz, Intent intent, bool isDeviceToPcs)
    {
        var pcsAdjustment = intent switch
        {
            Intent.Perceptual when ProfileVersion == 2 || !hasPerceptualHandling => PcsAdjustment.Perceptual,
            Intent.AbsoluteColorimetric => PcsAdjustment.AbsoluteColorimetric,
            _ => PcsAdjustment.None
        };

        /*
         * when using LAB PCS, negative values are clipped before PCS adjustment
         * see DemoIccMAX --- IccCmm.cpp > CIccXform::AdjustPCS > CIccPCS::LabToXyz > IccUtil.cpp icLabtoXYZ > icICubeth (assumes #ifndef SAMPLEICC_NOCLIPLABTOXYZ)
         * triggered by CIccXform::CheckDstAbs after transform when device-to-PCS
         * triggered by CIccXform::CheckSrcAbs before transform when PCS-to-device
         */
        if (IsLabPcs)
        {
            xyz = pcsAdjustment switch
            {
                PcsAdjustment.None => xyz,
                _ => NegativeClip(xyz)
            };
        }
        
        xyz = pcsAdjustment switch
        {
            PcsAdjustment.Perceptual => AdjustXyzPerceptual(xyz, isDeviceToPcs),
            PcsAdjustment.AbsoluteColorimetric => AdjustXyzAbsolute(xyz, isDeviceToPcs),
            _ => xyz
        };

        /*
         * when using XYZ PCS, negative values are clipped after PCS adjustment
         * see DemoIccMAX --- IccCmm.cpp > CIccXform::AdjustPCS > CIccPCS::NegClip (assumes #ifndef SAMPLEICC_NOCLIPLABTOXYZ)
         * triggered by CIccXform::CheckDstAbs after transform when device-to-PCS
         * triggered by CIccXform::CheckSrcAbs before transform when PCS-to-device
         */
        if (!IsLabPcs)
        {
            xyz = pcsAdjustment switch
            {
                PcsAdjustment.None => xyz,
                _ => NegativeClip(xyz)
            };
        }
        
        return xyz;
    }
    
    private static double[] AdjustXyzPerceptual(double[] xyz, bool isDeviceToPcs)
    {
        double GetScale(int i) => isDeviceToPcs ? 1 - RefBlack[i] / RefWhite[i] : 1 / (1 - RefBlack[i] / RefWhite[i]);
        double GetOffset(int i) => isDeviceToPcs ? RefBlack[i] : -RefBlack[i] * GetScale(i);
        double GetAdjusted(int i) => xyz[i] * GetScale(i) + GetOffset(i);
        
        var adjustedXyz = Enumerable.Range(0, 3).Select(GetAdjusted).ToArray();
        return adjustedXyz;
    }

    private double[] AdjustXyzAbsolute(double[] xyz, bool isDeviceToPcs)
    {
        double GetScale(int i) => isDeviceToPcs ? MediaWhite[i] / PcsIlluminant[i] : PcsIlluminant[i] / MediaWhite[i];
        double GetAdjusted(int i) => xyz[i] * GetScale(i);
        
        var adjustedXyz = Enumerable.Range(0, 3).Select(GetAdjusted).ToArray();
        return adjustedXyz;
    }
    
    internal static double[] IccLabToLab(double[] iccLab)
    {
        var l = iccLab[0] * 100;
        var a = iccLab[1] * (127 + 128) - 128;
        var b = iccLab[2] * (127 + 128) - 128;
        return new[] { l, a, b };
    }

    internal static double[] LabToIccLab(double[] lab)
    {
        var l = lab[0] / 100.0;
        var a = (lab[1] + 128) / (127 + 128);
        var b = (lab[2] + 128) / (127 + 128);
        return new[] { l, a, b };
    }
    
    internal static double[] IccLab2ToIccLab4(double[] iccLab2) => iccLab2.Select(x => x * 65535.0 / 65280.0).ToArray();
    internal static double[] IccLab4ToIccLab2(double[] iccLab2) => iccLab2.Select(x => x * 65280.0 / 65535.0).ToArray();

    internal static double[] XyzToIccXyz(double[] xyz) => xyz.Select(XyzToIccXyz).ToArray();
    internal static double[] IccXyzToXyz(double[] iccXyz) => iccXyz.Select(IccXyzToXyz).ToArray();
    
    private static double XyzToIccXyz(double value) => value * 32768.0 / 65535.0;
    private static double IccXyzToXyz(double value) => value * 65535.0 / 32768.0;
    
    internal static double[] LabToXyz(double[] lab) => Lab.ToXyz(new Lab(lab[0], lab[1], lab[2]), XyzD50).ToArray();
    internal static double[] XyzToLab(double[] xyz) => Lab.FromXyz(new Xyz(xyz[0], xyz[1], xyz[2]), XyzD50).ToArray();

    private static double[] NegativeClip(double[] xyz) => xyz.Select(NegativeClip).ToArray();
    private static double NegativeClip(double value) => Math.Max(value, 0);
}