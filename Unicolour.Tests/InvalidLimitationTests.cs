using System;
using System.Reflection;
using NUnit.Framework;

namespace Wacton.Unicolour.Tests;

public class InvalidLimitationTests
{
    [Test]
    public void Unset()
    {
        var colour1 = new Unicolour("#FF0000");
        var colour2 = new Unicolour("#0000FF");
        
        ForceUnsetLimitation(colour1.Rgb);
        Assert.Throws<InvalidOperationException>(() => colour1.Mix(colour2, ColourSpace.Rgb));
    }
    
    private static void ForceUnsetLimitation(ColourBase colourBase)
    {
        // ColourBase.Limitation is an internal get-only property, which results in a generated backing field
        var backingField = typeof(ColourBase).GetField("<LimitationBaseline>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!;
        backingField.SetValue(colourBase, Limitation.Unset);
    }
}