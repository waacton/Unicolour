namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using static Wacton.Unicolour.Utils;

public class UtilsTests
{
    [TestCase(0.5)]
    [TestCase(0.0)]
    [TestCase(1.0)]
    public void ClampWithinRange(double value) => Assert.That(value.Clamp(0, 1), Is.EqualTo(value));

    [TestCase(1.00001, 1.0)]
    [TestCase(-0.00001, 0.0)]
    public void ClampOutwithRange(double value, double clamped) => Assert.That(value.Clamp(0, 1), Is.EqualTo(clamped));

    [TestCase(8, 2)]
    [TestCase(0.027, 0.3)]

    public void CubeRootPositive(double value, double result) => AssertCubeRoot(value, result);

    [TestCase(-8, -2)]
    [TestCase(-0.027, -0.3)]
    public void CubeRootNegative(double value, double result) => AssertCubeRoot(value, result);

    [Test]
    public void CubeRootZero() => AssertCubeRoot(0, 0);
    
    [Test]
    public void SignedPositive() => Assert.That(Signed(0.0001), Is.EqualTo("+0.0001"));
    
    [Test]
    public void SignedNegative() => Assert.That(Signed(-0.0001), Is.EqualTo("-0.0001"));
    
    [Test]
    public void SignedZero() => Assert.That(Signed(0), Is.EqualTo("0"));

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
    
    private static void AssertCubeRoot(double value, double result) => Assert.That(CubeRoot(value), Is.EqualTo(result).Within(0.0000000000000001));

    private static void AssertModulo(double dividend, double modulus, double expected)
    {
        Assert.That(dividend.Modulo(modulus), Is.EqualTo(expected).Within(0.00000000005));
    }
}