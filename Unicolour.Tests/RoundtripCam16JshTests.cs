using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripCam16JshTests
{
    private const double Tolerance = 0.00005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Cam16Jsh, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Cam16Jsh(triplet.First, triplet.Second, triplet.Third);
        var xyz = Cam16.ToXyz(new Cam16(original, CamConfig.View16), CamConfig.WhitePoint, XyzConfig.ChromaticAdaptor);
        var roundtrip = Cam16.FromXyz(xyz, CamConfig.WhitePoint, CamConfig.View16, XyzConfig.ChromaticAdaptor).Jsh;
        
        // CAM <-> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        // if the NaN occurs during CAM -> XYZ: all value are NaN
        // if the NaN occurs during XYZ -> CAM: J, H, Q have values and C, M, S are NaN
        if (roundtrip.Limitation == Limitation.NaN)
        {
            var allNaN = double.IsNaN(roundtrip.H);
            Cam16Jsh originalWithNaN = allNaN ? new(double.NaN, double.NaN, double.NaN) : new(roundtrip.J, double.NaN, roundtrip.H);
            TestUtils.AssertTriplet(roundtrip, originalWithNaN, Tolerance);
            return;
        }
        
        TestUtils.AssertTriplet(roundtrip, original, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaCam16Ucs(ColourTriplet triplet)
    {
        var original = new Cam16Jsh(triplet.First, triplet.Second, triplet.Third);
        var cam16 = new Cam16(original, CamConfig.View16);
        var cam16Ucs = Cam16Ucs.FromCam16(cam16);
        var roundtrip = Cam16Ucs.ToCam16(cam16Ucs, CamConfig.View16).Jsh;
        
        switch (cam16.Model.M)
        {
            case < -43.859649122807014: // -1 / 0.1628, any smaller than this and the Math.Log in the UCS conversion will receive a negative and return NaN
                TestUtils.AssertTriplet(roundtrip, new Cam16Jsh(original.J, double.NaN, double.NaN), Tolerance);
                break;
            case < 0:
                TestUtils.AssertTriplet(roundtrip, new Cam16Jsh(original.J, 0, 0), Tolerance);
                break;
            default:
                TestUtils.AssertTriplet(roundtrip, original, Tolerance);
                break;
        }
    }
}