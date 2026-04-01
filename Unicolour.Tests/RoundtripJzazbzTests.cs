using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripJzazbzTests
{
    private const double Tolerance = 0.00000005;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private static readonly DynamicRange DynamicRange = DynamicRange.Standard;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Jzazbz, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        var original = new Jzazbz(triplet.First, triplet.Second, triplet.Third);
        var xyz = Jzazbz.ToXyz(original, XyzConfig.ChromaticAdaptor, DynamicRange);
        var roundtrip = Jzazbz.FromXyz(xyz, XyzConfig.ChromaticAdaptor, DynamicRange);
        
        // Jzazbz -> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        TestUtils.AssertTriplet(roundtrip.Triplet, xyz.Limitation == Limitation.NaN ? new(double.NaN, double.NaN, double.NaN) : original.Triplet, Tolerance);
    }
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaJzczhz(ColourTriplet triplet)
    {
        var original = new Jzazbz(triplet.First, triplet.Second, triplet.Third);
        var jzczhz = Jzczhz.FromJzazbz(original);
        var roundtrip = Jzczhz.ToJzazbz(jzczhz);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}