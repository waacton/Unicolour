using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class SystemOfLinearEquationsTests
{
    // ReSharper disable CollectionNeverQueried.Global - used in test case sources by name
    public static readonly List<TestCaseData> LuDecompositionTestData = [];
    public static readonly List<TestCaseData> SolveTestData = [];
    // ReSharper restore CollectionNeverQueried.Global

    static SystemOfLinearEquationsTests()
    {
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { 1.0, 2.0, 3 }, { 4.0, 5.0, 6 }, { 7.0, 8.0, 9 } }).SetName("every row pivots"));
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { 1.0, 2.0, -3 }, { 4.0, 5.0, -6 }, { 7.0, 8.0, -9 } }).SetName("every row pivots (negative)"));
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { 9.0, 8.0, 7 }, { 6.0, 5.0, 4 }, { 3.0, 2.0, 1 } }).SetName("no row pivots"));
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { -9.0, 8.0, 7 }, { -6.0, 5.0, 4 }, { -3.0, 2.0, 1 } }).SetName("no row pivots (negative)"));
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { -5.0, 10.0, 10 }, { -10.0, 5.0, 10  }, { 10.0, -5.0, 10 } }).SetName("multiple pivots"));
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { 4.0, 8.0, 1 }, { 0.0, 0.0, 1  }, { 2.0, 12.0, 1 } }).SetName("pivot value also above diagonal")); // 2nd column after pivot 1 becomes 8.0, 0.0, 8 - only 2nd 8 should be found
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { 0.0, 5.0, 5 }, { 5.0, 0.0, 5 }, { 5.0, 5.0, 0 } }).SetName("zero diagonal"));
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { 0.0, 1.0, 2 }, { 0.0, 0.0, 3 }, { 0.0, 0.0, 0 } }).SetName("zero lower triangle"));
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity }, { double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity }, { double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity } }).SetName("infinity"));
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { double.NaN, double.NaN, double.NaN }, { double.NaN, double.NaN, double.NaN }, { double.NaN, double.NaN, double.NaN } }).SetName("not number"));
        LuDecompositionTestData.Add(new TestCaseData(new[,] { { TestUtils.RandomDouble(), TestUtils.RandomDouble(), TestUtils.RandomDouble() }, { TestUtils.RandomDouble(), TestUtils.RandomDouble(), TestUtils.RandomDouble() }, { TestUtils.RandomDouble(), TestUtils.RandomDouble(), TestUtils.RandomDouble() } }).SetName("random"));

        int[] matrixOrders = [5, 10, 15, 20, 25, 30, 35, 40, 45, 50];
        foreach (var order in matrixOrders)
        {
            for (var i = 0; i < 10; i++)
            {
                var a = RandomMatrix(order);
                var b = new double[order].Select(_ => TestUtils.RandomDouble()).ToArray();
                SolveTestData.Add(new TestCaseData(a, b).SetName($"{order}x{order} {i}"));
            }
        }
        
        SolveTestData.Add(new TestCaseData(new[,] { { double.PositiveInfinity, double.PositiveInfinity }, { double.NegativeInfinity, double.NegativeInfinity } }, new[] { double.NegativeInfinity, double.PositiveInfinity }).SetName("infinity"));
        SolveTestData.Add(new TestCaseData(new[,] { { double.NaN, double.NaN }, { double.NaN, double.NaN } }, new[] { double.NaN, double.NaN }).SetName("not number"));
        return;
        
        double[,] RandomMatrix(int order)
        {
            var data = new double[order, order];
            for (var row = 0; row < order; row++)
            {
                for (var col = 0; col < order; col++)
                {
                    data[row, col] = TestUtils.RandomDouble();
                }
            }

            return data;
        }
    }
    
    [TestCaseSource(nameof(LuDecompositionTestData))]
    public void LuDecomposition(double[,] data)
    {
        var expected = Matrix<double>.Build.DenseOfArray(data).LU();
        var expectedFactors = GetMathNetFactors(expected);
        var expectedPivots = GetMathNetPivots(expected);
        var actual = SystemOfLinearEquations.LuDecomposition(new Matrix(data));
        Assert.That(actual.factors.Data, Is.EqualTo(expectedFactors.ToArray()).Within(1e-12));
        Assert.That(actual.pivots, Is.EqualTo(expectedPivots).Within(1e-12));
    }

    // implicitly tests results of ApplyPivot, ForwardSubstitution, BackwardSubstitution
    // which MathNet does not expose (and has no reason to)
    [TestCaseSource(nameof(SolveTestData))]
    public void Solve(double[,] a, double[] b)
    {
        var expected = Matrix<double>.Build.DenseOfArray(a).LU().Solve(Vector<double>.Build.DenseOfArray(b));
        var actual = SystemOfLinearEquations.Solve(new Matrix(a), b);
        Assert.That(actual, Is.EqualTo(expected.ToArray()).Within(5e-09));
    }

    private static Matrix<double> GetMathNetFactors(LU<double> lu) => (Matrix<double>)GetField(lu, "Factors");
    private static int[] GetMathNetPivots(LU<double> lu) => (int[])GetField(lu, "Pivots");
    private static object GetField(LU<double> lu, string name)
    {
        return lu.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(lu)!;
    }
}