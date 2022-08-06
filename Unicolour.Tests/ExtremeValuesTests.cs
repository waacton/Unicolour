namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public static class ExtremeValuesTests
{
    [Test, Combinatorial]
    public static void Rgb(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromRgb(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Hsb(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromHsb(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Hsl(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromHsl(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Xyz(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromXyz(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Lab(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromLab(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Lchab(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromLchab(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Luv(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromLuv(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Lchuv(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromLchuv(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Jzazbz(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromJzazbz(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Jzczhz(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromJzczhz(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Oklab(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromOklab(first, second, third));
    }
    
    [Test, Combinatorial]
    public static void Oklch(
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double first, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double second, 
        [Values(double.MinValue, double.MaxValue, double.Epsilon, double.NegativeInfinity, double.PositiveInfinity, double.NaN)] double third)
    {
        AssertUtils.AssertNoPropertyError(Unicolour.FromOklch(first, second, third));
    }
}