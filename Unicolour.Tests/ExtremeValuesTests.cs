namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ExtremeValuesTests
{
    [Test, Combinatorial]
    public void CreateFromTriplet(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double first, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double second, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double third)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(colourSpace, first, second, third));
    }
    
    [Test, Combinatorial]
    public void CreateFromTripletWithAlpha(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double luminance,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double alpha)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(ColourSpace.RgbLinear, luminance, luminance, luminance, alpha));
    }
    
    [Test, Combinatorial]
    public void CreateFromHexWithAlpha(
        [Values("#000000", "#798081", "#FFFFFF")] string hex,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double alpha)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(hex, alpha));
    }
    
    [Test, Combinatorial]
    public void CreateFromChromaticity(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double x, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double y, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double luminance)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(new Chromaticity(x, y), luminance));
    }
    
    [Test, Combinatorial]
    public void CreateFromTemperature(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double cct, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double duv, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double luminance)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(new Temperature(cct, duv), luminance));
    }
    
    [Test, Combinatorial]
    public void CreateFromSpd(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double power1, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double power2, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double power3)
    {
        var spd = new Spd
        {
            { int.MinValue, power1 },
            { 0, power2 },
            { int.MaxValue, power3 }
        };
        TestUtils.AssertNoPropertyError(new Unicolour(spd));
    }
    
    [Test, Combinatorial]
    public void Mix( 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double amount)
    {
        var unicolour1 = RandomColours.UnicolourFrom(colourSpace);
        var unicolour2 = RandomColours.UnicolourFrom(colourSpace);
        TestUtils.AssertNoPropertyError(unicolour1.Mix(unicolour2, colourSpace, amount));
    }
}