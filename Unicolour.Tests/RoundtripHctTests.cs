namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripHctTests
{
    private const double Tolerance = 0.00005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HctTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Hct(triplet.First, triplet.Second, triplet.Third);
        var xyz = Hct.ToXyz(original, XyzConfig);
        var roundtrip = Hct.FromXyz(xyz, XyzConfig);
    
        // very rarely, HCT -> XYZ fails to converge, in which case it's assumed there is no suitable XYZ
        // so far, after 100,000s of randomly generated tests, only seen where HCT has high C value or low T value
        if (!xyz.HctToXyzSearchResult!.Converged)
        {
            Assert.That(original.C > 88 || original.T < 12, Is.True);
            AssertUtils.AssertTriplet(roundtrip.Triplet, new(double.NaN, double.NaN, double.NaN), Tolerance);
            return;
        }
    
        // slightly less rarely, sometimes HCT -> XYZ converges
        // but results in an XYZ that produces a NaN during XYZ -> CAM16 due to negative number to a fractional power
        // which presents as at least a NaN CAM16.Model.C, while LAB.L is calculated correctly 
        if (roundtrip.IsNaN)
        {
            var cam16 = Hct.Cam16Component(xyz);
            var lab = Hct.LabComponent(xyz);
            Assert.That(cam16.Model.C, Is.NaN);
            Assert.That(lab.L, Is.NaN.Or.EqualTo(original.T).Within(Tolerance));
            return;
        }
    
        AssertUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}