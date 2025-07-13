using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class LineTests
{
    [Test]
    public void None()
    {
        Chromaticity point1 = new(1, 1);
        Chromaticity point2 = new(1, 1);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.NaN);
        Assert.That(line.Intercept, Is.NaN);
    }
    
    [Test]
    public void Vertical()
    {
        Chromaticity point1 = new(1, 2);
        Chromaticity point2 = new(1, 3);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(double.PositiveInfinity));
        Assert.That(line.Intercept, Is.EqualTo(point1.X));
    }
    
    [Test]
    public void Horizontal()
    {
        Chromaticity point1 = new(2, 1);
        Chromaticity point2 = new(3, 1);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(0));
        Assert.That(line.Intercept, Is.EqualTo(point1.Y));
    }
    
    [Test]
    public void DiagonalPositive1()
    {
        Chromaticity point1 = new(1, 1);
        Chromaticity point2 = new(2, 2);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(1));
        Assert.That(line.Intercept, Is.EqualTo(0));
    }
    
    [Test]
    public void DiagonalPositive2()
    {
        Chromaticity point1 = new(1, 2);
        Chromaticity point2 = new(2, 4);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(2));
        Assert.That(line.Intercept, Is.EqualTo(0));
    }
    
    [Test]
    public void DiagonalNegative1()
    {
        Chromaticity point1 = new(1, -1);
        Chromaticity point2 = new(2, -2);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(-1));
        Assert.That(line.Intercept, Is.EqualTo(0));
    }
    
    [Test]
    public void DiagonalNegative2()
    {
        Chromaticity point1 = new(1, -2);
        Chromaticity point2 = new(2, -4);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(-2));
        Assert.That(line.Intercept, Is.EqualTo(0));
    }
    
    [Test]
    public void DiagonalPositive1Offset()
    {
        Chromaticity point1 = new(1, 11);
        Chromaticity point2 = new(2, 12);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(1));
        Assert.That(line.Intercept, Is.EqualTo(10));
    }
    
    [Test]
    public void DiagonalPositive2Offset()
    {
        Chromaticity point1 = new(1, 12);
        Chromaticity point2 = new(2, 14);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(2));
        Assert.That(line.Intercept, Is.EqualTo(10));
    }
    
    [Test]
    public void DiagonalNegative1Offset()
    {
        Chromaticity point1 = new(1, -11);
        Chromaticity point2 = new(2, -12);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(-1));
        Assert.That(line.Intercept, Is.EqualTo(-10));
    }
    
    [Test]
    public void DiagonalNegative2Offset()
    {
        Chromaticity point1 = new(1, -12);
        Chromaticity point2 = new(2, -14);
        var line = new LineSegment(point1, point2).Line;
        Assert.That(line.Slope, Is.EqualTo(-2));
        Assert.That(line.Intercept, Is.EqualTo(-10));
    }

    [Test]
    public void IntersectVertical()
    {
        var horizontalLine = new LineSegment((2, 1), (3, 1)).Line;
        var verticalLine = new LineSegment((5, 10), (5, 20)).Line;
        AssertIntersect(horizontalLine, verticalLine, 5, 1);
    }
    
    [Test]
    public void IntersectDiagonal()
    {
        var diagonalPositiveLine = new LineSegment((-5, -5), (5, 5)).Line;
        var diagonalNegativeLine = new LineSegment((5, -5), (-5, 5)).Line;
        AssertIntersect(diagonalPositiveLine, diagonalNegativeLine, 0, 0);
    }

    [Test]
    public void DifferentPointsSameLine()
    {
        var line1 = new LineSegment((0, 0), (10, 10)).Line;
        var line2 = new LineSegment((20, 20), (50, 50)).Line;
        AssertIntersect(line1, line2, double.NaN, double.NaN);
        TestUtils.AssertEqual(line1, line2);
    }
    
    private static void AssertIntersect(Line line1, Line line2, double expectedX, double expectedY)
    {
        var intersect1 = line1.GetIntersect(line2);
        Assert.That(intersect1.X, Is.EqualTo(expectedX));
        Assert.That(intersect1.Y, Is.EqualTo(expectedY));
        
        var intersect2 = line2.GetIntersect(line1);
        Assert.That(intersect2.X, Is.EqualTo(expectedX));
        Assert.That(intersect2.Y, Is.EqualTo(expectedY));
    }
}