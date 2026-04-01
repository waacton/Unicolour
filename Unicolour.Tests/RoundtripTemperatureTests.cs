using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class RoundtripTemperatureTests
{
    internal static List<Temperature> Temperatures = new int[1000].Select(_ => Rng.Temperature()).ToList();
    
    [TestCaseSource(nameof(Temperatures))]
    public void ViaChromaticity(Temperature temperature)
    {
        var original = temperature;
        var chromaticity = Temperature.ToChromaticity(original, Observer.Degree2);
        var roundtrip = Temperature.FromChromaticity(chromaticity, TestUtils.PlanckianObserverDegree2);
        Assert.That(roundtrip.Cct, Is.EqualTo(original.Cct).Within(1));
        Assert.That(roundtrip.Duv, Is.EqualTo(original.Duv).Within(0.00001));
    }
}