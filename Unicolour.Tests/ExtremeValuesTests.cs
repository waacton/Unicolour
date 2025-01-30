using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

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
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, colourSpace, first, second, third));
    }
    
    [Test, Combinatorial]
    public void CreateFromTripletWithAlpha(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double luminance,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double alpha)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(ColourSpace.RgbLinear, luminance, luminance, luminance, alpha));
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, ColourSpace.RgbLinear, luminance, luminance, luminance, alpha));
    }
    
    [Test, Combinatorial]
    public void CreateFromHexWithAlpha(
        [Values("#000000", "#798081", "#FFFFFF")] string hex,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double alpha)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(hex, alpha));
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, hex, alpha));
    }
    
    [Test, Combinatorial]
    public void CreateFromChromaticity(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double x, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double y, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double luminance)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(new Chromaticity(x, y), luminance));
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, new Chromaticity(x, y), luminance));
    }
    
    [Test, Combinatorial]
    public void CreateFromTemperature(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double cct, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double duv, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double luminance)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(new Temperature(cct, duv), luminance));
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, new Temperature(cct, duv), luminance));
    }
    
    [Test, Combinatorial]
    public void CreateFromSpd(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double power1, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double power2, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double power3)
    {
        // -2147483647 nm = power1, 0 nm = power2, 2147483647 nm = power 3
        var spd = new Spd(start: -int.MaxValue, interval: int.MaxValue, power1, power2, power3);
        TestUtils.AssertNoPropertyError(new Unicolour(spd));
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, spd));
    }
    
    [Test, Combinatorial]
    public void CreateFromPigmentSingleConstant(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double coefficient, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double weight)
    {
        double[] coefficients = [coefficient, coefficient, coefficient];
        var pigment = new Pigment(int.MinValue, int.MaxValue, coefficients);
        TestUtils.AssertNoPropertyError(new Unicolour([pigment], [weight]));
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, [pigment], [weight]));
    }
    
    [Test, Combinatorial]
    public void CreateFromPigmentTwoConstant(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double coefficient, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double weight, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double correction)
    {
        double[] coefficients = [coefficient, coefficient, coefficient];
        var pigment = new Pigment(int.MinValue, int.MaxValue, coefficients, coefficients, correction, correction);
        TestUtils.AssertNoPropertyError(new Unicolour([pigment], [weight]));
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, [pigment], [weight]));
    }
    
    [Test, Combinatorial]
    public void CreateFromIcc(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double first, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double second, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double third,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double fourth)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(new Channels(first, second, third, fourth)));
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, new Channels(first, second, third, fourth)));
    }
    
    [Test, Combinatorial]
    public void CreateFromIccWithAlpha(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double channel,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double alpha)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(new Channels(channel, channel, channel, channel), alpha));
        TestUtils.AssertNoPropertyError(new Unicolour(TestUtils.DefaultFogra39Config, new Channels(channel, channel, channel, channel), alpha));
    }
    
    [Test, Combinatorial]
    public void Mix( 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double amount)
    {
        var unicolour1 = RandomColours.UnicolourFrom(colourSpace);
        var unicolour2 = RandomColours.UnicolourFrom(colourSpace);
        TestUtils.AssertNoPropertyError(unicolour1.Mix(unicolour2, colourSpace, amount));
        
        unicolour1 = RandomColours.UnicolourFrom(colourSpace, TestUtils.DefaultFogra39Config);
        unicolour2 = RandomColours.UnicolourFrom(colourSpace, TestUtils.DefaultFogra39Config);
        TestUtils.AssertNoPropertyError(unicolour1.Mix(unicolour2, colourSpace, amount));
    }
}