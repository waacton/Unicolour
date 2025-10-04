using System;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class BoundaryTests
{
    private static readonly Lazy<Chromaticity[]> Points = new(() => [new(-1, 1), new(1, 1), new(1, -1), new(-1, -1)]);
    private static readonly Boundary Boundary = new(Points);
    private static readonly Chromaticity Reference = new(0, 0);

    // when the sample is exactly the same as the reference
    // there is no line that connects them
    // resulting in no possible intersects
    [Test] 
    public void SampleIsReference()
    {
        var intersects = Boundary.GetIntersects(sample: Reference, reference: Reference);
        Assert.That(intersects, Is.Null);
    }
    
    // when the sample coordinate contains infinity
    // intersect can be calculated but distance to any other coordinate is infinity
    // resulting in infinity boundary intersect coordinates (no distinction between which is "near" vs "far")
    [Test]
    public void InfiniteDistanceToSampleX([Values(0.1, 0, -0.1)] double offset)
    {
        var sample = new Chromaticity(double.PositiveInfinity, Reference.Y + offset);
        var intersects = Boundary.GetIntersects(sample, Reference);
        Assert.That(intersects, Is.Null);
    }
    
    // when the sample coordinate contains infinity
    // intersect can be calculated but distance to any other coordinate is infinity
    // resulting in infinity boundary intersect coordinates (no distinction between which is "near" vs "far")
    [Test]
    public void InfiniteDistanceToSampleY([Values(0.1, 0, -0.1)] double offset)
    {
        var sample = new Chromaticity(Reference.X + offset, double.PositiveInfinity);
        var intersects = Boundary.GetIntersects(sample, Reference);
        Assert.That(intersects, Is.Null);
    }
    
    [TestCase(0.1, true)]
    [TestCase(-0.1, true)]
    [TestCase(1.1, false)]
    [TestCase(-1.1, false)]
    public void HorizontalIntersect(double offset, bool expectedInside)
    {
        var sample = new Chromaticity(Reference.X + offset, Reference.Y);
        var (near, far) = Boundary.GetIntersects(sample, Reference)!.Value;
        Assert.That(near.Point.Y, Is.EqualTo(sample.Y));
        Assert.That(far.Point.Y, Is.EqualTo(sample.Y));
        Assert.That(Boundary.Contains(sample), Is.EqualTo(expectedInside));
    }
    
    [TestCase(0.1, true)]
    [TestCase(-0.1, true)]
    [TestCase(1.1, false)]
    [TestCase(-1.1, false)]
    public void VerticalIntersect(double offset, bool expectedInside)
    {
        var sample = new Chromaticity(Reference.X, Reference.Y + offset);
        var (near, far) = Boundary.GetIntersects(sample, Reference)!.Value;
        Assert.That(near.Point.X, Is.EqualTo(sample.X));
        Assert.That(far.Point.X, Is.EqualTo(sample.X));
        Assert.That(Boundary.Contains(sample), Is.EqualTo(expectedInside));
    }
    
    [Test]
    public void SameSample()
    {
        var sample1 = new Chromaticity(Reference.X + 0.1, Reference.Y + 0.1);
        var sample2 = new Chromaticity(Reference.X + 0.1, Reference.Y + 0.1);
        var (near1, far1) = Boundary.GetIntersects(sample1, Reference)!.Value;
        var (near2, far2) = Boundary.GetIntersects(sample2, Reference)!.Value;
        TestUtils.AssertEqual(near1, near2);
        TestUtils.AssertEqual(far1, far2);
    }
    
    [Test]
    public void DifferentSampleSameLine()
    {
        var sample1 = new Chromaticity(Reference.X + 0.1, Reference.Y + 0.1);
        var sample2 = new Chromaticity(Reference.X + 1.1, Reference.Y + 1.1);
        var (near1, far1) = Boundary.GetIntersects(sample1, Reference)!.Value;
        var (near2, far2) = Boundary.GetIntersects(sample2, Reference)!.Value;
        
        TestUtils.AssertEqual(near1.Segment, near2.Segment);
        TestUtils.AssertNotEqual(near1.DistanceFromSample, near2.DistanceFromSample);
        TestUtils.AssertEqual(near1.DistanceFromReference, near2.DistanceFromReference);
        TestUtils.AssertEqual(far1.Segment, far2.Segment);
        TestUtils.AssertNotEqual(far1.DistanceFromSample, far2.DistanceFromSample);
        TestUtils.AssertEqual(far1.DistanceFromReference, far2.DistanceFromReference);
        
        TestUtils.AssertNotEqual(near1, near2);
        TestUtils.AssertNotEqual(far1, far2);
    }
    
    [Test]
    public void DifferentSampleDifferentLine()
    {
        var sample1 = new Chromaticity(Reference.X + 0.1, Reference.Y + 0.1);
        var sample2 = new Chromaticity(Reference.X + 0.1, Reference.Y + 0.2);
        var (near1, far1) = Boundary.GetIntersects(sample1, Reference)!.Value;
        var (near2, far2) = Boundary.GetIntersects(sample2, Reference)!.Value;
        
        TestUtils.AssertNotEqual(near1.Segment, near2.Segment);
        TestUtils.AssertNotEqual(near1.DistanceFromSample, near2.DistanceFromSample);
        TestUtils.AssertNotEqual(near1.DistanceFromReference, near2.DistanceFromReference);
        TestUtils.AssertNotEqual(far1.Segment, far2.Segment);
        TestUtils.AssertNotEqual(far1.DistanceFromSample, far2.DistanceFromSample);
        TestUtils.AssertNotEqual(far1.DistanceFromReference, far2.DistanceFromReference);
        
        TestUtils.AssertNotEqual(near1, near2);
        TestUtils.AssertNotEqual(far1, far2);
    }
    
    [Test]
    public void BoundaryPointMultipleIntersect()
    {
        /*
         * sample is exact same point as top-right corner of the square
         * with (0, 0) as the reference, all 4 segments are intersected by line (0, 0) -> (1, 1)
         * top & right are on the intersecting segment, and will be treated as the near intersects
         * bottom & left are not on the intersecting segment, and will be treated as the far intersects
         * only one of each is selected to represent near and far (does not matter which because they are all equidistant)
         */
        var sample = new Chromaticity(1, 1); 
        var opposite = new Chromaticity(-1, -1); 
        var (near, far) = Boundary.GetIntersects(sample, Reference)!.Value;
        Assert.That(near.Point, Is.EqualTo(sample));
        Assert.That(far.Point, Is.EqualTo(opposite));
        Assert.That(Boundary.Contains(sample), Is.True);
    }
    
    [Test]
    public void BoundaryPointSingleIntersect()
    {
        /*
         * sample is exact same point as top-right corner of the square
         * with (2, 0) as the reference, only 2 segments (top & right) are intersected by line (2, 0) -> (1, 1)
         * both are initially treated as the near intersects, and there are no far intersects
         * so one will be selected to represent far (does not matter which because they are equidistant)
         */
        var sample = new Chromaticity(1, 1);
        var reference = new Chromaticity(2, 0);
        var (near, far) = Boundary.GetIntersects(sample, reference)!.Value;
        Assert.That(near.Point, Is.EqualTo(sample));
        Assert.That(far.Point, Is.EqualTo(sample));
        Assert.That(far, Is.Not.EqualTo(near));
        Assert.That(Boundary.Contains(sample), Is.True);
        Assert.That(Boundary.Contains(reference), Is.False);
    }
    
    [Test]
    public void NoIntersect()
    {
        var sample = new Chromaticity(2, 0);
        var reference = new Chromaticity(2, 1);
        var intersects = Boundary.GetIntersects(sample, reference);
        var isInside = Boundary.Contains(sample);
        Assert.That(intersects, Is.Null);
        Assert.That(isInside, Is.False);
    }
}