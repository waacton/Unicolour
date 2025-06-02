using System;
using System.IO;
using NUnit.Framework;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests;

public class IccUnsupportedFileTests : IccUnsupportedTests
{
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotFound(Source source)
    {
        const string path = "🚫";
        Assert.Throws<FileNotFoundException>(() => { _ = new Profile(path); });
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not found");
        AssertNotFound(iccConfig, source);
    }
    
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotEnoughBytes(Source source)
    {
        const string path = "not_enough_bytes.icc";
        File.WriteAllBytes(path, CreateBytes(64));
        Assert.Throws<ArgumentException>(() => { _ = new Profile(path); });

        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not enough bytes");
        AssertNotEnoughBytes(iccConfig, source);
        
        File.Delete(path);
    }
    
    [TestCase(Source.FromChannels)]
    [TestCase(Source.FromRgb)]
    public void NotParseable(Source source)
    {
        const string path = "not_parseable.icc";
        File.WriteAllBytes(path, CreateBytes(512));
        Assert.Throws<ArgumentException>(() => { _ = new Profile(path); });
        
        var iccConfig = new IccConfiguration(path, Intent.Unspecified, "not parseable");
        AssertNotParseable(iccConfig, source);
        
        File.Delete(path);
    }
}