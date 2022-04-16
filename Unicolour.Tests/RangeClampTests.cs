namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public static class RangeClampTests
{
    [Test]
    public static void RgbRange()
    {
        Range rRange = new(0.0, 1.0);
        Range gRange = new(0.0, 1.0);
        Range bRange = new(0.0, 1.0);
        var beyondMax = new Rgb(rRange.BeyondMax, gRange.BeyondMax, bRange.BeyondMax, Configuration.Default);
        var beyondMin = new Rgb(rRange.BeyondMin, gRange.BeyondMin, bRange.BeyondMin, Configuration.Default);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);
    }

    [Test]
    public static void HsbRange()
    {
        Range hRange = new(0.0, 360.0);
        Range sRange = new(0.0, 1.0);
        Range bRange = new(0.0, 1.0);
        var beyondMax = new Hsb(hRange.BeyondMax, sRange.BeyondMax, bRange.BeyondMax);
        var beyondMin = new Hsb(hRange.BeyondMin, sRange.BeyondMin, bRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);
    }
    
    [Test]
    public static void HslRange()
    {
        Range hRange = new(0.0, 360.0);
        Range sRange = new(0.0, 1.0);
        Range lRange = new(0.0, 1.0);
        var beyondMax = new Hsl(hRange.BeyondMax, sRange.BeyondMax, lRange.BeyondMax);
        var beyondMin = new Hsl(hRange.BeyondMin, sRange.BeyondMin, lRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);
    }

    private static void AssertConstrained(ColourTriplet lesser, ColourTriplet greater)
    {
        Assert.That(lesser.First, Is.LessThan(greater.First));
        Assert.That(lesser.Second, Is.LessThan(greater.Second));
        Assert.That(lesser.Third, Is.LessThan(greater.Third));
    }
    
    private record Range(double Min, double Max)
    {
        public double BeyondMax => Max + 0.0001;
        public double BeyondMin => Min - 0.0001;
    }
}

