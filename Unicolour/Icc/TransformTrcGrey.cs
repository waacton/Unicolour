namespace Wacton.Unicolour.Icc;

internal class TransformTrcGrey : Transform
{
    private readonly Lazy<List<Curve>> bCurves;
    private readonly Lazy<List<Curve>> bCurvesInverse;

    // when LAB PCS, grey TRC is performed on the normalised "IccLab"
    private static readonly double[] RefWhiteIccLab = LabToIccLab(XyzToLab(RefWhite));

    internal TransformTrcGrey(Header header, Tags tags) 
        : base(header, tags, hasPerceptualHandling: false)
    {
        bCurves = new Lazy<List<Curve>>(() => new List<Curve> { tags.GreyTrc.Value! });
        bCurvesInverse = new Lazy<List<Curve>>(() => new List<Curve> { tags.GreyTrc.Value!.Inverse() });
    }
    
    internal override double[] ToXyz(double[] deviceValues, Intent intent)
    {
        // PCS is the single-channel "GRAY"
        var pcsValues = ToPcs(deviceValues);
        var grey = pcsValues.Single(); 
        
        double[] xyz;
        if (IsLabPcs)
        {
            var iccLab = RefWhiteIccLab.Select(value => value * grey).ToArray();
            var lab = IccLabToLab(iccLab);
            xyz = LabToXyz(lab);
        }
        else
        {
            xyz = RefWhite.Select(value => value * grey).ToArray();
        }
        
        return AdjustXyz(xyz, intent, isDeviceToPcs: true);
    }

    internal override double[] FromXyz(double[] xyz, Intent intent)
    {
        xyz = AdjustXyz(xyz, intent, isDeviceToPcs: false);

        double grey;
        if (IsLabPcs)
        {
            var lab = XyzToLab(xyz);
            var iccLab = LabToIccLab(lab);
            grey = iccLab[0] / RefWhiteIccLab[0]; // L is the lightness
        }
        else
        {
            grey = xyz[1] / RefWhite[1]; // Y is the luminance
        }
        
        var pcsValues = new[] { grey };
        return ToDevice(pcsValues);
    }
    
    private double[] ToPcs(double[] deviceValues) => B(deviceValues, bCurves.Value);
    private double[] ToDevice(double[] pcsValues) => B(pcsValues, bCurvesInverse.Value);
}