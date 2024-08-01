using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripIctcpTests
{
    private const double DefaultTolerance = 0.00000000001;
    private static readonly XyzConfiguration XyzConfig = XyzConfiguration.D65;
    private const double IctcpScalar = 100;

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.IctcpTriplets))]
    public void ViaXyz(ColourTriplet triplet)
    {
        // Ictcp -> XYZ can produce NaNs due to a negative number to a fractional power in the conversion process
        var original = new Ictcp(triplet.First, triplet.Second, triplet.Third);
        var xyz = Ictcp.ToXyz(original, IctcpScalar, XyzConfig);
        var roundtrip = Ictcp.FromXyz(xyz, IctcpScalar, XyzConfig);
        TestUtils.AssertTriplet(roundtrip.Triplet, roundtrip.IsNaN ? new(double.NaN, double.NaN, double.NaN) : original.Triplet, DefaultTolerance);
    }
}