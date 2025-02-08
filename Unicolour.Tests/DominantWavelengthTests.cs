using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class DominantWavelengthTests
{
    private static readonly Configuration RgbStandardXyzD65 = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65);
    private static readonly Configuration RgbStandardXyzE = new(RgbConfiguration.StandardRgb, new XyzConfiguration(Illuminant.E, Observer.Degree2));
    private static readonly Configuration RgbA98XyzD65 = new(RgbConfiguration.A98, XyzConfiguration.D65);
    private static readonly Configuration RgbA98XyzD50 = new(RgbConfiguration.A98, XyzConfiguration.D50);
    private static readonly Configuration RgbProPhotoXyzE = new(RgbConfiguration.ProPhoto, new XyzConfiguration(Illuminant.E, Observer.Degree2));
    private static readonly Configuration RgbProPhotoXyzA = new(RgbConfiguration.ProPhoto, new XyzConfiguration(Illuminant.A, Observer.Degree2));
    private static readonly Configuration RgbProPhotoXyzD75 = new(RgbConfiguration.ProPhoto, new XyzConfiguration(Illuminant.D75, Observer.Degree2));
    
    /*
     * expected colour values for these tests based on calculations from
     * http://www.brucelindbloom.com/index.html?ColorCalculator.html
     */
    private static readonly List<TestCaseData> RgbTestData =
    [
        new TestCaseData(RgbStandardXyzD65, 1, 0, 0, 611.4).SetName("sRGB, D65, Red"),
        new TestCaseData(RgbStandardXyzD65, 0, 1, 0, 549.1).SetName("sRGB, D65, Green"),
        new TestCaseData(RgbStandardXyzD65, 0, 0, 1, 464.2).SetName("sRGB, D65, Blue"),
        new TestCaseData(RgbStandardXyzD65, 0, 1, 1, 491.4).SetName("sRGB, D65, Cyan"),
        new TestCaseData(RgbStandardXyzD65, 1, 0, 1, -549.1).SetName("sRGB, D65, Magenta"),
        new TestCaseData(RgbStandardXyzD65, 1, 1, 0, 570.5).SetName("sRGB, D65, Yellow"),
        new TestCaseData(RgbStandardXyzD65, 1, 1, 1, double.NaN).SetName("sRGB, D65, White"),
        new TestCaseData(RgbStandardXyzD65, 0, 0, 0, double.NaN).SetName("sRGB, D65, Black"),

        new TestCaseData(RgbStandardXyzE, 1, 0, 0, 612.1).SetName("sRGB, E, Red"),
        new TestCaseData(RgbStandardXyzE, 0, 1, 0, 552.6).SetName("sRGB, E, Green"),
        new TestCaseData(RgbStandardXyzE, 0, 0, 1, 464.2).SetName("sRGB, E, Blue"),
        new TestCaseData(RgbStandardXyzE, 0, 1, 1, 491.8).SetName("sRGB, E, Cyan"),
        new TestCaseData(RgbStandardXyzE, 1, 0, 1, -552.6).SetName("sRGB, E, Magenta"),
        new TestCaseData(RgbStandardXyzE, 1, 1, 0, 573.2).SetName("sRGB, E, Yellow"),
        new TestCaseData(RgbStandardXyzE, 1, 1, 1, double.NaN).SetName("sRGB, E, White"),
        new TestCaseData(RgbStandardXyzE, 0, 0, 0, double.NaN).SetName("sRGB, E, Black"),

        new TestCaseData(RgbA98XyzD65, 1, 0, 0, 611.4).SetName("A98, D65, Red"),
        new TestCaseData(RgbA98XyzD65, 0, 1, 0, 534.7).SetName("A98, D65, Green"),
        new TestCaseData(RgbA98XyzD65, 0, 0, 1, 464.2).SetName("A98, D65, Blue"),
        new TestCaseData(RgbA98XyzD65, 0, 1, 1, 491.4).SetName("A98, D65, Cyan"),
        new TestCaseData(RgbA98XyzD65, 1, 0, 1, -534.7).SetName("A98, D65, Magenta"),
        new TestCaseData(RgbA98XyzD65, 1, 1, 0, 570.5).SetName("A98, D65, Yellow"),
        new TestCaseData(RgbA98XyzD65, 1, 1, 1, double.NaN).SetName("A98, D65, White"),
        new TestCaseData(RgbA98XyzD65, 0, 0, 0, double.NaN).SetName("A98, D65, Black"),

        new TestCaseData(RgbA98XyzD50, 1, 0, 0, 611.8).SetName("A98, D50, Red"),
        new TestCaseData(RgbA98XyzD50, 0, 1, 0, 536.9).SetName("A98, D50, Green"),
        new TestCaseData(RgbA98XyzD50, 0, 0, 1, 463.9).SetName("A98, D50, Blue"),
        new TestCaseData(RgbA98XyzD50, 0, 1, 1, 493.9).SetName("A98, D50, Cyan"),
        new TestCaseData(RgbA98XyzD50, 1, 0, 1, -536.9).SetName("A98, D50, Magenta"),
        new TestCaseData(RgbA98XyzD50, 1, 1, 0, 572.5).SetName("A98, D50, Yellow"),
        new TestCaseData(RgbA98XyzD50, 1, 1, 1, double.NaN).SetName("A98, D50, White"),
        new TestCaseData(RgbA98XyzD50, 0, 0, 0, double.NaN).SetName("A98, D50, Black"),

        new TestCaseData(RgbProPhotoXyzE, 1, 0, 0, -493.8).SetName("ProPhoto, E, Red"),
        new TestCaseData(RgbProPhotoXyzE, 0, 1, 0, 534.1).SetName("ProPhoto, E, Green"),
        new TestCaseData(RgbProPhotoXyzE, 0, 0, 1, 473.2).SetName("ProPhoto, E, Blue"),
        new TestCaseData(RgbProPhotoXyzE, 0, 1, 1, 493.8).SetName("ProPhoto, E, Cyan"),
        new TestCaseData(RgbProPhotoXyzE, 1, 0, 1, -534.1).SetName("ProPhoto, E, Magenta"),
        new TestCaseData(RgbProPhotoXyzE, 1, 1, 0, 576.1).SetName("ProPhoto, E, Yellow"),
        new TestCaseData(RgbProPhotoXyzE, 1, 1, 1, double.NaN).SetName("ProPhoto, E, White"),
        new TestCaseData(RgbProPhotoXyzE, 0, 0, 0, double.NaN).SetName("ProPhoto, E, Black"),

        new TestCaseData(RgbProPhotoXyzA, 1, 0, 0, 638.7).SetName("ProPhoto, A, Red"),
        new TestCaseData(RgbProPhotoXyzA, 0, 1, 0, 540.3).SetName("ProPhoto, A, Green"),
        new TestCaseData(RgbProPhotoXyzA, 0, 0, 1, 480.5).SetName("ProPhoto, A, Blue"),
        new TestCaseData(RgbProPhotoXyzA, 0, 1, 1, 503.2).SetName("ProPhoto, A, Cyan"),
        new TestCaseData(RgbProPhotoXyzA, 1, 0, 1, -540.3).SetName("ProPhoto, A, Magenta"),
        new TestCaseData(RgbProPhotoXyzA, 1, 1, 0, 582.7).SetName("ProPhoto, A, Yellow"),
        new TestCaseData(RgbProPhotoXyzA, 1, 1, 1, double.NaN).SetName("ProPhoto, A, White"),
        new TestCaseData(RgbProPhotoXyzA, 0, 0, 0, double.NaN).SetName("ProPhoto, A, Black"),

        new TestCaseData(RgbProPhotoXyzD75, 1, 0, 0, -492.3).SetName("ProPhoto, D75, Red"),
        new TestCaseData(RgbProPhotoXyzD75, 0, 1, 0, 530.2).SetName("ProPhoto, D75, Green"),
        new TestCaseData(RgbProPhotoXyzD75, 0, 0, 1, 472.2).SetName("ProPhoto, D75, Blue"),
        new TestCaseData(RgbProPhotoXyzD75, 0, 1, 1, 492.3).SetName("ProPhoto, D75, Cyan"),
        new TestCaseData(RgbProPhotoXyzD75, 1, 0, 1, -530.2).SetName("ProPhoto, D75, Magenta"),
        new TestCaseData(RgbProPhotoXyzD75, 1, 1, 0, 572.7).SetName("ProPhoto, D75, Yellow"),
        new TestCaseData(RgbProPhotoXyzD75, 1, 1, 1, double.NaN).SetName("ProPhoto, D75, White"),
        new TestCaseData(RgbProPhotoXyzD75, 0, 0, 0, double.NaN).SetName("ProPhoto, D75, Black")
    ];
    
    [TestCaseSource(nameof(RgbTestData))]
    public void RgbGamut(Configuration configuration, double r, double g, double b, double expectedWavelength)
    {
        var colour = new Unicolour(configuration, ColourSpace.Rgb, r, g, b);
        var hasLuminance = colour.Xyy.Luminance > 0;
        Assert.That(colour.DominantWavelength, Is.EqualTo(hasLuminance ? expectedWavelength : double.NaN).Within(0.25));
    }
    
    private static readonly Dictionary<(Illuminant illuminant, Observer observer), Configuration> Configurations = new()
    {
        { (Illuminant.D65, Observer.Degree2), new(xyzConfig: new(Illuminant.D65, Observer.Degree2)) },
        { (Illuminant.D65, Observer.Degree10), new(xyzConfig: new(Illuminant.D65, Observer.Degree10)) },
        { (Illuminant.E, Observer.Degree2), new(xyzConfig: new(Illuminant.E, Observer.Degree2)) },
        { (Illuminant.E, Observer.Degree10), new(xyzConfig: new(Illuminant.E, Observer.Degree10)) }
    };
    
    [Test]
    public void Monochromatic(
        [Range(360, 700)] int wavelength,
        [Values(nameof(Observer.Degree2), nameof(Observer.Degree10))] string observerName,
        [Values(nameof(Illuminant.D65), nameof(Illuminant.E))] string illuminantName)
    {
        var illuminant = TestUtils.Illuminants[illuminantName];
        var observer = TestUtils.Observers[observerName];
        var config = Configurations[(illuminant, observer)];
        
        var colour = new Unicolour(config, Spd.Monochromatic(wavelength));
        Assert.That(colour.DominantWavelength, Is.EqualTo(wavelength).Within(0.000000005));
    }
    
    /*
     * expected colour values for these tests based on calculations from
     * http://www.brucelindbloom.com/index.html?ColorCalculator.html
     */
    private static readonly List<TestCaseData> ImaginaryTestData =
    [
        new TestCaseData(RgbStandardXyzD65, 0, 0, 477.2).SetName("sRGB, D65, (0,0)"),
        new TestCaseData(RgbStandardXyzD65, 0, 1, 520.4).SetName("sRGB, D65, (0,1)"),
        new TestCaseData(RgbStandardXyzD65, 1, 0, -497.3).SetName("sRGB, D65, (1,0)"),
        new TestCaseData(RgbStandardXyzD65, 1, 1, 577.2).SetName("sRGB, D65, (1,1)"),

        new TestCaseData(RgbStandardXyzE, 0, 0, 476.8).SetName("sRGB, E, (0,0)"),
        new TestCaseData(RgbStandardXyzE, 0, 1, 521.2).SetName("sRGB, E, (0,1)"),
        new TestCaseData(RgbStandardXyzE, 1, 0, -498.2).SetName("sRGB, E, (1,0)"),
        new TestCaseData(RgbStandardXyzE, 1, 1, 578.1).SetName("sRGB, E, (1,1)"),

        new TestCaseData(RgbA98XyzD65, 0, 0, 477.2).SetName("A98, D65, (0,0)"),
        new TestCaseData(RgbA98XyzD65, 0, 1, 520.4).SetName("A98, D65, (0,1)"),
        new TestCaseData(RgbA98XyzD65, 1, 0, -497.3).SetName("A98, D65, (1,0)"),
        new TestCaseData(RgbA98XyzD65, 1, 1, 577.2).SetName("A98, D65, (1,1)"),

        new TestCaseData(RgbA98XyzD50, 0, 0, 477.1).SetName("A98, D50, (0,0)"),
        new TestCaseData(RgbA98XyzD50, 0, 1, 522.1).SetName("A98, D50, (0,1)"),
        new TestCaseData(RgbA98XyzD50, 1, 0, -500.2).SetName("A98, D50, (1,0)"),
        new TestCaseData(RgbA98XyzD50, 1, 1, 577.3).SetName("A98, D50, (1,1)"),

        new TestCaseData(RgbProPhotoXyzE, 0, 0, 476.8).SetName("ProPhoto, E, (0,0)"),
        new TestCaseData(RgbProPhotoXyzE, 0, 1, 521.2).SetName("ProPhoto, E, (0,1)"),
        new TestCaseData(RgbProPhotoXyzE, 1, 0, -498.2).SetName("ProPhoto, E, (1,0)"),
        new TestCaseData(RgbProPhotoXyzE, 1, 1, 578.1).SetName("ProPhoto, E, (1,1)"),

        new TestCaseData(RgbProPhotoXyzA, 0, 0, 476.0).SetName("ProPhoto, A, (0,0)"),
        new TestCaseData(RgbProPhotoXyzA, 0, 1, 528.4).SetName("ProPhoto, A, (0,1)"),
        new TestCaseData(RgbProPhotoXyzA, 1, 0, -508.9).SetName("ProPhoto, A, (1,0)"),
        new TestCaseData(RgbProPhotoXyzA, 1, 1, 580.7).SetName("ProPhoto, A, (1,1)"),

        new TestCaseData(RgbProPhotoXyzD75, 0, 0, 477.2).SetName("ProPhoto, D75, (0,0)"),
        new TestCaseData(RgbProPhotoXyzD75, 0, 1, 519.8).SetName("ProPhoto, D75, (0,1)"),
        new TestCaseData(RgbProPhotoXyzD75, 1, 0, -496.1).SetName("ProPhoto, D75, (1,0)"),
        new TestCaseData(RgbProPhotoXyzD75, 1, 1, 577.2).SetName("ProPhoto, D75, (1,1)")
    ];
    
    [TestCaseSource(nameof(ImaginaryTestData))]
    public void Imaginary(Configuration config, double x, double y, double expectedWavelength)
    {
        var colour = new Unicolour(config, new Chromaticity(x, y));
        Assert.That(colour.DominantWavelength, Is.EqualTo(expectedWavelength).Within(0.25));
    }
    
    [Test]
    public void SampleBetweenWhiteAndNear()
    {
        // white point (0.333, 0.333), sample right & down (0.34, 0.32), near = line of purples, far = green boundary
        // sample between: white point -> near (line of purples) = negative wavelength
        var colour = new Unicolour(RgbStandardXyzE, new Chromaticity(0.34, 0.32));
        Assert.That(colour.DominantWavelength, Is.Negative);
        Assert.That(colour.IsImaginary, Is.False);
    }
    
    [Test]
    public void SampleBetweenWhiteAndFar()
    {
        // white point (0.333, 0.333), sample left & up (0.34, 0.32), near = line of purples, far = green boundary
        // sample between: white point -> far (green boundary) = positive wavelength
        var colour = new Unicolour(RgbStandardXyzE, new Chromaticity(0.32, 0.34));
        Assert.That(colour.DominantWavelength, Is.Positive);
        Assert.That(colour.IsImaginary, Is.False);
    }
    
    [Test]
    public void SampleImaginaryNearNegative()
    {
        // white point (0.333, 0.333), sample right & down (0.5, 0.1), near = line of purples, far = green boundary
        // sample outside boundary: -> near (line of purples) = negative wavelength
        var colour = new Unicolour(RgbStandardXyzE, new Chromaticity(0.5, 0.1));
        Assert.That(colour.DominantWavelength, Is.Negative);
        Assert.That(colour.IsImaginary, Is.True);
    }
    
    [Test]
    public void SampleImaginaryNearPositive()
    {
        // white point (0.333, 0.333), sample left & up (0.0, 0.9), near = green boundary, far = line of purples
        // sample outside boundary: -> near (green boundary) = positive wavelength
        var colour = new Unicolour(RgbStandardXyzE, new Chromaticity(0.0, 0.9));
        Assert.That(colour.DominantWavelength, Is.Positive);
        Assert.That(colour.IsImaginary, Is.True);
    }
}