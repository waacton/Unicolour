namespace Wacton.Unicolour.Icc;

internal abstract record Curve
{
    internal abstract double Lookup(double value);
    protected abstract double[] AsTable();

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
                return TableCurve.Identity;
            case 1:
            {
                var gamma = stream.ReadU8Fixed8();
                return new ParametricCurve(x => Math.Pow(x, gamma), $"gamma {gamma}");
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

        return new ParametricCurve(function, $"type {functionType}");
    }
    
    internal Curve Inverse()
    {
        var tableToInvert = AsTable();
        var inverse = new List<double>();

        const double length = 2048;
        for (var i = 0; i < length; i++)
        {
            var value = i / (length - 1);
            inverse.Add(GetNormalisedIndex(tableToInvert, value));
        }

        return new TableCurve(inverse.ToArray());
    }

    private static double GetNormalisedIndex(double[] table, double value)
    {
        var index = Array.BinarySearch(table, value);
        
        double exactIndex;
        if (index >= 0)
        {
            exactIndex = index;
        }
        else
        {
            // if value is not found, returns bitwise complement of first index larger than it
            var upperIndex = ~index;
            var lowerIndex = upperIndex - 1;
            var valueDifference = value - table[lowerIndex];
            var totalDifference = table[upperIndex] - table[lowerIndex];
            var indexDistance = valueDifference / totalDifference;
            exactIndex = lowerIndex + indexDistance;
        }

        var normalised = exactIndex / (table.Length - 1);
        return normalised;
    }
}

internal record TableCurve(params double[] Table) : Curve
{
    internal static TableCurve Identity => new(0.0, 1.0);
    
    internal double[] Table { get; } = Table;
    private bool IsIdentity => Table.Length == 2 && Table[0] == 0.0 && Table[1] == 1.0;
    
    internal override double Lookup(double value)
    {
        // identity does not clamp values, whereas the lookup will
        if (IsIdentity) return value;
        var (lowerValue, upperValue, distance) = Lut.Lookup(Table, value);
        return Interpolation.Interpolate(lowerValue, upperValue, distance);
    }

    protected override double[] AsTable() => Table;
    
    public override string ToString() => $"Table curve: {Table.Length}";
}

internal record ParametricCurve(Func<double, double> function, string name) : Curve
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
    private readonly string name = name;

    internal override double Lookup(double value) => function(value);

    protected override double[] AsTable()
    {
        var table = new List<double>();

        const double length = 2048;
        for (var i = 0; i < length; i++)
        {
            var value = i / (length - 1);
            table.Add(Lookup(value));
        }

        return table.ToArray();
    }

    public override string ToString() => $"Parametric curve: {name}";
}