namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class GreyscaleTests
{
    private static readonly List<ColourSpace> NaNProducingSpaces = new()
        { ColourSpace.Ictcp, ColourSpace.Jzazbz, ColourSpace.Jzczhz, ColourSpace.Cam02, ColourSpace.Cam16, ColourSpace.Hct };

    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(-0.00000000001, 0.0, -0.0, true)]
    [TestCase(-0.0, -0.00000000001, 0.0, true)]
    [TestCase(0.0, -0.0, -0.00000000001, true)]
    [TestCase(0.00000000001, 0.0, 0.0, false)]
    [TestCase(0.0, 0.00000000001, 0.0, false)]
    [TestCase(0.0, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.5, 0.5, true)]
    [TestCase(0.50000000001, 0.5, 0.5, false)]
    [TestCase(0.5, 0.50000000001, 0.5, false)]
    [TestCase(0.5, 0.5, 0.50000000001, false)]
    [TestCase(0.49999999999, 0.5, 0.5, false)]
    [TestCase(0.5, 0.49999999999, 0.5, false)]
    [TestCase(0.5, 0.5, 0.49999999999, false)]
    [TestCase(1.0, 1.0, 1.0, true)]
    [TestCase(1.00000000001, 1.0, 1.0, true)]
    [TestCase(1.0, 1.00000000001, 1.0, true)]
    [TestCase(1.0, 1.0, 1.00000000001, true)]
    [TestCase(0.99999999999, 1.0, 1.0, false)]
    [TestCase(1.0, 0.99999999999, 1.0, false)]
    [TestCase(1.0, 1.0, 0.99999999999, false)]
    public void Rgb(double r, double g, double b, bool expected) => AssertUnicolour(Unicolour.FromRgb(r, g, b), expected);

    [TestCase(180.0, 0.0, 0.5, true)]
    [TestCase(180.0, -0.00000000001, 0.5, true)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, true)]
    [TestCase(180.0, 0.5, -0.00000000001, true)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    public void Hsb(double h, double s, double b, bool expected) => AssertUnicolour(Unicolour.FromHsb(h, s, b), expected);

    [TestCase(180.0, 0.0, 0.5, true)]
    [TestCase(180.0, -0.00000000001, 0.5, true)]
    [TestCase(180.0, 0.00000000001, 0.5, false)]
    [TestCase(180.0, 0.5, 0.0, true)]
    [TestCase(180.0, 0.5, -0.00000000001, true)]
    [TestCase(180.0, 0.5, 0.00000000001, false)]
    [TestCase(180.0, 0.5, 1.0, true)]
    [TestCase(180.0, 0.5, 1.00000000001, true)]
    [TestCase(180.0, 0.5, 0.99999999999, false)]
    public void Hsl(double h, double s, double l, bool expected) => AssertUnicolour(Unicolour.FromHsl(h, s, l), expected);
    
    [TestCase(180.0, 1.0, 0.0, true)]
    [TestCase(180.0, 1.00000000001, 0.0, true)]
    [TestCase(180.0, 0.99999999999, 0.0, false)]
    [TestCase(180.0, 0.0, 1.0, true)]
    [TestCase(180.0, 0.0, 1.00000000001, true)]
    [TestCase(180.0, 0.0, 0.99999999999, false)]
    [TestCase(180.0, 0.5, 0.5, true)]
    [TestCase(180.0, 0.50000000001, 0.5, true)]
    [TestCase(180.0, 0.49999999999, 0.5, false)]
    [TestCase(180.0, 0.5, 0.5, true)]
    [TestCase(180.0, 0.5, 0.50000000001, true)]
    [TestCase(180.0, 0.5, 0.49999999999, false)]
    public void Hwb(double h, double w, double b, bool expected) => AssertUnicolour(Unicolour.FromHwb(h, w, b), expected);

    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(0.0, -0.00000000001, 0.0, true)]
    [TestCase(0.00000000001, 0.0, 0.00000000001, true)]
    [TestCase(0.0, 0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.5, 0.5, false)]
    [TestCase(0.25, 0.5, 0.75, false)]
    [TestCase(0.95047, 1.0, 1.08883, false)]
    public void Xyz(double x, double y, double z, bool expected) => AssertUnicolour(Unicolour.FromXyz(x, y, z), expected);
    
    [TestCase(0.0, 0.0, 0.0, true)]
    [TestCase(0.0, 0.0, -0.00000000001, true)]
    [TestCase(0.0, 0.0, 0.00000000001, false)]
    [TestCase(1.0, 1.0, 0.0, true)]
    [TestCase(1.0, 1.0, -0.00000000001, true)]
    [TestCase(1.0, 1.0, 0.00000000001, false)]
    public void Xyy(double x, double y, double upperY, bool expected) => AssertUnicolour(Unicolour.FromXyy(x, y, upperY), expected);

    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.00000000001, -0.00000000001, true)]
    [TestCase(-0.00000000001, 50, -50, true)]
    [TestCase(0.00000000001, 50, -50, false)]
    [TestCase(100.0, 50, -50, true)]
    [TestCase(100.00000000001, 50, -50, true)]
    [TestCase(99.99999999999, 50, -50, false)]
    public void Lab(double l, double a, double b, bool expected) => AssertUnicolour(Unicolour.FromLab(l, a, b), expected);

    [TestCase(50.0, 0.0, 180.0, true)]
    [TestCase(50.0, -0.00000000001, 180.0, true)]
    [TestCase(50.0, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 50.0, 180.0, true)]
    [TestCase(-0.00000000001, 50.0, 180.0, true)]
    [TestCase(0.00000000001, 50.0, 180.0, false)]
    [TestCase(100.0, 50.0, 180.0, true)]
    [TestCase(100.00000000001, 50.0, 180.0, true)]
    [TestCase(99.99999999999, 50.0, 180.0, false)]
    public void Lchab(double l, double c, double h, bool expected) => AssertUnicolour(Unicolour.FromLchab(l, c, h), expected);

    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.00000000001, -0.00000000001, true)]
    [TestCase(-0.00000000001, 50, -50, true)]
    [TestCase(0.00000000001, 50, -50, false)]
    [TestCase(100.0, 50, -50, true)]
    [TestCase(100.00000000001, 50, -50, true)]
    [TestCase(99.99999999999, 50, -50, false)]
    public void Luv(double l, double u, double v, bool expected) => AssertUnicolour(Unicolour.FromLuv(l, u, v), expected);

    [TestCase(50.0, 0.0, 180.0, true)]
    [TestCase(50.0, -0.00000000001, 180.0, true)]
    [TestCase(50.0, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 50.0, 180.0, true)]
    [TestCase(-0.00000000001, 50.0, 180.0, true)]
    [TestCase(0.00000000001, 50.0, 180.0, false)]
    [TestCase(100.0, 50.0, 180.0, true)]
    [TestCase(100.00000000001, 50.0, 180.0, true)]
    [TestCase(99.99999999999, 50.0, 180.0, false)]
    public void Lchuv(double l, double c, double h, bool expected) => AssertUnicolour(Unicolour.FromLchuv(l, c, h), expected);

    [TestCase(180.0, 0.0, 50, true)]
    [TestCase(180.0, -0.00000000001, 50, true)]
    [TestCase(180.0, 0.00000000001, 50, false)]
    [TestCase(180.0, 50, 0.0, true)]
    [TestCase(180.0, 50, -0.00000000001, true)]
    [TestCase(180.0, 50, 0.00000000001, false)]
    [TestCase(180.0, 50, 100.0, true)]
    [TestCase(180.0, 50, 100.00000000001, true)]
    [TestCase(180.0, 50, 0.99999999999, false)]
    public void Hsluv(double h, double s, double l, bool expected) => AssertUnicolour(Unicolour.FromHsluv(h, s, l), expected);

    [TestCase(180.0, 0.0, 50, true)]
    [TestCase(180.0, -0.00000000001, 50, true)]
    [TestCase(180.0, 0.00000000001, 50, false)]
    [TestCase(180.0, 50, 0.0, true)]
    [TestCase(180.0, 50, -0.00000000001, true)]
    [TestCase(180.0, 50, 0.00000000001, false)]
    [TestCase(180.0, 50, 100.0, true)]
    [TestCase(180.0, 50, 100.00000000001, true)]
    [TestCase(180.0, 50, 0.99999999999, false)]
    public void Hpluv(double h, double s, double l, bool expected) => AssertUnicolour(Unicolour.FromHpluv(h, s, l), expected);

    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, true)]
    [TestCase(-0.00000000001, 0.1, -0.1, true)]
    [TestCase(0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    public void Ictcp(double i, double ct, double cp, bool expected) => AssertUnicolour(Unicolour.FromIctcp(i, ct, cp), expected);

    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.1, -0.1, true)]
    [TestCase(-0.00000000001, 0.1, -0.1, true)]
    [TestCase(0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, false)]
    public void Jzazbz(double jz, double az, double bz, bool expected) => AssertUnicolour(Unicolour.FromJzazbz(jz, az, bz), expected);

    [TestCase(0.5, 0.0, 180.0, true)]
    [TestCase(0.5, -0.00000000001, 180.0, true)]
    [TestCase(0.5, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 0.1, 180.0, true)]
    [TestCase(-0.00000000001, 0.1, 180.0, true)]
    [TestCase(0.00000000001, 0.1, 180.0, false)]
    [TestCase(1.0, 0.1, 180.0, false)]
    public void Jzczhz(double jz, double cz, double hz, bool expected) => AssertUnicolour(Unicolour.FromJzczhz(jz, cz, hz), expected);

    [TestCase(0.5, 0.0, 0.0, true)]
    [TestCase(0.5, 0.00000000001, 0.0, false)]
    [TestCase(0.5, -0.00000000001, 0.0, false)]
    [TestCase(0.5, 0.0, 0.00000000001, false)]
    [TestCase(0.5, 0.0, -0.00000000001, false)]
    [TestCase(0.0, 0.00000000001, -0.00000000001, true)]
    [TestCase(-0.00000000001, 0.1, -0.1, true)]
    [TestCase(0.00000000001, 0.1, -0.1, false)]
    [TestCase(1.0, 0.1, -0.1, true)]
    [TestCase(1.00000000001, 0.1, -0.1, true)]
    [TestCase(0.99999999999, 0.1, -0.1, false)]
    public void Oklab(double l, double a, double b, bool expected) => AssertUnicolour(Unicolour.FromOklab(l, a, b), expected);

    [TestCase(0.5, 0.0, 180.0, true)]
    [TestCase(0.5, -0.00000000001, 180.0, true)]
    [TestCase(0.5, 0.00000000001, 180.0, false)]
    [TestCase(0.0, 0.25, 180.0, true)]
    [TestCase(-0.00000000001, 0.25, 180.0, true)]
    [TestCase(0.00000000001, 0.25, 180.0, false)]
    [TestCase(1.0, 0.25, 180.0, true)]
    [TestCase(1.00000000001, 0.25, 180.0, true)]
    [TestCase(0.99999999999, 0.25, 180.0, false)]
    public void Oklch(double l, double c, double h, bool expected) => AssertUnicolour(Unicolour.FromOklch(l, c, h), expected);
    
    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    public void Cam02(double j, double a, double b, bool expected) => AssertUnicolour(Unicolour.FromCam02(j, a, b), expected);
    
    [TestCase(50.0, 0.0, 0.0, true)]
    [TestCase(50.0, 0.00000000001, 0.0, false)]
    [TestCase(50.0, -0.00000000001, 0.0, false)]
    [TestCase(50.0, 0.0, 0.00000000001, false)]
    [TestCase(50.0, 0.0, -0.00000000001, false)]
    public void Cam16(double j, double a, double b, bool expected) => AssertUnicolour(Unicolour.FromCam16(j, a, b), expected);
    
    [TestCase(180.0, 0.0, 50, true)]
    [TestCase(180.0, -0.00000000001, 50, true)]
    [TestCase(180.0, 0.00000000001, 50, false)]
    [TestCase(180.0, 50, 0.0, true)]
    [TestCase(180.0, 50, -0.00000000001, true)]
    [TestCase(180.0, 50, 0.00000000001, false)]
    [TestCase(180.0, 50, 100.0, true)]
    [TestCase(180.0, 50, 100.00000000001, true)]
    [TestCase(180.0, 50, 99.99999999999, false)]
    public void Hct(double h, double c, double t, bool expected) => AssertUnicolour(Unicolour.FromHct(h, c, t), expected);

    private static void AssertUnicolour(Unicolour unicolour, bool shouldBeGreyscale)
    {
        var data = new ColourHeritageData(unicolour);
        var initialRepresentation = unicolour.InitialRepresentation;
        var initialColourSpace = unicolour.InitialColourSpace;
        AssertInitialRepresentation(initialRepresentation, shouldBeGreyscale);

        if (!initialRepresentation.IsGreyscale)
        {
            // if initial representation is non-greyscale
            // some downstream representations should have no greyscale or NaN heritage
            // though it's still possible for them to actually result in greyscale or NaN, especially with outlier values e.g. negatives
            var spaces = AssertUtils.AllColourSpaces.Except(new[] { initialColourSpace }).ToList();
            Assert.That(data.Heritages(spaces), Has.Some.EqualTo(ColourHeritage.None).Or.EqualTo(ColourHeritage.Hued));
        }
        else
        {
            var initialSpaceCanProduceNaN = NaNProducingSpaces.Contains(initialColourSpace);
            if (initialSpaceCanProduceNaN)
            {
                AssertDownstreamFromInitialMaybeNaN(initialColourSpace, data);
            }
            else
            {
                AssertDownstreamNotNaN(initialColourSpace, data);
                AssertDownstreamMaybeNaN(data);
            }
        }
    }
    
    private static void AssertInitialRepresentation(ColourRepresentation initial, bool shouldBeGreyscale)
    {
        Assert.That(initial.Heritage, Is.EqualTo(ColourHeritage.None));
        Assert.That(initial.IsGreyscale, Is.EqualTo(shouldBeGreyscale));
        Assert.That(initial.UseAsGreyscale, Is.EqualTo(shouldBeGreyscale));
        Assert.That(initial.UseAsHued, Is.EqualTo(initial.HasHueAxis));
        Assert.That(initial.UseAsNaN, Is.False);
    }

    private static void AssertDownstreamNotNaN(ColourSpace initialColourSpace, ColourHeritageData data)
    {
        var excludedSpaces = NaNProducingSpaces.Concat(new[] { initialColourSpace });
        var spaces = AssertUtils.AllColourSpaces.Except(excludedSpaces).ToList();
        
        // if initial representation is greyscale, downstream non-NaN-producing representations should all be greyscale too
        // (adjacent hue spaces will also have hue, e.g. HSB -> HSL will carry hue even when greyscale)
        Assert.That(data.Heritages(spaces), Has.All.EqualTo(ColourHeritage.Greyscale).Or.EqualTo(ColourHeritage.GreyscaleAndHued));
        Assert.That(data.UseAsGreyscale(spaces), Has.All.True);
        Assert.That(data.UseAsHued(spaces), Has.Some.False);
        Assert.That(data.UseAsNaN(spaces), Has.All.False);
    }

    private static void AssertDownstreamMaybeNaN(ColourHeritageData data)
    {
        var spaces = NaNProducingSpaces;
        
        // if initial representation is greyscale, downstream NaN-producing representations should all be either greyscale or NaN
        var greyscaleOrNan = data.UseAsGreyscale(spaces).Zip(data.UseAsNaN(spaces), (a, b) => a || b).ToList();
        Assert.That(data.Heritages(spaces), Has.All.EqualTo(ColourHeritage.Greyscale).Or.EqualTo(ColourHeritage.NaN));
        Assert.That(greyscaleOrNan, Has.All.True);
        Assert.That(data.UseAsHued(spaces), Has.All.False);
    }

    private static void AssertDownstreamFromInitialMaybeNaN(ColourSpace initialColourSpace, ColourHeritageData data)
    {
        var excludedSpaces = NaNProducingSpaces.Concat(new[] { initialColourSpace });
        var spaces = AssertUtils.AllColourSpaces.Except(excludedSpaces).ToList();

        // if initial representation is greyscale and NaN-producing, downstream representations should all be either greyscale or NaN
        var greyscaleOrNan = data.UseAsGreyscale(spaces).Zip(data.UseAsNaN(spaces), (a, b) => a || b).ToList();
        Assert.That(data.Heritages(spaces), Has.None.EqualTo(ColourHeritage.None));
        Assert.That(greyscaleOrNan, Has.All.True);
        Assert.That(data.UseAsHued(spaces), Has.All.False);
    }
}