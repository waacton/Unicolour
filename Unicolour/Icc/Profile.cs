namespace Wacton.Unicolour.Icc;

public class Profile
{
    private static readonly double[] RefWhite = { 0.9642, 1.0000, 0.8249 };
    private static readonly double[] RefBlack = { 0.00336, 0.0034731, 0.00287 };
    internal static readonly XyzConfiguration XyzD50 = new(WhitePoint.FromXyz(new Xyz(RefWhite[0], RefWhite[1], RefWhite[2])), "ICC XYZ D50");
    
    public FileInfo FileInfo { get; }
    public Header Header { get; }
    public Tags Tags { get; }
    
    private double[] mediaWhite => Tags.MediaWhite.Value.ToArray();
    
    public Profile(string filePath)
    {
        FileInfo = new FileInfo(filePath);
        if (FileInfo.Length < 128)
        {
            throw new ArgumentException($"[{FileInfo}] does not contain enough bytes to be an ICC profile");
        }

        try
        {
            Header = Header.FromFile(FileInfo);
            Tags = Tags.FromFile(FileInfo);
        }
        catch (Exception e)
        {
            throw new ArgumentException($"[{FileInfo}] could not be parsed as ICC data", e);
        }
    }

    internal void ErrorIfUnsupported(Intent intent)
    {
        if (Header.ProfileFileSignature != Signatures.Profile)
        {
            throw new ArgumentException($"[{FileInfo}] signature is incorrect: expected [{Signatures.Profile}] but was [{Header.ProfileFileSignature}]");
        }
        
        ErrorIfUnsupportedHeader();
        ErrorIfUnsupportedIntent(intent);
    }

    private void ErrorIfUnsupportedHeader()
    {
        var isHeaderSupported = Header is { Pcs: Signatures.Lab, ProfileClass: Signatures.Output };
        if (isHeaderSupported) return;
        const string expected = $"PCS {Signatures.Lab}, Profile class {Signatures.Output}";
        var actual = $"PCS {Header.Pcs}, Class {Header.ProfileClass}";
        throw new ArgumentException($"[{FileInfo}] is not supported: expected [{expected}] but was [{actual}]");
    }

    private void ErrorIfUnsupportedIntent(Intent intent)
    {
        var lutsSignatures = new List<string> { GetLutsSignature(intent, true), GetLutsSignature(intent, false) };
        
        var requiredSignatures = new List<string>(lutsSignatures);
        if (intent == Intent.AbsoluteColorimetric)
        {
            requiredSignatures.Add(Signatures.MediaWhitePoint);
        }
        
        var signatures = Tags.Select(x => x.Signature).ToList();
        foreach (var requiredSignature in requiredSignatures)
        {
            var isMissing = !signatures.Contains(requiredSignature);
            if (isMissing)
            {
                throw new ArgumentException($"[{FileInfo}] with intent [{intent}] is not supported: tag [{requiredSignature}] is required");
            }
        }

        // NOTE: this will be removed once "lutAToB" and "lutBToA" types (v4+) are supported
        foreach (var lutsSignature in lutsSignatures)
        {
            var tag = Tags.Single(x => x.Signature == lutsSignature);
            using var stream = new MemoryStream(tag.Data);
            var mftSignature = stream.ReadSignature();
            if (mftSignature is not (Signatures.MultiFunctionTable1Byte or Signatures.MultiFunctionTable2Byte))
            {
                throw new ArgumentException($"[{FileInfo}] with intent [{intent}] is not supported: tag [{lutsSignature}] " +
                                            $"expected [{Signatures.MultiFunctionTable1Byte}] or [{Signatures.MultiFunctionTable2Byte}] but was [{mftSignature}]");
            }
        }
    }
    
    internal Xyz ToXyz(double[] deviceValues, XyzConfiguration xyzConfig, Intent intent)
    {
        var xyzD50 = ToXyzStandardD50(deviceValues, intent);
        return Convert.XyzD50ToXyz(xyzD50, xyzConfig);
    }
    
    // NOTE: iccMAX allows "profile connection conditions" (customToStandardPCC, standardToCustomPCC)
    // but if it is ever implemented, it probably doesn't change this device-to-"StandardD50" calculation
    internal Xyz ToXyzStandardD50(double[] deviceValues, Intent intent)
    {
        const bool isDeviceToPcs = true;
        var luts = Tags.GetLuts(GetLutsSignature(intent, isDeviceToPcs));
        
        var pcsValues = ApplyTransform(deviceValues, luts);
        
        // NOTE: adjustment of PCS happens in more cases than these; expand when a wider variety of profiles are supported
        var iccLab = luts.Is16Bit ? Convert.IccLab2ToIccLab4(pcsValues) : pcsValues;
        switch (intent)
        {
            case Intent.Perceptual when Header.ProfileVersion.Major == 2:
            {
                var iccXyz = Convert.IccLabToIccXyz(iccLab);
                var adjustedIccXyz = Convert.IccXyzToAdjustedPerceptual(iccXyz, RefBlack, RefWhite, isDeviceToPcs);
                return Convert.IccXyzToXyz(adjustedIccXyz);
            }
            case Intent.AbsoluteColorimetric:
            {
                var iccXyz = Convert.IccLabToIccXyz(iccLab);
                var adjustedIccXyz = Convert.IccXyzToAdjustedAbsolute(iccXyz, RefWhite, mediaWhite, isDeviceToPcs);
                return Convert.IccXyzToXyz(adjustedIccXyz);
            }
            default:
            {
                var lab = Convert.IccLabToLab(iccLab);
                return Lab.ToXyz(lab, XyzD50);
            }
        }
    }
    
    internal double[] FromXyz(Xyz xyz, XyzConfiguration xyzConfig, Intent intent)
    {
        var xyzD50 = Convert.XyzToD50Xyz(xyz, xyzConfig);
        return FromStandardXyzD50(xyzD50, intent);
    }
    
    // NOTE: iccMAX allows "profile connection conditions" (customToStandardPCC, standardToCustomPCC)
    // but if it is ever implemented, it probably doesn't change this "StandardD50"-to-device calculation
    internal double[] FromStandardXyzD50(Xyz xyz, Intent intent)
    {
        const bool isDeviceToPcs = false;
        var luts = Tags.GetLuts(GetLutsSignature(intent, isDeviceToPcs));
        
        // NOTE: adjustment of PCS happens in more cases than these; expand when a wider variety of profiles are supported
        double[] pcsValues;
        switch (intent)
        {
            case Intent.Perceptual when Header.ProfileVersion.Major == 2:
            {
                var iccXyz = Convert.XyzToIccXyz(xyz);
                var adjustedIccXyz = Convert.IccXyzToAdjustedPerceptual(iccXyz, RefBlack, RefWhite, isDeviceToPcs);
                pcsValues = Convert.AdjustedIccXyzToLab2(adjustedIccXyz);
                break;
            }
            case Intent.AbsoluteColorimetric:
            {
                var iccXyz = Convert.XyzToIccXyz(xyz);
                var adjustedIccXyz = Convert.IccXyzToAdjustedAbsolute(iccXyz, RefWhite, mediaWhite, isDeviceToPcs);
                pcsValues = Convert.AdjustedIccXyzToLab2(adjustedIccXyz);
                break;
            }
            default:
            {
                var lab = Lab.FromXyz(xyz, XyzD50);
                var iccLab = Convert.LabToIccLab(lab);
                pcsValues = luts.Is16Bit ? Convert.IccLab4ToIccLab2(iccLab) : iccLab;
                break;
            }
        }

        var deviceValues = ApplyTransform(pcsValues, luts);
        return deviceValues;
    }
    
    private static string GetLutsSignature(Intent intent, bool isDeviceToPcs)
    {
        var signature = intent switch
        {
            Intent.Perceptual => isDeviceToPcs ? Signatures.AToB0 : Signatures.BToA0,
            Intent.RelativeColorimetric => isDeviceToPcs ? Signatures.AToB1 : Signatures.BToA1,
            Intent.Saturation => isDeviceToPcs ? Signatures.AToB2 : Signatures.BToA2,
            Intent.AbsoluteColorimetric => isDeviceToPcs ? Signatures.AToB1 : Signatures.BToA1,
            // v5+ / iccMAX: use D2B3 and B2D3 tags instead if present
            _ => throw new ArgumentOutOfRangeException(nameof(intent), intent, null)
        };

        return signature;
    }
    
    private static double[] ApplyTransform(double[] inputValues, Luts luts)
    {
        var inputCount = luts.InputCurves.Count;
        var outputCount = luts.OutputCurves.Count;
        
        if (inputValues.Any(double.IsNaN))
        {
            return Enumerable.Range(0, luts.OutputCurves.Count).Select(_ => double.NaN).ToArray();
        }
        
        var afterInputCurve = new double[inputCount];
        for (var i = 0; i < inputCount; i++)
        {
            // default to 0.0 if not enough channels have been provided
            // (e.g. using 7CLR Fogra55 but only input 4 values, last 3 dimensions default to 0.0)
            var value = i < inputValues.Length ? inputValues[i] : 0.0;
            var curve = luts.InputCurves[i];
            afterInputCurve[i] = curve.Lookup(value);
        }
        
        var afterClut = luts.Clut.Lookup(afterInputCurve);

        var afterOutputCurve = new double[outputCount];
        for (var i = 0; i < outputCount; i++)
        {
            var value = afterClut[i];
            var curve = luts.OutputCurves[i];
            afterOutputCurve[i] = curve.Lookup(value);
        }

        return afterOutputCurve;
    }

    // not useful until v4 profiles are supported (nothing to compare it to until then)
    // private static readonly int[] indexesToZeroForHash = { 44, 45, 46, 47, 64, 65, 66, 67, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
    // public byte[] CalculateProfileId()
    // {
    //     var bytes = File.ReadAllBytes(FileInfo.FullName);
    //     foreach (var index in indexesToZeroForHash)
    //     {
    //         bytes[index] = 0;
    //     }
    //     
    //     var md5 = MD5.Create();
    //     return md5.ComputeHash(bytes);
    // }

    public override string ToString() => $"{FileInfo} · {Header} · {Tags.Count} tags";
}
