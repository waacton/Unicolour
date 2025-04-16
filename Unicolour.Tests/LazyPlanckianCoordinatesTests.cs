using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class LazyPlanckianCoordinatesTests
{
    [Test]
    public void DirectAccess()
    {
        var planckian = new Planckian(Observer.Degree2);
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.False);

        _ = planckian.StandardRangeCoordinates.Value;
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.True);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.False);
        
        _ = planckian.BelowRangeCoordinates.Value;
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.True);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.True);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.False);
        
        _ = planckian.AboveRangeCoordinates.Value;
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.True);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.True);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.True);
    }
    
    [Test]
    public void IndirectAccessStandardRange()
    {
        // new instance of XyzConfiguration will create a new instance of Planckian, so other tests do not affect this one
        var xyzConfig = new XyzConfiguration(Illuminant.D65, Observer.Degree2);
        var config = new Configuration(xyzConfig: xyzConfig);
        
        var white = new Unicolour(config, ColourSpace.Rgb, 1, 1, 1);
        var planckian = white.Configuration.Xyz.Planckian;
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.False);

        _ = white.Temperature;
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.True);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.False);
        
        var black = new Unicolour(config, ColourSpace.Rgb, 0, 0, 0);
        Assert.That(black.Configuration.Xyz.Planckian, Is.EqualTo(planckian));
    }
    
    [Test]
    public void IndirectAccessBelowRange()
    {
        // new instance of XyzConfiguration will create a new instance of Planckian, so other tests do not affect this one
        var xyzConfig = new XyzConfiguration(Illuminant.D65, Observer.Degree2);
        var config = new Configuration(xyzConfig: xyzConfig);
        
        var red = new Unicolour(config, ColourSpace.Rgb, 1, 0, 0);
        var planckian = red.Configuration.Xyz.Planckian;
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.False);

        _ = red.Temperature;
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.True);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.True);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.False);
        
        var black = new Unicolour(config, ColourSpace.Rgb, 0, 0, 0);
        Assert.That(black.Configuration.Xyz.Planckian, Is.EqualTo(planckian));
    }
    
    [Test]
    public void IndirectAccessAboveRange()
    {
        // new instance of XyzConfiguration will create a new instance of Planckian, so other tests do not affect this one
        var xyzConfig = new XyzConfiguration(Illuminant.D65, Observer.Degree2);
        var config = new Configuration(xyzConfig: xyzConfig);
        
        var blue = new Unicolour(config, ColourSpace.Rgb, 0, 0, 1);
        var planckian = blue.Configuration.Xyz.Planckian;
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.False);

        _ = blue.Temperature;
        Assert.That(planckian.StandardRangeCoordinates.IsValueCreated, Is.True);
        Assert.That(planckian.BelowRangeCoordinates.IsValueCreated, Is.False);
        Assert.That(planckian.AboveRangeCoordinates.IsValueCreated, Is.True);
        
        var black = new Unicolour(config, ColourSpace.Rgb, 0, 0, 0);
        Assert.That(black.Configuration.Xyz.Planckian, Is.EqualTo(planckian));
    }
}

