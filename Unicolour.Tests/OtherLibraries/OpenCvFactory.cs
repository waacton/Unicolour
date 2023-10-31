namespace Wacton.Unicolour.Tests.OtherLibraries;

using System;
using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;
using Wacton.Unicolour.Tests.Utils;

/*
 * OpenCV does not support HWB / xyY / LCHab / LCHuv / HSLuv / HPLuv / ICtCp / Jzazbz / Jzczhz / Oklab / Oklch / CAM02 / CAM16 / HCT
 * OpenCV does not expose linear RGB
 * OpenCV RGB -> XYZ expects to linear RGB values (that have not been companded)
 * OpenCV XYZ -> RGB actually converts to linear RGB (not companded)
 * OpenCV LAB / LUV -> RGB clamps values, causing errors in subsequent conversions (e.g. LAB -> RGB -> XYZ / LUV)
 * OpenCV RGB -> LUV doesn't seem to work when LUV contains negative values
 */
internal class OpenCvFactory : ITestColourFactory
{
    public static readonly Tolerances Tolerances = new()
    {
        Rgb = 0.05, Hsb = 0.05, Hsl = 0.01, Xyz = 0.0005, 
        Lab = 1.0, Luv = 1.0
    };
    
    public TestColour FromRgb(double r, double g, double b, string name)
    {
        var rgbVec = AsVec(r, g, b);
        var rgb = GetInputVec(rgbVec);
        var hsb = GetConvertedVec(rgbVec, ColorConversionCodes.RGB2HSV_FULL);
        var hls = GetConvertedVec(rgbVec, ColorConversionCodes.RGB2HLS_FULL);
        var xyz = GetConvertedVec(ToLinear(rgbVec), ColorConversionCodes.RGB2XYZ);
        var lab = GetConvertedVec(rgbVec, ColorConversionCodes.RGB2Lab);
        var luv = GetConvertedVec(rgbVec, ColorConversionCodes.RGB2Luv);
        return Create(name, rgb, hsb, hls, xyz, lab, luv, Tolerances);
    }

    public TestColour FromHsb(double h, double s, double b, string name)
    {
        var hsbVec = AsVec(h, s, b);
        var hsb = GetInputVec(hsbVec);
        var rgb = GetConvertedVec(hsbVec, ColorConversionCodes.HSV2RGB);
        
        var hls = GetConvertedVec(rgb, ColorConversionCodes.RGB2HLS_FULL);
        var xyz = GetConvertedVec(ToLinear(rgb), ColorConversionCodes.RGB2XYZ);
        var lab = GetConvertedVec(rgb, ColorConversionCodes.RGB2Lab);
        var luv = GetConvertedVec(rgb, ColorConversionCodes.RGB2Luv);
        return Create(name, rgb, hsb, hls, xyz, lab, luv, Tolerances);
    }
    
    public TestColour FromHsl(double h, double s, double l, string name)
    {
        var hlsVec = AsVec(h, l, s);
        var hls = GetInputVec(hlsVec);
        var rgb = GetConvertedVec(hlsVec, ColorConversionCodes.HLS2RGB);
        
        var hsb = GetConvertedVec(rgb, ColorConversionCodes.RGB2HSV_FULL);
        var xyz = GetConvertedVec(ToLinear(rgb), ColorConversionCodes.RGB2XYZ);
        var lab = GetConvertedVec(rgb, ColorConversionCodes.RGB2Lab);
        var luv = GetConvertedVec(rgb, ColorConversionCodes.RGB2Luv);
        return Create(name, rgb, hsb, hls, xyz, lab, luv, Tolerances);
    }
    
    // OpenCV XYZ -> RGB converts to linear RGB, which does not easily feed into any more conversions
    public TestColour FromXyz(double x, double y, double z, string name)
    {
        var xyzVec = AsVec(x, y, z);
        var xyz = GetInputVec(xyzVec);
        var rgbLinear = GetConvertedVec(xyzVec, ColorConversionCodes.XYZ2RGB);
        
        return new TestColour
        {
            Name = name,
            RgbLinear = new(rgbLinear.Item0, rgbLinear.Item1, rgbLinear.Item2),
            Xyz = new(xyz.Item0, xyz.Item1, xyz.Item2),
            Tolerances = Tolerances with {RgbLinear = 0.005},
            IsRgbLinearConstrained = false
        };
    }
    
    public TestColour FromXyy(double x, double y, double upperY, string name) => throw new NotImplementedException();
    
    // OpenCV LAB -> RGB clamps values, causing errors in subsequent conversions
    public TestColour FromLab(double l, double a, double b, string name)
    {
        var labVec = AsVec(l, a, b);
        var lab = GetInputVec(labVec);
        var rgb = GetConvertedVec(labVec, ColorConversionCodes.Lab2RGB);
        
        var hsb = GetConvertedVec(rgb, ColorConversionCodes.RGB2HSV_FULL);
        var hls = GetConvertedVec(rgb, ColorConversionCodes.RGB2HLS_FULL);
        return Create(name, rgb, hsb, hls, null, lab, null, Tolerances);
    }
    
    // OpenCV LUV -> RGB clamps values, causing errors in subsequent conversions
    // OpenCV LUV -> RGB doesn't seem to work when LUV contains negative values
    public TestColour FromLuv(double l, double u, double v, string name)
    {
        var canHandleLuv = l >= 0 && u >= 0 && v >= 0;
        if (!canHandleLuv) return new TestColour { Name = name, Tolerances = Tolerances };
        
        var luvVec = AsVec(l, u, v);
        var luv = GetInputVec(luvVec);
        var rgb = GetConvertedVec(luvVec, ColorConversionCodes.Luv2RGB);
        
        var hsb = GetConvertedVec(rgb, ColorConversionCodes.RGB2HSV_FULL);
        var hls = GetConvertedVec(rgb, ColorConversionCodes.RGB2HLS_FULL);
        return Create(name, rgb, hsb, hls, null, null, luv, Tolerances with {Rgb = 0.075, Hsb = 0.075, Hsl = 0.025});
    }
    
    public TestColour FromLchab(double l, double c, double h, string name) => throw new NotImplementedException();
    public TestColour FromLchuv(double l, double c, double h, string name) => throw new NotImplementedException();
    
    private static TestColour Create(string name, 
        Vec3f rgb, Vec3f hsb, Vec3f hls,
        Vec3f? xyz, Vec3f? lab, Vec3f? luv,
        Tolerances tolerances)
    {
        return new TestColour
        {
            Name = name,
            Rgb = new(rgb.Item0, rgb.Item1, rgb.Item2),
            Hsb = new(hsb.Item0, hsb.Item1, hsb.Item2),
            Hsl = new(hls.Item0, hls.Item2, hls.Item1),
            Xyz = xyz == null ? null : new(xyz.Value.Item0, xyz.Value.Item1, xyz.Value.Item2),
            Lab = lab == null ? null : new(lab.Value.Item0, lab.Value.Item1, lab.Value.Item2),
            Luv = luv == null ? null : new(luv.Value.Item0, luv.Value.Item1, luv.Value.Item2),
            Tolerances = tolerances,
            ExcludeFromHsxTestReasons = HsxExclusions(rgb)
        };
    }
    
    private static List<string> HsxExclusions(Vec3f rgb)
    {
        var exclusions = new List<string>();
        if (HasLowChroma(rgb)) exclusions.Add("OpenCV converts via RGB and does not handle low RGB chroma");
        return exclusions;
    }

    private static Vec3f GetInputVec(Vec3f input)
    {
        var (i1, i2, i3) = input;
        var mat = new Mat(1, 1, MatType.CV_32FC3, new Scalar(i1, i2, i3));
        return mat.Get<Vec3f>(0, 0);
    }

    private static Vec3f GetConvertedVec(Vec3f input, ColorConversionCodes conversionCode)
    {
        var (i1, i2, i3) = input;
        var matIn = new Mat(1, 1, MatType.CV_32FC3, new Scalar(i1, i2, i3));
        var matOut = new Mat(1, 1, MatType.CV_32FC3);
        Cv2.CvtColor(matIn, matOut, conversionCode);
        return matOut.Get<Vec3f>(0, 0);
    }
    
    private static Vec3f AsVec(double first, double second, double third) => new((float)first, (float)second, (float)third);
    private static Vec3f ToLinear(Vec3f input) => DoOperation(input, value => (float) Companding.StandardRgb.ToLinear(value));
    private static Vec3f DoOperation(Vec3f input, Func<float, float> function) => new(function(input.Item0), function(input.Item1), function(input.Item2));
    
    private static bool HasLowChroma(Vec3f rgb)
    {
        // OpenCV can end up with extreme values (e.g. 0 or multiple of 60 hue) if the chroma is small
        // (potentially due to using floats instead of doubles)
        // which causes significant deviation from Unicolour calculations with very small values
        var components = new[] {rgb.Item0, rgb.Item1, rgb.Item2};
        var chroma = components.Max() - components.Min();
        return chroma < 0.000001;
    }
}