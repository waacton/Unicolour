using System;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class IccUnsupportedDataTests : IccUnsupportedTests
{
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotProvided(Source source)
    {
        var iccConfig = new IccConfiguration(null, "not provided");
        AssertNotProvided(iccConfig, source);
    }
    
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotSupportedHeader(Source source)
    {
        var profile = IccFile.CxHue45Abstract.GetProfile();
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        
        var iccConfig = new IccConfiguration(profile, Intent.Unspecified, "not supported header");
        AssertNotSupportedHeader(iccConfig, profile.Header, source);
    }
    
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotSupportedTransformDToB(Source source)
    {
        var bytes = GetProfileWithDToBTags();
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(bytes); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        Assert.Throws<NotSupportedException>(() => { profile.Transform.ToXyz([], Intent.Unspecified); });
        Assert.Throws<NotSupportedException>(() => { profile.Transform.FromXyz([], Intent.Unspecified); });
        
        var iccConfig = new IccConfiguration(bytes, Intent.Unspecified, "not supported transform");
        AssertNotSupportedTransformDToB(iccConfig, profile.Header, source);
    }
    
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotSupportedTransformNone(Source source)
    {
        var bytes = GetProfileWithNoTransformTags();
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(bytes); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        Assert.Throws<NotSupportedException>(() => { profile.Transform.ToXyz([], Intent.Unspecified); });
        Assert.Throws<NotSupportedException>(() => { profile.Transform.FromXyz([], Intent.Unspecified); });
        
        var iccConfig = new IccConfiguration(bytes, Intent.Unspecified, "not supported transform");
        AssertNotSupportedTransformNone(iccConfig, profile.Header, source);
    }
    
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotCorrectSignature(Source source)
    {
        var bytes = CorruptProfileSignature(IccFile.Fogra39);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(bytes); });
        Assert.Throws<ArgumentException>(() => { profile.ErrorIfUnsupported(); });
        
        var iccConfig = new IccConfiguration(bytes, Intent.Unspecified, "not correct signature");
        AssertNotCorrectProfileSignature(iccConfig, profile.Header, source);
    }
    
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotValidLut(Source source)
    {
        var bytes = CorruptLutSignature(IccFile.Swop2013);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(bytes); });
        Assert.DoesNotThrow(() => { profile.ErrorIfUnsupported(); });
        
        var iccConfig = new IccConfiguration(bytes, Intent.Unspecified, "not valid lut");
        AssertNotValidTagSignature(iccConfig, profile.Header, source);
    }
    
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotValidCurve(Source source)
    {
        var bytes = CorruptCurveSignature(IccFile.Swop2013);
        Profile profile = null!;
        Assert.DoesNotThrow(() => { profile = new Profile(bytes); });
        Assert.DoesNotThrow(() => { profile.ErrorIfUnsupported(); });
        
        var iccConfig = new IccConfiguration(bytes, Intent.Unspecified, "not valid curve");
        AssertNotValidTagSignature(iccConfig, profile.Header, source);
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
        Assert.That(iccConfig.Profile, Is.Not.Null);
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
        Assert.That(iccConfig.Profile, Is.Null);
        Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
        Assert.That(iccConfig.Error, Is.Null);
        Assert.That(actualUnicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(expectedUnicolour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
        Assert.That(actualUnicolour.Icc.Error, Is.Null);
        Assert.That(expectedUnicolour.Icc.Error, Is.Null);
    }
}