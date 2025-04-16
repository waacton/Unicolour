namespace Wacton.Unicolour.Experimental;

/*
 * extensions to make the lagrangian maths of reflectance generator more readable
 * makes big assumptions about types and dimensions, not for general purpose
 */
internal static class MatrixUtils
{
    private const int Count = PigmentGenerator.Wavelengths;

    // assumes both left and right are 1D
    internal static double[] Concat(Matrix left, Matrix right)
    {
        return left.GetCol(0).Concat(right.GetCol(0)).ToArray();
    }

    // assumes upperLeft = 36x36, upperRight = 3x36, lowerLeft = 36x3, lowerRight = 3x3
    internal static Matrix Concat(Matrix upperLeft, Matrix upperRight, Matrix lowerLeft, Matrix lowerRight)
    {
        var data = new double[Count + 3, Count + 3];
        for (var row = 0; row < Count + 3; row++)
        {
            for (var col = 0; col < Count + 3; col++)
            {
                data[row, col] = row switch
                {
                    < Count when col < Count => upperLeft[row, col],
                    < Count when col >= Count => upperRight[row, col - Count],
                    >= Count when col < Count => lowerLeft[row - Count, col],
                    _ => lowerRight[row - Count, col - Count] // lower right
                };
            } 
        }

        return new Matrix(data);
    }
    
    internal static Matrix Diag(IEnumerable<double> values)
    {
        var array = values.ToArray();
        var dimension = array.Length;
        var data = new double[dimension, dimension];
        for (var i = 0; i < dimension; i++)
        {
            data[i, i] = array[i];
        }

        return new Matrix(data);
    }
    
    // assumes 1D array
    internal static Matrix Multiply(this Matrix matrix, IEnumerable<double> values)
    {
        return matrix.Multiply(values.ToArray().ToMatrix());
    }
    
    internal static double[] Negate(this double[] values) => values.Select(x => -x).ToArray();
    
    internal static Matrix Zip(this Matrix matrix, Matrix other, Func<double, double, double> operation)
    {
        var rows = matrix.Rows;
        var cols = matrix.Cols;
        var result = new Matrix(new double[rows, cols]);
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                result[row, col] = operation(matrix[row, col], other[row, col]);
            } 
        }

        return result;
    }
    
    // returns a Nx1 matrix from a 1D array
    private static Matrix ToMatrix(this IEnumerable<double> values)
    {
        var array = values.ToArray();
        var data = new double[array.Length, 1];
        for (var i = 0; i < array.Length; i++)
        {
            data[i, 0] = array[i];
        }

        return new Matrix(data);
    }

    internal static double[] GetCol(this Matrix matrix, int col)
    {
        var array = new double[matrix.Rows];
        for (var row = 0; row < matrix.Rows; row++)
        {
            array[row] = matrix.Data[row, col];
        }

        return array;
    }
    
    internal static double[] GetRow(this Matrix matrix, int row)
    {
        var array = new double[matrix.Cols];
        for (var col = 0; col < matrix.Cols; col++)
        {
            array[col] = matrix.Data[row, col];
        }

        return array;
    }
    
    internal static void SetRow(this Matrix matrix, int rowIndex, double[] values)
    {
        for (var col = 0; col < matrix.Cols; col++)
        {
            matrix.Data[rowIndex, col] = values[col];
        }
    }
}