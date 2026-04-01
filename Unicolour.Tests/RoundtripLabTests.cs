using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripLabTests
{
    private const double Tolerance = 0.00000000001;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Lab, 1500);

    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Lab(triplet.First, triplet.Second, triplet.Third);
        var xyz = Lab.ToXyz(original, XyzConfig.WhitePoint);
        var roundtrip = Lab.FromXyz(xyz);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaLchab(ColourTriplet triplet)
    {
        var original = new Lab(triplet.First, triplet.Second, triplet.Third);
        var lchab = Lchab.FromLab(original);
        var roundtrip = Lchab.ToLab(lchab);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}