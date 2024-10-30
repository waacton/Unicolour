namespace Wacton.Unicolour.Icc;

internal static class Convert
{
    internal static double[] IccLabToXyz(double[] iccLab, Intent intent, LutType lutType, double[] refBlack, double[] refWhite, double[] mediaWhite, int version)
    {
        const bool isDeviceToPcs = true;
        
        iccLab = lutType == LutType.Lut16 ? IccLab2ToIccLab4(iccLab) : iccLab;
        switch (intent)
        {
            case Intent.Perceptual when version == 2:
            {
                var iccXyz = IccLabToIccXyz(iccLab);
                var adjustedIccXyz = IccXyzToAdjustedPerceptual(iccXyz, refBlack, refWhite, isDeviceToPcs);
                return IccXyzToXyz(adjustedIccXyz);
            }
            case Intent.AbsoluteColorimetric:
            {
                var iccXyz = IccLabToIccXyz(iccLab);
                var adjustedIccXyz = IccXyzToAdjustedAbsolute(iccXyz, refWhite, mediaWhite, isDeviceToPcs);
                return IccXyzToXyz(adjustedIccXyz);
            }
            default:
            {
                var lab = IccLabToLab(iccLab);
                return LabToXyz(lab);
            }
        }
    }
    
    internal static double[] XyzToIccLab(double[] xyz, Intent intent, LutType lutType, double[] refBlack, double[] refWhite, double[] mediaWhite, int version)
    {
        const bool isDeviceToPcs = false;
        
        switch (intent)
        {
            case Intent.Perceptual when version == 2:
            {
                var iccXyz = XyzToIccXyz(xyz, isFromLab: true).ToArray();
                var adjustedIccXyz = IccXyzToAdjustedPerceptual(iccXyz, refBlack, refWhite, isDeviceToPcs);
                var adjustedIccLab = AdjustedIccXyzToLab4(adjustedIccXyz);
                return IccLab4ToIccLab2(adjustedIccLab);
            }
            case Intent.AbsoluteColorimetric:
            {
                var iccXyz = XyzToIccXyz(xyz, isFromLab: true).ToArray();
                var adjustedIccXyz = IccXyzToAdjustedAbsolute(iccXyz, refWhite, mediaWhite, isDeviceToPcs);
                var adjustedIccLab = AdjustedIccXyzToLab4(adjustedIccXyz);
                return version == 2 ? IccLab4ToIccLab2(adjustedIccLab) : adjustedIccLab;
            }
            default:
            {
                var lab = XyzToLab(xyz);
                var iccLab = LabToIccLab(lab);
                return lutType == LutType.Lut16 ? IccLab4ToIccLab2(iccLab) : iccLab;
            }
        }
    }
    
    internal static double[] IccXyzToXyz(double[] iccXyz, Intent intent, double[] refWhite, double[] mediaWhite)
    {
        const bool isDeviceToPcs = true;
        switch (intent)
        {
            case Intent.AbsoluteColorimetric:
            {
                var adjustedIccXyz = IccXyzToAdjustedAbsolute(iccXyz, refWhite, mediaWhite, isDeviceToPcs);
                var clippedAdjustedIccXyz = adjustedIccXyz.Select(value => Math.Max(0, value)).ToArray(); // assumes DemoIccMAX #ifndef SAMPLEICC_NOCLIPLABTOXYZ
                return IccXyzToXyz(clippedAdjustedIccXyz);
            }
            default:
            {
                return IccXyzToXyz(iccXyz);
            }
        }
    }
    
    internal static double[] XyzToIccXyz(double[] xyz, Intent intent, double[] refWhite, double[] mediaWhite)
    {
        const bool isDeviceToPcs = false;
        switch (intent)
        {
            case Intent.AbsoluteColorimetric:
            {
                var iccXyz = XyzToIccXyz(xyz, isFromLab: false);
                var adjustedIccXyz = IccXyzToAdjustedAbsolute(iccXyz, refWhite, mediaWhite, isDeviceToPcs);
                var clippedAdjustedIccXyz = adjustedIccXyz.Select(value => Math.Max(0, value)).ToArray(); // assumes DemoIccMAX #ifndef SAMPLEICC_NOCLIPLABTOXYZ
                return clippedAdjustedIccXyz;
            }
            default:
            {
                return XyzToIccXyz(xyz, isFromLab: false);
            }
        }
    }
    
    private static double[] IccLabToLab(double[] iccLab)
    {
        var l = iccLab[0] * 100;
        var a = iccLab[1] * (127 + 128) - 128;
        var b = iccLab[2] * (127 + 128) - 128;
        return new[] { l, a, b };
    }

    private static double[] LabToIccLab(double[] lab)
    {
        var l = lab[0] / 100.0;
        var a = (lab[1] + 128) / (127 + 128);
        var b = (lab[2] + 128) / (127 + 128);
        return new[] { l, a, b };
    }
    
    private static double[] IccLab2ToIccLab4(double[] iccLab2) => iccLab2.Select(x => x * 65535.0 / 65280.0).ToArray();
    private static double[] IccLab4ToIccLab2(double[] iccLab2) => iccLab2.Select(x => x * 65280.0 / 65535.0).ToArray();
    
    private static double[] IccLabToIccXyz(double[] iccLab)
    {
        var lab = IccLabToLab(iccLab);
        var xyz = LabToXyz(lab);
        return XyzToIccXyz(xyz, isFromLab: true);
    }

    // XYZ values are clipped in DemoIccMAX XYZ -> LAB conversion (IccUtil.cpp : icLabToXYZ > icICubeth)
    private static double[] XyzToIccXyz(double[] xyz, bool isFromLab) => xyz.Select(value => XyzToIccXyz(isFromLab ? Math.Max(value, 0) : value)).ToArray();
    private static double XyzToIccXyz(double value) => value * 32768.0 / 65535.0;
    private static double[] IccXyzToXyz(double[] iccXyz) => iccXyz.Select(x => x * 65535.0 / 32768.0).ToArray();
    
    private static double[] IccXyzToAdjustedPerceptual(double[] iccXyz, double[] refBlack, double[] refWhite, bool isDeviceToPcs)
    {
        double GetScale(int i) => isDeviceToPcs ? 1 - refBlack[i] / refWhite[i] : 1 / (1 - refBlack[i] / refWhite[i]);
        double GetOffset(int i) => isDeviceToPcs ? XyzToIccXyz(refBlack[i]) : -XyzToIccXyz(refBlack[i]) * GetScale(i);
        double GetAdjusted(int i) => iccXyz[i] * GetScale(i) + GetOffset(i);
        
        var adjustedIccXyz = Enumerable.Range(0, 3).Select(GetAdjusted).ToArray();
        return adjustedIccXyz;
    }
    
    private static double[] IccXyzToAdjustedAbsolute(double[] iccXyz, double[] refWhite, double[] mediaWhite, bool isDeviceToPcs)
    {
        double GetScale(int i) => isDeviceToPcs ? mediaWhite[i] / refWhite[i] : refWhite[i] / mediaWhite[i];
        double GetAdjusted(int i) => iccXyz[i] * GetScale(i);
        
        var adjustedIccXyz = Enumerable.Range(0, 3).Select(GetAdjusted).ToArray();
        return adjustedIccXyz;
    }
    
    private static double[] AdjustedIccXyzToLab4(double[] adjustedIccXyz)
    {
        var adjustedXyz = IccXyzToXyz(adjustedIccXyz);
        var adjustedLab = XyzToLab(adjustedXyz);
        var adjustedIccLab4 = LabToIccLab(adjustedLab);
        return adjustedIccLab4;
    }
    
    private static double[] LabToXyz(double[] lab) => Lab.ToXyz(new Lab(lab[0], lab[1], lab[2]), Profile.XyzD50).Triplet.ToArray();
    private static double[] XyzToLab(double[] xyz) => Lab.FromXyz(new Xyz(xyz[0], xyz[1], xyz[2]), Profile.XyzD50).Triplet.ToArray();
}