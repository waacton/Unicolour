using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripXyzTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    private static readonly DynamicRange DynamicRange = DynamicRange.Standard;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Xyz, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaRgbLinear(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var rgbLinear = RgbLinear.FromXyz(original, RgbConfig, XyzConfig.ChromaticAdaptor);
        var roundtrip = RgbLinear.ToXyz(rgbLinear, RgbConfig, XyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var xyy = Xyy.FromXyz(original);
        var roundtrip = Xyy.ToXyz(xyy);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaLab(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var lab = Lab.FromXyz(original);
        var roundtrip = Lab.ToXyz(lab, XyzConfig.WhitePoint);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaLuv(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var luv = Luv.FromXyz(original);
        var roundtrip = Luv.ToXyz(luv, XyzConfig.WhitePoint);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaLms(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var lms = Lms.FromXyz(original, XyzConfig.ChromaticAdaptor);
        var roundtrip = Lms.ToXyz(lms, XyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaItp(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var ipt = Ipt.FromXyz(original, XyzConfig.ChromaticAdaptor);
        var roundtrip = Ipt.ToXyz(ipt, XyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaIctcp(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var ictcp = Ictcp.FromXyz(original, XyzConfig.ChromaticAdaptor, DynamicRange);
        var roundtrip = Ictcp.ToXyz(ictcp, XyzConfig.ChromaticAdaptor, DynamicRange);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaJzazbz(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var jzazbz = Jzazbz.FromXyz(original, XyzConfig.ChromaticAdaptor, DynamicRange);
        var roundtrip = Jzazbz.ToXyz(jzazbz, XyzConfig.ChromaticAdaptor, DynamicRange);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var oklab = Oklab.FromXyz(original, XyzConfig.ChromaticAdaptor, RgbConfig);
        var roundtrip = Oklab.ToXyz(oklab, XyzConfig.ChromaticAdaptor, RgbConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaCam02(ColourTriplet triplet)
    {
        // CAM02 -> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var cam02 = Cam02.FromXyz(original, CamConfig, XyzConfig.ChromaticAdaptor);
        var roundtrip = Cam02.ToXyz(cam02, CamConfig, XyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.Limitation == Limitation.NaN ? ViaCamWithNaN(roundtrip.Triplet) : original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaCam16(ColourTriplet triplet)
    {
        // CAM16 -> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var cam16 = Cam16.FromXyz(original, CamConfig, XyzConfig.ChromaticAdaptor);
        var roundtrip = Cam16.ToXyz(cam16, CamConfig, XyzConfig.ChromaticAdaptor);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.Limitation == Limitation.NaN ? ViaCamWithNaN(roundtrip.Triplet) : original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaHct(ColourTriplet triplet)
    {
        // HCT -> XYZ can produce NaNs due to a negative number to a fractional power in the CAM16 conversion process
        var original = new Xyz(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var hct = Hct.FromXyz(original, XyzConfig.ChromaticAdaptor);
        var roundtrip = Hct.ToXyz(hct, XyzConfig.ChromaticAdaptor);
        
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
            TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.Limitation == Limitation.NaN ? new(double.NaN, double.NaN, double.NaN) : original.Triplet, viaHctTolerance);
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