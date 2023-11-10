namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;
using static Datasets.Macbeth;

public class DatasetMacbethTests
{
    private static readonly Configuration ConfigIlluminantC = new(xyzConfiguration: new XyzConfiguration(WhitePoint.From(Illuminant.C)));
    
    private static readonly List<TestCaseData> XyyIlluminantC = new()
    {
        new TestCaseData(DarkSkin, new Xyy(0.400, 0.350, 10.1 / 100.0)).SetName("Dark skin"),
        new TestCaseData(LightSkin, new Xyy(0.377, 0.345, 35.8 / 100.0)).SetName("Light skin"),
        new TestCaseData(BlueSky, new Xyy(0.247, 0.251, 19.3 / 100.0)).SetName("Blue sky"),
        new TestCaseData(Foliage, new Xyy(0.337, 0.422, 13.3 / 100.0)).SetName("Foliage"),
        new TestCaseData(BlueFlower, new Xyy(0.265, 0.240, 24.3 / 100.0)).SetName("Blue flower"),
        new TestCaseData(BluishGreen, new Xyy(0.261, 0.343, 43.1 / 100.0)).SetName("Bluish green"),
        new TestCaseData(Orange, new Xyy(0.506, 0.407, 30.1 / 100.0)).SetName("Orange"),
        new TestCaseData(PurplishBlue, new Xyy(0.211, 0.175, 12 / 100.0)).SetName("Purplish blue"),
        new TestCaseData(ModerateRed, new Xyy(0.453, 0.306, 19.8 / 100.0)).SetName("Moderate red"),
        new TestCaseData(Purple, new Xyy(0.285, 0.202, 6.6 / 100.0)).SetName("Purple"),
        new TestCaseData(YellowGreen, new Xyy(0.38, 0.489, 44.3 / 100.0)).SetName("Yellow green"),
        new TestCaseData(OrangeYellow, new Xyy(0.473, 0.438, 43.1 / 100.0)).SetName("Orange yellow"),
        new TestCaseData(Blue, new Xyy(0.187, 0.129, 6.1 / 100.0)).SetName("Blue"),
        new TestCaseData(Green, new Xyy(0.305, 0.478, 23.4 / 100.0)).SetName("Green"),
        new TestCaseData(Red, new Xyy(0.539, 0.313, 12 / 100.0)).SetName("Red"),
        new TestCaseData(Yellow, new Xyy(0.448, 0.47, 59.1 / 100.0)).SetName("Yellow"),
        new TestCaseData(Magenta, new Xyy(0.364, 0.233, 19.8 / 100.0)).SetName("Magenta"),
        new TestCaseData(Cyan, new Xyy(0.196, 0.252, 19.8 / 100.0)).SetName("Cyan"),
        new TestCaseData(White, new Xyy(0.31, 0.316, 90 / 100.0)).SetName("White"),
        new TestCaseData(Neutral8, new Xyy(0.31, 0.316, 59.1 / 100.0)).SetName("Neutral 8"),
        new TestCaseData(Neutral65, new Xyy(0.31, 0.316, 36.2 / 100.0)).SetName("Neutral 6.5"),
        new TestCaseData(Neutral5, new Xyy(0.31, 0.316, 19.8 / 100.0)).SetName("Neutral 5"),
        new TestCaseData(Neutral35, new Xyy(0.31, 0.316, 9 / 100.0)).SetName("Neutral 3.5"),
        new TestCaseData(Black, new Xyy(0.31, 0.316, 3.1 / 100.0)).SetName("Black")
    };
    
    [TestCaseSource(nameof(XyyIlluminantC))]
    public void Test(Unicolour unicolour, Xyy expectedXyy)
    {
        var unicolourIlluminantC = unicolour.ConvertToConfiguration(ConfigIlluminantC);
        TestUtils.AssertTriplet<Xyy>(unicolourIlluminantC, expectedXyy.Triplet, 0.02);
    }
    
    [Test]
    public void All() => Assert.That(Macbeth.All.Distinct().Count(), Is.EqualTo(24));
}