using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class KnownJzazbzTests
{
    private const double Tolerance = 0.0005;
    private static readonly Configuration Jzazbz203Config = new(jzazbzScalar: 203);
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red.ConvertToConfiguration(Jzazbz203Config);
        TestUtils.AssertTriplet<Jzazbz>(red, new(0.13438, 0.11789, 0.11188), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green.ConvertToConfiguration(Jzazbz203Config);
        TestUtils.AssertTriplet<Jzazbz>(green, new(0.17681, -0.10904, 0.11899), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue.ConvertToConfiguration(Jzazbz203Config);
        TestUtils.AssertTriplet<Jzazbz>(blue, new(0.09577, -0.04085, -0.18585), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black.ConvertToConfiguration(Jzazbz203Config);
        TestUtils.AssertTriplet<Jzazbz>(black, new(0.00000, 0.00000, 0.00000), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White.ConvertToConfiguration(Jzazbz203Config);
        TestUtils.AssertTriplet<Jzazbz>(white, new(0.22207, -0.00016, -0.00012), Tolerance);
    }
}