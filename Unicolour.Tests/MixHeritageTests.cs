namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class MixHeritageTests
{
    private static readonly List<ColourSpace> HuedSpaces = new()
    {
        ColourSpace.Hsb, ColourSpace.Hsl, ColourSpace.Hwb, ColourSpace.Lchab, ColourSpace.Lchuv, 
        ColourSpace.Hsluv, ColourSpace.Hpluv, ColourSpace.Jzczhz, ColourSpace.Oklch, ColourSpace.Hct
    };

    private static readonly List<ColourSpace> NonHuedSpaces = AssertUtils.AllColourSpaces.Except(HuedSpaces).ToList();
    
    [Test] 
    public void HuedToHued()
    {
        // hued + hued --> hued
        var unicolour1 = Unicolour.FromRgb(1, 0, 1);
        var unicolour2 = Unicolour.FromRgb(1, 1, 0);
        AssertInitialHeritage(unicolour1, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        AssertInitialHeritage(unicolour2, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        // initial mixed representation: no heritage, non-NaN, non-greyscale;
        // all representations are used as hued where hue axis is present
        var mixed = unicolour1.MixRgb(unicolour2, 0.5);
        AssertInitialHeritage(mixed, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsHued(HuedSpaces), Has.All.True);
        Assert.That(data.UseAsHued(NonHuedSpaces), Has.All.False);
        
        // representations of mixed colour has same behaviour when mixed via hued space
        mixed = unicolour1.MixHsb(unicolour2, 0.5);
        AssertInitialHeritage(mixed, ColourHeritage.Hued, isHued: true, isGreyscale: false, isNotNumber: false);

        data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsHued(HuedSpaces), Has.All.True);
        Assert.That(data.UseAsHued(NonHuedSpaces), Has.All.False);
    }
    
    [Test]
    public void HuedToGreyscale()
    {
        // hued + hued --> grey
        var unicolour1 = Unicolour.FromRgb(1, 0, 1);
        var unicolour2 = Unicolour.FromRgb(0, 1, 0);
        AssertInitialHeritage(unicolour1, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        AssertInitialHeritage(unicolour2, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        // initial mixed representation: no heritage, greyscale values;
        // all representations are used as greyscale, none are used as hued
        var mixed = unicolour1.MixRgb(unicolour2, 0.5);
        AssertInitialHeritage(mixed, ColourHeritage.None, isHued: false, isGreyscale: true, isNotNumber: false);

        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsGreyscale(AssertUtils.AllColourSpaces), Has.All.True);
        Assert.That(data.UseAsHued(AssertUtils.AllColourSpaces), Has.All.False);
    }
    
    [Test]
    public void GreyscaleToHued()
    {
        // grey + hued --> hued (no other possibility)
        var unicolour1 = Unicolour.FromRgb(1, 1, 1);
        var unicolour2 = Unicolour.FromRgb(0, 1, 0);
        AssertInitialHeritage(unicolour1, ColourHeritage.None, isHued: false, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(unicolour2, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        // initial mixed representation: no heritage, non-greyscale values;
        // all representations are used as hued where hue axis is present
        var mixed = unicolour1.MixRgb(unicolour2, 0.5);
        AssertInitialHeritage(mixed, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);

        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsHued(HuedSpaces), Has.All.True);
        Assert.That(data.UseAsHued(NonHuedSpaces), Has.All.False);
    }
    
    [Test]
    public void GreyscaleToGreyscale()
    {
        // grey + grey --> grey (no other possibility)
        var unicolour1 = Unicolour.FromRgb(1, 1, 1);
        var unicolour2 = Unicolour.FromRgb(0, 0, 0);
        AssertInitialHeritage(unicolour1, ColourHeritage.None, isHued: false, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(unicolour2, ColourHeritage.None, isHued: false, isGreyscale: true, isNotNumber: false);
        
        // initial mixed representation: greyscale heritage, greyscale values;
        // all representations are used as greyscale, none are used as hued
        var mixed = unicolour1.MixLab(unicolour2, 0.5);
        AssertInitialHeritage(mixed, ColourHeritage.Greyscale, isHued: false, isGreyscale: true, isNotNumber: false);

        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsGreyscale(AssertUtils.AllColourSpaces), Has.All.True);
        Assert.That(data.UseAsHued(AssertUtils.AllColourSpaces), Has.All.False);
    }
    
    [Test]
    public void NotNumberToNotNumber()
    {
        // NaN + X --> NaN (no other possibility)
        var unicolour1 = Unicolour.FromRgb(double.NaN, double.NaN, double.NaN);
        var unicolour2 = Unicolour.FromRgb(0, 1, 0);
        AssertInitialHeritage(unicolour1, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: true);
        AssertInitialHeritage(unicolour2, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        // initial mixed representation: NaN heritage, NaN values;
        // all representations are used as NaN, none are used as hued or greyscale
        var mixed = unicolour1.MixRgb(unicolour2, 0.5);
        AssertInitialHeritage(mixed, ColourHeritage.NaN, isHued: false, isGreyscale: false, isNotNumber: true);
        
        var data = new ColourHeritageData(mixed);
        Assert.That(data.UseAsNaN(AssertUtils.AllColourSpaces), Has.All.True);
        Assert.That(data.UseAsGreyscale(AssertUtils.AllColourSpaces), Has.All.False);
        Assert.That(data.UseAsHued(AssertUtils.AllColourSpaces), Has.All.False);
    }
    
    /*
     * this test ensures that, when using a consistent hued space,
     * hue information is retained over multiple mixes even when the colours themselves are greyscale values
     * (e.g. starting with HSB and also mixing HSB, or even mixing an HSB-adjacent hued space such as HSL)
     */
    [Test]
    public void GreyscaleAndHued()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0, 0);
        var unicolour2 = Unicolour.FromHsb(120, 0, 0);
        AssertInitialHeritage(unicolour1, ColourHeritage.None, isHued: true, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(unicolour2, ColourHeritage.None, isHued: true, isGreyscale: true, isNotNumber: false);
        
        var mixed1 = unicolour1.MixHsb(unicolour2, 0.75); // 90, 0, 0
        var mixed2 = unicolour1.MixHsb(unicolour2, 0.25); // 30, 0, 0
        AssertInitialHeritage(mixed1, ColourHeritage.GreyscaleAndHued, isHued: true, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(mixed2, ColourHeritage.GreyscaleAndHued, isHued: true, isGreyscale: true, isNotNumber: false);
        
        var mixed3 = mixed1.MixHsb(mixed2, 0.5); // 60, 0, 0
        var unicolour3 = Unicolour.FromHsb(240, 1, 1);
        AssertInitialHeritage(mixed3, ColourHeritage.GreyscaleAndHued, isHued: true, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(unicolour3, ColourHeritage.None, isHued: true, isGreyscale: false, isNotNumber: false);
        
        var mixed4 = mixed3.MixHsb(unicolour3, 0.5); // 180, 0.5, 0.5
        AssertInitialHeritage(mixed4, ColourHeritage.Hued, isHued: true, isGreyscale: false, isNotNumber: false);
        Assert.That(mixed4.Hsb.H, Is.EqualTo(150));
        
        // HSL is a transform of HSB, hue information should also still be intact
        var mixedHsl = mixed3.MixHsl(unicolour3, 0.5);
        AssertInitialHeritage(mixed4, ColourHeritage.Hued, isHued: true, isGreyscale: false, isNotNumber: false);
        Assert.That(mixedHsl.Hsb.H, Is.EqualTo(150));
    }
    
    private static void AssertInitialHeritage(Unicolour unicolour, ColourHeritage colourHeritage, bool isHued, bool isGreyscale, bool isNotNumber)
    {
        var initialRepresentation = unicolour.InitialRepresentation;
        Assert.That(initialRepresentation.Heritage, Is.EqualTo(colourHeritage));
        Assert.That(initialRepresentation.UseAsHued, Is.EqualTo(isHued));
        Assert.That(initialRepresentation.UseAsGreyscale, Is.EqualTo(isGreyscale));
        Assert.That(initialRepresentation.UseAsNaN, Is.EqualTo(isNotNumber));
    }
}