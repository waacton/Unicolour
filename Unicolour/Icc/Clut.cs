using System.Text;

namespace Wacton.Unicolour.Icc;

internal class Clut
{
    private readonly Array clutGrid;
    
    internal int InputChannels { get; }
    internal int GridPoints { get; }
    internal int OutputChannels { get; }

    // NOTE: # of values should = gridPoints ^ inputChannels * outputChannels (e.g. 37 ^ 3 * 4 = 202,612)
    internal Clut(double[] values, int inputChannels, int gridPoints, int outputChannels)
    {
        InputChannels = inputChannels;
        GridPoints = gridPoints;
        OutputChannels = outputChannels;
        clutGrid = InitialiseClutGrid(values);
    }
    
    private Array InitialiseClutGrid(double[] values)
    {
        // e.g. CLUT grid with 4 input channels, 25 grid points, 3 output channels
        // = 5D-CLUT of [25, 25, 25, 25, 3]
        var dimensionLengths = new List<int>();
        for (var i = 0; i < InputChannels; i++)
        {
            dimensionLengths.Add(GridPoints);
        }

        dimensionLengths.Add(OutputChannels);

        var grid = Array.CreateInstance(typeof(double), dimensionLengths.ToArray());
        var inputGridCoordinates = GenerateVectorsOfBaseN(InputChannels, GridPoints);
        foreach (var inputGridCoordinate in inputGridCoordinates)
        {
            for (var outputChannel = 0; outputChannel < OutputChannels; outputChannel++)
            {
                var index = GetIndex(inputGridCoordinate, outputChannel);

                var indexes = inputGridCoordinate.ToList();
                indexes.Add(outputChannel);
                grid.SetValue(values[index], indexes.ToArray());
            }
        }

        return grid;
    }
    
    /*
     * finds the section of the CLUT grid that is bound by the indexes of the input channels
     * where each coordinate that forms the bounding area is an array of output channel values
     * then interpolates within that grid section to find a precise values for the output channels
     * particularly tricky because it is generalised to N-dimensions
     * (Fogra39 CMYK -> LAB has a 4D CLUT grid, Fogra55 CMYKOGV has a 7D CLUT grid)
     * ----------
     * e.g. 3-dimensional CLUT:
     * - each input dimension lies between a lower index (0) and an upper index (1)
     * - the bounding area for 3 dimensions is a cube of eight points: 000, 001, 010, 011, 100, 101, 110, 111
     * - point:000
     *   - contains the output for (input[0].Lower, input[1].Lower, input[2].Lower)
     *   - has a distance from the input of input[0].DistanceToLower * input[1].DistanceToLower * input[2].DistanceToLower
     *   - has a weighted output value of (input[0].Lower * distance, input[1].Lower * distance, input[2].Lower * distance)
     * - point:111
     *   - contains the output array (input[0].Upper, input[1].Upper, input[2].Upper)
     *   - has a distance from the input of input[0].DistanceToUpper * input[1].DistanceToUpper * input[2].DistanceToUpper
     *   - has a weighted output array (input[0].Upper * distance, input[1].Upper * distance, input[2].Upper * distance)
     * - the result interpolated value is the sum of each channel of all the weighted points
     * - i.e. if each point represents a CMYK value, final C = weightedPoint:000.C + weightedPoint:001.C + ... + weightedPoint:111.C:
     */
    internal double[] Lookup(double[] clutInputs)
    {
        var clutIndexes = clutInputs.Select(clutInput => new ClutIndex(clutInput, GridPoints)).ToList();
        
        var binaryVectors = GenerateVectorsOfBaseN(InputChannels, 2);
        var weightedOutputs = new List<double[]>();
        foreach (var binaryVector in binaryVectors)
        {
            var inputChannelIndexes = new List<int>();
            var distanceComponents = new List<double>();
            for (var i = 0; i < binaryVector.Length; i++)
            {
                var binary = binaryVector[i];
                var clutIndex = clutIndexes[i];
                inputChannelIndexes.Add(binary == 0 ? clutIndex.Lower : clutIndex.Upper);
                distanceComponents.Add(binary == 0 ? clutIndex.DistanceToLower : clutIndex.DistanceToUpper);
            }

            var output = GetOutput(inputChannelIndexes.ToArray());
            var distance = distanceComponents.Aggregate((accumulate, item) => accumulate * item);
            var weightedOutput = output.Select(x => x * distance).ToArray();
            weightedOutputs.Add(weightedOutput);
        }
        
        var outputComponents = Enumerable.Range(0, OutputChannels).Select(outputChannel => weightedOutputs.Select(x => x[outputChannel])).ToArray();
        var outputSummed = outputComponents.Select(x => x.Sum()).ToArray();
        return outputSummed;
    }

    private static List<int[]> GenerateVectorsOfBaseN(int n, int @base)
    {
        var totalVectors = (int)Math.Pow(@base, n);

        var vectors = new List<int[]>();
        for (var i = 0; i < totalVectors; i++)
        {
            var vector = new int[n];

            var dimensionIndex = n - 1;
            var value = i;
            while (value > 0)
            {
                var remainder = value % @base;
                vector[dimensionIndex] = remainder;
                value /= @base;
                dimensionIndex--;
            }

            vectors.Add(vector);
        }

        return vectors;
    }
    
    /*
     * e.g. Fogra39 CMYK -> LAB: 4 input channels, 25 grid points, 3 output channels
     * index = (cGrid * 25^3 * 3) + (mGrid * 25^2 * 3) + (yGrid * 25^1 * 3) + (kGrid * 25^0 * 3)
     *
     * e.g. Fogra55 CMYKOGV -> LAB: 7 input channels, 5 grid points, 3 output channels
     * index = (cGrid * 5^6 * 3) + (mGrid * 5^5 * 3) + (yGrid * 5^4 * 3) + (kGrid * 5^3 * 3) +
     *         (oGrid * 5^2 * 3) + (gGrid * 5^1 * 3) + (vGrid * 5^0 * 3)
     */
    private int GetIndex(int[] gridPointInputs, int outputChannel)
    {
        var index = 0;
        for (var gridPointInputIndex = 0; gridPointInputIndex < gridPointInputs.Length; gridPointInputIndex++)
        {
            var gridPointInput = gridPointInputs[gridPointInputIndex];
            var power = gridPointInputs.Length - 1 - gridPointInputIndex;
            index += gridPointInput * (int)Math.Pow(GridPoints, power) * OutputChannels;
        }

        index += outputChannel;
        return index;
    }
    
    private double[] GetOutput(int[] inputChannelIndexes)
    {
        var output = new double[OutputChannels];
        for (var i = 0; i < OutputChannels; i++)
        {
            var indexes = inputChannelIndexes.Concat(new[] { i }).ToArray();
            var value = (double)clutGrid.GetValue(indexes);
            output[i] = value;
        }

        return output;
    }
    
    private class ClutIndex
    {
        internal double NormalisedValue { get; set; }
        internal int Max { get; set; }
        internal int Lower { get; }
        internal int Upper { get; }
        internal double DistanceToLower { get; }
        internal double DistanceToUpper { get; }
        
        internal ClutIndex(double normalisedValue, int numberOfValues)
        {
            NormalisedValue = normalisedValue;
            Max = numberOfValues - 1;
            (Lower, Upper, DistanceToUpper) = Lut.Lookup(numberOfValues, normalisedValue);
            DistanceToLower = 1 - DistanceToUpper;
        }
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        for (var i = 0; i < InputChannels; i++)
        {
            stringBuilder.Append($"{GridPoints},");
        }

        stringBuilder.Append($"{OutputChannels}]");
        return stringBuilder.ToString();
    }
}