using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

/*
 * test data extracted from ICC's demonstration implementation of iccMAX
 * (https://github.com/InternationalColorConsortium/DemoIccMAX)
 * if these values are wrong because the ICC can't write software
 * that conforms to their own complex specification
 * then what chance do I have?
 */
public class IccConversionTests
{
    // ReSharper disable CollectionNeverQueried.Local - used in test case sources by name
    private static readonly List<TestCaseData> ToPcsTestData = [];
    private static readonly List<TestCaseData> ToDeviceTestData = [];
    // ReSharper restore CollectionNeverQueried.Local

    private static List<TestCaseData> DeviceToUnicolourD65TestData;
    private static List<TestCaseData> UnicolourD65ToDeviceTestData;

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

        DeviceToUnicolourD65TestData = GenerateDeviceToUnicolourTestData();
        UnicolourD65ToDeviceTestData = GenerateUnicolourToDeviceTestData();
    }
    
    [TestCaseSource(nameof(ToPcsTestData))]
    public void DeviceToPcs(IccTestColour testColour)
    {
        var expected = testColour.Output;
        var pcs = testColour.Profile.Header.Pcs; 
        var deviceValues = testColour.Input;
        var intent = testColour.Intent;
        
        var actual = testColour.Profile.Transform.ToXyz(deviceValues, intent);
        if (pcs == Signatures.Lab)
        {
            var xyz = new Xyz(actual[0], actual[1], actual[2]);
            actual = Lab.FromXyz(xyz, Transform.XyzD50).ToArray();
        }
        
        var tolerance = pcs switch
        {
            Signatures.Lab => intent == Intent.AbsoluteColorimetric ? 0.005 : 0.0001,
            Signatures.Xyz => intent == Intent.AbsoluteColorimetric ? 0.0000075 : 0.00000075,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
    }
    
    [TestCaseSource(nameof(ToDeviceTestData))]
    public void PcsToDevice(IccTestColour testColour)
    {
        var expected = testColour.Output;
        var pcs = testColour.Profile.Header.Pcs;
        var device = testColour.Profile.Header.DataColourSpace;
        var intent = testColour.Intent;
        
        var (first, second, third) = (testColour.Input[0], testColour.Input[1], testColour.Input[2]);
        var xyz = pcs == Signatures.Lab
            ? Lab.ToXyz(new Lab(first, second, third), Transform.XyzD50)
            : new Xyz(first, second, third);
        
        var actual = testColour.Profile.Transform.FromXyz(xyz.ToArray(), intent);
        var tolerance = device switch
        {
            Signatures.Cmyk => intent == Intent.AbsoluteColorimetric ? 0.00025 : 0.00000125,
            Signatures.Clr7 => intent == Intent.AbsoluteColorimetric ? 0.00005 : 0.00000125,
            Signatures.Rgb => intent == Intent.AbsoluteColorimetric ? 0.000125 : 0.000015,
            Signatures.Grey => 0.000005,
            _ => throw new ArgumentOutOfRangeException()
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
        
        var iccConfig = new IccConfiguration(profile, Intent.Unspecified, "no reverse transform");
        var config = new Configuration(iccConfig: iccConfig);
        var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(colour.Icc.Values, Is.EqualTo(expected));
        Assert.That(colour.Icc.ColourSpace, Is.EqualTo(profile.Header.DataColourSpace));
        Assert.That(colour.Icc.Error!.Contains("transform is not defined"));
    }
    
    [TestCaseSource(nameof(DeviceToUnicolourD65TestData))]
    public void DeviceToColourXyzD65(IccFile iccFile, Intent intent, double[] deviceValues)
    {
        var profile = iccFile.GetProfile();
        
        // device channels values are used to create D65 unicolour
        var iccD65Config = GetConfig(XyzConfiguration.D65, iccFile, intent);
        var colourD65 = new Unicolour(iccD65Config, new Channels(deviceValues));
        
        // unicolour converted to the ICC D50 white point
        var iccD50Config = GetConfig(Transform.XyzD50, iccFile, intent);
        var colourD50 = colourD65.ConvertToConfiguration(iccD50Config);
        
        // the XYZ values should be the same as calling the core ICC profile function
        var expectedXyzD50 = profile.Transform.ToXyz(deviceValues, intent);
        Assert.That(colourD50.Xyz.ToArray(), Is.EqualTo(expectedXyzD50).Within(1e-15));
    }
    
    [TestCaseSource(nameof(UnicolourD65ToDeviceTestData))]
    public void ColourXyzD65ToDevice(IccFile iccFile, Intent intent, double[] xyzValues)
    {
        var profile = iccFile.GetProfile();
        
        // XYZ values are used to create D50 unicolour
        var iccD50Config = GetConfig(Transform.XyzD50, iccFile, intent);
        var colourD50 = new Unicolour(iccD50Config, ColourSpace.Xyz, xyzValues[0], xyzValues[1], xyzValues[2]);
        
        // unicolour converted to the ICC D65 white point
        var iccD65Config = GetConfig(XyzConfiguration.D65, iccFile, intent);
        var colourD65 = colourD50.ConvertToConfiguration(iccD65Config);
        
        // the device channel values should be the same as calling the core ICC profile function
        var expectedDevice = profile.Transform.FromXyz(xyzValues, intent);
        Assert.That(colourD65.Icc.Values, Is.EqualTo(expectedDevice).Within(1e-15));
    }

    private static void AddTestData(IccFile iccFile)
    {
        const string DataSource = "ICC";
        var profile = iccFile.GetProfile();
        var toPcsTestData = new List<TestCaseData>();
        var toDeviceTestData = new List<TestCaseData>();

        foreach (var intent in Intents)
        {
            toPcsTestData.AddRange(ParseTestCaseData(IccTransform.ToPcs, intent));
            toDeviceTestData.AddRange(ParseTestCaseData(IccTransform.ToDevice, intent));
        }
        
        List<TestCaseData> ParseTestCaseData(IccTransform transform, Intent intent)
        {
            var transformName = transform switch
            {
                IccTransform.ToPcs => "ToPcs",
                IccTransform.ToDevice => "ToDevice",
                _ => throw new ArgumentOutOfRangeException(nameof(transform), transform, null)
            };

            var intentValue = (int)intent;
            var csvFileName = $"{iccFile.Name}_{transformName}_{DataSource}-{intentValue}.csv";
            var csvPath = Path.Combine(IccFile.DataFolderName, csvFileName);
            if (!File.Exists(csvPath))
            {
                return [];
            }
            
            var testColours = ParseCsv(profile, transform, csvPath, DataSource, intent).ToList();
            var testCases = testColours.Select(x => new TestCaseData(x).SetName($"{iccFile}, {x}"));
            return testCases.ToList();
        }

        ToPcsTestData.AddRange(toPcsTestData);
        ToDeviceTestData.AddRange(toDeviceTestData);
    }

    private static List<IccTestColour> ParseCsv(Profile profile, IccTransform iccTransform, string csvFile, string source, Intent intent)
    {
        var lines = File.ReadAllLines(csvFile);
        double[] ArrayFromString(string value) => value.Split(",").Select(double.Parse).ToArray();
        
        var data = new List<IccTestColour>();
        foreach (var line in lines)
        {
            var split = line.Split(",-->,").ToList();
            var input = ArrayFromString(split[0]); // before the "-->"
            var output = ArrayFromString(split[1]); // after the "-->"
            data.Add(new IccTestColour(profile, iccTransform, source, intent, input, output));
        }

        return data;
    }
    
    private static List<TestCaseData> GenerateDeviceToUnicolourTestData()
    {
        var testCases = new List<TestCaseData>();
        var iccFiles = new[] { IccFile.Fogra39, IccFile.Fogra55 };
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
    
    private static List<TestCaseData> GenerateUnicolourToDeviceTestData()
    {
        var testCases = new List<TestCaseData>();
        var iccFiles = new[] { IccFile.Fogra39, IccFile.Fogra55 };
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

    private static Configuration GetConfig(XyzConfiguration xyzConfig, IccFile iccFile, Intent intent)
    {
        var iccConfig = new IccConfiguration(iccFile.GetProfile(), intent);
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