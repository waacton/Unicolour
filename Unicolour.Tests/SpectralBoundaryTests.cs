using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class SpectralBoundaryTests
{
    private static readonly Observer Observer = Observer.Degree2;
    private static readonly WhitePoint WhitePoint = new(0.3, 0.3);
    private static readonly SpectralBoundary SpectralBoundary = new(Observer, WhitePoint);

    // when the sample is exactly the same as the white point
    // there is no line that connects them
    // resulting in no possible intersects
    [Test] 
    public void SampleIsWhitePoint()
    {
        var intersects = SpectralBoundary.GetIntersects(WhitePoint.Chromaticity);
        Assert.That(intersects, Is.Null);
    }
    
    [Test]
    public void SingleIntersect()
    {
        /*
         * there is only 1 intersect when white point is 360nm monochromatic light (bottom-left of horseshoe) and sample is at (0.8, 0.2)
         * which is the same location on the boundary as the white point (reference)
         * in which case:
         * - the near and far intersects are the same intersect
         * - the intersect point is the exact location of the white point
         * - the dominant wavelength is the monochromatic wavelength
         * - excitation purity cannot be calculated; the intersect is both pure monochromatic light AND the white point where there is no colour
         */
        var minWavelengthXyz = Xyz.FromSpd(Spd.Monochromatic(SpectralBoundary.MinWavelength), Observer.Degree2, WhitePoint);
        var minWavelengthWhitePoint = new WhitePoint(minWavelengthXyz.X, minWavelengthXyz.Y, minWavelengthXyz.Z);
        var boundary = new SpectralBoundary(Observer.Degree2, minWavelengthWhitePoint);
        var sample = new Chromaticity(0.8, 0.2);
        var (near, far) = boundary.GetIntersects(sample)!.Value;
        var isSampleInside = boundary.Contains(sample);
        var isWhiteInside = boundary.Contains(minWavelengthWhitePoint.Chromaticity);
        Assert.That(far, Is.EqualTo(near));
        Assert.That(near.Point, Is.EqualTo(minWavelengthWhitePoint.Chromaticity));
        Assert.That(near.DistanceFromReference, Is.Zero);
        Assert.That(isSampleInside, Is.False);
        Assert.That(isWhiteInside, Is.True);
    }
    
    [Test]
    public void NoIntersect()
    {
        var whitePoint = new WhitePoint(0.8, 0.4);
        var sample = new Chromaticity(0.4, 0.8);
        var boundary = new SpectralBoundary(Observer.Degree2, whitePoint);
        var intersects = boundary.GetIntersects(sample);
        var isSampleInside = boundary.Contains(sample);
        var isWhiteInside = boundary.Contains(whitePoint.Chromaticity);
        Assert.That(intersects, Is.Null);
        Assert.That(isSampleInside, Is.False);
        Assert.That(isWhiteInside, Is.False);
    }
    
    private const int MidWavelength = SpectralBoundary.MinWavelength + (SpectralBoundary.MaxWavelength - SpectralBoundary.MinWavelength) / 2;
    
    [TestCase(SpectralBoundary.MinWavelength, double.NaN, -SpectralBoundary.MinWavelength)]
    [TestCase(SpectralBoundary.MaxWavelength, -SpectralBoundary.MaxWavelength, double.NaN)]
    [TestCase(MidWavelength, -MidWavelength, -MidWavelength)]
    public void WhitePointOnLocus(int wavelength, double expectedMinNegative, double expectedMaxNegative)
    {
        var (x, y, z) = Xyz.FromSpd(Spd.Monochromatic(wavelength), Observer.Degree2);
        var whitePoint = new WhitePoint(x, y, z);
        var boundary = new SpectralBoundary(Observer.Degree2, whitePoint);
        Assert.That(boundary.MinNegativeWavelength, Is.EqualTo(expectedMinNegative).Within(0.000000000001));
        Assert.That(boundary.MaxNegativeWavelength, Is.EqualTo(expectedMaxNegative).Within(0.000000000001));
    }
}