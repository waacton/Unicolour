using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class AlphaTests
{
    [TestCase(0, 0, "00")]
    [TestCase(0.25, 64, "40")]
    [TestCase(0.5, 128, "80")]
    [TestCase(0.75, 191, "BF")]
    [TestCase(1, 255, "FF")]
    [TestCase(double.Epsilon, 0, "00")]
    public void InRange(double value, int value255, string hex) => AssertAlpha(value, value255, hex, inRange: true);

    [TestCase(-0.00000000001, 0, "00")]
    [TestCase(-0.5, 0, "00")]
    [TestCase(1.00000000001, 255, "FF")]
    [TestCase(1.5, 255, "FF")]
    [TestCase(double.MinValue, 0, "00")]
    [TestCase(double.MaxValue, 255, "FF")]
    [TestCase(double.NegativeInfinity, 0, "00")]
    [TestCase(double.PositiveInfinity, 255, "FF")]
    [TestCase(double.NaN, 0, "00")]
    public void OutRange(double value, int value255, string hex) => AssertAlpha(value, value255, hex, inRange: false);

    private static void AssertAlpha(double value, int value255, string hex, bool inRange)
    {
        var alpha = new Alpha(value);

        if (inRange)
        {
            Assert.That(alpha.A, Is.EqualTo(value));
            Assert.That(alpha.A255, Is.EqualTo(value255));
            Assert.That(alpha.Hex, Is.EqualTo(hex));
            
            Assert.That(alpha.Clipped.A, Is.EqualTo(value));
            Assert.That(alpha.Clipped.A255, Is.EqualTo(value255));
            Assert.That(alpha.Clipped.Hex, Is.EqualTo(hex));
        }
        else
        {
            Assert.That(alpha.A, Is.EqualTo(value));
            Assert.That(alpha.Clipped.A, Is.EqualTo(value255 / 255.0));
            Assert.That(alpha.Clipped.A255, Is.EqualTo(value255));
            Assert.That(alpha.Clipped.Hex, Is.EqualTo(hex));
        }
    }
}