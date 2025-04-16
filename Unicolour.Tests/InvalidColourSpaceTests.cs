using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Wacton.Unicolour.Tests;

public class InvalidColourSpaceTests
{
    private const ColourSpace BadColourSpace = (ColourSpace)int.MaxValue;

    private Unicolour? colour;
    
    [SetUp]
    public void Init()
    {
        colour = new Unicolour(ColourSpace.Xyz, 0.1, 0.2, 0.3);
    }
    
    [Test]
    public void InvalidConstructor()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Unicolour(BadColourSpace, 0, 0, 0));
    }
    
    [Test]
    public void InvalidUnicolourProperty()
    {
        const string fieldName = "SourceColourSpace";
        SetPrivateField(fieldName, BadColourSpace);
        
        const string methodName = "EvaluateXyz";
        AssertThrows<ArgumentOutOfRangeException>(() => InvokePrivateMethod(methodName));
    }
    
    [Test]
    public void InvalidInterpolationParameter()
    {
        var colour1 = new Unicolour(ColourSpace.Rgb, 0.1, 0.2, 0.3);
        var colour2 = new Unicolour(ColourSpace.Rgb, 0.7, 0.8, 0.9);
        Assert.Throws<ArgumentOutOfRangeException>(() => Interpolation.Mix(colour1, colour2, BadColourSpace, 0.5, HueSpan.Shorter, true));
    }

    private static void AssertDoesNotThrow(Action action) => Assert.DoesNotThrow(action.Invoke);
    private static void AssertThrows<T>(Action action) => Assert.Throws(ExceptionConstraint<T>(), action.Invoke);
    private static ExactTypeConstraint ExceptionConstraint<T>() => Is.TypeOf<TargetInvocationException>().And.InnerException.TypeOf<T>();

    private void InvokePrivateMethod(string name, params object[] args) => GetPrivateMethod(name).Invoke(colour, args);
    private void InvokePrivateGenericMethod(string name, Type genericType, params object[] args) => GetPrivateMethod(name, genericType).Invoke(colour, args);
    private void SetPrivateField(string name, object value) => GetPrivateField(name).SetValue(colour, value);

    private static MethodInfo GetPrivateMethod(string name) => typeof(Unicolour).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static MethodInfo GetPrivateMethod(string name, Type genericType) => GetPrivateMethod(name).MakeGenericMethod(genericType);
    private static FieldInfo GetPrivateField(string name) => typeof(Unicolour).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
}