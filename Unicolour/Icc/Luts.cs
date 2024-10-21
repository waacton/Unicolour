namespace Wacton.Unicolour.Icc;

/*
 * valid processing elements AtoB:
 * B
 * M -> Matrix -> B
 * A -> CLUT -> B
 * A -> CLUT -> M -> Matrix -> B
 *
 * valid processing element BtoA:
 * B
 * B -> Matrix -> M
 * B -> CLUT -> A
 * B -> Matrix -> M -> CLUT -> A
 */
internal record Luts(List<Curve> InputCurves, Clut Clut, List<Curve>? MatrixCurves, Matrix? Matrix, List<Curve> OutputCurves, LutType LutType)
{
    internal List<Curve> InputCurves { get; } = InputCurves;
    internal Clut Clut { get; } = Clut;
    internal List<Curve>? MatrixCurves { get; } = MatrixCurves;
    internal Matrix? Matrix { get; } = Matrix;
    internal List<Curve> OutputCurves { get; } = OutputCurves;
    internal LutType LutType { get; } = LutType;

    internal static Luts FromStream(Stream stream)
    {
        var signature = stream.ReadSignature(); // bytes 0 - 3
        stream.Seek(-4, SeekOrigin.Current);    // revert the stream, allowing LUT reading to be better encapsulated
        return signature switch
        {
            Signatures.MultiFunctionTable1Byte => ReadTables(stream, is8Bit: true),
            Signatures.MultiFunctionTable2Byte => ReadTables(stream, is8Bit: false),
            Signatures.MultiFunctionAToB => ReadTablesAB(stream, isAToB: true),
            Signatures.MultiFunctionBToA => ReadTablesAB(stream, isAToB: false),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Luts ReadTables(Stream stream, bool is8Bit)
    {
        stream.ReadSignature();                     // bytes 0 - 3
        stream.ReadBytes(4); // reserved            // bytes 4 - 7
        var inputChannelsI = stream.ReadUInt8();    // byte 8
        var outputChannelsO = stream.ReadUInt8();   // byte 9
        var clutGridPointsG = stream.ReadUInt8();   // byte 10
        stream.ReadByte(); // reserved (padding)    // byte 11

        // use if PCS XYZ is supported (so far I've found no CMYK <-> XYZ profiles)
        // https://github.com/InternationalColorConsortium/DemoIccMAX/issues/46
        stream.ReadS15Fixed16(); // e1              // bytes 12 - 15
        stream.ReadS15Fixed16(); // e2              // bytes 16 - 19
        stream.ReadS15Fixed16(); // e3              // bytes 20 - 23
        stream.ReadS15Fixed16(); // e4              // bytes 24 - 27
        stream.ReadS15Fixed16(); // e5              // bytes 28 - 31
        stream.ReadS15Fixed16(); // e6              // bytes 32 - 35
        stream.ReadS15Fixed16(); // e7              // bytes 36 - 39
        stream.ReadS15Fixed16(); // e8              // bytes 40 - 43
        stream.ReadS15Fixed16(); // e9              // bytes 44 - 47
        
        return is8Bit
            ? Read8BitTables(stream, inputChannelsI, clutGridPointsG, outputChannelsO)
            : Read16BitTables(stream, inputChannelsI, clutGridPointsG, outputChannelsO);
    }

    private static Luts ReadTablesAB(Stream stream, bool isAToB)
    {
        stream.ReadSignature();                     // bytes 0 - 3
        stream.ReadBytes(4); // reserved            // bytes 4 - 7
        var inputChannelsI = stream.ReadUInt8();    // byte 8
        var outputChannelsO = stream.ReadUInt8();   // byte 9
        stream.ReadBytes(2); // reserved (padding)  // bytes 10 - 11
        var bCurvesOffset = stream.ReadUInt32();    // bytes 12 - 15
        var matrixOffset = stream.ReadUInt32();     // bytes 16 - 19
        var mCurvesOffset = stream.ReadUInt32();    // bytes 20 - 23
        var clutOffset = stream.ReadUInt32();       // bytes 24 - 27
        var aCurvesOffset = stream.ReadUInt32();    // bytes 28 - 31
        
        var bCurvesCount = isAToB ? outputChannelsO : inputChannelsI;
        var mCurvesCount = outputChannelsO; // TODO: is this actually "output" number of curves or "LutAB B curves / LutAB A curves"?
        var aCurvesCount = isAToB ? inputChannelsI : outputChannelsO;
        
        // bytes 32 - end (variable, requires seeking data using offset, unlike Lut8 or Lut16)
        var bCurves = ReadCurves(stream, bCurvesOffset, bCurvesCount);
        Matrix? matrix = null; // TODO: matrix (matrix is 3 x 4)
        var mCurves = ReadCurves(stream, mCurvesOffset, mCurvesCount);
        var clut = ReadClut(stream, clutOffset, inputChannelsI, outputChannelsO);;
        var aCurves = ReadCurves(stream, aCurvesOffset, aCurvesCount);

        var inputCurves = isAToB ? aCurves : bCurves;
        var outputCurves = isAToB ? bCurves : aCurves;
        var lutType = isAToB ? LutType.LutAB : LutType.LutBA;
        return new Luts(inputCurves, clut, mCurves, matrix, outputCurves, lutType);
    }
    
    private static Luts Read8BitTables(Stream stream, byte inputChannelsI, byte clutGridPointsG, byte outputChannelsO)
    {
        const int inputTableEntriesN = 256;
        const int outputTableEntriesM = 256;
        
        // bytes 48 - 57+(256*i)
        var inputCurves = new List<Curve>();
        for (var i = 0; i < inputChannelsI; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt8, inputTableEntriesN);
            inputCurves.Add(new Curve(DataTypes.From8BitPrecision(array)));
        }
        
        // bytes 48+(256*i) - 47+(256*i)+(2*g*o)
        var clutValuesSize = (int)Math.Pow(clutGridPointsG, inputChannelsI) * outputChannelsO;
        var clutValues = stream.ReadArray(NumberTypes.ReadUInt8, clutValuesSize);
        var clut = new Clut(DataTypes.From8BitPrecision(clutValues), inputChannelsI, clutGridPointsG, outputChannelsO);
        
        // bytes 47+(256*i)+(2*g*o) - end
        var outputCurves = new List<Curve>();
        for (var i = 0; i < outputChannelsO; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt8, outputTableEntriesM);
            outputCurves.Add(new Curve(DataTypes.From8BitPrecision(array)));
        }

        return new Luts(inputCurves, clut, MatrixCurves: null, Matrix: null, outputCurves, LutType.Lut8);
    }
    
    private static Luts Read16BitTables(Stream stream, byte inputChannelsI, byte clutGridPointsG, byte outputChannelsO)
    {
        var inputTableEntriesN = stream.ReadUInt16();   // bytes 48 - 49
        var outputTableEntriesM = stream.ReadUInt16();  // bytes 50 - 51

        // bytes 52 - 51+(2*n*i)
        var inputCurves = new List<Curve>();
        for (var i = 0; i < inputChannelsI; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt16, inputTableEntriesN);
            inputCurves.Add(new Curve(DataTypes.From16BitPrecision(array)));
        }
        
        // bytes 52+(2*n*i) - 51+(2*n*i)+(2*g^i*o)
        var clutValuesSize = (int)Math.Pow(clutGridPointsG, inputChannelsI) * outputChannelsO;
        var clutValues = stream.ReadArray(NumberTypes.ReadUInt16, clutValuesSize);
        var clut = new Clut(DataTypes.From16BitPrecision(clutValues), inputChannelsI, clutGridPointsG, outputChannelsO);
        
        // bytes 52+(2*n*i)+(2*g^i*o) - end
        var outputCurves = new List<Curve>();
        for (var i = 0; i < outputChannelsO; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt16, outputTableEntriesM);
            outputCurves.Add(new Curve(DataTypes.From16BitPrecision(array)));
        }

        return new Luts(inputCurves, clut, MatrixCurves: null, Matrix: null, outputCurves, LutType.Lut16);
    }

    private static List<Curve>? ReadCurves(Stream stream, uint offset, byte count)
    {
        if (offset == 0) return null;

        var curves = new List<Curve>();
        stream.Seek(offset, SeekOrigin.Begin);
        for (var i = 0; i < count; i++)
        {
            var curve = Curve.FromStream(stream);
            curves.Add(curve);
        }

        return curves;
    }

    private static Clut? ReadClut(Stream stream, uint offset, byte inputChannelsI, byte outputChannelsO)
    {
        if (offset == 0) return null;
        
        stream.Seek(offset, SeekOrigin.Begin);
        var gridPoints = stream.ReadArray(NumberTypes.ReadUInt8, 16).Take(inputChannelsI).ToArray();
        var precision = stream.ReadUInt8();
        stream.ReadBytes(3); // reserved

        var is8Bit = precision == 0x01;
        var clutValuesSize = gridPoints.Aggregate(1, (accumulator, value) => accumulator * value) * outputChannelsO;

        double[] clutValues;
        if (is8Bit)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt8, clutValuesSize);
            clutValues = DataTypes.From8BitPrecision(array);
        }
        else
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt16, clutValuesSize);
            clutValues = DataTypes.From16BitPrecision(array);
        }

        // TODO: current CLUT assumes each dimension is same length - find and test an example where this is not the case
        var clutGridPointsG = gridPoints[0];
        var clut = new Clut(clutValues, inputChannelsI, clutGridPointsG, outputChannelsO);
        return clut;
    }

    // TODO: include matrix curves and matrix
    public override string ToString() => $"{InputCurves.Count}x curves -> {Clut} CLUT -> {OutputCurves.Count}x curves";
}

internal enum LutType
{
    Lut8,
    Lut16,
    LutAB,
    LutBA
}