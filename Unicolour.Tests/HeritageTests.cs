using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class HeritageTests
{
    private static readonly Configuration F11Config = new(xyzConfig: new(Illuminant.F11, Observer.Degree2));
    
    // greyscale heritage is preserved for all downstream spaces
    // important for wavelength processing, Pointer's gamut, and accurate ToString() when hue is powerless
    [Test]
    public void Greyscale()
    {
        var greyD65 = new Unicolour(Configuration.Default, ColourSpace.Luv, 50, 0, 0, alpha: 0.5);
        AssertSource<Luv>(greyD65, ColourHeritage.None);
        AssertHeritage(greyD65.Luv, ColourHeritage.None, isGreyscale: true, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyD65.Lchuv, ColourHeritage.Greyscale, isGreyscale: true, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyD65.Hsluv, ColourHeritage.Greyscale, isGreyscale: true, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyD65.Xyz, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);

        var greyD65Clone = AssertClone(greyD65);
        AssertSource<Luv>(greyD65Clone, ColourHeritage.None);
        AssertHeritage(greyD65Clone.Luv, ColourHeritage.None, isGreyscale: true, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyD65Clone.Lchuv, ColourHeritage.Greyscale, isGreyscale: true, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyD65Clone.Hsluv, ColourHeritage.Greyscale, isGreyscale: true, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyD65Clone.Xyz, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);

        // converting configuration always results in an XYZ colour
        // A & B should not be exactly 0 after conversion to D50, but we still know it represents a greyscale colour
        var greyF11 = greyD65.ConvertToConfiguration(F11Config);
        AssertSource<Xyz>(greyF11, ColourHeritage.Greyscale); 
        AssertHeritage(greyF11.Xyz, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyF11.Luv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyF11.Lchuv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyF11.Hsluv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);

        var greyF11Clone = AssertClone(greyF11);
        AssertSource<Xyz>(greyF11Clone, ColourHeritage.Greyscale); 
        AssertHeritage(greyF11Clone.Xyz, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyF11Clone.Luv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyF11Clone.Lchuv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyF11Clone.Hsluv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
    }
    
    // hued heritage is only preserved as long as downstream spaces are also hued, and lost as soon as hue is irrelevant
    // (all spaces can be greyscale, only spaces with a hue component can be hued)
    // important for interpolation (e.g. one colour's hue should be ignored because it is powerless, and the value is a fallback like 0 in during conversion)
    [Test]
    public void Hued()
    {
        var huedD65 = new Unicolour(Configuration.Default, ColourSpace.Lchuv, 50, 25, 180, alpha: 0.5);
        AssertSource<Lchuv>(huedD65, ColourHeritage.None);
        AssertHeritage(huedD65.Lchuv, ColourHeritage.None, isGreyscale: false, useAsGreyscale: false, useAsHued: true);
        AssertHeritage(huedD65.Hsluv, ColourHeritage.Hued, isGreyscale: false, useAsGreyscale: false, useAsHued: true);
        AssertHeritage(huedD65.Luv, ColourHeritage.Hued, isGreyscale: false, useAsGreyscale: false, useAsHued: false);
        AssertHeritage(huedD65.Xyz, ColourHeritage.None, isGreyscale: false, useAsGreyscale: false, useAsHued: false);

        var huedD65Clone = AssertClone(huedD65);
        AssertSource<Lchuv>(huedD65Clone, ColourHeritage.None);
        AssertHeritage(huedD65Clone.Lchuv, ColourHeritage.None, isGreyscale: false, useAsGreyscale: false, useAsHued: true);
        AssertHeritage(huedD65Clone.Hsluv, ColourHeritage.Hued, isGreyscale: false, useAsGreyscale: false, useAsHued: true);
        AssertHeritage(huedD65Clone.Luv, ColourHeritage.Hued, isGreyscale: false, useAsGreyscale: false, useAsHued: false);
        AssertHeritage(huedD65Clone.Xyz, ColourHeritage.None, isGreyscale: false, useAsGreyscale: false, useAsHued: false);

        // converting configuration always results in an XYZ colour
        var huedF11 = huedD65.ConvertToConfiguration(F11Config);
        AssertSource<Xyz>(huedF11, ColourHeritage.Hued);
        AssertHeritage(huedF11.Xyz, ColourHeritage.Hued, isGreyscale: false, useAsGreyscale: false, useAsHued: false);
        AssertHeritage(huedF11.Luv, ColourHeritage.None, isGreyscale: false, useAsGreyscale: false, useAsHued: false);
        AssertHeritage(huedF11.Lchuv, ColourHeritage.None, isGreyscale: false, useAsGreyscale: false, useAsHued: true);
        AssertHeritage(huedF11.Hsluv, ColourHeritage.Hued, isGreyscale: false, useAsGreyscale: false, useAsHued: true);
        
        var huedF11Clone = AssertClone(huedF11);
        AssertSource<Xyz>(huedF11Clone, ColourHeritage.Hued); 
        AssertHeritage(huedF11Clone.Xyz, ColourHeritage.Hued, isGreyscale: false, useAsGreyscale: false, useAsHued: false);
        AssertHeritage(huedF11Clone.Luv, ColourHeritage.None, isGreyscale: false, useAsGreyscale: false, useAsHued: false);
        AssertHeritage(huedF11Clone.Lchuv, ColourHeritage.None, isGreyscale: false, useAsGreyscale: false, useAsHued: true);
        AssertHeritage(huedF11Clone.Hsluv, ColourHeritage.Hued, isGreyscale: false, useAsGreyscale: false, useAsHued: true);
    }
    
    // a slightly unusual case: a colour can be both greyscale and hued, but only when explicitly defined as such
    // (e.g. a colour defined with a hue value but zero chroma - it is greyscale but the hue should not be ignored)
    // the greyscale aspect is preserved for all downstream spaces
    // the hue aspect is only preserved as long as downstream spaces are also hued
    [Test]
    public void GreyscaleHued()
    {
        var greyHuedD65 = new Unicolour(Configuration.Default, ColourSpace.Lchuv, 50, 0, 180, alpha: 0.5);
        AssertSource<Lchuv>(greyHuedD65, ColourHeritage.None);
        AssertHeritage(greyHuedD65.Lchuv, ColourHeritage.None, isGreyscale: true, useAsGreyscale: true, useAsHued: true);
        AssertHeritage(greyHuedD65.Hsluv, ColourHeritage.GreyscaleAndHued, isGreyscale: true, useAsGreyscale: true, useAsHued: true);
        AssertHeritage(greyHuedD65.Luv, ColourHeritage.GreyscaleAndHued, isGreyscale: true, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyHuedD65.Xyz, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);

        var greyHuedD65Clone = AssertClone(greyHuedD65);
        AssertSource<Lchuv>(greyHuedD65Clone, ColourHeritage.None);
        AssertHeritage(greyHuedD65Clone.Lchuv, ColourHeritage.None, isGreyscale: true, useAsGreyscale: true, useAsHued: true);
        AssertHeritage(greyHuedD65Clone.Hsluv, ColourHeritage.GreyscaleAndHued, isGreyscale: true, useAsGreyscale: true, useAsHued: true);
        AssertHeritage(greyHuedD65Clone.Luv, ColourHeritage.GreyscaleAndHued, isGreyscale: true, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyHuedD65Clone.Xyz, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);

        // converting configuration always results in an XYZ colour
        var greyHuedF11 = greyHuedD65.ConvertToConfiguration(F11Config);
        AssertSource<Xyz>(greyHuedF11, ColourHeritage.GreyscaleAndHued);
        AssertHeritage(greyHuedF11.Xyz, ColourHeritage.GreyscaleAndHued, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyHuedF11.Luv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyHuedF11.Lchuv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyHuedF11.Hsluv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        
        var greyHuedF11Clone = AssertClone(greyHuedF11);
        AssertSource<Xyz>(greyHuedF11Clone, ColourHeritage.GreyscaleAndHued); 
        AssertHeritage(greyHuedF11Clone.Xyz, ColourHeritage.GreyscaleAndHued, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyHuedF11Clone.Luv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyHuedF11Clone.Lchuv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
        AssertHeritage(greyHuedF11Clone.Hsluv, ColourHeritage.Greyscale, isGreyscale: false, useAsGreyscale: true, useAsHued: false);
    }
    
    // NaN heritage is preserved for all downstream spaces
    // important for gracefully handling invalid values without crashing and allowing early exits
    [Test]
    public void NotNumber()
    {
        var nanD65 = new Unicolour(Configuration.Default, ColourSpace.Lchuv, double.NaN, double.NaN, double.NaN, alpha: 0.5);
        AssertSource<Lchuv>(nanD65, ColourHeritage.None);
        AssertNotNumberHeritage(nanD65.Lchuv, ColourHeritage.None);
        AssertNotNumberHeritage(nanD65.Hsluv, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanD65.Luv, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanD65.Xyz, ColourHeritage.NaN);

        var nanD65Clone = AssertClone(nanD65);
        AssertSource<Lchuv>(nanD65Clone, ColourHeritage.None);
        AssertNotNumberHeritage(nanD65Clone.Lchuv, ColourHeritage.None);
        AssertNotNumberHeritage(nanD65Clone.Hsluv, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanD65Clone.Luv, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanD65Clone.Xyz, ColourHeritage.NaN);

        // converting configuration always results in an XYZ colour
        var nanF11 = nanD65.ConvertToConfiguration(F11Config);
        AssertSource<Xyz>(nanF11, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanF11.Xyz, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanF11.Luv, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanF11.Lchuv, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanF11.Hsluv, ColourHeritage.NaN);
        
        var nanF11Clone = AssertClone(nanF11);
        AssertSource<Xyz>(nanF11Clone, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanF11Clone.Xyz, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanF11Clone.Luv, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanF11Clone.Lchuv, ColourHeritage.NaN);
        AssertNotNumberHeritage(nanF11Clone.Hsluv, ColourHeritage.NaN);
    }

    private static void AssertSource<T>(Unicolour unicolour, ColourHeritage heritage)
    {
        Assert.That(unicolour.SourceRepresentation.GetType(), Is.EqualTo(typeof(T)));
        Assert.That(unicolour.SourceRepresentation.Heritage, Is.EqualTo(heritage));
    }

    private static void AssertHeritage(ColourRepresentation representation, ColourHeritage heritage, bool isGreyscale, bool useAsGreyscale, bool useAsHued)
    {
        Assert.That(representation.Heritage, Is.EqualTo(heritage));
        Assert.That(representation.IsGreyscale, Is.EqualTo(isGreyscale));
        Assert.That(representation.UseAsGreyscale, Is.EqualTo(useAsGreyscale));
        Assert.That(representation.UseAsHued, Is.EqualTo(useAsHued));
        Assert.That(representation.UseAsNaN, Is.False);
    }

    private static void AssertNotNumberHeritage(ColourRepresentation representation, ColourHeritage heritage)
    {
        Assert.That(representation.Heritage, Is.EqualTo(heritage));
        Assert.That(representation.UseAsNaN, Is.True);
        Assert.That(representation.UseAsGreyscale, Is.False);
        Assert.That(representation.UseAsHued, Is.False);
    }

    private static Unicolour AssertClone(Unicolour colour)
    {
        var cloned = colour.Clone();
        Assert.That(Equals(cloned, colour), Is.True);
        Assert.That(ReferenceEquals(cloned, colour), Is.False);
        return cloned;
    }
}