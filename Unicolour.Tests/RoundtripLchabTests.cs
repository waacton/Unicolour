using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripLchabTests
{
    private const double Tolerance = 0.00000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Lchab, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaLab(ColourTriplet triplet)
    {
        var original = new Lchab(triplet.First, triplet.Second, triplet.Third);
        var lab = Lchab.ToLab(original);
        var roundtrip = Lchab.FromLab(lab);

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