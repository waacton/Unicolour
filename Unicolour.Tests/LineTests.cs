namespace Wacton.Unicolour.Tests;

using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

public class LineTests
{
    [Test]
    public void None()
    {
        (double x, double y) point1 = (1, 1);
        (double x, double y) point2 = (1, 1);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.NaN);
        Assert.That(line.Intercept, Is.NaN);
    }
    
    [Test]
    public void Vertical()
    {
        (double x, double y) point1 = (1, 2);
        (double x, double y) point2 = (1, 3);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(double.PositiveInfinity));
        Assert.That(line.Intercept, Is.EqualTo(point1.x));
    }
    
    [Test]
    public void Horizontal()
    {
        (double x, double y) point1 = (2, 1);
        (double x, double y) point2 = (3, 1);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(0));
        Assert.That(line.Intercept, Is.EqualTo(point1.y));
    }
    
    [Test]
    public void DiagonalPositive1()
    {
        (double x, double y) point1 = (1, 1);
        (double x, double y) point2 = (2, 2);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(1));
        Assert.That(line.Intercept, Is.EqualTo(0));
    }
    
    [Test]
    public void DiagonalPositive2()
    {
        (double x, double y) point1 = (1, 2);
        (double x, double y) point2 = (2, 4);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(2));
        Assert.That(line.Intercept, Is.EqualTo(0));
    }
    
    [Test]
    public void DiagonalNegative1()
    {
        (double x, double y) point1 = (1, -1);
        (double x, double y) point2 = (2, -2);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(-1));
        Assert.That(line.Intercept, Is.EqualTo(0));
    }
    
    [Test]
    public void DiagonalNegative2()
    {
        (double x, double y) point1 = (1, -2);
        (double x, double y) point2 = (2, -4);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(-2));
        Assert.That(line.Intercept, Is.EqualTo(0));
    }
    
    [Test]
    public void DiagonalPositive1Offset()
    {
        (double x, double y) point1 = (1, 11);
        (double x, double y) point2 = (2, 12);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(1));
        Assert.That(line.Intercept, Is.EqualTo(10));
    }
    
    [Test]
    public void DiagonalPositive2Offset()
    {
        (double x, double y) point1 = (1, 12);
        (double x, double y) point2 = (2, 14);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(2));
        Assert.That(line.Intercept, Is.EqualTo(10));
    }
    
    [Test]
    public void DiagonalNegative1Offset()
    {
        (double x, double y) point1 = (1, -11);
        (double x, double y) point2 = (2, -12);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(-1));
        Assert.That(line.Intercept, Is.EqualTo(-10));
    }
    
    [Test]
    public void DiagonalNegative2Offset()
    {
        (double x, double y) point1 = (1, -12);
        (double x, double y) point2 = (2, -14);
        var line = Line.FromPoints(point1, point2);
        Assert.That(line.Slope, Is.EqualTo(-2));
        Assert.That(line.Intercept, Is.EqualTo(-10));
    }

    [Test]
    public void IntersectVertical()
    {
        var horizontalLine = Line.FromPoints((2, 1), (3, 1));
        var verticalLine = Line.FromPoints((5, 10), (5, 20));
        AssertIntersect(horizontalLine, verticalLine, 5, 1);
    }
    
    [Test]
    public void IntersectDiagonal()
    {
        var diagonalPositiveLine = Line.FromPoints((-5, -5), (5, 5));
        var diagonalNegativeLine = Line.FromPoints((5, -5), (-5, 5));
        AssertIntersect(diagonalPositiveLine, diagonalNegativeLine, 0, 0);
    }

    [Test]
    public void DifferentPointsSameLine()
    {
        var line1 = Line.FromPoints((0, 0), (10, 10));
        var line2 = Line.FromPoints((20, 20), (50, 50));
        AssertIntersect(line1, line2, double.NaN, double.NaN);
        TestUtils.AssertEqual(line1, line2);
    }
    
    private static void AssertIntersect(Line line1, Line line2, double expectedX, double expectedY)
    {
        var intersect1 = line1.GetIntersect(line2);
        Assert.That(intersect1.x, Is.EqualTo(expectedX));
        Assert.That(intersect1.y, Is.EqualTo(expectedY));
        
        var intersect2 = line2.GetIntersect(line1);
        Assert.That(intersect2.x, Is.EqualTo(expectedX));
        Assert.That(intersect2.y, Is.EqualTo(expectedY));
    }
}