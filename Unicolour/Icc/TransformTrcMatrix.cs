namespace Wacton.Unicolour.Icc;

internal class TransformTrcMatrix : Transform
{
    private readonly Lazy<List<Curve>> bCurves;
    private readonly Lazy<List<Curve>> bCurvesInverse;
    private readonly Lazy<Matrices> matrices;
    private readonly Lazy<Matrices> matricesInverse;
    private readonly List<Curve> mCurves = new() { TableCurve.Identity, TableCurve.Identity, TableCurve.Identity };
    
    internal TransformTrcMatrix(Header header, Tags tags) 
        : base(header, tags, hasPerceptualHandling: false)
    {
        bCurves = new Lazy<List<Curve>>(() => new List<Curve> { tags.RedTrc.Value!, tags.GreenTrc.Value!, tags.BlueTrc.Value! });
        matrices = new Lazy<Matrices>(() => new Matrices(Multiply: GetMatrix(), Offset: Matrices.ZeroOffset));

        bCurvesInverse = new Lazy<List<Curve>>(() => new List<Curve> { tags.RedTrc.Value!.Inverse(), tags.GreenTrc.Value!.Inverse(), tags.BlueTrc.Value!.Inverse() });
        matricesInverse = new Lazy<Matrices>(() => new Matrices(Multiply: GetMatrix().Inverse(), Offset: Matrices.ZeroOffset));
    }
    
    internal override double[] ToXyz(double[] deviceValues, Intent intent)
    {
        // this transform can only be used with XYZ PCS, no need to handle LAB PCS
        var xyz = ToPcs(deviceValues);
        return AdjustXyz(xyz, intent, isDeviceToPcs: true);
    }

    internal override double[] FromXyz(double[] xyz, Intent intent)
    {
        // this transform can only be used with XYZ PCS, no need to handle LAB PCS
        xyz = AdjustXyz(xyz, intent, isDeviceToPcs: false);
        return ToDevice(xyz);
    }
    
    private double[] ToPcs(double[] deviceValues) => BM(deviceValues, bCurves.Value, matrices.Value, mCurves);
    private double[] ToDevice(double[] pcsValues) => MB(pcsValues, mCurves, matricesInverse.Value, bCurvesInverse.Value);
    
    private Matrix GetMatrix()
    {
        var redMatrix = tags.RedMatrixColumn.Value!;
        var greenMatrix = tags.GreenMatrixColumn.Value!;
        var blueMatrix = tags.BlueMatrixColumn.Value!;
        
        return new Matrix(new[,]
        {
            { redMatrix.x, greenMatrix.x, blueMatrix.x },
            { redMatrix.y, greenMatrix.y, blueMatrix.y },
            { redMatrix.z, greenMatrix.z, blueMatrix.z }
        });
    }
}