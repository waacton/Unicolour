using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class GreyTests
{
    private static readonly Dictionary<ColourSpace, (ColourSpace[] chainedHued, ColourSpace upstreamUnhued, ColourSpace? downstreamUnhued)> HuedSpaces = new()
    {
        { ColourSpace.Hsb, (chainedHued: [ColourSpace.Hsl, ColourSpace.Hwb], upstreamUnhued: ColourSpace.Rgb, downstreamUnhued: null) },
        { ColourSpace.Hsl, (chainedHued: [ColourSpace.Hsb, ColourSpace.Hwb], upstreamUnhued: ColourSpace.Rgb, downstreamUnhued: null) },
        { ColourSpace.Hwb, (chainedHued: [ColourSpace.Hsb, ColourSpace.Hsl], upstreamUnhued: ColourSpace.Rgb, downstreamUnhued: null) },
        { ColourSpace.Hsi, (chainedHued: [], upstreamUnhued: ColourSpace.Rgb, downstreamUnhued: null) },
        { ColourSpace.Wxy, (chainedHued: [], upstreamUnhued: ColourSpace.Xyy, downstreamUnhued: null) },
        { ColourSpace.Lchab, (chainedHued: [], upstreamUnhued: ColourSpace.Lab, downstreamUnhued: null) },
        { ColourSpace.Lchuv, (chainedHued: [ColourSpace.Hsluv, ColourSpace.Hpluv], upstreamUnhued: ColourSpace.Luv, downstreamUnhued: null) },
        { ColourSpace.Hsluv, (chainedHued: [ColourSpace.Lchuv, ColourSpace.Hpluv], upstreamUnhued: ColourSpace.Luv, downstreamUnhued: null) },
        { ColourSpace.Hpluv, (chainedHued: [ColourSpace.Lchuv, ColourSpace.Hsluv], upstreamUnhued: ColourSpace.Luv, downstreamUnhued: null) },
        { ColourSpace.Tsl, (chainedHued: [], upstreamUnhued: ColourSpace.Rgb, downstreamUnhued: null) },
        { ColourSpace.Jzczhz, (chainedHued: [], upstreamUnhued: ColourSpace.Jzazbz, downstreamUnhued: null) },
        { ColourSpace.Oklch, (chainedHued: [], upstreamUnhued: ColourSpace.Oklab, downstreamUnhued: null) },
        { ColourSpace.Okhsv, (chainedHued: [ColourSpace.Okhwb], upstreamUnhued: ColourSpace.Oklab, downstreamUnhued: null) },
        { ColourSpace.Okhsl, (chainedHued: [], upstreamUnhued: ColourSpace.Oklab, downstreamUnhued: null) },
        { ColourSpace.Okhwb, (chainedHued: [ColourSpace.Okhsv], upstreamUnhued: ColourSpace.Oklab, downstreamUnhued: null) },
        { ColourSpace.Oklrch, (chainedHued: [], upstreamUnhued: ColourSpace.Oklrab, downstreamUnhued: null) },
        { ColourSpace.Cam02Jch, (chainedHued: [ColourSpace.Cam02Jmh, ColourSpace.Cam02Jsh, ColourSpace.Cam02Qch, ColourSpace.Cam02Qmh, ColourSpace.Cam02Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam02Ucs) },
        { ColourSpace.Cam02Jmh, (chainedHued: [ColourSpace.Cam02Jch, ColourSpace.Cam02Jsh, ColourSpace.Cam02Qch, ColourSpace.Cam02Qmh, ColourSpace.Cam02Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam02Ucs) },
        { ColourSpace.Cam02Jsh, (chainedHued: [ColourSpace.Cam02Jch, ColourSpace.Cam02Jmh, ColourSpace.Cam02Qch, ColourSpace.Cam02Qmh, ColourSpace.Cam02Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam02Ucs) },
        { ColourSpace.Cam02Qch, (chainedHued: [ColourSpace.Cam02Jch, ColourSpace.Cam02Jmh, ColourSpace.Cam02Jsh, ColourSpace.Cam02Qmh, ColourSpace.Cam02Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam02Ucs) },
        { ColourSpace.Cam02Qmh, (chainedHued: [ColourSpace.Cam02Jch, ColourSpace.Cam02Jmh, ColourSpace.Cam02Jsh, ColourSpace.Cam02Qch, ColourSpace.Cam02Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam02Ucs) },
        { ColourSpace.Cam02Qsh, (chainedHued: [ColourSpace.Cam02Jch, ColourSpace.Cam02Jmh, ColourSpace.Cam02Jsh, ColourSpace.Cam02Qch, ColourSpace.Cam02Qmh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam02Ucs) },
        { ColourSpace.Cam16Jch, (chainedHued: [ColourSpace.Cam16Jmh, ColourSpace.Cam16Jsh, ColourSpace.Cam16Qch, ColourSpace.Cam16Qmh, ColourSpace.Cam16Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam16Ucs) },
        { ColourSpace.Cam16Jmh, (chainedHued: [ColourSpace.Cam16Jch, ColourSpace.Cam16Jsh, ColourSpace.Cam16Qch, ColourSpace.Cam16Qmh, ColourSpace.Cam16Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam16Ucs) },
        { ColourSpace.Cam16Jsh, (chainedHued: [ColourSpace.Cam16Jch, ColourSpace.Cam16Jmh, ColourSpace.Cam16Qch, ColourSpace.Cam16Qmh, ColourSpace.Cam16Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam16Ucs) },
        { ColourSpace.Cam16Qch, (chainedHued: [ColourSpace.Cam16Jch, ColourSpace.Cam16Jmh, ColourSpace.Cam16Jsh, ColourSpace.Cam16Qmh, ColourSpace.Cam16Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam16Ucs) },
        { ColourSpace.Cam16Qmh, (chainedHued: [ColourSpace.Cam16Jch, ColourSpace.Cam16Jmh, ColourSpace.Cam16Jsh, ColourSpace.Cam16Qch, ColourSpace.Cam16Qsh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam16Ucs) },
        { ColourSpace.Cam16Qsh, (chainedHued: [ColourSpace.Cam16Jch, ColourSpace.Cam16Jmh, ColourSpace.Cam16Jsh, ColourSpace.Cam16Qch, ColourSpace.Cam16Qmh], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: ColourSpace.Cam16Ucs) },
        { ColourSpace.Hct, (chainedHued: [], upstreamUnhued: ColourSpace.Xyz, downstreamUnhued: null) },
        { ColourSpace.Munsell, (chainedHued: [], upstreamUnhued: ColourSpace.Xyy, downstreamUnhued: null) }
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
    [TestCase(ColourSpace.Cam02Jch, 0, 0, 180)]
    [TestCase(ColourSpace.Cam02Jmh, 0, 0, 180)]
    [TestCase(ColourSpace.Cam02Jsh, 0, 0, 180)]
    [TestCase(ColourSpace.Cam02Qch, 0, 0, 180)]
    [TestCase(ColourSpace.Cam02Qmh, 0, 0, 180)]
    [TestCase(ColourSpace.Cam02Qsh, 0, 0, 180)]
    [TestCase(ColourSpace.Cam16Jch, 0, 0, 180)]
    [TestCase(ColourSpace.Cam16Jmh, 0, 0, 180)]
    [TestCase(ColourSpace.Cam16Jsh, 0, 0, 180)]
    [TestCase(ColourSpace.Cam16Qch, 0, 0, 180)]
    [TestCase(ColourSpace.Cam16Qmh, 0, 0, 180)]
    [TestCase(ColourSpace.Cam16Qsh, 0, 0, 180)]
    [TestCase(ColourSpace.Hct, 180, 0, 0)]
    [TestCase(ColourSpace.Munsell, 180, 0, 0)]
    public void WithHue(ColourSpace colourSpace, double first, double second, double third)
    {
        var (chainedHued, upstreamUnhued, downstreamUnhued) = HuedSpaces[colourSpace];
        ColourSpace[] firstUnhued = downstreamUnhued == null ? [upstreamUnhued] : [upstreamUnhued, downstreamUnhued.Value];
        var other = TestUtils.AllColourSpaces.Except(chainedHued.Concat(firstUnhued).Concat([colourSpace])).ToArray();
        
        // grey != achromatic, hue-based colours that are created with 3 values explicitly have chroma information, even though they appear greyscale
        // (e.g. hue 180 chroma 0 has a cyan hue, just the lack of chroma makes it ineffectual) 
        var colour = new Unicolour(colourSpace, first, second, third);
        Assert.That(colour.SourceTriplet.LimitationBaseline, Is.EqualTo(Limitation.None));
        
        // hued spaces chained to the initial space also do not have achromatic limitation, they have received a hue value, just with no chroma
        Assert.That(TestUtils.Limitations(colour, chainedHued, baselines: true), Has.All.EqualTo(Limitation.None));
        Assert.That(TestUtils.Limitations(colour, chainedHued, baselines: false), Has.All.EqualTo(Limitation.None));
        
        // first upstream and downstream unhued spaces will inherit no limitation, but report as achromatic
        // (e.g. HSB -> RGB, since the lack of a hue component means grey truly a colour with no known hue)
        Assert.That(TestUtils.Limitations(colour, firstUnhued, baselines: true), Has.All.EqualTo(Limitation.None));
        Assert.That(TestUtils.Limitations(colour, firstUnhued, baselines: false), Has.All.EqualTo(Limitation.Achromatic));
        
        // all other spaces will inherit that achromatic limitation
        Assert.That(TestUtils.Limitations(colour, other, baselines: true), Has.All.EqualTo(Limitation.Achromatic));
        Assert.That(TestUtils.Limitations(colour, other, baselines: false), Has.All.EqualTo(Limitation.Achromatic));
    }

    private static readonly Configuration Config = new(ybrConfig: YbrConfiguration.Jpeg); // allows YCbCr to start from 0 instead of 16, otherwise produces negative XYZ leading to NaN CAM values
    
    [Test]
    public void WithoutHue([ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace)
    {
        // when a hue-based colour is created with a single value, it represents grey level and there is no chroma information - at this point it is truly achromatic
        var colour = new Unicolour(Config, colourSpace, 0);
        
        var initial = colour.SourceTriplet;
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
        new TestCaseData(new Cam02Jch(75), new Cam02Jch(75, 0, 0)).SetName(nameof(Cam02Jch)),
        new TestCaseData(new Cam02Jmh(75), new Cam02Jmh(75, 0, 0)).SetName(nameof(Cam02Jmh)),
        new TestCaseData(new Cam02Jsh(75), new Cam02Jsh(75, 0, 0)).SetName(nameof(Cam02Jsh)),
        new TestCaseData(new Cam02Qch(75), new Cam02Qch(75, 0, 0)).SetName(nameof(Cam02Qch)),
        new TestCaseData(new Cam02Qmh(75), new Cam02Qmh(75, 0, 0)).SetName(nameof(Cam02Qmh)),
        new TestCaseData(new Cam02Qsh(75), new Cam02Qsh(75, 0, 0)).SetName(nameof(Cam02Qsh)),
        new TestCaseData(new Cam02Ucs(75), new Cam02Ucs(75, 0, 0)).SetName(nameof(Cam02Ucs)),
        new TestCaseData(new Cam16Jch(75), new Cam16Jch(75, 0, 0)).SetName(nameof(Cam16Jch)),
        new TestCaseData(new Cam16Jmh(75), new Cam16Jmh(75, 0, 0)).SetName(nameof(Cam16Jmh)),
        new TestCaseData(new Cam16Jsh(75), new Cam16Jsh(75, 0, 0)).SetName(nameof(Cam16Jsh)),
        new TestCaseData(new Cam16Qch(75), new Cam16Qch(75, 0, 0)).SetName(nameof(Cam16Qch)),
        new TestCaseData(new Cam16Qmh(75), new Cam16Qmh(75, 0, 0)).SetName(nameof(Cam16Qmh)),
        new TestCaseData(new Cam16Qsh(75), new Cam16Qsh(75, 0, 0)).SetName(nameof(Cam16Qsh)),
        new TestCaseData(new Cam16Ucs(75), new Cam16Ucs(75, 0, 0)).SetName(nameof(Cam16Ucs)),
        new TestCaseData(new Hct(75), new Hct(0, 0, 75)).SetName(nameof(Hct)),
        new TestCaseData(new Munsell(7.5), new Munsell(0, 7.5, 0)).SetName(nameof(Munsell))
    ];

    [TestCaseSource(nameof(GreyData))]
    public void AchromaticConstructor(ColourTriplet achromatic, ColourTriplet grey)
    {
        Assert.That(achromatic.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(grey.LimitationBaseline, Is.EqualTo(Limitation.None));
        
        Assert.That(achromatic.Limitation, Is.EqualTo(Limitation.Achromatic));
        Assert.That(grey.Limitation, Is.EqualTo(grey.HasHueComponent ? Limitation.None : Limitation.Achromatic));
        
        Assert.That(achromatic.Tuple, Is.EqualTo(grey.Tuple));
    }
}