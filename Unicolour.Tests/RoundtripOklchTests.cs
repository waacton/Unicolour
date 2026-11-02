using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOklchTests
{
    private const double Tolerance = 0.00000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Oklch, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaOklab(ColourTriplet triplet)
    {
        var original = new Oklch(triplet.First, triplet.Second, triplet.Third);
        var oklab = Oklch.ToOklab(original);
        var roundtrip = Oklch.FromOklab(oklab);
        
        if (original.C < 0)
        {
            TestUtils.AssertTriplet(roundtrip, new(original.L, 0, 0, hueIndex: 2), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip, original, Tolerance);
        }
    }
}