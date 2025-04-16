using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Tests;

public class DatasetArtistPaintTests
{
    [Test]
    public void All()
    {
        Assert.That(ArtistPaint.All.Count(), Is.EqualTo(19));
        Assert.That(ArtistPaint.All.Distinct().Count(), Is.EqualTo(19));

        foreach (var pigment in ArtistPaint.All)
        {
            Assert.That(pigment.ToString(), Does.Contain(pigment.Name));
        }
    }
}