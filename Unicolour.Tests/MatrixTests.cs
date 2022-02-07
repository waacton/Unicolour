namespace Wacton.Unicolour.Tests;

using System;
using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;

public static class MatrixTests
{
    private static readonly double[,] DataA = {
        {0.0, 1.0, 10.0},
        {-10.0, -1.0, 0.0},
        {-0.25, 0.0, 0.25}
    };
    
    private static readonly double[,] DataB = {
        {1.0, 2.0, 3.0},
        {8.0, 9.0, 4.0},
        {7.0, 6.0, 5.0}
    };
    
    private static readonly double[,] ExpectedMultiplied = {
        {78.0, 69.0, 54.0},
        {-18.0, -29.0, -34.0},
        {1.5, 1.0, 0.5}
    };
    
    private static readonly double[,] ExpectedInverseB = {
        {-21/48.0, -8/48.0, 19/48.0},
        {12/48.0, 16/48.0, -20/48.0},
        {15/48.0, -8/48.0, 7/48.0}
    };

    private static double[,] GetColumn(double[,] data, int index)
    {
        return new[,] {{data[0, index]}, {data[1, index]}, {data[2, index]}};
    }

    [Test]
    public static void MultiplyCompatibleDimensions()
    {
        // 3x3 * 3x3
        AssertMatrixMultiply(DataA, DataB, ExpectedMultiplied);
        
        // 3x3 * 3x1
        AssertMatrixMultiply(DataA, GetColumn(DataB, 0), GetColumn(ExpectedMultiplied, 0));
        AssertMatrixMultiply(DataA, GetColumn(DataB, 1), GetColumn(ExpectedMultiplied, 1));
        AssertMatrixMultiply(DataA, GetColumn(DataB, 2), GetColumn(ExpectedMultiplied, 2));
    }

    [Test]
    public static void MultiplyIncompatibleDimensions()
    {
        // 3x3 * 1x3
        var matrixA = new Matrix(DataA);
        var matrixB = new Matrix(new[,] {{1.0, 2.0, 3.0} });
        Assert.Catch<ArgumentException>(() => matrixA.Multiply(matrixB));
    }

    [Test]
    public static void InverseThreeByThree()
    {
        // DataA has determinant of 0, no inverse
        Assert.Catch<InvalidOperationException>(() => new Matrix(DataA).Inverse());
        AssertMatrixInverse(DataB, ExpectedInverseB);
    }
    
    [Test]
    public static void InverseUnsupported()
    {
        // invert only supports 3x3 invert, as that is all that is required for colours
        var twoByTwo = new Matrix(new[,]
        {
            {1.0, 2.0},
            {3.0, 4.0}
        });
        var threeByOne = new Matrix(new[,] {{1.0}, {2.0}, {3.0}});
        var oneByThree = new Matrix(new[,] {{1.0, 2.0, 3.0}});
        
        Assert.Catch<InvalidOperationException>(() => twoByTwo.Inverse());
        Assert.Catch<InvalidOperationException>(() => threeByOne.Inverse());
        Assert.Catch<InvalidOperationException>(() => oneByThree.Inverse());
    }

    private static void AssertMatrixMultiply(double[,] dataA, double[,] dataB, double[,] expectedResult)
    {
        var matrixA = new Matrix(dataA);
        var matrixB = new Matrix(dataB);
        var matrixC = matrixA.Multiply(matrixB);
        
        var mathNetMatrixA = Matrix<double>.Build.DenseOfArray(dataA);
        var mathNetMatrixB = Matrix<double>.Build.DenseOfArray(dataB);
        var mathNetMatrixC = mathNetMatrixA.Multiply(mathNetMatrixB);

        Assert.That(matrixC.Data, Is.EqualTo(expectedResult));
        Assert.That(mathNetMatrixC.ToArray(), Is.EqualTo(expectedResult).Within(0.0000000000000001));
    }
    
    private static void AssertMatrixInverse(double[,] data, double[,] expectedResult)
    {
        var matrixA = new Matrix(data);
        var inverseA = matrixA.Inverse();
        
        var mathNetMatrixA = Matrix<double>.Build.DenseOfArray(data);
        var mathNetInverseA = mathNetMatrixA.Inverse();

        Assert.That(inverseA.Data, Is.EqualTo(expectedResult));
        Assert.That(mathNetInverseA.ToArray(), Is.EqualTo(expectedResult).Within(0.0000000000000001));
    }
}