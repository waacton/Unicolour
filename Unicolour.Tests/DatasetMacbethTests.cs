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
    
    // https://poynton.ca/notes/color/GretagMacbeth-ColorChecker.html
    private static readonly List<TestCaseData> XyyTestData = new()
    {
        new TestCaseData(DarkSkin, 0.400, 0.350, 10.1).SetName("Dark skin"),
        new TestCaseData(LightSkin, 0.377, 0.345, 35.8).SetName("Light skin"),
        new TestCaseData(BlueSky, 0.247, 0.251, 19.3).SetName("Blue sky"),
        new TestCaseData(Foliage, 0.337, 0.422, 13.3).SetName("Foliage"),
        new TestCaseData(BlueFlower, 0.265, 0.240, 24.3).SetName("Blue flower"),
        new TestCaseData(BluishGreen, 0.261, 0.343, 43.1).SetName("Bluish green"),
        new TestCaseData(Orange, 0.506, 0.407, 30.1).SetName("Orange"),
        new TestCaseData(PurplishBlue, 0.211, 0.175, 12).SetName("Purplish blue"),
        new TestCaseData(ModerateRed, 0.453, 0.306, 19.8).SetName("Moderate red"),
        new TestCaseData(Purple, 0.285, 0.202, 6.6).SetName("Purple"),
        new TestCaseData(YellowGreen, 0.38, 0.489, 44.3).SetName("Yellow green"),
        new TestCaseData(OrangeYellow, 0.473, 0.438, 43.1).SetName("Orange yellow"),
        new TestCaseData(Blue, 0.187, 0.129, 6.1).SetName("Blue"),
        new TestCaseData(Green, 0.305, 0.478, 23.4).SetName("Green"),
        new TestCaseData(Red, 0.539, 0.313, 12).SetName("Red"),
        new TestCaseData(Yellow, 0.448, 0.47, 59.1).SetName("Yellow"),
        new TestCaseData(Magenta, 0.364, 0.233, 19.8).SetName("Magenta"),
        new TestCaseData(Cyan, 0.196, 0.252, 19.8).SetName("Cyan"),
        new TestCaseData(White, 0.31, 0.316, 90).SetName("White"),
        new TestCaseData(Neutral8, 0.31, 0.316, 59.1).SetName("Neutral 8"),
        new TestCaseData(Neutral65, 0.31, 0.316, 36.2).SetName("Neutral 6.5"),
        new TestCaseData(Neutral5, 0.31, 0.316, 19.8).SetName("Neutral 5"),
        new TestCaseData(Neutral35, 0.31, 0.316, 9).SetName("Neutral 3.5"),
        new TestCaseData(Black, 0.31, 0.316, 3.1).SetName("Black")
    };
    
    [TestCaseSource(nameof(XyyTestData))]
    public void Xyy(Unicolour unicolour, double expectedX, double expectedY, double expectedLuminance)
    {
        var unicolourIlluminantC = unicolour.ConvertToConfiguration(ConfigIlluminantC);
        var expectedXyy = new Xyy(expectedX, expectedY, expectedLuminance / 100.0);
        TestUtils.AssertTriplet<Xyy>(unicolourIlluminantC, expectedXyy.Triplet, 0.02);
    }
    
    [Test]
    public void All() => Assert.That(Macbeth.All.Distinct().Count(), Is.EqualTo(24));
}