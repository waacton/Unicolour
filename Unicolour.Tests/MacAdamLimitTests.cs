using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class MacAdamLimitsTests
{
    // 0.025, 0.05, ..., 0.95, 0.975 (0 and 1 are special cases)
    private static readonly List<double> Luminances = Enumerable.Range(1, 39).Select(x => x / 40.0).ToList();

    private const double Offset = 0.00000000001;
    private static Configuration[] DifferentConfigs = [TestUtils.CConfig, TestUtils.D50Config, TestUtils.EqualEnergyConfig];

    [Test]
    public void BoundaryInside([ValueSource(nameof(Luminances))] double y)
    {
        var limits = MacAdamLimits.Get(y);
        var lowerLeft = limits.First();

        var offsetUpRight = new Chromaticity(lowerLeft.X + Offset, lowerLeft.Y + Offset);
        var colour = new Unicolour(offsetUpRight, y);
        Assert.That(colour.IsInMacAdamLimits, Is.True);
    }
    
    [Test]
    public void BoundaryOutside([ValueSource(nameof(Luminances))] double y)
    {
        var limits = MacAdamLimits.Get(y);
        var lowerLeft = limits.First();

        var offsetLeft = new Chromaticity(lowerLeft.X - Offset, lowerLeft.Y);
        var colour = new Unicolour(offsetLeft, y);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
        
        var offsetDown = new Chromaticity(lowerLeft.X, lowerLeft.Y - Offset);
        colour = new Unicolour(offsetDown, y);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
    }
    
    [Test]
    public void BoundaryInsideDifferentConfig(
        [ValueSource(nameof(Luminances))] double y,
        [ValueSource(nameof(DifferentConfigs))] Configuration config)
    {
        var limits = MacAdamLimits.Get(y);
        var lowerLeft = limits.First();

        var offsetUpRight = new Chromaticity(lowerLeft.X + Offset, lowerLeft.Y + Offset);
        var colour = new Unicolour(offsetUpRight, y).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.True);
    }
    
    [Test]
    public void BoundaryOutsideDifferentConfig(
        [ValueSource(nameof(Luminances))] double y,
        [ValueSource(nameof(DifferentConfigs))] Configuration config)
    {
        var limits = MacAdamLimits.Get(y);
        var lowerLeft = limits.First();

        var offsetLeft = new Chromaticity(lowerLeft.X - Offset, lowerLeft.Y);
        var colour = new Unicolour(offsetLeft, y).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
        
        var offsetDown = new Chromaticity(lowerLeft.X, lowerLeft.Y - Offset);
        colour = new Unicolour(offsetDown, y).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
    }

    [Test]
    public void LuminanceZeroInside()
    {
        var limits = MacAdamLimits.Get(0);
        var lowerLeft = limits.First();
        
        var offsetUpRight = new Chromaticity(lowerLeft.X + Offset, lowerLeft.Y + Offset);
        var colour = new Unicolour(offsetUpRight, 0);
        Assert.That(colour.IsInMacAdamLimits, Is.True);
    }
    
    [Test]
    public void LuminanceZeroOutside()
    {
        var limits = MacAdamLimits.Get(0);
        var lowerLeft = limits.First();
        
        var offsetLeft = new Chromaticity(lowerLeft.X - Offset, lowerLeft.Y);
        var colour = new Unicolour(offsetLeft, 0);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
        
        var offsetDown = new Chromaticity(lowerLeft.X, lowerLeft.Y - Offset);
        colour = new Unicolour(offsetDown, 0);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
    }
    
    [Test]
    public void LuminanceZeroDifferentConfig([ValueSource(nameof(DifferentConfigs))] Configuration config)
    {
        var limits = MacAdamLimits.Get(0);
        var lowerLeft = limits.First();
        
        // adapting a colour of no luminance results in a white point chromaticity
        // so regardless of the original chromaticity, the adapted colour will always be within limits
        
        var offsetLeft = new Chromaticity(lowerLeft.X - Offset, lowerLeft.Y);
        var colour = new Unicolour(offsetLeft, 0).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.True);
        
        var offsetDown = new Chromaticity(lowerLeft.X, lowerLeft.Y - Offset);
        colour = new Unicolour(offsetDown, 0).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.True);

        var offsetRight = new Chromaticity(lowerLeft.X + Offset, lowerLeft.Y);
        colour = new Unicolour(offsetRight, 0).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.True);
        
        var offsetUp = new Chromaticity(lowerLeft.X, lowerLeft.Y + Offset);
        colour = new Unicolour(offsetUp, 0).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.True);
    }
    
    [Test]
    public void LuminanceMaxInside()
    {
        // limits at 100% luminance is the white point singularity
        // only the exact white point is inside limits
        var colour = new Unicolour(TestUtils.D65Config.Xyz.WhiteChromaticity, 1);
        Assert.That(colour.IsInMacAdamLimits, Is.True);
    }
    
    [Test]
    public void LuminanceMaxOutside()
    {
        var limits = MacAdamLimits.Get(1);
        var lowerLeft = limits.First();
        
        // limits at 100% luminance is the white point singularity
        // any chromaticity that is not the white point is not inside limits
        
        var offsetLeft = new Chromaticity(lowerLeft.X - Offset, lowerLeft.Y);
        var colour = new Unicolour(offsetLeft, 1);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
        
        var offsetDown = new Chromaticity(lowerLeft.X, lowerLeft.Y - Offset);
        colour = new Unicolour(offsetDown, 1);
        Assert.That(colour.IsInMacAdamLimits, Is.False);

        var offsetRight = new Chromaticity(lowerLeft.X + Offset, lowerLeft.Y);
        colour = new Unicolour(offsetRight, 1);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
        
        var offsetUp = new Chromaticity(lowerLeft.X, lowerLeft.Y + Offset);
        colour = new Unicolour(offsetUp, 1);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
    }
    
    [Test]
    public void LuminanceMaxInsideDifferentConfig([ValueSource(nameof(DifferentConfigs))] Configuration config)
    {
        // limits at 100% luminance is the white point singularity
        // only the exact white point is inside limits
        var colour = new Unicolour(TestUtils.D65Config.Xyz.WhiteChromaticity, 1).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.True);
    }
    
    [Test]
    public void LuminanceMaxOutsideDifferentConfig([ValueSource(nameof(DifferentConfigs))] Configuration config)
    {
        var limits = MacAdamLimits.Get(1);
        var lowerLeft = limits.First();
        
        // adapting a colour of 100% luminance results in a white point chromaticity
        // so regardless of the original chromaticity, the adapted colour will always be within limits
        
        var offsetLeft = new Chromaticity(lowerLeft.X - Offset, lowerLeft.Y);
        var colour = new Unicolour(offsetLeft, 1).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
        
        var offsetDown = new Chromaticity(lowerLeft.X, lowerLeft.Y - Offset);
        colour = new Unicolour(offsetDown, 1).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.False);

        var offsetRight = new Chromaticity(lowerLeft.X + Offset, lowerLeft.Y);
        colour = new Unicolour(offsetRight, 1).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
        
        var offsetUp = new Chromaticity(lowerLeft.X, lowerLeft.Y + Offset);
        colour = new Unicolour(offsetUp, 1).ConvertToConfiguration(config);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
    }
    
    [Test]
    public void NotNumberChromaticity()
    {
        var colour = new Unicolour(new Chromaticity(double.NaN, double.NaN), 0.5);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
    }

    [Test]
    public void NotNumberLuminance()
    {
        var colour = new Unicolour(new Chromaticity(0.3, 0.3), double.NaN);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
    }

    [Test]
    public void MapInsideLimits([ValueSource(nameof(Luminances))] double y)
    {
        var limits = MacAdamLimits.Get(y);
        var lowerLeft = limits.First();

        var offsetUpRight = new Chromaticity(lowerLeft.X + Offset, lowerLeft.Y + Offset);
        var inside = new Unicolour(offsetUpRight, y);
        var mapped = inside.MapToMacAdamLimits();
        Assert.That(mapped.IsInMacAdamLimits, Is.True);
        Assert.That(Equals(mapped, inside), Is.True);
        Assert.That(ReferenceEquals(mapped, inside), Is.False);
    }
    
    [Test]
    public void MapOutsideLimits([ValueSource(nameof(Luminances))] double y)
    {
        var limits = MacAdamLimits.Get(y);
        var lowerLeft = limits.First();

        var offsetDownLeft = new Chromaticity(lowerLeft.X - Offset, lowerLeft.Y - Offset);
        var outside = new Unicolour(offsetDownLeft, y);
        var mapped = outside.MapToMacAdamLimits();
        Assert.That(outside.IsInMacAdamLimits, Is.False);
        Assert.That(mapped.IsInMacAdamLimits, Is.True);
    }

    [TestCase]
    public void MapBeyondMaxLuminance()
    {
        var colour = new Unicolour(ColourSpace.Xyy, 0.8, 0.8, 1.5);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
        
        var mapped = colour.MapToMacAdamLimits();
        Assert.That(mapped.IsInMacAdamLimits, Is.True);
        Assert.That(mapped.Chromaticity, Is.EqualTo(MacAdamLimits.Config.Xyz.WhiteChromaticity));
    }
    
    [TestCase]
    public void MapBeyondMinLuminance()
    {
        var colour = new Unicolour(ColourSpace.Xyy, 0.8, 0.8, -1.5);
        Assert.That(colour.IsInMacAdamLimits, Is.False);
        
        var mapped = colour.MapToMacAdamLimits();
        Assert.That(mapped.IsInMacAdamLimits, Is.True);
    }
    
    private static readonly int[] PositiveWavelengths = Enumerable.Range(360, 341).ToArray();
    private static readonly int[] NegativeWavelengths = Enumerable.Range(-566, 73).ToArray();
    private static readonly int[] Wavelengths = PositiveWavelengths.Concat(NegativeWavelengths).ToArray();

    [Test]
    public void MapFromSpectralLocus([ValueSource(nameof(Wavelengths))] int wavelength)
    {
        var colour = new Unicolour(ColourSpace.Wxy, wavelength, 1.0, 0.5);
        Assert.That(colour.IsInMacAdamLimits, Is.False);

        var mapped = colour.MapToMacAdamLimits();
        Assert.That(mapped.IsInMacAdamLimits, Is.True);
        
        Assert.That(mapped.DominantWavelength, Is.EqualTo(colour.DominantWavelength).Within(0.00000001));
        Assert.That(mapped.ExcitationPurity, Is.LessThan(colour.ExcitationPurity));
        Assert.That(mapped.RelativeLuminance, Is.EqualTo(colour.RelativeLuminance));
        
        Assert.That(mapped.Wxy.W, Is.EqualTo(colour.Wxy.W).Within(0.00000001));
        Assert.That(mapped.Wxy.X, Is.LessThan(colour.Wxy.X));
        Assert.That(mapped.Wxy.Y, Is.EqualTo(colour.Wxy.Y));
    }
    
    [Test, Combinatorial]
    public void ExtremeValues([ValueSource(typeof(TestUtils), nameof(TestUtils.ExtremeDoubles))] double value)
    {
        // if extreme values are being used for the colour space in which mapping takes place
        // mapping should still return an in-gamut colour, with the exception of NaNs
        var original = new Unicolour(MacAdamLimits.Config, ColourSpace.Xyy, value, value, value);
        var gamutMapped = original.MapToMacAdamLimits();
        Assert.That(gamutMapped.IsInMacAdamLimits, gamutMapped.Xyy.UseAsNaN ? Is.False : Is.True);
    }
}