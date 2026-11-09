using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Hellwig;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHellwigUcsTests
{
    private const double Tolerance = 0.00005;
    private static readonly CamConfiguration CamConfig = CamConfiguration.StandardRgb;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.HellwigUcs, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaHellwig(ColourTriplet triplet)
    {
        var original = new Ucs(triplet.First, triplet.Second, triplet.Third);
        var Hellwig = Ucs.ToCam(original, CamConfig.ViewHellwig);
        var roundtrip = Ucs.FromCam(Hellwig);
        TestUtils.AssertTriplet(roundtrip, original, Tolerance);
    }
}