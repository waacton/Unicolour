namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;
using static Planckian;

public class PlanckianTableTests
{
    [Test]
    public void Coordinates()
    {
        // because the observer's CMF matches the same value for all wavelengths
        // each CCT will result in equal XYZ components
        // and therefore an xy-chromaticity of (0.3333, 0.3333)
        var equalCmf = new Cmf(Cmf.RequiredWavelengths.ToDictionary(wavelength => wavelength, _ => (1.0, 1.0, 1.0)));
        var equalObserver = new Observer(equalCmf, "constant");
        var chromaticity = new Chromaticity(1 / 3.0, 1 / 3.0);
        var expectedCoordinates = new List<Coordinate>
        {
            new(0, double.NaN, double.NaN),
            new(100, chromaticity.U, chromaticity.V),
            new(200, chromaticity.U, chromaticity.V),
            new(400, chromaticity.U, chromaticity.V),
            new(800, chromaticity.U, chromaticity.V),
            new(1600, chromaticity.U, chromaticity.V),
            new(3200, chromaticity.U, chromaticity.V)
        };

        var planckian = new Planckian(equalObserver);
        var actualCoordinates = planckian.Get(startCct: 100, endCct: 1600, stepPercentage: 100);
        AssertCoordinates(actualCoordinates, expectedCoordinates);
        AssertSearchResults(GetSearchResult(actualCoordinates), GetSearchResult(expectedCoordinates));
    }
    
    private static SearchResult GetSearchResult(List<Coordinate> coordinates)
    {
        var previous = new Distance(coordinates[2], 0.00005);
        var closest = new Distance(coordinates[3], 0.00001);
        var next = new Distance(coordinates[4], 0.00005);
        return new SearchResult(previous, closest, next, 0.25);
    }

    private void AssertCoordinates(List<Coordinate> actual, List<Coordinate> expected)
    {
        Assert.That(actual, Is.EquivalentTo(expected));
        for (var i = 0; i < expected.Count; i++)
        {
            TestUtils.AssertEqual(actual[i], expected[i]);
        }
    }
    
    private void AssertSearchResults(SearchResult actual, SearchResult expected)
    {
        TestUtils.AssertEqual(actual.Previous, expected.Previous);
        TestUtils.AssertEqual(actual.Closest, expected.Closest);
        TestUtils.AssertEqual(actual.Next, expected.Next);
        TestUtils.AssertEqual(actual.Vx, expected.Vx);
        TestUtils.AssertEqual(actual, expected);
    }
}