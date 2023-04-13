namespace Wacton.Unicolour.Tests;

using System;
using System.Drawing;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ConversionTests
{
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
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

    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))] // no point doing this test starting with Wikipedia's HSB / HSL values since they're rounded
    public void NamedColoursMatchRgbConversion(TestColour namedColour) => AssertRgbConversion(namedColour);

    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void RgbNamedSameAfterRoundTripConversion(TestColour namedColour) => AssertRgbRoundTrip(namedColour);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Rgb255Triplets))]
    public void Rgb255SameAfterRoundTripConversion(ColourTriplet triplet) => AssertRgb255RoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.RgbTriplets))]
    public void RgbSameAfterRoundTripConversion(ColourTriplet triplet) => AssertRgbRoundTrip(triplet);

    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void HsbNamedSameAfterRoundTripConversion(TestColour namedColour) => AssertHsbRoundTrip(namedColour);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsbTriplets))]
    public void HsbSameAfterRoundTripConversion(ColourTriplet triplet) => AssertHsbRoundTrip(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void HslNamedSameAfterRoundTripConversion(TestColour namedColour) => AssertHslRoundTrip(namedColour);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HslTriplets))]
    public void HslSameAfterRoundTripConversion(ColourTriplet triplet) => AssertHslRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HwbTriplets))]
    public void HwbSameAfterRoundTripConversion(ColourTriplet triplet) => AssertHwbRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyzTriplets))]
    public void XyzSameAfterRoundTripConversion(ColourTriplet triplet) => AssertXyzRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void XyySameAfterRoundTripConversion(ColourTriplet triplet) => AssertXyyRoundTrip(triplet);

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LabTriplets))]
    public void LabSameAfterRoundTripConversion(ColourTriplet triplet) => AssertLabRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchabTriplets))]
    public void LchabSameAfterRoundTripConversion(ColourTriplet triplet) => AssertLchabRoundTrip(triplet);

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LuvTriplets))]
    public void LuvSameAfterRoundTripConversion(ColourTriplet triplet) => AssertLuvRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.LchuvTriplets))]
    public void LchuvSameAfterRoundTripConversion(ColourTriplet triplet) => AssertLchuvRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HsluvTriplets))]
    public void HsluvSameAfterRoundTripConversion(ColourTriplet triplet) => AssertHsluvRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HpluvTriplets))]
    public void HpluvSameAfterRoundTripConversion(ColourTriplet triplet) => AssertHpluvRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.IctcpTriplets))]
    public void IctcpSameAfterRoundTripConversion(ColourTriplet triplet) => AssertIctcpRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.JzazbzTriplets))]
    public void JzazbzSameAfterRoundTripConversion(ColourTriplet triplet) => AssertJzazbzRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.JzczhzTriplets))]
    public void JzczhzSameAfterRoundTripConversion(ColourTriplet triplet) => AssertJzczhzRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklabTriplets))]
    public void OklabSameAfterRoundTripConversion(ColourTriplet triplet) => AssertOklabRoundTrip(triplet);
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.OklchTriplets))]
    public void OklchSameAfterRoundTripConversion(ColourTriplet triplet) => AssertOklchRoundTrip(triplet);
    
    private static void AssertRgbConversion(TestColour namedColour)
    {
        var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
        var rgb = new Rgb(systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0, RgbConfig);
        var hsb = Conversion.RgbToHsb(rgb);
        var hsl = Conversion.HsbToHsl(hsb);
        
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
        var viaHsb = Conversion.HsbToRgb(Conversion.RgbToHsb(original), RgbConfig);
        AssertUtils.AssertTriplet(viaHsb.Triplet, original.Triplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.ConstrainedTriplet, original.ConstrainedTriplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.Linear.Triplet, original.Linear.Triplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.Linear.ConstrainedTriplet, original.Linear.ConstrainedTriplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.Byte255.Triplet, original.Byte255.Triplet, RbgTolerance);
        AssertUtils.AssertTriplet(viaHsb.Byte255.ConstrainedTriplet, original.Byte255.ConstrainedTriplet, RbgTolerance);
        
        var viaXyz = Conversion.XyzToRgb(Conversion.RgbToXyz(original, RgbConfig, XyzConfig), RgbConfig, XyzConfig);
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
        var viaRgb = Conversion.RgbToHsb(Conversion.HsbToRgb(original, RgbConfig));
        AssertUtils.AssertTriplet(viaRgb.Triplet, original.Triplet, HsbTolerance);
        
        var viaHsl = Conversion.HslToHsb(Conversion.HsbToHsl(original));
        AssertUtils.AssertTriplet(viaHsl.Triplet, original.Triplet, HsbTolerance);
        
        var viaHwb = Conversion.HwbToHsb(Conversion.HsbToHwb(original));
        AssertUtils.AssertTriplet(viaHwb.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertHslRoundTrip(TestColour namedColour) => AssertHslRoundTrip(namedColour.Hsl!);
    private static void AssertHslRoundTrip(ColourTriplet triplet) => AssertHslRoundTrip(new Hsl(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHslRoundTrip(Hsl original)
    {
        var viaHsb = Conversion.HsbToHsl(Conversion.HslToHsb(original));
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
            var hsbFromOriginal = Conversion.HwbToHsb(original);
            var hsbFromScaled = Conversion.HwbToHsb(scaledHwb);
            AssertUtils.AssertTriplet(hsbFromOriginal.Triplet, hsbFromScaled.Triplet, DefaultTolerance);
        }

        var viaHsb = Conversion.HsbToHwb(Conversion.HwbToHsb(original));
        var expectedHwb = needsScaling ? scaledHwb.Triplet : original.Triplet;
        AssertUtils.AssertTriplet(viaHsb.Triplet, expectedHwb, DefaultTolerance);
    }
    
    private static void AssertXyzRoundTrip(ColourTriplet triplet) => AssertXyzRoundTrip(new Xyz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertXyzRoundTrip(Xyz original)
    {
        var viaRgb = Conversion.RgbToXyz(Conversion.XyzToRgb(original, RgbConfig,  XyzConfig), RgbConfig, XyzConfig);
        AssertUtils.AssertTriplet(viaRgb.Triplet, original.Triplet, XyzTolerance);
        
        var viaXyy = Conversion.XyyToXyz(Conversion.XyzToXyy(original, XyzConfig));
        AssertUtils.AssertTriplet(viaXyy.Triplet, original.Triplet, XyzTolerance);

        var viaLab = Conversion.LabToXyz(Conversion.XyzToLab(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaLab.Triplet, original.Triplet, XyzTolerance);
        
        var viaLuv = Conversion.LuvToXyz(Conversion.XyzToLuv(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaLuv.Triplet, original.Triplet, XyzTolerance);
        
        var viaIctcp = Conversion.IctcpToXyz(Conversion.XyzToIctcp(original, XyzConfig, IctcpScalar), XyzConfig, IctcpScalar);
        AssertUtils.AssertTriplet(viaIctcp.Triplet, viaIctcp.Triplet, XyzTolerance);
        
        var viaJzazbz = Conversion.JzazbzToXyz(Conversion.XyzToJzazbz(original, XyzConfig, JzazbzScalar), XyzConfig, JzazbzScalar);
        AssertUtils.AssertTriplet(viaJzazbz.Triplet, viaJzazbz.Triplet, XyzTolerance);
        
        var viaOklab = Conversion.OklabToXyz(Conversion.XyzToOklab(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaOklab.Triplet, original.Triplet, XyzTolerance);
    }
    
    private static void AssertXyyRoundTrip(ColourTriplet triplet) => AssertXyyRoundTrip(new Xyy(triplet.First, triplet.Second, triplet.Third));
    private static void AssertXyyRoundTrip(Xyy original)
    {
        var viaXyz = Conversion.XyzToXyy(Conversion.XyyToXyz(original), XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, original.Triplet, XyzTolerance);
    }
    
    private static void AssertLabRoundTrip(ColourTriplet triplet) => AssertLabRoundTrip(new Lab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLabRoundTrip(Lab original)
    {
        var viaXyz = Conversion.XyzToLab(Conversion.LabToXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, original.Triplet, DefaultTolerance);
        
        var viaLchab = Conversion.LchabToLab(Conversion.LabToLchab(original));
        AssertUtils.AssertTriplet(viaLchab.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertLchabRoundTrip(ColourTriplet triplet) => AssertLchabRoundTrip(new Lchab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLchabRoundTrip(Lchab original)
    {
        var viaLab = Conversion.LabToLchab(Conversion.LchabToLab(original));
        AssertUtils.AssertTriplet(viaLab.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertLuvRoundTrip(ColourTriplet triplet) => AssertLuvRoundTrip(new Luv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLuvRoundTrip(Luv original)
    {
        var viaXyz = Conversion.XyzToLuv(Conversion.LuvToXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, original.Triplet, LuvTolerance);
        
        var viaLchuv = Conversion.LchuvToLuv(Conversion.LuvToLchuv(original));
        AssertUtils.AssertTriplet(viaLchuv.Triplet, original.Triplet, LuvTolerance);
    }
    
    private static void AssertLchuvRoundTrip(ColourTriplet triplet) => AssertLchuvRoundTrip(new Lchuv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertLchuvRoundTrip(Lchuv original)
    {
        var viaLuv = Conversion.LuvToLchuv(Conversion.LchuvToLuv(original));
        AssertUtils.AssertTriplet(viaLuv.Triplet, original.Triplet, DefaultTolerance);
        
        var viaHsluv = Conversion.HsluvToLchuv(Conversion.LchuvToHsluv(original));
        AssertUtils.AssertTriplet(viaHsluv.Triplet, original.Triplet, DefaultTolerance);
        
        var viaHpluv = Conversion.HpluvToLchuv(Conversion.LchuvToHpluv(original));
        AssertUtils.AssertTriplet(viaHpluv.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertHsluvRoundTrip(ColourTriplet triplet) => AssertHsluvRoundTrip(new Hsluv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHsluvRoundTrip(Hsluv original)
    {
        var viaLch = Conversion.LchuvToHsluv(Conversion.HsluvToLchuv(original));
        AssertUtils.AssertTriplet(viaLch.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertHpluvRoundTrip(ColourTriplet triplet) => AssertHpluvRoundTrip(new Hpluv(triplet.First, triplet.Second, triplet.Third));
    private static void AssertHpluvRoundTrip(Hpluv original)
    {
        var viaLch = Conversion.LchuvToHpluv(Conversion.HpluvToLchuv(original));
        AssertUtils.AssertTriplet(viaLch.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertIctcpRoundTrip(ColourTriplet triplet) => AssertIctcpRoundTrip(new Ictcp(triplet.First, triplet.Second, triplet.Third));
    private static void AssertIctcpRoundTrip(Ictcp original)
    {
        // Ictcp -> XYZ often produces NaNs due to a negative number to a fractional power in the conversion process
        var viaXyz = Conversion.XyzToIctcp(Conversion.IctcpToXyz(original, XyzConfig, IctcpScalar), XyzConfig, IctcpScalar);
        AssertUtils.AssertTriplet(viaXyz.Triplet, viaXyz.IsNaN ? new(double.NaN, double.NaN, double.NaN) : original.Triplet, DefaultTolerance);
    }
    
    private static void AssertJzazbzRoundTrip(ColourTriplet triplet) => AssertJzazbzRoundTrip(new Jzazbz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertJzazbzRoundTrip(Jzazbz original)
    {
        // cannot test round trip of XYZ space as Jzazbz <-> XYZ is not 1:1, e.g.
        // - when Jzazbz inputs produces negative XYZ values, which are clamped during XYZ -> Jzazbz
        // - when Jzazbz negative inputs trigger a negative number to a fractional power, producing NaNs
        // var viaXyz = Conversion.XyzToJzazbz(Conversion.JzazbzToXyz(original, xyzConfig), xyzConfig);
        // AssertUtils.AssertTriplet(viaXyz.Triplet, viaXyz.IsNaN ? new(double.NaN, double.NaN, double.NaN) : original.Triplet, JzazbzTolerance);

        var viaJzczhz = Conversion.JzczhzToJzazbz(Conversion.JzazbzToJzczhz(original));
        AssertUtils.AssertTriplet(viaJzczhz.Triplet, original.Triplet, JzazbzTolerance);
    }
    
    private static void AssertJzczhzRoundTrip(ColourTriplet triplet) => AssertJzczhzRoundTrip(new Jzczhz(triplet.First, triplet.Second, triplet.Third));
    private static void AssertJzczhzRoundTrip(Jzczhz original)
    {
        var viaJzazbz = Conversion.JzazbzToJzczhz(Conversion.JzczhzToJzazbz(original));
        AssertUtils.AssertTriplet(viaJzazbz.Triplet, original.Triplet, DefaultTolerance);
    }
    
    private static void AssertOklabRoundTrip(ColourTriplet triplet) => AssertOklabRoundTrip(new Oklab(triplet.First, triplet.Second, triplet.Third));
    private static void AssertOklabRoundTrip(Oklab original)
    {
        var viaXyz = Conversion.XyzToOklab(Conversion.OklabToXyz(original, XyzConfig), XyzConfig);
        AssertUtils.AssertTriplet(viaXyz.Triplet, original.Triplet, OklabTolerance);
        
        var viaOklch = Conversion.OklchToOklab(Conversion.OklabToOklch(original));
        AssertUtils.AssertTriplet(viaOklch.Triplet, original.Triplet, OklabTolerance);
    }
    
    private static void AssertOklchRoundTrip(ColourTriplet triplet) => AssertOklchRoundTrip(new Oklch(triplet.First, triplet.Second, triplet.Third));
    private static void AssertOklchRoundTrip(Oklch original)
    {
        var viaOklab = Conversion.OklabToOklch(Conversion.OklchToOklab(original));
        AssertUtils.AssertTriplet(viaOklab.Triplet, original.Triplet, DefaultTolerance);
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
}