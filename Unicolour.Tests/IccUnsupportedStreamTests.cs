using System;
using NUnit.Framework;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests;

public class IccUnsupportedStreamTests : IccUnsupportedTests
{
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotEnoughBytes(Source source)
    {
        var bytes = CreateBytes(64);
        Assert.Throws<ArgumentException>(() => { _ = new Profile(bytes); });

        var iccConfig = new IccConfiguration(bytes, Intent.Unspecified, "not enough bytes");
        AssertNotEnoughBytes(iccConfig, source);
    }
    
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotParseable(Source source)
    {
        var bytes = CreateBytes(512);
        Assert.Throws<ArgumentException>(() => { _ = new Profile(bytes); });
        
        var iccConfig = new IccConfiguration(bytes, Intent.Unspecified, "not parseable");
        AssertNotParseable(iccConfig, source);
    }
}