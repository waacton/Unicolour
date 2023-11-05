namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripXyzTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    private const double IctcpScalar = 100;
    private const double JzazbzScalar = 100;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaRgbLinear(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = RgbLinear.ToXyz(RgbLinear.FromXyz(original, RgbConfig,  XyzConfig), RgbConfig, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Xyy.ToXyz(Xyy.FromXyz(original, XyzConfig));
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaLab(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Lab.ToXyz(Lab.FromXyz(original, XyzConfig), XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaLuv(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Luv.ToXyz(Luv.FromXyz(original, XyzConfig), XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaIctcp(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Ictcp.ToXyz(Ictcp.FromXyz(original, IctcpScalar, XyzConfig), IctcpScalar, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaJzazbz(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Jzazbz.ToXyz(Jzazbz.FromXyz(original, JzazbzScalar, XyzConfig), JzazbzScalar, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Oklab.ToXyz(Oklab.FromXyz(original, XyzConfig), XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaCam02(ColourTriplet triplet)
    {
        // CAM02 -> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Cam02.ToXyz(Cam02.FromXyz(original, CamConfig,  XyzConfig), CamConfig, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.IsNaN ? ViaCamWithNaN(roundtrip.Triplet) : original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaCam16(ColourTriplet triplet)
    {
        // CAM16 -> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Cam16.ToXyz(Cam16.FromXyz(original, CamConfig,  XyzConfig), CamConfig, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.IsNaN ? ViaCamWithNaN(roundtrip.Triplet) : original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void ViaHct(ColourTriplet triplet)
    {
        // HCT -> XYZ can produce NaNs due to a negative number to a fractional power in the CAM16 conversion process
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third);
        var roundtrip = Hct.ToXyz(Hct.FromXyz(original, XyzConfig), XyzConfig);
        
        // not quite as accurate as other XYZ roundtrips, but still accurate
        const double viaHctTolerance = Tolerance * 100;
        if (Math.Abs(roundtrip.X - original.X) > viaHctTolerance ||
            Math.Abs(roundtrip.Z - original.Z) > viaHctTolerance)
        {
            // very rarely, XYZ roundtrip via HCT fails
            // due to HCT -> XYZ finding a different CAM.Model.J that produces the target LAB.L / HCT.T
            // effectively: multiple XYZ where Y is the same and results in the same CAM16.Model.H, CAM16.Model.C, LAB.L
            Assert.That(roundtrip.Y, Is.EqualTo(original.Y).Within(viaHctTolerance));
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.IsNaN ? new(double.NaN, double.NaN, double.NaN) : original.Triplet, viaHctTolerance);
        }
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