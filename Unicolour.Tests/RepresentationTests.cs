using NUnit.Framework;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

internal record RepresentationTests() : TestRepresentation(0.1, 0.2, 0.3, ColourHeritage.None)
{
    [Test]
    public void UnconstrainedValues()
    {
        Assert.That(First, Is.EqualTo(0.1));
        Assert.That(Second, Is.EqualTo(0.2));
        Assert.That(Third, Is.EqualTo(0.3));
        Assert.That(Triplet, Is.EqualTo(new ColourTriplet(0.1, 0.2, 0.3)));
    }
    
    [Test]
    public void ConstrainedValues()
    {
        Assert.That(ConstrainedFirst, Is.EqualTo(0.1));
        Assert.That(ConstrainedSecond, Is.EqualTo(0.2));
        Assert.That(ConstrainedThird, Is.EqualTo(0.3));
        Assert.That(ConstrainedTriplet, Is.EqualTo(new ColourTriplet(0.1, 0.2, 0.3)));
    }
}