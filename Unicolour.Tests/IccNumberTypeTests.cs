using System.IO;
using NUnit.Framework;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests;

public class IccNumberTypeTests
{
    private static readonly byte[] testBytes = [85, 110, 105, 99, 111, 108, 111, 117, 114, 33];

    [TestCase(0, new byte[] { })]
    [TestCase(1, new byte[] { 85 })]
    [TestCase(2, new byte[] { 85, 110 })]
    [TestCase(10, new byte[] { 85, 110, 105, 99, 111, 108, 111, 117, 114, 33 })]
    [TestCase(11, null)]
    public void ReadBytes(int count, byte[]? expected)
    {
        using var stream = Stream(testBytes);

        if (expected == null)
        {
            Assert.Throws<InvalidDataException>(() => stream.ReadBytes(count));
            return;
        }

        var actual = stream.ReadBytes(count);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(new byte[] { 0 }, 0, TestName = "0 (min byte)")]
    [TestCase(new byte[] { 127 }, 127, TestName = "127 (halfway)")]
    [TestCase(new byte[] { 255 }, 255, TestName = "255 (max byte)")]
    public void UInt8(byte[] bytes, byte expected)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadUInt8();
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [TestCase(new byte[] { 0, 0 }, (ushort)0, TestName = "0 (min ushort)")]
    [TestCase(new byte[] { 0, 255 }, (ushort)255, TestName = "255 (least significant byte)")]
    [TestCase(new byte[] { 127, 127 }, (ushort)32639, TestName = "32639 (halfway bytes)")]
    [TestCase(new byte[] { 127, 255 }, (ushort)32767, TestName = "32767 (max short)")]
    [TestCase(new byte[] { 255, 0 }, (ushort)65280, TestName = "65280 (most significant byte)")]
    [TestCase(new byte[] { 255, 255 }, (ushort)65535, TestName = "65535 (max ushort)")]
    public void UInt16(byte[] bytes, ushort expected)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadUInt16();
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [TestCase(new byte[] { 0, 0, 0, 0 }, (uint)0, TestName = "0 (min uint)")]
    [TestCase(new byte[] { 0, 0, 0, 255 }, (uint)255, TestName = "255 (least significant byte)")]
    [TestCase(new byte[] { 127, 127, 127, 127 }, (uint)2139062143, TestName = "2139062143 (halfway bytes)")]
    [TestCase(new byte[] { 127, 255, 255, 255 }, (uint)2147483647, TestName = "2147483647 (max int)")]
    [TestCase(new byte[] { 255, 0, 0, 0 }, (uint)4278190080, TestName = "4278190080 (most significant byte)")]
    [TestCase(new byte[] { 255, 255, 255, 255 }, (uint)4294967295, TestName = "4294967295 (max uint)")]
    public void UInt32(byte[] bytes, uint expected)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadUInt32();
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, (ulong)0, TestName = "0 (min ulong)")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 255 }, (ulong)255, TestName = "255 (least significant byte)")]
    [TestCase(new byte[] { 127, 127, 127, 127, 127, 127, 127, 127 }, (ulong)9187201950435737471, TestName = "9187201950435737471 (halfway bytes)")]
    [TestCase(new byte[] { 127, 255, 255, 255, 255, 255, 255, 255 }, (ulong)9223372036854775807, TestName = "9223372036854775807 (max long)")]
    [TestCase(new byte[] { 255, 0, 0, 0, 0, 0, 0, 0 }, (ulong)18374686479671623680, TestName = "18374686479671623680 (most significant byte)")]
    [TestCase(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }, (ulong)18446744073709551615, TestName = "18446744073709551615 (max ulong)")]
    public void UInt64(byte[] bytes, ulong expected)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadUInt64();
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [TestCase(new byte[] { 0x80, 0x00, 0x00, 0x00 }, -32768, TestName = "-32768 (min: sign 1, integer 0s, fraction 0s)")]
    [TestCase(new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, TestName = "0")]
    [TestCase(new byte[] { 0x00, 0x01, 0x00, 0x00 }, 1, TestName = "1")]
    [TestCase(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF }, 32767 + 65535.0 / 65536.0, TestName = "32767.999985 (max: sign 0, integer 1s, fraction 1s)")]
    [TestCase(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }, -1 + 65535.0 / 65536.0, TestName = "-0.000015 (largest negative: sign 1, integer 1s, fraction 1s)")]
    [TestCase(new byte[] { 0x7F, 0xFF, 0x00, 0x00 }, 32767, TestName = "32767 (sign 0, integer 1s, fraction 0s)")]
    [TestCase(new byte[] { 0xFF, 0xFF, 0x00, 0x00 }, -1, TestName = "-1 (sign 1, integer 1s, fraction 0s)")]
    [TestCase(new byte[] { 0x00, 0x00, 0xFF, 0xFF }, 65535.0 / 65536.0, TestName = "0.999985 (sign 0, integer 0s, fraction 1s)")]
    [TestCase(new byte[] { 0x80, 0x00, 0xFF, 0xFF }, -32768 + 65535.0 / 65536.0, TestName = "-32,767.000015 (sign 1, integer 0s, fraction 1s)")]
    public void S15Fixed16(byte[] bytes, double expected)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadS15Fixed16();
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [TestCase(new byte[] { 0x00, 0x00 }, 0, TestName = "0")]
    [TestCase(new byte[] { 0x01, 0x00 }, 1, TestName = "1")]
    [TestCase(new byte[] { 0xFF, 0xFF }, 255 + 255.0 / 256.0, TestName = "255.99609375 (integer 1s, fraction 1s)")]
    [TestCase(new byte[] { 0xFF, 0x00 }, 255, TestName = "255 (integer 1s, fraction 0s)")]
    [TestCase(new byte[] { 0x00, 0xFF }, 255.0 / 256.0, TestName = "0.99609375 (integer 0s, fraction 1s)")]
    public void U8Fixed8(byte[] bytes, double expected)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadU8Fixed8();
        Assert.That(actual, Is.EqualTo(expected));
    }

    private static MemoryStream Stream(byte[] bytes) => new(bytes);
}