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
    public void InvalidColourSpaceGet()
    {
        const string genericMethodName = "Get";
        AssertDoesNotThrow(() => InvokePrivateGenericMethod(genericMethodName, typeof(ColourRepresentation), GoodColourSpace));
        AssertThrows<ArgumentOutOfRangeException>(() => InvokePrivateGenericMethod(genericMethodName, typeof(ColourRepresentation), BadColourSpace));
    }
    
    [Test]
    public void InvalidColourSpaceSet()
    {
        const string methodName = "SetBackingField";
        AssertDoesNotThrow(() => InvokePrivateMethod(methodName, GoodColourSpace));
        AssertThrows<ArgumentOutOfRangeException>(() => InvokePrivateMethod(methodName, BadColourSpace));
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