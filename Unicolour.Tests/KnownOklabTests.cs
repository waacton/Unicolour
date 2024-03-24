namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class KnownOklabTests
{
    private const double Tolerance = 0.0005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Oklab>(red, new(0.62796, 0.22486, 0.12585), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Oklab>(green, new(0.86644, -0.23389, 0.1795), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Oklab>(blue, new(0.45201, -0.03246, -0.31153), Tolerance);
    }
    
    [Test]
    public void Black()
    {
        var black = StandardRgb.Black;
        TestUtils.AssertTriplet<Oklab>(black, new(0.0, 0.0, 0.0), Tolerance);
    }
    
    [Test]
    public void White()
    {
        var white = StandardRgb.White;
        TestUtils.AssertTriplet<Oklab>(white, new(1.0, 0.0, 0.0), Tolerance);
    }
    
    // Oklab actually provides test values 🙌 (https://bottosson.github.io/posts/oklab/#table-of-example-xyz-and-oklab-pairs)
    // so it has its own dedicated set of tests based on those
    private static readonly List<(ColourTriplet xyz, ColourTriplet expectedOklab)> TestData = new()
    {
        (new(0.950, 1.000, 1.089), new(1.000, 0.000, 0.000)),
        (new(1.000, 0.000, 0.000), new(0.450, 1.236, -0.019)),
        (new(0.000, 1.000, 0.000), new(0.922, -0.671, 0.263)),
        (new(0.000, 0.000, 1.000), new(0.153, -1.415, -0.449))
    };
    
    [Test]
    public void FromXyzD65()
    {
        foreach (var (xyz, expectedOklab) in TestData)
        {
            AssertFromXyzD65(xyz, expectedOklab);
        }
    }
    
    [Test]
    public void FromXyzD50()
    {
        foreach (var (xyz, expectedOklab) in TestData)
        {
            AssertFromXyzD50(xyz, expectedOklab);
        }
    }
    
    private static void AssertFromXyzD65(ColourTriplet xyz, ColourTriplet expectedOklab)
    {
        var (x, y, z) = xyz;
        var oklab = Oklab.FromXyz(new Xyz(x, y, z), XyzConfiguration.D65);
        AssertOklab(oklab, expectedOklab);
    }

    private static void AssertFromXyzD50(ColourTriplet xyz, ColourTriplet expectedOklab)
    {
        // create unicolour from default D65 XYZ whitepoint
        var fromXyzD65 = new Unicolour(ColourSpace.Xyz, xyz.Tuple);
        var rgb = fromXyzD65.Rgb;
        
        // using the D65 RGB, create a unicolour based in D50 XYZ
        var configXyzD50 = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D50);
        
        var toXyzD50 = new Unicolour(configXyzD50, ColourSpace.Rgb, rgb.Triplet.Tuple);
        var oklabFromXyzD50 = toXyzD50.Oklab;
        
        // since Oklab specifically uses a D65 whitepoint
        // ensure Oklab results are still as expected, despite starting in D50 XYZ
        AssertOklab(oklabFromXyzD50, expectedOklab);
        Assert.That(fromXyzD65.Xyz, Is.Not.EqualTo(toXyzD50.Xyz));
    }

    private static void AssertOklab(Oklab oklab, ColourTriplet expected)
    {
        Assert.That(Math.Round(oklab.L, 3), Is.EqualTo(expected.First));
        Assert.That(Math.Round(oklab.A, 3), Is.EqualTo(expected.Second));
        Assert.That(Math.Round(oklab.B, 3), Is.EqualTo(expected.Third));
    }
}