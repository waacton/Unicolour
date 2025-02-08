using System.Globalization;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class UnseenTests
{
    [Test]
    public void EighthColour()
    {
        var hex = $"{Unicolour.UnseenName.ToUpper()}";
        var colour = new Unicolour(hex);
        AssertEighthColour(colour);
    }
    
    [Test]
    public void EighthColourWithAlphaOverride()
    {
        var hex = $"{new CultureInfo("en-GB", false).TextInfo.ToTitleCase(Unicolour.UnseenName)}";
        var colour = new Unicolour(hex, 1.0);
        AssertEighthColour(colour);
    }
    
    [Test]
    public void SeenAlphaUnaffected()
    {
        var colour = new Unicolour("#C0FFEE", 1.0);
        Assert.That(colour.Alpha.A, Is.EqualTo(1.0));
    }

    private static void AssertEighthColour(Unicolour colour)
    {
        Assert.That(colour.Hex, Is.EqualTo(Unicolour.UnseenName));
        Assert.That(colour.Description, Is.EqualTo(Unicolour.UnseenDescription));
        TestUtils.AssertTriplet<Rgb>(colour, new(double.NaN, double.NaN, double.NaN), 0);
        Assert.That(colour.Alpha.A, Is.EqualTo(0.0));
    }
}