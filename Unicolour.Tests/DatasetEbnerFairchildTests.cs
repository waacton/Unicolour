using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using static Wacton.Unicolour.Datasets.EbnerFairchild;

namespace Wacton.Unicolour.Tests;

public class DatasetEbnerFairchildTests
{
    private static readonly List<TestCaseData> ReferenceHues =
    [
        new TestCaseData(Hue0Ref, 0).SetName("0"),
        new TestCaseData(Hue24Ref, 24).SetName("24"),
        new TestCaseData(Hue48Ref, 48).SetName("48"),
        new TestCaseData(Hue72Ref, 72).SetName("72"),
        new TestCaseData(Hue96Ref, 96).SetName("96"),
        new TestCaseData(Hue120Ref, 120).SetName("120"),
        new TestCaseData(Hue144Ref, 144).SetName("144"),
        new TestCaseData(Hue168Ref, 168).SetName("168"),
        new TestCaseData(Hue192Ref, 192).SetName("192"),
        new TestCaseData(Hue216Ref, 216).SetName("216"),
        new TestCaseData(Hue240Ref, 240).SetName("240"),
        new TestCaseData(Hue264Ref, 264).SetName("264"),
        new TestCaseData(Hue288Ref, 288).SetName("288"),
        new TestCaseData(Hue312Ref, 312).SetName("312"),
        new TestCaseData(Hue336Ref, 336).SetName("336")
    ];
    
    private static readonly List<TestCaseData> GroupedByHue =
    [
        new TestCaseData(AllHue0, 0, 21).SetName("0"),
        new TestCaseData(AllHue24, 24, 21).SetName("24"),
        new TestCaseData(AllHue48, 48, 21).SetName("48"),
        new TestCaseData(AllHue72, 72, 21).SetName("72"),
        new TestCaseData(AllHue96, 96, 21).SetName("96"),
        new TestCaseData(AllHue120, 120, 21).SetName("120"),
        new TestCaseData(AllHue144, 144, 21).SetName("144"),
        new TestCaseData(AllHue168, 168, 21).SetName("168"),
        new TestCaseData(AllHue192, 192, 21).SetName("192"),
        new TestCaseData(AllHue216, 216, 21).SetName("216"),
        new TestCaseData(AllHue240, 240, 20).SetName("240"),
        new TestCaseData(AllHue264, 264, 21).SetName("264"),
        new TestCaseData(AllHue288, 288, 24).SetName("288"),
        new TestCaseData(AllHue312, 312, 25).SetName("312"),
        new TestCaseData(AllHue336, 336, 21).SetName("336")
    ];
    
    [TestCaseSource(nameof(ReferenceHues))]
    public void ReferenceHue(Unicolour colour, int expectedHue)
    {
        const double tolerance = 0.05;
        Assert.That(colour.Lchab.H, Is.EqualTo(expectedHue).Within(tolerance).Or.EqualTo(expectedHue + 360).Within(tolerance));
    }

    [TestCaseSource(nameof(GroupedByHue))]
    public void GroupedHue(List<Unicolour> colours, int expectedHue, int expectedCount)
    {
        Assert.That(colours.Count, Is.EqualTo(expectedCount));
        
        // questionable, but would be surprised if a Lab's hue for a group was beyond a neighbouring group's hue
        const int tolerance = 24;
        var hues = colours.Select(x => x.Lchab.H);
        Assert.That(hues, Has.All.EqualTo(expectedHue).Within(tolerance).Or.EqualTo(expectedHue + 360).Within(tolerance));
    }
    
    [Test]
    public void All() => Assert.That(EbnerFairchild.All.Distinct().Count(), Is.EqualTo(321));
}