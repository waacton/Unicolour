using NUnit.Framework;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests;

public class IccLookupTests
{
    private static readonly double[] Table = [0, 0.1, 0.2, 0.4, 0.8, 1.0];
    private static readonly Curve Curve = new TableCurve(Table);
    private static readonly Curve InverseCurve = Curve.Inverse();
    
    // input is % distance along the curve, e.g. 0.5 -> find value halfway along curve -> 0.3
    [TestCase(0.25, 0.1, 0.2, 0.25)]
    [TestCase(0.5, 0.2, 0.4, 0.5)]
    [TestCase(0.75, 0.4, 0.8, 0.75)]
    public void TableLookup(double input, double lower, double upper, double distance)
    {
        var lookup = Lut.Lookup(Table, input);
        Assert.That(lookup.lower, Is.EqualTo(lower));
        Assert.That(lookup.upper, Is.EqualTo(upper));
        Assert.That(lookup.distance, Is.EqualTo(distance));
    }
    
    // input is % distance along the curve, e.g. 0.5 -> find value halfway along curve -> 0.3
    [TestCase(0.25, 0.125)]
    [TestCase(0.5, 0.3)]
    [TestCase(0.75, 0.7)]
    public void CurveLookup(double input, double expected)
    {
        var actual = Curve.Lookup(input);
        Assert.That(actual, Is.EqualTo(expected).Within(0.000000000000001));
    }
    
    // output is % distance along the curve, e.g. 0.3 -> is halfway along curve -> 0.5
    [TestCase(0.125, 0.25)]
    [TestCase(0.3, 0.50)]
    [TestCase(0.7, 0.75)]
    public void InverseCurveLookup(double input, double expected)
    {
        var actual = InverseCurve.Lookup(input);
        Assert.That(actual, Is.EqualTo(expected).Within(0.000000000000001));
    }
}