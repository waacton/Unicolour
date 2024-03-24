namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using NUnit.Framework;

public class RangeClampTests
{
    [Test]
    public void RgbRange()
    {
        Range rRange = new(0.0, 1.0);
        Range gRange = new(0.0, 1.0);
        Range bRange = new(0.0, 1.0);
        var beyondMax = new Rgb(rRange.BeyondMax, gRange.BeyondMax, bRange.BeyondMax);
        var beyondMin = new Rgb(rRange.BeyondMin, gRange.BeyondMin, bRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedR, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedG, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedB, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void Rgb255Range()
    {
        Range rRange = new(0, 255);
        Range gRange = new(0, 255);
        Range bRange = new(0, 255);
        var beyondMax = new Rgb255(rRange.BeyondMax, gRange.BeyondMax, bRange.BeyondMax);
        var beyondMin = new Rgb255(rRange.BeyondMin, gRange.BeyondMin, bRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedR, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedG, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedB, x => x.ConstrainedTriplet.Third);
    }

    [Test]
    public void RgbLinearRange()
    {
        Range rRange = new(0.0, 1.0);
        Range gRange = new(0.0, 1.0);
        Range bRange = new(0.0, 1.0);
        var beyondMax = new RgbLinear(rRange.BeyondMax, gRange.BeyondMax, bRange.BeyondMax);
        var beyondMin = new RgbLinear(rRange.BeyondMin, gRange.BeyondMin, bRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedR, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedG, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedB, x => x.ConstrainedTriplet.Third);
    }

    [Test]
    public void HsbRange()
    {
        Range hRange = new(0.0, 360.0);
        Range sRange = new(0.0, 1.0);
        Range bRange = new(0.0, 1.0);
        var beyondMax = new Hsb(hRange.BeyondMax, sRange.BeyondMax, bRange.BeyondMax);
        var beyondMin = new Hsb(hRange.BeyondMin, sRange.BeyondMin, bRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedS, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedB, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void HslRange()
    {
        Range hRange = new(0.0, 360.0);
        Range sRange = new(0.0, 1.0);
        Range lRange = new(0.0, 1.0);
        var beyondMax = new Hsl(hRange.BeyondMax, sRange.BeyondMax, lRange.BeyondMax);
        var beyondMin = new Hsl(hRange.BeyondMin, sRange.BeyondMin, lRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedS, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedL, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void HwbRange()
    {
        Range hRange = new(0.0, 360.0);
        Range wRange = new(0.0, 1.0);
        Range bRange = new(0.0, 1.0);
        var beyondMax = new Hwb(hRange.BeyondMax, wRange.BeyondMax, bRange.BeyondMax);
        var beyondMin = new Hwb(hRange.BeyondMin, wRange.BeyondMin, bRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedW, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedB, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void LchabRange() // only the hue is constrained
    {
        Range hRange = new(0.0, 360.0);
        var beyondMax = new Lchab(100, 230, hRange.BeyondMax);
        var beyondMin = new Lchab(0, 0, hRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet.Third, beyondMax.Triplet.Third);
        AssertConstrained(beyondMin.Triplet.Third, beyondMin.ConstrainedTriplet.Third);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void LchuvRange() // only the hue is constrained
    {
        Range hRange = new(0.0, 360.0);
        var beyondMax = new Lchuv(100, 230, hRange.BeyondMax);
        var beyondMin = new Lchuv(0, 0, hRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet.Third, beyondMax.Triplet.Third);
        AssertConstrained(beyondMin.Triplet.Third, beyondMin.ConstrainedTriplet.Third);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void HsluvRange()
    {
        Range hRange = new(0.0, 360.0);
        Range sRange = new(0.0, 100.0);
        Range lRange = new(0.0, 100.0);
        var beyondMax = new Hsluv(hRange.BeyondMax, sRange.BeyondMax, lRange.BeyondMax);
        var beyondMin = new Hsluv(hRange.BeyondMin, sRange.BeyondMin, lRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedS, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedL, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void HpluvRange()
    {
        Range hRange = new(0.0, 360.0);
        Range sRange = new(0.0, 100.0);
        Range lRange = new(0.0, 100.0);
        var beyondMax = new Hpluv(hRange.BeyondMax, sRange.BeyondMax, lRange.BeyondMax);
        var beyondMin = new Hpluv(hRange.BeyondMin, sRange.BeyondMin, lRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedS, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedL, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void YpbprRange()
    {
        Range yRange = new(0.0, 1.0);
        Range pbRange = new(-0.5, 0.5);
        Range prRange = new(-0.5, 0.5);
        var beyondMax = new Ypbpr(yRange.BeyondMax, pbRange.BeyondMax, prRange.BeyondMax);
        var beyondMin = new Ypbpr(yRange.BeyondMin, pbRange.BeyondMin, prRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedY, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedPb, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedPr, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void YcbcrRange()
    {
        Range yRange = new(0.0, 255.0);
        Range pbRange = new(0.0, 255.0);
        Range prRange = new(0.0, 255.0);
        var beyondMax = new Ycbcr(yRange.BeyondMax, pbRange.BeyondMax, prRange.BeyondMax);
        var beyondMin = new Ycbcr(yRange.BeyondMin, pbRange.BeyondMin, prRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedY, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedCb, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedCr, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void YcgcoRange()
    {
        Range yRange = new(0.0, 1.0);
        Range cgRange = new(-0.5, 0.5);
        Range coRange = new(-0.5, 0.5);
        var beyondMax = new Ycgco(yRange.BeyondMax, cgRange.BeyondMax, coRange.BeyondMax);
        var beyondMin = new Ycgco(yRange.BeyondMin, cgRange.BeyondMin, coRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedY, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedCg, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedCo, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void YuvRange()
    {
        Range yRange = new(0.0, 1.0);
        Range uRange = new(-0.436, 0.436);
        Range vRange = new(-0.614, 0.614);
        var beyondMax = new Yuv(yRange.BeyondMax, uRange.BeyondMax, vRange.BeyondMax);
        var beyondMin = new Yuv(yRange.BeyondMin, uRange.BeyondMin, vRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedY, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedU, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedV, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void YiqRange()
    {
        Range yRange = new(0.0, 1.0);
        Range iRange = new(-0.59508066230844403, 0.59508066230844403);
        Range qRange = new(-0.52228557764675743, 0.52228557764675743);
        var beyondMax = new Yiq(yRange.BeyondMax, iRange.BeyondMax, qRange.BeyondMax);
        var beyondMin = new Yiq(yRange.BeyondMin, iRange.BeyondMin, qRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedY, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedI, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedQ, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void YdbdrRange()
    {
        Range yRange = new(0.0, 1.0);
        Range dbRange = new(-3.0573394495412844, 3.0573394495412844);
        Range drRange = new(-2.1710097719869705, 2.1710097719869705);
        var beyondMax = new Ydbdr(yRange.BeyondMax, dbRange.BeyondMax, drRange.BeyondMax);
        var beyondMin = new Ydbdr(yRange.BeyondMin, dbRange.BeyondMin, drRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet, beyondMax.Triplet);
        AssertConstrained(beyondMin.Triplet, beyondMin.ConstrainedTriplet);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedY, x => x.ConstrainedTriplet.First);
        AssertConstrainedValue(representations, x => x.ConstrainedDb, x => x.ConstrainedTriplet.Second);
        AssertConstrainedValue(representations, x => x.ConstrainedDr, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void JzczhzRange() // only the hue is constrained
    {
        Range hRange = new(0.0, 360.0);
        var beyondMax = new Jzczhz(1, 0.1, hRange.BeyondMax);
        var beyondMin = new Jzczhz(0, 0, hRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet.Third, beyondMax.Triplet.Third);
        AssertConstrained(beyondMin.Triplet.Third, beyondMin.ConstrainedTriplet.Third);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void OklchRange() // only the hue is constrained
    {
        Range hRange = new(0.0, 360.0);
        var beyondMax = new Oklch(100, 230, hRange.BeyondMax);
        var beyondMin = new Oklch(0, 0, hRange.BeyondMin);
        AssertConstrained(beyondMax.ConstrainedTriplet.Third, beyondMax.Triplet.Third);
        AssertConstrained(beyondMin.Triplet.Third, beyondMin.ConstrainedTriplet.Third);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.Third);
    }
    
    [Test]
    public void HctRange() // only the hue is constrained
    {
        Range hRange = new(0.0, 360.0);
        var beyondMax = new Hct(hRange.BeyondMax, 100, 100);
        var beyondMin = new Hct(hRange.BeyondMin, 0, 0);
        AssertConstrained(beyondMax.ConstrainedTriplet.First, beyondMax.Triplet.First);
        AssertConstrained(beyondMin.Triplet.First, beyondMin.ConstrainedTriplet.First);

        var representations = new[] { beyondMax, beyondMin };
        AssertConstrainedValue(representations, x => x.ConstrainedH, x => x.ConstrainedTriplet.First);
    }

    private static void AssertConstrained(ColourTriplet lesser, ColourTriplet greater)
    {
        AssertConstrained(lesser.First, greater.First);
        AssertConstrained(lesser.Second, greater.Second);
        AssertConstrained(lesser.Third, greater.Third);
    }

    private static void AssertConstrained(double lesser, double greater) => Assert.That(lesser, Is.LessThan(greater));
    
    private static void AssertConstrainedValue<T>(IEnumerable<T> representations, Func<T, double> directValue, Func<T, double> tripletValue) 
        where T : ColourRepresentation
    {
        foreach (var representation in representations)
        {
            Assert.That(directValue(representation), Is.EqualTo(tripletValue(representation)));
        }
    }

    private record Range(double Min, double Max)
    {
        public double BeyondMax => Max + 0.0001;
        public double BeyondMin => Min - 0.0001;
    }
}

