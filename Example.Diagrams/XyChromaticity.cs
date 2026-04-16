using ScottPlot;
using ScottPlot.Plottables;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Example.Diagrams;

public static class XyChromaticity
{
    private const int Height = 640;
    private static readonly (double min, double max) RangeX = (0.0, 0.8);
    private static readonly (double min, double max) RangeY = (0.0, 0.9);
    private static readonly int Width = Utils.GetWidth(RangeX, RangeY, Height);

    public static void SpectralLocus()
    {
        var plot = Utils.GetEmptyPlot(RangeX, RangeY, majorTickInterval: 0.1);
        plot.DataBackground = new BackgroundStyle { Color = Color.Gray(96) };
        plot.Grid.MajorLineColor = Color.Gray(128);
    
        var spectralLocus = Utils.GetSpectralLocus();
        var spectralCoordinates = spectralLocus.Select(colour => new Coordinates(colour.Chromaticity.X, colour.Chromaticity.Y)).ToList();
        var spectralLocusScatter = plot.Add.Scatter(spectralCoordinates);
        spectralLocusScatter.Color = Color.Gray(64);
        spectralLocusScatter.LineWidth = 1;
        spectralLocusScatter.MarkerStyle = MarkerStyle.None;
    
        var firstCoordinate = new Coordinates(spectralLocus.First().Chromaticity.X, spectralLocus.First().Chromaticity.Y);
        var lastCoordinate = new Coordinates(spectralLocus.Last().Chromaticity.X, spectralLocus.Last().Chromaticity.Y);
        var lineOfPurples = plot.Add.Line(firstCoordinate, lastCoordinate);
        lineOfPurples.Color = new Color(255, 0, 255);
        lineOfPurples.LineWidth = 1;
        lineOfPurples.MarkerStyle = MarkerStyle.None;
    
        foreach (var colour in spectralLocus)
        {
            var chromaticity = colour.Chromaticity;
            var marker = new Marker
            {
                X = colour.Chromaticity.X, 
                Y = colour.Chromaticity.Y,
                Shape = MarkerShape.FilledCircle,
                Color = Utils.GetPlotColour(chromaticity)!.Value, 
                Size = 5f
            };
        
            plot.Add.Plottable(marker);
        }
    
        plot.SavePng(Utils.GetOutputPath("diagram-xy-spectral-locus.png"), Width, Height);
    }
    
    public static void RgbTriangle()
    {
        var plot = Utils.GetEmptyPlot(RangeX, RangeY, majorTickInterval: 0.1);
    
        var fillMarkers = Utils.GetXyFillMarkers(RangeX, RangeY, increment: 0.001);
        foreach (var marker in fillMarkers)
        {
            plot.Add.Plottable(marker);
        }
        
        AddSpectralOutline(plot);

        var rgbGamut = Utils.GetRgbGamut();
        var rgbCoordinates = rgbGamut.Select(colour => new Coordinates(colour.Chromaticity.X, colour.Chromaticity.Y));
        var rgbPolygon = plot.Add.Polygon(rgbCoordinates.ToArray());
        rgbPolygon.LineStyle = new LineStyle { Color = new Color(0f, 0f, 0f, 0.25f), Width = 2.5f };
        rgbPolygon.FillStyle = new FillStyle { Color = Colors.Transparent };
    
        plot.SavePng(Utils.GetOutputPath("diagram-xy-chromaticity-rgb.png"), Width, Height);
    }

    public static void Blackbody()
    {
        var plot = Utils.GetEmptyPlot(RangeX, RangeY, majorTickInterval: 0.1);
    
        var fillMarkers = Utils.GetXyFillMarkers(RangeX, RangeY, increment: 0.001);
        foreach (var marker in fillMarkers)
        {
            plot.Add.Plottable(marker);
        }

        AddSpectralOutline(plot);

        var blackbodyLocus = Utils.GetBlackbodyLocus();
        var blackbodyCoordinates = blackbodyLocus.Select(colour => new Coordinates(colour.Chromaticity.X, colour.Chromaticity.Y)).ToList();
        var blackbodyScatter = plot.Add.Scatter(blackbodyCoordinates);
        blackbodyScatter.Color = Colors.Black;
        blackbodyScatter.LineWidth = 2.5f;
        blackbodyScatter.MarkerStyle = MarkerStyle.None;

        var isotherms = Utils.GetIsotherms();
        foreach (var (startColour, endColour) in isotherms)
        {
            var startCoordinates = new Coordinates(startColour.Chromaticity.X, startColour.Chromaticity.Y);
            var endCoordinates = new Coordinates(endColour.Chromaticity.X, endColour.Chromaticity.Y);
            var duvLine = plot.Add.Line(startCoordinates, endCoordinates);
            duvLine.Color = Colors.Black;
            duvLine.LineWidth = 2.5f;
        }

        plot.SavePng(Utils.GetOutputPath("diagram-xy-chromaticity-blackbody.png"), Width, Height);
    }

    public static void MacAdamLimits()
    {
        var plot = Utils.GetEmptyPlot(RangeX, RangeY, majorTickInterval: 0.1);
        
        AddSpectralOutline(plot);
        
        IEnumerable<Unicolour>[] macAdamLimits =
        [
            MacAdam.Limits10, MacAdam.Limits20, MacAdam.Limits30, MacAdam.Limits40, MacAdam.Limits50, 
            MacAdam.Limits60, MacAdam.Limits70, MacAdam.Limits80, MacAdam.Limits90, MacAdam.Limits95
        ];
        
        foreach (var limits in macAdamLimits)
        {
            var coordinates = limits.Select(colour => new Coordinates(colour.Chromaticity.X, colour.Chromaticity.Y)).ToList();
            coordinates.Add(coordinates.First());
            var scatter = plot.Add.Scatter(coordinates);

            var luminance = limits.First().RelativeLuminance; // all colours in the group will have the same luminance
            var mapColour = Colourmaps.Flare.Map(1 - luminance).Rgb.Byte255;
            var color = new Color((byte)mapColour.R, (byte)mapColour.G, (byte)mapColour.B);
            scatter.LineStyle = new LineStyle { Color = color, Width = 2.5f };
            scatter.MarkerStyle = MarkerStyle.None;
        }

        plot.SavePng(Utils.GetOutputPath("diagram-xy-macadam-limits.png"), Width, Height);
    }

    private static void AddSpectralOutline(Plot plot)
    {
        var spectralLocus = Utils.GetSpectralLocus();
        var spectralCoordinates = spectralLocus.Select(colour => new Coordinates(colour.Chromaticity.X, colour.Chromaticity.Y)).ToList();
        spectralCoordinates.Add(spectralCoordinates.First());
        var spectralLocusScatter = plot.Add.Scatter(spectralCoordinates);
        spectralLocusScatter.Color = Colors.Black;
        spectralLocusScatter.LineWidth = 2.5f;
        spectralLocusScatter.MarkerStyle = MarkerStyle.None;
    }
}