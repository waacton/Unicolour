using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Wacton.Unicolour.Tests.Utils;

internal static class HsluvTestColour
{
    // ReSharper disable CollectionNeverQueried.Global - used in test case sources by name
    internal static readonly List<TestColour> All = [];
    // ReSharper restore CollectionNeverQueried.Global

    static HsluvTestColour()
    {
        var snapshotText = File.ReadAllText(Path.Combine("Data", "HSLuv-snapshot-rev4.json"));
        var snapshotJson = JsonDocument.Parse(snapshotText).RootElement;
        
        foreach (var colourElement in snapshotJson.EnumerateObject())
        {
            All.Add(new TestColour
            {
                Hex = colourElement.Name,
                Rgb = ParseTriplet(colourElement, "rgb"),
                Xyz = ParseTriplet(colourElement, "xyz"),
                Luv = ParseTriplet(colourElement, "luv"),
                Lchuv = ParseTriplet(colourElement, "lch"),
                Hsluv = ParseTriplet(colourElement, "hsluv"),
                Hpluv = ParseTriplet(colourElement, "hpluv")
            });
        }
    }
    
    private static ColourTriplet ParseTriplet(JsonProperty colourProperty, string colourSpaceText)
    {
        var colourSpaceElement = colourProperty.Value.GetProperty(colourSpaceText);
        var array = JsonSerializer.Deserialize<double[]>(colourSpaceElement.GetRawText())!;
        return new ColourTriplet(array[0], array[1], array[2]);
    }
}