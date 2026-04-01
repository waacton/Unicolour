using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHctTests
{
    private const double Tolerance = 0.00005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Hct, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Hct(triplet.First, triplet.Second, triplet.Third);
        var xyz = Hct.ToXyz(original, XyzConfig.ChromaticAdaptor);
        var roundtrip = Hct.FromXyz(xyz, XyzConfig.ChromaticAdaptor);
    
        // very rarely, HCT -> XYZ fails to converge, in which case it's assumed there is no suitable XYZ
        // so far, after 100,000s of randomly generated tests, only seen where HCT has high C value or low T value
        // (or when C is negative, when using out-of-gamut values)
        if (!xyz.HctToXyzSearchResult!.Converged)
        {
            Assert.That(original.C < 0 || original.C > 88 || original.T < 12, Is.True);
            TestUtils.AssertTriplet(roundtrip.Triplet, new(double.NaN, double.NaN, double.NaN), Tolerance);
            return;
        }
    
        // slightly less rarely, sometimes HCT -> XYZ converges
        // but results in an XYZ that produces a NaN during XYZ -> CAM16 due to negative number to a fractional power
        // which presents as at least a NaN CAM16.Model.C, while LAB.L is calculated correctly 
        if (roundtrip.Limitation == Limitation.NaN)
        {
            var cam16 = Hct.Cam16Component(xyz);
            var lab = Hct.LabComponent(xyz);
            Assert.That(cam16.Model.C, Is.NaN);
            Assert.That(lab.L, Is.NaN.Or.EqualTo(original.T).Within(Tolerance));
            return;
        }
    
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}