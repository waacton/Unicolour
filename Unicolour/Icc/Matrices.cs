namespace Wacton.Unicolour.Icc;

internal record Matrices(Matrix Multiply, Matrix Offset)
{
    internal Matrix Multiply { get; } = Multiply;
    internal Matrix Offset { get; } = Offset;

    internal static readonly Matrix ZeroOffset = new(new[,] { { 0.0 }, { 0.0 }, { 0.0 } });
    
    internal double[] Apply(double[] inputs)
    {
        var multiplied = Multiply.Multiply(Matrix.From(inputs[0], inputs[1], inputs[2])).ToTriplet().ToArray();
        var offsetArray = Offset.ToTriplet().ToArray();

        // NOTE: apparently should be clipped to 0 - 1 before used downstream
        // but can't find evidence of this in DemoIccMAX reference implementation
        var matrixOutput = new double[3];
        matrixOutput[0] = multiplied[0] + offsetArray[0];
        matrixOutput[1] = multiplied[1] + offsetArray[1];
        matrixOutput[2] = multiplied[2] + offsetArray[2];
        return matrixOutput;
    }
}