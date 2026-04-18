using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Tests;

public class DatasetIsccNbsTests
{
    [Test]
    public void All()
    {
        Assert.That(IsccNbs.All.Count(), Is.EqualTo(267));
        Assert.That(IsccNbs.All.Distinct().Count(), Is.EqualTo(267));
    }
}