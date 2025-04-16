using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Tests;

public class DatasetNordTests
{
    private static readonly Unicolour[] AllNord = Nord.All.ToArray();
    
    [TestCase(0, "#2e3440")]
    [TestCase(1, "#3b4252")]
    [TestCase(2, "#434c5e")]
    [TestCase(3, "#4c566a")]
    [TestCase(4, "#d8dee9")]
    [TestCase(5, "#e5e9f0")]
    [TestCase(6, "#eceff4")]
    [TestCase(7, "#8fbcbb")]
    [TestCase(8, "#88c0d0")]
    [TestCase(9, "#81a1c1")]
    [TestCase(10, "#5e81ac")]
    [TestCase(11, "#bf616a")]
    [TestCase(12, "#d08770")]
    [TestCase(13, "#ebcb8b")]
    [TestCase(14, "#a3be8c")]
    [TestCase(15, "#b48ead")]
    public void ByIndex(int index, string expected)
    {
        var colour = AllNord[index];
        Assert.That(colour.Hex.ToLower(), Is.EqualTo(expected.ToLower()));
    }
    
    [Test]
    public void All()
    {
        Assert.That(Nord.All.Count(), Is.EqualTo(16));
        Assert.That(Nord.All.Distinct().Count(), Is.EqualTo(16));
    }
}