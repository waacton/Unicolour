using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Cam02;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripCam02UcsTests
{
    private const double Tolerance = 0.00005;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Cam02Ucs, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaCam02(ColourTriplet triplet)
    {
        var original = new Ucs(triplet.First, triplet.Second, triplet.Third);
        var cam02 = Ucs.ToCam(original, CamConfig.View02);
        var roundtrip = Ucs.FromCam(cam02);
        TestUtils.AssertTriplet(roundtrip, original, Tolerance);
    }
}