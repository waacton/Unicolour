namespace Wacton.Unicolour.Tests;

using System;
using System.Reflection;
using NUnit.Framework.Constraints;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class InvalidUnicolourTests
{
    private const ColourSpace GoodColourSpace = ColourSpace.Rgb;
    private const ColourSpace BadColourSpace = (ColourSpace)int.MaxValue;
    
    private readonly Unicolour unicolour = Unicolour.FromXyz(0.1, 0.2, 0.3);
    private readonly ColourRepresentation goodColourRepresentation = new Rgb(0.4, 0.5, 0.6, Configuration.Default);
    private ColourRepresentation badColourRepresentation = null!;

    [SetUp]
    public void Init()
    {
        var testRepresentation = new TestRepresentation(0.7, 0.8, 0.9, ColourMode.Unset);
        testRepresentation.OverrideColourSpace(BadColourSpace);
        badColourRepresentation = testRepresentation;
    }

    [Test]
    public void InvalidRequestedColourSpace()
    {
        const string methodName = "GetBackingRepresentation";
        AssertDoesNotThrow(() => InvokeMethod(methodName, GoodColourSpace));
        AssertThrows<ArgumentOutOfRangeException>(() => InvokeMethod(methodName, BadColourSpace));
        
        const string genericMethodName = "Get";
        AssertDoesNotThrow(() => InvokeGenericMethod(genericMethodName, typeof(ColourRepresentation), GoodColourSpace));
        AssertThrows<ArgumentOutOfRangeException>(() => InvokeGenericMethod(genericMethodName, typeof(ColourRepresentation), BadColourSpace));
    }
    
    [Test]
    public void InvalidInitialColourSpace()
    {
        const string methodName = "SetBackingRepresentation";
        AssertDoesNotThrow(() => InvokeMethod(methodName, GoodColourSpace));
        SetField("initialColourSpace", BadColourSpace);
        AssertThrows<ArgumentOutOfRangeException>(() => InvokeMethod(methodName, GoodColourSpace));
    }

    [Test]
    public void InvalidInitialRepresentation()
    {
        const string methodName = "SetInitialRepresentation";
        AssertDoesNotThrow(() => InvokeMethod(methodName, goodColourRepresentation));
        AssertThrows<ArgumentOutOfRangeException>(() => InvokeMethod(methodName, badColourRepresentation));
    }

    private static void AssertDoesNotThrow(Action action) => Assert.DoesNotThrow(action.Invoke);
    private static void AssertThrows<T>(Action action) => Assert.Throws(ExceptionConstraint<T>(), action.Invoke);
    private static ExactTypeConstraint ExceptionConstraint<T>() => Is.TypeOf<TargetInvocationException>().And.InnerException.TypeOf<T>();
    
    private void SetField(string name, object argument) => GetPrivateField(name).SetValue(unicolour, argument);
    private void InvokeMethod(string name, params object[] args) => GetPrivateMethod(name).Invoke(unicolour, args);
    private void InvokeGenericMethod(string name, Type genericType, params object[] args) => GetPrivateMethod(name, genericType).Invoke(unicolour, args);
    
    private static FieldInfo GetPrivateField(string name) => typeof(Unicolour).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static MethodInfo GetPrivateMethod(string name) => typeof(Unicolour).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static MethodInfo GetPrivateMethod(string name, Type genericType) => GetPrivateMethod(name).MakeGenericMethod(genericType);
}