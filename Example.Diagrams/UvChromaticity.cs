using ScottPlot;
using ScottPlot.Plottables;
using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Example.Diagrams;

public static class UvChromaticity
{
    private const int Height = 640;
    private static readonly (double min, double max) RangeU = (0.0, 0.7);
    private static readonly (double min, double max) RangeV = (0.0, 0.4);
    private static readonly int Width = Utils.GetWidth(RangeU, RangeV, Height);

    public static void SpectralLocus()
    {
        var plot = Utils.GetEmptyPlot(RangeU, RangeV, majorTickInterval: 0.05);
        plot.DataBackground = new BackgroundStyle { Color = Color.Gray(96) };
        plot.Grid.MajorLineColor = Color.Gray(128);
    
        var spectralLocus = Utils.GetSpectralLocus();
        var spectralCoordinates = spectralLocus.Select(colour => new Coordinates(colour.Chromaticity.U, colour.Chromaticity.V)).ToList();
        var spectralLocusScatter = plot.Add.Scatter(spectralCoordinates);
        spectralLocusScatter.Color = Color.Gray(64);
        spectralLocusScatter.LineWidth = 1;
        spectralLocusScatter.MarkerStyle = MarkerStyle.None;
    
        var firstCoordinate = new Coordinates(spectralLocus.First().Chromaticity.U, spectralLocus.First().Chromaticity.V);
        var lastCoordinate = new Coordinates(spectralLocus.Last().Chromaticity.U, spectralLocus.Last().Chromaticity.V);
        var lineOfPurples = plot.Add.Line(firstCoordinate, lastCoordinate);
        lineOfPurples.Color = new Color(255, 0, 255);
        lineOfPurples.LineWidth = 1;
        lineOfPurples.MarkerStyle = MarkerStyle.None;
    
        foreach (var colour in spectralLocus)
        {
            var chromaticity = colour.Chromaticity;
            var marker = new Marker
            {
                X = colour.Chromaticity.U, 
                Y = colour.Chromaticity.V,
                Shape = MarkerShape.FilledCircle,
                Color = Utils.GetPlotColour(chromaticity)!.Value, 
                Size = 5f
            };
        
            plot.Add.Plottable(marker);
        }
    
        plot.SavePng(Utils.GetOutputPath("diagram-uv-spectral-locus.png"), Width, Height);
    }
    
    public static void RgbTriangle()
    {
        var plot = Utils.GetEmptyPlot(RangeU, RangeV, majorTickInterval: 0.05);
    
        var fillMarkers = Utils.GetUvFillMarkers(RangeU, RangeV, increment: 0.00025);
        foreach (var marker in fillMarkers)
        {
            plot.Add.Plottable(marker);
        }
        
        AddSpectralOutline(plot);

        var rgbGamut = Utils.GetRgbGamut();
        var rgbCoordinates = rgbGamut.Select(colour => new Coordinates(colour.Chromaticity.U, colour.Chromaticity.V));
        var rgbPolygon = plot.Add.Polygon(rgbCoordinates.ToArray());
        rgbPolygon.LineStyle = new LineStyle { Color = new Color(0f, 0f, 0f, 0.25f), Width = 2.5f };
        rgbPolygon.FillStyle = new FillStyle { Color = Colors.Transparent };
    
        plot.SavePng(Utils.GetOutputPath("diagram-uv-chromaticity-rgb.png"), Width, Height);
    }
    
    public static void Blackbody()
    {
        var plot = Utils.GetEmptyPlot(RangeU, RangeV, majorTickInterval: 0.05);
    
        var fillMarkers = Utils.GetUvFillMarkers(RangeU, RangeV, increment: 0.00025);
        foreach (var marker in fillMarkers)
        {
            plot.Add.Plottable(marker);
        }

        AddSpectralOutline(plot);

        var blackbodyLocus = Utils.GetBlackbodyLocus();
        var blackbodyCoordinates = blackbodyLocus.Select(colour => new Coordinates(colour.Chromaticity.U, colour.Chromaticity.V)).ToList();
        var blackbodyScatter = plot.Add.Scatter(blackbodyCoordinates);
        blackbodyScatter.Color = Colors.Black;
        blackbodyScatter.LineWidth = 2.5f;
        blackbodyScatter.MarkerStyle = MarkerStyle.None;
    
        var isotherms = Utils.GetIsotherms();
        foreach (var (startColour, endColour) in isotherms)
        {
            var startCoordinates = new Coordinates(startColour.Chromaticity.U, startColour.Chromaticity.V);
            var endCoordinates = new Coordinates(endColour.Chromaticity.U, endColour.Chromaticity.V);
            var duvLine = plot.Add.Line(startCoordinates, endCoordinates);
            duvLine.Color = Colors.Black;
            duvLine.LineWidth = 2.5f;
        }
    
        plot.SavePng(Utils.GetOutputPath("diagram-uv-chromaticity-blackbody.png"), Width, Height);
    }

    public static void MacAdamLimits()
    {
        var plot = Utils.GetEmptyPlot(RangeU, RangeV, majorTickInterval: 0.05);
        
        AddSpectralOutline(plot);
    
        IEnumerable<Unicolour>[] macAdamLimits =
        [
            MacAdam.Limits10, MacAdam.Limits20, MacAdam.Limits30, MacAdam.Limits40, MacAdam.Limits50, 
            MacAdam.Limits60, MacAdam.Limits70, MacAdam.Limits80, MacAdam.Limits90, MacAdam.Limits95
        ];
        
        foreach (var limits in macAdamLimits)
        {
            var coordinates = limits.Select(colour => new Coordinates(colour.Chromaticity.U, colour.Chromaticity.V)).ToList();
            coordinates.Add(coordinates.First());
            var scatter = plot.Add.Scatter(coordinates);

            var luminance = limits.First().RelativeLuminance; // all colours in the group will have the same luminance
            var mapColour = Colourmaps.Flare.Map(1 - luminance).Rgb.Byte255;
            var color = new Color((byte)mapColour.R, (byte)mapColour.G, (byte)mapColour.B);
            scatter.LineStyle = new LineStyle { Color = color, Width = 2.5f };
            scatter.MarkerStyle = MarkerStyle.None;
        }

        plot.SavePng(Utils.GetOutputPath("diagram-uv-macadam-limits.png"), Width, Height);
    }
    
    private static void AddSpectralOutline(Plot plot)
    {
        var spectralLocus = Utils.GetSpectralLocus();
        var spectralCoordinates = spectralLocus.Select(colour => new Coordinates(colour.Chromaticity.U, colour.Chromaticity.V)).ToList();
        spectralCoordinates.Add(spectralCoordinates.First());
        var spectralLocusScatter = plot.Add.Scatter(spectralCoordinates);
        spectralLocusScatter.Color = Colors.Black;
        spectralLocusScatter.LineWidth = 2.5f;
        spectralLocusScatter.MarkerStyle = MarkerStyle.None;
    }
}