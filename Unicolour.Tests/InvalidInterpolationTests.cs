namespace Wacton.Unicolour.Tests;

using System;
using System.Reflection;
using NUnit.Framework.Constraints;
using NUnit.Framework;

public class InvalidInterpolationTests
{
    private const ColourSpace GoodColourSpace = ColourSpace.Rgb;
    private const ColourSpace BadColourSpace = (ColourSpace)int.MaxValue;
    
    private readonly Unicolour unicolour1 = Unicolour.FromRgb(0.1, 0.2, 0.3);
    private readonly Unicolour unicolour2 = Unicolour.FromRgb(0.7, 0.8, 0.9);

    [Test]
    public void InvalidInterpolationInput()
    {
        const string methodName = "Interpolate";
        var types = new[] { typeof(ColourSpace), typeof(Unicolour), typeof(Unicolour), typeof(double) };
        AssertDoesNotThrow(() => InvokeMethod(methodName, types, GoodColourSpace, unicolour1, unicolour2, 0.5));
        AssertThrows<InvalidOperationException>(() => InvokeMethod(methodName, types, BadColourSpace, unicolour1, unicolour2, 0.5));
    }

    [Test] 
    public void InvalidInterpolationOutput()
    {
        const string methodName = "UnicolourCreator";
        var types = new[] { typeof(ColourSpace) };
        AssertDoesNotThrow(() => InvokeMethod(methodName, types, GoodColourSpace));
        AssertThrows<ArgumentOutOfRangeException>(() => InvokeMethod(methodName, types, BadColourSpace));
    }

    private static void AssertDoesNotThrow(Action action) => Assert.DoesNotThrow(action.Invoke);
    private static void AssertThrows<T>(Action action) => Assert.Throws(ExceptionConstraint<T>(), action.Invoke);
    private static ExactTypeConstraint ExceptionConstraint<T>() => Is.TypeOf<TargetInvocationException>().And.InnerException.TypeOf<T>();
    
    private static void InvokeMethod(string name, Type[] types, params object[] args) => GetPrivateMethod(name, types).Invoke(null, args);

    // need to include types to disambiguate between the multiple private "Interpolate()" functions
    private static MethodInfo GetPrivateMethod(string name, Type[] types) => typeof(Interpolation).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static, types)!;
}