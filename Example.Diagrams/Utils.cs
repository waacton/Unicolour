using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.TickGenerators;

namespace Wacton.Unicolour.Example.Diagrams;

internal static class Utils
{
    internal static Plot GetEmptyPlot((double min, double max) rangeX, (double min, double max) rangeY, double majorTickInterval)
    {
        var plot = new Plot();
        plot.Axes.SetLimits(rangeX.min, rangeX.max, rangeY.min, rangeY.max);
        plot.Axes.Bottom.TickGenerator = GetTickGenerator(rangeX.min, rangeX.max, majorTickInterval);
        plot.Axes.Left.TickGenerator = GetTickGenerator(rangeY.min, rangeY.max, majorTickInterval);
        return plot;
    }
    
    internal static List<Unicolour> GetSpectralLocus()
    {
        var data = new List<Unicolour>();
        for (var nm = 360; nm < 700; nm++)
        {
            data.Add(new Unicolour(new Spd(start: nm, interval: 1, 1.0)));
        }

        return data;
    }

    internal static List<Unicolour> GetRgbGamut()
    {
        var r = new Unicolour(ColourSpace.Rgb, 1, 0, 0);
        var g = new Unicolour(ColourSpace.Rgb, 0, 1, 0);
        var b = new Unicolour(ColourSpace.Rgb, 0, 0, 1);
        return new List<Unicolour> { r, g, b };
    }
    
    internal static List<Unicolour> GetBlackbodyLocus()
    {
        var data = new List<Unicolour>();
        for (var cct = 500; cct < 20000; cct += 100)
        {
            data.Add(new Unicolour(new Temperature(cct)));
        }

        return data;
    }
    
    internal static List<(Unicolour start, Unicolour end)> GetIsotherms()
    {
        var data = new List<(Unicolour start, Unicolour end)>();
        for (var cct = 2000; cct < 10000; cct += 1000)
        {
            var upper = new Unicolour(new Temperature(cct, 0.05));
            var lower = new Unicolour(new Temperature(cct, -0.05));
            data.Add((upper, lower));
        }

        return data;
    }

    internal static List<Marker> GetXyFillMarkers((double min, double max) rangeX, (double min, double max) rangeY, double increment)
    {
        var data = new List<Marker>();
        for (var x = rangeX.min; x < rangeX.max; x += increment)
        {
            for (var y = rangeY.min; y < rangeY.max; y += increment)
            {
                var chromaticity = new Chromaticity(x, y);
                var color = GetPlotColour(chromaticity);
                if (color == null) continue;

                var markerStyle = new MarkerStyle
                {
                    Shape = MarkerShape.FilledCircle,
                    FillColor = color.Value,
                    OutlineColor = Colors.Transparent,
                    OutlineWidth = 0f,
                    Size = 1.5f
                };
                
                var marker = new Marker { X = x, Y = y, MarkerStyle = markerStyle };
                data.Add(marker);
            }
        }
        
        return data;
    }
    
    internal static List<Marker> GetUvFillMarkers((double min, double max) rangeU, (double min, double max) rangeV, double increment)
    {
        var data = new List<Marker>();
        for (var u = rangeU.min; u < rangeU.max; u += increment)
        {
            for (var v = rangeV.min; v < rangeV.max; v += increment)
            {
                var chromaticity = Chromaticity.FromUv(u, v);
                var color = GetPlotColour(chromaticity);
                if (color == null) continue;
                
                var markerStyle = new MarkerStyle
                {
                    Shape = MarkerShape.FilledCircle,
                    FillColor = color.Value,
                    OutlineColor = Colors.Transparent,
                    OutlineWidth = 0f,
                    Size = 2f
                };
                
                var marker = new Marker { X = u, Y = v, MarkerStyle = markerStyle };
                data.Add(marker);
            }
        }

        return data;
    }
    
    private static readonly Dictionary<Chromaticity, Color?> ChromaticityCache = new();
    internal static Color? GetPlotColour(Chromaticity chromaticity)
    {
        Color? color;
        if (ChromaticityCache.ContainsKey(chromaticity))
        {
            color = ChromaticityCache[chromaticity];
        }
        else
        {
            var colour = new Unicolour(chromaticity);
            color = colour.IsImaginary ? null : GetScaledColour(colour.Rgb);
            ChromaticityCache.Add(chromaticity, color);
        }

        return color;
    }

    private static Color GetScaledColour(Rgb rgb)
    {
        var components = new[] { rgb.R, rgb.G, rgb.B };
        var max = components.Max();
        var scaled = components.Select(component => Clamp(component / max, 0, 1)).ToList();
        return new Color((float)scaled[0], (float)scaled[1], (float)scaled[2]);
    }

    internal static int GetWidth((double min, double max) rangeX, (double min, double max) rangeY, int height)
    {
        return (int)(height * (rangeX.max - rangeX.min) / (rangeY.max - rangeY.min));
    }
    
    private static NumericManual GetTickGenerator(double min, double max, double majorTickInterval)
    {
        const double increment = 0.05;
        var ticks = new List<Tick>();
        var nextMajorTick = min;
        for (var i = min; i < max + increment; i += increment)
        {
            var rounded = Math.Round(i, 2);
            var isMajor = IsEffectivelyZero(rounded - nextMajorTick);
            if (isMajor)
            {
                nextMajorTick += majorTickInterval;
            }
            
            ticks.Add(new Tick(rounded, isMajor ? $"{rounded:F2}" : string.Empty, isMajor));
        }

        return new NumericManual(ticks.ToArray());
    }

    private static bool IsEffectivelyZero(double x) => Math.Abs(x) < 5e-14;
    
    private static double Clamp(double value, double min, double max) => value < min ? min : value > max ? max : value;
}