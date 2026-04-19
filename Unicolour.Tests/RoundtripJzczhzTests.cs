using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripJzczhzTests
{
    private const double Tolerance = 0.00000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Jzczhz, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaJzazbz(ColourTriplet triplet)
    {
        var original = new Jzczhz(triplet.First, triplet.Second, triplet.Third);
        var jzazbz = Jzczhz.ToJzazbz(original);
        var roundtrip = Jzczhz.FromJzazbz(jzazbz);
        
        if (original.C < 0)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, new(original.J, 0, 0, HueIndex: 2), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}