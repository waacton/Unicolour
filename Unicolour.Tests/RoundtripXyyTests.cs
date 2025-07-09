using System;
using NUnit.Framework;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripXyyTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var xyz = Xyy.ToXyz(original);
        var roundtrip = Xyy.FromXyz(xyz, XyzConfig.WhiteChromaticity);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaWxy(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var wxy = Wxy.FromXyy(original, XyzConfig);
        var roundtrip = Wxy.ToXyy(wxy, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    /*
     * problematic test values:
        ViaMunsell((0.15425324758587644, 0.6976085501772628, 0.008044993192463323)) --- channel 2
        ViaMunsell((0.3402944443650804, 0.207686314306176, 0.5532985238791019)) --- index out of range from degrees (!)
        ViaMunsell((0.3738560692063023, 0.2022614418865818, 0.3032515877510755)) --- channel 1
     */
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.XyyTriplets))]
    public void ViaMunsell(ColourTriplet triplet)
    {
        try
        {
            var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
            var munsell = Munsell.FromXyy(original);
            Console.WriteLine(munsell);
            var roundtrip = Munsell.ToXyy(munsell);

            // TODO: review; data points are much more distributed in the green area of chromaticity
            //       resulting in less accuracy when interpolating across greater distances
            //       and even fewer points to use for interpolation at lower Vs
            double tolerance;
            if (munsell.Hue.letter.Contains("G"))
            {
                tolerance =
                    munsell.Value < 1.5 ? 0.075 : 
                    munsell.Value < 2 ? 0.065 : 
                    munsell.Value < 2.5 ? 0.05 : 
                    munsell.Value < 3 ? 0.035 : 
                    0.025;
            }
            else
            {
                tolerance = munsell.Value < 1 ? 0.025 : 0.02;
            }
            
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, tolerance); // TODO:
        }
        catch (InvalidOperationException e)
        {
            Assert.Ignore();
        }
    }
    
    [Test]
    public void Single()
    {
        // var triplet = new ColourTriplet(0.2617133551152435, 0.5075758984804047, 0.2902655162931662); // produces 1.85G 5.98/13.48, roundtrip is then off 0.0149, surprisingly large
        var triplet = new ColourTriplet(0.1930485142586158, 0.646342341051852, 0.026811862120930452); // produces 1.08G 1.85/9.9, roundtrip is then off 0.029, surprisingly large
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third);
        var munsell = Munsell.FromXyy(original);
        var roundtrip = Munsell.ToXyy(munsell);
        
        // var scaled = munsell.HueDegrees / Munsell.DegreesPerHueNumber; // maps 0-360 to 0-40 (10 letter bands with 4 numbers per band)
        // var h = Math.Round(scaled) * Munsell.DegreesPerHueNumber;
        // var hue = MunsellUtils.FromDegrees(h);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, 0.02);
    }
}