namespace Wacton.Unicolour.Tests;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using Colourful;
using NUnit.Framework;
using OpenCvSharp;
using Wacton.Unicolour;
using Wacton.Unicolour.Tests.Lookups;
using ColorMineRgb = ColorMine.ColorSpaces.Rgb;
using ColorMineHsb = ColorMine.ColorSpaces.Hsb;
using ColorMineXyz = ColorMine.ColorSpaces.Xyz;
using ColorMineLab = ColorMine.ColorSpaces.Lab;

public class OtherLibraryTests
{
    private static bool IsWindows() => Environment.OSVersion.Platform == PlatformID.Win32NT;

    [Test]
    // https://docs.opencv.org/4.5.5/de/d25/imgproc_color_conversions.html
    // at this point I'm pretty sure OpenCV doesn't calculate RGB -> LAB correctly
    // since all other libraries and online tools calculate the same LAB as Unicolour
    // the LAB test tolerances are so large is not really worth testing against
    public void OpenCvWindows()
    {
        Assume.That(IsWindows()); // I've given up trying to make OpenCvSharp work in a dockerised unix environment...
        foreach (var namedColour in TestColours.NamedColours)
        {
            var (r, g, b) = HexToRgb255(namedColour.Hex!);
            var unicolour = Unicolour.FromRgb(r, g, b);
            var testColour = ToOpenCvTestColour(namedColour.Name!, r, g, b);
            var tolerances = new Tolerances { Rgb = 0.0005, Hsb = 0.0005, Xyz = 0.0005, Lab = 50.0 };
            AssertTestColour(unicolour, testColour, tolerances);
        }
    }
    
    [Test]
    // in order to test OpenCV in a non-windows environment, this compares against pre-computed values
    public void OpenCvCrossPlatform()
    {
        var openCvColours = TestColours.OpenCvColours;
        foreach (var namedColour in TestColours.NamedColours)
        {
            var (r, g, b) = HexToRgb255(namedColour.Hex!);
            var unicolour = Unicolour.FromRgb(r, g, b);
            var testColour = openCvColours.Single(x => x.Name == namedColour.Name);
            var tolerances = new Tolerances { Rgb = 0.0005, Hsb = 0.0005, Xyz = 0.0005, Lab = 50.0 };
            AssertTestColour(unicolour, testColour, tolerances);
        }
    }

    [Test]
    // Colourful does not support HSB...
    public void Colourful()
    {
        foreach (var namedColour in TestColours.NamedColours)
        {
            var (r, g, b) = HexToRgb255(namedColour.Hex!);
            var unicolour = Unicolour.FromRgb(r, g, b);
            var testColour = ToColourfulTestColour(namedColour.Name!, r, g, b);
            var tolerances = new Tolerances { Rgb = 0.00000000001, RgbLinear = 0.00000000001, Xyz = 0.0005, Lab = 0.05 };
            AssertTestColour(unicolour, testColour, tolerances);
        }
    }
    
    [Test]
    public void ColorMine()
    {
        foreach (var namedColour in TestColours.NamedColours)
        {
            var (r, g, b) = HexToRgb255(namedColour.Hex!);
            var unicolour = Unicolour.FromRgb(r, g, b);
            var testColour = ToColorMineTestColour(namedColour.Name!, r, g, b);
            var tolerances = new Tolerances { Rgb = 0.00000000001, Hsb = 0.0005, Xyz = 0.0005, Lab = 0.0005 };
            AssertTestColour(unicolour, testColour, tolerances);
        }
    }

    private void AssertTestColour(Unicolour unicolour, TestColour testColour, Tolerances tolerances)
    {
        string FailMessage() => $"colour:{testColour.Name}";

        if (tolerances.Rgb.HasValue)
        {
            Assert.That(unicolour.Rgb.R, Is.EqualTo(testColour.Rgb.r).Within(tolerances.Rgb), FailMessage);
            Assert.That(unicolour.Rgb.G, Is.EqualTo(testColour.Rgb.g).Within(tolerances.Rgb), FailMessage);
            Assert.That(unicolour.Rgb.B, Is.EqualTo(testColour.Rgb.b).Within(tolerances.Rgb), FailMessage);
        }
        
        if (tolerances.RgbLinear.HasValue)
        {
            Assert.That(unicolour.Rgb.RLinear, Is.EqualTo(testColour.RgbLinear.r).Within(tolerances.RgbLinear), FailMessage);
            Assert.That(unicolour.Rgb.GLinear, Is.EqualTo(testColour.RgbLinear.g).Within(tolerances.RgbLinear), FailMessage);
            Assert.That(unicolour.Rgb.BLinear, Is.EqualTo(testColour.RgbLinear.b).Within(tolerances.RgbLinear), FailMessage);
        }
        
        if (tolerances.Hsb.HasValue)
        {
            Assert.That(unicolour.Hsb.H, Is.EqualTo(testColour.Hsb.h).Within(tolerances.Hsb), FailMessage);
            Assert.That(unicolour.Hsb.S, Is.EqualTo(testColour.Hsb.s).Within(tolerances.Hsb), FailMessage);
            Assert.That(unicolour.Hsb.B, Is.EqualTo(testColour.Hsb.b).Within(tolerances.Hsb), FailMessage);
        }
        
        if (tolerances.Xyz.HasValue)
        {
            Assert.That(unicolour.Xyz.X, Is.EqualTo(testColour.Xyz.x).Within(tolerances.Xyz), FailMessage);
            Assert.That(unicolour.Xyz.Y, Is.EqualTo(testColour.Xyz.y).Within(tolerances.Xyz), FailMessage);
            Assert.That(unicolour.Xyz.Z, Is.EqualTo(testColour.Xyz.z).Within(tolerances.Xyz), FailMessage);
        }
        
        if (tolerances.Lab.HasValue)
        {
            Assert.That(unicolour.Lab.L, Is.EqualTo(testColour.Lab.l).Within(tolerances.Lab), FailMessage);
            Assert.That(unicolour.Lab.A, Is.EqualTo(testColour.Lab.a).Within(tolerances.Lab), FailMessage);
            Assert.That(unicolour.Lab.B, Is.EqualTo(testColour.Lab.b).Within(tolerances.Lab), FailMessage);
        }
    }
    
    private static (byte r, byte g, byte b) HexToRgb255(string hex)
    {
        var systemColour = ColorTranslator.FromHtml(hex);
        return (systemColour.R, systemColour.G, systemColour.B);
    }

    private static TestColour ToOpenCvTestColour(string name, double r255, double g255, double b255)
    {
        var r = r255 / 255.0;
        var g = g255 / 255.0;
        var b = b255 / 255.0;
        var rLinear = Rgb.LinearCorrection(r);
        var gLinear = Rgb.LinearCorrection(g);
        var bLinear = Rgb.LinearCorrection(b);
        
        // it appears that OpenCV's RGB -> XYZ and RGB -> LAB conversions 
        // expect to receive RGB values that have already undergone linear correction...
        var rgb = GetRgbVec(r, g, b);
        var hsb = GetHsbVec(r, g, b);
        var xyz = GetXyzVec(rLinear, gLinear, bLinear);
        var lab = GetLabVec(rLinear, gLinear, bLinear);
        
        return new TestColour
        {
            Name = name,
            Rgb = (rgb.Item0, rgb.Item1, rgb.Item2),
            Hsb = (hsb.Item0, hsb.Item1, hsb.Item2),
            Xyz = (xyz.Item0, xyz.Item1, xyz.Item2),
            Lab = (lab.Item0, lab.Item1, lab.Item2)
        };
    }
    
    private static TestColour ToColourfulTestColour(string name, double r255, double g255, double b255)
    {
        var rgb = new RGBColor(r255 / 255.0, g255 / 255.0, b255 / 255.0);

        var rgbLinearConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLinearRGB().Build();
        var rgbLinear = rgbLinearConverter.Convert(rgb);

        var xyzConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToXYZ(Illuminants.D65).Build();
        var xyz = xyzConverter.Convert(rgb);

        var labConverter = new ConverterBuilder().FromRGB(RGBWorkingSpaces.sRGB).ToLab(Illuminants.D65).Build();
        var lab = labConverter.Convert(rgb);
        
        return new TestColour
        {
            Name = name,
            Rgb = (rgb.R, rgb.G, rgb.B),
            RgbLinear = (rgbLinear.R, rgbLinear.G, rgbLinear.B),
            Xyz = (xyz.X, xyz.Y, xyz.Z),
            Lab = (lab.L, lab.a, lab.b)
        };
    }
    
    private static TestColour ToColorMineTestColour(string name, double r255, double g255, double b255)
    {
        var rgb = new ColorMineRgb {R = r255, G = g255, B = b255};
        var hsb = rgb.To<ColorMineHsb>();
        var xyz = rgb.To<ColorMineXyz>();
        var lab = rgb.To<ColorMineLab>();
        
        return new TestColour
        {
            Name = name,
            Rgb = (rgb.R / 255.0, rgb.G / 255.0, rgb.B / 255.0),
            Hsb = (hsb.H, hsb.S, hsb.B),
            Xyz = (xyz.X / 100.0, xyz.Y / 100.0, xyz.Z / 100.0),
            Lab = (lab.L, lab.A, lab.B)
        };
    }

    private static Vec3f GetRgbVec(double r, double g, double b)
    {
        var matRgb = new Mat(1, 1, MatType.CV_32FC3, new Scalar(r, g, b));
        return matRgb.Get<Vec3f>(0, 0);
    }
    
    private static Vec3f GetHsbVec(double r, double g, double b)
    {
        var matRgb = new Mat(1, 1, MatType.CV_32FC3, new Scalar(r, g, b));
        var matHsv = new Mat(1, 1, MatType.CV_32FC3);
        Cv2.CvtColor(matRgb, matHsv, ColorConversionCodes.RGB2HSV);
        return matHsv.Get<Vec3f>(0, 0);
    }
    
    private static Vec3f GetXyzVec(double r, double g, double b)
    {
        var matRgb = new Mat(1, 1, MatType.CV_32FC3, new Scalar(r, g, b));
        var matXyz = new Mat(1, 1, MatType.CV_32FC3);
        Cv2.CvtColor(matRgb, matXyz, ColorConversionCodes.RGB2XYZ);
        return matXyz.Get<Vec3f>(0, 0);
    }
    
    private static Vec3f GetLabVec(double r, double g, double b)
    {
        var matRgb = new Mat(1, 1, MatType.CV_32FC3, new Scalar(r, g, b));
        var matLab = new Mat(1, 1, MatType.CV_32FC3);
        Cv2.CvtColor(matRgb, matLab, ColorConversionCodes.RGB2Lab);
        return matLab.Get<Vec3f>(0, 0);
    }
    
    private static void GenerateOpenCvColoursFile()
    {
        var rows = new List<string>();
        foreach (var namedColour in TestColours.NamedColours)
        {
            var systemColour = ColorTranslator.FromHtml(namedColour.Hex!);
            var (r, g, b) = (systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0);
            var testColour = ToOpenCvTestColour(namedColour.Name!, r, g, b);

            string Stringify(double value) => value.ToString(CultureInfo.InvariantCulture);
            var row = new List<string>
            {
                testColour.Name!, 
                Stringify(testColour.Rgb.r), Stringify(testColour.Rgb.g), Stringify(testColour.Rgb.b),
                Stringify(testColour.Hsb.h), Stringify(testColour.Hsb.s), Stringify(testColour.Hsb.b),
                Stringify(testColour.Xyz.x), Stringify(testColour.Xyz.y), Stringify(testColour.Xyz.z),
                Stringify(testColour.Lab.l), Stringify(testColour.Lab.a), Stringify(testColour.Lab.b)
            };
            
            rows.Add(string.Join(", ", row));
        }
        
        File.WriteAllLines(Path.Combine("Lookups", "OpenCvColours.csv"), rows);
    }

    private class Tolerances {
        public double? Rgb, RgbLinear, Hsb, Xyz, Lab;
    }
}