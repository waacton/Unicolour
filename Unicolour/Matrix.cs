namespace Wacton.Unicolour;

internal class Matrix
{
    public double[,] Data { get; }
    public double this[int row, int col] => Data[row, col];
    public int Rows => Data.GetLength(0);
    public int Cols => Data.GetLength(1);
    
    public Matrix(double[,] data)
    {
        Data = data;
    }

    public Matrix Multiply(Matrix other)
    {
        if (other.Rows != Cols)
        {
            throw new ArgumentException($"Cannot multiply {this} matrix by {other} matrix, incompatible dimensions");
        }
        
        var rows = Rows;
        var cols = other.Cols;
        
        var result = new double[rows, cols];
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                result[row, col] = this[row, 0] * other[0, col] + 
                                   this[row, 1] * other[1, col] +
                                   this[row, 2] * other[2, col];
            }
        }
        
        return new Matrix(result);
    }

    public Matrix Inverse()
    {
        if (Rows != 3 || Cols != 3)
        {
            throw new InvalidOperationException("Only inverse of 3x3 matrix is supported");
        }
        
        var a = this[0, 0];
        var b = this[0, 1];
        var c = this[0, 2];
        var d = this[1, 0];
        var e = this[1, 1];
        var f = this[1, 2];
        var g = this[2, 0];
        var h = this[2, 1];
        var i = this[2, 2];

        var determinant = a*e*i + b*f*g + c*d*h - c*e*g - a*f*h - b*d*i;
        if (determinant == 0)
        {
            throw new InvalidOperationException("Matrix has determinant of 0, is not invertible");
        }

        var adjugate = new[,]
        {
            {e*i - f*h, h*c - i*b, b*f - c*e},
            {g*f - d*i, a*i - g*c, d*c - a*f},
            {d*h - g*e, g*b - a*h, a*e - d*b}
        };
        
        var inverse = new double[3, 3];
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                inverse[row, col] = adjugate[row, col] / determinant;
            }
        }

        return new Matrix(inverse);
    }

    public override string ToString() => $"{Rows}x{Cols}";
}