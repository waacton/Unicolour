namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class ImaginaryTests
{
    [Test]
    public void RgbGamut([Values(0, 255)] int r, [Values(0, 255)] int g, [Values(0, 255)] int b)
    {
        var unicolour = new Unicolour(ColourSpace.Rgb255, r, g, b);
        Assert.That(unicolour.IsImaginary, Is.False);
    }

    [Test]
    public void Greyscale([Range(0, 1, 0.1)] double value)
    {
        var unicolour = new Unicolour(ColourSpace.Rgb, value, value, value);
        Assert.That(unicolour.IsImaginary, Is.False);
    }
    
    private static readonly Dictionary<(Illuminant illuminant, Observer observer), Configuration> Configurations = new()
    {
        { (Illuminant.D65, Observer.Degree2), new(xyzConfiguration: new(Illuminant.D65, Observer.Degree2)) },
        { (Illuminant.D65, Observer.Degree10), new(xyzConfiguration: new(Illuminant.D65, Observer.Degree10)) },
        { (Illuminant.E, Observer.Degree2), new(xyzConfiguration: new(Illuminant.E, Observer.Degree2)) },
        { (Illuminant.E, Observer.Degree10), new(xyzConfiguration: new(Illuminant.E, Observer.Degree10)) }
    };
    
    [Test]
    public void Monochromatic(
        [Range(360, 700)] int wavelength,
        [Values(nameof(Observer.Degree2), nameof(Observer.Degree10))] string observerName,
        [Values(nameof(Illuminant.D65), nameof(Illuminant.E))] string illuminantName)
    {
        var illuminant = TestUtils.Illuminants[illuminantName];
        var observer = TestUtils.Observers[observerName];
        var config = Configurations[(illuminant, observer)];
        
        var unicolour = new Unicolour(config, new Spd { { wavelength, 1.0 } });
        Assert.That(unicolour.IsImaginary, Is.False);
    }

    private const double Offset = 0.0000001;
    [TestCase(Edge.Bottom, 0, Offset, false)] // coordinates directly above are inside the boundary
    [TestCase(Edge.Bottom, 0, -Offset, true)]
    [TestCase(Edge.Bottom, Offset, 0, true)] 
    [TestCase(Edge.Bottom, -Offset, 0, true)]
    [TestCase(Edge.Left, 0, Offset, true)] 
    [TestCase(Edge.Left, 0, -Offset, true)]
    [TestCase(Edge.Left, Offset, 0, false)] // coordinates directly to the right are inside the boundary
    [TestCase(Edge.Left, -Offset, 0, true)] 
    [TestCase(Edge.Right, 0, Offset, true)] 
    [TestCase(Edge.Right, 0, -Offset, true)] 
    [TestCase(Edge.Right, Offset, 0, true)]
    [TestCase(Edge.Right, -Offset, 0, false)] // coordinates directly to the left are inside the boundary
    [TestCase(Edge.Top, 0, Offset, true)] 
    [TestCase(Edge.Top, 0, -Offset, false)] // coordinates directly below are inside the boundary
    [TestCase(Edge.Top, Offset, 0, true)] 
    [TestCase(Edge.Top, -Offset, 0, true)] 
    public void BoundaryEdge(Edge edge, double xOffset, double yOffset, bool expectedImaginary)
    {
        // effectively the bounding box, based on 2 degree observer
        var wavelength = edge switch
        {
            Edge.Bottom => 404,
            Edge.Left => 504,
            Edge.Top => 521,
            Edge.Right => 699,
            _ => throw new ArgumentOutOfRangeException(nameof(edge), edge, null)
        };
        
        var monochromatic = new Unicolour(new Spd { { wavelength, 1.0 } });
        var chromaticity = monochromatic.Chromaticity;
        var offsetChromaticity = new Chromaticity(chromaticity.X + xOffset, chromaticity.Y + yOffset);
        var unicolour = new Unicolour(offsetChromaticity);
        Assert.That(unicolour.IsImaginary, Is.EqualTo(expectedImaginary));
    }

    public enum Edge { Bottom, Left, Top, Right }
}