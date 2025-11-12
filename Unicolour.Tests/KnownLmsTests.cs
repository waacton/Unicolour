using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownLmsTests
{
    private const double Tolerance = 0.00025;

    [TestCase(nameof(Illuminant.D65))]
    [TestCase(nameof(Illuminant.D50))]
    [TestCase(nameof(Illuminant.E))]
    public void Black(string illuminantName)
    {
        var black = new Unicolour(ConfigUtils.GetConfigWithStandardRgb(illuminantName), ColourSpace.Xyz, 0, 0, 0);
        TestUtils.AssertTriplet<Lms>(black, new(0.0, 0.0, 0.0), Tolerance);
    }
    
    [TestCase(nameof(Illuminant.D65))]
    [TestCase(nameof(Illuminant.D50))]
    [TestCase(nameof(Illuminant.E))]
    public void White(string illuminantName)
    {
        var whiteChromaticity = TestUtils.Illuminants[illuminantName].GetWhitePoint(Observer.Degree2).ToChromaticity();
        var white = new Unicolour(ConfigUtils.GetConfigWithStandardRgb(illuminantName), whiteChromaticity);
        TestUtils.AssertTriplet<Lms>(white, new(1.0, 1.0, 1.0), Tolerance); // white is always normalised as [1, 1, 1] regardless of XYZ illuminant
    }
    
    [TestCase(nameof(Illuminant.D65))]
    [TestCase(nameof(Illuminant.D50))]
    [TestCase(nameof(Illuminant.E))]
    public void Grey(string illuminantName)
    {
        var grey = StandardRgb.Grey.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        var luminance = grey.RelativeLuminance;
        TestUtils.AssertTriplet<Lms>(grey, new(luminance, luminance, luminance), Tolerance);
    }
    
    /*
     * not yet found reliable reference values for non-greyscale colours
     * so these tests are confirming expected relative LMS values
     */

    [TestCase(nameof(Illuminant.D65))]
    [TestCase(nameof(Illuminant.D50))]
    [TestCase(nameof(Illuminant.E))]
    public void Red(string illuminantName)
    {
        var red = StandardRgb.Red.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        var lms = red.Lms;
        Assert.That(lms.L, Is.GreaterThan(lms.M));
        Assert.That(lms.M, Is.GreaterThan(lms.S));
    }
    
    [TestCase(nameof(Illuminant.D65))]
    [TestCase(nameof(Illuminant.D50))]
    [TestCase(nameof(Illuminant.E))]
    public void Green(string illuminantName)
    {
        var green = StandardRgb.Green.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));
        var lms = green.Lms;
        Assert.That(lms.M, Is.GreaterThan(lms.L));
        Assert.That(lms.L, Is.GreaterThan(lms.S));
    }
    
    [TestCase(nameof(Illuminant.D65))]
    [TestCase(nameof(Illuminant.D50))]
    [TestCase(nameof(Illuminant.E))]
    public void Blue(string illuminantName)
    {
        var blue = StandardRgb.Blue.ConvertToConfiguration(ConfigUtils.GetConfigWithStandardRgb(illuminantName));;
        var lms = blue.Lms;
        Assert.That(lms.S, Is.GreaterThan(lms.M));
        Assert.That(lms.M, Is.GreaterThan(lms.L));
    }
}