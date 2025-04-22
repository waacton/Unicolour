using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class PointerGamutTests
{
    [TestCase(15, 0, 10)]
    [TestCase(15, 350, 15)]
    [TestCase(15, 360, 10)]
    [TestCase(90, 0, 8)]
    [TestCase(90, 350, 6)]
    [TestCase(90, 360, 8)]
    [TestCase(50, 170, 75)]
    [TestCase(50, 180, 71)]
    [TestCase(55, 170, 76)]
    [TestCase(55, 180, 72)]
    [TestCase(15, 210, 0)]
    [TestCase(80, 80, 115)]
    public void MatchingLH(double l, double h, double expectedC) => AssertMaxC(l, h, expectedC);

    [TestCase(15, 2.5, 11.25)]
    [TestCase(15, 5, 12.5)]
    [TestCase(15, 7.5, 13.75)]
    [TestCase(15, 342.5, 23.25)]
    [TestCase(15, 345, 20.5)]
    [TestCase(15, 347.5, 17.75)]
    [TestCase(15, 352.5, 13.75)]
    [TestCase(15, 355, 12.5)]
    [TestCase(15, 357.5, 11.25)]
    [TestCase(90, 2.5, 7.75)]
    [TestCase(90, 5, 7.5)]
    [TestCase(90, 7.5, 7.25)]
    [TestCase(90, 342.5, 4.5)]
    [TestCase(90, 345, 5)]
    [TestCase(90, 347.5, 5.5)]
    [TestCase(90, 352.5, 6.5)]
    [TestCase(90, 355, 7)]
    [TestCase(90, 357.5, 7.5)]
    [TestCase(50, 172.5, 74)]
    [TestCase(50, 175, 73)]
    [TestCase(50, 177.5, 72)]
    [TestCase(55, 172.5, 75)]
    [TestCase(55, 175, 74)]
    [TestCase(55, 177.5, 73)]
    public void InterpolateH(double l, double h, double expectedC) => AssertMaxC(l, h, expectedC);
    
    [TestCase(16.25, 0, 15)]
    [TestCase(17.5, 0, 20)]
    [TestCase(18.75, 0, 25)]
    [TestCase(16.25, 350, 20.5)]
    [TestCase(17.5, 350, 26)]
    [TestCase(18.75, 350, 31.5)]
    [TestCase(16.25, 360, 15)]
    [TestCase(17.5, 360, 20)]
    [TestCase(18.75, 360, 25)]
    [TestCase(86.25, 0, 16.25)]
    [TestCase(87.5, 0, 13.5)]
    [TestCase(88.75, 0, 10.75)]
    [TestCase(86.25, 350, 14.25)]
    [TestCase(87.5, 350, 11.5)]
    [TestCase(88.75, 350, 8.75)]
    [TestCase(86.25, 360, 16.25)]
    [TestCase(87.5, 360, 13.5)]
    [TestCase(88.75, 360, 10.75)]
    [TestCase(51.25, 170, 75.25)]
    [TestCase(52.5, 170, 75.5)]
    [TestCase(53.75, 170, 75.75)]
    [TestCase(51.25, 180, 71.25)]
    [TestCase(52.5, 180, 71.5)]
    [TestCase(53.75, 180, 71.75)]
    public void InterpolateL(double l, double h, double expectedC) => AssertMaxC(l, h, expectedC);
    
    [TestCase(51.25, 172.5, 74.25)]
    [TestCase(51.25, 175, 73.25)]
    [TestCase(51.25, 177.5, 72.25)]
    [TestCase(52.5, 172.5, 74.5)]
    [TestCase(52.5, 175, 73.5)]
    [TestCase(52.5, 177.5, 72.5)]
    [TestCase(53.75, 172.5, 74.75)]
    [TestCase(53.75, 175, 73.75)]
    [TestCase(53.75, 177.5, 72.75)]
    public void InterpolateLH(double l, double h, double expectedC) => AssertMaxC(l, h, expectedC);

    [Test]
    public void BelowMinL([Values(14.9999999999, 1, 0, -0.0000000001, double.Epsilon, double.MinValue, double.NegativeInfinity)] double l)
    {
        var colour = new Unicolour(PointerGamut.Config.Value, ColourSpace.Lchab, l, 50, 180);
        Assert.That(colour.IsInPointerGamut, Is.False);
    }
    
    [Test]
    public void AboveMaxL([Values(90.0000000001, 100, double.MaxValue, double.PositiveInfinity)] double l)
    {
        var colour = new Unicolour(PointerGamut.Config.Value, ColourSpace.Lchab, l, 50, 180);
        Assert.That(colour.IsInPointerGamut, Is.False);
    }

    [Test]
    public void NotNumberL()
    {
        var colour = new Unicolour(PointerGamut.Config.Value, ColourSpace.Lchab, double.NaN, 50, 180);
        Assert.That(colour.IsInPointerGamut, Is.False);
    }
    
    [Test]
    public void NotNumberC()
    {
        var colour = new Unicolour(PointerGamut.Config.Value, ColourSpace.Lchab, 50, double.NaN, 180);
        Assert.That(colour.IsInPointerGamut, Is.False);
    }
    
    [Test]
    public void NotNumberH()
    {
        var colour = new Unicolour(PointerGamut.Config.Value, ColourSpace.Lchab, 50, 50, double.NaN);
        Assert.That(colour.IsInPointerGamut, Is.False);
    }

    [TestCase(nameof(StandardRgb.Red), 0.95687, 0.18251, 0.09074, 92.945)]
    [TestCase(nameof(StandardRgb.Green), 0.69091, 0.92211, 0.6165, 45.843)]
    [TestCase(nameof(StandardRgb.Blue), 0.31602, 0.18804, 0.73389, 84.319)]
    [TestCase(nameof(StandardRgb.Black), 0.14771, 0.14771, 0.14771, 0)]
    [TestCase(nameof(StandardRgb.White), 0.88757, 0.88757, 0.88757, 0)]
    public void MapOutsideGamut(string name, double expectedR, double expectedG, double expectedB, double expectedC)
    {
        var colour = StandardRgb.Lookup[name];
        Assert.That(colour.IsInPointerGamut, Is.False);

        var mapped = colour.MapToPointerGamut();
        Assert.That(mapped.IsInPointerGamut, Is.True);
        TestUtils.AssertTriplet<Rgb>(mapped, new(expectedR, expectedG, expectedB), 0.00005);
        Assert.That(mapped.Lchab.C, Is.EqualTo(expectedC).Within(0.005));
    }

    [TestCase(0.5, 0.5, 0.5)]
    [TestCase(1.0, 0.5, 0.0)]
    public void MapInsideGamut(double r, double g, double b)
    {
        var colour = new Unicolour(ColourSpace.Rgb, r, g, b);
        Assert.That(colour.IsInPointerGamut, Is.True);
        
        var mapped = colour.MapToPointerGamut();
        Assert.That(Equals(mapped, colour), Is.True);
        Assert.That(ReferenceEquals(mapped, colour), Is.False);
    }
    
    [Test, Combinatorial]
    public void ExtremeValues([ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double value)
    {
        // if extreme values are being used for the colour space in which mapping takes place
        // mapping should still return an in-gamut colour, with the exception of NaNs
        var original = new Unicolour(PointerGamut.Config.Value, ColourSpace.Lchab, value, value, value);
        var gamutMapped = original.MapToPointerGamut();
        Assert.That(gamutMapped.IsInPointerGamut, gamutMapped.Lchab.UseAsNaN ? Is.False : Is.True);
    }
    
    private static void AssertMaxC(double l, double h, double expectedC)
    {
        var maxC = PointerGamut.GetMaxC(l, h);
        Assert.That(maxC, Is.EqualTo(expectedC));

        var colourInGamut = new Unicolour(PointerGamut.Config.Value, ColourSpace.Lchab, l, maxC, h);
        Assert.That(colourInGamut.IsInPointerGamut, Is.True);

        var beyondMaxC = maxC + 0.00000000001;
        var colourOutOfGamut = new Unicolour(PointerGamut.Config.Value, ColourSpace.Lchab, l, beyondMaxC, h);
        Assert.That(colourOutOfGamut.IsInPointerGamut, Is.False); 
    }
}