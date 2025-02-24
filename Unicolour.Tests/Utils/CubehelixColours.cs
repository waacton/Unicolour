﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Wacton.Unicolour.Tests.Utils;

internal static class CubehelixColours
{
    internal static readonly List<ColourTriplet> Default = [];
    internal static readonly List<ColourTriplet> Custom = [];

    static CubehelixColours()
    {
        var defaultData = File.ReadAllLines(Path.Combine("Data", "Cubehelix-default")).Where(x => !x.StartsWith('#'));
        var customData = File.ReadAllLines(Path.Combine("Data", "Cubehelix-custom")).Where(x => !x.StartsWith('#'));

        foreach (var data in defaultData)
        {
            var split = data.Split(' ').Select(double.Parse).ToArray();
            Default.Add(new(split[1], split[2], split[3]));
        }
        
        foreach (var data in customData)
        {
            var split = data.Split(' ').Select(double.Parse).ToArray();
            Custom.Add(new(split[1], split[2], split[3]));
        }
    }
}