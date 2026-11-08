using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripCam16UcsTests
{
    private const double Tolerance = 0.00005;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Cam16Ucs, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaCam16(ColourTriplet triplet)
    {
        var original = new Cam16Ucs(triplet.First, triplet.Second, triplet.Third);
        var cam16 = Cam16Ucs.ToCam16(original, CamConfig.View16);
        var roundtrip = Cam16Ucs.FromCam16(cam16);
        TestUtils.AssertTriplet(roundtrip, original, Tolerance);
    }
}