using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class PigmentTests
{
    private static readonly double[][] r =
    [
        [1.00, 0.50, 1.00, 0.50, 1.00],
        [1.00, 1.00, 0.25, 0.50, 0.50],
        [1.00, 1.00, 0.75, 0.25, 0.25]
    ];
    
    // calculated independently
    private static readonly List<TestCaseData> SingleConstantData =
    [
        new(1.0, 0.0, 0.0, r[0]),
        new(0.0, 1.0, 0.0, r[1]),
        new(0.0, 0.0, 1.0, r[2]),
        new(0.5, 0.5, 0.0, new[] { 1.000000000000000, 0.609611796797792, 0.361914205481341, 0.500000000000000, 0.609611796797792 }),
        new(0.5, 0.0, 0.5, new[] { 1.000000000000000, 0.609611796797792, 0.815648795795914, 0.328214801816778, 0.361914205481341 }),
        new(0.0, 0.5, 0.5, new[] { 1.000000000000000, 1.000000000000000, 0.355756678111981, 0.328214801816778, 0.328214801816778 }),
        new(0.5, 0.3, 0.2, new[] { 1.000000000000000, 0.609611796797792, 0.445129519024150, 0.409802974787652, 0.469337613708193 }),
        new(0.2, 0.5, 0.3, new[] { 1.000000000000000, 0.729843788128358, 0.358190647636204, 0.377750578321363, 0.395304493075425 }),
        new(0.3, 0.2, 0.5, new[] { 1.000000000000000, 0.680506654048513, 0.502798546500009, 0.328214801816778, 0.347527174204916 })
    ];
    
    [TestCaseSource(nameof(SingleConstantData))]
    public void SingleConstantKubelkaMunk(double c1, double c2, double c3, double[] expected)
    {
        Pigment pigment1 = new(400, 10, r[0]);
        Pigment pigment2 = new(400, 10, r[1]);
        Pigment pigment3 = new(400, 10, r[2]);
        Pigment[] pigments = [pigment1, pigment2, pigment3];
        double[] concentrations = [c1, c2, c3];
        AssertReflectance(pigments, concentrations, expected, expectedXyzNaN: false);
    }
    
    [Test]
    public void SingleConstantKubelkaMunkWithZero()
    {
        // R of 0 would result in divide-by-zero when calculating K/S
        Pigment pigment = new(400, 10, [0.2, 0.4, 0.0, 0.8, 1.0]);
        Pigment[] pigments = [pigment];
        double[] concentrations = [1.0];
        AssertReflectance(pigments, concentrations, [0.2, 0.4, double.NaN, 0.8, 1.0], expectedXyzNaN: true);
    }
    
    private static readonly double[][] k =
    [
        [1.00, 0.50, 1.00, 0.50, 1.00],
        [1.00, 1.00, 0.25, 0.50, 0.50],
        [1.00, 1.00, 0.75, 0.25, 0.25]
    ];
    
    private static readonly double[][] s =
    [
        [0.20, 0.40, 0.60, 0.80, 1.00],
        [1.00, 0.80, 0.60, 0.40, 0.20],
        [0.20, 0.80, 0.20, 0.80, 0.20]
    ];
    
    // calculated independently
    private static readonly List<TestCaseData> TwoConstantData =
    [
        new(1.0, 0.0, 0.0, new[] { 0.083920216900384, 0.234435562925363, 0.194600504301446, 0.344131154255050, 0.267949192431123 }),
        new(0.0, 1.0, 0.0, new[] { 0.267949192431123, 0.234435562925363, 0.413200451767309, 0.234435562925363, 0.145898033750315 }),
        new(0.0, 0.0, 1.0, new[] { 0.083920216900384, 0.234435562925363, 0.106456094748323, 0.462408093204035, 0.234435562925363 }),
        new(0.5, 0.5, 0.0, new[] { 0.194600504301446, 0.234435562925363, 0.261665886392181, 0.296742590451186, 0.234435562925363 }),
        new(0.5, 0.0, 0.5, new[] { 0.083920216900384, 0.234435562925363, 0.160924996799187, 0.393005345121343, 0.261665886392181 }),
        new(0.0, 0.5, 0.5, new[] { 0.194600504301446, 0.234435562925363, 0.234435562925363, 0.344131154255050, 0.179517668394022 }),
        new(0.5, 0.3, 0.2, new[] { 0.156520636181344, 0.234435562925363, 0.218836327679683, 0.334563915603707, 0.244572900888201 }),
        new(0.2, 0.5, 0.3, new[] { 0.194600504301446, 0.234435562925363, 0.247254734895954, 0.323269155117111, 0.212581170298022 }),
        new(0.3, 0.2, 0.5, new[] { 0.134756375387777, 0.234435562925363, 0.183782394398364, 0.375000000000000, 0.241247780463508 })
    ];
    
    private const double k1 = 0.033;
    private const double k2 = 0.66;
    
    // calculated independently
    private static readonly List<TestCaseData> TwoConstantCorrectedData =
    [
        new(1.0, 0.0, 0.0, new[] { 0.029209103554298, 0.091186832391423, 0.073409156685747, 0.146393232455631, 0.107022969538771 }),
        new(0.0, 1.0, 0.0, new[] { 0.107022969538771, 0.091186832391423, 0.186792715165618, 0.091186832391423, 0.053079526586978 }),
        new(0.0, 0.0, 1.0, new[] { 0.029209103554298, 0.091186832391423, 0.037645657200200, 0.218808579037984, 0.091186832391423 }),
        new(0.5, 0.5, 0.0, new[] { 0.073409156685747, 0.091186832391423, 0.103989431373653, 0.121324432254632, 0.091186832391423 }),
        new(0.5, 0.0, 0.5, new[] { 0.029209103554298, 0.091186832391423, 0.059196175746731, 0.174465870279250, 0.103989431373653 }),
        new(0.0, 0.5, 0.5, new[] { 0.073409156685747, 0.091186832391423, 0.091186832391423, 0.146393232455631, 0.066954726195394 }),
        new(0.5, 0.3, 0.2, new[] { 0.057389386089411, 0.091186832391423, 0.084095017367215, 0.141169974626911, 0.095888880683781 }),
        new(0.2, 0.5, 0.3, new[] { 0.073409156685747, 0.091186832391423, 0.097145384967362, 0.135111504979980, 0.081298973598661 }),
        new(0.3, 0.2, 0.5, new[] { 0.048630345501823, 0.091186832391423, 0.068764910347273, 0.163843853820598, 0.094338327216122 })
    ];
    
    [TestCaseSource(nameof(TwoConstantData))]
    public void TwoConstantKubelkaMunk(double c1, double c2, double c3, double[] expected)
    {
        Pigment pigment1 = new(400, 10, k[0], s[0]);
        Pigment pigment2 = new(400, 10, k[1], s[1]);
        Pigment pigment3 = new(400, 10, k[2], s[2]);
        Pigment[] pigments = [pigment1, pigment2, pigment3];
        double[] concentrations = [c1, c2, c3];
        AssertReflectance(pigments, concentrations, expected, expectedXyzNaN: false);
    }
    
    [TestCaseSource(nameof(TwoConstantCorrectedData))]
    public void TwoConstantKubelkaMunkCorrected(double c1, double c2, double c3, double[] expected)
    {
        Pigment pigment1 = new(400, 10, k[0], s[0], k1, k2);
        Pigment pigment2 = new(400, 10, k[1], s[1], k1, k2);
        Pigment pigment3 = new(400, 10, k[2], s[2], k1, k2);
        Pigment[] pigments = [pigment1, pigment2, pigment3];
        double[] concentrations = [c1, c2, c3];
        AssertReflectance(pigments, concentrations, expected, expectedXyzNaN: false);
    }
    
    [Test]
    public void TwoConstantKubelkaMunkWithZero()
    {
        // S of 0 would result in divide-by-zero when calculating R
        Pigment pigment = new(400, 10, [0.2, 0.4, 0.6, 0.8, 1.0], [0.2, 0.4, 0.0, 0.8, 1.0]);
        Pigment[] pigments = [pigment];
        double[] concentrations = [1.0];
        const double expectedR = 0.267949192431123; // when K/S = 1
        AssertReflectance(pigments, concentrations, [expectedR, expectedR, double.NaN, expectedR, expectedR], expectedXyzNaN: true);
    }
    
    [Test]
    public void MismatchWavelength()
    {
        // pigment1 is missing wavelength 460, pigment2 is missing wavelength 400
        Pigment pigment1 = new(400, 10, [0.5, 0.5, 0.5, 0.5, 0.5], [0.5, 0.5, 0.5, 0.5, 0.5]);
        Pigment pigment2 = new(410, 10, [0.5, 0.5, 0.5, 0.5, 0.5], [0.5, 0.5, 0.5, 0.5, 0.5]);
        Pigment[] pigments = [pigment1, pigment2];
        double[] concentrations = [0.5, 0.5];
        AssertReflectance(pigments, concentrations, expected: null, expectedXyzNaN: true);
    }
    
    [Test]
    public void MismatchKubelkaMunk()
    {
        // pigment1 is single-constant, pigment2 is two-constant
        Pigment pigment1 = new(400, 10, [0.5, 0.5, 0.5, 0.5, 0.5]);
        Pigment pigment2 = new(400, 10, [0.5, 0.5, 0.5, 0.5, 0.5], [0.5, 0.5, 0.5, 0.5, 0.5]);
        Pigment[] pigments = [pigment1, pigment2];
        double[] concentrations = [0.5, 0.5];
        AssertReflectance(pigments, concentrations, expected: null, expectedXyzNaN: true);
    }
    
    [Test]
    public void MismatchCorrectionK1()
    {
        // pigments have different K1 correction constants
        Pigment pigment1 = new(400, 10, [0.5, 0.5, 0.5, 0.5, 0.5], [0.5, 0.5, 0.5, 0.5, 0.5], k1, k2);
        Pigment pigment2 = new(400, 10, [0.5, 0.5, 0.5, 0.5, 0.5], [0.5, 0.5, 0.5, 0.5, 0.5], k1 + 0.0001, k2);
        Pigment[] pigments = [pigment1, pigment2];
        double[] concentrations = [0.5, 0.5];
        AssertReflectance(pigments, concentrations, expected: null, expectedXyzNaN: true);
    }
    
    [Test]
    public void MismatchCorrectionK2()
    {
        // pigments have different K2 correction constants
        Pigment pigment1 = new(400, 10, [0.5, 0.5, 0.5, 0.5, 0.5], [0.5, 0.5, 0.5, 0.5, 0.5], k1, k2);
        Pigment pigment2 = new(400, 10, [0.5, 0.5, 0.5, 0.5, 0.5], [0.5, 0.5, 0.5, 0.5, 0.5], k1, k2 + 0.0001);
        Pigment[] pigments = [pigment1, pigment2];
        double[] concentrations = [0.5, 0.5];
        AssertReflectance(pigments, concentrations, expected: null, expectedXyzNaN: true);
    }
    
    [Test]
    public void NoConcentration()
    {
        Pigment pigment1 = new(400, 10, [0.5, 0.5, 0.5, 0.5, 0.5], [0.5, 0.5, 0.5, 0.5, 0.5], k1, k2);
        Pigment pigment2 = new(400, 10, [0.5, 0.5, 0.5, 0.5, 0.5], [0.5, 0.5, 0.5, 0.5, 0.5], k1, k2);
        Pigment[] pigments = [pigment1, pigment2];
        double[] concentrations = [-0.5, 0.0];
        AssertReflectance(pigments, concentrations, expected: null, expectedXyzNaN: true);
    }

    private static readonly Configuration configWithIlluminantSpd = new(xyzConfig: XyzConfiguration.D50); // contains D50 SPD (as well as precalculated D65 white point)
    private static readonly Configuration configWithoutIlluminantSpd = new(xyzConfig: new(new WhitePoint(96.422, 100.000, 82.521))); // D50 white point only
    
    private static void AssertReflectance(Pigment[] pigments, double[] concentrations, double[]? expected, bool expectedXyzNaN)
    {
        var reflectance = Pigment.GetReflectance(pigments, concentrations)!;

        if (expected == null)
        {
            Assert.That(reflectance, Is.Null);
        }
        else
        {
            for (var i = 0; i < reflectance.Coefficients.Length; i++)
            {
                Assert.That(reflectance.Coefficients[i], Is.EqualTo(expected[i]).Within(1e-15));
            }
        }
        
        var colourFromIlluminantSpd = new Unicolour(configWithIlluminantSpd, pigments, concentrations); // will calculate XYZ using D50 SPD directly
        var colourFromWhitePoint = new Unicolour(configWithoutIlluminantSpd, pigments, concentrations); // will calculate XYZ using default D65 SPD, and then adapt white point
        Assert.That(colourFromIlluminantSpd.Xyz.X, expectedXyzNaN ? Is.NaN : Is.Not.NaN);
        Assert.That(colourFromIlluminantSpd.Xyz.Y, expectedXyzNaN ? Is.NaN : Is.Not.NaN);
        Assert.That(colourFromIlluminantSpd.Xyz.Z, expectedXyzNaN ? Is.NaN : Is.Not.NaN);
        Assert.That(colourFromWhitePoint.Xyz.X, expectedXyzNaN ? Is.NaN : Is.Not.NaN);
        Assert.That(colourFromWhitePoint.Xyz.Y, expectedXyzNaN ? Is.NaN : Is.Not.NaN);
        Assert.That(colourFromWhitePoint.Xyz.Z, expectedXyzNaN ? Is.NaN : Is.Not.NaN);
        TestUtils.AssertTriplet(colourFromWhitePoint.Xyz.Triplet, colourFromIlluminantSpd.Xyz.Triplet, 0.0075);
    }
}