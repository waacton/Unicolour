using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

public class SpectralLocusTests
{
    private static readonly Observer Observer = Observer.Degree2;
    private static readonly Chromaticity WhiteChromaticity = new(0.3, 0.3);
    private static readonly Spectral Spectral = new(Observer, WhiteChromaticity);

    // when the sample is exactly the same as the white point
    // there is no line that connects them
    // resulting in no possible intersects
    [Test] 
    public void SampleIsWhitePoint()
    {
        var intersects = Spectral.FindBoundaryIntersects(WhiteChromaticity);
        Assert.That(intersects, Is.Null);
    }
    
    // when the sample coordinate contains infinity
    // intersect can be calculated but distance to any other coordinate is infinity
    // resulting in infinity boundary intersect coordinates (no distinction between which is "near" vs "far")
    [Test]
    public void InfiniteDistanceToSampleX([Values(0.1, 0, -0.1)] double offset)
    {
        var intersects = Spectral.FindBoundaryIntersects(new(double.PositiveInfinity, WhiteChromaticity.Y + offset));
        Assert.That(intersects, Is.Null);
    }
    
    // when the sample coordinate contains infinity
    // intersect can be calculated but distance to any other coordinate is infinity
    // resulting in infinity boundary intersect coordinates (no distinction between which is "near" vs "far")
    [Test]
    public void InfiniteDistanceToSampleY([Values(0.1, 0, -0.1)] double offset)
    {
        var intersects = Spectral.FindBoundaryIntersects(new(WhiteChromaticity.X + offset, double.PositiveInfinity));
        Assert.That(intersects, Is.Null);
    }
    
    [TestCase(0.1, false)]
    [TestCase(-0.1, false)]
    [TestCase(1.1, true)]
    [TestCase(-1.1, true)]
    public void HorizontalIntersect(double offset, bool expectedImaginary)
    {
        var sample = new Chromaticity(WhiteChromaticity.X + offset, WhiteChromaticity.Y);
        var intersects = Spectral.FindBoundaryIntersects(sample)!;
        var isImaginary = Spectral.IsImaginary(sample);
        Assert.That(intersects.Sample.Y, Is.EqualTo(sample.Y));
        Assert.That(intersects.White.Y, Is.EqualTo(sample.Y));
        Assert.That(intersects.Near.Chromaticity.Y, Is.EqualTo(sample.Y));
        Assert.That(intersects.Far.Chromaticity.Y, Is.EqualTo(sample.Y));
        Assert.That(isImaginary, Is.EqualTo(expectedImaginary));
    }
    
    [TestCase(0.1, false)]
    [TestCase(-0.1, false)]
    [TestCase(1.1, true)]
    [TestCase(-1.1, true)]
    public void VerticalIntersect(double offset, bool expectedImaginary)
    {
        var sample = new Chromaticity(WhiteChromaticity.X, WhiteChromaticity.Y + offset);
        var intersects = Spectral.FindBoundaryIntersects(sample)!;
        var isImaginary = Spectral.IsImaginary(sample);
        Assert.That(intersects.Sample.X, Is.EqualTo(sample.X));
        Assert.That(intersects.White.X, Is.EqualTo(sample.X));
        Assert.That(intersects.Near.Chromaticity.X, Is.EqualTo(sample.X));
        Assert.That(intersects.Far.Chromaticity.X, Is.EqualTo(sample.X));
        Assert.That(isImaginary, Is.EqualTo(expectedImaginary));
    }

    [Test]
    public void SameSample()
    {
        var sample1 = new Chromaticity(WhiteChromaticity.X + 0.1, WhiteChromaticity.Y + 0.1);
        var sample2 = new Chromaticity(WhiteChromaticity.X + 0.1, WhiteChromaticity.Y + 0.1);
        var intersects1 = Spectral.FindBoundaryIntersects(sample1)!;
        var intersects2 = Spectral.FindBoundaryIntersects(sample2)!;
        TestUtils.AssertEqual(intersects1.Near, intersects2.Near);
        TestUtils.AssertEqual(intersects1.Far, intersects2.Far);
        TestUtils.AssertEqual(intersects1.Sample, intersects2.Sample);
        TestUtils.AssertEqual(intersects1.White, intersects2.White);
        TestUtils.AssertEqual(intersects1, intersects2);
    }
    
    [Test]
    public void DifferentSampleSameLine()
    {
        var sample1 = new Chromaticity(WhiteChromaticity.X + 0.1, WhiteChromaticity.Y + 0.1);
        var sample2 = new Chromaticity(WhiteChromaticity.X + 1.1, WhiteChromaticity.Y + 1.1);
        var intersects1 = Spectral.FindBoundaryIntersects(sample1)!;
        var intersects2 = Spectral.FindBoundaryIntersects(sample2)!;
        
        TestUtils.AssertEqual(intersects1.Near.Segment, intersects2.Near.Segment);
        TestUtils.AssertEqual(intersects1.Near.Wavelength, intersects2.Near.Wavelength);
        TestUtils.AssertEqual(intersects1.Near.DistanceToWhite, intersects2.Near.DistanceToWhite);
        TestUtils.AssertNotEqual(intersects1.Near.DistanceToSample, intersects2.Near.DistanceToSample);
        TestUtils.AssertEqual(intersects1.Far.Segment, intersects2.Far.Segment);
        TestUtils.AssertEqual(intersects1.Far.Wavelength, intersects2.Far.Wavelength);
        TestUtils.AssertEqual(intersects1.Far.DistanceToWhite, intersects2.Far.DistanceToWhite);
        TestUtils.AssertNotEqual(intersects1.Far.DistanceToSample, intersects2.Far.DistanceToSample);
        
        TestUtils.AssertNotEqual(intersects1.Near, intersects2.Near);
        TestUtils.AssertNotEqual(intersects1.Far, intersects2.Far);
        TestUtils.AssertNotEqual(intersects1.Sample, intersects2.Sample);
        TestUtils.AssertEqual(intersects1.White, intersects2.White);
        
        TestUtils.AssertNotEqual(intersects1, intersects2);
    }
    
    [Test]
    public void DifferentSampleDifferentLine()
    {
        var sample1 = new Chromaticity(WhiteChromaticity.X + 0.1, WhiteChromaticity.Y + 0.1);
        var sample2 = new Chromaticity(WhiteChromaticity.X - 0.1, WhiteChromaticity.Y + 0.1);
        var intersects1 = Spectral.FindBoundaryIntersects(sample1)!;
        var intersects2 = Spectral.FindBoundaryIntersects(sample2)!;
        
        TestUtils.AssertNotEqual(intersects1.Near.Segment, intersects2.Near.Segment);
        TestUtils.AssertNotEqual(intersects1.Near.Wavelength, intersects2.Near.Wavelength);
        TestUtils.AssertNotEqual(intersects1.Near.DistanceToWhite, intersects2.Near.DistanceToWhite);
        TestUtils.AssertNotEqual(intersects1.Near.DistanceToSample, intersects2.Near.DistanceToSample);
        TestUtils.AssertNotEqual(intersects1.Far.Segment, intersects2.Far.Segment);
        TestUtils.AssertNotEqual(intersects1.Far.Wavelength, intersects2.Far.Wavelength);
        TestUtils.AssertNotEqual(intersects1.Far.DistanceToWhite, intersects2.Far.DistanceToWhite);
        TestUtils.AssertNotEqual(intersects1.Far.DistanceToSample, intersects2.Far.DistanceToSample);
        
        TestUtils.AssertNotEqual(intersects1.Near, intersects2.Near);
        TestUtils.AssertNotEqual(intersects1.Far, intersects2.Far);
        TestUtils.AssertNotEqual(intersects1.Sample, intersects2.Sample);
        TestUtils.AssertEqual(intersects1.White, intersects2.White);
        
        TestUtils.AssertNotEqual(intersects1, intersects2);
    }

    [Test]
    public void SingleIntersect()
    {
        /*
         * there is only 1 intersect when white point is 360nm monochromatic light (bottom-left of horseshoe) and sample is at (0.8, 0.2)
         * which is the same location on the boundary as the white point
         * in which case:
         * - the near and far intersects are the same intersect
         * - the intersection is the exact location of the white point
         * - the dominant wavelength is the monochromatic wavelength
         * - excitation purity cannot be calculated; the intersect is both pure monochromatic light AND the white point where there is no colour
         */
        var minWavelengthXyz = Xyz.FromSpd(Spd.Monochromatic(Spectral.MinWavelength), Observer.Degree2);
        var minWavelengthWhite = WhitePoint.FromXyz(minWavelengthXyz).ToChromaticity();
        var spectral = new Spectral(Observer.Degree2, minWavelengthWhite);
        var sample = new Chromaticity(0.8, 0.2);
        var intersects = spectral.FindBoundaryIntersects(sample)!;
        var isImaginarySample = spectral.IsImaginary(sample);
        var isImaginaryWhitePoint = spectral.IsImaginary(minWavelengthWhite);
        Assert.That(intersects.Far, Is.EqualTo(intersects.Near));
        Assert.That(intersects.Near.DistanceToWhite, Is.Zero);
        Assert.That(intersects.DominantWavelength, Is.EqualTo(Spectral.MinWavelength));
        Assert.That(intersects.ExcitationPurity, Is.NaN);
        Assert.That(isImaginarySample, Is.True);
        Assert.That(isImaginaryWhitePoint, Is.False);
    }
    
    [Test]
    public void NoIntersect()
    {
        var white = new Chromaticity(0.8, 0.4);
        var sample = new Chromaticity(0.4, 0.8);
        var spectral = new Spectral(Observer.Degree2, white);
        var intersects = spectral.FindBoundaryIntersects(sample)!;
        var isImaginarySample = spectral.IsImaginary(sample);
        var isImaginaryWhitePoint = spectral.IsImaginary(white);
        Assert.That(intersects, Is.Null);
        Assert.That(isImaginarySample, Is.True);
        Assert.That(isImaginaryWhitePoint, Is.True);
    }
    
    private const int MidWavelength = Spectral.MinWavelength + (Spectral.MaxWavelength - Spectral.MinWavelength) / 2;
    
    [TestCase(Spectral.MinWavelength, double.NaN, -Spectral.MinWavelength)]
    [TestCase(Spectral.MaxWavelength, -Spectral.MaxWavelength, double.NaN)]
    [TestCase(MidWavelength, -MidWavelength, -MidWavelength)]
    public void WhitePointOnLocus(int wavelength, double expectedMinNegative, double expectedMaxNegative)
    {
        var xyz = Xyz.FromSpd(Spd.Monochromatic(wavelength), Observer.Degree2);
        var white = WhitePoint.FromXyz(xyz).ToChromaticity();
        var spectral = new Spectral(Observer.Degree2, white);
        Assert.That(spectral.MinNegativeWavelength, Is.EqualTo(expectedMinNegative).Within(0.000000000001));
        Assert.That(spectral.MaxNegativeWavelength, Is.EqualTo(expectedMaxNegative).Within(0.000000000001));
    }
}