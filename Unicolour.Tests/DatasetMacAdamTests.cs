using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Tests;

public class DatasetMacAdamTests
{
    private static readonly TestCaseData[] StartWavelengthPoints =
    [
        new TestCaseData(MacAdam.Limits10, 0.1346, 0.0747).SetName("10"),
        new TestCaseData(MacAdam.Limits20, 0.1268, 0.1365).SetName("20"),
        new TestCaseData(MacAdam.Limits30, 0.1282, 0.1889).SetName("30"),
        new TestCaseData(MacAdam.Limits40, 0.1360, 0.2324).SetName("40"),
        new TestCaseData(MacAdam.Limits50, 0.1491, 0.2679).SetName("50"),
        new TestCaseData(MacAdam.Limits60, 0.1674, 0.2959).SetName("60"),
        new TestCaseData(MacAdam.Limits70, 0.1916, 0.3164).SetName("70"),
        new TestCaseData(MacAdam.Limits80, 0.2232, 0.3290).SetName("80"),
        new TestCaseData(MacAdam.Limits90, 0.2639, 0.3331).SetName("90"),
        new TestCaseData(MacAdam.Limits95, 0.2875, 0.3320).SetName("95")
    ];
    
    [TestCaseSource(nameof(StartWavelengthPoints))]
    public void StartWavelengthPoint(Unicolour[] limits, double x, double y)
    {
        Assert.That(limits.Length, Is.EqualTo(24));

        var point = limits.First();
        Assert.That(point.Chromaticity.X, Is.EqualTo(x));
        Assert.That(point.Chromaticity.Y, Is.EqualTo(y));
    }
    
    [Test]
    public void All()
    {
        Assert.That(MacAdam.All.Count(), Is.EqualTo(240));
        Assert.That(MacAdam.All.Distinct().Count(), Is.EqualTo(240));
    }
}