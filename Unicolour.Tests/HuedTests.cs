namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class HuedTests
{
    [Test]
    public void Hsb() => AssertUnicolour(Unicolour.FromHsb(180, 0, 0), new List<ColourSpace> { ColourSpace.Hsl, ColourSpace.Hwb });
    
    [Test]
    public void Hsl() => AssertUnicolour(Unicolour.FromHsl(180, 0, 0), new List<ColourSpace> { ColourSpace.Hsb, ColourSpace.Hwb });
    
    [Test]
    public void Hwb() => AssertUnicolour(Unicolour.FromHwb(180, 0, 1), new List<ColourSpace> { ColourSpace.Hsb, ColourSpace.Hsl });
    
    [Test]
    public void Lchab() => AssertUnicolour(Unicolour.FromLchab(0, 0, 180), new List<ColourSpace>());
    
    [Test]
    public void Lchuv() => AssertUnicolour(Unicolour.FromLchuv(0, 0, 180), new List<ColourSpace> { ColourSpace.Hsluv, ColourSpace.Hpluv });
    
    [Test]
    public void Hsluv() => AssertUnicolour(Unicolour.FromHsluv(180, 0, 0), new List<ColourSpace> { ColourSpace.Lchuv, ColourSpace.Hpluv });
    
    [Test]
    public void Hpluv() => AssertUnicolour(Unicolour.FromHpluv(180, 0, 0), new List<ColourSpace> { ColourSpace.Lchuv, ColourSpace.Hsluv });
    
    [Test]
    public void Jzczhz() => AssertUnicolour(Unicolour.FromJzczhz(0, 0, 180), new List<ColourSpace>());
    
    [Test]
    public void Oklch() => AssertUnicolour(Unicolour.FromOklch(0, 0, 180), new List<ColourSpace>());

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
        var otherSpaces = AssertUtils.AllColourSpaces.Except(adjacentHuedSpaces.Concat(new[] { unicolour.InitialColourSpace })).ToList();
        Assert.That(data.Heritages(otherSpaces), Has.One.EqualTo(ColourHeritage.GreyscaleAndHued));
        Assert.That(data.Heritages(otherSpaces), Has.Exactly(otherSpaces.Count - 1).EqualTo(ColourHeritage.Greyscale));
        Assert.That(data.UseAsHued(otherSpaces), Has.All.False);
        Assert.That(data.UseAsGreyscale(otherSpaces), Has.All.True);
    }
}