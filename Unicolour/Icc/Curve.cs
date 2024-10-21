namespace Wacton.Unicolour.Icc;

internal record Curve(double[] Table)
{
    internal double[] Table { get; } = Table;
    
    internal double Lookup(double value)
    {
        var (lowerValue, upperValue, distance) = Lut.Lookup(Table, value);
        return Interpolation.Interpolate(lowerValue, upperValue, distance);
    }

    internal static Curve FromStream(Stream stream)
    {
        // TODO: handle 'para' parametric curve type (this just handles 'curv' curve type)
        var signature = stream.ReadSignature();
        if (signature == Signatures.ParametricCurve)
        {
            throw new NotSupportedException(signature);
        }
        
        stream.ReadBytes(4); // reserved
        var entries = stream.ReadUInt32();
        
        // TODO: handle special case entries == 1 (value encoded as u8Fixed8Number)
        if (entries == 1)
        {
            throw new NotSupportedException();
        }
        
        if (entries == 0) // identity response
        {
            return new Curve(new[] { 0.0, 1.0 });
        }
        
        var array = stream.ReadArray(NumberTypes.ReadUInt16, (int)entries);
        return new Curve(DataTypes.From16BitPrecision(array));
    }
}