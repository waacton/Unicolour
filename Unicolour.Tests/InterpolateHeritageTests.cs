namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class InterpolateHeritageTests
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
        
        // initial interpolated representation: no heritage, non-NaN, non-greyscale;
        // all representations are used as hued where hue axis is present
        var interpolated = unicolour1.InterpolateRgb(unicolour2, 0.5);
        AssertInitialHeritage(interpolated, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);
        
        var data = new ColourHeritageData(interpolated);
        Assert.That(data.UseAsHued(HuedSpaces), Has.All.True);
        Assert.That(data.UseAsHued(NonHuedSpaces), Has.All.False);
        
        // representations of interpolated colour has same behaviour when interpolated via hued space
        interpolated = unicolour1.InterpolateHsb(unicolour2, 0.5);
        AssertInitialHeritage(interpolated, ColourHeritage.Hued, isHued: true, isGreyscale: false, isNotNumber: false);

        data = new ColourHeritageData(interpolated);
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
        
        // initial interpolated representation: no heritage, greyscale values;
        // all representations are used as greyscale, none are used as hued
        var interpolated = unicolour1.InterpolateRgb(unicolour2, 0.5);
        AssertInitialHeritage(interpolated, ColourHeritage.None, isHued: false, isGreyscale: true, isNotNumber: false);

        var data = new ColourHeritageData(interpolated);
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
        
        // initial interpolated representation: no heritage, non-greyscale values;
        // all representations are used as hued where hue axis is present
        var interpolated = unicolour1.InterpolateRgb(unicolour2, 0.5);
        AssertInitialHeritage(interpolated, ColourHeritage.None, isHued: false, isGreyscale: false, isNotNumber: false);

        var data = new ColourHeritageData(interpolated);
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
        
        // initial interpolated representation: greyscale heritage, greyscale values;
        // all representations are used as greyscale, none are used as hued
        var interpolated = unicolour1.InterpolateLab(unicolour2, 0.5);
        AssertInitialHeritage(interpolated, ColourHeritage.Greyscale, isHued: false, isGreyscale: true, isNotNumber: false);

        var data = new ColourHeritageData(interpolated);
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
        
        // initial interpolated representation: NaN heritage, NaN values;
        // all representations are used as NaN, none are used as hued or greyscale
        var interpolated = unicolour1.InterpolateRgb(unicolour2, 0.5);
        AssertInitialHeritage(interpolated, ColourHeritage.NaN, isHued: false, isGreyscale: false, isNotNumber: true);
        
        var data = new ColourHeritageData(interpolated);
        Assert.That(data.UseAsNaN(AssertUtils.AllColourSpaces), Has.All.True);
        Assert.That(data.UseAsGreyscale(AssertUtils.AllColourSpaces), Has.All.False);
        Assert.That(data.UseAsHued(AssertUtils.AllColourSpaces), Has.All.False);
    }
    
    /*
     * this test ensures that, when using a consistent hued space,
     * hue information is retained over multiple interpolations even when the colours themselves are greyscale values
     * (e.g. starting with HSB and also interpolating HSB, or even interpolating an HSB-adjacent hued space such as HSL)
     */
    [Test]
    public void GreyscaleAndHued()
    {
        var unicolour1 = Unicolour.FromHsb(0, 0, 0);
        var unicolour2 = Unicolour.FromHsb(120, 0, 0);
        AssertInitialHeritage(unicolour1, ColourHeritage.None, isHued: true, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(unicolour2, ColourHeritage.None, isHued: true, isGreyscale: true, isNotNumber: false);
        
        var interpolated1 = unicolour1.InterpolateHsb(unicolour2, 0.75); // 90, 0, 0
        var interpolated2 = unicolour1.InterpolateHsb(unicolour2, 0.25); // 30, 0, 0
        AssertInitialHeritage(interpolated1, ColourHeritage.GreyscaleAndHued, isHued: true, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(interpolated2, ColourHeritage.GreyscaleAndHued, isHued: true, isGreyscale: true, isNotNumber: false);
        
        var interpolated3 = interpolated1.InterpolateHsb(interpolated2, 0.5); // 60, 0, 0
        var unicolour3 = Unicolour.FromHsb(240, 1, 1);
        AssertInitialHeritage(interpolated3, ColourHeritage.GreyscaleAndHued, isHued: true, isGreyscale: true, isNotNumber: false);
        AssertInitialHeritage(unicolour3, ColourHeritage.None, isHued: true, isGreyscale: false, isNotNumber: false);
        
        var interpolated4 = interpolated3.InterpolateHsb(unicolour3, 0.5); // 180, 0.5, 0.5
        AssertInitialHeritage(interpolated4, ColourHeritage.Hued, isHued: true, isGreyscale: false, isNotNumber: false);
        Assert.That(interpolated4.Hsb.H, Is.EqualTo(150));
        
        // HSL is a transform of HSB, hue information should also still be intact
        var interpolatedHsl = interpolated3.InterpolateHsl(unicolour3, 0.5);
        AssertInitialHeritage(interpolated4, ColourHeritage.Hued, isHued: true, isGreyscale: false, isNotNumber: false);
        Assert.That(interpolatedHsl.Hsb.H, Is.EqualTo(150));
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