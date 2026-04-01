using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripOklrchTests
{
    private const double Tolerance = 0.00000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Oklrch, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaOklrab(ColourTriplet triplet)
    {
        var original = new Oklrch(triplet.First, triplet.Second, triplet.Third);
        var oklrab = Oklrch.ToOklrab(original);
        var roundtrip = Oklrch.FromOklrab(oklrab);

        if (original.C < 0)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, new(original.L, 0, 0, HueIndex: 2), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}