using System.IO;
using System.Linq;
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
    public void ConvertBetweenProfiles(string sourceFileName, string destinationFileName)
    {
        var sourceFile = IccFile.Lookup[sourceFileName];
        var sourceIccConfig = new IccConfiguration(sourceFile.Path, Intent.Unspecified, "source");
        var sourceConfig = new Configuration(iccConfig: sourceIccConfig);
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
        var destinationConfig = new Configuration(iccConfig: destinationIccConfig);
        var destinationUnicolour = new Unicolour(destinationConfig, ColourSpace.Rgb, connectingRgb.Tuple);
        var destinationCmyk = destinationUnicolour.Icc;

        var convertedUnicolour = sourceUnicolour.ConvertToConfiguration(destinationConfig);
        var convertedCmyk = convertedUnicolour.Icc;
        Assert.That(convertedCmyk.Values, Is.EqualTo(destinationCmyk.Values).Within(1.75e-15));
    }

    [TestCase(Intent.Unspecified)]
    [TestCase(Intent.Perceptual)]
    [TestCase(Intent.RelativeColorimetric)]
    [TestCase(Intent.Saturation)]
    [TestCase(Intent.AbsoluteColorimetric)]
    public void DifferentConstructors(Intent intent)
    {
        var iccFile = IccFile.Fogra39;
        
        var path = iccFile.Path;
        var fromPath = new IccConfiguration(path, intent, "from path").Profile!;
        
        var bytes = File.ReadAllBytes(path);
        var fromBytes = new IccConfiguration(bytes, intent, "from bytes").Profile!;

        using var stream = File.OpenRead(path);
        var fromStream = new IccConfiguration(stream, intent, "from stream").Profile!;

        var profile = iccFile.GetProfile();
        var fromProfile = new IccConfiguration(profile, intent, "from profile").Profile!;

        AssertConfig(fromPath, fromBytes);
        AssertConfig(fromPath, fromStream);
        AssertConfig(fromPath, fromProfile);
    }

    private static void AssertConfig(Profile profile1, Profile profile2)
    {
        Assert.That(profile1.Length, Is.EqualTo(profile2.Length));

        var header1 = profile1.Header;
        var header2 = profile2.Header;
        Assert.That(header1.Intent, Is.EqualTo(header2.Intent));
        Assert.That(header1.ProfileSize, Is.EqualTo(header2.ProfileSize));
        Assert.That(header1.PreferredCmmType, Is.EqualTo(header2.PreferredCmmType));
        Assert.That(header1.ProfileVersion, Is.EqualTo(header2.ProfileVersion));
        Assert.That(header1.ProfileClass, Is.EqualTo(header2.ProfileClass));
        Assert.That(header1.DataColourSpace, Is.EqualTo(header2.DataColourSpace));
        Assert.That(header1.Pcs, Is.EqualTo(header2.Pcs));
        Assert.That(header1.DateTime, Is.EqualTo(header2.DateTime));
        Assert.That(header1.ProfileFileSignature, Is.EqualTo(header2.ProfileFileSignature));
        Assert.That(header1.PrimaryPlatform, Is.EqualTo(header2.PrimaryPlatform));
        Assert.That(header1.ProfileFlags, Is.EqualTo(header2.ProfileFlags));
        Assert.That(header1.DeviceManufacturer, Is.EqualTo(header2.DeviceManufacturer));
        Assert.That(header1.DeviceModel, Is.EqualTo(header2.DeviceModel));
        Assert.That(header1.DeviceAttributes, Is.EqualTo(header2.DeviceAttributes));
        Assert.That(header1.Intent, Is.EqualTo(header2.Intent));
        Assert.That(header1.PcsIlluminant, Is.EqualTo(header2.PcsIlluminant));
        Assert.That(header1.ProfileCreator, Is.EqualTo(header2.ProfileCreator));
        Assert.That(header1.ProfileId, Is.EqualTo(header2.ProfileId));
        Assert.That(header1.ToString(), Is.EqualTo(header2.ToString()));
        
        var tags1 = profile1.Tags;
        var tags2 = profile2.Tags;
        Assert.That(tags1.Count, Is.EqualTo(tags2.Count));
        for (var i = 0; i < tags1.Count; i++)
        {
            Assert.That(tags1[i].Signature, Is.EqualTo(tags2[i].Signature));
            Assert.That(tags1[i].Offset, Is.EqualTo(tags2[i].Offset));
            Assert.That(tags1[i].Size, Is.EqualTo(tags2[i].Size));
            Assert.That(tags1[i].Data.Length, Is.EqualTo(tags2[i].Data.Length));
            Assert.That(tags1[i].Data.First(), Is.EqualTo(tags2[i].Data.First()));
            Assert.That(tags1[i].Data.Last(), Is.EqualTo(tags2[i].Data.Last()));
        }
    }

    [Test]
    public void NoIntent()
    {
        var profile = IccFile.Fogra39.GetProfile();
        var iccConfig = new IccConfiguration(profile, Intent.Unspecified, "unspecified");
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
    }
}