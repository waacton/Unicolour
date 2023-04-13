namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class HsluvTests
{
    /*
     * note that at each step of the conversion process, more tolerance is required to match the HSLuv test data set
     * this divergence is likely due to slight differences in the initial RGB -> XYZ matrix used
     * HSLuv reference implementation uses hardcoded values, for which I can't find the source       (first matrix value: 0.41239079926595)
     * Unicolour calculates matrix from chromaticities and matches Bruce Lindbloom's sRGB D65 matrix (first matrix value: 0.41245643908969187)
     */
    [TestCaseSource(typeof(HsluvTestColour), nameof(HsluvTestColour.All))]
    public static void SnapshotTestColour(TestColour testColour)
    {
        var hex = testColour.Hex!;
        var unicolour = Unicolour.FromHex(hex);
        var info = $"{hex} \n-> RGB: {unicolour.Rgb} \n-> XYZ: {unicolour.Xyz} \n-> LUV: {unicolour.Luv} \n-> LCH: {unicolour.Lchuv}";
        
        ColourTriplet HandleNoHue(ColourTriplet triplet, bool hasHue) => hasHue ? triplet : triplet.WithHueOverride(0);
        var lchuv = HandleNoHue(unicolour.Lchuv.Triplet, unicolour.Lchuv.IsEffectivelyHued);
        var hsluv = HandleNoHue(unicolour.Hsluv.Triplet, unicolour.Hsluv.IsEffectivelyHued);
        var hpluv = HandleNoHue(unicolour.Hpluv.Triplet, unicolour.Hpluv.IsEffectivelyHued);
        
        // accuracy drops off when saturation goes beyond 100% (mostly at 1,500%+), so be slightly more tolerant for larger values
        Assert(unicolour.Rgb.Triplet, testColour.Rgb!, 0.00000000001, info);
        Assert(unicolour.Xyz.Triplet, testColour.Xyz!, 0.0005, info);
        Assert(unicolour.Luv.Triplet, testColour.Luv!, 0.025, info);
        Assert(lchuv, testColour.Lchuv!, 0.25, info);
        Assert(hsluv, testColour.Hsluv!, tolerance: 0.06, info);
        Assert(hpluv, testColour.Hpluv!, tolerance: hpluv.Second > 100 ? 0.135 : 0.06, info);
    }

    private static void Assert(ColourTriplet actual, ColourTriplet expected, double tolerance, string info)
    {
        AssertUtils.AssertTriplet(actual, expected, tolerance, info);
    }
}