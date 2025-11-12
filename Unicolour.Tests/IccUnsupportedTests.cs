using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public abstract class IccUnsupportedTests
{
    private static Channels channels => new(0.8, 0.6, 0.4, 0.2);
    private static Rgb rgb => new(0.25, 0.5, 1.0);

    private const string ErrorNotFound = "could not find file";
    private const string ErrorNotEnoughBytes = "does not contain enough bytes";
    private const string ErrorNotParsed = "could not be parsed";
    private const string ErrorNotSupported = "not supported";
    private const string ErrorNotCorrectSignature = "signature is incorrect";
    
    internal static void AssertNotProvided(IccConfiguration iccConfig, Source source)
    {
        if (source == Source.FromChannels)
        {
            var expected = Channels.UncalibratedToRgb(channels);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, channels);
            TestUtils.AssertTriplet<Rgb>(colour, expected.Triplet, 0);
            Assert.That(iccConfig.Profile, Is.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
        else
        {
            var expected = Channels.UncalibratedFromRgb(rgb);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
            Assert.That(iccConfig.Profile, Is.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
            Assert.That(colour.Icc, Is.EqualTo(expected));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
    }
    
    internal static void AssertNotFound(IccConfiguration iccConfig, Source source)
    {
        if (source == Source.FromChannels)
        {
            var expected = Channels.UncalibratedToRgb(channels);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, channels);
            TestUtils.AssertTriplet<Rgb>(colour, expected.Triplet, 0);
            Assert.That(iccConfig.Profile, Is.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
            Assert.That(iccConfig.Error!.StartsWith(ErrorNotFound, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
        else
        {
            var expected = Channels.UncalibratedFromRgb(rgb);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
            Assert.That(iccConfig.Profile, Is.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
            Assert.That(iccConfig.Error!.StartsWith(ErrorNotFound, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc, Is.EqualTo(expected));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
    }
    
    internal static void AssertNotEnoughBytes(IccConfiguration iccConfig, Source source)
    {
        if (source == Source.FromChannels)
        {
            var expected = Channels.UncalibratedToRgb(channels);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, channels);
            TestUtils.AssertTriplet<Rgb>(colour, expected.Triplet, 0);
            Assert.That(iccConfig.Profile, Is.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
            Assert.That(iccConfig.Error!.Contains(ErrorNotEnoughBytes, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
        else
        {
            var expected = Channels.UncalibratedFromRgb(rgb);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
            Assert.That(iccConfig.Profile, Is.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
            Assert.That(iccConfig.Error!.Contains(ErrorNotEnoughBytes, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc, Is.EqualTo(expected));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
    }
    
    internal static void AssertNotParseable(IccConfiguration iccConfig, Source source)
    {
        if (source == Source.FromChannels)
        {
            var expected = Channels.UncalibratedToRgb(channels);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, channels);
            TestUtils.AssertTriplet<Rgb>(colour, expected.Triplet, 0);
            Assert.That(iccConfig.Profile, Is.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
            Assert.That(iccConfig.Error!.Contains(ErrorNotParsed, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
        else
        {
            var expected = Channels.UncalibratedFromRgb(rgb);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
            Assert.That(iccConfig.Profile, Is.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(Intent.Unspecified));
            Assert.That(iccConfig.Error!.Contains(ErrorNotParsed, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc, Is.EqualTo(expected));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
    }
    
    internal static void AssertNotSupportedHeader(IccConfiguration iccConfig, Header header, Source source)
    {
        if (source == Source.FromChannels)
        {
            var expected = Channels.UncalibratedToRgb(channels);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, channels);
            TestUtils.AssertTriplet<Rgb>(colour, expected.Triplet, 0);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error!.Contains(ErrorNotSupported, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
        else
        {
            var expected = Channels.UncalibratedFromRgb(rgb);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error!.Contains(ErrorNotSupported, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc, Is.EqualTo(expected));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
    }
    
    internal static void AssertNotSupportedTransformDToB(IccConfiguration iccConfig, Header header, Source source)
    {
        if (source == Source.FromChannels)
        {
            var expected = Channels.UncalibratedToRgb(channels);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, channels);
            TestUtils.AssertTriplet<Rgb>(colour, expected.Triplet, 0);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error!.Contains(ErrorNotSupported, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
        else
        {
            var expected = Channels.UncalibratedFromRgb(rgb);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error!.Contains(ErrorNotSupported, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc, Is.EqualTo(expected));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
    }
    
    internal static void AssertNotSupportedTransformNone(IccConfiguration iccConfig, Header header, Source source)
    {
        if (source == Source.FromChannels)
        {
            var expected = Channels.UncalibratedToRgb(channels);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, channels);
            TestUtils.AssertTriplet<Rgb>(colour, expected.Triplet, 0);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error!.Contains(ErrorNotSupported, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
        else
        {
            var expected = Channels.UncalibratedFromRgb(rgb);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error!.Contains(ErrorNotSupported, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc, Is.EqualTo(expected));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
    }
    
    internal static void AssertNotCorrectProfileSignature(IccConfiguration iccConfig, Header header, Source source)
    {
        if (source == Source.FromChannels)
        {
            var expected = Channels.UncalibratedToRgb(channels);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, channels);
            TestUtils.AssertTriplet<Rgb>(colour, expected.Triplet, 0);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error!.Contains(ErrorNotCorrectSignature, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
        else
        {
            var expected = Channels.UncalibratedFromRgb(rgb);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error!.Contains(ErrorNotCorrectSignature, StringComparison.CurrentCultureIgnoreCase));
            Assert.That(colour.Icc, Is.EqualTo(expected));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Channels.UncalibratedCmyk));
            Assert.That(colour.Icc.Error, Is.Null);
        }
    }
    
    internal static void AssertNotValidTagSignature(IccConfiguration iccConfig, Header header, Source source)
    {
        if (source == Source.FromChannels)
        {
            var expected = new Rgb(double.NaN, double.NaN, double.NaN);
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, channels);
            TestUtils.AssertTriplet<Rgb>(colour, expected.Triplet, 0);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error, Is.Null);
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Signatures.Cmyk));
            Assert.That(colour.Icc.Error!.Contains(nameof(ArgumentOutOfRangeException)));
        }
        else
        {
            var expected = Enumerable.Range(0, 15).Select(_ => double.NaN).ToArray();
            var config = new Configuration(iccConfig: iccConfig);
            var colour = new Unicolour(config, ColourSpace.Rgb, rgb.Tuple);
            Assert.That(iccConfig.Profile, Is.Not.Null);
            Assert.That(iccConfig.Intent, Is.EqualTo(header.Intent));
            Assert.That(iccConfig.Error, Is.Null);
            Assert.That(colour.Icc.Values, Is.EqualTo(expected));
            Assert.That(colour.Icc.ColourSpace, Is.EqualTo(Signatures.Cmyk));
            Assert.That(colour.Icc.Error!.Contains(nameof(ArgumentOutOfRangeException)));
        }
    }

    internal static byte[] CreateBytes(int length) => Enumerable.Range(0, length).Select(_ => (byte)119).ToArray();
    
    internal static byte[] CorruptProfileSignature(IccFile iccFile)
    {
        var bytes = File.ReadAllBytes(iccFile.Path);
        CorruptSignature(bytes, index: 36);
        return bytes;
    }

    internal static byte[] CorruptLutSignature(IccFile iccFile)
    {
        var profile = iccFile.GetProfile();
        var a2b0 = profile.Tags.Single(x => x.Signature == Signatures.AToB0);
        var b2a0 = profile.Tags.Single(x => x.Signature == Signatures.BToA0);
        
        // first bytes in the LUT data is the signature
        var bytes = File.ReadAllBytes(iccFile.Path);
        CorruptSignature(bytes, a2b0.Offset);
        CorruptSignature(bytes, b2a0.Offset);
        return bytes;
    }
    
    internal static byte[] CorruptCurveSignature(IccFile iccFile)
    {
        var profile = iccFile.GetProfile();
        var a2b0 = profile.Tags.Single(x => x.Signature == Signatures.AToB0);
        var b2a0 = profile.Tags.Single(x => x.Signature == Signatures.BToA0);
        
        // B curves start at byte 32 of LUT data, first bytes in the curve data is the signature
        var bytes = File.ReadAllBytes(iccFile.Path);
        CorruptSignature(bytes, a2b0.Offset + 32);
        CorruptSignature(bytes, b2a0.Offset + 32);
        return bytes;
    }
    
    internal static byte[] GetProfileWithDToBTags()
    {
        // with ROMM RGB profile: tag table index 1 = A2B0, tag table index 2 = B2A0
        var iccFile = IccFile.RommRgb;
        const uint a2b0TagTableIndex = 128 + 4 + 1 * 12;
        const uint b2a0TagTableIndex = 128 + 4 + 2 * 12;
        
        // ROMM RGB only has A2B0 and B2A0, this replaces both with D2B0 and B2D0 in the tag table
        var bytes = File.ReadAllBytes(iccFile.Path);
        ModifySignature(bytes, a2b0TagTableIndex, [68, 50, 66, 48]); // "D2B0"
        ModifySignature(bytes, b2a0TagTableIndex, [66, 50, 68, 48]); // "B2D0"
        return bytes;
    }
    
    internal static byte[] GetProfileWithNoTransformTags()
    {
        // with ROMM RGB profile: tag table index 1 = A2B0, tag table index 2 = B2A0
        var iccFile = IccFile.RommRgb;
        const uint a2b0TagTableIndex = 128 + 4 + 1 * 12;
        const uint b2a0TagTableIndex = 128 + 4 + 2 * 12;
        
        // ROMM RGB only has A2B0 and B2A0; this replaces both with "NONE" in the tag table
        // effectively creating a profile with no transform tags
        var bytes = File.ReadAllBytes(iccFile.Path);
        ModifySignature(bytes, a2b0TagTableIndex, [78, 79, 78, 69]); // "NONE"
        ModifySignature(bytes, b2a0TagTableIndex, [78, 79, 78, 69]); // "NONE"
        return bytes;
    }

    private static void CorruptSignature(byte[] bytes, uint index) => ModifySignature(bytes, index, [115, 109, 101, 103]);
    private static void ModifySignature(byte[] bytes, uint index, byte[] modifiedBytes)
    {
        // var result = bytes.ToArray();
        bytes[index + 0] = modifiedBytes[0];
        bytes[index + 1] = modifiedBytes[1];
        bytes[index + 2] = modifiedBytes[2];
        bytes[index + 3] = modifiedBytes[3];
    }
    
    public enum Source { FromChannels, FromRgb }
}