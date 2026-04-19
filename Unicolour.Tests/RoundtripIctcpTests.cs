using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripIctcpTests
{
    private const double Tolerance = 0.00000000001;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly DynamicRange DynamicRange = DynamicRange.Standard;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Ictcp, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Ictcp(triplet.First, triplet.Second, triplet.Third);
        var xyz = Ictcp.ToXyz(original, XyzConfig.ChromaticAdaptor, DynamicRange);
        var roundtrip = Ictcp.FromXyz(xyz, XyzConfig.ChromaticAdaptor, DynamicRange);

        var maxXyz = xyz.Triplet.ToArray().Max(Math.Abs);
        if (roundtrip.Limitation == Limitation.NaN)
        {
            Assert.That(maxXyz, Is.EqualTo(double.NaN).Or.GreaterThanOrEqualTo(1000000000000000));
        }
        else if (TestUtils.MaxDiff(roundtrip.Triplet, original.Triplet) > Tolerance)
        {
            Assert.That(maxXyz, Is.GreaterThanOrEqualTo(100000));
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}