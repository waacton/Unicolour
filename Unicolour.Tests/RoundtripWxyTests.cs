using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripWxyTests
{
    private const double Tolerance = 0.0000000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Wxy, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaXyy(ColourTriplet triplet)
    {
        var original = new Wxy(triplet.First, triplet.Second, triplet.Third);
        var xyy = Wxy.ToXyy(original, XyzConfig.SpectralBoundary);
        var roundtrip = Wxy.FromXyy(xyy, XyzConfig.SpectralBoundary);

        if (original.X >= 0)
        {
            AssertPositivePurity(roundtrip, original);
        }
        else
        {
            AssertNegativePurity(roundtrip, original, xyy.Chromaticity);
        }
    }

    private static void AssertPositivePurity(Wxy actual, Wxy expected)
    {
        // wavelengths are 2 separate number ranges splices together
        // values from outside both of those ranges are difficult to reason about directly
        // so use their degree representation instead
        if (IsValidWavelength(expected.W))
        {
            TestUtils.AssertTriplet(actual.Triplet, expected.Triplet, Tolerance);
        }
        else
        {
            var tolerance = Tolerance * 2;
            TestUtils.AssertTriplet(MapToHue(actual), MapToHue(expected), tolerance);
        }
    }
    
    // negative purity is the same as moving towards the complementary wavelength instead of the dominant wavelength
    // since XYY to WXY finds the dominant, need to confirm:
    // 1) roundtrip is the dominant value, 2) original is the negated complementary value
    private static void AssertNegativePurity(Wxy roundtrip, Wxy original, Chromaticity chromaticity)
    {
        var dominant = XyzConfig.SpectralBoundary.GetWavelengthAndPurity(chromaticity, forDominant: true)!.Value;
        var complement = XyzConfig.SpectralBoundary.GetWavelengthAndPurity(chromaticity, forDominant: false)!.Value;

        var expectedDominant = new Wxy(dominant.wavelength, dominant.purity, original.Y);
        var expectedComplement = new Wxy(complement.wavelength, -complement.purity, original.Y);

        // wavelengths are 2 separate number ranges splices together
        // values from outside both of those ranges are difficult to reason about directly
        // so use their degree representation instead
        if (IsValidWavelength(original.W))
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, expectedDominant.Triplet, Tolerance);
            TestUtils.AssertTriplet(original.Triplet, expectedComplement.Triplet, Tolerance);
        }
        else
        {
            var tolerance = Tolerance * 2;
            TestUtils.AssertTriplet(MapToHue(roundtrip), MapToHue(expectedDominant), tolerance);
            TestUtils.AssertTriplet(MapToHue(original), MapToHue(expectedComplement), [0.75, tolerance, tolerance]);
        }
    }

    private static bool IsValidWavelength(double wavelength)
    {
        var isOnSpectralLocus = wavelength >= SpectralBoundary.MinWavelength && wavelength <= SpectralBoundary.MaxWavelength;
        var isOnLineOfPurples = wavelength >= XyzConfig.SpectralBoundary.MinNegativeWavelength && wavelength <= XyzConfig.SpectralBoundary.MaxNegativeWavelength;
        return isOnSpectralLocus || isOnLineOfPurples;
    }

    private static ColourTriplet MapToHue(Wxy wxy)
    {
        return wxy.Triplet.WithHueMap(w => Hue.FromWavelength(w, XyzConfig));
    }
}