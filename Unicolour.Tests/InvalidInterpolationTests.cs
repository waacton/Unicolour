namespace Wacton.Unicolour.Tests;

using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;

public class InvalidInterpolationTests
{
    private const ColourSpace GoodColourSpace = ColourSpace.Rgb;
    private const ColourSpace BadColourSpace = (ColourSpace)int.MaxValue;
    
    private readonly Unicolour unicolour1 = Unicolour.FromRgb(0.1, 0.2, 0.3);
    private readonly Unicolour unicolour2 = Unicolour.FromRgb(0.7, 0.8, 0.9);

    [Test]
    public void InvalidInterpolationInput()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Interpolation.Mix(BadColourSpace, unicolour1, unicolour2, 0.5, true));
    }

    [Test] 
    public void InvalidInterpolationOutput()
    {
        const string methodName = "GetConstructor";
        AssertDoesNotThrow(() => InvokeMethod(methodName, GoodColourSpace));
        AssertThrows<ArgumentOutOfRangeException>(() => InvokeMethod(methodName, BadColourSpace));
    }

    private static void AssertDoesNotThrow(Action action) => Assert.DoesNotThrow(action.Invoke);
    private static void AssertThrows<T>(Action action) => Assert.Throws(ExceptionConstraint<T>(), action.Invoke);
    private static ExactTypeConstraint ExceptionConstraint<T>() => Is.TypeOf<TargetInvocationException>().And.InnerException.TypeOf<T>();
    
    private static void InvokeMethod(string name, params object[] args) => GetPrivateMethod(name).Invoke(null, args);
    
    private static MethodInfo GetPrivateMethod(string name) => typeof(Interpolation).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)!;
}