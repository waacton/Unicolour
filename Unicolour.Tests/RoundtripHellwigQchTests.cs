using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Hellwig;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHellwigQchTests
{
    private const double Tolerance = 0.00005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.HellwigQch, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Qch(triplet.First, triplet.Second, triplet.Third);
        var xyz = Cam.ToXyz(new Cam(original, CamConfig.ViewHellwig), CamConfig.WhitePoint, XyzConfig.ChromaticAdaptor);
        var roundtrip = Cam.FromXyz(xyz, CamConfig.WhitePoint, CamConfig.ViewHellwig, XyzConfig.ChromaticAdaptor).Qch;
        
        // CAM <-> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        // if the NaN occurs during CAM -> XYZ: all value are NaN
        // if the NaN occurs during XYZ -> CAM: J, H, Q have values and C, M, S are NaN
        if (roundtrip.Limitation == Limitation.NaN)
        {
            var allNaN = double.IsNaN(roundtrip.H);
            Qch originalWithNaN = allNaN ? new(double.NaN, double.NaN, double.NaN) : new(roundtrip.Q, double.NaN, roundtrip.H);
            TestUtils.AssertTriplet(roundtrip, originalWithNaN, Tolerance);
            return;
        }
        
        TestUtils.AssertTriplet(roundtrip, original, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaHellwigUcs(ColourTriplet triplet)
    {
        var original = new Qch(triplet.First, triplet.Second, triplet.Third);
        var Hellwig = new Cam(original, CamConfig.ViewHellwig);
        var HellwigUcs = Ucs.FromCam(Hellwig);
        var roundtrip = Ucs.ToCam(HellwigUcs, CamConfig.ViewHellwig).Qch;
        
        switch (Hellwig.Model.M)
        {
            case < -43.859649122807014: // -1 / 0.1628, any smaller than this and the Math.Log in the UCS conversion will receive a negative and return NaN
                TestUtils.AssertTriplet(roundtrip, new Qch(original.Q, double.NaN, double.NaN), Tolerance);
                break;
            case < 0:
                TestUtils.AssertTriplet(roundtrip, new Qch(original.Q, 0, 0), Tolerance);
                break;
            default:
                TestUtils.AssertTriplet(roundtrip, original, Tolerance);
                break;
        }
    }
}