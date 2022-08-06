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
    
    [Test]
    public static void LchabRange() // only the hue is constrained
    {
        Range hRange = new(0.0, 360.0);
        var beyondMax = new Lchab(100, 230, hRange.BeyondMax);
        var beyondMin = new Lchab(0, 0, hRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet.Third, beyondMax.Triplet.Third);
        AssertConstrained(beyondMin.Triplet.Third, beyondMin.ConstrainedTriplet.Third);
    }
    
    [Test]
    public static void LchuvRange() // only the hue is constrained
    {
        Range hRange = new(0.0, 360.0);
        var beyondMax = new Lchuv(100, 230, hRange.BeyondMax);
        var beyondMin = new Lchuv(0, 0, hRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet.Third, beyondMax.Triplet.Third);
        AssertConstrained(beyondMin.Triplet.Third, beyondMin.ConstrainedTriplet.Third);
    }
    
    [Test]
    public static void JzczhzRange() // only the hue is constrained
    {
        Range hRange = new(0.0, 360.0);
        var beyondMax = new Jzczhz(1, 0.1, hRange.BeyondMax);
        var beyondMin = new Jzczhz(0, 0, hRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet.Third, beyondMax.Triplet.Third);
        AssertConstrained(beyondMin.Triplet.Third, beyondMin.ConstrainedTriplet.Third);
    }
    
    [Test]
    public static void OklchRange() // only the hue is constrained
    {
        Range hRange = new(0.0, 360.0);
        var beyondMax = new Oklch(100, 230, hRange.BeyondMax);
        var beyondMin = new Oklch(0, 0, hRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet.Third, beyondMax.Triplet.Third);
        AssertConstrained(beyondMin.Triplet.Third, beyondMin.ConstrainedTriplet.Third);
    }

    private static void AssertConstrained(ColourTriplet lesser, ColourTriplet greater)
    {
        AssertConstrained(lesser.First, greater.First);
        AssertConstrained(lesser.Second, greater.Second);
        AssertConstrained(lesser.Third, greater.Third);
    }

    private static void AssertConstrained(double lesser, double greater) => Assert.That(lesser, Is.LessThan(greater));

    private record Range(double Min, double Max)
    {
        public double BeyondMax => Max + 0.0001;
        public double BeyondMin => Min - 0.0001;
    }
}

