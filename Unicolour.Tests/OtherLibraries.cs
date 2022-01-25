namespace Wacton.Unicolour.Tests;

using System.Drawing;
using Colourful;
using NUnit.Framework;
using OpenCvSharp;
using Wacton.Unicolour;
using ColorMineRgb = ColorMine.ColorSpaces.Rgb;
using ColorMineHsb = ColorMine.ColorSpaces.Hsb;
using ColorMineXyz = ColorMine.ColorSpaces.Xyz;
using ColorMineLab = ColorMine.ColorSpaces.Lab;

public class OtherLibraries
{
    // https://docs.opencv.org/4.5.5/de/d25/imgproc_color_conversions.html
    [Test]
    public void OpenCV()
    {
        foreach (var name in ColourReference.Names)
        {
            var hex = ColourReference.Hexs[name];
            var systemColour = ColorTranslator.FromHtml(hex);
            var (r, g, b) = (systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0);

            var unicolour = Unicolour.FromRgb(r, g, b);
            var rgb = unicolour.Rgb;
            var hsb = unicolour.Hsb;
            var xyz = unicolour.Xyz;
            var lab = unicolour.Lab;

            string FailMessage() => $"colour:{name}";

            var matRgb = new Mat(1, 1, MatType.CV_32FC3, new Scalar(r, g, b));
            var openCvRgb = matRgb.Get<Vec3f>(0, 0);
            Assert.That(rgb.R, Is.EqualTo(openCvRgb.Item0).Within(0.0005), FailMessage);
            Assert.That(rgb.G, Is.EqualTo(openCvRgb.Item1).Within(0.0005), FailMessage);
            Assert.That(rgb.B, Is.EqualTo(openCvRgb.Item2).Within(0.0005), FailMessage);

            var matHsv = new Mat(1, 1, MatType.CV_32FC3);
            Cv2.CvtColor(matRgb, matHsv, ColorConversionCodes.RGB2HSV);
            var openCvHsb = matHsv.Get<Vec3f>(0, 0);
            Assert.That(hsb.H, Is.EqualTo(openCvHsb.Item0).Within(0.0005), FailMessage);
            Assert.That(hsb.S, Is.EqualTo(openCvHsb.Item1).Within(0.0005), FailMessage);
            Assert.That(hsb.B, Is.EqualTo(openCvHsb.Item2).Within(0.0005), FailMessage);

            // it appears that OpenCV's RGB -> XYZ and RGB -> LAB conversions 
            // expect to receive RGB values that have already undergone linear correction...
            var matRgbLinear = new Mat(1, 1, MatType.CV_32FC3, new Scalar(rgb.RLinear, rgb.GLinear, rgb.BLinear));

            var matXyz = new Mat(1, 1, MatType.CV_32FC3);
            Cv2.CvtColor(matRgbLinear, matXyz, ColorConversionCodes.RGB2XYZ);
            var openCvXyz = matXyz.Get<Vec3f>(0, 0);
            Assert.That(xyz.X, Is.EqualTo(openCvXyz.Item0).Within(0.0005), FailMessage);
            Assert.That(xyz.Y, Is.EqualTo(openCvXyz.Item1).Within(0.0005), FailMessage);
            Assert.That(xyz.Z, Is.EqualTo(openCvXyz.Item2).Within(0.0005), FailMessage);

            // at this point I'm pretty sure OpenCV doesn't calculate RGB -> LAB correctly
            // as all other libraries and online tools calculate the same LAB as Unicolour
            // the tolerance is so large, this particular part of the test is redundant
            var matLab = new Mat(1, 1, MatType.CV_32FC3);
            Cv2.CvtColor(matRgbLinear, matLab, ColorConversionCodes.RGB2Lab);
            var openCvLab = matLab.Get<Vec3f>(0, 0);
            Assert.That(lab.L, Is.EqualTo(openCvLab.Item0).Within(50.0), FailMessage);
            Assert.That(lab.A, Is.EqualTo(openCvLab.Item1).Within(50.0), FailMessage);
            Assert.That(lab.B, Is.EqualTo(openCvLab.Item2).Within(50.0), FailMessage);
        }
    }

    // Colourful does not support HSB...
    [Test]
    public void Colourful()
    {
        foreach (var name in ColourReference.Names)
        {
            var hex = ColourReference.Hexs[name];
            var systemColour = ColorTranslator.FromHtml(hex);
            var (r, g, b) = (systemColour.R / 255.0, systemColour.G / 255.0, systemColour.B / 255.0);

            var unicolour = Unicolour.FromRgb(systemColour.R, systemColour.G, systemColour.B);
            var rgb = unicolour.Rgb;
            var xyz = unicolour.Xyz;
            var lab = unicolour.Lab;

            string FailMessage() => $"colour:{name}";

            var colourfulRgb = new RGBColor(r, g, b);
            Assert.That(rgb.R, Is.EqualTo(colourfulRgb.R).Within(0.00000000001), FailMessage);
            Assert.That(rgb.G, Is.EqualTo(colourfulRgb.G).Within(0.00000000001), FailMessage);
            Assert.That(rgb.B, Is.EqualTo(colourfulRgb.B).Within(0.00000000001), FailMessage);

            var rgbLinearConverter = new ConverterBuilder()
                .FromRGB(RGBWorkingSpaces.sRGB)
                .ToLinearRGB()
                .Build();
            var colourfulRgbLinear = rgbLinearConverter.Convert(colourfulRgb);
            Assert.That(rgb.RLinear, Is.EqualTo(colourfulRgbLinear.R).Within(0.00000000001), FailMessage);
            Assert.That(rgb.GLinear, Is.EqualTo(colourfulRgbLinear.G).Within(0.00000000001), FailMessage);
            Assert.That(rgb.BLinear, Is.EqualTo(colourfulRgbLinear.B).Within(0.00000000001), FailMessage);

            var xyzConverter = new ConverterBuilder()
                .FromRGB(RGBWorkingSpaces.sRGB)
                .ToXYZ(Illuminants.D65)
                .Build();
            var colourfulXyz = xyzConverter.Convert(colourfulRgb);
            Assert.That(xyz.X, Is.EqualTo(colourfulXyz.X).Within(0.0005), FailMessage);
            Assert.That(xyz.Y, Is.EqualTo(colourfulXyz.Y).Within(0.0005), FailMessage);
            Assert.That(xyz.Z, Is.EqualTo(colourfulXyz.Z).Within(0.0005), FailMessage);

            var labConverter = new ConverterBuilder()
                .FromRGB(RGBWorkingSpaces.sRGB)
                .ToLab(Illuminants.D65)
                .Build();
            var colourfulLab = labConverter.Convert(colourfulRgb);
            Assert.That(lab.L, Is.EqualTo(colourfulLab.L).Within(0.05), FailMessage);
            Assert.That(lab.A, Is.EqualTo(colourfulLab.a).Within(0.05), FailMessage);
            Assert.That(lab.B, Is.EqualTo(colourfulLab.b).Within(0.05), FailMessage);
        }
    }
    
    [Test]
    public void ColorMine()
    {
        foreach (var name in ColourReference.Names)
        {
            var hex = ColourReference.Hexs[name];
            var systemColour = ColorTranslator.FromHtml(hex);
            var (r, g, b) = (systemColour.R, systemColour.G, systemColour.B);

            var unicolour = Unicolour.FromRgb(systemColour.R, systemColour.G, systemColour.B);
            var rgb = unicolour.Rgb;
            var hsb = unicolour.Hsb;
            var xyz = unicolour.Xyz;
            var lab = unicolour.Lab;

            string FailMessage() => $"colour:{name}";

            var colorMineRgb = new ColorMineRgb {R = r, G = g, B = b};
            Assert.That(rgb.R, Is.EqualTo(colorMineRgb.R / 255.0).Within(0.00000000001), FailMessage);
            Assert.That(rgb.G, Is.EqualTo(colorMineRgb.G / 255.0).Within(0.00000000001), FailMessage);
            Assert.That(rgb.B, Is.EqualTo(colorMineRgb.B / 255.0).Within(0.00000000001), FailMessage);

            var colorMineHsb = colorMineRgb.To<ColorMineHsb>();
            Assert.That(hsb.H, Is.EqualTo(colorMineHsb.H).Within(0.0005), FailMessage);
            Assert.That(hsb.S, Is.EqualTo(colorMineHsb.S).Within(0.0005), FailMessage);
            Assert.That(hsb.B, Is.EqualTo(colorMineHsb.B).Within(0.0005), FailMessage);

            var colorMineXyz = colorMineRgb.To<ColorMineXyz>();
            Assert.That(xyz.X, Is.EqualTo(colorMineXyz.X / 100.0).Within(0.0005), FailMessage);
            Assert.That(xyz.Y, Is.EqualTo(colorMineXyz.Y / 100.0).Within(0.0005), FailMessage);
            Assert.That(xyz.Z, Is.EqualTo(colorMineXyz.Z / 100.0).Within(0.0005), FailMessage);

            var colorMineLab = colorMineRgb.To<ColorMineLab>();
            Assert.That(lab.L, Is.EqualTo(colorMineLab.L).Within(0.0005), FailMessage);
            Assert.That(lab.A, Is.EqualTo(colorMineLab.A).Within(0.0005), FailMessage);
            Assert.That(lab.B, Is.EqualTo(colorMineLab.B).Within(0.0005), FailMessage);
        }
    }
}