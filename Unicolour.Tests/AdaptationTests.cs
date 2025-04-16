using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

/*
 * expected matrix values for these tests from
 * http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
 */
public class AdaptationTests
{
    private static readonly TestCaseData[] D65ToD50TestData =
    [
        new TestCaseData(Adaptation.XyzScaling, new[,]
        {
            { 1.0144665, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 0.7578869 }
        }).SetName(nameof(Adaptation.XyzScaling)),

        new TestCaseData(Adaptation.Bradford, new[,]
        {
            { 1.0478112, 0.0228866, -0.0501270 },
            { 0.0295424, 0.9904844, -0.0170491 },
            { -0.0092345, 0.0150436, 0.7521316 }
        }).SetName(nameof(Adaptation.Bradford)),

        new TestCaseData(Adaptation.VonKries, new[,]
        {
            { 1.0160803, 0.0552297, -0.0521326 },
            { 0.0060666, 0.9955661, -0.0012235 },
            { 0.0000000, 0.0000000, 0.7578869 }
        }).SetName(nameof(Adaptation.VonKries))
    ];

    [TestCaseSource(nameof(D65ToD50TestData))]
    public void D65ToD50(double[,] adaptation, double[,] expected) => AssertM(Illuminant.D65, Illuminant.D50, adaptation, expected);
    
    private static readonly TestCaseData[] D50ToD65TestData =
    [
        new TestCaseData(Adaptation.XyzScaling, new[,]
        {
            { 0.9857398, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 1.3194581 }
        }).SetName(nameof(Adaptation.XyzScaling)),

        new TestCaseData(Adaptation.Bradford, new[,]
        {
            { 0.9555766, -0.0230393, 0.0631636 },
            { -0.0282895, 1.0099416, 0.0210077 },
            { 0.0122982, -0.0204830, 1.3299098 }
        }).SetName(nameof(Adaptation.Bradford)),

        new TestCaseData(Adaptation.VonKries, new[,]
        {
            { 0.9845002, -0.0546158, 0.0676324 },
            { -0.0059992, 1.0047864, 0.0012095 },
            { 0.0000000, 0.0000000, 1.3194581 },
        }).SetName(nameof(Adaptation.VonKries))
    ];

    [TestCaseSource(nameof(D50ToD65TestData))]
    public void D50ToD65(double[,] adaptation, double[,] expected) => AssertM(Illuminant.D50, Illuminant.D65, adaptation, expected);

    private static readonly TestCaseData[] CToETestData =
    [
        new TestCaseData(Adaptation.XyzScaling, new[,]
        {
            { 1.0196382, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 0.8457947 }
        }).SetName(nameof(Adaptation.XyzScaling)),

        new TestCaseData(Adaptation.Bradford, new[,]
        {
            { 1.0399770, 0.0198119, -0.0336279 },
            { 0.0266883, 0.9877806, -0.0118030 },
            { -0.0056861, 0.0089182, 0.8429683 }
        }).SetName(nameof(Adaptation.Bradford)),

        new TestCaseData(Adaptation.VonKries, new[,]
        {
            { 1.0133781, 0.0461460, -0.0338372 },
            { 0.0050688, 0.9962378, -0.0010226 },
            { 0.0000000, 0.0000000, 0.8457947 }
        }).SetName(nameof(Adaptation.VonKries))
    ];

    [TestCaseSource(nameof(CToETestData))]
    public void CToE(double[,] adaptation, double[,] expected) => AssertM(Illuminant.C, Illuminant.E, adaptation, expected);

    private static readonly TestCaseData[] EToCTestData =
    [
        new TestCaseData(Adaptation.XyzScaling, new[,]
        {
            { 0.9807400, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 1.1823200 }
        }).SetName(nameof(Adaptation.XyzScaling)),

        new TestCaseData(Adaptation.Bradford, new[,]
        {
            { 0.9622722, -0.0196444, 0.0381122 },
            { -0.0259182, 1.0127717, 0.0131466 },
            { 0.0067650, -0.0108472, 1.1864022 }
        }).SetName(nameof(Adaptation.Bradford)),

        new TestCaseData(Adaptation.VonKries, new[,]
        {
            { 0.9870272, -0.0457193, 0.0394321 },
            { -0.0050219, 1.0040090, 0.0010130 },
            { 0.0000000, 0.0000000, 1.1823200 }
        }).SetName(nameof(Adaptation.VonKries))
    ];

    [TestCaseSource(nameof(EToCTestData))]
    public void EToC(double[,] adaptation, double[,] expected) => AssertM(Illuminant.E, Illuminant.C, adaptation, expected);

    private static readonly TestCaseData[] AToF7TestData =
    [
        new TestCaseData(Adaptation.XyzScaling, new[,]
        {
            { 0.8651889, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 3.0559786 }
        }).SetName(nameof(Adaptation.XyzScaling)),

        new TestCaseData(Adaptation.Bradford, new[,]
        {
            { 0.8447932, -0.1178395, 0.3941104 },
            { -0.1365823, 1.1041477, 0.1289531 },
            { 0.0796929, -0.1346275, 3.1882950 }
        }).SetName(nameof(Adaptation.Bradford)),

        new TestCaseData(Adaptation.VonKries, new[,]
        {
            { 0.9395426, -0.2337454, 0.4273371 },
            { -0.0256753, 1.0263637, 0.0051723 },
            { 0.0000000, 0.0000000, 3.0559786 }
        }).SetName(nameof(Adaptation.VonKries))
    ];

    [TestCaseSource(nameof(AToF7TestData))]
    public void AToF7(double[,] adaptation, double[,] expected) => AssertM(Illuminant.A, Illuminant.F7, adaptation, expected);

    private static readonly TestCaseData[] F7ToATestData =
    [
        new TestCaseData(Adaptation.XyzScaling, new[,]
        {
            { 1.1558170, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 0.3272274 }
        }).SetName(nameof(Adaptation.XyzScaling)),

        new TestCaseData(Adaptation.Bradford, new[,]
        {
            { 1.2162616, 0.1109265, -0.1548306 },
            { 0.1532455, 0.9152079, -0.0559592 },
            { -0.0239302, 0.0358725, 0.3151544 }
        }).SetName(nameof(Adaptation.Bradford)),

        new TestCaseData(Adaptation.VonKries, new[,]
        {
            { 1.0710133, 0.2439140, -0.1501795 },
            { 0.0267922, 0.9804152, -0.0054059 },
            { 0.0000000, 0.0000000, 0.3272274 }
        }).SetName(nameof(Adaptation.VonKries))
    ];

    [TestCaseSource(nameof(F7ToATestData))]
    public void F7ToA(double[,] adaptation, double[,] expected) => AssertM(Illuminant.F7, Illuminant.A, adaptation, expected);
    
    private static readonly double[,] Identity = new[,]
    {
        { 1.0000000, 0.0000000, 0.0000000 },
        { 0.0000000, 1.0000000, 0.0000000 },
        { 0.0000000, 0.0000000, 1.0000000 }
    };

    [Test]
    public void SameIlluminant(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant illuminant,
        [Values(nameof(Adaptation.Bradford), nameof(Adaptation.VonKries), nameof(Adaptation.XyzScaling))] string adaptationName)
    {
        var adaptation = adaptationName switch
        {
            nameof(Adaptation.Bradford) => Adaptation.Bradford,
            nameof(Adaptation.VonKries) => Adaptation.VonKries,
            nameof(Adaptation.XyzScaling) => Adaptation.XyzScaling,
            _ => throw new ArgumentOutOfRangeException(nameof(adaptationName), adaptationName, null)
        };

        AssertM(illuminant, illuminant, adaptation, Identity);
    }
    
    [Test]
    public void Zeroes()
    {
        var zeroes = new[,]
        {
            { 0.0000000, 0.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 0.0000000 }
        };

        var notNumbers = new[,]
        {
            { double.NaN, double.NaN, double.NaN },
            { double.NaN, double.NaN, double.NaN },
            { double.NaN, double.NaN, double.NaN }
        };

        // illuminant doesn't matter - zero values result in divide by zero
        var sourceIlluminant = Illuminant.D65;
        var targetIlluminant = Illuminant.D50;
        AssertM(sourceIlluminant, targetIlluminant, zeroes, notNumbers);

        var originalXyz = new Xyz(0.5, 0.5, 0.5);
        var adaptedXyz = Adaptation.WhitePoint(originalXyz, sourceIlluminant.GetWhitePoint(Observer.Degree2), targetIlluminant.GetWhitePoint(Observer.Degree2), new Matrix(zeroes));
        TestUtils.AssertTriplet(adaptedXyz.Triplet, new(double.NaN, double.NaN, double.NaN), 0);
    }

    private static void AssertM(Illuminant source, Illuminant target, double[,] adaptation, double[,] expected)
    {
        var sourceWhitePoint = source.GetWhitePoint(Observer.Degree2);
        var targetWhitePoint = target.GetWhitePoint(Observer.Degree2);
        var m = Adaptation.M(sourceWhitePoint, targetWhitePoint, new Matrix(adaptation));
        Assert.That(m.Data, Is.EqualTo(expected).Within(0.00000005));
    }
}