namespace Wacton.Unicolour.Icc;

internal class TransformDToB : Transform
{
    internal TransformDToB(Header header, Tags tags) 
        : base(header, tags, hasPerceptualHandling: true)
    {
    }

    internal override double[] ToXyz(double[] deviceValues, Intent intent)
    {
        throw new NotSupportedException("Transform DToB is not supported");
    }

    internal override double[] FromXyz(double[] xyz, Intent intent)
    {
        // if ever supported, check whether BToD is required
        // or whether it might be missing, like BToA in input `scnr` profiles
        // (see TransformAToB)
        throw new NotSupportedException("Transform BToD is not supported");
    }
}