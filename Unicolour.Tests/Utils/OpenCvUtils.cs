namespace Wacton.Unicolour.Tests.Utils;

using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using OpenCvSharp;
using Wacton.Unicolour.Tests.Lookups;

// https://docs.opencv.org/4.5.5/de/d25/imgproc_color_conversions.html
internal static class OpenCvUtils
{
    public static TestColour FromRgb255(int r255, int g255, int b255) => FromRgb255(r255, g255, b255, $"{r255:000} {g255:000} {b255:000}");
    public static TestColour FromRgb255(int r255, int g255, int b255, string name)
    {
        var r = r255 / 255.0;
        var g = g255 / 255.0;
        var b = b255 / 255.0;
        return FromRgb(r, g, b, name);
    }
    
    public static TestColour FromRgb(double r, double g, double b) => FromRgb(r, g, b, $"{r:F2} {g:F2} {b:F2}");
    public static TestColour FromRgb(double r, double g, double b, string name)
    {
        var rLinear = Rgb.LinearCorrection(r);
        var gLinear = Rgb.LinearCorrection(g);
        var bLinear = Rgb.LinearCorrection(b);
        
        // it appears that OpenCV's RGB -> XYZ and RGB -> LAB conversions 
        // expect to receive RGB values that have already undergone linear correction...
        var rgb = GetInputVec((r, g, b));
        var hsb = GetConvertedVec((r, g, b), ColorConversionCodes.RGB2HSV);
        var xyz = GetConvertedVec((rLinear, gLinear, bLinear), ColorConversionCodes.RGB2XYZ);
        var lab = GetConvertedVec((rLinear, gLinear, bLinear), ColorConversionCodes.RGB2Lab);
        
        return new TestColour
        {
            Name = name,
            Rgb = (rgb.Item0, rgb.Item1, rgb.Item2),
            Hsb = (hsb.Item0, hsb.Item1, hsb.Item2),
            Xyz = (xyz.Item0, xyz.Item1, xyz.Item2),
            Lab = (lab.Item0, lab.Item1, lab.Item2)
        };
    }
    
    public static TestColour FromHsb(double h, double s, double b) => FromHsb(h, s, b, $"{h:F2} {s:F2} {b:F2}");
    public static TestColour FromHsb(double h, double s, double b, string name)
    {
        var rgb = GetConvertedVec((h, s, b), ColorConversionCodes.HSV2RGB);
        var hsb = GetInputVec((h, s, b));
        
        return new TestColour
        {
            Name = name,
            Rgb = (rgb.Item0, rgb.Item1, rgb.Item2),
            Hsb = (hsb.Item0, hsb.Item1, hsb.Item2),
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

    public static void GenerateOpenCvColoursFile()
    {
        var rows = new List<string>();
        foreach (var namedColour in TestColours.NamedColours)
        {
            var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
            var (r, g, b) = (systemColour.R, systemColour.G, systemColour.B);
            var testColour = FromRgb255(r, g, b, namedColour.Name!);

            string Stringify(double value) => value.ToString(CultureInfo.InvariantCulture);
            var row = new List<string>
            {
                testColour.Name!, 
                Stringify(testColour.Rgb.Value.r), Stringify(testColour.Rgb.Value.g), Stringify(testColour.Rgb.Value.b),
                Stringify(testColour.Hsb.Value.h), Stringify(testColour.Hsb.Value.s), Stringify(testColour.Hsb.Value.b),
                Stringify(testColour.Xyz.Value.x), Stringify(testColour.Xyz.Value.y), Stringify(testColour.Xyz.Value.z),
                Stringify(testColour.Lab.Value.l), Stringify(testColour.Lab.Value.a), Stringify(testColour.Lab.Value.b)
            };
            
            rows.Add(string.Join(", ", row));
        }
        
        File.WriteAllLines("OpenCvColours.csv", rows);
    }
}