using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

/*
 * HDR nominal white luminance is 203 cd/m² with a PQ signal of 58%
 * however, the 58% is part of the PQ transfer function definition, so in practice it is a fixed value
 * ----------
 * HDR nominal white luminance is 203 cd/m² with a HLG signal of 75%
 * however, the 75% is not part of the HLG transfer function definition, so it cannot be enforced
 * (displays are expected to scale accordingly to 75%, but may deviate based on display characteristics)
 * ----------
 * Unicolour doesn't support scene light white luminance != display light white luminance
 * it assumes a scene of X cd/m² should display X cd/m²
 * these tests just simulate the workflow for testing purposes
 */
public class DynamicRangeTests
{
    /*
     * nominal scene white 203 cd/m²
     * scene light 203 cd/m² (100% luminance or 1.0 linear value)
     * display white 203 cd/m²
     * - sent to display encoded as InverseEOTF(1.0) * nominal 203 = 0.58 [i.e. the 58% definition]
     * - display light is the decoded EOTF(0.58) / display 203 = 1.0 [203 / 203]
     */
    [TestCase(203, 203, 1)]
    /*
     * nominal scene white 203 cd/m²
     * scene light 101.5 cd/m² (50% luminance or 0.5 linear value)
     * display white 406 cd/m²
     * - sent to display encoded as InverseEOTF(0.5) * nominal 203 = 0.510
     * - display light is the decoded EOTF(0.510) / display 406 = 0.25 [101.5 / 406]
     */
    [TestCase(101.5, 406, 0.25)]
    /*
     * 0 scene light -> 0 display light
     * negative scene light -> NaN display light (not possible)
     */
    [TestCase(0, 203, 0)] 
    [TestCase(-0.000001, 203, double.NaN)]
    public void PqSceneToDisplay(double sceneLuminance, double displayWhite, double expectedDisplayLevel)
    {
        const double Tolerance = 0.00000000001;

        var sceneLevel = sceneLuminance / DynamicRange.NominalWhiteLuminance;
        
        // scene -> display
        var pqSignal = Pq.Smpte.InverseEotf(sceneLevel, DynamicRange.NominalWhiteLuminance);
        var displayLevel = Pq.Smpte.Eotf(pqSignal, displayWhite);
        Assert.That(displayLevel, Is.EqualTo(expectedDisplayLevel).Within(Tolerance));

        // display -> scene
        var reversePqSignal = Pq.Smpte.InverseEotf(displayLevel, displayWhite);
        Assert.That(reversePqSignal, Is.EqualTo(pqSignal).Within(Tolerance));
        var sceneLevelRoundtrip = Pq.Smpte.Eotf(reversePqSignal, DynamicRange.NominalWhiteLuminance);
        Assert.That(sceneLevelRoundtrip, sceneLevel >= 0
            ? Is.EqualTo(sceneLevel).Within(Tolerance)
            : Is.NaN);
    }
    
    /*
     * nominal scene white 203 cd/m² @ 75% HLG signal
     * scene light 203 cd/m² (100% luminance or 1.0 linear value)
     * display white 203 cd/m² @ 75% HLG signal
     * - sent to display encoded as OETF(InverseOETF(0.75) * 1.0) = OETF(0.265 * 1.0) = 0.75
     * - display light is the decoded InverseOETF(0.75) / InverseOETF(0.75) = 1.0 [0.265 / 0.265]
     */
    [TestCase(0.75, 203, 203, 0.75, 1)]
    /*
     * nominal scene white 203 cd/m² @ 100% HLG signal
     * scene light 203 cd/m² (100% luminance or 1.0 linear value)
     * display white 203 cd/m² @ 100% HLG signal
     * - sent to display encoded as OETF(InverseOETF(1.0) * 1.0) = OETF(1.0 * 1.0) = 1.0
     * - display light is the decoded InverseOETF(1.0) / InverseOETF(1.0) = 1.0 [1 / 1]
     */
    [TestCase(1, 203, 203, 1, 1)]
    /*
     * nominal scene white 203 cd/m² @ 75% HLG signal
     * scene light 203 cd/m² (100% luminance or 1.0 linear value)
     * display white 203 cd/m² @ 100% HLG signal
     * - sent to display encoded as OETF(InverseOETF(0.75) * 1.0) = OETF(0.265 * 1.0) = 0.75
     * - display light is the decoded InverseOETF(0.75) / InverseOETF(1.0) = 0.265 [0.265 / 1]
     */
    [TestCase(0.75, 203, 203, 1.00, 0.26496255333016128)]
    /*
     * nominal scene white 203 cd/m² @ 75% HLG signal
     * scene light 101.5 cd/m² (50% luminance or 0.5 linear value)
     * display white 406 cd/m² @ 75% HLG signal
     * - sent to display encoded as OETF(InverseOETF(0.75) * 0.5) = OETF(0.265 * 0.5) = 0.608
     * - display light is the decoded InverseOETF(0.608) / InverseOETF(0.75) * (203 / 406) = 0.25 [0.1325 / 0.265 * 0.5]
     */ 
    [TestCase(0.75, 101.5, 406, 0.75, 0.25)]
    /*
     * nominal scene white 203 cd/m² @ 100% HLG signal
     * scene light 101.5 cd/m² (50% luminance or 0.5 linear value)
     * display white 406 cd/m² @ 100% HLG signal
     * - sent to display encoded as OETF(InverseOETF(1.0) * 0.5) = OETF(1.0 * 0.5) = 0.872
     * - display light is the decoded InverseOETF(0.872) / InverseOETF(1.0) * (203 / 406) = 0.25 [0.5 / 1.0 * 0.5]
     */ 
    [TestCase(1, 101.5, 406, 1, 0.25)]
    /*
     * nominal scene white 203 cd/m² @ 50% HLG signal
     * scene light 101.5 cd/m² (50% luminance or 0.5 linear value)
     * display white 406 cd/m² @ 50% HLG signal
     * - sent to display encoded as OETF(InverseOETF(0.5) * 0.5) = OETF(0.0833 * 0.5) = 0.354
     * - display light is the decoded InverseOETF(0.354) / InverseOETF(0.5) * (203 / 406) = 0.25 [0.0417 / 0.0833 * 0.5]
     */ 
    [TestCase(0.5, 101.5, 406, 0.5, 0.25)]
    /*
     * nominal scene white 203 cd/m² @ 75% HLG signal
     * scene light 101.5 cd/m² (50% luminance or 0.5 linear value)
     * display white 406 cd/m² @ 100% HLG signal
     * - sent to display encoded as OETF(InverseOETF(0.75) * 0.5) = OETF(0.265 * 0.5) = 0.608
     * - display light is the decoded InverseOETF(0.608) / InverseOETF(1.0) * (203 / 406) = 0.06625 [0.1325 / 1.0 * 0.5]
     */
    [TestCase(0.75, 101.5, 406, 1, 0.066240638332540333)]
    /*
     * nominal scene white 203 cd/m² @ 75% HLG signal
     * scene light 101.5 cd/m² (50% luminance or 0.5 linear value)
     * display white 406 cd/m² @ 50% HLG signal
     * - sent to display encoded as OETF(InverseOETF(0.75) * 0.5) = OETF(0.265 * 0.5) = 0.608
     * - display light is the decoded InverseOETF(0.608) / InverseOETF(0.5) * (203 / 406) = 0.795 [0.1325 / 0.0833 * 0.5]
     */
    [TestCase(0.75, 101.5, 406, 0.5, 0.79488767935920068)]
    /*
     * nominal scene white 203 cd/m² @ 100% HLF signal
     * scene light 406 cd/m² (200% luminance or 2.0 linear value)
     * display white 101.5 cd/m² @ 100% HLG signal
     * - sent to display encoded as OETF(InverseOETF(1.0) * 2.0) = OETF(1.0 * 2.0) = 1.126
     * - display light is the decoded InverseOETF(1.126) / InverseOETF(1.0) * (203 / 101.5) = 4.0 [2 / 1 * 2]
     */
    [TestCase(1, 406, 101.5, 1, 4)]
    /*
     * nominal scene white 203 cd/m² @ 50% HLF signal
     * scene light 406 cd/m² (200% luminance or 2.0 linear value)
     * display white 101.5 cd/m² @ 75% HLG signal
     * - sent to display encoded as OETF(InverseOETF(0.75) * 2.0) = OETF(0.0833 * 2.0) = 0.656
     * - display light is the decoded InverseOETF(0.656) / InverseOETF(0.75) * (203 / 101.5) = 1.258 [0.1666 / 0.265 * 2]
     */
    [TestCase(0.5, 406, 101.5, 0.75, 1.2580393758350248)]
    /*
     * 0 scene light -> 0 display light
     * negative scene light -> NaN display light (not possible)
     */
    [TestCase(0.75, 0, 203, 0.75, 0)] 
    [TestCase(0.75, -0.000001, 203, 0.75, double.NaN)]
    public void HlgSceneToDisplay(double sceneWhiteLevel, double sceneLuminance, double displayWhite, double displayWhiteLevel, double expectedDisplayLevel)
    {
        const double Tolerance = 0.00000000001;
        
        var sceneLevel = sceneLuminance / DynamicRange.NominalWhiteLuminance;

        var nominalDynamicRange = new DynamicRange(DynamicRange.NominalWhiteLuminance, 1000, 0, sceneWhiteLevel);
        var displayDynamicRange = new DynamicRange(displayWhite, 1000, 0, displayWhiteLevel);
        
        // scene -> display
        var hlgSignal = Hlg.Oetf(sceneLevel, nominalDynamicRange);
        var displayLevel = Hlg.InverseOetf(hlgSignal, displayDynamicRange);
        Assert.That(displayLevel, Is.EqualTo(expectedDisplayLevel).Within(Tolerance));

        // display -> scene
        var reverseHlgSignal = Hlg.Oetf(displayLevel, displayDynamicRange);
        Assert.That(reverseHlgSignal, Is.EqualTo(hlgSignal).Within(Tolerance));
        var sceneLevelRoundtrip = Hlg.InverseOetf(reverseHlgSignal, nominalDynamicRange);
        Assert.That(sceneLevelRoundtrip, sceneLevel >= 0
            ? Is.EqualTo(sceneLevel).Within(Tolerance)
            : Is.NaN);
    }

    /*
     * dynamic range min luminance adjusts signal with a black lift, effectively mapping signal from [0, 1] to [beta, 1]
     * which shifts the range of all display values from [0, max] to [min, max], e.g.
     * - display white 203 cd/m² @ 100% HLG signal & min luminance 0 cd/m² -> signals [0, 1] map to display [0, 1]
     * - display white 203 cd/m² @ 75% HLG signal & min luminance 0 cd/m² -> signals [0, 0.75, 1] map to display [0, 1, 3.7741]
     * - display white 203 cd/m² @ 50% HLG signal & min luminance 0 cd/m² -> signals [0, 0.5, 1] map to display [0, 1, 12.0000]
     * - display white 203 cd/m² @ 100% HLG signal & min luminance 10 cd/m² -> signals [0, 1] map to display [0.02154, 1]
     * - display white 203 cd/m² @ 75% HLG signal & min luminance 10 cd/m² -> signals [0, 0.75, 1] map to display [0.08131, 1.3885, 3.7741]
     * - display white 203 cd/m² @ 50% HLG signal & min luminance 10 cd/m² -> signals [0, 0.5, 1] map to display [0.2585, 1.7408, 12.0000]
     * - display white 406 cd/m² @ 50% HLG signal & min luminance 10 cd/m² -> signals [0, 0.5, 1] map to display [0.1293, 0.8704, 6.0000]
     */
    [Test]
    public void HlgBlackLift(
        [Values(203, 406, 101.5)] double displayWhite, 
        [Values(0.75, 1, 0.5)] double displayWhiteLevel, 
        [Values(0, 0.005, 1, 10, 100)] double minLuminance,
        [Values(1000, 2000, 10000)] double maxLuminance)
    {
        const double Tolerance = 0.00000075;

        var blackLift = Hlg.BlackLift(maxLuminance, minLuminance);
        var whiteScale = displayWhite / DynamicRange.NominalWhiteLuminance;
        var scale = Hlg.InverseOetf(displayWhiteLevel) * whiteScale;
        
        var dynamicRange = new DynamicRange(displayWhite, maxLuminance, minLuminance, displayWhiteLevel);
        
        // 0 -> min value
        var displayLevelWhenZero = AssertDisplayLevel(0.0);
        var minDisplayLevel = Hlg.InverseOetf(blackLift) * (1 / scale);
        Assert.That(displayLevelWhenZero, Is.EqualTo(minDisplayLevel));
        Assert.That(displayLevelWhenZero, Is.EqualTo(dynamicRange.HlgMinLinear).Within(Tolerance));

        // 1.0 -> max value
        // (1.0000 when 203 white @ 100% · 0.5000 when 406 white @ 100% · 3.7741 when 203 white @ 75% · 1.8871 when 406 white @ 75% · 12.0000 when 203 white @ 50% · 6.0000 when 406 white @ 50%)
        var displayLevelWhenOne = AssertDisplayLevel(1.0);
        var maxDisplayLevel = 1.0 * (1 / scale);
        Assert.That(displayLevelWhenOne,  Is.EqualTo(maxDisplayLevel).Within(Tolerance));
        
        // white -> 1.0 (203 display white) · 0.5 (406 display white) · 2.0 (101.5 display white)
        var displayLevelWhenWhite = AssertDisplayLevel(displayWhiteLevel);
        var minWhiteLevel = 1.0 * (1 / whiteScale);
        Assert.That(displayLevelWhenWhite, minLuminance == 0
            ? Is.EqualTo(minWhiteLevel).Within(Tolerance)
            : Is.GreaterThanOrEqualTo(minWhiteLevel));
        
        // assuming sensible configuration so that HLG white level <= 100%
        // DisplayLevel(white) <= DisplayLevel(1)
        Assert.That(displayLevelWhenWhite, Is.LessThanOrEqualTo(displayLevelWhenOne));
        return;

        double AssertDisplayLevel(double signal)
        {
            var blackLiftedSignal = (1 - blackLift) * signal + blackLift;
            var displayLevelBeforeScale = Hlg.InverseOetf(blackLiftedSignal);
            var expectedDisplayLevel = displayLevelBeforeScale * (1 / scale);
            var actualDisplayLevel = Hlg.InverseOetf(signal, dynamicRange);
            Assert.That(actualDisplayLevel, Is.EqualTo(expectedDisplayLevel).Within(Tolerance));
            return expectedDisplayLevel;
        }
    }
}