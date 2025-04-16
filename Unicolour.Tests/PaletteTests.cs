using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class PaletteTests
{
    private static readonly HueSpan[] HueSpans = [HueSpan.Shorter, HueSpan.Longer, HueSpan.Increasing, HueSpan.Decreasing];
    
    [Test, Combinatorial]
    public void PaletteMultiple(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(nameof(HueSpans))] HueSpan hueSpan,
        [Values(true, false)] bool premultiplyAlpha)
    {
        var colour1 = RandomColours.UnicolourFrom(colourSpace);
        var colour2 = RandomColours.UnicolourFrom(colourSpace);
        
        const int count = 9;
        var palette = colour1.Palette(colour2, colourSpace, count, hueSpan, premultiplyAlpha).ToArray();
        Assert.That(palette.Length, Is.EqualTo(count));

        double[] distances = [0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1];
        for (var i = 0; i < count; i++)
        {
            var mixed = colour1.Mix(colour2, colourSpace, distances[i], hueSpan, premultiplyAlpha);
            Assert.That(palette[i], Is.EqualTo(mixed));
        }
    }

    [Test, Combinatorial]
    public void PaletteTwo(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(nameof(HueSpans))] HueSpan hueSpan,
        [Values(true, false)] bool premultiplyAlpha)
    {
        var colour1 = RandomColours.UnicolourFrom(colourSpace);
        var colour2 = RandomColours.UnicolourFrom(colourSpace);
        var palette = colour1.Palette(colour2, colourSpace, count: 2, hueSpan, premultiplyAlpha).ToArray();
        Assert.That(palette.Length, Is.EqualTo(2));
        Assert.That(palette[0], Is.EqualTo(colour1.Mix(colour2, colourSpace, 0, hueSpan, premultiplyAlpha)));
        Assert.That(palette[1], Is.EqualTo(colour1.Mix(colour2, colourSpace, 1, hueSpan, premultiplyAlpha)));
    }
    
    [Test, Combinatorial]
    public void PaletteOne(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(nameof(HueSpans))] HueSpan hueSpan,
        [Values(true, false)] bool premultiplyAlpha)
    {
        var colour1 = RandomColours.UnicolourFrom(colourSpace);
        var colour2 = RandomColours.UnicolourFrom(colourSpace);
        var palette = colour1.Palette(colour2, colourSpace, count: 1, hueSpan, premultiplyAlpha).ToArray();
        Assert.That(palette.Length, Is.EqualTo(1));
        Assert.That(palette[0], Is.EqualTo(colour1.Mix(colour2, colourSpace, 0.5, hueSpan, premultiplyAlpha)));
    }
    
    [Test, Combinatorial]
    public void PaletteZero(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(nameof(HueSpans))] HueSpan hueSpan,
        [Values(true, false)] bool premultiplyAlpha)
    {
        var colour1 = RandomColours.UnicolourFrom(colourSpace);
        var colour2 = RandomColours.UnicolourFrom(colourSpace);
        var palette = colour1.Palette(colour2, colourSpace, count: 0, hueSpan, premultiplyAlpha).ToArray();
        Assert.That(palette, Is.Empty);
    }
    
    [Test, Combinatorial]
    public void PaletteNegative(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(nameof(HueSpans))] HueSpan hueSpan,
        [Values(true, false)] bool premultiplyAlpha)
    {
        var colour1 = RandomColours.UnicolourFrom(colourSpace);
        var colour2 = RandomColours.UnicolourFrom(colourSpace);
        var palette = colour1.Palette(colour2, colourSpace, count: -1, hueSpan, premultiplyAlpha).ToArray();
        Assert.That(palette, Is.Empty);
    }
}