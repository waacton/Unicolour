using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownOkrlabTests
{
    private const double Tolerance = 0.000005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertColour(red, new Oklrab(0.56808, 0.22486306106597398, 0.1258462985307351), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertColour(green, new Oklrab(0.84453, -0.23388757418790818, 0.17949847989672985), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertColour(blue, new Oklrab(0.36657, -0.03245698416876397, -0.3115281476783751), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertColour(black, new Oklrab(0.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertColour(white, new Oklrab(1.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertColour(grey, new Oklrab(0.53376, 4.842454215392422e-11, 2.2296533230825588e-8), Tolerance);
    }
}