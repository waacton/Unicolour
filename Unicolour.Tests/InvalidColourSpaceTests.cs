namespace Wacton.Unicolour.Tests;

using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;

public class InvalidColourSpaceTests
{
    private const ColourSpace BadColourSpace = (ColourSpace)int.MaxValue;

    private Unicolour? unicolour;
    
    [SetUp]
    public void Init()
    {
        unicolour = new Unicolour(ColourSpace.Xyz, 0.1, 0.2, 0.3);
    }
    
    [Test]
    public void InvalidConstructor()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Unicolour(BadColourSpace, 0, 0, 0));
    }
    
    [Test]
    public void InvalidInterpolationInput()
    {
        var unicolour1 = new Unicolour(ColourSpace.Rgb, 0.1, 0.2, 0.3);
        var unicolour2 = new Unicolour(ColourSpace.Rgb, 0.7, 0.8, 0.9);
        Assert.Throws<ArgumentOutOfRangeException>(() => Interpolation.Mix(unicolour1, unicolour2, BadColourSpace, 0.5, true));
    }
    
    [Test]
    public void InvalidXyzEvaluation()
    {
        const string fieldName = "InitialColourSpace";
        SetPrivateField(fieldName, BadColourSpace);
        
        const string methodName = "EvaluateXyz";
        AssertThrows<ArgumentOutOfRangeException>(() => InvokePrivateMethod(methodName));
    }

    private static void AssertDoesNotThrow(Action action) => Assert.DoesNotThrow(action.Invoke);
    private static void AssertThrows<T>(Action action) => Assert.Throws(ExceptionConstraint<T>(), action.Invoke);
    private static ExactTypeConstraint ExceptionConstraint<T>() => Is.TypeOf<TargetInvocationException>().And.InnerException.TypeOf<T>();

    private void InvokePrivateMethod(string name, params object[] args) => GetPrivateMethod(name).Invoke(unicolour, args);
    private void InvokePrivateGenericMethod(string name, Type genericType, params object[] args) => GetPrivateMethod(name, genericType).Invoke(unicolour, args);
    private void SetPrivateField(string name, object value) => GetPrivateField(name).SetValue(unicolour, value);

    private static MethodInfo GetPrivateMethod(string name) => typeof(Unicolour).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static MethodInfo GetPrivateMethod(string name, Type genericType) => GetPrivateMethod(name).MakeGenericMethod(genericType);
    private static FieldInfo GetPrivateField(string name) => typeof(Unicolour).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
}