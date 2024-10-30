using System;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;
using Convert = System.Convert;

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

        AssertLuts(tags.AToB0.Value!, lutType: LutType.Lut16, LutElements.AB, 4, 3, [25, 25, 25, 25, 3]);
        AssertLuts(tags.AToB1.Value!, lutType: LutType.Lut16, LutElements.AB, 4, 3, [25, 25, 25, 25, 3]);
        AssertLuts(tags.AToB2.Value!, lutType: LutType.Lut16, LutElements.AB, 4, 3, [25, 25, 25, 25, 3]);
        AssertLuts(tags.BToA0.Value!, lutType: LutType.Lut16, LutElements.BA, 4, 3, [37, 37, 37, 4]);
        AssertLuts(tags.BToA1.Value!, lutType: LutType.Lut16, LutElements.BA, 4, 3, [37, 37, 37, 4]);
        AssertLuts(tags.BToA2.Value!, lutType: LutType.Lut16, LutElements.BA, 4, 3, [37, 37, 37, 4]);
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

        AssertLuts(tags.AToB0.Value!, lutType: LutType.Lut16, LutElements.AB, 7, 3, [5, 5, 5, 5, 5, 5, 5, 3]);
        AssertLuts(tags.AToB1.Value!, lutType: LutType.Lut16, LutElements.AB, 7, 3, [5, 5, 5, 5, 5, 5, 5, 3]);
        AssertLuts(tags.AToB2.Value!, lutType: LutType.Lut16, LutElements.AB, 7, 3, [4, 4, 4, 4, 4, 4, 4, 3]);
        AssertLuts(tags.BToA0.Value!, lutType: LutType.Lut16, LutElements.BA, 7, 3, [33, 33, 33, 7]);
        AssertLuts(tags.BToA1.Value!, lutType: LutType.Lut16, LutElements.BA, 7, 3, [33, 33, 33, 7]);
        AssertLuts(tags.BToA2.Value!, lutType: LutType.Lut16, LutElements.BA, 7, 3, [17, 17, 17, 7]);
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

        AssertLuts(tags.AToB0.Value!, lutType: LutType.Lut8, LutElements.AB, 4, 3, [17, 17, 17, 17, 3]);
        AssertLuts(tags.AToB1.Value!, lutType: LutType.Lut16, LutElements.AB, 4, 3, [17, 17, 17, 17, 3]);
        AssertLuts(tags.AToB2.Value!, lutType: LutType.Lut8, LutElements.AB, 4, 3, [17, 17, 17, 17, 3]);
        AssertLuts(tags.BToA0.Value!, lutType: LutType.Lut16, LutElements.BA, 4, 3, [33, 33, 33, 4]);
        AssertLuts(tags.BToA1.Value!, lutType: LutType.Lut16, LutElements.BA, 4, 3, [33, 33, 33, 4]);
        AssertLuts(tags.BToA2.Value!, lutType: LutType.Lut8, LutElements.BA, 4, 3, [33, 33, 33, 4]);
        Assert.That(tags.MediaWhite.Value, Is.EqualTo((0.82787, 0.85606, 0.72072)).Within(0.000005));
    }
    
    [Test]
    public void Prmg()
    {
        var profile = IccFile.Prmg.GetProfile();
        var header = profile.Header;
        var tags = profile.Tags;
        
        Assert.That(profile.FileInfo.Length, Is.EqualTo(header.ProfileSize));
        
        Assert.That(header.ProfileSize, Is.EqualTo(2601512));
        Assert.That(header.PreferredCmmType, Is.EqualTo("appl"));
        Assert.That(header.ProfileVersion, Is.EqualTo(new Version(4, 2, 0)));
        Assert.That(header.ProfileClass, Is.EqualTo(Signatures.Output));
        Assert.That(header.DataColourSpace, Is.EqualTo(Signatures.Cmyk));
        Assert.That(header.Pcs, Is.EqualTo(Signatures.Lab));
        Assert.That(header.DateTime, Is.EqualTo(new DateTime(2013, 02, 25, 15, 18, 08)));
        Assert.That(header.ProfileFileSignature, Is.EqualTo(Signatures.Profile));
        Assert.That(header.PrimaryPlatform, Is.EqualTo("MSFT"));
        Assert.That(header.ProfileFlags, Is.EqualTo(new[] { DataTypes.NotEmbedded, DataTypes.Independent }));
        Assert.That(header.DeviceManufacturer, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceModel, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceAttributes, Is.EqualTo(new[] { DataTypes.Reflective, DataTypes.Glossy, DataTypes.Positive, DataTypes.Colour }));
        Assert.That(header.Intent, Is.EqualTo(Intent.Perceptual));
        Assert.That(header.PcsIlluminant, Is.EqualTo((0.96420, 1.00000, 0.82491)).Within(0.000005));
        Assert.That(header.ProfileCreator, Is.EqualTo("KODA"));
        Assert.That(header.ProfileId, Is.EqualTo(HexToBytes("a80356bf-2ac2ddb3-6e263ccd-ccf31828")));
        Assert.That(profile.CalculateProfileId(), Is.EqualTo(header.ProfileId));
        
        Assert.That(tags.Count, Is.EqualTo(13));
        AssertTag(tags[0], "cprt", 288, 166);
        AssertTag(tags[1], "desc", 2601448, 64);
        AssertTag(tags[2], Signatures.MediaWhitePoint, 456, 20);
        AssertTag(tags[3], Signatures.AToB1, 476, 503324);
        AssertTag(tags[4], Signatures.BToA1, 539856, 321824);
        AssertTag(tags[5], Signatures.AToB0, 1183504, 503324);
        AssertTag(tags[6], Signatures.BToA0, 861680, 321824);
        AssertTag(tags[7], Signatures.AToB2, 2008652, 503324);
        AssertTag(tags[8], Signatures.BToA2, 1686828, 321824);
        AssertTag(tags[9], "gamt", 503800, 36056);
        AssertTag(tags[10], "targ", 2511976, 88308);
        AssertTag(tags[11], "imnK", 2600284, 762);
        AssertTag(tags[12], "imnM", 2601048, 399);

        AssertLuts(tags.AToB0.Value!, lutType: LutType.LutAB, LutElements.AB, 4, 3, [17, 17, 17, 17, 3]);
        AssertLuts(tags.AToB1.Value!, lutType: LutType.LutAB, LutElements.AB, 4, 3, [17, 17, 17, 17, 3]);
        AssertLuts(tags.AToB2.Value!, lutType: LutType.LutAB, LutElements.AB, 4, 3, [17, 17, 17, 17, 3]);
        AssertLuts(tags.BToA0.Value!, lutType: LutType.LutBA, LutElements.BA, 4, 3, [33, 33, 33, 4]);
        AssertLuts(tags.BToA1.Value!, lutType: LutType.LutBA, LutElements.BA, 4, 3, [33, 33, 33, 4]);
        AssertLuts(tags.BToA2.Value!, lutType: LutType.LutBA, LutElements.BA, 4, 3, [33, 33, 33, 4]);
        Assert.That(tags.MediaWhite.Value, Is.EqualTo((0.96420, 1.00000, 0.82491)).Within(0.000005));
    }
    
    [Test]
    public void StandardRgbV4()
    {
        var profile = IccFile.StandardRgbV4.GetProfile();
        var header = profile.Header;
        var tags = profile.Tags;
        
        Assert.That(profile.FileInfo.Length, Is.EqualTo(header.ProfileSize));
        
        Assert.That(header.ProfileSize, Is.EqualTo(60960));
        Assert.That(header.PreferredCmmType, Is.EqualTo(Signatures.Null));
        Assert.That(header.ProfileVersion, Is.EqualTo(new Version(4, 2, 0)));
        Assert.That(header.ProfileClass, Is.EqualTo(Signatures.ColourSpace));
        Assert.That(header.DataColourSpace, Is.EqualTo(Signatures.Rgb));
        Assert.That(header.Pcs, Is.EqualTo(Signatures.Lab));
        Assert.That(header.DateTime, Is.EqualTo(new DateTime(2007, 07, 25, 00, 05, 37)));
        Assert.That(header.ProfileFileSignature, Is.EqualTo(Signatures.Profile));
        Assert.That(header.PrimaryPlatform, Is.EqualTo(Signatures.Null));
        Assert.That(header.ProfileFlags, Is.EqualTo(new[] { DataTypes.NotEmbedded, DataTypes.Independent }));
        Assert.That(header.DeviceManufacturer, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceModel, Is.EqualTo(Signatures.Null));
        Assert.That(header.DeviceAttributes, Is.EqualTo(new[] { DataTypes.Reflective, DataTypes.Glossy, DataTypes.Positive, DataTypes.Colour }));
        Assert.That(header.Intent, Is.EqualTo(Intent.Perceptual));
        Assert.That(header.PcsIlluminant, Is.EqualTo((0.96420, 1.00000, 0.82491)).Within(0.000005));
        Assert.That(header.ProfileCreator, Is.EqualTo(Signatures.Null));
        Assert.That(header.ProfileId, Is.EqualTo(HexToBytes("34562abf-994ccd06-6d2c5721-d0d68c5d")));
        Assert.That(profile.CalculateProfileId(), Is.EqualTo(header.ProfileId));
        
        Assert.That(tags.Count, Is.EqualTo(9));
        AssertTag(tags[0], "desc", 240, 118);
        AssertTag(tags[1], Signatures.AToB0, 360, 29712);
        AssertTag(tags[2], Signatures.AToB1, 30072, 436);
        AssertTag(tags[3], Signatures.BToA0, 30508, 29748);
        AssertTag(tags[4], Signatures.BToA1, 60256, 508);
        AssertTag(tags[5], "rig0", 60764, 12);
        AssertTag(tags[6], Signatures.MediaWhitePoint, 60776, 20);
        AssertTag(tags[7], "cprt", 60796, 118);
        AssertTag(tags[8], "chad", 60916, 44);

        AssertLuts(tags.AToB0.Value!, lutType: LutType.LutAB, LutElements.AMB, 3, 3, [17, 17, 17, 3]);
        AssertLuts(tags.AToB1.Value!, lutType: LutType.LutAB, LutElements.AMB, 3, 3, [2, 2, 2, 3]);
        AssertLuts(tags.BToA0.Value!, lutType: LutType.LutBA, LutElements.BMA, 3, 3, [17, 17, 17, 3]);
        AssertLuts(tags.BToA1.Value!, lutType: LutType.LutBA, LutElements.BMA, 3, 3, [2, 2, 2, 3]);
        Assert.That(tags.MediaWhite.Value, Is.EqualTo((0.96420, 1.00000, 0.82491)).Within(0.000005));
    }
    
    [Test]
    public void RommRgb()
    {
        var profile = IccFile.RommRgb.GetProfile();
        var header = profile.Header;
        var tags = profile.Tags;
        
        Assert.That(profile.FileInfo.Length, Is.EqualTo(header.ProfileSize));
        
        Assert.That(header.ProfileSize, Is.EqualTo(864));
        Assert.That(header.PreferredCmmType, Is.EqualTo("none"));
        Assert.That(header.ProfileVersion, Is.EqualTo(new Version(4, 0, 0)));
        Assert.That(header.ProfileClass, Is.EqualTo(Signatures.ColourSpace));
        Assert.That(header.DataColourSpace, Is.EqualTo(Signatures.Rgb));
        Assert.That(header.Pcs, Is.EqualTo(Signatures.Xyz));
        Assert.That(header.DateTime, Is.EqualTo(new DateTime(2006, 11, 19, 15, 19, 53)));
        Assert.That(header.ProfileFileSignature, Is.EqualTo(Signatures.Profile));
        Assert.That(header.PrimaryPlatform, Is.EqualTo(Signatures.Null));
        Assert.That(header.ProfileFlags, Is.EqualTo(new[] { DataTypes.NotEmbedded, DataTypes.Independent }));
        Assert.That(header.DeviceManufacturer, Is.EqualTo("none"));
        Assert.That(header.DeviceModel, Is.EqualTo("none"));
        Assert.That(header.DeviceAttributes, Is.EqualTo(new[] { DataTypes.Reflective, DataTypes.Glossy, DataTypes.Positive, DataTypes.Colour }));
        Assert.That(header.Intent, Is.EqualTo(Intent.Perceptual));
        Assert.That(header.PcsIlluminant, Is.EqualTo((0.96420, 1.00000, 0.82491)).Within(0.000005));
        Assert.That(header.ProfileCreator, Is.EqualTo("none"));
        Assert.That(header.ProfileId, Is.EqualTo(HexToBytes("2c98a166-95257d52-13906e04-02c0eac9")));
        Assert.That(profile.CalculateProfileId(), Is.EqualTo(header.ProfileId));
        
        Assert.That(tags.Count, Is.EqualTo(6));
        AssertTag(tags[0], "desc", 204, 84);
        AssertTag(tags[1], Signatures.AToB0, 288, 212);
        AssertTag(tags[2], Signatures.BToA0, 500, 212);
        AssertTag(tags[3], Signatures.MediaWhitePoint, 712, 20);
        AssertTag(tags[4], "cprt", 732, 88);
        AssertTag(tags[5], "chad", 820, 44);

        AssertLuts(tags.AToB0.Value!, lutType: LutType.LutAB, LutElements.MB, 3, 3, null);
        AssertLuts(tags.BToA0.Value!, lutType: LutType.LutBA, LutElements.BM, 3, 3, null);
        Assert.That(tags.MediaWhite.Value, Is.EqualTo((0.85809, 0.89000, 0.73421)).Within(0.000005));
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

    private static void AssertLuts(Luts luts, LutType lutType, LutElements lutElements, int aCount, int bCount, double[]? clutDimensions)
    {
        Assert.That(luts.LutType, Is.EqualTo(lutType));
        Assert.That(luts.LutElements, Is.EqualTo(lutElements));

        var expectedStringStart = luts.IsAToB switch
        {
            true when lutElements is LutElements.MB => $"M [x{bCount}]",
            true => $"A [x{aCount}]",
            false => $"B [x{bCount}]"
        };
        
        var expectedStringEnd = luts.IsAToB switch
        {
            false when lutElements is LutElements.BM => $"M [x{bCount}]",
            false => $"A [x{aCount}]",
            true => $"B [x{bCount}]"
        };
        
        Assert.That(luts.ToString().StartsWith(expectedStringStart));
        Assert.That(luts.ToString().EndsWith(expectedStringEnd));
        
        if (clutDimensions == null) return;
        var clutString = $"[{string.Join(",", clutDimensions)}]";
        Assert.That(luts.Clut!.ToString(), Is.EqualTo(clutString));
        Assert.That(luts.ToString().Contains(clutString));
    }

    private static byte[] HexToBytes(string hex)
    {
        hex = hex.Replace("-", string.Empty);

        byte GetByte(int index)
        {
            var substring = hex.Substring(index * 2, 2);
            return Convert.ToByte(substring, fromBase: 16);
        }
        
        var bytes = Enumerable.Range(0, hex.Length / 2).Select(GetByte).ToArray();
        return bytes;
    }
}