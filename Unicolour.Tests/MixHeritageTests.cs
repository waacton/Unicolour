using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class MixHeritageTests
{
    private static readonly List<ColourSpace> HuedSpaces =
    [
        ColourSpace.Hsb, ColourSpace.Hsl, ColourSpace.Hwb, ColourSpace.Hsi,
        ColourSpace.Wxy,
        ColourSpace.Lchab, ColourSpace.Lchuv, ColourSpace.Hsluv, ColourSpace.Hpluv,
        ColourSpace.Tsl,
        ColourSpace.Jzczhz,
        ColourSpace.Oklch, ColourSpace.Okhsv, ColourSpace.Okhsl, ColourSpace.Okhwb, ColourSpace.Oklrch,
        ColourSpace.Hct
    ];

    private static readonly List<ColourSpace> NonHuedSpaces = TestUtils.AllColourSpaces.Except(HuedSpaces).ToList();
    
    [Test] 
    public void HuedToHued()
    {
        // hued + hued --> hued
        var colour1 = new Unicolour(ColourSpace.Rgb, 1, 0, 1);
        var colour2 = new Unicolour(ColourSpace.Rgb, 1, 1, 0);
        AssertInitialHeritage(colour1, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        AssertInitialHeritage(colour2, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        // initial mixed representation: no heritage, non-NaN, non-greyscale;
        // all representations are used as hued where hue component is present
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        AssertInitialHeritage(mixed, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsHued(HuedSpaces), Has.All.True);
        Assert.That(data.UseAsHued(NonHuedSpaces), Has.All.False);
        
        // representations of mixed colour has same behaviour when mixed via hued space
        mixed = colour1.Mix(colour2, ColourSpace.Hsb, premultiplyAlpha: false);
        AssertInitialHeritage(mixed, ColourHeritage.Hued, isHued: true, isGreyscale: false, isNotNumber: false);

        data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsHued(HuedSpaces), Has.All.True);
        Assert.That(data.UseAsHued(NonHuedSpaces), Has.All.False);
    }
    
    [Test]
    public void HuedToGreyscale()
    {
        // hued + hued --> grey
        var colour1 = new Unicolour(ColourSpace.Rgb, 1, 0, 1);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
        AssertInitialHeritage(colour1, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        AssertInitialHeritage(colour2, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        // initial mixed representation: no heritage, greyscale values;
        // all representations are used as greyscale, none are used as hued
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        AssertInitialHeritage(mixed, ColourHeritage.None, isHued: false, isGreyscale: true, isNotNumber: false);

        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsGreyscale(TestUtils.AllColourSpaces), Has.All.True);
        Assert.That(data.UseAsHued(TestUtils.AllColourSpaces), Has.All.False);
    }
    
    [Test]
    public void GreyscaleToHued()
    {
        // grey + hued --> hued (no other possibility)
        var colour1 = new Unicolour(ColourSpace.Rgb, 1, 1, 1);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
        AssertInitialHeritage(colour1, ColourHeritage.None, isHued: false, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(colour2, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        // initial mixed representation: no heritage, non-greyscale values;
        // all representations are used as hued where hue component is present
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        AssertInitialHeritage(mixed, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);

        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsHued(HuedSpaces), Has.All.True);
        Assert.That(data.UseAsHued(NonHuedSpaces), Has.All.False);
    }
    
    [Test]
    public void GreyscaleToGreyscale()
    {
        // grey + grey --> grey (no other possibility)
        var colour1 = new Unicolour(ColourSpace.Rgb, 1, 1, 1);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0, 0, 0);
        AssertInitialHeritage(colour1, ColourHeritage.None, isHued: false, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(colour2, ColourHeritage.None, isHued: false, isGreyscale: true, isNotNumber: false);
        
        // initial mixed representation: greyscale heritage, greyscale values;
        // all representations are used as greyscale, none are used as hued
        var mixed = colour1.Mix(colour2, ColourSpace.Lab, premultiplyAlpha: false);
        AssertInitialHeritage(mixed, ColourHeritage.Greyscale, isHued: false, isGreyscale: true, isNotNumber: false);

        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsGreyscale(TestUtils.AllColourSpaces), Has.All.True);
        Assert.That(data.UseAsHued(TestUtils.AllColourSpaces), Has.All.False);
    }
    
    [Test]
    public void NotNumberToNotNumber()
    {
        // NaN + X --> NaN (no other possibility)
        var colour1 = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
        AssertInitialHeritage(colour1, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: true);
        AssertInitialHeritage(colour2, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        // initial mixed representation: NaN heritage, NaN values;
        // all representations are used as NaN, none are used as hued or greyscale
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        AssertInitialHeritage(mixed, ColourHeritage.NaN, isHued: false, isGreyscale: false, isNotNumber: true);
        
        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsNaN(TestUtils.AllColourSpaces), Has.All.True);
        Assert.That(data.UseAsGreyscale(TestUtils.AllColourSpaces), Has.All.False);
        Assert.That(data.UseAsHued(TestUtils.AllColourSpaces), Has.All.False);
    }
    
    /*
     * this test ensures that, when using a consistent hued space,
     * hue information is retained over multiple mixes even when the colours themselves are greyscale values
     * (e.g. starting with HSB and also mixing HSB, or even mixing an HSB-adjacent hued space such as HSL)
     */
    [Test]
    public void GreyscaleAndHued()
    {
        var colour1 = new Unicolour(ColourSpace.Hsb, 0, 0, 0);
        var colour2 = new Unicolour(ColourSpace.Hsb, 120, 0, 0);
        AssertInitialHeritage(colour1, ColourHeritage.None, isHued: true, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(colour2, ColourHeritage.None, isHued: true, isGreyscale: true, isNotNumber: false);
        
        var mixed1 = colour1.Mix(colour2, ColourSpace.Hsb, 0.75, premultiplyAlpha: false); // 90, 0, 0
        var mixed2 = colour1.Mix(colour2, ColourSpace.Hsb, 0.25, premultiplyAlpha: false); // 30, 0, 0
        AssertInitialHeritage(mixed1, ColourHeritage.GreyscaleAndHued, isHued: true, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(mixed2, ColourHeritage.GreyscaleAndHued, isHued: true, isGreyscale: true, isNotNumber: false);
        
        var mixed3 = mixed1.Mix(mixed2, ColourSpace.Hsb, premultiplyAlpha: false); // 60, 0, 0
        var colour3 = new Unicolour(ColourSpace.Hsb, 240, 1, 1);
        AssertInitialHeritage(mixed3, ColourHeritage.GreyscaleAndHued, isHued: true, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(colour3, ColourHeritage.None, isHued: true, isGreyscale: false, isNotNumber: false);
        
        var mixed4 = mixed3.Mix(colour3, ColourSpace.Hsb, premultiplyAlpha: false); // 180, 0.5, 0.5
        AssertInitialHeritage(mixed4, ColourHeritage.Hued, isHued: true, isGreyscale: false, isNotNumber: false);
        Assert.That(mixed4.Hsb.H, Is.EqualTo(150));
        
        // HSL is a transform of HSB, hue information should also still be intact
        var mixedHsl = mixed3.Mix(colour3, ColourSpace.Hsl, premultiplyAlpha: false);
        AssertInitialHeritage(mixed4, ColourHeritage.Hued, isHued: true, isGreyscale: false, isNotNumber: false);
        Assert.That(mixedHsl.Hsb.H, Is.EqualTo(150));
    }
    
    private static void AssertInitialHeritage(Unicolour colour, ColourHeritage colourHeritage, bool isHued, bool isGreyscale, bool isNotNumber)
    {
        var sourceRepresentation = colour.SourceRepresentation;
        Assert.That(sourceRepresentation.Heritage, Is.EqualTo(colourHeritage));
        Assert.That(sourceRepresentation.UseAsHued, Is.EqualTo(isHued));
        Assert.That(sourceRepresentation.UseAsGreyscale, Is.EqualTo(isGreyscale));
        Assert.That(sourceRepresentation.UseAsNaN, Is.EqualTo(isNotNumber));
    }
}