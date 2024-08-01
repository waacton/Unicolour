namespace Wacton.Unicolour.Icc;

internal record Luts(List<Curve> InputCurves, Clut Clut, List<Curve> OutputCurves, bool Is16Bit)
{
    internal List<Curve> InputCurves { get; } = InputCurves;
    internal Clut Clut { get; } = Clut;
    internal List<Curve> OutputCurves { get; } = OutputCurves;
    internal bool Is16Bit { get; } = Is16Bit;

    internal static Luts FromStream(Stream stream)
    {
        var signature = stream.ReadSignature();     // bytes 0 - 3
        stream.ReadBytes(4); // reserved            // bytes 4 - 7
        var inputChannelsI = stream.ReadUInt8();    // byte 8
        var outputChannelsO = stream.ReadUInt8();   // byte 9

        // if (signature is Signatures.MultiFunctionTable1Byte or Signatures.MultiFunctionTable2Byte)
        // {
        var clutGridPointsG = stream.ReadUInt8();   // byte 10
        stream.ReadByte(); // reserved for padding  // byte 11
    
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

        return signature == Signatures.MultiFunctionTable1Byte
            ? Get8BitLookupTables(stream, inputChannelsI, clutGridPointsG, outputChannelsO)
            : Get16BitLookupTables(stream, inputChannelsI, clutGridPointsG, outputChannelsO);
        // }
        // else // Signatures.MultiFunctionAToB or Signatures.MultiFunctionBToA
        // {
        //     var aToB = signature == Signatures.MultiFunctionAToB;
        //     var reservedForPadding = ReadBytes(stream, 2); // bytes 10 - 11
        //     var bCurveOffset = GetUInt32(stream); // bytes 12 - 15
        //     var matrixOffset = GetUInt32(stream); // bytes 16 - 19
        //     var mCurveOffset = GetUInt32(stream); // bytes 20 - 23
        //     var clutOffset = GetUInt32(stream); // bytes 24 - 27
        //     var aCurveOffset = GetUInt32(stream); // bytes 28 - 31
        //     throw new InvalidOperationException($"LUT signature {signature} not supported");
        //
        //     return GetAToBLookupTables(stream, inputChannelsI, outputChannelsO, aToB, 
        //         bCurveOffset, matrixOffset, mCurveOffset, clutOffset, aCurveOffset);
        // }
    }
    
    private static Luts Get16BitLookupTables(Stream stream, byte inputChannelsI, byte clutGridPointsG, byte outputChannelsO)
    {
        const bool is16Bit = true;
        const double maxValue = 65535.0;
        var inputTableEntriesN = stream.ReadUInt16();   // bytes 48 - 49
        var outputTableEntriesM = stream.ReadUInt16();  // bytes 50 - 51

        // bytes 52 - 51+(2*n*i)
        var inputCurves = new List<Curve>();
        for (var i = 0; i < inputChannelsI; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt16, inputTableEntriesN);
            var normalised = array.Select(x => x / maxValue).ToArray();
            inputCurves.Add(new Curve(normalised));
        }
        
        // bytes 52+(2*n*i) - 51+(2*n*i)+(2*g^i*o)
        var clutValuesSize = (int)Math.Pow(clutGridPointsG, inputChannelsI) * outputChannelsO;
        var clutValues = stream.ReadArray(NumberTypes.ReadUInt16, clutValuesSize);
        var normalisedClutValues = clutValues.Select(x => x / maxValue).ToArray();
        var clut = new Clut(normalisedClutValues, inputChannelsI, clutGridPointsG, outputChannelsO);
        
        // bytes 52+(2*n*i)+(2*g^i*o) - end
        var outputCurves = new List<Curve>();
        for (var i = 0; i < outputChannelsO; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt16, outputTableEntriesM);
            var normalised = array.Select(x => x / maxValue).ToArray();
            outputCurves.Add(new Curve(normalised));
        }

        return new Luts(inputCurves, clut, outputCurves, is16Bit);
    }
    
    private static Luts Get8BitLookupTables(Stream stream, byte inputChannelsI, byte clutGridPointsG, byte outputChannelsO)
    {
        const bool is16Bit = false;
        const double maxValue = 255.0;
        const int inputTableEntriesN = 256;
        const int outputTableEntriesM = 256;
        
        // bytes 48 - 57+(256*i)
        var inputTables = new List<Curve>();
        for (var i = 0; i < inputChannelsI; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt8, inputTableEntriesN);
            var normalised = array.Select(x => x / maxValue).ToArray();
            inputTables.Add(new Curve(normalised));
        }
        
        // bytes 48+(256*i) - 47+(256*i)+(2*g*o)
        var clutValuesSize = (int)Math.Pow(clutGridPointsG, inputChannelsI) * outputChannelsO;
        var clutValues = stream.ReadArray(NumberTypes.ReadUInt8, clutValuesSize);
        var normalisedClutValues = clutValues.Select(x => x / maxValue).ToArray();
        var clut = new Clut(normalisedClutValues, inputChannelsI, clutGridPointsG, outputChannelsO);
        
        // bytes 47+(256*i)+(2*g*o) - end
        var outputTables = new List<Curve>();
        for (var i = 0; i < outputChannelsO; i++)
        {
            var array = stream.ReadArray(NumberTypes.ReadUInt8, outputTableEntriesM);
            var normalised = array.Select(x => x / maxValue).ToArray();
            outputTables.Add(new Curve(normalised));
        }

        return new Luts(inputTables, clut, outputTables, is16Bit);
    }
    
    // internal static LookupTables GetAToBLookupTables(Stream stream, byte inputChannelsI, byte outputChannelsO, bool aToB,
    //     uint bCurveOffset, uint matrixOffset, uint mCurveOffset, uint clutOffset, uint aCurveOffset)
    // {
    //     var inputTables = new List<ushort[]>();
    //     if (aToB)
    //     {
    //         if (aCurveOffset != 0)
    //         {
    //             // GetCurve()
    //         }
    //     }
    //     else
    //     {
    //         if (bCurveOffset != 0)
    //         {
    //             // GetCurve()
    //         }
    //     }
    //     
    //     ...
    // }

    public override string ToString() => $"{InputCurves.Count}x curves -> {Clut} CLUT -> {OutputCurves.Count}x curves";
}
