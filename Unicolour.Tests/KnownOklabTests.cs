using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

// matches values produced by https://github.com/bottosson/bottosson.github.io/blob/master/misc/colorpicker/colorconversion.js
public class KnownOklabTests
{
    private const double Tolerance = 0.0000005;
    
    [Test]
    public void Red()
    {
        var red = StandardRgb.Red;
        TestUtils.AssertTriplet<Oklab>(red, new(0.6279553606145516, 0.22486306106597398, 0.1258462985307351), Tolerance);
    }
    
    [Test]
    public void Green()
    {
        var green = StandardRgb.Green;
        TestUtils.AssertTriplet<Oklab>(green, new(0.8664396115356694, -0.23388757418790818, 0.17949847989672985), Tolerance);
    }
    
    [Test]
    public void Blue()
    {
        var blue = StandardRgb.Blue;
        TestUtils.AssertTriplet<Oklab>(blue, new(0.4520137183853429, -0.03245698416876397, -0.3115281476783751), Tolerance);
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
    
    [Test]
    public void Grey()
    {
        var grey = StandardRgb.Grey;
        TestUtils.AssertTriplet<Oklab>(grey, new(0.5981807266228486, 4.842454215392422e-11, 2.2296533230825588e-8), Tolerance);
    }
    
    /*
     * Oklab provides XYZ test values (https://bottosson.github.io/posts/oklab/#table-of-example-xyz-and-oklab-pairs)
     * except... they seem to be derived from the M1 matrix used in XYZ -> OKLAB as shown in the implementation article
     * which would be fine except the source code uses a direct RGB -> OKLAB conversion (https://github.com/bottosson/bottosson.github.io/blob/f6f08b7fde9436be1f20f66cebbc739d660898fd/misc/colorpicker/colorconversion.js#L163) 
     * and RGB -> OKLAB behaviour only matches XYZ -> OKLAB when white point values are the ones that were used to derive M1
     * however there is no agreement on white point values 😒 (https://ninedegreesbelow.com/photography/well-behaved-profiles-quest.html#white-point-values)
     * --------------------
     * the options are to either...
     * A) assume the XYZ test values are intended to be "correct"
     * - use explicit M1 matrix (from https://bottosson.github.io/posts/oklab/#converting-from-xyz-to-oklab)
     * - XYZ -> OKLAB is accurate to these XYZ test values (3.d.p, which is the accuracy of the data provided)
     * - RGB -> OKLAB is less accurate to the source code (3.d.p)
     * B) assume the source code is intended to be "correct"
     * - calculate M1 matrix from config (use explicit RGB -> LMS matrix from https://github.com/bottosson/bottosson.github.io/blob/f6f08b7fde9436be1f20f66cebbc739d660898fd/misc/colorpicker/colorconversion.js#L163)
     * - XYZ -> OKLAB is less accurate to these XYZ test values (mostly 3.d.p, but 2.d.p in one case)
     * - RGB -> OKLAB is accurate to the source code (6 d.p)
     * --------------------
     * Unicolour assumes B) and that the intention of OKLAB is that RGB -> OKLAB should match the example code
     */
    private static readonly List<TestCaseData> TestData =
    [
        new TestCaseData(new ColourTriplet(0.950, 1.000, 1.089), new ColourTriplet(1.000, 0.000, 0.000)),
        new TestCaseData(new ColourTriplet(1.000, 0.000, 0.000), new ColourTriplet(0.450, 1.236, -0.019)),
        new TestCaseData(new ColourTriplet(0.000, 1.000, 0.000), new ColourTriplet(0.922, -0.671, 0.263)),
        new TestCaseData(new ColourTriplet(0.000, 0.000, 1.000), new ColourTriplet(0.153, -1.415, -0.449))
    ];
    
    [TestCaseSource(nameof(TestData))]
    public void FromXyz(ColourTriplet xyz, ColourTriplet expected)
    {
        AssertFromXyzD65(xyz, expected);
        AssertFromXyzD50(xyz, expected);
    }
    
    private static void AssertFromXyzD65(ColourTriplet xyz, ColourTriplet expected)
    {
        var (x, y, z) = xyz;
        var oklab = Oklab.FromXyz(new Xyz(x, y, z), XyzConfiguration.D65, RgbConfiguration.StandardRgb);
        AssertOklab(oklab, expected);
    }

    private static void AssertFromXyzD50(ColourTriplet xyz, ColourTriplet expected)
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
        AssertOklab(oklabFromXyzD50, expected);
        Assert.That(fromXyzD65.Xyz, Is.Not.EqualTo(toXyzD50.Xyz));
    }

    private static void AssertOklab(Oklab oklab, ColourTriplet expected)
    {
        Assert.That(Math.Round(oklab.L, 2), Is.EqualTo(Math.Round(expected.First, 2)));
        Assert.That(Math.Round(oklab.A, 2), Is.EqualTo(Math.Round(expected.Second, 2)));
        Assert.That(Math.Round(oklab.B, 2), Is.EqualTo(Math.Round(expected.Third, 2)));
    }
}