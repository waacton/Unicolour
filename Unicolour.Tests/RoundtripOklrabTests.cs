using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOklrabTests
{
    private const double Tolerance = 0.00000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Oklrab, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Oklrab(triplet.First, triplet.Second, triplet.Third);
        var oklab = Oklrab.ToOklab(original);
        var roundtrip = Oklrab.FromOklab(oklab);

        // OKLrAB <-> OKLAB is not robust when L is negative due to the Okhsv Toe functions
        // some instances of these out-of-gamut values do roundtrip
        // but this confirms that when they do not, it is related to these unusual values
        if (TestUtils.MaxDiff(roundtrip.Triplet, original.Triplet) > Tolerance)
        {
            Assert.That(original.L < 0);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaOklrch(ColourTriplet triplet)
    {
        var original = new Oklrab(triplet.First, triplet.Second, triplet.Third);
        var oklrch = Oklrch.FromOklrab(original);
        var roundtrip = Oklrch.ToOklrab(oklrch);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}