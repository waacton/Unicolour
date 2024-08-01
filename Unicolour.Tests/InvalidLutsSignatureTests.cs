using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class InvalidLutsSignatureTests
{
    private const string BadSignature = "nope";
    
    [Test]
    public void InvalidParameter()
    {
        var profile = IccFile.Fogra39.GetProfile();
        Assert.Throws<ArgumentOutOfRangeException>(() => profile.Tags.GetLuts(BadSignature));
    }
}