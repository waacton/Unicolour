namespace Wacton.Unicolour.Icc;

internal class TransformAToB : Transform
{
    private readonly Lazy<Luts?> ab0;
    private readonly Lazy<Luts?> ab1;
    private readonly Lazy<Luts?> ab2;
    
    private readonly Lazy<Luts?> ba0;
    private readonly Lazy<Luts?> ba1;
    private readonly Lazy<Luts?> ba2;
    
    internal TransformAToB(Header header, Tags tags) 
        : base(header, tags, hasPerceptualHandling: true)
    {
        ab0 = tags.AToB0;
        ab1 = tags.AToB1;
        ab2 = tags.AToB2;
        ba0 = tags.BToA0;
        ba1 = tags.BToA1;
        ba2 = tags.BToA2;
    }

    internal override double[] ToXyz(double[] deviceValues, Intent intent)
    {
        var lazyLuts = intent switch
        {
            Intent.Perceptual => ab0,
            Intent.RelativeColorimetric => tags.Has(Signatures.AToB1) ? ab1 : ab0,
            Intent.Saturation => tags.Has(Signatures.AToB2) ? ab2 : ab0,
            Intent.AbsoluteColorimetric => tags.Has(Signatures.AToB1) ? ab1 : ab0,
            _ => throw new ArgumentOutOfRangeException(nameof(intent), intent, null)
        };
        
        var luts = lazyLuts.Value!;
        var pcsValues = ToPcs(deviceValues, luts);
        
        double[] xyz;
        if (IsLabPcs)
        {
            var iccLab = pcsValues;
            iccLab = luts.Type == LutType.Lut16 ? IccLab2ToIccLab4(iccLab) : iccLab;
            var lab = IccLabToLab(iccLab);
            xyz = LabToXyz(lab);
        }
        else
        {
            var iccXyz = pcsValues;
            xyz = IccXyzToXyz(iccXyz);
        }
        
        return AdjustXyz(xyz, intent, isDeviceToPcs: true);
    }

    internal override double[] FromXyz(double[] xyz, Intent intent)
    {
        // unlike TRC transforms which are easily reversible
        // AToB transform needs explicit tag for the reverse operation, which is not guaranteed to be present
        // (e.g. input `scnr` profiles only need scanner device -> PCS, little need to convert the other way)
        if (!tags.Has(Signatures.BToA0))
        {
            throw new ArgumentException("BToA transform is not defined");
        }
        
        var lazyLuts = intent switch
        {
            Intent.Perceptual => ba0,
            Intent.RelativeColorimetric => tags.Has(Signatures.BToA1) ? ba1 : ba0,
            Intent.Saturation => tags.Has(Signatures.BToA2) ? ba2 : ba0,
            Intent.AbsoluteColorimetric => tags.Has(Signatures.BToA1) ? ba1 : ba0,
            _ => throw new ArgumentOutOfRangeException(nameof(intent), intent, null)
        };

        var luts = lazyLuts.Value!;
        
        xyz = AdjustXyz(xyz, intent, isDeviceToPcs: false);
        
        double[] pcsValues;
        if (IsLabPcs)
        {
            var lab = XyzToLab(xyz);
            var iccLab = LabToIccLab(lab);
            iccLab = luts.Type == LutType.Lut16 ? IccLab4ToIccLab2(iccLab) : iccLab;
            pcsValues = iccLab;
        }
        else
        {
            var iccXyz = XyzToIccXyz(xyz);
            pcsValues = iccXyz;
        }
        
        return ToDevice(pcsValues, luts);
    }
    
    private static double[] ToPcs(double[] deviceValues, Luts luts) => ApplyLuts(deviceValues, luts);
    private static double[] ToDevice(double[] pcsValues, Luts luts) => ApplyLuts(pcsValues, luts);

    private static double[] ApplyLuts(double[] inputValues, Luts luts)
    {
        var outputChannels = luts.Elements switch
        {
            LutElements.AB => luts.BCurves.Count,
            LutElements.BA => luts.ACurves!.Count,
            LutElements.AMB => luts.BCurves.Count,
            LutElements.BMA => luts.ACurves!.Count,
            LutElements.MB => luts.BCurves.Count,
            LutElements.BM => luts.MCurves!.Count,
            _ => luts.BCurves.Count
        };
        
        if (inputValues.Any(double.IsNaN))
        {
            return Enumerable.Range(0, outputChannels).Select(_ => double.NaN).ToArray();
        }

        return luts.Elements switch
        {
            LutElements.AB => AB(inputValues, luts.ACurves!, luts.Clut!, luts.BCurves),
            LutElements.BA => BA(inputValues, luts.BCurves, luts.Clut!, luts.ACurves!),
            LutElements.AMB => AMB(inputValues, luts.ACurves!, luts.Clut!, luts.MCurves!, luts.Matrices!, luts.BCurves),
            LutElements.BMA => BMA(inputValues, luts.BCurves, luts.Matrices!, luts.MCurves!, luts.Clut!, luts.ACurves!),
            LutElements.MB => MB(inputValues, luts.MCurves!, luts.Matrices!, luts.BCurves),
            LutElements.BM => BM(inputValues, luts.BCurves, luts.Matrices!, luts.MCurves!),
            _ => B(inputValues, luts.BCurves)
        };
    }
}