namespace Wacton.Unicolour.Tests;

using System.Globalization;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class UnseenTests
{
    [Test]
    public void EighthColour()
    {
        var hex = $"{Unicolour.UnseenName.ToUpper()}";
        var unicolour = new Unicolour(hex);
        AssertEighthColour(unicolour);
    }
    
    [Test]
    public void EighthColourWithAlphaOverride()
    {
        var hex = $"{new CultureInfo("en-GB", false).TextInfo.ToTitleCase(Unicolour.UnseenName)}";
        var unicolour = new Unicolour(hex, 1.0);
        AssertEighthColour(unicolour);
    }
    
    [Test]
    public void SeenAlphaUnaffected()
    {
        var unicolour = new Unicolour("#C0FFEE", 1.0);
        Assert.That(unicolour.Alpha.A, Is.EqualTo(1.0));
    }

    private static void AssertEighthColour(Unicolour unicolour)
    {
        Assert.That(unicolour.Hex, Is.EqualTo(Unicolour.UnseenName));
        Assert.That(unicolour.Description, Is.EqualTo(Unicolour.UnseenDescription));
        TestUtils.AssertTriplet<Rgb>(unicolour, new(double.NaN, double.NaN, double.NaN), 0);
        Assert.That(unicolour.Alpha.A, Is.EqualTo(0.0));
    }
}