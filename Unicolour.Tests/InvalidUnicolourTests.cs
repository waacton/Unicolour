namespace Wacton.Unicolour.Tests;

using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;

public class InvalidUnicolourTests
{
    private const ColourSpace GoodColourSpace = ColourSpace.Rgb;
    private const ColourSpace BadColourSpace = (ColourSpace)int.MaxValue;

    private Unicolour? unicolour;
    
    [SetUp]
    public void Init()
    {
        unicolour = Unicolour.FromXyz(0.1, 0.2, 0.3);
    }

    [Test]
    public void InvalidRequestedRepresentation()
    {
        const string genericMethodName = "Get";
        AssertDoesNotThrow(() => InvokeGenericMethod(genericMethodName, typeof(ColourRepresentation), GoodColourSpace));
        AssertThrows<ArgumentOutOfRangeException>(() => InvokeGenericMethod(genericMethodName, typeof(ColourRepresentation), BadColourSpace));
    }
    
    [Test]
    public void InvalidInitialRepresentation()
    {
        const string methodName = "SetBackingField";
        AssertDoesNotThrow(() => InvokeMethod(methodName, GoodColourSpace));
        AssertThrows<ArgumentOutOfRangeException>(() => InvokeMethod(methodName, BadColourSpace));
    }

    private static void AssertDoesNotThrow(Action action) => Assert.DoesNotThrow(action.Invoke);
    private static void AssertThrows<T>(Action action) => Assert.Throws(ExceptionConstraint<T>(), action.Invoke);
    private static ExactTypeConstraint ExceptionConstraint<T>() => Is.TypeOf<TargetInvocationException>().And.InnerException.TypeOf<T>();

    private void InvokeMethod(string name, params object[] args) => GetPrivateMethod(name).Invoke(unicolour, args);
    private void InvokeGenericMethod(string name, Type genericType, params object[] args) => GetPrivateMethod(name, genericType).Invoke(unicolour, args);

    private static MethodInfo GetPrivateMethod(string name) => typeof(Unicolour).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static MethodInfo GetPrivateMethod(string name, Type genericType) => GetPrivateMethod(name).MakeGenericMethod(genericType);
}