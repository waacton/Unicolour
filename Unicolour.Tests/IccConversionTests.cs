using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

/*
 * test data extracted from iccDEV, the ICC's reference implementation
 * (https://github.com/waacton/iccDEV-ConversionTestData · https://github.com/InternationalColorConsortium/iccDEV)
 * if these values are wrong because the ICC can't write software that conforms to their own complex specification
 * then what chance do I have?
 */
public class IccConversionTests
{
    // ReSharper disable CollectionNeverQueried.Local - used in test case sources by name
    private static readonly List<TestCaseData> ToPcsTestData = [];
    private static readonly List<TestCaseData> ToPcsNoClipTestData = [];
    private static readonly List<TestCaseData> ToDeviceTestData = [];
    private static readonly List<TestCaseData> ToDeviceNoClipTestData = [];
    // ReSharper restore CollectionNeverQueried.Local

    private static List<TestCaseData> UnicolourFromCmykTestData;
    private static List<TestCaseData> UnicolourToCmykTestData;

    static IccConversionTests()
    {
        AddTestData(IccFile.Fogra39);
        AddTestData(IccFile.Fogra55);
        AddTestData(IccFile.Swop2006);
        AddTestData(IccFile.Swop2013);
        AddTestData(IccFile.JapanColor2011);
        AddTestData(IccFile.JapanColor2003);
        AddTestData(IccFile.RommRgb);
        AddTestData(IccFile.StandardRgbV4);
        AddTestData(IccFile.StandardRgbV2);
        AddTestData(IccFile.StandardRgbGreyV4);
        AddTestData(IccFile.D65Xyz);
        AddTestData(IccFile.CxMonitorWeird);
        AddTestData(IccFile.CxCmykProof);
        AddTestData(IccFile.CxScannerGrey); // CxScannerGrey is one-way, to PCS only
        AddTestData(IccFile.HackCxCmykKtrc);

        UnicolourFromCmykTestData = GenerateUnicolourFromCmykTestData();
        UnicolourToCmykTestData = GenerateUnicolourToCmykTestData();
    }
    
    [TestCaseSource(nameof(ToPcsTestData))]
    public void DeviceToPcs(IccTestColour testColour) => DeviceToPcs(testColour, noClip: false);
    
    [TestCaseSource(nameof(ToPcsNoClipTestData))]
    public void DeviceToPcsNoClip(IccTestColour testColour) => DeviceToPcs(testColour, noClip: true);
    
    private void DeviceToPcs(IccTestColour testColour, bool noClip)
    {
        var expected = testColour.Output;
        var pcs = testColour.Profile.Header.Pcs; 
        var deviceValues = testColour.Input;
        var intent = testColour.Intent;
        
        var actual = testColour.Profile.Transform.ToXyz(deviceValues, intent, noClip);
        if (pcs == Signatures.Lab)
        {
            var xyz = new Xyz(actual[0], actual[1], actual[2], Transform.XyzD50.WhitePoint);
            actual = Lab.FromXyz(xyz).Values;
        }
        
        var tolerance = pcs switch
        {
            Signatures.Lab => 0.0001,
            Signatures.Xyz => 0.00000075,
            _ => throw new ArgumentOutOfRangeException(nameof(testColour), testColour, pcs)
        };
        
        Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
    }

    [TestCaseSource(nameof(ToDeviceTestData))]
    public void PcsToDevice(IccTestColour testColour) => PcsToDevice(testColour, noClip: false);
    
    [TestCaseSource(nameof(ToDeviceNoClipTestData))]
    public void PcsToDeviceNoClip(IccTestColour testColour) => PcsToDevice(testColour, noClip: true);
        
    private void PcsToDevice(IccTestColour testColour, bool noClip)
    {
        var expected = testColour.Output;
        var pcs = testColour.Profile.Header.Pcs;
        var device = testColour.Profile.Header.DataColourSpace;
        var intent = testColour.Intent;
        
        var (first, second, third) = (testColour.Input[0], testColour.Input[1], testColour.Input[2]);
        var xyz = pcs == Signatures.Lab
            ? Lab.ToXyz(new Lab(first, second, third), Transform.XyzD50.WhitePoint)
            : new Xyz(first, second, third, Transform.XyzD50.WhitePoint);
        
        var actual = testColour.Profile.Transform.FromXyz(xyz.Values, intent, noClip);
        var tolerance = device switch
        {
            Signatures.Cmyk => 0.00000175,
            Signatures.Clr7 => 0.00000175,
            Signatures.Rgb => 0.000015,
            Signatures.Grey => 0.000005,
            _ => throw new ArgumentOutOfRangeException(nameof(testColour), testColour, device)
        };
        
        Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
    }
    
    [Test] // slightly unusual case; it's possible for a profile to be supported but only have A2B transform
    public void NoReverseTransform()
    {
        var profile = IccFile.CxScannerGrey.GetProfile();
        Assert.DoesNotThrow(() => { profile.ErrorIfUnsupported(); });
        Assert.DoesNotThrow(() => { profile.Transform.ToXyz([], Intent.RelativeColorimetric); });
        Assert.Throws<ArgumentException>(() => { profile.Transform.FromXyz([], Intent.RelativeColorimetric); });

        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Enumerable.Range(0, 15).Select(_ => double.NaN).ToArray();
        
        var iccConfig = new IccConfiguration(profile, Intent.Unspecified, name: "no reverse transform");
        var config = new Configuration(iccConfig: iccConfig);
        var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.NoClip, Is.False);
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(colour.Icc.Values, Is.EqualTo(expected));
        Assert.That(colour.Icc.ColourSpace, Is.EqualTo(profile.Header.DataColourSpace));
        Assert.That(colour.Icc.Error!.Contains("transform is not defined"));
    }
    
    [TestCaseSource(nameof(UnicolourFromCmykTestData))]
    public void UnicolourFromCmyk(IccFile iccFile, Intent intent, double[] cmykValues) => UnicolourFromCmyk(iccFile, intent, cmykValues, noClip: false);
    
    [TestCaseSource(nameof(UnicolourFromCmykTestData))]
    public void UnicolourFromCmykNoClip(IccFile iccFile, Intent intent, double[] cmykValues) => UnicolourFromCmyk(iccFile, intent, cmykValues, noClip: true);

    private static void UnicolourFromCmyk(IccFile iccFile, Intent intent, double[] cmykValues, bool noClip)
    {
        var profile = iccFile.GetProfile();
        
        // device channels values are used to create D65 unicolour
        var iccD65Config = GetConfig(XyzConfiguration.D65, iccFile, intent, noClip);
        var colourD65 = new Unicolour(iccD65Config, new Channels(cmykValues));
        
        // unicolour converted to the ICC D50 white point
        var iccD50Config = GetConfig(Transform.XyzD50, iccFile, intent, noClip);
        var colourD50 = colourD65.ConvertToConfiguration(iccD50Config);
        
        // the XYZ values should be the same as calling the core ICC profile function
        var expectedXyzD50 = profile.Transform.ToXyz(cmykValues, intent, noClip);
        Assert.That(colourD50.Xyz.Values, Is.EqualTo(expectedXyzD50).Within(1e-15));
    }
    
    [TestCaseSource(nameof(UnicolourToCmykTestData))]
    public void UnicolourToCmyk(IccFile iccFile, Intent intent, double[] xyzValues) => UnicolourToCmyk(iccFile, intent, xyzValues, noClip: false);
    
    [TestCaseSource(nameof(UnicolourToCmykTestData))]
    public void UnicolourToCmykNoClip(IccFile iccFile, Intent intent, double[] xyzValues) => UnicolourToCmyk(iccFile, intent, xyzValues, noClip: true);
    
    private static void UnicolourToCmyk(IccFile iccFile, Intent intent, double[] xyzValues, bool noClip)
    {
        var profile = iccFile.GetProfile();
        
        // XYZ values are used to create D50 unicolour
        var iccD50Config = GetConfig(Transform.XyzD50, iccFile, intent, noClip);
        var colourD50 = new Unicolour(iccD50Config, ColourSpace.Xyz, xyzValues[0], xyzValues[1], xyzValues[2]);
        
        // unicolour converted to the ICC D65 white point
        var iccD65Config = GetConfig(XyzConfiguration.D65, iccFile, intent, noClip);
        var colourD65 = colourD50.ConvertToConfiguration(iccD65Config);
        
        // the CMYK values should be the same as calling the core ICC profile function
        var expectedCmyk = profile.Transform.FromXyz(xyzValues, intent);
        Assert.That(colourD65.Icc.Values, Is.EqualTo(expectedCmyk).Within(1e-15));
    }
    
    [Test]
    public void NoClipDifferenceUnicolourFromCmyk()
    {
        // values from test data generated using v2.4.2.6 of iccDEV (via https://github.com/waacton/iccDEV-ConversionTestData)
        var iccFile = IccFile.Fogra39;
        const Intent intent = Intent.Perceptual;
        var cmyk = new Channels(1, 1, 1, 1);
        var expectedLabD50 = new Lab(3.137238, 0.045380, -0.009506);
        var expectedNoClipLabD50 = new Lab(3.137238, -0.122009, 6.133102);
        const double tolerance = 0.00005;
        
        var colourD65 = new Unicolour(GetConfig(XyzConfiguration.D65, iccFile, intent), cmyk);
        var colourD50 = colourD65.ConvertToConfiguration(GetConfig(Transform.XyzD50, iccFile, intent));
        Assert.That(colourD65.Configuration.Icc.NoClip, Is.False);
        TestUtils.AssertTriplet(colourD50.Lab, expectedLabD50, tolerance);
        
        var colourD65NoClip = new Unicolour(GetConfig(XyzConfiguration.D65, iccFile, intent, noClip: true), cmyk);
        var colourD50NoClip = colourD65NoClip.ConvertToConfiguration(GetConfig(Transform.XyzD50, iccFile, intent, noClip: true));
        Assert.That(colourD65NoClip.Configuration.Icc.NoClip, Is.True);
        TestUtils.AssertTriplet(colourD50NoClip.Lab, expectedNoClipLabD50, tolerance);
    }
    
    [Test]
    public void NoClipDifferenceUnicolourToCmyk()
    {
        // values from test data generated using v2.4.2.6 of iccDEV (via https://github.com/waacton/iccDEV-ConversionTestData)
        var iccFile = IccFile.Fogra39;
        const Intent intent = Intent.Perceptual;
        var labD50 = new Lab(0, 25, 25);
        double[] expectedCmyk = [0.342139, 0.998789, 0.255864, 0.940463];
        double[] expectedNoClipCmyk = [0.202649, 0.989950, 0.214706, 0.965072];
        const double tolerance = 0.00005;
        
        var colourD50 = new Unicolour(GetConfig(Transform.XyzD50, iccFile, intent), ColourSpace.Lab, labD50.Tuple);
        var colourD65 = colourD50.ConvertToConfiguration(GetConfig(XyzConfiguration.D65, iccFile, intent));
        Assert.That(colourD50.Configuration.Icc.NoClip, Is.False);
        Assert.That(colourD65.Icc.Values, Is.EqualTo(expectedCmyk).Within(tolerance));
        
        var colourD50NoClip = new Unicolour(GetConfig(Transform.XyzD50, iccFile, intent, noClip: true), ColourSpace.Lab, labD50.Tuple);
        var colourD65NoClip = colourD50NoClip.ConvertToConfiguration(GetConfig(XyzConfiguration.D65, iccFile, intent, noClip: true));
        Assert.That(colourD50NoClip.Configuration.Icc.NoClip, Is.True);
        Assert.That(colourD65NoClip.Icc.Values, Is.EqualTo(expectedNoClipCmyk).Within(tolerance));
    }

    private static void AddTestData(IccFile iccFile)
    {
        var profile = iccFile.GetProfile();
        List<TestCaseData> toPcsTestData = [];
        List<TestCaseData> toPcsNoClipTestData = [];
        List<TestCaseData> toDeviceTestData = [];
        List<TestCaseData> toDeviceNoClipTestData = [];

        foreach (var intent in Intents)
        {
            toPcsTestData.AddRange(ParseTestCaseData(IccTransform.ToPcs, intent));
            toPcsNoClipTestData.AddRange(ParseTestCaseData(IccTransform.ToPcs, intent, noClip: true));
            toDeviceTestData.AddRange(ParseTestCaseData(IccTransform.ToDevice, intent));
            toDeviceNoClipTestData.AddRange(ParseTestCaseData(IccTransform.ToDevice, intent, noClip: true));
        }
        
        List<TestCaseData> ParseTestCaseData(IccTransform transform, Intent intent, bool noClip = false)
        {
            var transformName = transform switch
            {
                IccTransform.ToPcs => "DeviceToPcs",
                IccTransform.ToDevice => "PcsToDevice",
                _ => throw new ArgumentOutOfRangeException(nameof(transform), transform, null)
            };

            var intentValue = (int)intent;
            var noClipSuffix = noClip ? "_NoClip" : string.Empty;
            var csvFileName = $"{iccFile.Name}_{transformName}_Intent{intentValue}{noClipSuffix}.csv";
            var csvPath = Path.Combine(IccFile.DataFolderName, csvFileName);
            if (!File.Exists(csvPath))
            {
                return [];
            }
            
            var testColours = ParseCsv(profile, transform, csvPath, intent).ToList();
            var testCases = testColours.Select(x => new TestCaseData(x).SetName($"{iccFile}, {x}"));
            return testCases.ToList();
        }

        ToPcsTestData.AddRange(toPcsTestData);
        ToPcsNoClipTestData.AddRange(toPcsNoClipTestData);
        ToDeviceTestData.AddRange(toDeviceTestData);
        ToDeviceNoClipTestData.AddRange(toDeviceNoClipTestData);
    }

    private static List<IccTestColour> ParseCsv(Profile profile, IccTransform iccTransform, string csvFile, Intent intent)
    {
        var lines = File.ReadAllLines(csvFile);
        double[] ArrayFromString(string value) => value.Split(",").Select(double.Parse).ToArray();
        
        List<IccTestColour> data = [];
        foreach (var line in lines)
        {
            var split = line.Split(",-->,").ToList();
            var input = ArrayFromString(split[0]); // before the "-->"
            var output = ArrayFromString(split[1]); // after the "-->"
            data.Add(new IccTestColour(profile, iccTransform, intent, input, output));
        }

        return data;
    }
    
    private static List<TestCaseData> GenerateUnicolourFromCmykTestData()
    {
        List<TestCaseData> testCases = [];
        IccFile[] iccFiles = [IccFile.Fogra39, IccFile.Fogra55];
        foreach (var iccFile in iccFiles)
        {
            foreach (var intent in Intents)
            {
                var deviceChannels = IccFile.GetDeviceChannels(iccFile);
                for (var channel = 0; channel < deviceChannels; channel++)
                {
                    var channelValues = new double[deviceChannels];
                    channelValues[channel] = 1.0;
                    var testCaseName = $"{iccFile}, {intent}, [{string.Join(",", channelValues)}]";
                    testCases.Add(new TestCaseData(iccFile, intent, channelValues).SetName(testCaseName));
                }
            }
        }
        
        return testCases;
    }
    
    private static List<TestCaseData> GenerateUnicolourToCmykTestData()
    {
        List<TestCaseData> testCases = [];
        IccFile[] iccFiles = [IccFile.Fogra39, IccFile.Fogra55];
        foreach (var iccFile in iccFiles)
        {
            foreach (var intent in Intents)
            {
                for (var channel = 0; channel < 3; channel++)
                {
                    var channelValues = new double[3];
                    channelValues[channel] = 0.5;
                    var testCaseName = $"{iccFile}, {intent}, [{string.Join(",", channelValues)}]";
                    testCases.Add(new TestCaseData(iccFile, intent, channelValues).SetName(testCaseName));
                }
            }
        }

        return testCases;
    }

    private static Configuration GetConfig(XyzConfiguration xyzConfig, IccFile iccFile, Intent intent, bool noClip = false)
    {
        var iccConfig = new IccConfiguration(iccFile.GetProfile(), intent, noClip);
        return new Configuration(xyzConfig: xyzConfig, iccConfig: iccConfig);
    }
    
    private static readonly Intent[] Intents =
    [
        Intent.Perceptual,
        Intent.RelativeColorimetric,
        Intent.Saturation,
        Intent.AbsoluteColorimetric
    ];
}