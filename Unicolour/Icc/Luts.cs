namespace Wacton.Unicolour.Icc;

internal record Luts(List<Curve>? ACurves, Clut? Clut, List<Curve>? MCurves, Matrices? Matrices, List<Curve> BCurves, LutType LutType, bool IsAToB)
{
    internal List<Curve>? ACurves { get; } = ACurves;
    internal Clut? Clut { get; } = Clut;
    internal List<Curve>? MCurves { get; } = MCurves;
    internal Matrices? Matrices { get; } = Matrices;
    internal List<Curve> BCurves { get; } = BCurves;
    internal LutType LutType { get; } = LutType;
    internal bool IsAToB { get; } = IsAToB;
    internal readonly LutElements LutElements = GetElements(IsAToB, ACurves != null, MCurves != null && Matrices != null);
    
    internal double[] Apply(double[] inputs)
    {
        var outputChannels = LutElements switch
        {
            LutElements.AB => BCurves.Count,
            LutElements.BA => ACurves!.Count,
            LutElements.AMB => BCurves.Count,
            LutElements.BMA => ACurves!.Count,
            LutElements.MB => BCurves.Count,
            LutElements.BM => MCurves!.Count,
            _ => BCurves.Count
        };
        
        if (inputs.Any(double.IsNaN))
        {
            return Enumerable.Range(0, outputChannels).Select(_ => double.NaN).ToArray();
        }
        
        return LutElements switch
        {
            LutElements.AB => Transform.AB(inputs, ACurves!, Clut!, BCurves),
            LutElements.BA => Transform.BA(inputs, BCurves, Clut!, ACurves!),
            LutElements.AMB => Transform.AMB(inputs, ACurves!, Clut!, MCurves!, Matrices!, BCurves),
            LutElements.BMA => Transform.BMA(inputs, BCurves, Matrices!, MCurves!, Clut!, ACurves!),
            LutElements.MB => Transform.MB(inputs, MCurves!, Matrices!, BCurves),
            LutElements.BM => Transform.BM(inputs, BCurves, Matrices!, MCurves!),
            _ => Transform.B(inputs, BCurves)
        };
    }
    
    private static LutElements GetElements(bool isAToB, bool hasA, bool hasM)
    {
        return isAToB switch
        {
            true when hasA => hasM ? LutElements.AMB : LutElements.AB,
            true => hasM ? LutElements.MB : LutElements.B,
            false when hasM => hasA ? LutElements.BMA : LutElements.BM,
            false => hasA ? LutElements.BA : LutElements.B
        };
    }

    internal static Luts AToBFromStream(Stream stream) => FromStream(stream, isAToB: true);
    internal static Luts BToAFromStream(Stream stream) => FromStream(stream, isAToB: false);
    private static Luts FromStream(Stream stream, bool isAToB)
    {
        var lutSignature = stream.ReadSignature(); // bytes 0 - 3
        stream.Seek(-4, SeekOrigin.Current);    // revert the stream, allowing LUT reading to be better encapsulated
        return lutSignature switch
        {
            Signatures.MultiFunctionTable1Byte => ReadTables(stream, isAToB, is8Bit: true),
            Signatures.MultiFunctionTable2Byte => ReadTables(stream, isAToB, is8Bit: false),
            Signatures.MultiFunctionAToB => ReadTablesAB(stream, isAToB),
            Signatures.MultiFunctionBToA => ReadTablesAB(stream, isAToB),
            _ => throw new ArgumentOutOfRangeException(nameof(lutSignature), lutSignature, null)
        };
    }

    private static Luts ReadTables(Stream stream, bool isAToB, bool is8Bit)
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
            ? Read8BitTables(stream, inputChannelsI, clutGridPointsG, outputChannelsO, isAToB)
            : Read16BitTables(stream, inputChannelsI, clutGridPointsG, outputChannelsO, isAToB);
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
        var mCurvesCount = isAToB ? outputChannelsO : inputChannelsI;
        var aCurvesCount = isAToB ? inputChannelsI : outputChannelsO;
        
        // bytes 32 - end (variable, requires seeking data using offset, unlike Lut8 or Lut16)
        var bCurves = ReadCurves(stream, bCurvesOffset, bCurvesCount)!;
        var matrix = ReadMatrices(stream, matrixOffset);
        var mCurves = ReadCurves(stream, mCurvesOffset, mCurvesCount);
        var clut = ReadClut(stream, clutOffset, inputChannelsI, outputChannelsO);
        var aCurves = ReadCurves(stream, aCurvesOffset, aCurvesCount)!;

        var lutType = isAToB ? LutType.LutAB : LutType.LutBA;
        return new Luts(aCurves, clut, mCurves, matrix, bCurves, lutType, isAToB);
    }
    
    private static Luts Read8BitTables(Stream stream, byte inputChannelsI, byte clutGridPointsG, byte outputChannelsO, bool isAToB)
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

        var aCurves = isAToB ? inputCurves : outputCurves;
        var bCurves = isAToB ? outputCurves : inputCurves;
        return new Luts(aCurves, clut, MCurves: null, Matrices: null, bCurves, LutType.Lut8, isAToB);
    }
    
    private static Luts Read16BitTables(Stream stream, byte inputChannelsI, byte clutGridPointsG, byte outputChannelsO, bool isAToB)
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

        var aCurves = isAToB ? inputCurves : outputCurves;
        var bCurves = isAToB ? outputCurves : inputCurves;
        return new Luts(aCurves, clut, MCurves: null, Matrices: null, bCurves, LutType.Lut16, isAToB);
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
        return LutElements switch
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