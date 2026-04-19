using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

/*
 * expected matrix values for these tests from
 * http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
 */
public class ChromaticAdaptationTests
{
    private static readonly TestCaseData[] D65ToD50TestData =
    [
        new TestCaseData(ChromaticAdaptation.XyzScaling, new[,]
        {
            { 1.0144665, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 0.7578869 }
        }).SetName(nameof(ChromaticAdaptation.XyzScaling)),

        new TestCaseData(ChromaticAdaptation.Bradford, new[,]
        {
            { 1.0478112, 0.0228866, -0.0501270 },
            { 0.0295424, 0.9904844, -0.0170491 },
            { -0.0092345, 0.0150436, 0.7521316 }
        }).SetName(nameof(ChromaticAdaptation.Bradford)),

        new TestCaseData(ChromaticAdaptation.VonKries, new[,]
        {
            { 1.0160803, 0.0552297, -0.0521326 },
            { 0.0060666, 0.9955661, -0.0012235 },
            { 0.0000000, 0.0000000, 0.7578869 }
        }).SetName(nameof(ChromaticAdaptation.VonKries))
    ];

    [TestCaseSource(nameof(D65ToD50TestData))]
    public void D65ToD50(ChromaticAdaptation chromaticAdaptation, double[,] expected) => AssertTransformMatrix(Illuminant.D65, Illuminant.D50, chromaticAdaptation, expected);
    
    private static readonly TestCaseData[] D50ToD65TestData =
    [
        new TestCaseData(ChromaticAdaptation.XyzScaling, new[,]
        {
            { 0.9857398, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 1.3194581 }
        }).SetName(nameof(ChromaticAdaptation.XyzScaling)),

        new TestCaseData(ChromaticAdaptation.Bradford, new[,]
        {
            { 0.9555766, -0.0230393, 0.0631636 },
            { -0.0282895, 1.0099416, 0.0210077 },
            { 0.0122982, -0.0204830, 1.3299098 }
        }).SetName(nameof(ChromaticAdaptation.Bradford)),

        new TestCaseData(ChromaticAdaptation.VonKries, new[,]
        {
            { 0.9845002, -0.0546158, 0.0676324 },
            { -0.0059992, 1.0047864, 0.0012095 },
            { 0.0000000, 0.0000000, 1.3194581 }
        }).SetName(nameof(ChromaticAdaptation.VonKries))
    ];

    [TestCaseSource(nameof(D50ToD65TestData))]
    public void D50ToD65(ChromaticAdaptation chromaticAdaptation, double[,] expected) => AssertTransformMatrix(Illuminant.D50, Illuminant.D65, chromaticAdaptation, expected);

    private static readonly TestCaseData[] CToETestData =
    [
        new TestCaseData(ChromaticAdaptation.XyzScaling, new[,]
        {
            { 1.0196382, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 0.8457947 }
        }).SetName(nameof(ChromaticAdaptation.XyzScaling)),

        new TestCaseData(ChromaticAdaptation.Bradford, new[,]
        {
            { 1.0399770, 0.0198119, -0.0336279 },
            { 0.0266883, 0.9877806, -0.0118030 },
            { -0.0056861, 0.0089182, 0.8429683 }
        }).SetName(nameof(ChromaticAdaptation.Bradford)),

        new TestCaseData(ChromaticAdaptation.VonKries, new[,]
        {
            { 1.0133781, 0.0461460, -0.0338372 },
            { 0.0050688, 0.9962378, -0.0010226 },
            { 0.0000000, 0.0000000, 0.8457947 }
        }).SetName(nameof(ChromaticAdaptation.VonKries))
    ];

    [TestCaseSource(nameof(CToETestData))]
    public void CToE(ChromaticAdaptation chromaticAdaptation, double[,] expected) => AssertTransformMatrix(Illuminant.C, Illuminant.E, chromaticAdaptation, expected);

    private static readonly TestCaseData[] EToCTestData =
    [
        new TestCaseData(ChromaticAdaptation.XyzScaling, new[,]
        {
            { 0.9807400, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 1.1823200 }
        }).SetName(nameof(ChromaticAdaptation.XyzScaling)),

        new TestCaseData(ChromaticAdaptation.Bradford, new[,]
        {
            { 0.9622722, -0.0196444, 0.0381122 },
            { -0.0259182, 1.0127717, 0.0131466 },
            { 0.0067650, -0.0108472, 1.1864022 }
        }).SetName(nameof(ChromaticAdaptation.Bradford)),

        new TestCaseData(ChromaticAdaptation.VonKries, new[,]
        {
            { 0.9870272, -0.0457193, 0.0394321 },
            { -0.0050219, 1.0040090, 0.0010130 },
            { 0.0000000, 0.0000000, 1.1823200 }
        }).SetName(nameof(ChromaticAdaptation.VonKries))
    ];

    [TestCaseSource(nameof(EToCTestData))]
    public void EToC(ChromaticAdaptation chromaticAdaptation, double[,] expected) => AssertTransformMatrix(Illuminant.E, Illuminant.C, chromaticAdaptation, expected);

    private static readonly TestCaseData[] AToF7TestData =
    [
        new TestCaseData(ChromaticAdaptation.XyzScaling, new[,]
        {
            { 0.8651889, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 3.0559786 }
        }).SetName(nameof(ChromaticAdaptation.XyzScaling)),

        new TestCaseData(ChromaticAdaptation.Bradford, new[,]
        {
            { 0.8447932, -0.1178395, 0.3941104 },
            { -0.1365823, 1.1041477, 0.1289531 },
            { 0.0796929, -0.1346275, 3.1882950 }
        }).SetName(nameof(ChromaticAdaptation.Bradford)),

        new TestCaseData(ChromaticAdaptation.VonKries, new[,]
        {
            { 0.9395426, -0.2337454, 0.4273371 },
            { -0.0256753, 1.0263637, 0.0051723 },
            { 0.0000000, 0.0000000, 3.0559786 }
        }).SetName(nameof(ChromaticAdaptation.VonKries))
    ];

    [TestCaseSource(nameof(AToF7TestData))]
    public void AToF7(ChromaticAdaptation chromaticAdaptation, double[,] expected) => AssertTransformMatrix(Illuminant.A, Illuminant.F7, chromaticAdaptation, expected);

    private static readonly TestCaseData[] F7ToATestData =
    [
        new TestCaseData(ChromaticAdaptation.XyzScaling, new[,]
        {
            { 1.1558170, 0.0000000, 0.0000000 },
            { 0.0000000, 1.0000000, 0.0000000 },
            { 0.0000000, 0.0000000, 0.3272274 }
        }).SetName(nameof(ChromaticAdaptation.XyzScaling)),

        new TestCaseData(ChromaticAdaptation.Bradford, new[,]
        {
            { 1.2162616, 0.1109265, -0.1548306 },
            { 0.1532455, 0.9152079, -0.0559592 },
            { -0.0239302, 0.0358725, 0.3151544 }
        }).SetName(nameof(ChromaticAdaptation.Bradford)),

        new TestCaseData(ChromaticAdaptation.VonKries, new[,]
        {
            { 1.0710133, 0.2439140, -0.1501795 },
            { 0.0267922, 0.9804152, -0.0054059 },
            { 0.0000000, 0.0000000, 0.3272274 }
        }).SetName(nameof(ChromaticAdaptation.VonKries))
    ];

    [TestCaseSource(nameof(F7ToATestData))]
    public void F7ToA(ChromaticAdaptation chromaticAdaptation, double[,] expected) => AssertTransformMatrix(Illuminant.F7, Illuminant.A, chromaticAdaptation, expected);
    
    private static readonly double[,] Identity = new[,]
    {
        { 1.0000000, 0.0000000, 0.0000000 },
        { 0.0000000, 1.0000000, 0.0000000 },
        { 0.0000000, 0.0000000, 1.0000000 }
    };

    [Test]
    public void SameIlluminant(
        [ValueSource(typeof(TestUtils), nameof(TestUtils.AllIlluminants))] Illuminant illuminant,
        [Values(nameof(ChromaticAdaptation.Bradford), nameof(ChromaticAdaptation.VonKries), nameof(ChromaticAdaptation.XyzScaling))] string adaptationName)
    {
        var adaptation = adaptationName switch
        {
            nameof(ChromaticAdaptation.Bradford) => ChromaticAdaptation.Bradford,
            nameof(ChromaticAdaptation.VonKries) => ChromaticAdaptation.VonKries,
            nameof(ChromaticAdaptation.XyzScaling) => ChromaticAdaptation.XyzScaling,
            _ => throw new ArgumentOutOfRangeException(nameof(adaptationName), adaptationName, null)
        };

        AssertTransformMatrix(illuminant, illuminant, adaptation, Identity);
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

        var expected = new[,]
        {
            { double.NaN, double.NaN, double.NaN },
            { double.NaN, double.NaN, double.NaN },
            { double.NaN, double.NaN, double.NaN }
        };

        // illuminant doesn't matter - zero values result in divide by zero
        var sourceIlluminant = Illuminant.D65;
        var targetIlluminant = Illuminant.D50;
        var chromaticAdaptation = new ChromaticAdaptation(zeroes);
        AssertTransformMatrix(sourceIlluminant, targetIlluminant, chromaticAdaptation, expected);

        var sourceWhitePoint = sourceIlluminant.GetWhitePoint(Observer.Degree2);
        var targetWhitePoint = targetIlluminant.GetWhitePoint(Observer.Degree2);
        var chromaticAdaptor = new ChromaticAdaptor(sourceWhitePoint, chromaticAdaptation);
        var originalXyz = new Xyz(0.5, 0.5, 0.5, sourceWhitePoint);
        var adaptedXyz = chromaticAdaptor.AdaptTo(originalXyz, targetWhitePoint);
        TestUtils.AssertTriplet(adaptedXyz.Triplet, new(double.NaN, double.NaN, double.NaN), 0);
    }
    
    private static void AssertTransformMatrix(Illuminant source, Illuminant target, ChromaticAdaptation chromaticAdaptation, double[,] expected)
    {
        var sourceWhitePoint = source.GetWhitePoint(Observer.Degree2);
        var targetWhitePoint = target.GetWhitePoint(Observer.Degree2);
        var transformMatrix = chromaticAdaptation.GetTransformMatrix(sourceWhitePoint, targetWhitePoint);
        Assert.That(transformMatrix.Data, Is.EqualTo(expected).Within(0.00000005));
    }
    
    [Test]
    public void NoData() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new double[,] {},
        expectedAdaptation: new[,]
        {
            { double.NaN, double.NaN, double.NaN },
            { double.NaN, double.NaN, double.NaN },
            { double.NaN, double.NaN, double.NaN }
        }
    );

    [Test]
    public void MissingRow() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { double.NaN, double.NaN, double.NaN }
        }
    );
    
    [Test]
    public void MissingColumn() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0 },
            { 0.0, 2.0 },
            { 0.0, 0.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, double.NaN },
            { 0.0, 2.0, double.NaN },
            { 0.0, 0.0, double.NaN }
        }
    );
    
    [Test]
    public void ExtraRow() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { 0.0, 0.0, 3.0 },
            { 9.0, 9.0, 9.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { 0.0, 0.0, 3.0 }
        }
    );
    
    [Test]
    public void ExtraColumn() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0, 9.0 },
            { 0.0, 2.0, 0.0, 9.0 },
            { 0.0, 0.0, 3.0, 9.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { 0.0, 0.0, 3.0 }
        }
    );
    
    [Test]
    public void ExtraRowAndColumn() => AssertInvalidChromaticAdaptation(
        invalidAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0, 9.0 },
            { 0.0, 2.0, 0.0, 9.0 },
            { 0.0, 0.0, 3.0, 9.0 },
            { 9.0, 9.0, 9.0, 9.0 }
        },
        expectedAdaptation: new[,]
        {
            { 1.0, 0.0, 0.0 },
            { 0.0, 2.0, 0.0 },
            { 0.0, 0.0, 3.0 }
        }
    );

    private static void AssertInvalidChromaticAdaptation(double[,] invalidAdaptation, double[,] expectedAdaptation)
    {
        var observer = Observer.Degree2;
        var sourceIlluminant = Illuminant.D65;
        var targetIlluminant = Illuminant.D50;
        var chromaticAdaptation = new ChromaticAdaptation(invalidAdaptation);
        Assert.That(chromaticAdaptation.AdaptationMatrix.Data, Is.EqualTo(expectedAdaptation));

        var sourceWhitePoint = sourceIlluminant.GetWhitePoint(observer);
        var targetWhitePoint = targetIlluminant.GetWhitePoint(observer);
        var actualChromaticAdaptor = new ChromaticAdaptor(sourceWhitePoint, chromaticAdaptation);
        var expectedChromaticAdaptor = new ChromaticAdaptor(sourceWhitePoint, new ChromaticAdaptation(expectedAdaptation));
        
        var originalXyz = new Xyz(0.5, 0.5, 0.5, sourceWhitePoint);
        var adaptedXyz = actualChromaticAdaptor.AdaptTo(originalXyz, targetWhitePoint);
        var expectedXyz = expectedChromaticAdaptor.AdaptTo(originalXyz, targetWhitePoint);
        TestUtils.AssertTriplet(adaptedXyz.Triplet, expectedXyz.Triplet, 0);
    }
}