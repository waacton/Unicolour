namespace Wacton.Unicolour.Icc;

internal static class Transform
{
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
}