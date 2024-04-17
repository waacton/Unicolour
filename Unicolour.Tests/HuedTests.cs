namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class HuedTests
{
    [Test]
    public void Hsb() => AssertUnicolour(new(ColourSpace.Hsb, 180, 0, 0), new List<ColourSpace> { ColourSpace.Hsl, ColourSpace.Hwb });
    
    [Test]
    public void Hsl() => AssertUnicolour(new(ColourSpace.Hsl, 180, 0, 0), new List<ColourSpace> { ColourSpace.Hsb, ColourSpace.Hwb });
    
    [Test]
    public void Hwb() => AssertUnicolour(new(ColourSpace.Hwb, 180, 0, 1), new List<ColourSpace> { ColourSpace.Hsb, ColourSpace.Hsl });
    
    [Test]
    public void Lchab() => AssertUnicolour(new(ColourSpace.Lchab, 0, 0, 180), new List<ColourSpace>());
    
    [Test]
    public void Lchuv() => AssertUnicolour(new(ColourSpace.Lchuv, 0, 0, 180), new List<ColourSpace> { ColourSpace.Hsluv, ColourSpace.Hpluv });
    
    [Test]
    public void Hsluv() => AssertUnicolour(new(ColourSpace.Hsluv, 180, 0, 0), new List<ColourSpace> { ColourSpace.Lchuv, ColourSpace.Hpluv });
    
    [Test]
    public void Hpluv() => AssertUnicolour(new(ColourSpace.Hpluv, 180, 0, 0), new List<ColourSpace> { ColourSpace.Lchuv, ColourSpace.Hsluv });
    
    [Test]
    public void Jzczhz() => AssertUnicolour(new(ColourSpace.Jzczhz, 0, 0, 180), new List<ColourSpace>());
    
    [Test]
    public void Oklch() => AssertUnicolour(new(ColourSpace.Oklch, 0, 0, 180), new List<ColourSpace>());
    
    [Test]
    public void Okhsv() => AssertUnicolour(new(ColourSpace.Okhsv, 180, 0, 0), new List<ColourSpace> { ColourSpace.Okhwb });
    
    [Test]
    public void Okhsl() => AssertUnicolour(new(ColourSpace.Okhsl, 180, 0, 0), new List<ColourSpace>());
    
    [Test]
    public void Okhwb() => AssertUnicolour(new(ColourSpace.Okhwb, 180, 0, 1), new List<ColourSpace> { ColourSpace.Okhsv });
    
    [Test]
    public void Hct() => AssertUnicolour(new(ColourSpace.Hct, 180, 0, 0), new List<ColourSpace>());

    private static void AssertUnicolour(Unicolour unicolour, List<ColourSpace> adjacentHuedSpaces)
    {
        var data = new ColourHeritageData(unicolour);
        var initial = unicolour.InitialRepresentation;
        Assert.That(initial.Heritage, Is.EqualTo(ColourHeritage.None));
        Assert.That(initial.UseAsHued, Is.True);
        Assert.That(initial.UseAsGreyscale, Is.True);
        Assert.That(data.Heritages(adjacentHuedSpaces), Has.All.EqualTo(ColourHeritage.GreyscaleAndHued));
        Assert.That(data.UseAsHued(adjacentHuedSpaces), Has.All.True);
        Assert.That(data.UseAsGreyscale(adjacentHuedSpaces), Has.All.True);

        // the first non-hued space to be converted to (e.g. RGB from HSB) will have hued heritage (since from HSB)
        var otherSpaces = TestUtils.AllColourSpaces.Except(adjacentHuedSpaces.Concat(new[] { unicolour.InitialColourSpace })).ToList();
        Assert.That(data.Heritages(otherSpaces), Has.One.EqualTo(ColourHeritage.GreyscaleAndHued));
        Assert.That(data.Heritages(otherSpaces), Has.Exactly(otherSpaces.Count - 1).EqualTo(ColourHeritage.Greyscale));
        Assert.That(data.UseAsHued(otherSpaces), Has.All.False);
        Assert.That(data.UseAsGreyscale(otherSpaces), Has.All.True);
    }
}