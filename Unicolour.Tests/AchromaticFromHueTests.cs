using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class AchromaticFromHueTests
{
    [Test]
    public void Hsb() => AssertUnicolour(new(ColourSpace.Hsb, 180, 0, 0), [ColourSpace.Hsl, ColourSpace.Hwb]);
    
    [Test]
    public void Hsl() => AssertUnicolour(new(ColourSpace.Hsl, 180, 0, 0), [ColourSpace.Hsb, ColourSpace.Hwb]);
    
    [Test]
    public void Hwb() => AssertUnicolour(new(ColourSpace.Hwb, 180, 0, 1), [ColourSpace.Hsb, ColourSpace.Hsl]);
    
    [Test]
    public void Hsi() => AssertUnicolour(new(ColourSpace.Hsi, 180, 0, 0), []);
    
    [Test]
    public void Wxy() => AssertUnicolour(new(ColourSpace.Wxy, 530, 0, 0), []);
    
    [Test]
    public void Lchab() => AssertUnicolour(new(ColourSpace.Lchab, 0, 0, 180), []);
    
    [Test]
    public void Lchuv() => AssertUnicolour(new(ColourSpace.Lchuv, 0, 0, 180), [ColourSpace.Hsluv, ColourSpace.Hpluv]);
    
    [Test]
    public void Hsluv() => AssertUnicolour(new(ColourSpace.Hsluv, 180, 0, 0), [ColourSpace.Lchuv, ColourSpace.Hpluv]);
    
    [Test]
    public void Hpluv() => AssertUnicolour(new(ColourSpace.Hpluv, 180, 0, 0), [ColourSpace.Lchuv, ColourSpace.Hsluv]);
    
    [Test]
    public void Tsl() => AssertUnicolour(new(ColourSpace.Tsl, 180, 0, 0), []);
    
    [Test]
    public void Jzczhz() => AssertUnicolour(new(ColourSpace.Jzczhz, 0, 0, 180), []);
    
    [Test]
    public void Oklch() => AssertUnicolour(new(ColourSpace.Oklch, 0, 0, 180), []);
    
    [Test]
    public void Okhsv() => AssertUnicolour(new(ColourSpace.Okhsv, 180, 0, 0), [ColourSpace.Okhwb]);
    
    [Test]
    public void Okhsl() => AssertUnicolour(new(ColourSpace.Okhsl, 180, 0, 0), []);
    
    [Test]
    public void Okhwb() => AssertUnicolour(new(ColourSpace.Okhwb, 180, 0, 1), [ColourSpace.Okhsv]);
    
    [Test]
    public void Oklrch() => AssertUnicolour(new(ColourSpace.Oklrch, 0, 0, 180), []);
    
    [Test]
    public void Hct() => AssertUnicolour(new(ColourSpace.Hct, 180, 0, 0), []);
    
    [Test]
    public void Munsell() => AssertUnicolour(new(ColourSpace.Munsell, 180, 0, 0), []);

    private static void AssertUnicolour(Unicolour colour, ColourSpace[] adjacentHuedSpaces)
    {
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
}