using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripCam16Tests
{
    private const double Tolerance = 0.00005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Cam16, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Cam16(triplet.First, triplet.Second, triplet.Third, CamConfig);
        var xyz = Cam16.ToXyz(original, CamConfig, XyzConfig.ChromaticAdaptor);
        var roundtrip = Cam16.FromXyz(xyz, CamConfig, XyzConfig.ChromaticAdaptor);
        
        // CAM <-> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.Limitation == Limitation.NaN ? ViaCamWithNaN(roundtrip.Triplet) : original.Triplet, Tolerance);
    }
    
    // when NaNs occur during CAM <-> XYZ conversion
    // if the NaN occurs during CAM -> XYZ: all value are NaN
    // if the NaN occurs during XYZ -> CAM: J, H, Q have values and C, M, S are NaN - J is the first item of the triplet
    private static ColourTriplet ViaCamWithNaN(ColourTriplet triplet)
    {
        var first = double.IsNaN(triplet.First) ? double.NaN : triplet.First;
        return new(first, double.NaN, double.NaN);
    }
}