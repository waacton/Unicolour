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
    public void FileNullFromChannels()
    {
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(channels);

        var iccConfig = new IccConfiguration(null, "not provided");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void FileNullFromRgb()
    {
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);

        var iccConfig = new IccConfiguration(null, "not provided");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void FileNotFoundFromChannels()
    {
        const string path = "🚫";
        Assert.Throws<FileNotFoundException>(() => { _ = new Profile(path); });
        
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(channels);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not found");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.StartsWith("could not find file", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void FileNotFoundFromRgb()
    {
        const string path = "🚫";
        Assert.Throws<FileNotFoundException>(() => { _ = new Profile(path); });
        
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not found");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.StartsWith("could not find file", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void FileNotEnoughBytesFromChannels()
    {
        const string path = "not_enough_bytes.icc";
        File.WriteAllBytes(path, CreateBytes(64));
        Assert.Throws<ArgumentException>(() => { _ = new Profile(path); });
        
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(channels);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not enough bytes");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.Contains("does not contain enough bytes", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
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
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.Contains("does not contain enough bytes", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
        File.Delete(path);
    }
    
    [Test]
    public void FileNotParseableFromChannels()
    {
        const string path = "not_parseable.icc";
        File.WriteAllBytes(path, CreateBytes(512));
        Assert.Throws<ArgumentException>(() => { _ = new Profile(path); });
        
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(channels);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not parseable");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.Contains("could not be parsed", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
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
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error!.Contains("could not be parsed", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
        File.Delete(path);
    }
    
    [Test]
    public void ProfileNotSupportedHeaderFromChannels()
    {
        var profile = IccFile.CxHue45Abstract.GetProfile();
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(channels);
        
        var iccConfig = new IccConfiguration(profile, Intent.Unspecified, "not supported header");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void ProfileNotSupportedHeaderFromRgb()
    {
        var profile = IccFile.CxHue45Abstract.GetProfile();
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(profile, Intent.Unspecified, "not supported header");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void ProfileNotSupportedTransformDToBFromChannels()
    {
        const string path = "bad_profile_transform.icc";
        WriteProfileWithDToBTags(path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        
        Assert.Throws<NotSupportedException>(() => { profile.Transform.ToXyz([], Intent.Unspecified); });
        
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(channels);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not supported transform");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void ProfileNotSupportedTransformDToBFromRgb()
    {
        const string path = "bad_profile_transform.icc";
        WriteProfileWithDToBTags(path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        
        Assert.Throws<NotSupportedException>(() => { profile.Transform.FromXyz([], Intent.Unspecified); });
        
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not supported transform");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void ProfileNotSupportedTransformNoneFromChannels()
    {
        const string path = "bad_profile_transform.icc";
        WriteProfileWithNoTransformTags(path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        
        Assert.Throws<NotSupportedException>(() => { profile.Transform.ToXyz([], Intent.Unspecified); });
        
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(channels);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not supported transform");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void ProfileNotSupportedTransformNoneFromRgb()
    {
        const string path = "bad_profile_transform.icc";
        WriteProfileWithNoTransformTags(path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        
        Assert.Throws<NotSupportedException>(() => { profile.Transform.FromXyz([], Intent.Unspecified); });
        
        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not supported transform");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("not supported", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
    }
    
    [Test]
    public void ProfileBadSignatureFromChannels()
    {
        const string path = "bad_profile_signature.icc";
        CorruptProfileSignature(IccFile.Fogra39, path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = Channels.UncalibratedToRgb(channels);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "bad profile signature");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("signature is incorrect", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
        File.Delete(path);
    }
    
    [Test]
    public void ProfileBadSignatureFromRgb()
    {
        const string path = "bad_profile_signature.icc";
        CorruptProfileSignature(IccFile.Fogra39, path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });

        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Channels.UncalibratedFromRgb(rgb);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "bad profile signature");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error!.Contains("signature is incorrect", StringComparison.CurrentCultureIgnoreCase));
        Assert.That(unicolour.Icc, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(unicolour.Icc.Error, Is.Null);
        File.Delete(path);
    }
    
    [Test]
    public void LutBadSignatureFromChannels()
    {
        const string path = "bad_lut_signature.icc";
        CorruptLutSignature(IccFile.Swop2013, path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.DoesNotThrow(() => { profile.ErrorIfUnsupported(); });
        
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = new Rgb(double.NaN, double.NaN, double.NaN);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "bad lut signature");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Signatures.Cmyk));
        Assert.That(unicolour.Icc.Error!.Contains(nameof(ArgumentOutOfRangeException)));
        File.Delete(path);
    }
    
    [Test]
    public void LutBadSignatureFromRgb()
    {
        const string path = "bad_lut_signature.icc";
        CorruptLutSignature(IccFile.Swop2013, path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.DoesNotThrow(() => { profile.ErrorIfUnsupported(); });

        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Enumerable.Range(0, 15).Select(_ => double.NaN).ToArray();
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "bad lut signature");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(unicolour.Icc.Values, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Signatures.Cmyk));
        Assert.That(unicolour.Icc.Error!.Contains(nameof(ArgumentOutOfRangeException)));
        File.Delete(path);
    }
    
    [Test]
    public void CurveBadSignatureFromChannels()
    {
        const string path = "bad_curve_signature.icc";
        CorruptCurveSignature(IccFile.Swop2013, path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.DoesNotThrow(() => { profile.ErrorIfUnsupported(); });
        
        var channels = new Channels(0.8, 0.6, 0.4, 0.2);
        var expected = new Rgb(double.NaN, double.NaN, double.NaN);
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "bad curve signature");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, channels);
        TestUtils.AssertTriplet<Rgb>(unicolour, expected.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Signatures.Cmyk));
        Assert.That(unicolour.Icc.Error!.Contains(nameof(ArgumentOutOfRangeException)));
        File.Delete(path);
    }
    
    [Test]
    public void CurveBadSignatureFromRgb()
    {
        const string path = "bad_curve_signature.icc";
        CorruptCurveSignature(IccFile.Swop2013, path);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(path); });
        Assert.DoesNotThrow(() => { profile.ErrorIfUnsupported(); });

        var rgb = new Rgb(0.25, 0.5, 1.0);
        var expected = Enumerable.Range(0, 15).Select(_ => double.NaN).ToArray();
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "bad curve signature");
        var config = new Configuration(iccConfig: iccConfig);
        var unicolour = new Unicolour(config, ColourSpace.Rgb, rgb.Triplet.Tuple);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(unicolour.Icc.Values, Is.EqualTo(expected));
        Assert.That(unicolour.Icc.ColourSpace, Is.EqualTo(Signatures.Cmyk));
        Assert.That(unicolour.Icc.Error!.Contains(nameof(ArgumentOutOfRangeException)));
        File.Delete(path);
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
        var config = new Configuration(iccConfig: iccConfig);
        var actualUnicolour = new Unicolour(config, new Channels(input));
        var expectedUnicolour = new Unicolour(config, new Channels(effectiveInput));
        
        TestUtils.AssertTriplet(actualUnicolour.Xyz.Triplet, expectedUnicolour.Xyz.Triplet, 0);
        TestUtils.AssertTriplet(actualUnicolour.Rgb.Triplet, expectedUnicolour.Rgb.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(profile.Header.Intent));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(actualUnicolour.Icc.ColourSpace, Is.EqualTo(profile.Header.DataColourSpace));
        Assert.That(expectedUnicolour.Icc.ColourSpace, Is.EqualTo(profile.Header.DataColourSpace));
        Assert.That(actualUnicolour.Icc.Error, Is.Null);
        Assert.That(expectedUnicolour.Icc.Error, Is.Null);
    }
    
    [TestCase(new[] { 0.8, 0.6, 0.4, 0.2, 1.0 }, new[] { 0.8, 0.6, 0.4, 0.2 })]
    [TestCase(new[] { 0.8, 0.6, 0.4 }, new[] { 0.8, 0.6, 0.4, 0.0 })]
    [TestCase(new[] { 0.8 }, new[] { 0.8, 0.0, 0.0, 0.0 })]
    [TestCase(new double[] { }, new[] { 0.0, 0.0, 0.0, 0.0 })]
    public void WrongDimensionsUncalibrated(double[] input, double[] effectiveInput)
    {
        var iccConfig = new IccConfiguration(null);
        var config = new Configuration(iccConfig: iccConfig);
        var actualUnicolour = new Unicolour(config, new Channels(input));
        var expectedUnicolour = new Unicolour(config, new Channels(effectiveInput));
        
        TestUtils.AssertTriplet(actualUnicolour.Rgb.Triplet, expectedUnicolour.Rgb.Triplet, 0);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(actualUnicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(expectedUnicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(actualUnicolour.Icc.Error, Is.Null);
        Assert.That(expectedUnicolour.Icc.Error, Is.Null);
    }

    private static byte[] CreateBytes(int length) => Enumerable.Range(0, length).Select(_ => (byte)119).ToArray();
    
    private static void CorruptProfileSignature(IccFile iccFile, string corruptedPath)
    {
        var profile = iccFile.GetProfile();
        var bytes = File.ReadAllBytes(profile.FileInfo.FullName);
        CorruptSignature(bytes, index: 36);
        File.WriteAllBytes(corruptedPath, bytes);
    }

    private static void CorruptLutSignature(IccFile iccFile, string corruptedPath)
    {
        var profile = iccFile.GetProfile();
        var a2b0 = profile.Tags.Single(x => x.Signature == Signatures.AToB0);
        var b2a0 = profile.Tags.Single(x => x.Signature == Signatures.BToA0);
        
        // first bytes in the LUT data is the signature
        var bytes = File.ReadAllBytes(profile.FileInfo.FullName);
        CorruptSignature(bytes, a2b0.Offset);
        CorruptSignature(bytes, b2a0.Offset);
        File.WriteAllBytes(corruptedPath, bytes);
    }
    
    private static void CorruptCurveSignature(IccFile iccFile, string corruptedPath)
    {
        var profile = iccFile.GetProfile();
        var a2b0 = profile.Tags.Single(x => x.Signature == Signatures.AToB0);
        var b2a0 = profile.Tags.Single(x => x.Signature == Signatures.BToA0);
        
        // B curves start at byte 32 of LUT data, first bytes in the curve data is the signature
        var bytes = File.ReadAllBytes(profile.FileInfo.FullName);
        CorruptSignature(bytes, a2b0.Offset + 32);
        CorruptSignature(bytes, b2a0.Offset + 32);
        File.WriteAllBytes(corruptedPath, bytes);
    }
    
    private static void WriteProfileWithDToBTags(string modifiedPath)
    {
        // with ROMM RGB profile: tag table index 1 = A2B0, tag table index 2 = B2A0
        var profile = IccFile.RommRgb.GetProfile();
        const uint a2b0TagTableIndex = 128 + 4 + 1 * 12;
        const uint b2a0TagTableIndex = 128 + 4 + 2 * 12;
        
        // ROMM RGB only has A2B0 and B2A0, this replaces both with D2B0 and B2D0 in the tag table
        var bytes = File.ReadAllBytes(profile.FileInfo.FullName);
        ModifySignature(bytes, a2b0TagTableIndex, [68, 50, 66, 48]); // "D2B0"
        ModifySignature(bytes, b2a0TagTableIndex, [66, 50, 68, 48]); // "B2D0"
        File.WriteAllBytes(modifiedPath, bytes);
    }
    
    private static void WriteProfileWithNoTransformTags(string modifiedPath)
    {
        // with ROMM RGB profile: tag table index 1 = A2B0, tag table index 2 = B2A0
        var profile = IccFile.RommRgb.GetProfile();
        const uint a2b0TagTableIndex = 128 + 4 + 1 * 12;
        const uint b2a0TagTableIndex = 128 + 4 + 2 * 12;
        
        // ROMM RGB only has A2B0 and B2A0; this replaces both with "NONE" in the tag table
        // effectively creating a profile with no transform tags
        var bytes = File.ReadAllBytes(profile.FileInfo.FullName);
        ModifySignature(bytes, a2b0TagTableIndex, [78, 79, 78, 69]); // "NONE"
        ModifySignature(bytes, b2a0TagTableIndex, [78, 79, 78, 69]); // "NONE"
        File.WriteAllBytes(modifiedPath, bytes);
    }
    
    private static void CorruptSignature(byte[] bytes, uint index) => ModifySignature(bytes, index, [115, 109, 101, 103]);
    private static void ModifySignature(byte[] bytes, uint index, byte[] modifiedBytes)
    {
        bytes[index + 0] = modifiedBytes[0];
        bytes[index + 1] = modifiedBytes[1];
        bytes[index + 2] = modifiedBytes[2];
        bytes[index + 3] = modifiedBytes[3];
    }
}