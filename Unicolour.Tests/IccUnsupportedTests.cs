using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class IccUnsupportedTests
{
    [Test]
    public void FileNullFromCmyk()
    {
        var cmyk = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(cmyk);

        var iccConfig = new IccConfiguration(null, "not provided");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, cmyk);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }
    
    [Test]
    public void FileNullFromRgb()
    {
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);

        var iccConfig = new IccConfiguration(null, "not provided");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }
    
    [Test]
    public void FileNotFoundFromCmyk()
    {
        const string path = "🚫";
        Assert.Throws<FileNotFoundException>(() => { _ = new Profile(path); });
        
        var cmyk = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(cmyk);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not found");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, cmyk);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.StartsWith("could not find file", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }
    
    [Test]
    public void FileNotFoundFromRgb()
    {
        const string path = "🚫";
        Assert.Throws<FileNotFoundException>(() => { _ = new Profile(path); });
        
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not found");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.StartsWith("could not find file", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }
    
    [Test]
    public void FileNotEnoughBytesFromCmyk()
    {
        const string path = "not_enough_bytes.icc";
        File.WriteAllBytes(path, CreateBytes(64));
        Assert.Throws<ArgumentException>(() => { _ = new Profile(path); });
        
        var cmyk = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(cmyk);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not enough bytes");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, cmyk);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.Contains("does not contain enough bytes", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        File.Delete(path);
    }
    
    [Test]
    public void FileNotEnoughBytesFromRgb()
    {
        const string path = "not_enough_bytes.icc";
        File.WriteAllBytes(path, CreateBytes(64));
        Assert.Throws<ArgumentException>(() => { _ = new Profile(path); });

        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not enough bytes");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.Contains("does not contain enough bytes", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        File.Delete(path);
    }
    
    [Test]
    public void FileNotParseableFromCmyk()
    {
        const string path = "not_parseable.icc";
        File.WriteAllBytes(path, CreateBytes(512));
        Assert.Throws<ArgumentException>(() => { _ = new Profile(path); });
        
        var cmyk = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(cmyk);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not parseable");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, cmyk);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.Contains("could not be parsed", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        File.Delete(path);
    }
    
    [Test]
    public void FileNotParseableFromRgb()
    {
        const string path = "not_parseable.icc";
        File.WriteAllBytes(path, CreateBytes(512));
        Assert.Throws<ArgumentException>(() => { _ = new Profile(path); });

        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not parseable");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.Contains("could not be parsed", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        File.Delete(path);
    }
    
    [Test]
    public void ProfileWrongSignatureFromCmyk()
    {
        const string path = "bad_signature.icc";
        CorruptSignature(IccFile.Fogra39, path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        foreach (var intent in intents)
        {
            Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(intent); });
        }
        
        var cmyk = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(cmyk);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "wrong signature");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, cmyk);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("signature is incorrect", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        File.Delete(path);
    }
    
    [Test]
    public void ProfileWrongSignatureFromRgb()
    {
        const string path = "bad_signature.icc";
        CorruptSignature(IccFile.Fogra39, path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        foreach (var intent in intents)
        {
            Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(intent); });
        }

        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "wrong signature");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("signature is incorrect", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        File.Delete(path);
    }
    
    [Test]
    public void ProfileNotSupportedHeaderFromCmyk()
    {
        var path = IccFile.StandardRgbV2.Path;
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        foreach (var intent in intents)
        {
            Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(intent); });
        }
        
        var cmyk = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(cmyk);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not supported header");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, cmyk);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }
    
    [Test]
    public void ProfileNotSupportedHeaderFromRgb()
    {
        var path = IccFile.StandardRgbV2.Path;
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        foreach (var intent in intents)
        {
            Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(intent); });
        }
        
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not supported header");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }
    
    [TestCase(Signatures.AToB0, Intent.Perceptual)]
    [TestCase(Signatures.AToB1, Intent.RelativeColorimetric)]
    [TestCase(Signatures.AToB1, Intent.AbsoluteColorimetric)]
    [TestCase(Signatures.AToB2, Intent.Saturation)]
    [TestCase(Signatures.BToA0, Intent.Perceptual)]
    [TestCase(Signatures.BToA1, Intent.RelativeColorimetric)]
    [TestCase(Signatures.BToA1, Intent.AbsoluteColorimetric)]
    [TestCase(Signatures.BToA2, Intent.Saturation)]
    [TestCase(Signatures.MediaWhitePoint, Intent.AbsoluteColorimetric)]
    public void ProfileNotSupportedIntentFromCmyk(string signature, Intent intent)
    {
        var profile = new Profile(IccFile.Fogra39.Path);
        profile.Tags.RemoveAll(x => x.Signature == signature);
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(intent); });

        var cmyk = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(cmyk);
        
        var iccConfig = new IccConfiguration(profile, intent, "not supported intent");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, cmyk);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }
    
    [TestCase(Signatures.AToB0, Intent.Perceptual)]
    [TestCase(Signatures.AToB1, Intent.RelativeColorimetric)]
    [TestCase(Signatures.AToB1, Intent.AbsoluteColorimetric)]
    [TestCase(Signatures.AToB2, Intent.Saturation)]
    [TestCase(Signatures.BToA0, Intent.Perceptual)]
    [TestCase(Signatures.BToA1, Intent.RelativeColorimetric)]
    [TestCase(Signatures.BToA1, Intent.AbsoluteColorimetric)]
    [TestCase(Signatures.BToA2, Intent.Saturation)]
    [TestCase(Signatures.MediaWhitePoint, Intent.AbsoluteColorimetric)]
    public void ProfileNotSupportedIntentFromRgb(string signature, Intent intent)
    {
        var profile = new Profile(IccFile.Fogra39.Path);
        profile.Tags.RemoveAll(x => x.Signature == signature);
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(intent); });
        
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(profile, intent, "not supported intent");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(iccConfig.Intent, Is.EqualTo(intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }
    
    [TestCase(Intent.Perceptual)]
    [TestCase(Intent.RelativeColorimetric)]
    [TestCase(Intent.Saturation)]
    [TestCase(Intent.AbsoluteColorimetric)]
    public void ProfileNotSupportedLutFromCmyk(Intent intent)
    {
        var path = IccFile.Prmg.Path;
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(intent); });

        var cmyk = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(cmyk);
        
        var iccConfig = new IccConfiguration(profile, intent, "not supported LUT");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, cmyk);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }
    
    [TestCase(Intent.Perceptual)]
    [TestCase(Intent.RelativeColorimetric)]
    [TestCase(Intent.Saturation)]
    [TestCase(Intent.AbsoluteColorimetric)]
    public void ProfileNotSupportedLutFromRgb(Intent intent)
    {
        var path = IccFile.Prmg.Path;
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(intent); });
        
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(profile, intent, "not supported LUT");
        var config = new Configuration(iccConfiguration: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(iccConfig.Intent, Is.EqualTo(intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }

    [TestCase(nameof(IccFile.Fogra39), new[] { 0.8, 0.6, 0.4, 0.2, 1.0 }, new[] { 0.8, 0.6, 0.4, 0.2 })]
    [TestCase(nameof(IccFile.Fogra39), new[] { 0.8, 0.6, 0.4 }, new[] { 0.8, 0.6, 0.4, 0.0 })]
    [TestCase(nameof(IccFile.Fogra39), new[] { 0.8 }, new[] { 0.8, 0.0, 0.0, 0.0 })]
    [TestCase(nameof(IccFile.Fogra39), new double[] { }, new[] { 0.0, 0.0, 0.0, 0.0 })]
    [TestCase(nameof(IccFile.Fogra55), new[] { 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1.0 }, new[] { 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875 })]
    [TestCase(nameof(IccFile.Fogra55), new[] { 0.125, 0.25, 0.375, 0.5, 0.625, 0.75 }, new[] { 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.0 })]
    [TestCase(nameof(IccFile.Fogra55), new[] { 0.125 }, new[] { 0.125, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 })]
    [TestCase(nameof(IccFile.Fogra55), new double[] { }, new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 })]
    public void WrongDimensions(string iccFileName, double[] input, double[] effectiveInput)
    {
        var iccFile = IccFile.Lookup[iccFileName];
        var profile = iccFile.GetProfile();
        var iccConfig = new IccConfiguration(profile);
        var config = new Configuration(iccConfiguration: iccConfig);
        var actualUnicolour = new Unicolour(config, new Channels(input));
        var expectedUnicolour = new Unicolour(config, new Channels(effectiveInput));
        
        TestUtils.AssertTriplet(actualUnicolour.Xyz.Triplet, expectedUnicolour.Xyz.Triplet, 0);
        TestUtils.AssertTriplet(actualUnicolour.Rgb.Triplet, expectedUnicolour.Rgb.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(actualUnicolour.Icc.ColourSpace, Is.EqualTo(profile.Header.DataColourSpace));
        Assert.That(expectedUnicolour.Icc.ColourSpace, Is.EqualTo(profile.Header.DataColourSpace));
    }
    
    [TestCase(new[] { 0.8, 0.6, 0.4, 0.2, 1.0 }, new[] { 0.8, 0.6, 0.4, 0.2 })]
    [TestCase(new[] { 0.8, 0.6, 0.4 }, new[] { 0.8, 0.6, 0.4, 0.0 })]
    [TestCase(new[] { 0.8 }, new[] { 0.8, 0.0, 0.0, 0.0 })]
    [TestCase(new double[] { }, new[] { 0.0, 0.0, 0.0, 0.0 })]
    public void WrongDimensionsUncalibrated(double[] input, double[] effectiveInput)
    {
        var iccConfig = new IccConfiguration(null);
        var config = new Configuration(iccConfiguration: iccConfig);
        var actualUnicolour = new Unicolour(config, new Channels(input));
        var expectedUnicolour = new Unicolour(config, new Channels(effectiveInput));
        
        TestUtils.AssertTriplet(actualUnicolour.Rgb.Triplet, expectedUnicolour.Rgb.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(actualUnicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(expectedUnicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
    }

    private static byte[] CreateBytes(int length) => Enumerable.Range(0, length).Select(_ => (byte)119).ToArray();
    
    private static void CorruptSignature(IccFile iccFile, string corruptedPath)
    {
        var profile = iccFile.GetProfile();
        var bytes = File.ReadAllBytes(profile.FileInfo.FullName);
        bytes[36] = 115;
        bytes[37] = 109;
        bytes[38] = 101;
        bytes[39] = 103;
        File.WriteAllBytes(corruptedPath, bytes);
    }
    
    private static readonly Intent[] intents =
    [
        Intent.Perceptual,
        Intent.RelativeColorimetric,
        Intent.Saturation,
        Intent.AbsoluteColorimetric
    ];
}