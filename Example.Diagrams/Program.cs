using ScottPlot;
using ScottPlot.Plottables;
using Wacton.Unicolour.Example.Diagrams;

const string outputDirectory = "../../../../Unicolour.Readme/docs/";
const int height = 640;

/*
 * note: chromaticity diagrams that are filled with colour take a couple of minutes to generate each
 * because they process 100,000s of colours, checking if imaginary and converting to RGB
 * could easily be much faster if not trying to make a solid block of colour
 */

var spectralLocus = Utils.GetSpectralLocus();
var rgbGamut = Utils.GetRgbGamut();
var blackbodyLocus = Utils.GetBlackbodyLocus();
var isotherms = Utils.GetIsotherms();

SpectralLocus();
XyChromaticityWithRgb();
XyChromaticityWithBlackbody();
UvChromaticity();
UvChromaticityWithBlackbody();
return;

void SpectralLocus()
{
    var rangeX = (0.0, 0.8);
    var rangeY = (0.0, 0.9);
    var plot = Utils.GetEmptyPlot(rangeX, rangeY, majorTickInterval: 0.1);
    plot.DataBackground = new BackgroundStyle { Color = Color.Gray(96) };
    plot.Grid.MajorLineColor = Color.Gray(128);
    
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
    
    var width = Utils.GetWidth(rangeX, rangeY, height);
    plot.SavePng(Path.Combine(outputDirectory, "diagram-spectral-locus.png"), width, height);
}

void XyChromaticityWithRgb()
{
    var rangeX = (0.0, 0.8);
    var rangeY = (0.0, 0.9);
    var plot = Utils.GetEmptyPlot(rangeX, rangeY, majorTickInterval: 0.1);
    
    var fillMarkers = Utils.GetXyFillMarkers(rangeX, rangeY, increment: 0.001);
    foreach (var marker in fillMarkers)
    {
        plot.Add.Plottable(marker);
    }

    var spectralCoordinates = spectralLocus.Select(colour => new Coordinates(colour.Chromaticity.X, colour.Chromaticity.Y)).ToList();
    spectralCoordinates.Add(spectralCoordinates.First());
    var spectralLocusScatter = plot.Add.Scatter(spectralCoordinates);
    spectralLocusScatter.Color = Colors.Black;
    spectralLocusScatter.LineWidth = 2.5f;
    spectralLocusScatter.MarkerStyle = MarkerStyle.None;

    var rgbCoordinates = rgbGamut.Select(colour => new Coordinates(colour.Chromaticity.X, colour.Chromaticity.Y));
    var rgbPolygon = plot.Add.Polygon(rgbCoordinates.ToArray());
    rgbPolygon.LineStyle = new LineStyle { Color = new Color(0f, 0f, 0f, 0.25f), Width = 2.5f };
    rgbPolygon.FillStyle = new FillStyle { Color = Colors.Transparent };
    
    var width = Utils.GetWidth(rangeX, rangeY, height);
    plot.SavePng(Path.Combine(outputDirectory, "diagram-xy-chromaticity-rgb.png"), width, height);
}

void XyChromaticityWithBlackbody()
{
    var rangeX = (0.0, 0.8);
    var rangeY = (0.0, 0.9);
    var plot = Utils.GetEmptyPlot(rangeX, rangeY, majorTickInterval: 0.1);
    
    var fillMarkers = Utils.GetXyFillMarkers(rangeX, rangeY, increment: 0.001);
    foreach (var marker in fillMarkers)
    {
        plot.Add.Plottable(marker);
    }

    var spectralCoordinates = spectralLocus.Select(colour => new Coordinates(colour.Chromaticity.X, colour.Chromaticity.Y)).ToList();
    spectralCoordinates.Add(spectralCoordinates.First());
    var spectralLocusScatter = plot.Add.Scatter(spectralCoordinates);
    spectralLocusScatter.Color = Colors.Black;
    spectralLocusScatter.LineWidth = 2.5f;
    spectralLocusScatter.MarkerStyle = MarkerStyle.None;

    var blackbodyCoordinates = blackbodyLocus.Select(colour => new Coordinates(colour.Chromaticity.X, colour.Chromaticity.Y)).ToList();
    var blackbodyScatter = plot.Add.Scatter(blackbodyCoordinates);
    blackbodyScatter.Color = Colors.Black;
    blackbodyScatter.LineWidth = 2.5f;
    blackbodyScatter.MarkerStyle = MarkerStyle.None;

    foreach (var (startColour, endColour) in isotherms)
    {
        var startCoordinates = new Coordinates(startColour.Chromaticity.X, startColour.Chromaticity.Y);
        var endCoordinates = new Coordinates(endColour.Chromaticity.X, endColour.Chromaticity.Y);
        var duvLine = plot.Add.Line(startCoordinates, endCoordinates);
        duvLine.Color = Colors.Black;
        duvLine.LineWidth = 2.5f;
    }

    var width = Utils.GetWidth(rangeX, rangeY, height);
    plot.SavePng(Path.Combine(outputDirectory, "diagram-xy-chromaticity-blackbody.png"), width, height);
}

void UvChromaticity()
{
    var rangeU = (0.0, 0.7);
    var rangeV = (0.0, 0.4);
    var plot = Utils.GetEmptyPlot(rangeU, rangeV, majorTickInterval: 0.05);
    
    var fillMarkers = Utils.GetUvFillMarkers(rangeU, rangeV, increment: 0.00025);
    foreach (var marker in fillMarkers)
    {
        plot.Add.Plottable(marker);
    }

    var spectralCoordinates = spectralLocus.Select(colour => new Coordinates(colour.Chromaticity.U, colour.Chromaticity.V)).ToList();
    spectralCoordinates.Add(spectralCoordinates.First());
    var spectralLocusScatter = plot.Add.Scatter(spectralCoordinates);
    spectralLocusScatter.Color = Colors.Black;
    spectralLocusScatter.LineWidth = 2.5f;
    spectralLocusScatter.MarkerStyle = MarkerStyle.None;
    
    var width = Utils.GetWidth(rangeU, rangeV, height);
    plot.SavePng(Path.Combine(outputDirectory, "diagram-uv-chromaticity.png"), width, height);
}

void UvChromaticityWithBlackbody()
{
    var rangeU = (0.1, 0.45);
    var rangeV = (0.25, 0.4);
    var plot = Utils.GetEmptyPlot(rangeU, rangeV, majorTickInterval: 0.05);
    
    var fillMarkers = Utils.GetUvFillMarkers(rangeU, rangeV, increment: 0.00025);
    foreach (var marker in fillMarkers)
    {
        plot.Add.Plottable(marker);
    }

    var spectralCoordinates = spectralLocus.Select(colour => new Coordinates(colour.Chromaticity.U, colour.Chromaticity.V)).ToList();
    spectralCoordinates.Add(spectralCoordinates.First());
    var spectralLocusScatter = plot.Add.Scatter(spectralCoordinates);
    spectralLocusScatter.Color = Colors.Black;
    spectralLocusScatter.LineWidth = 2.5f;
    spectralLocusScatter.MarkerStyle = MarkerStyle.None;

    var blackbodyCoordinates = blackbodyLocus.Select(colour => new Coordinates(colour.Chromaticity.U, colour.Chromaticity.V)).ToList();
    var blackbodyScatter = plot.Add.Scatter(blackbodyCoordinates);
    blackbodyScatter.Color = Colors.Black;
    blackbodyScatter.LineWidth = 2.5f;
    blackbodyScatter.MarkerStyle = MarkerStyle.None;

    foreach (var (startColour, endColour) in isotherms)
    {
        var startCoordinates = new Coordinates(startColour.Chromaticity.U, startColour.Chromaticity.V);
        var endCoordinates = new Coordinates(endColour.Chromaticity.U, endColour.Chromaticity.V);
        var duvLine = plot.Add.Line(startCoordinates, endCoordinates);
        duvLine.Color = Colors.Black;
        duvLine.LineWidth = 2.5f;
    }
    
    var width = Utils.GetWidth(rangeU, rangeV, height);
    plot.SavePng(Path.Combine(outputDirectory, "diagram-uv-chromaticity-blackbody.png"), width, height);
}