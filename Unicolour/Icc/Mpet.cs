namespace Wacton.Unicolour.Icc;

public static class Mpet
{
    internal static Mpe[] DToBFromStream(Stream stream)
    {
        var mpeSignature = stream.ReadSignature();                                              // bytes 0 - 3
        stream.ReadBytes(4); // reserved                                                        // bytes 4 - 7
        var inputChannels = stream.ReadUInt16();                                                // bytes 8 - 9
        var outputChannels = stream.ReadUInt16();                                               // bytes 10 - 11
        var elementCountN = (int)stream.ReadUInt32();                                           // bytes 12 - 15
        var elementPositions = stream.ReadArray(DataTypes.ReadPositionNumber, elementCountN);   // bytes 16 - 15+8*n

        // bytes 16+8*n - end (the actual multi process elements)
        var mpes = new Mpe[elementCountN];
        for (var i = 0; i < elementCountN; i++)
        {
            var elementPosition = elementPositions[i];
            mpes[i] = Mpe(stream, elementPosition.offset, elementPosition.size);
        }

        return mpes;
    }

    private static Mpe Mpe(Stream stream, int offset, int size)
    {
        var mpeSignature = stream.ReadSignature();  // bytes 0 - 3
        stream.Seek(-4, SeekOrigin.Current);        // revert the stream, allowing MPE reading to be better encapsulated
        return mpeSignature switch
        {
            "cvst" => ReadCurveSet(stream, offset),
            "matf" => ReadMatrix(stream, offset),
            _ => throw new ArgumentOutOfRangeException(nameof(mpeSignature), mpeSignature, null)
        };
    }
    
    private static MpeCurveSet ReadCurveSet(Stream stream, int offset)
    {
        stream.ReadSignature();                                                              // bytes 0 - 3
        stream.ReadBytes(4); // reserved                                                     // bytes 4 - 7
        var inputChannelsP = stream.ReadUInt16();                                            // bytes 8 - 9
        var outputChannelsQ = stream.ReadUInt16();                                           // bytes 10 - 11
        var curvePositions = stream.ReadArray(DataTypes.ReadPositionNumber, inputChannelsP); // bytes 12 - 11+8*p

        var mpeCurves = new MpeCurve[inputChannelsP];
        
        // bytes 12+8*p - end (the actual curve elements)
        for (var i = 0; i < inputChannelsP; i++)
        {
            var curvePosition = curvePositions[i];
            stream.Seek(offset + curvePosition.offset, SeekOrigin.Begin);
            
            // should be MPE curve (curf) that is composed of curve segments
            var signature = stream.ReadSignature();
            
            // now figure out segments and breakpoints
            stream.ReadBytes(4); // reserved
            var segmentCountN = (int)stream.ReadUInt16();
            stream.ReadBytes(2); // reserved
            var breakpoints = stream.ReadArray(NumberTypes.ReadFloat, segmentCountN - 1);

            var curveSegments = new MpeCurveSegment[segmentCountN];
            for (var j = 0; j < segmentCountN; j++)
            {
                var curveSegmentSignature = stream.ReadSignature();
                stream.Seek(-4, SeekOrigin.Current); // revert the stream, allowing curve reading to be better encapsulated
                curveSegments[j] = curveSegmentSignature switch
                {
                    "parf" => MpeFormulaCurveSegment.ReadFormulaCurveSegment(stream),
                    "samf" => MpeSampledCurveSegment.ReadSampledCurveSegment(stream),
                    _ => throw new ArgumentOutOfRangeException(nameof(curveSegmentSignature), curveSegmentSignature, null)
                };
            }

            mpeCurves[i] = new MpeCurve(curveSegments, breakpoints);
        }

        return new MpeCurveSet(mpeCurves);
    }
    
    private static MpeMatrix ReadMatrix(Stream stream, int offset)
    {
        stream.ReadSignature();                                                     // bytes 0 - 3
        stream.ReadBytes(4); // reserved                                            // bytes 4 - 7
        var inputChannelsP = stream.ReadUInt16();                                   // bytes 8 - 9
        var outputChannelsQ = stream.ReadUInt16();                                  // bytes 10 - 11

        var elementCount = outputChannelsQ * (inputChannelsP + 1);
        var matrixElements = stream.ReadArray(NumberTypes.ReadFloat, elementCount); // bytes 12 - end (4*q*(p+1))
        
        var multiplyElements = matrixElements.Take(outputChannelsQ * inputChannelsP).ToArray();
        var multiplyData = new double[outputChannelsQ, inputChannelsP];
        for (var row = 0; row < outputChannelsQ; row++)
        {
            for (var col = 0; col < inputChannelsP; col++)
            {
                var index = row * outputChannelsQ + col;
                multiplyData[row, col] = multiplyElements[index];
            }
        }
        
        var offsetElements = matrixElements.Skip(outputChannelsQ * inputChannelsP).ToArray();
        var offsetData = new double[outputChannelsQ, 1];
        for (var row = 0; row < outputChannelsQ; row++)
        {
            offsetData[row, 0] = offsetElements[row];
        }
        
        var multiplyMatrix = new Matrix(multiplyData);
        var offsetMatrix = new Matrix(offsetData);
        return new MpeMatrix(inputChannelsP, outputChannelsQ, multiplyMatrix, offsetMatrix);
    }
}

internal abstract record Mpe
{
    internal abstract double[] Apply(double[] inputs);
}

// TODO: too much overlap with existing Matrices class
internal record MpeMatrix(int inputChannels, int outputChannels, Matrix Multiply, Matrix Offset) : Mpe
{
    private int inputChannels { get; } = inputChannels;
    private int outputChannels { get; } = outputChannels;
    internal Matrix Multiply { get; } = Multiply;
    internal Matrix Offset { get; } = Offset;
    
    internal override double[] Apply(double[] inputs)
    {
        var inputData = new double[inputChannels, 1];
        for (var row = 0; row < inputChannels; row++)
        {
            inputData[row, 0] = inputs[row];
        }

        var input = new Matrix(inputData);
        var multiplied = Multiply.Multiply(input);

        var outputs = new double[outputChannels];
        for (var row = 0; row < outputChannels; row++)
        {
            // by now, multiplied and Offset matrices should be a single column
            outputs[row] = multiplied[row, 1] + Offset[row, 1];
        }

        return outputs;
    }
    
    public override string ToString() => $"multiply {Multiply.Rows}x{Multiply.Cols} · offset {Offset.Rows}x{Offset.Cols}";
}

internal record MpeCurveSet(MpeCurve[] curves) : Mpe
{
    private readonly MpeCurve[] curves = curves;
    
    internal override double[] Apply(double[] inputs)
    {
        var outputs = new double[inputs.Length];
        for (var i = 0; i < inputs.Length; i++)
        {
            outputs[i] = curves[i].Lookup(inputs[i]);
        }

        return outputs;
    }

    public override string ToString() => string.Join(", ", curves.Select(x => x.ToString()));
}

internal record MpeCurve(MpeCurveSegment[] segments, double[] breakpoints) : Curve
{
    // made of segments:
    // segment 1 ... -Inf --> breakpoint #1 ... specified by formula
    // segment K ... breakpoint #K-1 --> breakpoint #K ... specified by either formula or sample
    // segment N ... breakpoint #N-1 --> Inf ... specified by formula
    // if only 1 segment, -Inf --> Inf, specified by formula and no breakpoints specified
    // if has N segments, n - 1 breakpoints specified
    private readonly MpeCurveSegment[] segments = segments;
    private readonly double[] breakpoints = breakpoints;
    
    internal override double Lookup(double value)
    {
        // TODO: very rough, review carefully, handle when only 1 segment
        var breakpoint = breakpoints.Last(x => value <= x);
        var breakpointIndex = Array.IndexOf(breakpoints, breakpoint);
        var segment = segments[breakpointIndex - 1];
        return segment.Lookup(value);
    }

    // TODO: seems unlikely to be used
    protected override double[] AsTable()
    {
        throw new NotImplementedException();
    }
    
    public override string ToString() => $"{segments.Length} segments ({string.Join(" + ", segments.Select(x => x.ToString()))})";
}

internal abstract record MpeCurveSegment : Curve;

internal record MpeFormulaCurveSegment(Func<double, double> function, string name) : MpeCurveSegment
{
    private readonly Func<double, double> function = function;
    private readonly string name = name;
    
    internal override double Lookup(double value) => function(value);

    // TODO: seems unlikely to be used
    protected override double[] AsTable()
    {
        List<double> table = [];

        const double length = 2048;
        for (var i = 0; i < length; i++)
        {
            var value = i / (length - 1);
            table.Add(Lookup(value));
        }

        return table.ToArray();
    }
    
    public override string ToString() => $"Formula curve: {name}";
    
    // TODO: extract
    internal static MpeFormulaCurveSegment ReadFormulaCurveSegment(Stream stream)
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
                var gamma = stream.ReadFloat();
                var a = stream.ReadFloat();
                var b = stream.ReadFloat();
                var c = stream.ReadFloat();
                function = x => Math.Pow(a * x + b, gamma) + c;
                break;
            }
            case 1:
            {
                var gamma = stream.ReadFloat();
                var a = stream.ReadFloat();
                var b = stream.ReadFloat();
                var c = stream.ReadFloat();
                var d = stream.ReadFloat();
                function = x => a * Math.Log10(b * Math.Pow(x, gamma) + c) + d;
                break;
            }
            default:
            {
                var a = stream.ReadFloat();
                var b = stream.ReadFloat();
                var c = stream.ReadFloat();
                var d = stream.ReadFloat();
                var e = stream.ReadFloat();
                function = x => a * Math.Pow(b, c * x + d) + e;
                break;
            }
        }

        return new MpeFormulaCurveSegment(function, $"type {functionType}");
    }
}

internal record MpeSampledCurveSegment(double[] samples) : MpeCurveSegment
{
    private readonly double[] samples = samples;

    internal override double Lookup(double value)
    {
        var (lowerValue, upperValue, distance) = Lut.Lookup(samples, value);
        return Interpolation.Linear(lowerValue, upperValue, distance);
    }

    // TODO: seems unlikely to be used
    protected override double[] AsTable() => samples;
    
    public override string ToString() => $"Sample curve: {samples.Length}";
    
    // TODO: extract
    internal static MpeSampledCurveSegment ReadSampledCurveSegment(Stream stream)
    {
        stream.ReadSignature();
        stream.ReadBytes(4); // reserved
        var entryCount = (int)stream.ReadUInt32();
        var entries = stream.ReadArray(NumberTypes.ReadFloat, entryCount);
        return new MpeSampledCurveSegment(entries);
    }
}