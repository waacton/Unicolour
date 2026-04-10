using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class GreyTests
{
    private static readonly Dictionary<ColourSpace, ColourSpace[]> AdjacentHueSpaces = new()
    {
        { ColourSpace.Hsb, [ColourSpace.Hsl, ColourSpace.Hwb] },
        { ColourSpace.Hsl, [ColourSpace.Hsb, ColourSpace.Hwb] },
        { ColourSpace.Hwb, [ColourSpace.Hsb, ColourSpace.Hsl] },
        { ColourSpace.Hsi, [] },
        { ColourSpace.Wxy, [] },
        { ColourSpace.Lchab, [] },
        { ColourSpace.Lchuv, [ColourSpace.Hsluv, ColourSpace.Hpluv] },
        { ColourSpace.Hsluv, [ColourSpace.Lchuv, ColourSpace.Hpluv] },
        { ColourSpace.Hpluv, [ColourSpace.Lchuv, ColourSpace.Hsluv] },
        { ColourSpace.Tsl, [] },
        { ColourSpace.Jzczhz, [] },
        { ColourSpace.Oklch, [] },
        { ColourSpace.Okhsv, [ColourSpace.Okhwb] },
        { ColourSpace.Okhsl, [] },
        { ColourSpace.Okhwb, [ColourSpace.Okhsv] },
        { ColourSpace.Oklrch, [] },
        { ColourSpace.Hct, [] },
        { ColourSpace.Munsell, [] }
    };
    
    [TestCase(ColourSpace.Hsb, 180, 0, 0)]
    [TestCase(ColourSpace.Hsl, 180, 0, 0)]
    [TestCase(ColourSpace.Hwb, 180, 0, 1)]
    [TestCase(ColourSpace.Hsi, 180, 0, 0)]
    [TestCase(ColourSpace.Wxy, 530, 0, 0)]
    [TestCase(ColourSpace.Lchab, 0, 0, 180)]
    [TestCase(ColourSpace.Lchuv, 0, 0, 180)]
    [TestCase(ColourSpace.Hsluv, 180, 0, 0)]
    [TestCase(ColourSpace.Hpluv, 180, 0, 0)]
    [TestCase(ColourSpace.Tsl, 180, 0, 0)]
    [TestCase(ColourSpace.Jzczhz, 0, 0, 180)]
    [TestCase(ColourSpace.Oklch, 0, 0, 180)]
    [TestCase(ColourSpace.Okhsv, 180, 0, 0)]
    [TestCase(ColourSpace.Okhsl, 180, 0, 0)]
    [TestCase(ColourSpace.Okhwb, 180, 0, 1)]
    [TestCase(ColourSpace.Oklrch, 0, 0, 180)]
    [TestCase(ColourSpace.Hct, 180, 0, 0)]
    [TestCase(ColourSpace.Munsell, 180, 0, 0)]
    public void WithHue(ColourSpace colourSpace, double first, double second, double third)
    {
        // grey != achromatic, hue-based colours that are created with 3 values explicitly have chroma information, even though they appear greyscale
        var colour = new Unicolour(colourSpace, first, second, third);
        var adjacentHuedSpaces = AdjacentHueSpaces[colourSpace];
        
        var initial = colour.SourceRepresentation;
        Assert.That(initial.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(TestUtils.Limitations(colour, adjacentHuedSpaces, baselines: true), Has.All.EqualTo(Limitation.None));
        Assert.That(TestUtils.Limitations(colour, adjacentHuedSpaces, baselines: false), Has.All.EqualTo(Limitation.None));
        
        // hued-spaces and adjacent hued-spaces do not have achromatic limitation
        // (e.g. hue 180 chroma 0 has a cyan hue, just the lack of chroma makes it ineffectual) 
        // the first non-hued space to be converted to (e.g. RGB from HSB) will inherit no limitation, but report as achromatic
        // (since the lack of a hue component means grey truly a colour with no known hue)
        // and all subsequent downstream spaces will inherit that achromatic limitation
        var otherSpaces = TestUtils.AllColourSpaces.Except(adjacentHuedSpaces.Concat([colour.SourceColourSpace])).ToArray();
        Assert.That(TestUtils.Limitations(colour, otherSpaces, baselines: true), Has.One.EqualTo(Limitation.None));
        Assert.That(TestUtils.Limitations(colour, otherSpaces, baselines: true), Has.Exactly(otherSpaces.Length - 1).EqualTo(Limitation.Achromatic));
        Assert.That(TestUtils.Limitations(colour, otherSpaces, baselines: false), Has.All.EqualTo(Limitation.Achromatic));
    }

    private static readonly Configuration Config = new(ybrConfig: YbrConfiguration.Jpeg); // allows YCbCr to start from 0 instead of 16, otherwise produces negative XYZ leading to NaN CAM values
    
    [Test]
    public void WithoutHue([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        // when a hue-based colour is created with a single value, it represents grey level and there is no chroma information - at this point it is truly achromatic
        var colour = new Unicolour(Config, colourSpace, 0);
        
        var initial = colour.SourceRepresentation;
        Assert.That(initial.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(TestUtils.Limitations(colour, TestUtils.AllColourSpaces, baselines: true), Has.All.EqualTo(Limitation.Achromatic));
        Assert.That(TestUtils.Limitations(colour, TestUtils.AllColourSpaces, baselines: false), Has.All.EqualTo(Limitation.Achromatic));
    }
    
    private static readonly TestCaseData[] GreyData =
    [
        new TestCaseData(new Rgb255(192), new Rgb255(192, 192, 192)).SetName(nameof(Rgb255)),
        new TestCaseData(new Rgb(0.75), new Rgb(0.75, 0.75, 0.75)).SetName(nameof(Rgb)),
        new TestCaseData(new RgbLinear(0.75), new RgbLinear(0.75, 0.75, 0.75)).SetName(nameof(RgbLinear)),
        new TestCaseData(new Hsb(0.75), new Hsb(0, 0, 0.75)).SetName(nameof(Hsb)),
        new TestCaseData(new Hsl(0.75), new Hsl(0, 0, 0.75)).SetName(nameof(Hsl)),
        new TestCaseData(new Hwb(0.75), new Hwb(0, 0.75, 0.25)).SetName(nameof(Hwb)),
        new TestCaseData(new Hsi(0.75), new Hsi(0, 0, 0.75)).SetName(nameof(Hsi)),
        new TestCaseData(new Xyz(0.75, new WhitePoint(1, 1, 1)), new Xyz(0.75, 0.75, 0.75, new WhitePoint(1, 1, 1))).SetName(nameof(Xyz)),
        new TestCaseData(new Xyy(0.75, new WhitePoint(1, 1, 1)), new Xyy(1 / 3.0, 1 / 3.0, 0.75, new WhitePoint(1, 1, 1))).SetName(nameof(Xyy)),
        new TestCaseData(new Wxy(0.75), new Wxy(360, 0, 0.75)).SetName(nameof(Wxy)),
        new TestCaseData(new Lab(75), new Lab(75, 0, 0)).SetName(nameof(Lab)),
        new TestCaseData(new Lchab(75), new Lchab(75, 0, 0)).SetName(nameof(Lchab)),
        new TestCaseData(new Luv(75), new Luv(75, 0, 0)).SetName(nameof(Luv)),
        new TestCaseData(new Lchuv(75), new Lchuv(75, 0, 0)).SetName(nameof(Lchuv)),
        new TestCaseData(new Hsluv(75), new Hsluv(0, 0, 75)).SetName(nameof(Hsluv)),
        new TestCaseData(new Hpluv(75), new Hpluv(0, 0, 75)).SetName(nameof(Hpluv)),
        new TestCaseData(new Ypbpr(0.75), new Ypbpr(0.75, 0, 0)).SetName(nameof(Ypbpr)),
        new TestCaseData(new Ycbcr(192), new Ycbcr(192, 128, 128)).SetName(nameof(Ycbcr)),
        new TestCaseData(new Ycgco(0.75), new Ycgco(0.75, 0, 0)).SetName(nameof(Ycgco)),
        new TestCaseData(new Yuv(0.75), new Yuv(0.75, 0, 0)).SetName(nameof(Yuv)),
        new TestCaseData(new Yiq(0.75), new Yiq(0.75, 0, 0)).SetName(nameof(Yiq)),
        new TestCaseData(new Ydbdr(0.75), new Ydbdr(0.75, 0, 0)).SetName(nameof(Ydbdr)),
        new TestCaseData(new Tsl(0.75), new Tsl(0, 0, 0.75)).SetName(nameof(Tsl)),
        new TestCaseData(new Xyb(0.75), new Xyb(0, 0.75, 0)).SetName(nameof(Xyb)),
        new TestCaseData(new Lms(0.75), new Lms(0.75, 0.75, 0.75)).SetName(nameof(Lms)),
        new TestCaseData(new Ipt(0.75), new Ipt(0.75, 0, 0)).SetName(nameof(Ipt)),
        new TestCaseData(new Ictcp(0.75), new Ictcp(0.75, 0, 0)).SetName(nameof(Ictcp)),
        new TestCaseData(new Jzazbz(0.75), new Jzazbz(0.75, 0, 0)).SetName(nameof(Jzazbz)),
        new TestCaseData(new Jzczhz(0.75), new Jzczhz(0.75, 0, 0)).SetName(nameof(Jzczhz)),
        new TestCaseData(new Oklab(0.75), new Oklab(0.75, 0, 0)).SetName(nameof(Oklab)),
        new TestCaseData(new Oklch(0.75), new Oklch(0.75, 0, 0)).SetName(nameof(Oklch)),
        new TestCaseData(new Okhsv(0.75), new Okhsv(0, 0, 0.75)).SetName(nameof(Okhsv)),
        new TestCaseData(new Okhsl(0.75), new Okhsl(0, 0, 0.75)).SetName(nameof(Okhsl)),
        new TestCaseData(new Okhwb(0.75), new Okhwb(0, 0.75, 0.25)).SetName(nameof(Okhwb)),
        new TestCaseData(new Oklrab(0.75), new Oklrab(0.75, 0, 0)).SetName(nameof(Oklrab)),
        new TestCaseData(new Oklrch(0.75), new Oklrch(0.75, 0, 0)).SetName(nameof(Oklrch)),
        new TestCaseData(new Cam02(75, CamConfiguration.StandardRgb), new Cam02(75, 0, 0, CamConfiguration.StandardRgb)).SetName(nameof(Cam02)),
        new TestCaseData(new Cam16(75, CamConfiguration.StandardRgb), new Cam16(75, 0, 0, CamConfiguration.StandardRgb)).SetName(nameof(Cam16)),
        new TestCaseData(new Hct(75), new Hct(0, 0, 75)).SetName(nameof(Hct)),
        new TestCaseData(new Munsell(7.5), new Munsell(0, 7.5, 0)).SetName(nameof(Munsell))
    ];

    [TestCaseSource(nameof(GreyData))]
    public void AchromaticConstructor(ColourRepresentation achromatic, ColourRepresentation grey)
    {
        Assert.That(achromatic.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(grey.LimitationBaseline, Is.EqualTo(Limitation.None));
        
        Assert.That(achromatic.Limitation, Is.EqualTo(Limitation.Achromatic));
        Assert.That(grey.Limitation, Is.EqualTo(grey.HasHueComponent ? Limitation.None : Limitation.Achromatic));
        
        Assert.That(achromatic, Is.Not.EqualTo(grey));
        Assert.That(achromatic.Triplet, Is.EqualTo(grey.Triplet));
    }
}