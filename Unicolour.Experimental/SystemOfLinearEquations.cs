namespace Wacton.Unicolour.Experimental;

// NOTE: obviously slower than MathNet, sacrificing speed for understanding and readability
// even less concerned when it's part of the experimental package
// however, easy to replace with copied MathNet code in future if performance is problematic 
internal static class SystemOfLinearEquations
{
    // solve Ax = B
    internal static double[] Solve(Matrix a, double[] b)
    {
        var (lu, pivots) = LuDecomposition(a);  // PA = LU
        var pb = ApplyPivots(b, pivots);
        var y = ForwardSubstitution(lu, pb);    // LY = PB
        var x = BackwardSubstitution(lu, y);    // UX = Y
        return x;
    }
    
    /*
     * i increments both column and row, traversing the diagonal of the matrix top-left to bottom-right
     * (the lower triangular area becoming L, the upper triangular area becoming U)
     * j increments down through every row below the diagonal
     */
    internal static (Matrix factors, int[] pivots) LuDecomposition(Matrix matrix)
    {
        var order = matrix.Cols; // assuming square matrix, same as row count
        
        // the factorised matrix will be modified in situ
        var factors = new Matrix(new double[order, order]);
        Array.Copy(matrix.Data, factors.Data, order * order);
        
        var pivots = new int[order];
        
        for (var i = 0; i < order; i++)
        {
            // once we get to the final column, row permutation is complete
            // so the pivot is always the final row
            if (i == order - 1)
            {
                pivots[i] = i;
                continue;
            }
            
            // 1) pivot - swap the current row with the absolute max row, and keep a record
            var column = factors.GetCol(i);
            var pivot = GetPivot(i, order, column);
            pivots[i] = pivot;

            var pivotRow = factors.GetRow(pivot).ToArray();
            if (pivot != i)
            {
                var currentRow = factors.GetRow(i);
                factors.SetRow(i, pivotRow);
                factors.SetRow(pivot, currentRow);
            }

            // 2) eliminate - scale and subtract the pivot to calculate LU factors
            for (var j = i + 1; j < order; j++)
            {
                // would prefer to divide by zero so that multiplier = NaN and let NaNs propagate
                // but this matches MathNet behaviour (and so presumably LAPACK)
                if (pivotRow[i] == 0.0) continue;
                var resultRow = GetEliminatedRow(factors, j, i, pivotRow);
                factors.SetRow(j, resultRow);
            }
        }

        return (factors, pivots);
    }
    
    // iterates over a column from the diagonal (i) to the last element in the column
    // (e.g. column 3 of a 5x5 matrix will inspect indexes 3, 4, 5)
    // and finds the index of the largest absolute value
    private static int GetPivot(int i, int order, double[] column)
    {
        var pivot = i;
        for (var j = i + 1; j < order; j++)
        {
            // prefer !(>) over <= for NaN behaviour (comparison is always false)
            if (!(Math.Abs(column[j]) > Math.Abs(column[pivot])))
            {
                continue;
            }
                
            pivot = j;
        }

        return pivot;
    }

    // eliminates a row from the diagonal to the last element in the row
    // by subtracting the scaled pivot value, so that the ith element becomes zero (effectively the U part)
    // and store the scale where the value was eliminated (effectively the U part)
    private static double[] GetEliminatedRow(Matrix factors, int j, int i, double[] pivotRow)
    {
        // triangular U matrix part
        var eliminationRow = factors.GetRow(j).ToArray();
        var scale = eliminationRow[i] / pivotRow[i];
        var unchangedPart = eliminationRow.Take(i).ToArray();
        var eliminatedPart = pivotRow.Skip(i).Zip(eliminationRow.Skip(i), (p, e) => e - scale * p).ToArray();
        var resultRow = unchangedPart.Concat(eliminatedPart).ToArray();
                
        // triangular L matrix part
        resultRow[i] = scale;
        return resultRow;
    }

    private static double[] ApplyPivots(double[] b, int[] pivots)
    {
        var length = b.Length;
        
        // PB will be modified in situ
        var pb = new double[length];
        Array.Copy(b, pb, length);
        
        for (var i = 0; i < length; i++)
        {
            var pivot = pivots[i];
            if (i == pivot) continue;
            (pb[i], pb[pivot]) = (pb[pivot], pb[i]);
        }

        return pb;
    }

    // solves LY = PB to get Y
    // by iterating over the triangular L matrix from the top row (with a single Y term) and substituting previously solved Y values
    // first row has 1 Y term and is solvable, second row has 2 Y terms but 1st Y term is now known, etc.
    private static double[] ForwardSubstitution(Matrix lu, double[] pb)
    {
        var order = lu.Cols; // assuming square matrix, same as row count
        
        var y = new double[order];
        for (var i = 0; i < order; i++)
        {
            var l = lu.GetRow(i);
            var substitutedSolved = l.Take(i).Zip(y.Take(i), (ln, yn) => ln * yn);
            y[i] = pb[i] - substitutedSolved.Sum(); // no need to divide by diagonal, diagonal of L is 1
        }

        return y;
    }
    
    // solves UX = Y to get X
    // by iterating over the triangular U matrix from the bottom row (with a single X term) and substituting previously solved X values
    // first row has 1 X term and is solvable, second row has 2 X terms but 1st X term is now known, etc.
    private static double[] BackwardSubstitution(Matrix lu, double[] y)
    {
        var order = lu.Cols; // assuming square matrix, same as row count
        
        var x = new double[order];
        for (var i = order - 1; i >= 0; i--)
        {
            var u = lu.GetRow(i);
            var substitutedSolved = u.Skip(i).Zip(x.Skip(i), (un, xn) => un * xn);
            x[i] = (y[i] - substitutedSolved.Sum()) / u[i];
        }

        return x;
    }
}