using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripXyyTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Xyy, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var xyz = Xyy.ToXyz(original);
        var roundtrip = Xyy.FromXyz(xyz);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaWxy(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third, XyzConfig.WhitePoint);
        var wxy = Wxy.FromXyy(original, XyzConfig.SpectralBoundary);
        var roundtrip = Wxy.ToXyy(wxy, XyzConfig.SpectralBoundary);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }

    [TestCaseSource(nameof(Triplets))]
    public void ViaMunsell(ColourTriplet triplet)
    {
        var original = new Xyy(triplet.First, triplet.Second, triplet.Third, TestUtils.CConfig.Xyz.WhitePoint);
        var munsell = Munsell.FromXyy(original, TestUtils.CConfig.Xyz.ChromaticAdaptor);
        var roundtrip = Munsell.ToXyy(munsell, TestUtils.CConfig.Xyz.ChromaticAdaptor);
        
        var isTypicalRange = original.Chromaticity is { X: >= 0 and <= 1, Y: >= 0 and <= 1 };

        if (munsell.XyyToHvcSearchResult!.Converged)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [0.00001, 0.00001, 5e-15]);
        }
        else if (munsell.C == 0.0)
        {
            var white = TestUtils.CConfig.Xyz.WhitePoint.Chromaticity;
            var expected = new ColourTriplet(white.X, white.Y, original.Luminance);
            TestUtils.AssertTriplet(roundtrip.Triplet, expected, [0, 0, 1e-10]);
        }
        else if (!isTypicalRange)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [0.21, 0.21, 5e-15]);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, [0.16, 0.16, 5e-15]);
        }
    }
}