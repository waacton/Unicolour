using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using static Wacton.Unicolour.Icc.DataTypes;

namespace Wacton.Unicolour.Tests;

public class IccDataTypeTests
{
    [TestCase(new byte[] { 97, 99, 115, 112 }, "acsp")]
    [TestCase(new byte[] { 0, 255, 32, 127 }, "\0? \u007f")]
    public void Signature(byte[] bytes, string expected)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadSignature();
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(new byte[] { 0, 0b00000000, 0, 0 }, 0, 0, 0, TestName = "Min version")]
    [TestCase(new byte[] { 1, 0b00001111, 0, 127 }, 1, 0, 15, TestName = "No major version")]
    [TestCase(new byte[] { 127, 0b10100101, 127, 127 }, 127, 10, 5, TestName = "Normal version")]
    [TestCase(new byte[] { 254, 0b11110000, 127, 255 }, 254, 15, 0, TestName = "No minor version")]
    [TestCase(new byte[] { 255, 0b11111111, 255, 255 }, 255, 15, 15, TestName = "Max version")]
    public void Version(byte[] bytes, int expectedMajor, int expectedMinor, int expectedBugFix)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadVersion();
        Assert.That(actual, Is.EqualTo(new Version(expectedMajor, expectedMinor, expectedBugFix)));
    }
    
    private static List<TestCaseData> DateTimeTestCases =
    [
        new TestCaseData(new byte[] { 7, 208, 0, 1, 0, 2, 0, 3, 0, 4, 0, 5 }, new DateTime(2000, 01, 02, 03, 04, 05, DateTimeKind.Utc)).SetName("Normal date"),
        new TestCaseData(new byte[] { 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 }, new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc)).SetName("Min date"),
        new TestCaseData(new byte[] { 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 }, null).SetName("Year < 1"),
        new TestCaseData(new byte[] { 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, null).SetName("Month < 1"),
        new TestCaseData(new byte[] { 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, null).SetName("Day < 1"),
        new TestCaseData(new byte[] { 39, 15, 0, 12, 0, 31, 0, 23, 0, 59, 0, 59 }, new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc)).SetName("Max date"),
        new TestCaseData(new byte[] { 39, 16, 0, 12, 0, 31, 0, 23, 0, 59, 0, 59 }, null).SetName("Year > 9999"),
        new TestCaseData(new byte[] { 39, 15, 0, 13, 0, 31, 0, 23, 0, 59, 0, 59 }, null).SetName("Month > 12"),
        new TestCaseData(new byte[] { 39, 15, 0, 12, 0, 32, 0, 23, 0, 59, 0, 59 }, null).SetName("Day > 31"),
        new TestCaseData(new byte[] { 39, 15, 0, 12, 0, 31, 0, 24, 0, 59, 0, 59 }, null).SetName("Hour > 23"),
        new TestCaseData(new byte[] { 39, 15, 0, 12, 0, 31, 0, 23, 0, 60, 0, 59 }, null).SetName("Minute > 59"),
        new TestCaseData(new byte[] { 39, 15, 0, 12, 0, 31, 0, 23, 0, 59, 0, 60 }, null).SetName("Second > 59")
    ];
    
    [TestCaseSource(nameof(DateTimeTestCases))]
    public void DateTime(byte[] bytes, DateTime? expected)
    {
        using var stream = Stream(bytes);

        if (!expected.HasValue)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadDateTime());
            return;
        }
        
        var actual = stream.ReadDateTime();
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actual.Kind, Is.EqualTo(DateTimeKind.Utc));
    }
    
    [TestCase(new byte[] { 0, 0, 0, 0b00000000 }, new[] { NotEmbedded, Independent }, TestName = "00")]
    [TestCase(new byte[] { 0, 0, 0, 0b00000001 }, new[] { Embedded, Independent }, TestName = "01")]
    [TestCase(new byte[] { 0, 0, 0, 0b00000010 }, new[] { NotEmbedded, NotIndependent }, TestName = "10")]
    [TestCase(new byte[] { 0, 0, 0, 0b00000011 }, new[] { Embedded, NotIndependent }, TestName = "11")]
    public void ProfileFlags(byte[] bytes, string[] expected)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadProfileFlags();
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00000000 }, new[] { Reflective, Glossy, Positive, Colour }, TestName = "0000")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00000001 }, new[] { Transparency, Glossy, Positive, Colour }, TestName = "0001")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00000010 }, new[] { Reflective, Matte, Positive, Colour }, TestName = "0010")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00000011 }, new[] { Transparency, Matte, Positive, Colour }, TestName = "0011")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00000100 }, new[] { Reflective, Glossy, Negative, Colour }, TestName = "0100")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00000101 }, new[] { Transparency, Glossy, Negative, Colour }, TestName = "0101")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00000110 }, new[] { Reflective, Matte, Negative, Colour }, TestName = "0110")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00000111 }, new[] { Transparency, Matte, Negative, Colour }, TestName = "0111")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00001000 }, new[] { Reflective, Glossy, Positive, BlackAndWhite }, TestName = "1000")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00001001 }, new[] { Transparency, Glossy, Positive, BlackAndWhite }, TestName = "1001")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00001010 }, new[] { Reflective, Matte, Positive, BlackAndWhite }, TestName = "1010")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00001011 }, new[] { Transparency, Matte, Positive, BlackAndWhite }, TestName = "1011")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00001100 }, new[] { Reflective, Glossy, Negative, BlackAndWhite }, TestName = "1100")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00001101 }, new[] { Transparency, Glossy, Negative, BlackAndWhite }, TestName = "1101")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00001110 }, new[] { Reflective, Matte, Negative, BlackAndWhite }, TestName = "1110")]
    [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b00001111 }, new[] { Transparency, Matte, Negative, BlackAndWhite }, TestName = "1111")]
    public void DeviceAttributes(byte[] bytes, string[] expected)
    {
        using var stream = Stream(bytes);
        var actual = stream.ReadDeviceAttributes();
        Assert.That(actual, Is.EqualTo(expected));
    }

    private static MemoryStream Stream(byte[] bytes) => new(bytes);
}