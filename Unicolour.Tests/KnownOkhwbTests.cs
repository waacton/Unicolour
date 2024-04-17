namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownOkhwbTests
{
    private const double Tolerance = 0.0000005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Okhwb>(red, new(0.0812052366453962 * 360, 0.000478031, 0.000000000), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Okhwb>(green, new(0.3958203857994721 * 360, 0.000000279, 0.000000012), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Okhwb>(blue, new(0.7334778351057084 * 360, 0.000008909, 0.000000035), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Okhwb>(black, new(0, 0, 1), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Okhwb>(white, new(0.2496543419330623 * 360, 0.999999924, -0.000000027), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertTriplet<Okhwb>(grey, new(0.24965434119047608 * 360, 0.533759786, 0.466240158), Tolerance);
    }
}