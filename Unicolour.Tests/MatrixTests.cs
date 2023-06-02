namespace Wacton.Unicolour.Tests;

using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;

public static class MatrixTests
{
    private static readonly double[,] DataA =
    {
        { 0.0, 1.0, 10.0 },
        { -10.0, -1.0, 0.0 },
        { -0.25, 0.0, 0.25 }
    };
    
    private static readonly double[,] DataB =
    {
        { 1.0, 2.0, 3.0 },
        { 8.0, 9.0, 4.0 },
        { 7.0, 6.0, 5.0 }
    };

    private static readonly double[,] ExpectedMultiplied =
    {
        { 78.0, 69.0, 54.0 },
        { -18.0, -29.0, -34.0 },
        { 1.5, 1.0, 0.5 }
    };
    
    // DataA has determinant of 0, no inverse
    private static readonly double[,] ExpectedInverseA =
    {
        { double.NaN, double.NaN, double.NaN },
        { double.NaN, double.NaN, double.NaN },
        { double.NaN, double.NaN, double.NaN }
    };
    
    private static readonly double[,] ExpectedInverseB =
    {
        { -21 / 48.0, -8 / 48.0, 19 / 48.0 },
        { 12 / 48.0, 16 / 48.0, -20 / 48.0 },
        { 15 / 48.0, -8 / 48.0, 7 / 48.0 }
    };
    
    private static double[,] GetColumn(double[,] data, int index)
    {
        return new[,] { { data[0, index] }, { data[1, index] }, { data[2, index] } };
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
        var matrixB = new Matrix(new[,] { { 1.0, 2.0, 3.0 } });
        Assert.Throws<ArgumentException>(() => matrixA.Multiply(matrixB));
    }

    [Test]
    public static void InverseThreeByThree()
    {
        AssertMatrixInverse(DataA, ExpectedInverseA);
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
        var threeByOne = new Matrix(new[,] { { 1.0 }, { 2.0 }, { 3.0 } });
        var oneByThree = new Matrix(new[,] { { 1.0, 2.0, 3.0 } });
        
        Assert.Throws<InvalidOperationException>(() => twoByTwo.Inverse());
        Assert.Throws<InvalidOperationException>(() => threeByOne.Inverse());
        Assert.Throws<InvalidOperationException>(() => oneByThree.Inverse());
    }

    [Test]
    public static void Scalar()
    {
        var identity = new[,]
        {
            { 0.0, 1.0, 10.0 },
            { -10.0, -1.0, 0.0 },
            { -0.25, 0.0, 0.25 }
        };
        
        var specified = new[,]
        {
            { 9.9, 9.9, 9.9 },
            { 9.9, 9.9, 9.9 },
            { 9.9, 9.9, 9.9 }
        };
        
        var doubled = new[,]
        {
            { 0.0, 2.0, 20.0 },
            { -20.0, -2.0, 0.0 },
            { -0.50, 0.0, 0.50 }
        };
        
        var incremented = new[,]
        {
            { 1.0, 2.0, 11.0 },
            { -9.0, 0.0, 1.0 },
            { 0.75, 1.0, 1.25 }
        };
        
        var squared = new[,]
        {
            { 0.0, 1.0, 100.0 },
            { 100.0, 1.0, 0.0 },
            { 0.0625, 0.0, 0.0625 }
        };
        
        AssertMatrixScalar(DataA, x => x, identity);
        AssertMatrixScalar(DataA, x => 9.9, specified);
        AssertMatrixScalar(DataA, x => x * 2, doubled);
        AssertMatrixScalar(DataA, x => x + 1, incremented);
        AssertMatrixScalar(DataA, x => Math.Pow(x, 2), squared);
    }

    [Test]
    public static void ToTripletCompatibleDimensions()
    {
        var triplet = new ColourTriplet(1.1, 2.2, 3.3);
        var matrixFromData = new Matrix(new[,]
        {
            { triplet.First },
            { triplet.Second },
            { triplet.Third }
        });
        var matrixFromTriplet = Matrix.FromTriplet(triplet);
        var matrixFromValues = Matrix.FromTriplet(triplet.First, triplet.Second, triplet.Third);

        Assert.That(matrixFromData.ToTriplet(), Is.EqualTo(triplet));
        Assert.That(matrixFromTriplet.ToTriplet(), Is.EqualTo(triplet));
        Assert.That(matrixFromValues.ToTriplet(), Is.EqualTo(triplet));
    }
    
    [Test]
    public static void ToTripletIncompatibleDimensions()
    {
        var matrix = new Matrix(DataA);
        Assert.Throws<InvalidOperationException>(() => matrix.ToTriplet());
    }

    private static void AssertMatrixMultiply(double[,] dataA, double[,] dataB, double[,] expected)
    {
        var matrixA = new Matrix(dataA);
        var matrixB = new Matrix(dataB);
        var multipliedMatrix = matrixA.Multiply(matrixB);
        
        var mathNetMatrixA = Matrix<double>.Build.DenseOfArray(dataA);
        var mathNetMatrixB = Matrix<double>.Build.DenseOfArray(dataB);
        var mathNetMultipliedMatrix = mathNetMatrixA.Multiply(mathNetMatrixB);
        
        AssertMatrixEquals(multipliedMatrix, mathNetMultipliedMatrix, expected);
    }
    
    private static void AssertMatrixInverse(double[,] data, double[,] expected)
    {
        var matrixA = new Matrix(data);
        var inverseMatrix = matrixA.Inverse();
        
        var mathNetMatrixA = Matrix<double>.Build.DenseOfArray(data);
        var mathNetInverseMatrix = mathNetMatrixA.Inverse();
        
        AssertMatrixEquals(inverseMatrix, mathNetInverseMatrix, expected);
    }
    
    private static void AssertMatrixScalar(double[,] data, Func<double, double> operation, double[,] expected)
    {
        var matrix = new Matrix(data).Scalar(operation);
        var mathNetMatrix = Matrix<double>.Build.DenseOfArray(data).Map(operation);
        AssertMatrixEquals(matrix, mathNetMatrix, expected);
    }

    private static void AssertMatrixEquals(Matrix actual, Matrix<double> actualMathNet, double[,] expected)
    {
        Assert.That(actual.Data, Is.EqualTo(expected));
        
        // MathNet handles inverse matrix where determinant is 0 differently, with a combination of NaNs and Infinities
        // so only compare with MathNet if at least one item is real
        var containsNumber = expected.Cast<double>().Any(x => !double.IsNaN(x));
        if (!containsNumber) return;
        Assert.That(actualMathNet.ToArray(), Is.EqualTo(expected).Within(0.0000000000000001));
    }
}