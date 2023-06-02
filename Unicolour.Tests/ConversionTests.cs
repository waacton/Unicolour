namespace Wacton.Unicolour.Tests;

using System;
using System.Drawing;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ConversionTests
{
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    private const double IctcpScalar = 100;
    private const double JzazbzScalar = 100;

    private const double DefaultTolerance = 0.00000000001;
    private const double RbgTolerance = 0.00000005;
    private const double HsbTolerance = 0.000000001;
    private const double HslTolerance = 0.0000000001;
    private const double XyzTolerance = 0.0000000005;
    private const double LuvTolerance = 0.00000001;
    private const double JzazbzTolerance = 0.00000005;
    private const double OklabTolerance = 0.000005;
    private const double Cam02Tolerance = 0.0000001;
    private const double Cam16Tolerance = 0.00000001;

    // no point doing this test starting with Wikipedia's HSB / HSL values since they're rounded
    [TestCaseSource(typeof(NamedColours), nameof(Utils.NamedColours.All))]
    public void NamedColours(TestColour namedColour) => AssertRgbConversion(namedColour);

    [TestCaseSource(typeof(NamedColours), nameof(Utils.NamedColours.All))]
    public void RgbNamedRoundTrip(TestColour namedColour) => AssertRgbRoundTrip(namedColour);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void Rgb255RoundTrip(ColourTriplet triplet) => AssertRgb255RoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void RgbRoundTrip(ColourTriplet triplet) => AssertRgbRoundTrip(triplet);

    [TestCaseSource(typeof(NamedColours), nameof(Utils.NamedColours.All))]
    public void HsbNamedRoundTrip(TestColour namedColour) => AssertHsbRoundTrip(namedColour);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))]
    public void HsbRoundTrip(ColourTriplet triplet) => AssertHsbRoundTrip(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(Utils.NamedColours.All))]
    public void HslNamedRoundTrip(TestColour namedColour) => AssertHslRoundTrip(namedColour);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HslTriplets))]
    public void HslRoundTrip(ColourTriplet triplet) => AssertHslRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HwbTriplets))]
    public void HwbRoundTrip(ColourTriplet triplet) => AssertHwbRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void XyzRoundTrip(ColourTriplet triplet) => AssertXyzRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void XyyRoundTrip(ColourTriplet triplet) => AssertXyyRoundTrip(triplet);

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LabTriplets))]
    public void LabRoundTrip(ColourTriplet triplet) => AssertLabRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchabTriplets))]
    public void LchabRoundTrip(ColourTriplet triplet) => AssertLchabRoundTrip(triplet);

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LuvTriplets))]
    public void LuvRoundTrip(ColourTriplet triplet) => AssertLuvRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchuvTriplets))]
    public void LchuvRoundTrip(ColourTriplet triplet) => AssertLchuvRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsluvTriplets))]
    public void HsluvRoundTrip(ColourTriplet triplet) => AssertHsluvRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HpluvTriplets))]
    public void HpluvRoundTrip(ColourTriplet triplet) => AssertHpluvRoundTrip(triplet);

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.IctcpTriplets))]
    public void IctcpRoundTrip(ColourTriplet triplet) => AssertIctcpRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.JzazbzTriplets))]
    public void JzazbzRoundTrip(ColourTriplet triplet) => AssertJzazbzRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.JzczhzTriplets))]
    public void JzczhzRoundTrip(ColourTriplet triplet) => AssertJzczhzRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklabTriplets))]
    public void OklabRoundTrip(ColourTriplet triplet) => AssertOklabRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklchTriplets))]
    public void OklchRoundTrip(ColourTriplet triplet) => AssertOklchRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Cam02Triplets))]
    public void Cam02RoundTrip(ColourTriplet triplet) => AssertCam02RoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Cam16Triplets))]
    public void Cam16RoundTrip(ColourTriplet triplet) => AssertCam16RoundTrip(triplet);
    
    private static void AssertRgbConversion(TestColour namedColour)
    {
        var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
        var rgb = new Rgb(systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0, RgbConfig);
        var hsb = Hsb.FromRgb(rgb);
        var hsl = Hsl.FromHsb(hsb);
        
        var expectedRoundedHsb = namedColour.Hsb;
        var expectedRoundedHsl = namedColour.Hsl;
        
        Assert.That(Math.Round(hsb.H), Is.EqualTo(expectedRoundedHsb!.First), namedColour.Name!);
        Assert.That(Math.Round(hsb.S, 2), Is.EqualTo(expectedRoundedHsb.Second), namedColour.Name!);
        Assert.That(Math.Round(hsb.B, 2), Is.EqualTo(expectedRoundedHsb.Third), namedColour.Name!);

        // within 0.02 because it seems like some of wikipedia's HSL values have questionable rounding...
        Assert.That(Math.Round(hsl.H), Is.EqualTo(expectedRoundedHsl!.First), namedColour.Name!);
        Assert.That(Math.Round(hsl.S, 2), Is.EqualTo(expectedRoundedHsl.Second).Within(0.02), namedColour.Name!);
        Assert.That(Math.Round(hsl.L, 2), Is.EqualTo(expectedRoundedHsl.Third).Within(0.02), namedColour.Name!);
    }

    private static void AssertRgbRoundTrip(TestColour namedColour) => AssertRgbRoundTrip(GetRgbTripletFromHex(namedColour.Hex!));
    private static void AssertRgb255RoundTrip(ColourTriplet triplet) => AssertRgbRoundTrip(GetNormalisedRgb255Triplet(triplet));
    private static void AssertRgbRoundTrip(ColourTriplet triplet) => AssertRgbRoundTrip(new Rgb(triplet.First, triplet.Second, triplet.Third, RgbConfig));
    private static void AssertRgbRoundTrip(Rgb original)
    {
        var viaHsb = Hsb.ToRgb(Hsb.FromRgb(original), RgbConfig);
        AssertUtils.AssertTriplet(viaHsb.Triplet, original.Triplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.ConstrainedTriplet, original.ConstrainedTriplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.Linear.Triplet, original.Linear.Triplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.Linear.ConstrainedTriplet, original.Linear.ConstrainedTriplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.Byte255.Triplet, original.Byte255.Triplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.Byte255.ConstrainedTriplet, original.Byte255.ConstrainedTriplet, RbgTolerance);
        
        var viaXyz = Rgb.FromXyz(Rgb.ToXyz(original, RgbConfig, XyzConfig), RgbConfig, XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, original.Triplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaXyz.ConstrainedTriplet, original.ConstrainedTriplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaXyz.Linear.Triplet, original.Linear.Triplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaXyz.Linear.ConstrainedTriplet, original.Linear.ConstrainedTriplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaXyz.Byte255.Triplet, original.Byte255.Triplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaXyz.Byte255.ConstrainedTriplet, original.Byte255.ConstrainedTriplet, RbgTolerance);
    }

    private static void AssertHsbRoundTrip(TestColour namedColour) => AssertHsbRoundTrip(namedColour.Hsb!);
    private static void AssertHsbRoundTrip(ColourTriplet triplet) => AssertHsbRoundTrip(new Hsb(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHsbRoundTrip(Hsb original)
    {
        var viaRgb = Hsb.FromRgb(Hsb.ToRgb(original, RgbConfig));
        AssertUtils.AssertTriplet(viaRgb.Triplet, original.Triplet, HsbTolerance);
        
        var viaHsl = Hsl.ToHsb(Hsl.FromHsb(original));
        AssertUtils.AssertTriplet(viaHsl.Triplet, original.Triplet, HsbTolerance);
        
        var viaHwb = Hwb.ToHsb(Hwb.FromHsb(original));
        AssertUtils.AssertTriplet(viaHwb.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertHslRoundTrip(TestColour namedColour) => AssertHslRoundTrip(namedColour.Hsl!);
    private static void AssertHslRoundTrip(ColourTriplet triplet) => AssertHslRoundTrip(new Hsl(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHslRoundTrip(Hsl original)
    {
        var viaHsb = Hsl.FromHsb(Hsl.ToHsb(original));
        AssertUtils.AssertTriplet(viaHsb.Triplet, original.Triplet, HslTolerance);
    }
    
    private static void AssertHwbRoundTrip(ColourTriplet triplet) => AssertHwbRoundTrip(new Hwb(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHwbRoundTrip(Hwb original)
    {
        // note: cannot test round trip of all HWB values as HWB <-> HSB is not 1:1
        // since when HWB W + B > 100%, it is the same as another HWB where W + B = 100%
        // (e.g. W 100 B 50 == W 66.666 B 33.333)
        // and HSB -> HWB will always produce HWB that results in W + B <= 100%
        var scale = original.ConstrainedW + original.ConstrainedB;
        var scaledHwb = new Hwb(original.H, original.ConstrainedW / scale, original.ConstrainedB / scale);

        var needsScaling = scale > 1.0;
        if (needsScaling)
        {
            var hsbFromOriginal = Hwb.ToHsb(original);
            var hsbFromScaled = Hwb.ToHsb(scaledHwb);
            AssertUtils.AssertTriplet(hsbFromOriginal.Triplet, hsbFromScaled.Triplet, DefaultTolerance);
        }

        var viaHsb = Hwb.FromHsb(Hwb.ToHsb(original));
        var expectedHwb = needsScaling ? scaledHwb.Triplet : original.Triplet;
        AssertUtils.AssertTriplet(viaHsb.Triplet, expectedHwb, DefaultTolerance);
    }
    
    private static void AssertXyzRoundTrip(ColourTriplet triplet) => AssertXyzRoundTrip(new Xyz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertXyzRoundTrip(Xyz original)
    {
        var viaRgb = Rgb.ToXyz(Rgb.FromXyz(original, RgbConfig,  XyzConfig), RgbConfig, XyzConfig);
        AssertUtils.AssertTriplet(viaRgb.Triplet, original.Triplet, XyzTolerance);
        
        var viaXyy = Xyy.ToXyz(Xyy.FromXyz(original, XyzConfig));
        AssertUtils.AssertTriplet(viaXyy.Triplet, original.Triplet, XyzTolerance);
        
        var viaLab = Lab.ToXyz(Lab.FromXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaLab.Triplet, original.Triplet, XyzTolerance);
        
        var viaLuv = Luv.ToXyz(Luv.FromXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaLuv.Triplet, original.Triplet, XyzTolerance);

        var viaIctcp = Ictcp.ToXyz(Ictcp.FromXyz(original, IctcpScalar, XyzConfig), IctcpScalar, XyzConfig);
        AssertUtils.AssertTriplet(viaIctcp.Triplet, viaIctcp.Triplet, XyzTolerance);
        
        var viaJzazbz = Jzazbz.ToXyz(Jzazbz.FromXyz(original, JzazbzScalar, XyzConfig), JzazbzScalar, XyzConfig);
        AssertUtils.AssertTriplet(viaJzazbz.Triplet, viaJzazbz.Triplet, XyzTolerance);
        
        var viaOklab = Oklab.ToXyz(Oklab.FromXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaOklab.Triplet, original.Triplet, XyzTolerance);
        
        // CAM02 -> XYZ often produces NaNs due to a negative number to a fractional power in the conversion process
        var viaCam02 = Cam02.ToXyz(Cam02.FromXyz(original, CamConfig,  XyzConfig), CamConfig, XyzConfig);
        AssertUtils.AssertTriplet(viaCam02.Triplet, viaCam02.IsNaN ? ViaCamWithNaN(viaCam02.Triplet) : original.Triplet, XyzTolerance);
        
        // CAM16 -> XYZ often produces NaNs due to a negative number to a fractional power in the conversion process
        var viaCam16 = Cam16.ToXyz(Cam16.FromXyz(original, CamConfig,  XyzConfig), CamConfig, XyzConfig);
        AssertUtils.AssertTriplet(viaCam16.Triplet, viaCam16.IsNaN ? ViaCamWithNaN(viaCam16.Triplet) : original.Triplet, XyzTolerance);
    }
    
    private static void AssertXyyRoundTrip(ColourTriplet triplet) => AssertXyyRoundTrip(new Xyy(triplet.First, triplet.Second, triplet.Third));
    private static void AssertXyyRoundTrip(Xyy original)
    {
        var viaXyz = Xyy.FromXyz(Xyy.ToXyz(original), XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, original.Triplet, XyzTolerance);
    }
    
    private static void AssertLabRoundTrip(ColourTriplet triplet) => AssertLabRoundTrip(new Lab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLabRoundTrip(Lab original)
    {
        var viaXyz = Lab.FromXyz(Lab.ToXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, original.Triplet, DefaultTolerance);
        
        var viaLchab = Lchab.ToLab(Lchab.FromLab(original));
        AssertUtils.AssertTriplet(viaLchab.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertLchabRoundTrip(ColourTriplet triplet) => AssertLchabRoundTrip(new Lchab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLchabRoundTrip(Lchab original)
    {
        var viaLab = Lchab.FromLab(Lchab.ToLab(original));
        AssertUtils.AssertTriplet(viaLab.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertLuvRoundTrip(ColourTriplet triplet) => AssertLuvRoundTrip(new Luv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLuvRoundTrip(Luv original)
    {
        var viaXyz = Luv.FromXyz(Luv.ToXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, original.Triplet, LuvTolerance);
        
        var viaLchuv = Lchuv.ToLuv(Lchuv.FromLuv(original));
        AssertUtils.AssertTriplet(viaLchuv.Triplet, original.Triplet, LuvTolerance);
    }
    
    private static void AssertLchuvRoundTrip(ColourTriplet triplet) => AssertLchuvRoundTrip(new Lchuv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLchuvRoundTrip(Lchuv original)
    {
        var viaLuv = Lchuv.FromLuv(Lchuv.ToLuv(original));
        AssertUtils.AssertTriplet(viaLuv.Triplet, original.Triplet, DefaultTolerance);
        
        var viaHsluv = Hsluv.ToLchuv(Hsluv.FromLchuv(original));
        AssertUtils.AssertTriplet(viaHsluv.Triplet, original.Triplet, DefaultTolerance);
        
        var viaHpluv = Hpluv.ToLchuv(Hpluv.FromLchuv(original));
        AssertUtils.AssertTriplet(viaHpluv.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertHsluvRoundTrip(ColourTriplet triplet) => AssertHsluvRoundTrip(new Hsluv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHsluvRoundTrip(Hsluv original)
    {
        var viaLch = Hsluv.FromLchuv(Hsluv.ToLchuv(original));
        AssertUtils.AssertTriplet(viaLch.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertHpluvRoundTrip(ColourTriplet triplet) => AssertHpluvRoundTrip(new Hpluv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHpluvRoundTrip(Hpluv original)
    {
        var viaLch = Hpluv.FromLchuv(Hpluv.ToLchuv(original));
        AssertUtils.AssertTriplet(viaLch.Triplet, original.Triplet, DefaultTolerance);
    }

    private static void AssertIctcpRoundTrip(ColourTriplet triplet) => AssertIctcpRoundTrip(new Ictcp(triplet.First, triplet.Second, triplet.Third));
    private static void AssertIctcpRoundTrip(Ictcp original)
    {
        // Ictcp -> XYZ often produces NaNs due to a negative number to a fractional power in the conversion process
        var viaXyz = Ictcp.FromXyz(Ictcp.ToXyz(original, IctcpScalar, XyzConfig), IctcpScalar, XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, viaXyz.IsNaN ? new(double.NaN, double.NaN, double.NaN) : original.Triplet, DefaultTolerance);
    }
    
    private static void AssertJzazbzRoundTrip(ColourTriplet triplet) => AssertJzazbzRoundTrip(new Jzazbz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertJzazbzRoundTrip(Jzazbz original)
    {
        // cannot test round trip of XYZ space as Jzazbz <-> XYZ is not 1:1, e.g.
        // - when Jzazbz inputs produces negative XYZ values, which are clamped during XYZ -> Jzazbz
        // - when Jzazbz negative inputs trigger a negative number to a fractional power, producing NaNs
        // var viaXyz = Jzazbz.FromXyz(Jzazbz.ToXyz(original, xyzConfig), xyzConfig);
        // AssertUtils.AssertTriplet(viaXyz.Triplet, viaXyz.IsNaN ? new(double.NaN, double.NaN, double.NaN) : original.Triplet, JzazbzTolerance);

        var viaJzczhz = Jzczhz.ToJzazbz(Jzczhz.FromJzazbz(original));
        AssertUtils.AssertTriplet(viaJzczhz.Triplet, original.Triplet, JzazbzTolerance);
    }
    
    private static void AssertJzczhzRoundTrip(ColourTriplet triplet) => AssertJzczhzRoundTrip(new Jzczhz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertJzczhzRoundTrip(Jzczhz original)
    {
        var viaJzazbz = Jzczhz.FromJzazbz(Jzczhz.ToJzazbz(original));
        AssertUtils.AssertTriplet(viaJzazbz.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertOklabRoundTrip(ColourTriplet triplet) => AssertOklabRoundTrip(new Oklab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertOklabRoundTrip(Oklab original)
    {
        var viaXyz = Oklab.FromXyz(Oklab.ToXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, original.Triplet, OklabTolerance);
        
        var viaOklch = Oklch.ToOklab(Oklch.FromOklab(original));
        AssertUtils.AssertTriplet(viaOklch.Triplet, original.Triplet, OklabTolerance);
    }
    
    private static void AssertOklchRoundTrip(ColourTriplet triplet) => AssertOklchRoundTrip(new Oklch(triplet.First, triplet.Second, triplet.Third));
    private static void AssertOklchRoundTrip(Oklch original)
    {
        var viaOklab = Oklch.FromOklab(Oklch.ToOklab(original));
        AssertUtils.AssertTriplet(viaOklab.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertCam02RoundTrip(ColourTriplet triplet) => AssertCam02RoundTrip(new Cam02(triplet.First, triplet.Second, triplet.Third, CamConfig));
    private static void AssertCam02RoundTrip(Cam02 original)
    {
        // CAM <-> XYZ often produces NaNs due to a negative number to a fractional power in the conversion process
        var viaXyz = Cam02.FromXyz(Cam02.ToXyz(original, CamConfig, XyzConfig), CamConfig, XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, viaXyz.IsNaN ? ViaCamWithNaN(viaXyz.Triplet) : original.Triplet, Cam02Tolerance);
    }
    
    private static void AssertCam16RoundTrip(ColourTriplet triplet) => AssertCam16RoundTrip(new Cam16(triplet.First, triplet.Second, triplet.Third, CamConfig));
    private static void AssertCam16RoundTrip(Cam16 original)
    {
        // CAM <-> XYZ often produces NaNs due to a negative number to a fractional power in the conversion process
        var viaXyz = Cam16.FromXyz(Cam16.ToXyz(original, CamConfig, XyzConfig), CamConfig, XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, viaXyz.IsNaN ? ViaCamWithNaN(viaXyz.Triplet) : original.Triplet, Cam16Tolerance);
    }

    private static ColourTriplet GetRgbTripletFromHex(string hex)
    {
        var (r255, g255, b255, _) = Wacton.Unicolour.Utils.ParseColourHex(hex);
        return new(r255 / 255.0, g255 / 255.0, b255 / 255.0);
    }
    
    private static ColourTriplet GetNormalisedRgb255Triplet(ColourTriplet triplet)
    {
        var (r255, g255, b255) = triplet;
        return new(r255 / 255.0, g255 / 255.0, b255 / 255.0);
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