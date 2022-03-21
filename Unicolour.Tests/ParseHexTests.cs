namespace Wacton.Unicolour.Tests;

using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ParseHexTests
{
    [Test]
    public void ParsedHexSameRgbAsSystem()
    {
        AssertUtils.AssertNamedColours(namedColour => AssertHexParse(namedColour.Hex!));
        AssertUtils.AssertRandomHexColours(AssertHexParse);
    }
    
    [Test]
    public void ParseThrowsExceptionWhenInvalidLength()
    {
        Assert.Catch<NullReferenceException>(() => ParseColourHex(null!));
        Assert.Catch<ArgumentException>(() => ParseColourHex(string.Empty));
        Assert.Catch<ArgumentException>(() => ParseColourHex(GetString(1)));
        Assert.Catch<ArgumentException>(() => ParseColourHex(GetString(2)));
        Assert.Catch<ArgumentException>(() => ParseColourHex(GetString(3)));
        Assert.Catch<ArgumentException>(() => ParseColourHex(GetString(4)));
        Assert.Catch<ArgumentException>(() => ParseColourHex(GetString(5)));
        Assert.That(ParseColourHex(GetString(6)), Is.EqualTo((170, 170, 170, 255)));
        Assert.Catch<ArgumentException>(() => ParseColourHex(GetString(7)));
        Assert.That(ParseColourHex(GetString(8)), Is.EqualTo((170, 170, 170, 170)));
        Assert.Catch<ArgumentException>(() => ParseColourHex(GetString(9)));
        Assert.Catch<ArgumentException>(() => ParseColourHex(GetString(10)));
    }
    
    [Test]
    public void ParseThrowsExceptionWhenInvalidCharacter()
    {
        for (var charValue = 0; charValue < 128; charValue++)
        {
            for (var i = 0; i < 8; i++)
            {
                var character = (char) charValue;
                var chars = GetChars(8);
                chars[i] = character;
                var hex = new string(chars);

                if (Uri.IsHexDigit(character))
                {
                    Assert.DoesNotThrow(() => ParseColourHex(hex));
                }
                else
                {
                    Assert.Catch<ArgumentException>(() => ParseColourHex(hex));
                }
            }
        }
    }

    private static void AssertHexParse(string hex)
    {
        var systemColor = SystemColorUtils.HexToRgb255(hex);
        var parsed = ParseColourHex(hex);
        Assert.That(parsed.r255, Is.EqualTo(systemColor.r255), hex);
        Assert.That(parsed.g255, Is.EqualTo(systemColor.g255), hex);
        Assert.That(parsed.b255, Is.EqualTo(systemColor.b255), hex);
        Assert.That(parsed.a255, Is.EqualTo(systemColor.a255), hex);
    }
    
    private static (int r255, int g255, int b255, int a255) ParseColourHex(string hex) => Wacton.Unicolour.Utils.ParseColourHex(hex);

    private static string GetString(int length) => new(GetChars(length));

    private static char[] GetChars(int length)
    {
        var chars = new char[length];
        for (var i = 0; i < length; i++)
        {
            chars[i] = 'a';
        }

        return chars;
    }
    
}