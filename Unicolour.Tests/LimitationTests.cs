using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class LimitationTests
{
    private static readonly Configuration F11Config = new(xyzConfig: new(Illuminant.F11, Observer.Degree2));
    
    [Test]
    public void Greyscale()
    {
        var greyD65 = new Unicolour(Configuration.Default, ColourSpace.Luv, 50, 0, 0, alpha: 0.5);
        AssertSource<Luv>(greyD65, Limitation.None);
        Assert.That(greyD65.Luv.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(greyD65.Lchuv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyD65.Hsluv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyD65.Xyz.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));

        var greyD65Clone = AssertClone(greyD65);
        AssertSource<Luv>(greyD65Clone, Limitation.None);
        Assert.That(greyD65Clone.Luv.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(greyD65Clone.Lchuv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyD65Clone.Hsluv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyD65Clone.Xyz.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));

        // converting configuration always results in an XYZ colour
        // A & B should not be exactly 0 after conversion to D50, but we still know it represents a greyscale colour
        var greyF11 = greyD65.ConvertToConfiguration(F11Config);
        AssertSource<Xyz>(greyF11, Limitation.Achromatic); 
        Assert.That(greyF11.Xyz.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyF11.Luv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyF11.Lchuv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyF11.Hsluv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));

        var greyF11Clone = AssertClone(greyF11);
        AssertSource<Xyz>(greyF11Clone, Limitation.Achromatic); 
        Assert.That(greyF11Clone.Xyz.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyF11Clone.Luv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyF11Clone.Lchuv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
        Assert.That(greyF11Clone.Hsluv.LimitationBaseline, Is.EqualTo(Limitation.Achromatic));
    }
    
    [Test]
    public void NotNumber()
    {
        var nanD65 = new Unicolour(Configuration.Default, ColourSpace.Lchuv, double.NaN, double.NaN, double.NaN, alpha: 0.5);
        AssertSource<Lchuv>(nanD65, Limitation.None);
        Assert.That(nanD65.Lchuv.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(nanD65.Hsluv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanD65.Luv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanD65.Xyz.LimitationBaseline, Is.EqualTo(Limitation.NaN));

        var nanD65Clone = AssertClone(nanD65);
        AssertSource<Lchuv>(nanD65Clone, Limitation.None);
        Assert.That(nanD65Clone.Lchuv.LimitationBaseline, Is.EqualTo(Limitation.None));
        Assert.That(nanD65Clone.Hsluv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanD65Clone.Luv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanD65Clone.Xyz.LimitationBaseline, Is.EqualTo(Limitation.NaN));

        // converting configuration always results in an XYZ colour
        var nanF11 = nanD65.ConvertToConfiguration(F11Config);
        AssertSource<Xyz>(nanF11, Limitation.NaN);
        Assert.That(nanF11.Xyz.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanF11.Luv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanF11.Lchuv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanF11.Hsluv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        
        var nanF11Clone = AssertClone(nanF11);
        AssertSource<Xyz>(nanF11Clone, Limitation.NaN);
        Assert.That(nanF11Clone.Xyz.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanF11Clone.Luv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanF11Clone.Lchuv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
        Assert.That(nanF11Clone.Hsluv.LimitationBaseline, Is.EqualTo(Limitation.NaN));
    }

    private static void AssertSource<T>(Unicolour unicolour, Limitation limitation)
    {
        Assert.That(unicolour.SourceRepresentation.GetType(), Is.EqualTo(typeof(T)));
        Assert.That(unicolour.SourceRepresentation.LimitationBaseline, Is.EqualTo(limitation));
    }

    private static Unicolour AssertClone(Unicolour colour)
    {
        var cloned = colour.Clone();
        Assert.That(Equals(cloned, colour), Is.True);
        Assert.That(ReferenceEquals(cloned, colour), Is.False);
        return cloned;
    }
}