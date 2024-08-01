using System;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class InvalidIntentTests
{
    private const Intent BadIntent = (Intent)int.MaxValue;
    
    [Test]
    public void InvalidParameter()
    {
        var profile = IccFile.Fogra39.GetProfile();
        Assert.Throws<ArgumentOutOfRangeException>(() => profile.ToXyzStandardD50([0.5, 0.5, 0.5, 0.5], BadIntent));
        Assert.Throws<ArgumentOutOfRangeException>(() => profile.FromStandardXyzD50(new(0.5, 0.5, 0.5), BadIntent));
        Assert.Throws<ArgumentOutOfRangeException>(() => profile.ToXyzStandardD50([0.5, 0.5, 0.5, 0.5], Intent.Unspecified));
        Assert.Throws<ArgumentOutOfRangeException>(() => profile.FromStandardXyzD50(new(0.5, 0.5, 0.5), Intent.Unspecified));
    }
}