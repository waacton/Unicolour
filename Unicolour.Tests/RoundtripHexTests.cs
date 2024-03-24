namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripHexTests
{
    private static readonly RgbConfiguration RgbConfig = RgbConfiguration.StandardRgb;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void ViaRgbLinear(string hex) => AssertViaRgbLinear(hex);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaRgbLinearFromNamed(TestColour namedColour) => AssertViaRgbLinear(namedColour.Hex!);
    
    private static void AssertViaRgbLinear(string hex)
    {
        var (r, g, b, _) = Wacton.Unicolour.Utils.ParseHex(hex);
        var original = new Rgb(r, g, b);
        var rgbLinear = Rgb.ToRgbLinear(original, RgbConfig);
        var roundtrip = Rgb.FromRgbLinear(rgbLinear, RgbConfig);
        AssertRoundtrip(hex, original, roundtrip);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void ViaHsb(string hex) => AssertViaHsb(hex);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHsbFromNamed(TestColour namedColour) => AssertViaHsb(namedColour.Hex!);
    
    private static void AssertViaHsb(string hex)
    {
        var (r, g, b, _) = Wacton.Unicolour.Utils.ParseHex(hex);
        var original = new Rgb(r, g, b);
        var hsb = Hsb.FromRgb(original);
        var roundtrip = Hsb.ToRgb(hsb);
        AssertRoundtrip(hex, original, roundtrip);
    }

    private static void AssertRoundtrip(string hex, Rgb original, Rgb roundtrip)
    {
        // trim then insert ensures all hex start with #
        var standardisedHex = hex.Trim('#').Insert(0, "#")[..7].ToUpper();
        Assert.That(original.Byte255.ConstrainedHex, Is.EqualTo(standardisedHex));
        Assert.That(roundtrip.Byte255.ConstrainedHex, Is.EqualTo(standardisedHex));
        Assert.That(original.Byte255.ConstrainedHex, Is.EqualTo(roundtrip.Byte255.ConstrainedHex));
    }
}