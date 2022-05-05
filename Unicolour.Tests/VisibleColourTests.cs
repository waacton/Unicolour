namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public static class VisibleColourTests
{
    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(0.5, 0.5, 0.5)]
    [TestCase(1.0, 1.0, 1.0)]
    [TestCase(double.Epsilon, double.Epsilon, double.Epsilon)]
    public static void VisibleRgb(double r, double g, double b)
    {
        var unicolour = Unicolour.FromRgb(r, g, b);
        Assert.That(unicolour.CanBeDisplayed, Is.True);
    }
    
    [TestCase(-0.00001, 0.0, 0.0)]
    [TestCase(0.0, -0.00001, 0.0)]
    [TestCase(0.0, 0.0, -0.00001)]
    [TestCase(1.00001, 1.0, 1.0)]
    [TestCase(1.0, 1.00001, 1.0)]
    [TestCase(1.0, 1.0, 1.00001)]

    public static void NotVisibleRgb(double r, double g, double b)
    {
        var unicolour = Unicolour.FromRgb(r, g, b);
        Assert.That(unicolour.CanBeDisplayed, Is.False);
    }
}