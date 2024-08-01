namespace Wacton.Unicolour.Icc;

internal record Curve(double[] Table)
{
    internal double[] Table { get; } = Table;
    
    internal double Lookup(double value)
    {
        var (lowerValue, upperValue, distance) = Lut.Lookup(Table, value);
        return Interpolation.Interpolate(lowerValue, upperValue, distance);
    }
}