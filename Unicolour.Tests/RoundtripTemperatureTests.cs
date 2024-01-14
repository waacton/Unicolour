namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripTemperatureTests
{
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Temperatures))]
    public void ViaChromaticity(Temperature temperature)
    {
        var original = temperature;
        var roundtrip = Temperature.FromChromaticity(Temperature.ToChromaticity(original, Observer.Degree2), TestUtils.PlanckianObserverDegree2);
        Assert.That(roundtrip.Cct, Is.EqualTo(original.Cct).Within(1));
        Assert.That(roundtrip.Duv, Is.EqualTo(original.Duv).Within(0.00001));
    }
}