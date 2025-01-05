using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class ConfigureIccTests
{ 
    [TestCase(nameof(IccFile.Fogra39), nameof(IccFile.Fogra55))]
    [TestCase(nameof(IccFile.Fogra55), nameof(IccFile.Fogra39))]
    [TestCase(nameof(IccFile.Fogra39), nameof(IccFile.JapanColor2011))]
    [TestCase(nameof(IccFile.JapanColor2011), nameof(IccFile.Fogra39))]
    public void ConvertBetweenIccProfiles(string sourceFileName, string destinationFileName)
    {
        var sourceFile = IccFile.Lookup[sourceFileName];
        var sourceIccConfig = new IccConfiguration(sourceFile.Path, Intent.Unspecified, "source");
        var sourceConfig = new Configuration(iccConfiguration: sourceIccConfig);
        var sourceDeviceChannels = IccFile.GetDeviceChannels(sourceFile);
        var sourceCmyk = new double[sourceDeviceChannels];
        for (var i = 0; i < sourceDeviceChannels; i++)
        {
            sourceCmyk[i] = TestUtils.RandomDouble();
        }

        var sourceUnicolour = new Unicolour(sourceConfig, new Channels(sourceCmyk));
        
        var connectingRgb = sourceUnicolour.Rgb;
        
        var destinationFile = IccFile.Lookup[destinationFileName];
        var destinationIccConfig = new IccConfiguration(destinationFile.Path, Intent.Unspecified, "destination");
        var destinationConfig = new Configuration(iccConfiguration: destinationIccConfig);
        var destinationUnicolour = new Unicolour(destinationConfig, ColourSpace.Rgb, connectingRgb.Triplet.Tuple);
        var destinationCmyk = destinationUnicolour.Icc;

        var convertedUnicolour = sourceUnicolour.ConvertToConfiguration(destinationConfig);
        var convertedCmyk = convertedUnicolour.Icc;
        Assert.That(convertedCmyk.Values, Is.EqualTo(destinationCmyk.Values).Within(1e-15));
    }

    [Test]
    public void NoIntent()
    {
        var profile = IccFile.Fogra39.GetProfile();
        var iccConfig = new IccConfiguration(profile, Intent.Unspecified, "unspecified");
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
    }
}