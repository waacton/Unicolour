namespace Wacton.Unicolour;

public static class RgbModels
{
    // https://www.color.org/chardata/rgb/srgb.xalter · https://en.wikipedia.org/wiki/SRGB
    public static class StandardRgb
    {
        public static readonly Chromaticity R = Rec709.R;
        public static readonly Chromaticity G = Rec709.G;
        public static readonly Chromaticity B = Rec709.B;
        public static readonly WhitePoint WhitePoint = D65;
        
        public static double FromLinear(double linear)
        {
            return ReflectWhenNegative(linear, value =>
            {
                return value switch
                {
                    <= 0.0031308 => 12.92 * value,
                    _ => 1.055 * Gamma(value, 2.4) - 0.055
                };
            });
        }
        
        public static double ToLinear(double nonlinear)
        {
            return ReflectWhenNegative(nonlinear, value =>
            {
                return value switch
                {
                    <= 0.04045 => value / 12.92,
                    _ => InverseGamma((value + 0.055) / 1.055, 2.4)
                };
            });
        }

        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "sRGB");
    }
    
    // https://www.color.org/chardata/rgb/DisplayP3.xalter · https://en.wikipedia.org/wiki/DCI-P3#P3_colorimetry
    public static class DisplayP3
    {
        public static readonly Chromaticity R = new(0.680, 0.320);
        public static readonly Chromaticity G = new(0.265, 0.690);
        public static readonly Chromaticity B = new(0.150, 0.060);
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double value) => StandardRgb.FromLinear(value);
        public static double ToLinear(double value) => StandardRgb.ToLinear(value);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "Display P3");
    }
    
    // SD · https://www.itu.int/rec/R-REC-BT.601/ · https://en.wikipedia.org/wiki/Rec._601#Primary_chromaticities
    // NOTE: gamma correction uses the more accurate Rec. 2020 calculation (https://www.itu.int/rec/T-REC-H.273) otherwise roundtrip less reliable
    public static class Rec601Line625
    {
        public static readonly Chromaticity R = Rec1700Line625.R;
        public static readonly Chromaticity G = Rec1700Line625.G;
        public static readonly Chromaticity B = Rec1700Line625.B;
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => Rec2020.FromLinear(linear);
        public static double ToLinear(double nonlinear) => Rec2020.ToLinear(nonlinear);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "Rec. 601 (625-line)");
    }
    
    // SD · https://www.itu.int/rec/R-REC-BT.601/ · https://en.wikipedia.org/wiki/Rec._601#Primary_chromaticities
    // NOTE: gamma correction uses the more accurate Rec. 2020 calculation (https://www.itu.int/rec/T-REC-H.273) otherwise roundtrip less reliable
    public static class Rec601Line525
    {
        public static readonly Chromaticity R = Rec1700Line525.R;
        public static readonly Chromaticity G = Rec1700Line525.G;
        public static readonly Chromaticity B = Rec1700Line525.B;
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => Rec2020.FromLinear(linear);
        public static double ToLinear(double nonlinear) => Rec2020.ToLinear(nonlinear);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "Rec. 601 (525-line)");
    }
    
    // HD · https://www.itu.int/rec/R-REC-BT.709/ · https://en.wikipedia.org/wiki/Rec._709#Primary_chromaticities
    // NOTE: gamma correction uses the more accurate Rec. 2020 calculation (https://www.itu.int/rec/T-REC-H.273) otherwise roundtrip less reliable
    public static class Rec709
    {
        public static readonly Chromaticity R = new(0.6400, 0.3300);
        public static readonly Chromaticity G = new(0.3000, 0.6000);
        public static readonly Chromaticity B = new(0.1500, 0.0600);
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => Rec2020.FromLinear(linear);
        public static double ToLinear(double nonlinear) => Rec2020.ToLinear(nonlinear);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "Rec. 709");
    }
    
    // UHD · https://www.itu.int/rec/R-REC-BT.2020/ · https://en.wikipedia.org/wiki/Rec._2020#System_colorimetry
    public static class Rec2020
    {
        public static readonly Chromaticity R = new(0.708, 0.292);
        public static readonly Chromaticity G = new(0.170, 0.797);
        public static readonly Chromaticity B = new(0.131, 0.046);
        public static readonly WhitePoint WhitePoint = D65;
        
        internal const double Alpha = 1.09929682680944;
        internal const double Beta = 0.018053968510807;
        
        public static double FromLinear(double linear)
        {
            return ReflectWhenNegative(linear, e =>
            {
                return e switch
                {
                    >= Beta => Alpha * Math.Pow(e, 0.45) - (Alpha - 1),
                    _ => 4.5 * e
                };
            });
        }

        public static double ToLinear(double nonlinear)
        {
            return ReflectWhenNegative(nonlinear, ePrime =>
            {
                return ePrime switch
                {
                    >= Beta * 4.5 => Math.Pow((ePrime + (Alpha - 1)) / Alpha, 1 / 0.45),
                    _ => ePrime / 4.5
                };
            });
        }

        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "Rec. 2020");
    }
    
    // HDR · https://www.itu.int/rec/R-REC-BT.2100/ · https://en.wikipedia.org/wiki/Rec._2100#System_colorimetry
    public static class Rec2100Pq
    {
        public static readonly Chromaticity R = Rec2020.R;
        public static readonly Chromaticity G = Rec2020.G;
        public static readonly Chromaticity B = Rec2020.B;
        public static readonly WhitePoint WhitePoint = D65;
        
        public static double FromLinear(double linear, DynamicRange dynamicRange)
        {
            return ReflectWhenNegative(linear, f => Pq.Smpte.InverseEotf(f, dynamicRange.WhiteLuminance));
        }

        public static double ToLinear(double nonlinear, DynamicRange dynamicRange)
        {
            return ReflectWhenNegative(nonlinear, ePrime => Pq.Smpte.Eotf(ePrime, dynamicRange.WhiteLuminance));
        }

        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "Rec. 2100 PQ");
    }
    
    // HDR · https://www.itu.int/rec/R-REC-BT.2100/ · https://en.wikipedia.org/wiki/Rec._2100#System_colorimetry
    public static class Rec2100Hlg
    {
        public static readonly Chromaticity R = Rec2020.R;
        public static readonly Chromaticity G = Rec2020.G;
        public static readonly Chromaticity B = Rec2020.B;
        public static readonly WhitePoint WhitePoint = D65;
        
        public static double FromLinear(double linear, DynamicRange dynamicRange)
        {
            return ReflectWhenNegative(linear, e => Hlg.Oetf(e, dynamicRange));
        }

        public static double ToLinear(double nonlinear, DynamicRange dynamicRange)
        {
            return ReflectWhenNegative(nonlinear, ePrime => Hlg.InverseOetf(ePrime, dynamicRange));
        }

        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "Rec. 2100 HLG");
    }
    
    // https://en.wikipedia.org/wiki/Adobe_RGB_color_space#Specifications
    public static class A98
    {
        public static readonly Chromaticity R = new(0.6400, 0.3300);
        public static readonly Chromaticity G = new(0.2100, 0.7100);
        public static readonly Chromaticity B = new(0.1500, 0.0600);
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => SimpleGamma(linear, 563 / 256.0);
        public static double ToLinear(double nonlinear) => SimpleInverseGamma(nonlinear, 563 / 256.0);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "A98 RGB");
    }
    
    // https://en.wikipedia.org/wiki/ProPhoto_RGB_color_space
    public static class ProPhoto
    {
        public static readonly Chromaticity R = new(0.734699, 0.265301);
        public static readonly Chromaticity G = new(0.159597, 0.840403);
        public static readonly Chromaticity B = new(0.036598, 0.000105);
        public static readonly WhitePoint WhitePoint = D50;
        
        private const double Et = 1 / 512.0;

        public static double FromLinear(double linear)
        {
            return ReflectWhenNegative(linear, value =>
            {
                return value switch
                {
                    < Et => 16 * value,
                    _ => Gamma(value, 1.8)
                };
            });
        }

        public static double ToLinear(double nonlinear)
        {
            return ReflectWhenNegative(nonlinear, value =>
            {
                return value switch
                {
                    < Et * 16 => value / 16.0,
                    _ => InverseGamma(value, 1.8)
                };
            });
        }

        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "ProPhoto RGB");
    }
    
    // https://ieeexplore.ieee.org/document/7289895 · https://en.wikipedia.org/wiki/Academy_Color_Encoding_System
    // ACES 2065-1 is linear (i.e. gamma of 1)
    public static class Aces20651
    {
        public static readonly Chromaticity R = AcesAp0.R;
        public static readonly Chromaticity G = AcesAp0.G;
        public static readonly Chromaticity B = AcesAp0.B;
        public static readonly WhitePoint WhitePoint = Aces;
        public static double FromLinear(double linear) => linear;
        public static double ToLinear(double nonlinear) => nonlinear;
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "ACES 2065-1");
    }
    
    // https://docs.acescentral.com/specifications/acescg/ · https://en.wikipedia.org/wiki/Academy_Color_Encoding_System
    // ACEScg is linear (i.e. gamma of 1)
    public static class Acescg
    {
        public static readonly Chromaticity R = AcesAp1.R;
        public static readonly Chromaticity G = AcesAp1.G;
        public static readonly Chromaticity B = AcesAp1.B;
        public static readonly WhitePoint WhitePoint = Aces;
        public static double FromLinear(double linear) => linear;
        public static double ToLinear(double nonlinear) => nonlinear;
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "ACEScg");
    }
    
    // https://docs.acescentral.com/specifications/acescct/ · https://en.wikipedia.org/wiki/Academy_Color_Encoding_System
    public static class Acescct
    {
        public static readonly Chromaticity R = AcesAp1.R;
        public static readonly Chromaticity G = AcesAp1.G;
        public static readonly Chromaticity B = AcesAp1.B;
        public static readonly WhitePoint WhitePoint = Aces;

        internal const double MaxLinearValue = 65504;

        public static double FromLinear(double linear)
        {
            return linear switch
            {
                <= 0.0078125 => 10.5402377416545 * linear + 0.0729055341958355,
                _ => (Log2(linear) + 9.72) / 17.52
            };
        }
        
        public static double ToLinear(double nonlinear)
        {
            const double threshold = (15.999295387023411 + 9.72) / 17.52; // 15.999295387023411 = Log2(65504)
            if (double.IsNaN(nonlinear)) return double.NaN;
            return nonlinear switch
            {
                <= 0.155251141552511 => (nonlinear - 0.0729055341958355) / 10.5402377416545,
                < threshold => Math.Pow(2, nonlinear * 17.52 - 9.72),
                _ => MaxLinearValue
            };
        }
        
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "ACEScct");
    }
    
    // https://docs.acescentral.com/specifications/acescc/ · https://en.wikipedia.org/wiki/Academy_Color_Encoding_System
    public static class Acescc
    {
        public static readonly Chromaticity R = AcesAp1.R;
        public static readonly Chromaticity G = AcesAp1.G;
        public static readonly Chromaticity B = AcesAp1.B;
        public static readonly WhitePoint WhitePoint = Aces;
        
        internal static readonly double MinNonlinearValue = (Log2(Math.Pow(2, -16)) + 9.72) / 17.52;
        internal const double MaxLinearValue = 65504;

        public static double FromLinear(double linear)
        {
            const double threshold = 0.000030517578125; // Math.Pow(2, -15)
            return linear switch
            {
                <= 0 => MinNonlinearValue,
                < threshold => (Log2(Math.Pow(2, -16) + linear * 0.5) + 9.72) / 17.52,
                _ => (Log2(linear) + 9.72) / 17.52
            };
        }
        
        public static double ToLinear(double nonlinear)
        {
            const double threshold = (15.999295387023411 + 9.72) / 17.52; // 15.999295387023411 = Log2(65504)
            if (double.IsNaN(nonlinear)) return double.NaN;
            return nonlinear switch
            {
                <= (9.72 - 15) / 17.52 => (Math.Pow(2, nonlinear * 17.52 - 9.72) - Math.Pow(2, -16)) * 2,
                < threshold => Math.Pow(2, nonlinear * 17.52 - 9.72),
                _ => MaxLinearValue
            };
        }
        
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "ACEScc");
    }
    
    // http://doi.org/10.1889/1.2433175 · https://en.wikipedia.org/wiki/XvYCC
    // linear transfer functions are an extended form of Rec. 2020; uses the full alpha & beta values, otherwise roundtrip less reliable
    public static class XvYcc
    {
        public static readonly Chromaticity R = Rec709.R;
        public static readonly Chromaticity G = Rec709.G;
        public static readonly Chromaticity B = Rec709.B;
        public static readonly WhitePoint WhitePoint = D65;
        
        private const double Alpha = Rec2020.Alpha;
        private const double Beta = Rec2020.Beta;
        
        public static double FromLinear(double linear)
        {
            return linear switch
            {
                <= -Beta => -Alpha * Math.Pow(-linear, 0.45) + (Alpha - 1),
                >= Beta => Alpha * Math.Pow(linear, 0.45) - (Alpha - 1),
                _ => 4.5 * linear
            };
        }
        
        public static double ToLinear(double nonlinear)
        {
            return nonlinear switch
            {
                <= -Beta * 4.5 => -Math.Pow((nonlinear - (Alpha - 1)) / -Alpha, 1 / 0.45),
                >= Beta * 4.5 => Math.Pow((nonlinear + (Alpha - 1)) / Alpha, 1 / 0.45),
                _ => nonlinear / 4.5
            };
        }
        
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "xvYCC");
    }
    
    /*
     * ========================================
     * Analog systems
     * ========================================
     */
    
    // https://www.itu.int/rec/R-REC-BT.470-6-199811-S · https://en.wikipedia.org/wiki/PAL#Colorimetry · https://en.wikipedia.org/wiki/Gamma_correction#Analog_TV
    // seems to have been superseded by "625 PAL" from Rec. 1700 - note the change of gamma
    public static class Pal
    {
        public static readonly Chromaticity R = Rec470SystemNotM.R;
        public static readonly Chromaticity G = Rec470SystemNotM.G;
        public static readonly Chromaticity B = Rec470SystemNotM.B;
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => SimpleGamma(linear, 2.8);
        public static double ToLinear(double nonlinear) => SimpleInverseGamma(nonlinear, 2.8);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "PAL (Rec. 470)");
    }
    
    // https://www.itu.int/rec/R-REC-BT.470-6-199811-S · https://en.wikipedia.org/wiki/PAL#Colorimetry · https://en.wikipedia.org/wiki/Gamma_correction#Analog_TV
    // seems to have been superseded by "525 PAL" from Rec. 1700 - note the change of RGB chromaticity and gamma
    // (https://en.wikipedia.org/wiki/PAL#Colorimetry appears to incorrectly list PAL-M gamma as 2.2, but Rec. 470-6 is clearly 2.8 for M/PAL)
    public static class PalM
    {
        public static readonly Chromaticity R = Rec470SystemM.R;
        public static readonly Chromaticity G = Rec470SystemM.G;
        public static readonly Chromaticity B = Rec470SystemM.B;
        public static readonly WhitePoint WhitePoint = C;
        public static double FromLinear(double linear) => SimpleGamma(linear, 2.8);
        public static double ToLinear(double nonlinear) => SimpleInverseGamma(nonlinear, 2.8);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "PAL-M (Rec. 470)");
    }
    
    // https://www.itu.int/rec/R-REC-BT.1700
    // seems to supersede all non-M PAL systems in Rec. 470 (which use 625-line) - note the change of gamma
    // effectively the same as PAL/SECAM RGB on http://www.brucelindbloom.com/index.html?WorkingSpaceInfo.html#Specifications
    public static class Pal625
    {
        public static readonly Chromaticity R = Rec1700Line625.R;
        public static readonly Chromaticity G = Rec1700Line625.G;
        public static readonly Chromaticity B = Rec1700Line625.B;
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => SimpleGamma(linear, 2.2);
        public static double ToLinear(double nonlinear) => SimpleInverseGamma(nonlinear, 2.2);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "PAL 625 (Rec. 1700)");
    }
    
    // https://www.itu.int/rec/R-REC-BT.1700
    // seems to supersede PAL-M in Rec. 470 (which uses 525-line) - note the change of RGB chromaticity and gamma
    public static class Pal525
    {
        public static readonly Chromaticity R = Rec1700Line525.R;
        public static readonly Chromaticity G = Rec1700Line525.G;
        public static readonly Chromaticity B = Rec1700Line525.B;
        public static readonly WhitePoint WhitePoint = C;
        public static double FromLinear(double linear) => SimpleGamma(linear, 2.2);
        public static double ToLinear(double nonlinear) => SimpleInverseGamma(nonlinear, 2.2);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "PAL 525 (Rec. 1700)");
    }
    
    // https://www.itu.int/rec/R-REC-BT.470-6-199811-S · https://en.wikipedia.org/wiki/NTSC#Colorimetry · https://en.wikipedia.org/wiki/Gamma_correction#Analog_TV
    // seems to have been superseded by NTSC from Rec. 1700 / SMPTE 170M-2004 - note the change of gamma
    // effectively the same as NTSC RGB on http://www.brucelindbloom.com/index.html?WorkingSpaceInfo.html#Specifications
    public static class Ntsc
    {
        public static readonly Chromaticity R = Rec470SystemM.R;
        public static readonly Chromaticity G = Rec470SystemM.G;
        public static readonly Chromaticity B = Rec470SystemM.B;
        public static readonly WhitePoint WhitePoint = C;
        public static double FromLinear(double linear) => SimpleGamma(linear, 2.2);
        public static double ToLinear(double nonlinear) => SimpleInverseGamma(nonlinear, 2.2);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "NTSC (Rec. 470)");
    }
    
    // http://doi.org/10.1007/978-3-642-35947-7_12-2 · https://en.wikipedia.org/wiki/NTSC#SMPTE_C · https://en.wikipedia.org/wiki/Gamma_correction#Analog_TV
    // effectively the same as SMPTE-C RGB on http://www.brucelindbloom.com/index.html?WorkingSpaceInfo.html#Specifications
    public static class NtscSmpteC
    {
        public static readonly Chromaticity R = SmpteC.R;
        public static readonly Chromaticity G = SmpteC.G;
        public static readonly Chromaticity B = SmpteC.B;
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => SimpleGamma(linear, 2.2);
        public static double ToLinear(double nonlinear) => SimpleInverseGamma(nonlinear, 2.2);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "NTSC (SMPTE-C)");
    }
    
    // https://www.itu.int/rec/R-REC-BT.1700 (SMPTE 170M-2004)
    // seems to supersede NTSC in Rec. 470 - note the change of gamma
    // NOTE: gamma correction uses the more accurate Rec. 2020 calculation (https://www.itu.int/rec/T-REC-H.273) otherwise roundtrip less reliable
    public static class Ntsc525
    {
        public static readonly Chromaticity R = Rec1700Line525.R;
        public static readonly Chromaticity G = Rec1700Line525.G;
        public static readonly Chromaticity B = Rec1700Line525.B;
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => Rec2020.FromLinear(linear);
        public static double ToLinear(double nonlinear) => Rec2020.ToLinear(nonlinear);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "NTSC 525 (Rec. 1700)");
    }
    
    // https://www.itu.int/rec/R-REC-BT.470-6-199811-S · https://en.wikipedia.org/wiki/PAL#Colorimetry · https://en.wikipedia.org/wiki/Gamma_correction#Analog_TV
    // seems to have been superseded by "625-line SECAM" from Rec. 1700 - note the change of gamma
    // effectively the same as PAL/SECAM RGB on http://www.brucelindbloom.com/index.html?WorkingSpaceInfo.html#Specifications
    public static class Secam
    {
        public static readonly Chromaticity R = Rec470SystemNotM.R;
        public static readonly Chromaticity G = Rec470SystemNotM.G;
        public static readonly Chromaticity B = Rec470SystemNotM.B;
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => SimpleGamma(linear, 2.8);
        public static double ToLinear(double nonlinear) => SimpleInverseGamma(nonlinear, 2.8);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "SECAM (Rec. 470)");
    }
    
    // https://www.itu.int/rec/R-REC-BT.1700
    // seems to supersede SECAM in Rec. 470 - note the change of gamma
    public static class Secam625
    {
        public static readonly Chromaticity R = Rec1700Line625.R;
        public static readonly Chromaticity G = Rec1700Line625.G;
        public static readonly Chromaticity B = Rec1700Line625.B;
        public static readonly WhitePoint WhitePoint = D65;
        public static double FromLinear(double linear) => SimpleGamma(linear, 2.2);
        public static double ToLinear(double nonlinear) => SimpleInverseGamma(nonlinear, 2.2);
        public static RgbConfiguration RgbConfiguration => new(R, G, B, WhitePoint, FromLinear, ToLinear, "SECAM 625 (Rec. 1700)");
    }
    
    /*
     * ========================================
     * useful values for models to reference
     * https://www.itu.int/rec/T-REC-H.273
     * ========================================
     */
    
    private static readonly WhitePoint D65 = Illuminant.D65.GetWhitePoint(Observer.Degree2);
    private static readonly WhitePoint D50 = Illuminant.D50.GetWhitePoint(Observer.Degree2);
    private static readonly WhitePoint C = Illuminant.C.GetWhitePoint(Observer.Degree2);
    private static readonly WhitePoint Aces = new Chromaticity(0.32168, 0.33767).ToWhitePoint();
    
    private static class Rec470SystemNotM
    {
        internal static readonly Chromaticity R = new(0.64, 0.33);
        internal static readonly Chromaticity G = new(0.29, 0.60);
        internal static readonly Chromaticity B = new(0.15, 0.06);
    }
    
    private static class Rec470SystemM
    {
        internal static readonly Chromaticity R = new(0.67, 0.33);
        internal static readonly Chromaticity G = new(0.21, 0.71);
        internal static readonly Chromaticity B = new(0.14, 0.08);
    }
    
    private static class SmpteC
    {
        internal static readonly Chromaticity R = new(0.630, 0.340);
        internal static readonly Chromaticity G = new(0.310, 0.595);
        internal static readonly Chromaticity B = new(0.155, 0.070);
    }

    private static class Rec1700Line625
    {
        internal static readonly Chromaticity R = Rec470SystemNotM.R;
        internal static readonly Chromaticity G = Rec470SystemNotM.G;
        internal static readonly Chromaticity B = Rec470SystemNotM.B;
    }
    
    private static class Rec1700Line525
    {
        internal static readonly Chromaticity R = SmpteC.R;
        internal static readonly Chromaticity G = SmpteC.G;
        internal static readonly Chromaticity B = SmpteC.B;
    }
    
    private static class AcesAp0
    {
        internal static readonly Chromaticity R = new(0.7347, 0.2653);
        internal static readonly Chromaticity G = new(0.0000, 1.0000);
        internal static readonly Chromaticity B = new(0.0001, -0.0770);
    }
    
    private static class AcesAp1
    {
        internal static readonly Chromaticity R = new(0.713, 0.293);
        internal static readonly Chromaticity G = new(0.165, 0.830);
        internal static readonly Chromaticity B = new(0.128, 0.044);
    }
    
    private static double SimpleGamma(double linear, double gamma)
    {
        return ReflectWhenNegative(linear, value => Gamma(value, gamma));
    }
    
    private static double SimpleInverseGamma(double nonlinear, double gamma)
    {
        return ReflectWhenNegative(nonlinear, value => InverseGamma(value, gamma));
    }

    private static double Gamma(double value, double gamma) => Math.Pow(value, 1 / gamma);
    private static double InverseGamma(double value, double gamma) => Math.Pow(value, gamma);

    private static double ReflectWhenNegative(double value, Func<double, double> function)
    {
        if (double.IsNaN(value)) return double.NaN;
        return Sign(value) * function(Math.Abs(value));
    }

    // instead of Math.Sign() where Math.Sign(0) returns 0
    // this Sign() handles 0 as positive and -0 as negative
    // so if X -> 0 and -X -> -0, the roundtrip 0 -> X and -0 -> -X can be maintained (e.g. HLG)
    private static readonly long ZeroBits = BitConverter.DoubleToInt64Bits(0);
    private static int Sign(double value)
    {
        return value switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => BitConverter.DoubleToInt64Bits(value) == ZeroBits ? 1 : -1
        };
    }
    
    private static double Log2(double x) => Math.Log(x, 2);
}