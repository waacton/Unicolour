using NUnit.Framework;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests;

public class IccLookupTests
{
    private static readonly double[] table = [0, 0.1, 0.2, 0.4, 0.8, 1.0];
    private static readonly Curve curve = new TableCurve(table);
    private static readonly Curve inverseCurve = curve.Inverse();
    
    // input is % distance along the curve, e.g. 0.5 -> find value halfway along curve -> 0.3
    [TestCase(0.25, 0.1, 0.2, 0.25)]
    [TestCase(0.5, 0.2, 0.4, 0.5)]
    [TestCase(0.75, 0.4, 0.8, 0.75)]
    public void Lookup(double input, double lower, double upper, double distance)
    {
        var lookup = Lut.Lookup(table, input);
        Assert.That(lookup.lower, Is.EqualTo(lower));
        Assert.That(lookup.upper, Is.EqualTo(upper));
        Assert.That(lookup.distance, Is.EqualTo(distance));
    }
    
    // input is % distance along the curve, e.g. 0.5 -> find value halfway along curve -> 0.3
    [TestCase(0.25, 0.125)]
    [TestCase(0.5, 0.3)]
    [TestCase(0.75, 0.7)]
    public void Curve(double input, double expected)
    {
        var actual = curve.Lookup(input);
        Assert.That(actual, Is.EqualTo(expected).Within(0.000000000000001));
    }
    
    // output is % distance along the curve, e.g. 0.3 -> is halfway along curve -> 0.5
    [TestCase(0.125, 0.25)]
    [TestCase(0.3, 0.50)]
    [TestCase(0.7, 0.75)]
    public void InverseCurve(double input, double expected)
    {
        var actual = inverseCurve.Lookup(input);
        Assert.That(actual, Is.EqualTo(expected).Within(0.000000000000001));
    }
}