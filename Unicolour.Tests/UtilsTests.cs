namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;

public class UtilsTests
{
    [Test]
    public void GuardInRange()
    {
        Assert.DoesNotThrow(() => 0.5.Guard(0.0, 1.0, "test"));
    }
    
    [Test]
    public void GuardLowerLimit()
    {
        Assert.Catch<InvalidOperationException>(() => 0.99999.Guard(1.0, 2.0, "test"));
        Assert.DoesNotThrow(() => 1.00000.Guard(1.0, 2.0, "test"));
        Assert.DoesNotThrow(() => 1.00001.Guard(1.0, 2.0, "test"));
    }
    
    [Test]
    public void GuardUpperLimit()
    {
        Assert.Catch<InvalidOperationException>(() => 2.00001.Guard(1.0, 2.0, "test"));
        Assert.DoesNotThrow(() => 2.00000.Guard(1.0, 2.0, "test"));
        Assert.DoesNotThrow(() => 1.99999.Guard(1.0, 2.0, "test"));
    }
    
    [Test]
    public void GuardUnusual()
    {
        Assert.Catch<InvalidOperationException>(() => double.MinValue.Guard(-1.0, 1.0, "test"));
        Assert.Catch<InvalidOperationException>(() => double.MaxValue.Guard(-1.0, 1.0, "test"));
        Assert.Catch<InvalidOperationException>(() => double.PositiveInfinity.Guard(-1.0, 1.0, "test"));
        Assert.Catch<InvalidOperationException>(() => double.NegativeInfinity.Guard(-1.0, 1.0, "test"));
        Assert.Catch<InvalidOperationException>(() => double.NaN.Guard(-1.0, 1.0, "test"));
        Assert.Catch<InvalidOperationException>(() => double.Epsilon.Guard(0.00000000001, 1.0, "test"));
        Assert.DoesNotThrow(() => double.Epsilon.Guard(-1.0, 1.0, "test"));
    }
    
    [Test]
    public void ModuloSameAsDividend([Values(-10, -1, -0.1, 0.1, 1, 10)] double dividend)
    {
        AssertModulo(dividend, dividend, 0);
    }
    
    [Test]
    public void ModuloZero([Values(-10, -1, -0.1, 0.1, 1, 10)] double dividend)
    {
        AssertModulo(dividend, 0, double.NaN);
    }
    
    [TestCase(10, 3, 1)]
    [TestCase(10, 5, 0)]
    [TestCase(10, 7.5, 2.5)] // 10 / 7.5 = "1 remainder 2.5" [floor(1.33) -> 1 * 7.5 = 7.5 -> +2.5 = 10]
    [TestCase(10, 9.9, 0.1)]
    [TestCase(0.1, 0.03, 0.01)]
    [TestCase(0.1, 0.05, 0)]
    [TestCase(0.1, 0.075, 0.025)]
    [TestCase(0.1, 0.099, 0.001)]
    public void ModuloLessThanDividend(double dividend, double modulus, double expected)
    {
        AssertModulo(dividend, modulus, expected);
    }
    
    [TestCase(10, 10.1, 10)]
    [TestCase(10, 100, 10)] // 10 / 100 = "1 remainder 10" [floor(0.1) -> 0 * 100 = 0 -> +10 = 10]
    [TestCase(0.1, 0.11, 0.1)]
    [TestCase(0.1, 1, 0.1)]
    public void ModuloGreaterThanDividend(double dividend, double modulus, double expected)
    {
        AssertModulo(dividend, modulus, expected);
    }
    
    [TestCase(-10, 360, 350)] // e.g. cycling back to start of hue
    [TestCase(-10, 100, 90)] // -10 / 100 = "-1 remainder 90" [floor(-0.1) -> -1 * 100 = -100 -> +90 = -10]
    [TestCase(-10, 50, 40)]
    [TestCase(-10, 10, 0)]
    [TestCase(-10, 8, 6)] // -10 / 8 = "-2 remainder 6" [floor(-1.25) -> -2 * 8 = -16 -> +6 = -10]
    [TestCase(-10, 5, 0)] // -10 / 5 = "-2 remainder 0" [floor(-2) -> -2 * 5 = -10 -> +0 = -10]
    [TestCase(-10, 3, 2)] // -10 / 3 = "-4 remainder 2" [floor(-3.33) -> -4 * 3 = -12 -> +2 = -10]
    [TestCase(-0.1, 1, 0.9)]
    [TestCase(-0.1, 0.5, 0.4)]
    [TestCase(-0.1, 0.1, 0)]
    [TestCase(-0.1, 0.08, 0.06)]
    [TestCase(-0.1, 0.05, 0)]
    [TestCase(-0.1, 0.03, 0.02)]
    public void ModuloNegativeDividend(double dividend, double modulus, double expected)
    {
        AssertModulo(dividend, modulus, expected);
    }
    
    [TestCase(10, -100, -90)] // 10 / -100 = "-1 remainder -90" [floor(-0.1) -> -1 * -100 = 100 -> -90 = -10]
    [TestCase(10, -50, -40)]
    [TestCase(10, -10, 0)]
    [TestCase(10, -8, -6)] // 10 / -8 = "-2 remainder -6" [floor(-1.25) -> -2 * -8 = 16 -> -6 = 10]
    [TestCase(10, -5, 0)] // 10 / -5 = "-2 remainder 0" [floor(-2) -> -2 * -5 = 10 -> + 0 = 10]
    [TestCase(10, -3, -2)] // 10 / -3 = "-4 remainder -2" [floor(-3.33) -> -4 * -3 = 12 -> -2 = -10]
    [TestCase(0.1, -1, -0.9)]
    [TestCase(0.1, -0.5, -0.4)]
    [TestCase(0.1, -0.1, 0)]
    [TestCase(0.1, -0.08, -0.06)]
    [TestCase(0.1, -0.05, -0)]
    [TestCase(0.1, -0.03, -0.02)]
    public void ModuloNegativeModulus(double dividend, double modulus, double expected)
    {
        AssertModulo(dividend, modulus, expected);
    }
    
    [TestCase(-10, -100, -10)] // -10 / -100 = "0 remainder -10" [floor(0.1) -> 0 * -100 = 0 -> -10 = -10]
    [TestCase(-10, -50, -10)]
    [TestCase(-10, -10, 0)]
    [TestCase(-10, -8, -2)] // -10 / -8 = "1 remainder -2" [floor(1.25) -> 1 * -8 = -8 -> -2 = -10]
    [TestCase(-10, -5, 0)] // -10 / -5 = "2 remainder 0" [floor(2) -> 2 * -5 = -10 -> +0 = -10]
    [TestCase(-10, -3, -1)] // -10 / -3 = "3 remainder -1" [floor(3.33) -> 3 * -3 = -9 -> -1 = -10]
    [TestCase(-0.1, -1, -0.1)]
    [TestCase(-0.1, -0.5, -0.1)]
    [TestCase(-0.1, -0.1, 0)]
    [TestCase(-0.1, -0.08, -0.02)]
    [TestCase(-0.1, -0.05, -0)]
    [TestCase(-0.1, -0.03, -0.01)]
    public void ModuloNegativeDividendAndModulus(double dividend, double modulus, double expected)
    {
        AssertModulo(dividend, modulus, expected);
    }

    private static void AssertModulo(double dividend, double modulus, double expected)
    {
        Assert.That(dividend.Modulo(modulus), Is.EqualTo(expected).Within(0.00000000005));
    }
}