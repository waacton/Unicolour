namespace Wacton.Unicolour.Tests.Utils;

using System.Collections.Generic;
using NUnit.Framework;

public class AlphaInterpolationUtils
{
    public static readonly List<TestCaseData> PremultipliedNoHueComponent = new()
    {
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 0.00, new AlphaTriplet(new(0.500, 1.000, 0.500), 0.250)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 0.25, new AlphaTriplet(new(0.750, 0.750, 0.750), 0.375)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 0.50, new AlphaTriplet(new(0.875, 0.625, 0.875), 0.500)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 0.75, new AlphaTriplet(new(0.950, 0.550, 0.950), 0.625)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 0.5), 0.25), new AlphaTriplet(new(1.0, 0.5, 1.0), 0.75), 1.00, new AlphaTriplet(new(1.000, 0.500, 1.000), 0.750))
    };
    
    public static readonly List<TestCaseData> PremultipliedHueIndex0 = new()
    {
        new TestCaseData(new AlphaTriplet(new(90, 1.0, 0.5), 0.25), new AlphaTriplet(new(270, 0.5, 1.0), 0.75), 0.00, new AlphaTriplet(new(90, 1.000, 0.500), 0.250)),
        new TestCaseData(new AlphaTriplet(new(90, 1.0, 0.5), 0.25), new AlphaTriplet(new(270, 0.5, 1.0), 0.75), 0.25, new AlphaTriplet(new(135, 0.750, 0.750), 0.375)),
        new TestCaseData(new AlphaTriplet(new(90, 1.0, 0.5), 0.25), new AlphaTriplet(new(270, 0.5, 1.0), 0.75), 0.50, new AlphaTriplet(new(180, 0.625, 0.875), 0.500)),
        new TestCaseData(new AlphaTriplet(new(90, 1.0, 0.5), 0.25), new AlphaTriplet(new(270, 0.5, 1.0), 0.75), 0.75, new AlphaTriplet(new(225, 0.550, 0.950), 0.625)),
        new TestCaseData(new AlphaTriplet(new(90, 1.0, 0.5), 0.25), new AlphaTriplet(new(270, 0.5, 1.0), 0.75), 1.00, new AlphaTriplet(new(270, 0.500, 1.000), 0.750))
    };
    
    public static readonly List<TestCaseData> PremultipliedHueIndex2 = new()
    {
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 0.00, new AlphaTriplet(new(0.500, 1.000, 90), 0.250)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 0.25, new AlphaTriplet(new(0.750, 0.750, 135), 0.375)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 0.50, new AlphaTriplet(new(0.875, 0.625, 180), 0.500)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 0.75, new AlphaTriplet(new(0.950, 0.550, 225), 0.625)),
        new TestCaseData(new AlphaTriplet(new(0.5, 1.0, 90), 0.25), new AlphaTriplet(new(1.0, 0.5, 270), 0.75), 1.00, new AlphaTriplet(new(1.000, 0.500, 270), 0.750))
    };
}

public record AlphaTriplet(ColourTriplet Triplet, double Alpha)
{
    public (double first, double second, double third, double alpha) Tuple => (Triplet.First, Triplet.Second, Triplet.Third, Alpha);
    public override string ToString() => Tuple.ToString();
}
