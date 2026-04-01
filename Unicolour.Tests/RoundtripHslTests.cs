using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripHslTests
{
    private const double Tolerance = 0.0000000001;
    
    internal static readonly List<ColourTriplet> Triplets = Rng.Triplets(ColourSpace.Hsl, 1500);
    
    [TestCaseSource(nameof(Triplets))]
    public void ViaHsb(ColourTriplet triplet) => AssertViaHsb(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHsbFromNamed(TestColour namedColour) => AssertViaHsb(namedColour.Hsl!);
    
    private static void AssertViaHsb(ColourTriplet triplet)
    {
        var original = new Hsl(triplet.First, triplet.Second, triplet.Third);
        var hsb = Hsl.ToHsb(original);
        var roundtrip = Hsl.FromHsb(hsb);
        
        if (original.L is 0.0 or 1.0)
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, new(original.H, 0, original.L, HueIndex: 0), Tolerance);
        }
        else
        {
            TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
        }
    }
}