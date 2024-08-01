namespace Wacton.Unicolour.Icc;

public static class Convert
{
    internal static Lab IccLabToLab(double[] iccLab)
    {
        var l = iccLab[0] * 100;
        var a = iccLab[1] * (127 + 128) - 128;
        var b = iccLab[2] * (127 + 128) - 128;
        return new Lab(l, a, b);
    }

    internal static double[] LabToIccLab(Lab lab)
    {
        return new[]
        {
            lab.L / 100.0,
            (lab.A + 128) / (127 + 128),
            (lab.B + 128) / (127 + 128)
        };
    }
    
    internal static double[] IccLab2ToIccLab4(double[] iccLab2) => iccLab2.Select(x => x * 65535.0 / 65280.0).ToArray();
    internal static double[] IccLab4ToIccLab2(double[] iccLab2) => iccLab2.Select(x => x * 65280.0 / 65535.0).ToArray();
    
    internal static double[] IccLabToIccXyz(double[] iccLab)
    {
        var lab = IccLabToLab(iccLab);
        var xyz = Lab.ToXyz(lab, Profile.XyzD50);
        return XyzToIccXyz(xyz); // TODO: take into account clipping, find out how clipping gets set
    }

    internal static Xyz XyzToD50Xyz(Xyz xyz, XyzConfiguration xyzConfig)
    {
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var d50Xyz = Adaptation.WhitePoint(xyzMatrix, xyzConfig.WhitePoint, Profile.XyzD50.WhitePoint).ToTriplet();
        return new Xyz(d50Xyz.First, d50Xyz.Second, d50Xyz.Third);
    }
    
    internal static Xyz XyzD50ToXyz(Xyz xyzD50, XyzConfiguration xyzConfig)
    {
        var xyzD50Matrix = Matrix.FromTriplet(xyzD50.Triplet);
        var xyz = Adaptation.WhitePoint(xyzD50Matrix, Profile.XyzD50.WhitePoint, xyzConfig.WhitePoint).ToTriplet();
        return new Xyz(xyz.First, xyz.Second, xyz.Third);
    }
    
    // XYZ values are clipped in ICC XYZ -> LAB conversion (IccUtil.cpp : icLabToXYZ)
    // TODO: take into account clipping flag, find out how clipping gets set
    internal static double[] XyzToIccXyz(Xyz xyz) => xyz.Triplet.ToArray().Select(value => XyzToIccXyz(Math.Max(value, 0))).ToArray();
    internal static double XyzToIccXyz(double value) => value * 32768.0 / 65535.0;
    internal static Xyz IccXyzToXyz(double[] iccXyz)
    {
        var xyz = iccXyz.Select(x => x * 65535.0 / 32768.0).ToArray();
        return new Xyz(xyz[0], xyz[1], xyz[2]);
    }
    
    internal static double[] IccXyzToAdjustedPerceptual(double[] iccXyz, double[] refBlack, double[] refWhite, bool isDeviceToPcs)
    {
        double GetScale(int i) => isDeviceToPcs ? 1 - refBlack[i] / refWhite[i] : 1 / (1 - refBlack[i] / refWhite[i]);
        double GetOffset(int i) => isDeviceToPcs ? XyzToIccXyz(refBlack[i]) : -XyzToIccXyz(refBlack[i]) * GetScale(i);
        double GetAdjusted(int i) => iccXyz[i] * GetScale(i) + GetOffset(i);
        
        var adjustedIccXyz = Enumerable.Range(0, 3).Select(GetAdjusted).ToArray();
        return adjustedIccXyz;
    }
    
    internal static double[] IccXyzToAdjustedAbsolute(double[] iccXyz, double[] refWhite, double[] mediaWhite, bool isDeviceToPcs)
    {
        double GetScale(int i) => isDeviceToPcs ? mediaWhite[i] / refWhite[i] : refWhite[i] / mediaWhite[i];
        double GetAdjusted(int i) => iccXyz[i] * GetScale(i);
        
        var adjustedIccXyz = Enumerable.Range(0, 3).Select(GetAdjusted).ToArray();
        return adjustedIccXyz;
    }

    internal static double[] AdjustedIccXyzToLab2(double[] adjustedIccXyz)
    {
        var adjustedXyz = IccXyzToXyz(adjustedIccXyz);
        var adjustedLab = Lab.FromXyz(adjustedXyz, Profile.XyzD50);
        var adjustedIccLab4 = LabToIccLab(adjustedLab);
        return IccLab4ToIccLab2(adjustedIccLab4);
    }
}