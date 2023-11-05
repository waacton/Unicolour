namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ParseHexTests
{
    [TestCaseSource(typeof(NamedColours), nameof(NamedColours.All))]
    public void NamedHexSameRgbAsSystem(TestColour namedColour) => AssertHexParse(namedColour.Hex!);

    [TestCaseSource(typeof(RandomColours), nameof(RandomColours.HexStrings))]
    public void HexSameRgbAsSystem(string hex) => AssertHexParse(hex);

    [Test]
    public void ThrowsExceptionWhenInvalidLength() 
    {
        Assert.Throws<NullReferenceException>(() => ParseColourHex(null!));
        Assert.Throws<ArgumentException>(() => ParseColourHex(string.Empty));
        Assert.Throws<ArgumentException>(() => ParseColourHex(GetString(1)));
        Assert.Throws<ArgumentException>(() => ParseColourHex(GetString(2)));
        Assert.Throws<ArgumentException>(() => ParseColourHex(GetString(3)));
        Assert.Throws<ArgumentException>(() => ParseColourHex(GetString(4)));
        Assert.Throws<ArgumentException>(() => ParseColourHex(GetString(5)));
        Assert.That(ParseColourHex(GetString(6)), Is.EqualTo((170/255.0, 170/255.0, 170/255.0, 255/255.0)));
        Assert.Throws<ArgumentException>(() => ParseColourHex(GetString(7)));
        Assert.That(ParseColourHex(GetString(8)), Is.EqualTo((170/255.0, 170/255.0, 170/255.0, 170/255.0)));
        Assert.Throws<ArgumentException>(() => ParseColourHex(GetString(9)));
        Assert.Throws<ArgumentException>(() => ParseColourHex(GetString(10)));
    }

    [TestCaseSource(nameof(GeneratePotentialHexStrings))]
    public void ThrowsExceptionWhenInvalidCharacter(string potentialHex)
    {
        if (potentialHex.All(Uri.IsHexDigit))
        {
            Assert.DoesNotThrow(() => ParseColourHex(potentialHex));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => ParseColourHex(potentialHex));
        }
    }

    private static List<string> GeneratePotentialHexStrings()
    {
        var result = new List<string>();
        for (var charValue = 0; charValue < 128; charValue++)
        {
            for (var i = 0; i < 8; i++)
            {
                var character = (char) charValue;
                var chars = GetChars(8);
                chars[i] = character;
                result.Add(new string(chars));
            }
        }

        return result;
    }

    private static void AssertHexParse(string hex)
    {
        var systemColor = SystemColorUtils.HexToRgb255(hex);
        var parsed = ParseColourHex(hex);
        Assert.That(parsed.r * 255, Is.EqualTo(systemColor.r255), hex);
        Assert.That(parsed.g * 255, Is.EqualTo(systemColor.g255), hex);
        Assert.That(parsed.b * 255, Is.EqualTo(systemColor.b255), hex);
        Assert.That(parsed.a * 255, Is.EqualTo(systemColor.a255), hex);
    }
    
    private static (double r, double g, double b, double a) ParseColourHex(string hex) => Wacton.Unicolour.Utils.ParseColourHex(hex);

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