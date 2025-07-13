namespace Wacton.Unicolour;

internal static class Lut
{
    internal static (T lower, T upper, double distance) Lookup<T>(T[] table, double x)
    {
        var (lowerIndex, upperIndex, distance) = Lookup(table.Length, x);
        var lowerValue = table[lowerIndex];
        var upperValue = table[upperIndex];
        return (lowerValue, upperValue, distance);
    }
    
    internal static (int lowerIndex, int upperIndex, double distance) Lookup(double valueCount, double normalisedValue)
    {
        var exactIndex = Interpolation.Linear(0, valueCount - 1, normalisedValue.Clamp(0.0, 1.0));
        var lowerIndex = (int)Math.Floor(exactIndex);
        var upperIndex = (int)Math.Ceiling(exactIndex);
        var distance = exactIndex - lowerIndex;
        return (lowerIndex, upperIndex, distance);
    }
}