namespace Wacton.Unicolour.Tests.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

internal static class HsluvTestColour
{
    public static readonly List<TestColour> All = new();

    static HsluvTestColour()
    {
        var snapshotText = File.ReadAllText(Path.Combine("Utils", "HSLuv-snapshot-rev4.json"));
        var snapshotJson = JObject.Parse(snapshotText);
        
        foreach (var (hex, jsonData) in snapshotJson)
        {
            if (jsonData == null) throw new InvalidOperationException();
            All.Add(new TestColour
            {
                Hex = hex,
                Rgb = ParseJson(jsonData, "rgb"),
                Xyz = ParseJson(jsonData, "xyz"),
                Luv = ParseJson(jsonData, "luv"),
                Lchuv = ParseJson(jsonData, "lch"),
                Hsluv = ParseJson(jsonData, "hsluv"),
                Hpluv = ParseJson(jsonData, "hpluv")
            });
        }
    }
    
    private static ColourTriplet ParseJson(JToken jToken, string lookup)
    {
        var jArray = jToken[lookup] as JArray;
        var first = double.Parse(jArray[0].ToString());
        var second = double.Parse(jArray[1].ToString());
        var third = double.Parse(jArray[2].ToString());
        return new ColourTriplet(first, second, third);
    }
}