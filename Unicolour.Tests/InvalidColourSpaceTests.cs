using System;
using System.Reflection;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class InvalidColourSpaceTests
{
    private const ColourSpace BadColourSpace = (ColourSpace)int.MaxValue;
    
    [Test]
    public void InvalidConstructor()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Unicolour(BadColourSpace, 0, 0, 0));
    }
    
    [Test]
    public void InvalidGreyConstructor()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Unicolour(BadColourSpace, 0));
    }
    
    [Test]
    public void InvalidUnicolourProperty()
    {
        var colour = new Unicolour(ColourSpace.Xyz, 0.1, 0.2, 0.3);
        ForceBadColourSpace(colour);
        
        // the exception here is a reflection-based one
        // but the inner exception holds the actual exception that triggered it
        Assert.Throws(
            Is.TypeOf<TargetInvocationException>().And.InnerException.TypeOf<ArgumentOutOfRangeException>(), 
            () => CallPrivateEvaluateXyz(colour));
    }
    
    [Test]
    public void InvalidInterpolationParameter()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, 0.1, 0.2, 0.3);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0.7, 0.8, 0.9);
        Assert.Throws<ArgumentOutOfRangeException>(() => Interpolation.Mix(colour1, colour2, BadColourSpace, 0.5, HueSpan.Shorter, true));
    }
    
    private static void ForceBadColourSpace(Unicolour unicolour)
    {
        // Unicolour.SourceColourSpace is an internal readonly field
        var field = typeof(Unicolour).GetField("SourceColourSpace", BindingFlags.NonPublic | BindingFlags.Instance)!;
        field.SetValue(unicolour, BadColourSpace);
    }

    private static void CallPrivateEvaluateXyz(Unicolour unicolour)
    {
        var method = typeof(Unicolour).GetMethod("EvaluateXyz", BindingFlags.NonPublic | BindingFlags.Instance)!;
        method.Invoke(unicolour, parameters: []);
    }
}