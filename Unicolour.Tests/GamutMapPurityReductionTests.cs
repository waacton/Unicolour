using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class GamutMapPurityReductionTests
{
    [TestCase(360, 1, 0.5)]
    [TestCase(530, 1, 0.5)]
    [TestCase(700, 1, 0.5)]
    [TestCase(-490, 1, 0.5)]
    [TestCase(-560, 1, 0.5)]
    public void ReducePurity(double w, double x, double y)
    {
        var colour = new Unicolour(ColourSpace.Wxy, w, x, y);
        var gamutMapped = colour.MapToRgbGamut(GamutMap.WxyPurityReduction);
        Assert.That(gamutMapped.IsInRgbGamut, Is.True);
        
        // only X (excitation purity) should change
        Assert.That(gamutMapped.Wxy.W, Is.EqualTo(w));
        Assert.That(gamutMapped.Wxy.Y, Is.EqualTo(y));
        
        var increasedPurity = new Unicolour(ColourSpace.Wxy, w, gamutMapped.Wxy.X + 0.05, y);
        Assert.That(increasedPurity.IsInRgbGamut, Is.False);
    }

    [Test]
    public void BeyondMaxPurity()
    {
        var colour = new Unicolour(ColourSpace.Wxy, 530, 1.00000000001, 0.5);
        var gamutMapped = colour.MapToRgbGamut(GamutMap.WxyPurityReduction);
        Assert.That(gamutMapped.IsInRgbGamut, Is.True);
    }
    
    [Test]
    public void BeyondMinPurity()
    {
        var colour = new Unicolour(ColourSpace.Wxy, 530, -0.00000000001, 0.5);
        var gamutMapped = colour.MapToRgbGamut(GamutMap.WxyPurityReduction);
        Assert.That(gamutMapped.IsInRgbGamut, Is.True);
    }
}