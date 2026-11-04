using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripCam02QchTests
{
    private const double Tolerance = 0.00005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Cam02Qch, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Cam02Qch(triplet.First, triplet.Second, triplet.Third);
        var xyz = Cam02.ToXyz(new Cam02(original, CamConfig.View02), CamConfig.WhitePoint, XyzConfig.ChromaticAdaptor);
        var roundtrip = Cam02.FromXyz(xyz, CamConfig.WhitePoint, CamConfig.View02, XyzConfig.ChromaticAdaptor).Qch;
        
        // CAM <-> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        // if the NaN occurs during CAM -> XYZ: all value are NaN
        // if the NaN occurs during XYZ -> CAM: J, H, Q have values and C, M, S are NaN
        if (roundtrip.Limitation == Limitation.NaN)
        {
            var allNaN = double.IsNaN(roundtrip.H);
            Cam02Qch originalWithNaN = allNaN ? new(double.NaN, double.NaN, double.NaN) : new(roundtrip.Q, double.NaN, roundtrip.H);
            TestUtils.AssertTriplet(roundtrip, originalWithNaN, Tolerance);
            return;
        }
        
        TestUtils.AssertTriplet(roundtrip, original, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaCam02Ucs(ColourTriplet triplet)
    {
        var original = new Cam02Qch(triplet.First, triplet.Second, triplet.Third);
        var cam02 = new Cam02(original, CamConfig.View02);
        var cam02Ucs = Cam02Ucs.FromCam02(cam02);
        var roundtrip = Cam02Ucs.ToCam02(cam02Ucs, CamConfig.View02).Qch;
        
        switch (cam02.Model.M)
        {
            case < -43.859649122807014: // -1 / 0.0228, any smaller than this and the Math.Log in the UCS conversion will receive a negative and return NaN
                TestUtils.AssertTriplet(roundtrip, new(original.Q, double.NaN, double.NaN, hueIndex: 2), Tolerance);
                break;
            case < 0:
                TestUtils.AssertTriplet(roundtrip, new(original.Q, 0, 0, hueIndex: 2), Tolerance);
                break;
            default:
                TestUtils.AssertTriplet(roundtrip, original, Tolerance);
                break;
        }
    }
}