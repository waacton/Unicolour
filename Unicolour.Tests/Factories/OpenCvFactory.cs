namespace Wacton.Unicolour.Tests.Factories;

using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using OpenCvSharp;
using Wacton.Unicolour.Tests.Utils;

internal class OpenCvFactory : ITestColourFactory
{
    /*
     * OpenCV doesn't expose linear RGB
     * -----
     * at this point I'm pretty sure OpenCV doesn't calculate RGB -> LAB correctly
     * since all other libraries and online tools calculate the same LAB as Unicolour
     * the LAB test tolerances are so large it's only worth testing against for regression purposes
     */
    public static readonly Tolerances Tolerances = new() {Rgb = 0.005, Hsb = 0.00005, Hsl = 0.00005, Xyz = 0.0005, Lab = 50.0};
    
    public TestColour FromRgb(double r, double g, double b, string name)
    {
        var rLinear = Companding.InverseStandardRgb(r);
        var gLinear = Companding.InverseStandardRgb(g);
        var bLinear = Companding.InverseStandardRgb(b);
        
        // it appears that OpenCV's RGB -> XYZ and RGB -> LAB conversions 
        // expect to receive RGB values that have already undergone linear correction...
        var rgb = GetInputVec((r, g, b));
        var hsb = GetConvertedVec((r, g, b), ColorConversionCodes.RGB2HSV_FULL);
        var hls = GetConvertedVec((r, g, b), ColorConversionCodes.RGB2HLS_FULL);
        var xyz = GetConvertedVec((rLinear, gLinear, bLinear), ColorConversionCodes.RGB2XYZ);
        var lab = GetConvertedVec((rLinear, gLinear, bLinear), ColorConversionCodes.RGB2Lab);
        
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.Item0, rgb.Item1, rgb.Item2),
            Hsb = new(hsb.Item0, hsb.Item1, hsb.Item2),
            Hsl = new(hls.Item0, hls.Item2, hls.Item1),
            Xyz = new(xyz.Item0, xyz.Item1, xyz.Item2),
            Lab = new(lab.Item0, lab.Item1, lab.Item2),
            Tolerances = Tolerances
        };
    }

    // OpenCV can only convert to RGB from HSB
    public TestColour FromHsb(double h, double s, double b, string name)
    {
        var rgb = GetConvertedVec((h, s, b), ColorConversionCodes.HSV2RGB);
        var hsb = GetInputVec((h, s, b));
        
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.Item0, rgb.Item1, rgb.Item2),
            Hsb = new(hsb.Item0, hsb.Item1, hsb.Item2),
            Tolerances = Tolerances
        };
    }

    // OpenCV can only convert to RGB from HSL
    public TestColour FromHsl(double h, double s, double l, string name)
    {
        var rgb = GetConvertedVec((h, l, s), ColorConversionCodes.HLS2RGB);
        var hsl = GetInputVec((h, s, l));
        
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.Item0, rgb.Item1, rgb.Item2),
            Hsl = new(hsl.Item0, hsl.Item1, hsl.Item2),
            Tolerances = Tolerances
        };
    }

    private static Vec3f GetInputVec((double, double, double) input)
    {
        var (i1, i2, i3) = input;
        var mat = new Mat(1, 1, MatType.CV_32FC3, new Scalar(i1, i2, i3));
        return mat.Get<Vec3f>(0, 0);
    }
    
    private static Vec3f GetConvertedVec((double, double, double) input, ColorConversionCodes conversionCode)
    {
        var (i1, i2, i3) = input;
        var matIn = new Mat(1, 1, MatType.CV_32FC3, new Scalar(i1, i2, i3));
        var matOut = new Mat(1, 1, MatType.CV_32FC3);
        Cv2.CvtColor(matIn, matOut, conversionCode);
        return matOut.Get<Vec3f>(0, 0);
    }

    public void GenerateCsvFile()
    {
        var rows = new List<string>();
        foreach (var namedColour in TestColours.NamedColours)
        {
            var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
            var (r255, g255, b255) = (systemColour.R, systemColour.G, systemColour.B);
            var r = r255 / 255.0;
            var g = g255 / 255.0;
            var b = b255 / 255.0;
            var testColour = FromRgb(r, g, b, namedColour.Name!);

            string Stringify(double value) => value.ToString(CultureInfo.InvariantCulture);
            var row = new List<string>
            {
                testColour.Name!, 
                Stringify(testColour.Rgb!.First), Stringify(testColour.Rgb.Second), Stringify(testColour.Rgb.Third),
                Stringify(testColour.Hsb!.First), Stringify(testColour.Hsb.Second), Stringify(testColour.Hsb.Third),
                Stringify(testColour.Hsl!.First), Stringify(testColour.Hsl.Second), Stringify(testColour.Hsl.Third),
                Stringify(testColour.Xyz!.First), Stringify(testColour.Xyz.Second), Stringify(testColour.Xyz.Third),
                Stringify(testColour.Lab!.First), Stringify(testColour.Lab.Second), Stringify(testColour.Lab.Third)
            };
            
            rows.Add(string.Join(", ", row));
        }
        
        File.WriteAllLines("OpenCvColours.csv", rows);
    }
}