namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class RoundtripChromaticityTests
{
    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.Chromaticities))]
    public void ViaTemperature(Chromaticity chromaticity)
    {
        var original = chromaticity;
        var temperature = Temperature.FromChromaticity(original, TestUtils.PlanckianObserverDegree2);
        var roundtrip = Temperature.ToChromaticity(temperature, Observer.Degree2);
        
        if (double.IsNaN(temperature.Cct) || double.IsNaN(temperature.Duv))
        {
            Assert.That(roundtrip.X, Is.NaN);
            Assert.That(roundtrip.Y, Is.NaN);
            Assert.That(roundtrip.U, Is.NaN);
            Assert.That(roundtrip.V, Is.NaN);
        }
        
        if (!temperature.IsHighAccuracy)
        {
            Assert.That(temperature.Cct, Is.LessThan(1000).Or.GreaterThan(20000));
        }
        
        if (!temperature.IsValid)
        {
            Assert.That(temperature.Duv, Is.LessThan(-0.05).Or.GreaterThan(0.05));
            return;
        }

        const double tolerance = 0.0005;
        Assert.That(roundtrip.X, Is.EqualTo(original.X).Within(tolerance), () => temperature.ToString());
        Assert.That(roundtrip.Y, Is.EqualTo(original.Y).Within(tolerance), () => temperature.ToString());
        Assert.That(roundtrip.U, Is.EqualTo(original.U).Within(tolerance), () => temperature.ToString());
        Assert.That(roundtrip.V, Is.EqualTo(original.V).Within(tolerance), () => temperature.ToString());
    }
}