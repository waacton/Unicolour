using System;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class IccProfileTests
{
    [Test]
    public void Fogra39()
    {
        var profile = IccFile.Fogra39.GetProfile();
        var header = profile.Header;
        var tags = profile.Tags;
        
        Assert.That(profile.FileInfo.Length, Is.EqualTo(header.ProfileSize));
        
        Assert.That(header.ProfileSize, Is.EqualTo(8652444));
        Assert.That(header.PreferredCmmType, Is.EqualTo(Signatures.Null));
        Assert.That(header.ProfileVersion, Is.EqualTo(new Version(2, 1, 0)));
        Assert.That(header.ProfileClass, Is.EqualTo(Signatures.Output));
        Assert.That(header.DataColourSpace, Is.EqualTo(Signatures.Cmyk));
        Assert.That(header.Pcs, Is.EqualTo(Signatures.Lab));
        Assert.That(header.DateTime, Is.EqualTo(new DateTime(2013, 05, 29, 14, 36, 15)));
        Assert.That(header.ProfileFileSignature, Is.EqualTo(Signatures.Profile));
        Assert.That(header.PrimaryPlatform, Is.EqualTo("APPL"));
        Assert.That(header.ProfileFlags, Is.EqualTo(new[] { DataTypes.NotEmbedded, DataTypes.Independent }));
        Assert.That(header.DeviceManufacturer, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceModel, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceAttributes, Is.EqualTo(new[] { DataTypes.Reflective, DataTypes.Glossy, DataTypes.Positive, DataTypes.Colour }));
        Assert.That(header.Intent, Is.EqualTo(Intent.Perceptual));
        Assert.That(header.PcsIlluminant, Is.EqualTo((0.96420, 1.00000, 0.82491)).Within(0.000005));
        Assert.That(header.ProfileCreator, Is.EqualTo(Signatures.Null));
        Assert.That(header.ProfileId, Is.EqualTo(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
        
        Assert.That(tags.Count, Is.EqualTo(14));
        AssertTag(tags[0], "desc", 300, 177);
        AssertTag(tags[1], Signatures.MediaWhitePoint, 480, 20);
        AssertTag(tags[2], "bkpt", 500, 20);
        AssertTag(tags[3], "chad", 520, 44);
        AssertTag(tags[4], "cprt", 564, 31);
        AssertTag(tags[5], Signatures.AToB0, 596, 2376582);
        AssertTag(tags[6], Signatures.AToB1, 2377180, 2376582);
        AssertTag(tags[7], Signatures.AToB2, 4753764, 2376582);
        AssertTag(tags[8], Signatures.BToA0, 7130348, 462620);
        AssertTag(tags[9], Signatures.BToA1, 7592968, 462620);
        AssertTag(tags[10], Signatures.BToA2, 8055588, 462620);
        AssertTag(tags[11], "gamt", 8518208, 134126);
        AssertTag(tags[12], "view", 8652336, 36);
        AssertTag(tags[13], "meta", 8652372, 72);

        AssertLuts(tags.AToB0.Value, is16Bit: true, (4, 4096), (3, 2), (25, "[25,25,25,25,3]"));
        AssertLuts(tags.AToB1.Value, is16Bit: true, (4, 4096), (3, 2), (25, "[25,25,25,25,3]"));
        AssertLuts(tags.AToB2.Value, is16Bit: true, (4, 4096), (3, 2), (25, "[25,25,25,25,3]"));
        AssertLuts(tags.BToA0.Value, is16Bit: true, (3, 4096), (4, 4096), (37, "[37,37,37,4]"));
        AssertLuts(tags.BToA1.Value, is16Bit: true, (3, 4096), (4, 4096), (37, "[37,37,37,4]"));
        AssertLuts(tags.BToA2.Value, is16Bit: true, (3, 4096), (4, 4096), (37, "[37,37,37,4]"));
        Assert.That(tags.MediaWhite.Value, Is.EqualTo((0.84541, 0.87683, 0.74628)).Within(0.000005));
    }
    
    [Test]
    public void Fogra55()
    {
        var profile = IccFile.Fogra55.GetProfile();
        var header = profile.Header;
        var tags = profile.Tags;
        
        Assert.That(profile.FileInfo.Length, Is.EqualTo(header.ProfileSize));
        
        Assert.That(header.ProfileSize, Is.EqualTo(2547980));
        Assert.That(header.PreferredCmmType, Is.EqualTo("APPL"));
        Assert.That(header.ProfileVersion, Is.EqualTo(new Version(2, 4, 0)));
        Assert.That(header.ProfileClass, Is.EqualTo(Signatures.Output));
        Assert.That(header.DataColourSpace, Is.EqualTo(Signatures.Clr7));
        Assert.That(header.Pcs, Is.EqualTo(Signatures.Lab));
        Assert.That(header.DateTime, Is.EqualTo(new DateTime(2021, 05, 25, 09, 28, 46)));
        Assert.That(header.ProfileFileSignature, Is.EqualTo(Signatures.Profile));
        Assert.That(header.PrimaryPlatform, Is.EqualTo("APPL"));
        Assert.That(header.ProfileFlags, Is.EqualTo(new[] { DataTypes.NotEmbedded, DataTypes.Independent }));
        Assert.That(header.DeviceManufacturer, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceModel, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceAttributes, Is.EqualTo(new[] { DataTypes.Reflective, DataTypes.Glossy, DataTypes.Positive, DataTypes.Colour }));
        Assert.That(header.Intent, Is.EqualTo(Intent.Perceptual));
        Assert.That(header.PcsIlluminant, Is.EqualTo((0.96420, 1.00000, 0.82491)).Within(0.000005));
        Assert.That(header.ProfileCreator, Is.EqualTo("CoLg"));
        Assert.That(header.ProfileId, Is.EqualTo(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
        
        Assert.That(tags.Count, Is.EqualTo(16));
        AssertTag(tags[0], "desc", 324, 205);
        AssertTag(tags[1], "cprt", 532, 74);
        AssertTag(tags[2], Signatures.MediaWhitePoint, 608, 20);
        AssertTag(tags[3], "bkpt", 628, 20);
        AssertTag(tags[4], "kTRC", 648, 524);
        AssertTag(tags[5], Signatures.AToB0, 1172, 473922);
        AssertTag(tags[6], Signatures.AToB1, 475096, 473922);
        AssertTag(tags[7], Signatures.AToB2, 949020, 103476);
        AssertTag(tags[8], Signatures.BToA0, 1052496, 508290);
        AssertTag(tags[9], Signatures.BToA1, 1560788, 508290);
        AssertTag(tags[10], Signatures.BToA2, 2069080, 73954);
        AssertTag(tags[11], "gamt", 2143036, 72450);
        AssertTag(tags[12], "Info", 2215488, 1335);
        AssertTag(tags[13], "evts", 2216824, 1350);
        AssertTag(tags[14], "targ", 2218176, 329524);
        AssertTag(tags[15], "clrt", 2547700, 278);

        AssertLuts(tags.AToB0.Value, is16Bit: true, (7, 256), (3, 256), (5, "[5,5,5,5,5,5,5,3]"));
        AssertLuts(tags.AToB1.Value, is16Bit: true, (7, 256), (3, 256), (5, "[5,5,5,5,5,5,5,3]"));
        AssertLuts(tags.AToB2.Value, is16Bit: true, (7, 256), (3, 256), (4, "[4,4,4,4,4,4,4,3]"));
        AssertLuts(tags.BToA0.Value, is16Bit: true, (3, 256), (7, 256), (33, "[33,33,33,7]"));
        AssertLuts(tags.BToA1.Value, is16Bit: true, (3, 256), (7, 256), (33, "[33,33,33,7]"));
        AssertLuts(tags.BToA2.Value, is16Bit: true, (3, 256), (7, 256), (17, "[17,17,17,7]"));
        Assert.That(tags.MediaWhite.Value, Is.EqualTo((0.85278, 0.87619, 0.79289)).Within(0.000005));
    }
    
    [Test]
    public void JapanColor2011()
    {
        var profile = IccFile.JapanColor2011.GetProfile();
        var header = profile.Header;
        var tags = profile.Tags;
        
        Assert.That(profile.FileInfo.Length, Is.EqualTo(header.ProfileSize));
        
        Assert.That(header.ProfileSize, Is.EqualTo(1979304));
        Assert.That(header.PreferredCmmType, Is.EqualTo(Signatures.Null));
        Assert.That(header.ProfileVersion, Is.EqualTo(new Version(2, 4, 0)));
        Assert.That(header.ProfileClass, Is.EqualTo(Signatures.Output));
        Assert.That(header.DataColourSpace, Is.EqualTo(Signatures.Cmyk));
        Assert.That(header.Pcs, Is.EqualTo(Signatures.Lab));
        Assert.That(header.DateTime, Is.EqualTo(new DateTime(2011, 09, 13, 18, 01, 29)));
        Assert.That(header.ProfileFileSignature, Is.EqualTo(Signatures.Profile));
        Assert.That(header.PrimaryPlatform, Is.EqualTo("APPL"));
        Assert.That(header.ProfileFlags, Is.EqualTo(new[] { DataTypes.NotEmbedded, DataTypes.Independent }));
        Assert.That(header.DeviceManufacturer, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceModel, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceAttributes, Is.EqualTo(new[] { DataTypes.Reflective, DataTypes.Glossy, DataTypes.Positive, DataTypes.Colour }));
        Assert.That(header.Intent, Is.EqualTo(Intent.Perceptual));
        Assert.That(header.PcsIlluminant, Is.EqualTo((0.96420, 1.00000, 0.82491)).Within(0.000005));
        Assert.That(header.ProfileCreator, Is.EqualTo("XRIT"));
        Assert.That(header.ProfileId, Is.EqualTo(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
        
        Assert.That(tags.Count, Is.EqualTo(10));
        AssertTag(tags[0], "desc", 252, 164);
        AssertTag(tags[1], Signatures.MediaWhitePoint, 416, 20);
        AssertTag(tags[2], "cprt", 436, 31);
        AssertTag(tags[3], Signatures.AToB0, 468, 252403);
        AssertTag(tags[4], Signatures.AToB1, 252872, 533958);
        AssertTag(tags[5], Signatures.AToB2, 786832, 252403);
        AssertTag(tags[6], Signatures.BToA0, 1039236, 344892);
        AssertTag(tags[7], Signatures.BToA1, 1384128, 344892);
        AssertTag(tags[8], Signatures.BToA2, 1729020, 145588);
        AssertTag(tags[9], "gamt", 1874608, 104694);

        AssertLuts(tags.AToB0.Value, is16Bit: false, (4, 256), (3, 256), (17, "[17,17,17,17,3]"));
        AssertLuts(tags.AToB1.Value, is16Bit: true, (4, 4096), (3, 2), (17, "[17,17,17,17,3]"));
        AssertLuts(tags.AToB2.Value, is16Bit: false, (4, 256), (3, 256), (17, "[17,17,17,17,3]"));
        AssertLuts(tags.BToA0.Value, is16Bit: true, (3, 4096), (4, 4096), (33, "[33,33,33,4]"));
        AssertLuts(tags.BToA1.Value, is16Bit: true, (3, 4096), (4, 4096), (33, "[33,33,33,4]"));
        AssertLuts(tags.BToA2.Value, is16Bit: false, (3, 256), (4, 256), (33, "[33,33,33,4]"));
        Assert.That(tags.MediaWhite.Value, Is.EqualTo((0.82787, 0.85606, 0.72072)).Within(0.000005));
    }
    
    private static void AssertTag(Tag tag, string signature, int offset, int size)
    {
        Assert.That(tag.Signature, Is.EqualTo(signature));
        Assert.That(tag.Offset, Is.EqualTo(offset));
        Assert.That(tag.Size, Is.EqualTo(size));
        Assert.That(tag.ToString().StartsWith(signature));
        Assert.That(tag.ToString().Contains($"byte {offset}"));
        Assert.That(tag.ToString().EndsWith($"{size} bytes"));
    }

    private static void AssertLuts(Luts luts, bool is16Bit,
        (int channels, int entries) input, 
        (int channels, int entries) output,
        (int points, string text) clut)
    {
        Assert.That(luts.Is16Bit, Is.EqualTo(is16Bit));
        
        Assert.That(luts.InputCurves.Count, Is.EqualTo(input.channels));
        foreach (var curve in luts.InputCurves)
        {
            Assert.That(curve.Table.Length, Is.EqualTo(input.entries));
        }

        Assert.That(luts.OutputCurves.Count, Is.EqualTo(output.channels));
        foreach (var curve in luts.OutputCurves)
        {
            Assert.That(curve.Table.Length, Is.EqualTo(output.entries));
        }

        Assert.That(luts.Clut.InputChannels, Is.EqualTo(input.channels));
        Assert.That(luts.Clut.GridPoints, Is.EqualTo(clut.points));
        Assert.That(luts.Clut.OutputChannels, Is.EqualTo(output.channels));

        Assert.That(luts.Clut.ToString(), Is.EqualTo(clut.text));
        Assert.That(luts.ToString().StartsWith($"{input.channels}x curves"));
        Assert.That(luts.ToString().Contains(clut.text));
        Assert.That(luts.ToString().EndsWith($"{output.channels}x curves"));
    }
}