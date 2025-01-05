namespace Wacton.Unicolour.Icc;

internal class Luts
{
    internal List<Curve>? ACurves { get; }
    internal Clut? Clut { get; }
    internal List<Curve>? MCurves { get; }
    internal Matrices? Matrices { get; }
    internal List<Curve> BCurves { get; }
    internal LutType Type { get; }
    internal bool IsDeviceToPcs { get; }
    internal readonly LutElements Elements;

    private Luts(List<Curve>? aCurves, Clut? clut, List<Curve>? mCurves, Matrices? matrices, List<Curve> bCurves, LutType type, bool isDeviceToPcs)
    {
        ACurves = aCurves;
        Clut = clut;
        MCurves = mCurves;
        Matrices = matrices;
        BCurves = bCurves;
        Type = type;
        IsDeviceToPcs = isDeviceToPcs;

        var hasA = ACurves != null;
        var hasM = MCurves != null && Matrices != null;
        Elements = IsDeviceToPcs switch
        {
            true when hasA => hasM ? LutElements.AMB : LutElements.AB,
            true => hasM ? LutElements.MB : LutElements.B,
            false when hasM => hasA ? LutElements.BMA : LutElements.BM,
            false => hasA ? LutElements.BA : LutElements.B
        };
    }

    internal static Luts AToBFromStream(Stream stream) => FromStream(stream, isDeviceToPcs: true);
    internal static Luts BToAFromStream(Stream stream) => FromStream(stream, isDeviceToPcs: false);
    private static Luts FromStream(Stream stream, bool isDeviceToPcs)
    {
        var lutSignature = stream.ReadSignature(); // bytes 0 - 3
        stream.Seek(-4, SeekOrigin.Current);    // revert the stream, allowing LUT reading to be better encapsulated
        return lutSignature switch
        {
            Signatures.MultiFunctionTable1Byte => ReadTables(stream, isDeviceToPcs, is8Bit: true),
            Signatures.MultiFunctionTable2Byte => ReadTables(stream, isDeviceToPcs, is8Bit: false),
            Signatures.MultiFunctionAToB => ReadTablesAB(stream, isDeviceToPcs),
            Signatures.MultiFunctionBToA => ReadTablesAB(stream, isDeviceToPcs),
            _ => throw new ArgumentOutOfRangeException(nameof(lutSignature), lutSignature, null)
        };
    }

    private static Luts ReadTables(Stream stream, bool isDeviceToPcs, bool is8Bit)
    {
        stream.ReadSignature();                     // bytes 0 - 3
        stream.ReadBytes(4); // reserved            // bytes 4 - 7
        var inputChannelsI = stream.ReadUInt8();    // byte 8
        var outputChannelsO = stream.ReadUInt8();   // byte 9
        var clutGridPointsG = stream.ReadUInt8();   // byte 10
        stream.ReadByte(); // reserved (padding)    // byte 11

        /*
         * these matrix values are only used for output `prtr` profiles with XYZ PCS
         * and must be identity matrix values when not XYZ PCS
         * (see v4 spec 10.10 (13) pg 51 & 10.11 (15) pg 56)
         * TODO: find a profile that meets this criteria (i.e. CMYK <-> XYZ with A2B/B2A tags) to confirm how to apply these correctly
         * NOTE: it seems to be impossible to find a profile of this criteria
         *       do not plan to put in the extra work to support and test this extreme edge case until one is found
         * see also https://github.com/InternationalColorConsortium/DemoIccMAX/issues/46
         */
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
            ? Read8BitTables(stream, inputChannelsI, clutGridPointsG, outputChannelsO, isDeviceToPcs)
            : Read16BitTables(stream, inputChannelsI, clutGridPointsG, outputChannelsO, isDeviceToPcs);
    }

    private static Luts ReadTablesAB(Stream stream, bool isDeviceToPcs)
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
        
        var bCurvesCount = isDeviceToPcs ? outputChannelsO : inputChannelsI;
        var mCurvesCount = isDeviceToPcs ? outputChannelsO : inputChannelsI;
        var aCurvesCount = isDeviceToPcs ? inputChannelsI : outputChannelsO;
        
        // bytes 32 - end (variable, requires seeking data using offset, unlike Lut8 or Lut16)
        var bCurves = ReadCurves(stream, bCurvesOffset, bCurvesCount)!;
        var matrix = ReadMatrices(stream, matrixOffset);
        var mCurves = ReadCurves(stream, mCurvesOffset, mCurvesCount);
        var clut = ReadClut(stream, clutOffset, inputChannelsI, outputChannelsO);
        var aCurves = ReadCurves(stream, aCurvesOffset, aCurvesCount)!;

        var lutType = isDeviceToPcs ? LutType.LutAB : LutType.LutBA;
        return new Luts(aCurves, clut, mCurves, matrix, bCurves, lutType, isDeviceToPcs);
    }
    
    private static Luts Read8BitTables(Stream stream, byte inputChannelsI, byte clutGridPointsG, byte outputChannelsO, bool isDeviceToPcs)
    {
        const int inputTableEntriesN = 256;
        const int outputTableEntriesM = 256;
        
        // bytes 48 - 57+(256*i)
        var inputCurves = new List<Curve>();
        for (var i = 0; i < inputChannelsI; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt8, inputTableEntriesN);
            inputCurves.Add(new TableCurve(NumberTypes.From8BitPrecision(array)));
        }
        
        // bytes 48+(256*i) - 47+(256*i)+(2*g*o)
        var clutValuesSize = (int)Math.Pow(clutGridPointsG, inputChannelsI) * outputChannelsO;
        var clutValues = stream.ReadArray(NumberTypes.ReadUInt8, clutValuesSize);
        var clut = new Clut(NumberTypes.From8BitPrecision(clutValues), inputChannelsI, clutGridPointsG, outputChannelsO);
        
        // bytes 47+(256*i)+(2*g*o) - end
        var outputCurves = new List<Curve>();
        for (var i = 0; i < outputChannelsO; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt8, outputTableEntriesM);
            outputCurves.Add(new TableCurve(NumberTypes.From8BitPrecision(array)));
        }

        var aCurves = isDeviceToPcs ? inputCurves : outputCurves;
        var bCurves = isDeviceToPcs ? outputCurves : inputCurves;
        return new Luts(aCurves, clut, mCurves: null, matrices: null, bCurves, LutType.Lut8, isDeviceToPcs);
    }
    
    private static Luts Read16BitTables(Stream stream, byte inputChannelsI, byte clutGridPointsG, byte outputChannelsO, bool isDeviceToPcs)
    {
        var inputTableEntriesN = stream.ReadUInt16();   // bytes 48 - 49
        var outputTableEntriesM = stream.ReadUInt16();  // bytes 50 - 51

        // bytes 52 - 51+(2*n*i)
        var inputCurves = new List<Curve>();
        for (var i = 0; i < inputChannelsI; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt16, inputTableEntriesN);
            inputCurves.Add(new TableCurve(NumberTypes.From16BitPrecision(array)));
        }
        
        // bytes 52+(2*n*i) - 51+(2*n*i)+(2*g^i*o)
        var clutValuesSize = (int)Math.Pow(clutGridPointsG, inputChannelsI) * outputChannelsO;
        var clutValues = stream.ReadArray(NumberTypes.ReadUInt16, clutValuesSize);
        var clut = new Clut(NumberTypes.From16BitPrecision(clutValues), inputChannelsI, clutGridPointsG, outputChannelsO);
        
        // bytes 52+(2*n*i)+(2*g^i*o) - end
        var outputCurves = new List<Curve>();
        for (var i = 0; i < outputChannelsO; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt16, outputTableEntriesM);
            outputCurves.Add(new TableCurve(NumberTypes.From16BitPrecision(array)));
        }

        var aCurves = isDeviceToPcs ? inputCurves : outputCurves;
        var bCurves = isDeviceToPcs ? outputCurves : inputCurves;
        return new Luts(aCurves, clut, mCurves: null, matrices: null, bCurves, LutType.Lut16, isDeviceToPcs);
    }

    private static List<Curve>? ReadCurves(Stream stream, uint offset, byte count)
    {
        if (offset == 0) return null;

        var curves = new List<Curve>();
        stream.Seek(offset, SeekOrigin.Begin);
        for (var i = 0; i < count; i++)
        {
            var curve = Curve.FromStream(stream);
            MoveToFourByteBoundary(stream, offset);
            curves.Add(curve);
        }

        return curves;
    }
    
    private static Matrices? ReadMatrices(Stream stream, uint offset)
    {
        if (offset == 0) return null;

        stream.Seek(offset, SeekOrigin.Begin);
        var e = stream.ReadArray(NumberTypes.ReadS15Fixed16, 12);
        MoveToFourByteBoundary(stream, offset);

        double E(int element) => e[element - 1];
        
        var multiplyMatrix = new Matrix(new[,]
        {
            { E(1), E(2), E(3) },
            { E(4), E(5), E(6) },
            { E(7), E(8), E(9)}
        });
        
        var offsetMatrix = new Matrix(new[,]
        {
            { E(10) },
            { E(11) },
            { E(12) }
        });

        return new Matrices(multiplyMatrix, offsetMatrix);
    }

    private static void MoveToFourByteBoundary(Stream stream, long startPosition)
    {
        var endPosition = stream.Position;
        var distanceIntoBoundary = (int)((endPosition - startPosition) % 4);
        if (distanceIntoBoundary <= 0) return;
        var remainingDistance = 4 - distanceIntoBoundary;
        stream.ReadBytes(remainingDistance);
    }

    private static Clut? ReadClut(Stream stream, uint offset, byte inputChannelsI, byte outputChannelsO)
    {
        if (offset == 0) return null;
        
        stream.Seek(offset, SeekOrigin.Begin);
        var gridPoints = stream.ReadArray(NumberTypes.ReadUInt8, 16).Take(inputChannelsI).ToArray();
        var precision = stream.ReadUInt8();
        stream.ReadBytes(3); // reserved

        var is8Bit = precision == 1;
        var clutValuesSize = gridPoints.Aggregate(1, (accumulator, value) => accumulator * value) * outputChannelsO;

        double[] clutValues;
        if (is8Bit)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt8, clutValuesSize);
            clutValues = NumberTypes.From8BitPrecision(array);
        }
        else
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt16, clutValuesSize);
            clutValues = NumberTypes.From16BitPrecision(array);
        }

        // NOTE: current CLUT assumes each dimension is same length
        // not yet encountered an example where this is not the case, would require some CLUT rework
        var clutGridPointsG = gridPoints[0];
        var clut = new Clut(clutValues, inputChannelsI, clutGridPointsG, outputChannelsO);
        MoveToFourByteBoundary(stream, offset);
        return clut;
    }
    
    public override string ToString()
    {
        return Elements switch
        {
            LutElements.AB => $"A [x{ACurves!.Count}] -> CLUT {Clut} -> B [x{BCurves.Count}]",
            LutElements.BA => $"B [x{BCurves.Count}] -> CLUT {Clut} -> A [x{ACurves!.Count}]",
            LutElements.AMB => $"A [x{ACurves!.Count}] -> CLUT {Clut} -> M [x{MCurves!.Count}] -> Matrix -> B [x{BCurves.Count}]",
            LutElements.BMA => $"B [x{BCurves.Count}] -> Matrix -> M [x{MCurves!.Count}] -> CLUT {Clut} -> A [x{ACurves!.Count}]",
            LutElements.MB => $"M [x{MCurves!.Count}] -> Matrix -> B [x{BCurves.Count}]",
            LutElements.BM => $"B [x{BCurves.Count}] -> Matrix -> M [x{MCurves!.Count}]",
            _ => $"B [x{BCurves.Count}]"
        };
    }
}

internal enum LutType
{
    Lut8,
    Lut16,
    LutAB,
    LutBA
}

internal enum LutElements
{
    AB,
    BA,
    AMB,
    BMA,
    MB,
    BM,
    B
}