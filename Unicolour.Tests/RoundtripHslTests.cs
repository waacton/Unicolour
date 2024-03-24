namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripHslTests
{
    private const double Tolerance = 0.0000000001;
    
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HslTriplets))]
    public void ViaHsb(ColourTriplet triplet) => AssertViaHsb(triplet);
    
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void ViaHsbFromNamed(TestColour namedColour) => AssertViaHsb(namedColour.Hsl!);
    
    private static void AssertViaHsb(ColourTriplet triplet)
    {
        var original = new Hsl(triplet.First, triplet.Second, triplet.Third);
        var hsb = Hsl.ToHsb(original);
        var roundtrip = Hsl.FromHsb(hsb);
        TestUtils.AssertTriplet(roundtrip.Triplet, original.Triplet, Tolerance);
    }
}