namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ExtremeValuesTests
{
    [Test, Combinatorial]
    public void Create(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double first, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double second, 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double third)
    {
        TestUtils.AssertNoPropertyError(new Unicolour(colourSpace, first, second, third));
    }
    
    
    [Test, Combinatorial]
    public void Mix( 
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllColourSpaces))] ColourSpace colourSpace,
        [ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double amount)
    {
        var unicolour1 = RandomColours.UnicolourFrom(colourSpace);
        var unicolour2 = RandomColours.UnicolourFrom(colourSpace);
        TestUtils.AssertNoPropertyError(unicolour1.Mix(colourSpace, unicolour2, amount));
    }
}