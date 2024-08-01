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
    // device channels --> LAB (using ICC D50 white point)
    private static List<TestCaseData> Fogra39ToPcsTestData;
    private static List<TestCaseData> Fogra55ToPcsTestData;
    private static List<TestCaseData> Swop2006ToPcsTestData;
    private static List<TestCaseData> JapanColor2011ToPcsTestData;
    
    // LAB (using ICC D50 white point) --> device channels
    private static List<TestCaseData> Fogra39ToDeviceTestData;
    private static List<TestCaseData> Fogra55ToDeviceTestData;
    private static List<TestCaseData> Swop2006ToDeviceTestData;
    private static List<TestCaseData> JapanColor2011ToDeviceTestData;

    private static List<TestCaseData> DeviceToUnicolourD65TestData;
    private static List<TestCaseData> UnicolourD65ToDeviceTestData;
    
    static IccConversionTests()
    {
        (Fogra39ToPcsTestData, Fogra39ToDeviceTestData) = ParseTestData(IccFile.Fogra39);
        (Fogra55ToPcsTestData, Fogra55ToDeviceTestData) = ParseTestData(IccFile.Fogra55);
        (Swop2006ToPcsTestData, Swop2006ToDeviceTestData) = ParseTestData(IccFile.Swop2006);
        (JapanColor2011ToPcsTestData, JapanColor2011ToDeviceTestData) = ParseTestData(IccFile.JapanColor2011);
        DeviceToUnicolourD65TestData = GenerateDeviceToUnicolourTestData();
        UnicolourD65ToDeviceTestData = GenerateUnicolourToDeviceTestData();
    }
    
    [TestCaseSource(nameof(Fogra39ToPcsTestData))]
    [TestCaseSource(nameof(Fogra55ToPcsTestData))]
    [TestCaseSource(nameof(Swop2006ToPcsTestData))]
    [TestCaseSource(nameof(JapanColor2011ToPcsTestData))]
    public void DeviceToLabStandardD50(IccTestColour testColour)
    {
        var expected = testColour.Output;
        var xyz = testColour.Profile.ToXyzStandardD50(testColour.Input, testColour.Intent);
        var actual = Lab.FromXyz(xyz, Profile.XyzD50).Triplet;
        TestUtils.AssertTriplet(actual, new ColourTriplet(expected[0], expected[1], expected[2]), 0.0005);
    }
    
    [TestCaseSource(nameof(Fogra39ToDeviceTestData))]
    [TestCaseSource(nameof(Fogra55ToDeviceTestData))]
    [TestCaseSource(nameof(Swop2006ToDeviceTestData))]
    [TestCaseSource(nameof(JapanColor2011ToDeviceTestData))]
    public void DeviceFromLabStandardD50(IccTestColour testColour)
    {
        var expected = testColour.Output;
        var lab = new Lab(testColour.Input[0], testColour.Input[1], testColour.Input[2]);
        var xyz = Lab.ToXyz(lab, Profile.XyzD50);
        var actual = testColour.Profile.FromStandardXyzD50(xyz, testColour.Intent);
        var tolerance = testColour.Intent == Intent.AbsoluteColorimetric ? 0.000025 : 0.000005;
        Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
    }
    
    [TestCaseSource(nameof(DeviceToUnicolourD65TestData))]
    public void DeviceToXyzUnicolourD65(IccFile iccFile, Intent intent, double[] deviceValues)
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
        TestUtils.AssertTriplet<Xyz>(unicolourD50, expectedXyzD50.Triplet, 1e-15);
    }
    
    [TestCaseSource(nameof(UnicolourD65ToDeviceTestData))]
    public void DeviceFromXyzUnicolourD65(IccFile iccFile, Intent intent, double[] xyzValues)
    {
        var profile = iccFile.GetProfile();
        var (x, y, z) = (xyzValues[0], xyzValues[1], xyzValues[2]);
        
        // XYZ values are used to create D50 unicolour
        var iccD50Config = GetConfig(Profile.XyzD50, iccFile, intent);
        var unicolourD50 = new Unicolour(iccD50Config, ColourSpace.Xyz, x, y, z);
        
        // unicolour converted to the ICC D65 white point
        var iccD65Config = GetConfig(XyzConfiguration.D65, iccFile, intent);
        var unicolourD65 = unicolourD50.ConvertToConfiguration(iccD65Config);
        
        // the device channel values should be the same as calling the core ICC profile function
        var expectedDevice = profile.FromStandardXyzD50(new Xyz(x, y, z), intent);
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