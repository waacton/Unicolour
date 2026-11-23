namespace Wacton.Unicolour.Icc;

internal class TransformNone : Transform
{
    internal TransformNone(Header header, Tags tags) 
        : base(header, tags, hasPerceptualHandling: true)
    {
    }

    internal override double[] ToXyz(double[] deviceValues, Intent intent, bool clipNegativeXyz = true)
    {
        throw new NotSupportedException("Transform is not defined");
    }

    internal override double[] FromXyz(double[] xyz, Intent intent, bool clipNegativeXyz = true)
    {
        throw new NotSupportedException("Transform is not defined");
    }
}