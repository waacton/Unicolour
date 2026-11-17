using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Hellwig;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHellwigQmhTests
{
    private const double Tolerance = 0.00005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.HellwigQmh, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Qmh(triplet.First, triplet.Second, triplet.Third);
        var xyz = Cam.ToXyz(new Cam(original, CamConfig.ViewHellwig), CamConfig.WhitePoint, XyzConfig.ChromaticAdaptor);
        var roundtrip = Cam.FromXyz(xyz, CamConfig.WhitePoint, CamConfig.ViewHellwig, XyzConfig.ChromaticAdaptor).Qmh;
        
        // CAM <-> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        // if the NaN occurs during CAM -> XYZ: all value are NaN
        // if the NaN occurs during XYZ -> CAM: J, H, Q have values and C, M, S are NaN
        if (roundtrip.Limitation == Limitation.NaN)
        {
            var allNaN = double.IsNaN(roundtrip.H);
            Qmh originalWithNaN = allNaN ? new(double.NaN, double.NaN, double.NaN) : new(roundtrip.Q, double.NaN, roundtrip.H);
            TestUtils.AssertTriplet(roundtrip, originalWithNaN, Tolerance);
            return;
        }
        
        TestUtils.AssertTriplet(roundtrip, original, Tolerance);
    }
}