using System.Text.RegularExpressions;
using Wacton.Unicolour;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Experimental;
using Wacton.Unicolour.Icc;

const string repoReadme = "README.md";
const string wxyReadme = "docs/wxy-colour-space.md";

var sourceRoot = Path.GetFullPath("./docs");
var solutionRoot = AppDomain.CurrentDomain.BaseDirectory.Split("Unicolour.Readme")[0];
var docsRoot = Path.Combine(solutionRoot, "docs");

ProcessRepoReadme();
ProcessDocsReadme(Path.GetFullPath(repoReadme));
ProcessDocsReadme(Path.GetFullPath(wxyReadme));
CopyDirectory(sourceRoot, docsRoot);
return;

void ProcessRepoReadme()
{
    var text = File.ReadAllText(repoReadme);
    var textForRepo = text.Replace("../", string.Empty);
    
    File.WriteAllText(Path.Combine(solutionRoot, repoReadme), textForRepo);
}

void ProcessDocsReadme(string readmePath)
{
    var readmeFilename = Path.GetFileName(readmePath);
    var readmeAmericanPath = readmePath.Replace(".md", "_us.md");
    var readmeAmericanFilename = Path.GetFileName(readmeAmericanPath);
    
    var text = File.ReadAllText(readmePath);
    var textForDocs = text
        .Replace("docs/", string.Empty) // docs directory is flat
        .Replace("../", "https://github.com/waacton/Unicolour/tree/main/");
    
    // GitHub Pages currently doesn't support alerts or Mermaid 😑 - just remove them
    textForDocs = Regex.Replace(textForDocs, @"<details>(.|\n)*?<\/details>", string.Empty);
    textForDocs = textForDocs.Replace("> [!NOTE]", string.Empty);
    textForDocs = textForDocs.Replace("> [!TIP]", string.Empty);

    var ukText = textForDocs;
    ukText += Environment.NewLine;
    ukText += $"Also available in [American]({readmeAmericanFilename}) \ud83c\uddfa\ud83c\uddf8.";
    File.WriteAllText(Path.Combine(sourceRoot, Path.GetFileName(readmePath)), ukText);
    
    // substitute parts of the readme text that shouldn't change
    var guidSubstitutes = new (string original, string guid)[]
    {
        ("https://unicolour.wacton.xyz/wxy-colour-space", Guid.NewGuid().ToString()),
        ("https://play.unity.com/en/games/6826f61f-3806-4155-b824-7866b1edaed7/3d-colour-space-visualisation-unicolour-demo", Guid.NewGuid().ToString()),
        ("Unicolour", Guid.NewGuid().ToString()),
        ("unicolour", Guid.NewGuid().ToString()),
        ("ColourSpace", Guid.NewGuid().ToString())
    };

    var guidText = guidSubstitutes.Aggregate(textForDocs, (current, substitute) => current.Replace(substitute.original, substitute.guid));

    // could use regex but why bother?
    var usText = guidText
        .Replace("Colour ", "Color ")
        .Replace("Colours ", "Colors ")
        .Replace("Colour&", "Color&")
        .Replace("colour&", "color&")
        .Replace(" colour", " color")
        .Replace("colour.", "color.")
        .Replace("-colour", "-color")
        .Replace("colourful", "colorful")
        .Replace(" grey ", " gray ")
        .Replace(" grey.", " gray.")
        // .Replace("ise ", "ize ")
        .Replace("ised ", "ized ")
        .Replace("ises ", "izes ")
        .Replace("ising ", "izing ")
        .Replace("isation ", "ization ")
        .Replace("isations ", "izations ")
        .Replace("metre", "meter");
    
    usText = guidSubstitutes.Aggregate(usText, (current, substitute) => current.Replace(substitute.guid, substitute.original));
    usText += Environment.NewLine;
    usText += $"Also available in [British]({readmeFilename}) \ud83c\uddec\ud83c\udde7.";
    File.WriteAllText(Path.Combine(sourceRoot, Path.GetFileName(readmeAmericanPath)), usText);
}

void CopyDirectory(string sourcePath, string targetPath)
{
    Directory.CreateDirectory(targetPath);

    var sourceFiles = Directory.GetFiles(sourcePath);
    foreach (var sourceFile in sourceFiles)
    {
        var fileName = Path.GetFileName(sourceFile);
        var targetFile = Path.Combine(targetPath, fileName);
        File.Copy(sourceFile, targetFile, overwrite: true);
    }

    var sourceDirectories = Directory.GetDirectories(sourcePath);
    foreach (var sourceDirectory in sourceDirectories)
    {
        var directoryName = Path.GetFileName(sourceDirectory);
        var targetSubDirectory = Path.Combine(targetPath, directoryName);
        CopyDirectory(sourceDirectory, targetSubDirectory);
    }
}

/*
 --------------------------------------------------
 The following are examples used in the readme
 --------------------------------------------------
 */

#pragma warning disable CS8321 // Local function is declared but never used
// ReSharper disable UnusedVariable

void Overview()
{
    Unicolour pink = new("#FF1493");
    Console.WriteLine(pink.Oklab); // 0.65 +0.26 -0.01
}

void Installation()
{
    Unicolour colour = new(ColourSpace.Rgb255, 192, 255, 238);
}

void Quickstart()
{
    var cyan = new Unicolour("#00FFFF");
    Console.WriteLine(cyan.Hsl); // 180.0° 100.0% 50.0%

    var yellow = new Unicolour(ColourSpace.Rgb255, 255, 255, 0);
    Console.WriteLine(yellow.Hex); // #FFFF00
    
    var red = new Unicolour(ColourSpace.Rgb, 1.0, 0.0, 0.0);
    var blue = new Unicolour(ColourSpace.Hsb, 240, 1.0, 1.0);

    /* RGB: [1, 0, 0] ⟶ [0, 0, 1] = [0.5, 0, 0.5] */
    var purple = red.Mix(blue, ColourSpace.Rgb);
    Console.WriteLine(purple.Rgb); // 0.50 0.00 0.50
    Console.WriteLine(purple.Hex); // #800080

    /* HSL: [0, 1, 0.5] ⟶ [240, 1, 0.5] = [300, 1, 0.5] */
    var magenta = red.Mix(blue, ColourSpace.Hsl); 
    Console.WriteLine(magenta.Rgb); // 1.00 0.00 1.00
    Console.WriteLine(magenta.Hex); // #FF00FF
    
    // #FF0000, #FF0080, #FF00FF, #8000FF, #0000FF
    var palette = red.Palette(blue, ColourSpace.Hsl, 5);
    Console.WriteLine(palette.Select(colour => colour.Hex));

    var white = new Unicolour(ColourSpace.Oklab, 1.0, 0.0, 0.0);
    var black = new Unicolour(ColourSpace.Oklab, 0.0, 0.0, 0.0);
    var difference = white.Difference(black, DeltaE.Ciede2000);
    Console.WriteLine(difference); // 100.0000

    var equalEnergy = new Unicolour(ColourSpace.Xyz, 0.5, 0.5, 0.5);
    Console.WriteLine(equalEnergy.RelativeLuminance);  // 0.5
    Console.WriteLine(equalEnergy.Chromaticity.Xy);    // (0.3333, 0.3333)
    Console.WriteLine(equalEnergy.Chromaticity.Uv);    // (0.2105, 0.3158)
    Console.WriteLine(equalEnergy.Temperature);        // 5455.5 K (Δuv -0.00442)
    Console.WriteLine(equalEnergy.DominantWavelength); // 596.1
}

void FeatureConvert()
{
    Unicolour colour = new(ColourSpace.Rgb255, 192, 255, 238);
    var (l, c, h) = colour.Oklch;
    
    void Tip1()
    {
        Unicolour pink = new("ff1493");
        var hex = pink.Hex; // #FF1493
    }

    void Tip2()
    {
        Unicolour pink = new(ColourSpace.Munsell, Hue.FromMunsell(6.1, "RP"), 5.5, 19.5);
        Console.WriteLine(pink.Munsell); // 6.1RP 5.5/19.5
    }
}

void FeatureMix()
{
    var red = new Unicolour(ColourSpace.Rgb, 1.0, 0.0, 0.0, alpha: 1.0);
    var blue = new Unicolour(ColourSpace.Hsb, 240, 1.0, 1.0, alpha: 1.0);
    var magenta = red.Mix(blue, ColourSpace.Hsl, 0.5, HueSpan.Decreasing); 
    var green = red.Mix(blue, ColourSpace.Hsl, 0.5, HueSpan.Increasing);
    var palette = red.Palette(blue, ColourSpace.Hsl, 10, HueSpan.Longer);
}

void FeatureBlend()
{
    var blue = new Unicolour(ColourSpace.Hsb, 240, 1.0, 1.0, alpha: 0.5);
    var red = new Unicolour(ColourSpace.Rgb, 1.0, 0.0, 0.0);
    var purple = blue.Blend(red, BlendMode.Normal);
    var pink = blue.Blend(red, BlendMode.Screen);
}

void FeatureCompare()
{
    var red = new Unicolour(ColourSpace.Rgb, 1.0, 0.0, 0.0);
    var blue = new Unicolour(ColourSpace.Hsb, 240, 1.0, 1.0);
    var contrast = red.Contrast(blue);
    var difference = red.Difference(blue, DeltaE.Cie76);
}

void FeatureGamutMap()
{
    var veryRed = new Unicolour(ColourSpace.Rgb, 1.25, -0.39, -0.14);

    var isDisplayable = veryRed.IsInRgbGamut;
    var displayRed = veryRed.MapToRgbGamut();

    var isEmpiricalSurface = veryRed.IsInPointerGamut;
    var empiricalRed = veryRed.MapToPointerGamut();

    var isTheoreticalSurface = veryRed.IsInMacAdamLimits;
    var theoreticalRed = veryRed.MapToMacAdamLimits();
}

void FeatureCvd()
{
    var colour = new Unicolour(ColourSpace.Rgb255, 192, 255, 238);
    var missingRed = colour.Simulate(Cvd.Protanopia);
    var defectiveRed = colour.Simulate(Cvd.Protanomaly, 0.5);
}

void FeatureColourimetric()
{
    var grey = new Unicolour(ColourSpace.RgbLinear, 0.5, 0.5, 0.5);
    var chromaticity = grey.Chromaticity;
    var luminance = grey.RelativeLuminance;

    var white = new Unicolour(chromaticity, luminance: 1.0);
}

void FeatureTemperature()
{
    var chromaticity = new Chromaticity(0.3457, 0.3585);
    var d50 = new Unicolour(chromaticity);
    var (cct, duv) = d50.Temperature;

    var temperature = new Temperature(6504, 0.0032);
    var d65 = new Unicolour(temperature);
    var (x, y) = d65.Chromaticity;
}

void FeatureWavelength()
{
    var chromaticity = new Chromaticity(0.1, 0.8);
    var hyperGreen = new Unicolour(chromaticity);
    var dominantWavelength = hyperGreen.DominantWavelength;
    var excitationPurity = hyperGreen.ExcitationPurity;
    
    var laserRed = new Unicolour(ColourSpace.Wxy, 670, 1.0, 0.5);
}

void FeatureImaginary()
{
    var chromaticity = new Chromaticity(0.05, 0.05);
    var impossibleBlue = new Unicolour(chromaticity);
    var isImaginary = impossibleBlue.IsImaginary;
}

void FeatureSpd()
{
    /* [575 nm] -> 0.5 · [580 nm] -> 1.0 · [585 nm] -> 0.5 */
    var spd = new Spd(start: 575, interval: 5, coefficients: [0.5, 1.0, 0.5]);
    var intenseYellow = new Unicolour(spd);
}

void FeaturePigment()
{
    /* populate k and s with measurement data */
    var phthaloBlue = new Pigment(startWavelength: 380, wavelengthInterval: 10, k: [], s: []);
    var hansaYellow = new Pigment(startWavelength: 380, wavelengthInterval: 10, k: [], s: []);
    var green = new Unicolour(pigments: [phthaloBlue, hansaYellow], weights: [0.5, 0.5]);
}

void FeatureIcc()
{
    var fogra39 = new IccConfiguration("./Fogra39.icc", Intent.RelativeColorimetric);
    var config = new Configuration(iccConfig: fogra39);
    
    var navyRgb = new Unicolour(config, ColourSpace.Rgb255, 0, 0, 128);
    Console.WriteLine(navyRgb.Icc); // 1.0000 0.8977 0.0001 0.2867 CMYK

    var navyCmyk = new Unicolour(config, new Channels(1.0, 1.0, 0.0, 0.5));
    Console.WriteLine(navyCmyk.Rgb.Byte255); // 46 37 87
}

void FeatureInvalid()
{
    var bad1 = new Unicolour(ColourSpace.Oklab, double.NegativeInfinity, double.NaN, double.Epsilon);
    var bad2 = new Unicolour(ColourSpace.Cam16, double.NaN, double.MaxValue, double.MinValue);
    var bad3 = bad1.Mix(bad2, ColourSpace.Hct, amount: double.PositiveInfinity);
}

void FeatureDefaults()
{
    var defaultConfig = new Configuration(RgbConfiguration.StandardRgb, XyzConfiguration.D65);
    var colour = new Unicolour(defaultConfig, ColourSpace.Rgb255, 192, 255, 238);
}

void ConfigPredefined()
{
    Configuration config = new(RgbConfiguration.Rec2020, XyzConfiguration.D50);
    Unicolour colour = new(config, ColourSpace.Rgb255, 204, 64, 132);
}

void ConfigManual()
{
    var rgbConfig = new RgbConfiguration(
        chromaticityR: new(0.7347, 0.2653),
        chromaticityG: new(0.1152, 0.8264),
        chromaticityB: new(0.1566, 0.0177),
        whitePoint: Illuminant.D50.GetWhitePoint(Observer.Degree2),
        fromLinear: value => Math.Pow(value, 1 / 2.19921875),
        toLinear: value => Math.Pow(value, 2.19921875)
    );

    var xyzConfig = new XyzConfiguration(Illuminant.C, Observer.Degree10, Adaptation.VonKries);

    var config = new Configuration(rgbConfig, xyzConfig);
    var colour = new Unicolour(config, ColourSpace.Rgb255, 202, 97, 143);
}

void ConfigConvert()
{
    /* pure sRGB green */
    var srgbConfig = new Configuration(RgbConfiguration.StandardRgb);
    var srgbColour = new Unicolour(srgbConfig, ColourSpace.Rgb, 0, 1, 0);                         
    Console.WriteLine(srgbColour.Rgb); // 0.00 1.00 0.00

    /* ⟶ Display P3 */
    var displayP3Config = new Configuration(RgbConfiguration.DisplayP3);
    var displayP3Colour = srgbColour.ConvertToConfiguration(displayP3Config); 
    Console.WriteLine(displayP3Colour.Rgb); // 0.46 0.99 0.30

    /* ⟶ Rec. 2020 */
    var rec2020Config = new Configuration(RgbConfiguration.Rec2020);
    var rec2020Colour = displayP3Colour.ConvertToConfiguration(rec2020Config);
    Console.WriteLine(rec2020Colour.Rgb); // 0.57 0.96 0.27
}

void Datasets()
{
    var pink = Css.DeepPink;
    var green = Xkcd.NastyGreen;
    var mapped = Colourmaps.Viridis.Map(0.5);
    var palette = Colourmaps.Turbo.Palette(10);
}

void ExperimentalPigmentGenerator()
{
    var redPigment = PigmentGenerator.From(new Unicolour("#FF0000"));
    var bluePigment = PigmentGenerator.From(new Unicolour("#0000FF"));
    var magenta = new Unicolour([redPigment, bluePigment], [0.5, 0.5]);
}

void ExperimentalSpectralJs()
{
    var blue = new Unicolour("#0000FF");
    var yellow = new Unicolour("#FFFF00");
    var green = SpectralJs.Mix([blue, yellow], [0.5, 0.5]);
    var palette = SpectralJs.Palette(blue, yellow, 9);
}