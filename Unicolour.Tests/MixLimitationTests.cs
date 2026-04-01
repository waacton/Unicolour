using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class MixLimitationTests
{
    [Test] 
    public void NoneWithNoneToNone()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, 1, 0, 1);
        var colour2 = new Unicolour(ColourSpace.Rgb, 1, 1, 0);
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        
        Assert.That(colour1.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour1.SourceRepresentation.Limitation, Is.EqualTo(Limitation.None));
        Assert.That(colour2.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour2.SourceRepresentation.Limitation, Is.EqualTo(Limitation.None));
        Assert.That(mixed.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(mixed.SourceRepresentation.Limitation, Is.EqualTo(Limitation.None));
        Assert.That(TestUtils.Limitations(mixed, TestUtils.AllColourSpaces, baselines: false), Has.All.EqualTo(Limitation.None));
    }
    
    [Test]
    public void NoneWithNoneToAchromatic()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, 1, 0, 1);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        
        Assert.That(colour1.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour1.SourceRepresentation.Limitation, Is.EqualTo(Limitation.None));
        Assert.That(colour2.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour2.SourceRepresentation.Limitation, Is.EqualTo(Limitation.None));
        Assert.That(mixed.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(mixed.SourceRepresentation.Limitation, Is.EqualTo(Limitation.Achromatic));
        Assert.That(TestUtils.Limitations(mixed, TestUtils.AllColourSpaces, baselines: false), Has.All.EqualTo(Limitation.Achromatic));
    }
    
    [Test]
    public void AchromaticWithNoneToNone()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, 1, 1, 1);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        
        Assert.That(colour1.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour1.SourceRepresentation.Limitation, Is.EqualTo(Limitation.Achromatic));
        Assert.That(colour2.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour2.SourceRepresentation.Limitation, Is.EqualTo(Limitation.None));
        Assert.That(mixed.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(mixed.SourceRepresentation.Limitation, Is.EqualTo(Limitation.None));
        Assert.That(TestUtils.Limitations(mixed, TestUtils.AllColourSpaces, baselines: false), Has.All.EqualTo(Limitation.None));
    }
    
    [Test]
    public void AchromaticWithNoneToAchromatic()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, 1, 1, 1);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, 0, premultiplyAlpha: false);
        
        Assert.That(colour1.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour1.SourceRepresentation.Limitation, Is.EqualTo(Limitation.Achromatic));
        Assert.That(colour2.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour2.SourceRepresentation.Limitation, Is.EqualTo(Limitation.None));
        Assert.That(mixed.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(mixed.SourceRepresentation.Limitation, Is.EqualTo(Limitation.Achromatic));
        Assert.That(TestUtils.Limitations(mixed, TestUtils.AllColourSpaces, baselines: false), Has.All.EqualTo(Limitation.Achromatic));
    }
    
    [Test]
    public void AchromaticWithAchromatic()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, 1, 1, 1);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0, 0, 0);
        var mixed = colour1.Mix(colour2, ColourSpace.Lab, premultiplyAlpha: false);
        
        Assert.That(colour1.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour1.SourceRepresentation.Limitation, Is.EqualTo(Limitation.Achromatic));
        Assert.That(colour2.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour2.SourceRepresentation.Limitation, Is.EqualTo(Limitation.Achromatic));
        Assert.That(mixed.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(mixed.SourceRepresentation.Limitation, Is.EqualTo(Limitation.Achromatic));
        Assert.That(TestUtils.Limitations(mixed, TestUtils.AllColourSpaces, baselines: false), Has.All.EqualTo(Limitation.Achromatic));
    }
    
    [Test]
    public void NotNumber()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, double.NaN, double.NaN, double.NaN);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
        var mixed = colour1.Mix(colour2, ColourSpace.Rgb, premultiplyAlpha: false);
        
        Assert.That(colour1.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour1.SourceRepresentation.Limitation, Is.EqualTo(Limitation.NaN));
        Assert.That(colour2.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour2.SourceRepresentation.Limitation, Is.EqualTo(Limitation.None));
        Assert.That(mixed.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(mixed.SourceRepresentation.Limitation, Is.EqualTo(Limitation.NaN));
        Assert.That(TestUtils.Limitations(mixed, TestUtils.AllColourSpaces, baselines: false), Has.All.EqualTo(Limitation.NaN));
    }
    
    /*
     * this test ensures that, when using a consistent hued space,
     * hue information is retained over multiple mixes even when the colours themselves are greyscale values
     * (e.g. starting with HSB and also mixing HSB, or even mixing an HSB-adjacent hued space such as HSL)
     */
    [Test]
    public void NotAchromaticWhenGrey()
    {
        var colour1 = new Unicolour(ColourSpace.Hsb, 0, 0, 0);
        var colour2 = new Unicolour(ColourSpace.Hsb, 120, 0, 0);
        Assert.That(colour1.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour2.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        
        var mixed1 = colour1.Mix(colour2, ColourSpace.Hsb, 0.75, premultiplyAlpha: false);
        var mixed2 = colour1.Mix(colour2, ColourSpace.Hsb, 0.25, premultiplyAlpha: false);
        Assert.That(mixed1.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(mixed2.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        
        var mixed3 = mixed1.Mix(mixed2, ColourSpace.Hsb, premultiplyAlpha: false);
        var colour3 = new Unicolour(ColourSpace.Hsb, 240, 1, 1);
        Assert.That(mixed3.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(colour3.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        
        var mixed4a = mixed3.Mix(colour3, ColourSpace.Hsb, premultiplyAlpha: false);
        Assert.That(mixed4a.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(mixed4a.Hsb.H, Is.EqualTo(150));
        
        var mixed4b = mixed3.Mix(colour3, ColourSpace.Hsl, premultiplyAlpha: false);
        Assert.That(mixed4b.SourceRepresentation.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(mixed4b.Hsb.H, Is.EqualTo(150));
    }
}