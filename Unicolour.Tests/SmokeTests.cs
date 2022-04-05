namespace Wacton.Unicolour.Tests;

using NUnit.Framework;

public static class SmokeTests
{
    [Test]
    public static void UnicolourHex()
    {
        AssertHex("000000");
        AssertHex("FFFFFF");
        AssertHex("667788");
    }

    [Test]
    public static void UnicolourRgb255()
    {
        AssertRgb255((0, 0, 0));
        AssertRgb255((255, 255, 255));
        AssertRgb255((124, 125, 126));
    }
    
    [Test]
    public static void UnicolourRgb()
    {
        AssertRgb((0, 0, 0));
        AssertRgb((1, 1, 1));
        AssertRgb((0.4, 0.5, 0.6));
    }

    [Test]
    public static void UnicolourHsb()
    {
        AssertHsb((0, 0, 0));
        AssertHsb((360, 1, 1));
        AssertHsb((180, 0.4, 0.6));
    }
    
    [Test]
    public static void UnicolourHsl()
    {
        AssertHsl((0, 0, 0));
        AssertHsl((360, 1, 1));
        AssertHsl((180, 0.4, 0.6));
    }
    
    [Test]
    public static void UnicolourXyz()
    {
        AssertXyz((0, 0, 0));
        AssertXyz((1, 1, 1));
        AssertXyz((0.4, 0.5, 0.6));
    }
    
    [Test]
    public static void UnicolourLab()
    {
        AssertLab((0, -128, -128));
        AssertLab((100, 128, 128));
        AssertLab((50, -1, 1));
    }
    
    [Test]
    public static void UnicolourOklab()
    {
        AssertOklab((0, 0, 0));
        AssertOklab((1, 1, 1));
        AssertOklab((0.4, 0.5, 0.6));
    }

    private static void AssertRgb((double, double, double) tuple) => AssertInit(tuple, Unicolour.FromRgb, Unicolour.FromRgb, Unicolour.FromRgb, Unicolour.FromRgb);
    private static void AssertHsb((double, double, double) tuple) => AssertInit(tuple, Unicolour.FromHsb, Unicolour.FromHsb, Unicolour.FromHsb, Unicolour.FromHsb);
    private static void AssertHsl((double, double, double) tuple) => AssertInit(tuple, Unicolour.FromHsl, Unicolour.FromHsl, Unicolour.FromHsl, Unicolour.FromHsl);
    private static void AssertXyz((double, double, double) tuple) => AssertInit(tuple, Unicolour.FromXyz, Unicolour.FromXyz, Unicolour.FromXyz, Unicolour.FromXyz);
    private static void AssertLab((double, double, double) tuple) => AssertInit(tuple, Unicolour.FromLab, Unicolour.FromLab, Unicolour.FromLab, Unicolour.FromLab);
    private static void AssertOklab((double, double, double) tuple) => AssertInit(tuple, Unicolour.FromOklab, Unicolour.FromOklab, Unicolour.FromOklab, Unicolour.FromOklab);

    private delegate Unicolour FromValues(double first, double second, double third, double alpha = 1.0);
    private delegate Unicolour FromValuesWithConfig(Configuration config, double first, double second, double third, double alpha = 1.0);
    private delegate Unicolour FromTuple((double first, double second, double third) tuple, double alpha = 1.0);
    private delegate Unicolour FromTupleWithConfig(Configuration config, (double first, double second, double third) tuple, double alpha = 1.0);
    
    private static void AssertInit((double first, double second, double third) tuple, 
        FromValues fromValues, FromValuesWithConfig fromValuesWithConfig,
        FromTuple fromTuple, FromTupleWithConfig fromTupleWithConfig)
    {
        var (first, second, third) = tuple;
        
        var expected = fromValues(first, second, third);
        AssertNoError(expected, fromValues(first, second, third));
        AssertNoError(expected, fromValues(first, second, third, 1.0));
        AssertNoError(expected, fromValuesWithConfig(Configuration.Default, first, second, third));
        AssertNoError(expected, fromValuesWithConfig(Configuration.Default, first, second, third, 1.0));
        AssertNoError(expected, fromTuple(tuple));
        AssertNoError(expected, fromTuple(tuple, 1.0));
        AssertNoError(expected, fromTupleWithConfig(Configuration.Default, tuple));
        AssertNoError(expected, fromTupleWithConfig(Configuration.Default, tuple, 1.0));
    }
    
    // dedicated method due to using ints instead of doubles
    private static void AssertRgb255((int first, int second, int third) tuple)
    {
        var (first, second, third) = tuple;
        
        var expected = Unicolour.FromRgb255(first, second, third);
        AssertNoError(expected, Unicolour.FromRgb255(first, second, third));
        AssertNoError(expected, Unicolour.FromRgb255(first, second, third, 255));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, first, second, third));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, first, second, third, 255));
        AssertNoError(expected, Unicolour.FromRgb255(tuple));
        AssertNoError(expected, Unicolour.FromRgb255(tuple, 255));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, tuple));
        AssertNoError(expected, Unicolour.FromRgb255(Configuration.Default, tuple, 255));
    }
    
    private static void AssertHex(string hex)
    {
        var withHash = $"#{hex}";
        var withHashAndAlpha = $"#{hex}FF";
        var withoutHash = $"{hex}";
        var withoutHashAndAlpha = $"{hex}FF";
        
        var expected = Unicolour.FromHex(withHash);
        AssertNoError(expected, Unicolour.FromHex(withHash));
        AssertNoError(expected, Unicolour.FromHex(withHashAndAlpha));
        AssertNoError(expected, Unicolour.FromHex(withoutHash));
        AssertNoError(expected, Unicolour.FromHex(withoutHashAndAlpha));
    }

    private static void AssertNoError(Unicolour expected, Unicolour unicolour)
    {
        AssertNoError(unicolour);
        Assert.That(unicolour, Is.EqualTo(expected));
    }
    
    private static void AssertNoError(Unicolour unicolour)
    {
        Assert.DoesNotThrow(() => _ = unicolour.Rgb);
        Assert.DoesNotThrow(() => _ = unicolour.Hsb);
        Assert.DoesNotThrow(() => _ = unicolour.Hsl);
        Assert.DoesNotThrow(() => _ = unicolour.Xyz);
        Assert.DoesNotThrow(() => _ = unicolour.Lab);
        Assert.DoesNotThrow(() => _ = unicolour.Alpha);
        Assert.DoesNotThrow(() => _ = unicolour.Config);
        Assert.DoesNotThrow(() => _ = unicolour.Luminance);
    }
}

