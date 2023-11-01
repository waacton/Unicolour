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
        var (r255, g255, b255, _) = Wacton.Unicolour.Utils.ParseColourHex(hex);
        var original = new Rgb(r255 / 255.0, g255 / 255.0, b255 / 255.0);
        var roundtrip = Rgb.FromRgbLinear(Rgb.ToRgbLinear(original, RgbConfig), RgbConfig);
        AssertRoundtrip(hex, original, roundtrip);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void ViaHsb(string hex) => AssertViaHsb(hex);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHsbFromNamed(TestColour namedColour) => AssertViaHsb(namedColour.Hex!);
    
    private static void AssertViaHsb(string hex)
    {
        var (r255, g255, b255, _) = Wacton.Unicolour.Utils.ParseColourHex(hex);
        var original = new Rgb(r255 / 255.0, g255 / 255.0, b255 / 255.0);
        var roundtrip = Hsb.ToRgb(Hsb.FromRgb(original));
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