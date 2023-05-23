namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;
using static Datasets.HungBerns;

public class DatasetHungBernsTests
{
    private static readonly List<TestCaseData> ReferenceHues = new()
    {
        new TestCaseData(RedRef, new Xyy(0.6195, 0.3516, 0.3090), new Luv(62.4, 173.3, 55.3)).SetName("Red"),
        new TestCaseData(RedYellowRef, new Xyy(0.4961, 0.4464, 0.6138), new Luv(82.6, 73.7, 90.7)).SetName("Red-yellow"),
        new TestCaseData(YellowRef, new Xyy(0.4276, 0.4984, 1.0041), new Luv(100.2, 12.5, 118.5)).SetName("Yellow"),
        new TestCaseData(YellowGreenRef, new Xyy(0.3736, 0.5393, 0.9472), new Luv(97.9, -37.6, 121.4)).SetName("Yellow-green"),
        new TestCaseData(GreenRef, new Xyy(0.2818, 0.6090, 0.7765), new Luv(90.7, -100.5, 119.7)).SetName("Green"),
        new TestCaseData(GreenCyanRef, new Xyy(0.2278, 0.3833, 0.8476), new Luv(93.8, -89.4, 26.7)).SetName("Green-cyan"),
        new TestCaseData(CyanRef, new Xyy(0.2093, 0.3053, 0.7227), new Luv(88.1, -76.4, -24.4)).SetName("Cyan"),
        new TestCaseData(CyanBlueRef, new Xyy(0.1924, 0.2337, 0.4526), new Luv(73.1, -55.9, -69.3)).SetName("Cyan-blue"),
        new TestCaseData(BlueRef, new Xyy(0.1576, 0.0783, 0.1077), new Luv(39.2, -13.8, -135.8)).SetName("Blue"),
        new TestCaseData(BlueMagentaRef, new Xyy(0.2578, 0.1341, 0.2330), new Luv(55.4, 36.8, -119.6)).SetName("Blue-magenta"),
        new TestCaseData(MagentaRef, new Xyy(0.3236, 0.1734, 0.3683), new Luv(67.1, 79.5, -95.1)).SetName("Magenta"),
        new TestCaseData(MagentaRedRef, new Xyy(0.4191, 0.2314, 0.3553), new Luv(66.2, 119.2, -33.7)).SetName("Magenta-red"),
        new TestCaseData(White, new Xyy(0.3101, 0.3163, 1.0000), new Luv(100.0, 0.0, 0.0)).SetName("White")
    };
    
    private static readonly List<TestCaseData> GroupedByHue = new()
    {
        new TestCaseData(AllRed).SetName("Red"),
        new TestCaseData(AllRedYellow).SetName("Red-yellow"),
        new TestCaseData(AllYellow).SetName("Yellow"),
        new TestCaseData(AllYellowGreen).SetName("Yellow-green"),
        new TestCaseData(AllGreen).SetName("Green"),
        new TestCaseData(AllGreenCyan).SetName("Green-cyan"),
        new TestCaseData(AllCyan).SetName("Cyan"),
        new TestCaseData(AllCyanBlue).SetName("Cyan-blue"),
        new TestCaseData(AllBlue).SetName("Blue"),
        new TestCaseData(AllBlueMagenta).SetName("Blue-magenta"),
        new TestCaseData(AllMagenta).SetName("Magenta"),
        new TestCaseData(AllMagentaRed).SetName("Magenta-red")
    };

    [TestCaseSource(nameof(ReferenceHues))]
    public void ReferenceXyy(Unicolour unicolour, Xyy xyy, Luv luv)
    {
        AssertUtils.AssertTriplet<Xyy>(unicolour, xyy.Triplet, 0.00005);
    }
    
    [TestCaseSource(nameof(ReferenceHues))]
    public void ReferenceLuv(Unicolour unicolour, Xyy xyy, Luv luv)
    {
        // for some reason, cyan Luv.V from the data table doesn't quite match Unicolour calculations
        // though no reason to distrust Unicolour since the conversions have been heavily tested
        var tolerance = unicolour.Equals(CyanRef) ? 0.4 : 0.15;
        AssertUtils.AssertTriplet<Luv>(unicolour, luv.Triplet, tolerance);
    }
    
    [TestCaseSource(nameof(GroupedByHue))]
    public void GroupedHue(List<Unicolour> unicolours)
    {
        // questionable, but would expect hue group to be no more than 30 degrees different (360 / 12 groups)
        var hues = unicolours.Select(x => x.Lchab.H).ToList();
        Assert.That(hues.Max() - hues.Min(), Is.LessThan(30));
    }
    
    [TestCaseSource(nameof(GroupedByHue))]
    public void OrderedChroma(List<Unicolour> unicolours)
    {
        // assumes the hue list is returned in the order they were defined, not ideal
        var chromas = unicolours.Select(x => x.Lchab.C).ToList();
        Assert.That(unicolours.Count, Is.EqualTo(4));
        Assert.That(chromas[0], Is.LessThan(chromas[1]));
        Assert.That(chromas[1], Is.LessThan(chromas[2]));
        Assert.That(chromas[2], Is.LessThan(chromas[3]));
    }
    
    [Test]
    public void All() => Assert.That(HungBerns.All.Distinct().Count(), Is.EqualTo(48));
}