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
    // device channels --> PCS (LAB or XYZ, using ICC D50 white point)
    private static List<TestCaseData> Fogra39ToPcsTestData;
    private static List<TestCaseData> Fogra55ToPcsTestData;
    private static List<TestCaseData> Swop2006ToPcsTestData;
    private static List<TestCaseData> Swop2013ToPcsTestData;
    private static List<TestCaseData> JapanColor2011ToPcsTestData;
    private static List<TestCaseData> RommRgbToPcsTestData;
    private static List<TestCaseData> StandardRgbV4ToPcsTestData;
    
    // PCS (LAB or XYZ, using ICC D50 white point) --> device channels
    private static List<TestCaseData> Fogra39ToDeviceTestData;
    private static List<TestCaseData> Fogra55ToDeviceTestData;
    private static List<TestCaseData> Swop2006ToDeviceTestData;
    private static List<TestCaseData> Swop2013ToDeviceTestData;
    private static List<TestCaseData> JapanColor2011ToDeviceTestData;
    private static List<TestCaseData> RommRgbToDeviceTestData;
    private static List<TestCaseData> StandardRgbV4ToDeviceTestData;

    private static List<TestCaseData> DeviceToUnicolourD65TestData;
    private static List<TestCaseData> UnicolourD65ToDeviceTestData;
    
    static IccConversionTests()
    {
        (Fogra39ToPcsTestData, Fogra39ToDeviceTestData) = ParseTestData(IccFile.Fogra39);
        (Fogra55ToPcsTestData, Fogra55ToDeviceTestData) = ParseTestData(IccFile.Fogra55);
        (Swop2006ToPcsTestData, Swop2006ToDeviceTestData) = ParseTestData(IccFile.Swop2006);
        (Swop2013ToPcsTestData, Swop2013ToDeviceTestData) = ParseTestData(IccFile.Swop2013);
        (JapanColor2011ToPcsTestData, JapanColor2011ToDeviceTestData) = ParseTestData(IccFile.JapanColor2011);
        (RommRgbToPcsTestData, RommRgbToDeviceTestData) = ParseTestData(IccFile.RommRgb);
        (StandardRgbV4ToPcsTestData, StandardRgbV4ToDeviceTestData) = ParseTestData(IccFile.StandardRgbV4);
        
        DeviceToUnicolourD65TestData = GenerateDeviceToUnicolourTestData();
        UnicolourD65ToDeviceTestData = GenerateUnicolourToDeviceTestData();
    }
    
    [TestCaseSource(nameof(Fogra39ToPcsTestData))]
    [TestCaseSource(nameof(Fogra55ToPcsTestData))]
    [TestCaseSource(nameof(Swop2006ToPcsTestData))]
    [TestCaseSource(nameof(Swop2013ToPcsTestData))]
    [TestCaseSource(nameof(JapanColor2011ToPcsTestData))]
    [TestCaseSource(nameof(RommRgbToPcsTestData))]
    [TestCaseSource(nameof(StandardRgbV4ToPcsTestData))]
    public void DeviceToPcs(IccTestColour testColour)
    {
        var expected = testColour.Output;
        var pcs = testColour.Profile.Header.Pcs; 
        var deviceValues = testColour.Input;
        var intent = testColour.Intent;
        
        var actual = testColour.Profile.ToXyzStandardD50(deviceValues, intent);
        if (pcs == Signatures.Lab)
        {
            var xyz = new Xyz(actual[0], actual[1], actual[2]);
            actual = Lab.FromXyz(xyz, Profile.XyzD50).Triplet.ToArray();
        }
        
        var tolerance = pcs switch
        {
            Signatures.Lab => 0.00075,
            Signatures.Xyz => 0.0000075,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
    }
    
    [TestCaseSource(nameof(Fogra39ToDeviceTestData))]
    [TestCaseSource(nameof(Fogra55ToDeviceTestData))]
    [TestCaseSource(nameof(Swop2006ToDeviceTestData))]
    [TestCaseSource(nameof(Swop2013ToDeviceTestData))]
    [TestCaseSource(nameof(JapanColor2011ToDeviceTestData))]
    [TestCaseSource(nameof(RommRgbToDeviceTestData))]
    [TestCaseSource(nameof(StandardRgbV4ToDeviceTestData))]
    public void PcsToDevice(IccTestColour testColour)
    {
        var expected = testColour.Output;
        var pcs = testColour.Profile.Header.Pcs;
        var device = testColour.Profile.Header.DataColourSpace;
        var intent = testColour.Intent;
        
        var (first, second, third) = (testColour.Input[0], testColour.Input[1], testColour.Input[2]);
        var xyz = pcs == Signatures.Lab
            ? Lab.ToXyz(new Lab(first, second, third), Profile.XyzD50)
            : new Xyz(first, second, third);
        
        var actual = testColour.Profile.FromStandardXyzD50(xyz.Triplet.ToArray(), intent);
        var tolerance = device switch
        {
            Signatures.Cmyk => intent == Intent.AbsoluteColorimetric ? 0.00001 : 0.000001,
            Signatures.Clr7 => intent == Intent.AbsoluteColorimetric ? 0.000025 : 0.00000125,
            Signatures.Rgb => intent == Intent.AbsoluteColorimetric ? 0.000125 : 0.000015,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
    }
    
    [TestCaseSource(nameof(DeviceToUnicolourD65TestData))]
    public void DeviceToUnicolourXyzD65(IccFile iccFile, Intent intent, double[] deviceValues)
    {
        var profile = iccFile.GetProfile();
        
        // device channels values are used to create D65 unicolour
        var iccD65Config = GetConfig(XyzConfiguration.D65, iccFile, intent);
        var unicolourD65 = new Unicolour(iccD65Config, new Channels(deviceValues));
        
        // unicolour converted to the ICC D50 white point
        var iccD50Config = GetConfig(Profile.XyzD50, iccFile, intent);
        var unicolourD50 = unicolourD65.ConvertToConfiguration(iccD50Config);
        
        // the XYZ values should be the same as calling the core ICC profile function
        var expectedXyzD50 = profile.ToXyzStandardD50(deviceValues, intent);
        Assert.That(unicolourD50.Xyz.Triplet.ToArray(), Is.EqualTo(expectedXyzD50).Within(1e-15));
    }
    
    [TestCaseSource(nameof(UnicolourD65ToDeviceTestData))]
    public void UnicolourXyzD65ToDevice(IccFile iccFile, Intent intent, double[] xyzValues)
    {
        var profile = iccFile.GetProfile();
        
        // XYZ values are used to create D50 unicolour
        var iccD50Config = GetConfig(Profile.XyzD50, iccFile, intent);
        var unicolourD50 = new Unicolour(iccD50Config, ColourSpace.Xyz, xyzValues[0], xyzValues[1], xyzValues[2]);
        
        // unicolour converted to the ICC D65 white point
        var iccD65Config = GetConfig(XyzConfiguration.D65, iccFile, intent);
        var unicolourD65 = unicolourD50.ConvertToConfiguration(iccD65Config);
        
        // the device channel values should be the same as calling the core ICC profile function
        var expectedDevice = profile.FromStandardXyzD50(xyzValues, intent);
        Assert.That(unicolourD65.Icc.Values, Is.EqualTo(expectedDevice).Within(1e-15));
    }
    
    private static (List<TestCaseData> toPcs, List<TestCaseData> toDevice) ParseTestData(IccFile iccFile)
    {
        const string DataSource = "ICC";
        var profile = iccFile.GetProfile();
        var toPcsTestCaseData = new List<TestCaseData>();
        var toDeviceTestCaseData = new List<TestCaseData>();

        foreach (var intent in intents)
        {
            toPcsTestCaseData.AddRange(ParseTestCaseData(IccTransform.ToPcs, intent));
            toDeviceTestCaseData.AddRange(ParseTestCaseData(IccTransform.ToDevice, intent));
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
            var testColours = ParseCsv(profile, transform, csvPath, DataSource, intent).ToList();
            var testCases = testColours.Select(x => new TestCaseData(x).SetName($"{iccFile}, {x}"));
            return testCases.ToList();
        }

        return (toPcsTestCaseData, toDeviceTestCaseData);
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
            foreach (var intent in intents)
            {
                for (var channel = 0; channel < iccFile.DeviceChannels; channel++)
                {
                    var channelValues = new double[iccFile.DeviceChannels];
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
            foreach (var intent in intents)
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
        return new Configuration(xyzConfiguration: xyzConfig, iccConfiguration: iccConfig);
    }
    
    private static readonly Intent[] intents =
    [
        Intent.Perceptual,
        Intent.RelativeColorimetric,
        Intent.Saturation,
        Intent.AbsoluteColorimetric
    ];
}