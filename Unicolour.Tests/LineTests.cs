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
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.NaN);
        Assert.That(segment.Line.Intercept, Is.NaN);
        Assert.That(segment.Length, Is.Zero);
    }
    
    [Test]
    public void Vertical()
    {
        Chromaticity point1 = new(1, 2);
        Chromaticity point2 = new(1, 3);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(double.PositiveInfinity));
        Assert.That(segment.Line.Intercept, Is.EqualTo(point1.X));
        Assert.That(segment.Length, Is.EqualTo(1));
    }
    
    [Test]
    public void Horizontal()
    {
        Chromaticity point1 = new(2, 1);
        Chromaticity point2 = new(3, 1);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(0));
        Assert.That(segment.Line.Intercept, Is.EqualTo(point1.Y));
        Assert.That(segment.Length, Is.EqualTo(1));

    }
    
    [Test]
    public void DiagonalPositive1()
    {
        Chromaticity point1 = new(1, 1);
        Chromaticity point2 = new(2, 2);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(1));
        Assert.That(segment.Line.Intercept, Is.EqualTo(0));
        Assert.That(segment.Length, Is.EqualTo(1.4142135623731).Within(0.0000000000001));
    }
    
    [Test]
    public void DiagonalPositive2()
    {
        Chromaticity point1 = new(1, 2);
        Chromaticity point2 = new(2, 4);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(2));
        Assert.That(segment.Line.Intercept, Is.EqualTo(0));
        Assert.That(segment.Length, Is.EqualTo(2.2360679774998).Within(0.0000000000001));
    }
    
    [Test]
    public void DiagonalNegative1()
    {
        Chromaticity point1 = new(1, -1);
        Chromaticity point2 = new(2, -2);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(-1));
        Assert.That(segment.Line.Intercept, Is.EqualTo(0));
        Assert.That(segment.Length, Is.EqualTo(1.4142135623731).Within(0.0000000000001));
    }
    
    [Test]
    public void DiagonalNegative2()
    {
        Chromaticity point1 = new(1, -2);
        Chromaticity point2 = new(2, -4);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(-2));
        Assert.That(segment.Line.Intercept, Is.EqualTo(0));
        Assert.That(segment.Length, Is.EqualTo(2.2360679774998).Within(0.0000000000001));
    }
    
    [Test]
    public void DiagonalPositive1Offset()
    {
        Chromaticity point1 = new(1, 11);
        Chromaticity point2 = new(2, 12);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(1));
        Assert.That(segment.Line.Intercept, Is.EqualTo(10));
        Assert.That(segment.Length, Is.EqualTo(1.4142135623731).Within(0.0000000000001));
    }
    
    [Test]
    public void DiagonalPositive2Offset()
    {
        Chromaticity point1 = new(1, 12);
        Chromaticity point2 = new(2, 14);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(2));
        Assert.That(segment.Line.Intercept, Is.EqualTo(10));
        Assert.That(segment.Length, Is.EqualTo(2.2360679774998).Within(0.0000000000001));
    }
    
    [Test]
    public void DiagonalNegative1Offset()
    {
        Chromaticity point1 = new(1, -11);
        Chromaticity point2 = new(2, -12);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(-1));
        Assert.That(segment.Line.Intercept, Is.EqualTo(-10));
        Assert.That(segment.Length, Is.EqualTo(1.4142135623731).Within(0.0000000000001));
    }
    
    [Test]
    public void DiagonalNegative2Offset()
    {
        Chromaticity point1 = new(1, -12);
        Chromaticity point2 = new(2, -14);
        var segment = new LineSegment(point1, point2);
        Assert.That(segment.Line.Slope, Is.EqualTo(-2));
        Assert.That(segment.Line.Intercept, Is.EqualTo(-10));
        Assert.That(segment.Length, Is.EqualTo(2.2360679774998).Within(0.0000000000001));
    }

    [Test]
    public void IntersectVertical()
    {
        var horizontalSegment = new LineSegment((2, 1), (3, 1));
        var verticalSegment = new LineSegment((5, 10), (5, 20));
        AssertIntersect(horizontalSegment, verticalSegment, 5, 1);
    }
    
    [Test]
    public void IntersectDiagonal()
    {
        var diagonalPositiveSegment = new LineSegment((-5, -5), (5, 5));
        var diagonalNegativeSegment = new LineSegment((5, -5), (-5, 5));
        AssertIntersect(diagonalPositiveSegment, diagonalNegativeSegment, 0, 0);
    }
    
    [Test]
    public void SamePoints()
    {
        var segment1 = new LineSegment((20, 20), (50, 50));
        var segment2 = new LineSegment((20, 20), (50, 50));
        TestUtils.AssertEqual(segment1, segment2);
    }

    [Test]
    public void DifferentPointsSameLine()
    {
        var segment1 = new LineSegment((0, 0), (10, 10));
        var segment2 = new LineSegment((20, 20), (50, 50));
        AssertIntersect(segment1, segment2, double.NaN, double.NaN);
        TestUtils.AssertEqual(segment1.Line, segment2.Line);
    }
    
    private static readonly TestCaseData[] PolarCoordinateData =
    [
        new(new Chromaticity(1, 1), new Chromaticity(2, 1), 1, 0),
        new(new Chromaticity(1, 1), new Chromaticity(2, 2), 1.4142135623731, 45),
        new(new Chromaticity(1, 1), new Chromaticity(1, 2), 1, 90),
        new(new Chromaticity(1, 1), new Chromaticity(0, 2), 1.4142135623731, 135),
        new(new Chromaticity(1, 1), new Chromaticity(0, 1), 1, 180),
        new(new Chromaticity(1, 1), new Chromaticity(0, 0), 1.4142135623731, 225),
        new(new Chromaticity(1, 1), new Chromaticity(1, 0), 1, 270),
        new(new Chromaticity(1, 1), new Chromaticity(2, 0), 1.4142135623731, 315)
    ];

    [TestCaseSource(nameof(PolarCoordinateData))]
    public void PolarCoordinates(Chromaticity start, Chromaticity end, double expectedRadius, double expectedAngle)
    {
        var (radius, angle) = LineSegment.Polar(start, end);
        Assert.That(radius, Is.EqualTo(expectedRadius).Within(0.0000000000001));
        Assert.That(angle, Is.EqualTo(expectedAngle));
    }
    
    private static void AssertIntersect(LineSegment segment1, LineSegment segment2, double expectedX, double expectedY)
    {
        var intersect1 = segment1.Line.GetIntersect(segment2.Line);
        Assert.That(intersect1.X, Is.EqualTo(expectedX));
        Assert.That(intersect1.Y, Is.EqualTo(expectedY));
        
        var intersect2 = segment2.Line.GetIntersect(segment1.Line);
        Assert.That(intersect2.X, Is.EqualTo(expectedX));
        Assert.That(intersect2.Y, Is.EqualTo(expectedY));
    }
}