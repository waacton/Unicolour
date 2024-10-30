namespace Wacton.Unicolour.Icc;

internal abstract record Curve
{
    internal abstract double Lookup(double value);

    internal static Curve FromStream(Stream stream)
    {
        var curveSignature = stream.ReadSignature(); // bytes 0 - 3
        stream.Seek(-4, SeekOrigin.Current);    // revert the stream, allowing curve reading to be better encapsulated
        return curveSignature switch
        {
            Signatures.Curve => ReadCurve(stream),
            Signatures.ParametricCurve => ReadParametricCurve(stream),
            _ => throw new ArgumentOutOfRangeException(nameof(curveSignature), curveSignature, null)
        };
    }

    private static Curve ReadCurve(Stream stream)
    {
        stream.ReadSignature();
        stream.ReadBytes(4); // reserved
        var entries = stream.ReadUInt32();
        switch (entries)
        {
            case 0:
                return new TableCurve(new[] { 0.0, 1.0 });
            case 1:
            {
                var gamma = stream.ReadU8Fixed8();
                return new ParametricCurve(x => Math.Pow(x, gamma));
            }
            default:
            {
                var array = stream.ReadArray(NumberTypes.ReadUInt16, (int)entries);
                return new TableCurve(NumberTypes.From16BitPrecision(array));
            }
        }
    }

    private static ParametricCurve ReadParametricCurve(Stream stream)
    {
        stream.ReadSignature();
        stream.ReadBytes(4); // reserved
        var functionType = stream.ReadUInt16();
        stream.ReadBytes(2); // reserved

        Func<double, double> function;
        switch (functionType)
        {
            case 0:
            {
                var g = stream.ReadS15Fixed16();
                function = x => Math.Pow(x, g);
                break;
            }
            case 1:
            {
                var g = stream.ReadS15Fixed16();
                var a = stream.ReadS15Fixed16();
                var b = stream.ReadS15Fixed16();
                function = x => x < -b / a ? 0 : Math.Pow(a * x + b, g);
                break;
            }
            case 2:
            {
                var g = stream.ReadS15Fixed16();
                var a = stream.ReadS15Fixed16();
                var b = stream.ReadS15Fixed16();
                var c = stream.ReadS15Fixed16();
                function = x => x < -b / a ? c : Math.Pow(a * x + b, g) + c;
                break;
            }
            case 3:
            {
                var g = stream.ReadS15Fixed16();
                var a = stream.ReadS15Fixed16();
                var b = stream.ReadS15Fixed16();
                var c = stream.ReadS15Fixed16();
                var d = stream.ReadS15Fixed16();
                function = x => x < d ? c * x : Math.Pow(a * x + b, g);
                break;
            }
            default:
            {
                var g = stream.ReadS15Fixed16();
                var a = stream.ReadS15Fixed16();
                var b = stream.ReadS15Fixed16();
                var c = stream.ReadS15Fixed16();
                var d = stream.ReadS15Fixed16();
                var e = stream.ReadS15Fixed16();
                var f = stream.ReadS15Fixed16();
                function = x => x < d ? c * x + f : Math.Pow(a * x + b, g) + e;
                break;
            }
        }

        return new ParametricCurve(function);
    }
}

internal record TableCurve(double[] Table) : Curve
{
    internal double[] Table { get; } = Table;
    private bool IsIdentity => Table.Length == 2 && Table[0] == 0.0 && Table[1] == 1.0;
    
    internal override double Lookup(double value)
    {
        // identity does not clamp values, whereas the lookup will
        if (IsIdentity) return value;
        var (lowerValue, upperValue, distance) = Lut.Lookup(Table, value);
        return Interpolation.Interpolate(lowerValue, upperValue, distance);
    }
}

internal record ParametricCurve(Func<double, double> function) : Curve
{
    /*
     * specification says the value should be clamped:
     *     "The domain and range of each function shall be [0,0 1,0].
     *      Any function value outside the range shall be clipped to the range of the function."
     * however, this doesn't make sense for LAB parametric curves (e.g. sRGB v4 preference)
     * which are not at all in 0 - 1 range
     * (and DemoIccMAX reference implementation does not clip 🤷, and least not at this level)
     */ 
    private readonly Func<double, double> function = function;
    
    internal override double Lookup(double value) => function(value);
}