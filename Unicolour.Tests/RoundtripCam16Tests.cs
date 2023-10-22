namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripCam16Tests
{
    private const double Tolerance = 0.00005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Cam16Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        // CAM <-> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        var original = new Cam16(triplet.First, triplet.Second, triplet.Third, CamConfig);
        var roundtrip = Cam16.FromXyz(Cam16.ToXyz(original, CamConfig, XyzConfig), CamConfig, XyzConfig);
        AssertUtils.AssertTriplet(roundtrip.Triplet, roundtrip.IsNaN ? ViaCamWithNaN(roundtrip.Triplet) : original.Triplet, Tolerance);
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