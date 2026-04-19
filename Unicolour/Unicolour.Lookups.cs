using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour;

public partial class Unicolour
{
    public ColourRepresentation GetRepresentation(ColourSpace colourSpace)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => Rgb,
            ColourSpace.Rgb255 => Rgb.Byte255,
            ColourSpace.RgbLinear => RgbLinear,
            ColourSpace.Hsb => Hsb,
            ColourSpace.Hsl => Hsl,
            ColourSpace.Hwb => Hwb,
            ColourSpace.Hsi => Hsi,
            ColourSpace.Xyz => Xyz,
            ColourSpace.Xyy => Xyy,
            ColourSpace.Wxy => Wxy,
            ColourSpace.Lab => Lab,
            ColourSpace.Lchab => Lchab,
            ColourSpace.Luv => Luv,
            ColourSpace.Lchuv => Lchuv,
            ColourSpace.Hsluv => Hsluv,
            ColourSpace.Hpluv => Hpluv,
            ColourSpace.Ypbpr => Ypbpr,
            ColourSpace.Ycbcr => Ycbcr,
            ColourSpace.Ycgco => Ycgco,
            ColourSpace.Yuv => Yuv,
            ColourSpace.Yiq => Yiq,
            ColourSpace.Ydbdr => Ydbdr,
            ColourSpace.Tsl => Tsl,
            ColourSpace.Xyb => Xyb,
            ColourSpace.Lms => Lms,
            ColourSpace.Ipt => Ipt,
            ColourSpace.Ictcp => Ictcp,
            ColourSpace.Jzazbz => Jzazbz,
            ColourSpace.Jzczhz => Jzczhz,
            ColourSpace.Oklab => Oklab,
            ColourSpace.Oklch => Oklch,
            ColourSpace.Okhsv => Okhsv,
            ColourSpace.Okhsl => Okhsl,
            ColourSpace.Okhwb => Okhwb,
            ColourSpace.Oklrab => Oklrab,
            ColourSpace.Oklrch => Oklrch,
            ColourSpace.Cam02 => Cam02,
            ColourSpace.Cam16 => Cam16,
            ColourSpace.Hct => Hct,
            ColourSpace.Munsell => Munsell,
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }
    
    private static ColourRepresentation CreateRepresentation(ColourSpace colourSpace, double first, double second, double third, Configuration config, Limitation limitation)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => new Rgb(first, second, third, limitation),
            ColourSpace.RgbLinear => new RgbLinear(first, second, third, limitation),
            ColourSpace.Hsb => new Hsb(first, second, third, limitation),
            ColourSpace.Hsl => new Hsl(first, second, third, limitation),
            ColourSpace.Hwb => new Hwb(first, second, third, limitation),
            ColourSpace.Hsi => new Hsi(first, second, third, limitation),
            ColourSpace.Xyz => new Xyz(first, second, third, config.Xyz.WhitePoint, limitation),
            ColourSpace.Xyy => new Xyy(first, second, third, config.Xyz.WhitePoint, limitation),
            ColourSpace.Wxy => new Wxy(first, second, third, limitation),
            ColourSpace.Lab => new Lab(first, second, third, limitation),
            ColourSpace.Lchab => new Lchab(first, second, third, limitation),
            ColourSpace.Luv => new Luv(first, second, third, limitation),
            ColourSpace.Lchuv => new Lchuv(first, second, third, limitation),
            ColourSpace.Hsluv => new Hsluv(first, second, third, limitation),
            ColourSpace.Hpluv => new Hpluv(first, second, third, limitation),
            ColourSpace.Ypbpr => new Ypbpr(first, second, third, limitation),
            ColourSpace.Ycbcr => new Ycbcr(first, second, third, limitation),
            ColourSpace.Ycgco => new Ycgco(first, second, third, limitation),
            ColourSpace.Yuv => new Yuv(first, second, third, limitation),
            ColourSpace.Yiq => new Yiq(first, second, third, limitation),
            ColourSpace.Ydbdr => new Ydbdr(first, second, third, limitation),
            ColourSpace.Tsl => new Tsl(first, second, third, limitation),
            ColourSpace.Xyb => new Xyb(first, second, third, limitation),
            ColourSpace.Lms => new Lms(first, second, third, limitation),
            ColourSpace.Ipt => new Ipt(first, second, third, limitation),
            ColourSpace.Ictcp => new Ictcp(first, second, third, limitation),
            ColourSpace.Jzazbz => new Jzazbz(first, second, third, limitation),
            ColourSpace.Jzczhz => new Jzczhz(first, second, third, limitation),
            ColourSpace.Oklab => new Oklab(first, second, third, limitation),
            ColourSpace.Oklch => new Oklch(first, second, third, limitation),
            ColourSpace.Okhsv => new Okhsv(first, second, third, limitation),
            ColourSpace.Okhsl => new Okhsl(first, second, third, limitation),
            ColourSpace.Okhwb => new Okhwb(first, second, third, limitation),
            ColourSpace.Oklrab => new Oklrab(first, second, third, limitation),
            ColourSpace.Oklrch => new Oklrch(first, second, third, limitation),
            ColourSpace.Cam02 => new Cam02(new Cam.Ucs(first, second, third), config.Cam, limitation),
            ColourSpace.Cam16 => new Cam16(new Cam.Ucs(first, second, third), config.Cam, limitation),
            ColourSpace.Hct => new Hct(first, second, third, limitation),
            ColourSpace.Munsell => new Munsell(first, second, third, limitation),
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }
    
    private static ColourRepresentation CreateRepresentation(ColourSpace colourSpace, double grey, Configuration config)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => new Rgb(grey),
            ColourSpace.RgbLinear => new RgbLinear(grey),
            ColourSpace.Hsb => new Hsb(grey),
            ColourSpace.Hsl => new Hsl(grey),
            ColourSpace.Hwb => new Hwb(grey),
            ColourSpace.Hsi => new Hsi(grey),
            ColourSpace.Xyz => new Xyz(grey, config.Xyz.WhitePoint),
            ColourSpace.Xyy => new Xyy(grey, config.Xyz.WhitePoint),
            ColourSpace.Wxy => new Wxy(grey),
            ColourSpace.Lab => new Lab(grey),
            ColourSpace.Lchab => new Lchab(grey),
            ColourSpace.Luv => new Luv(grey),
            ColourSpace.Lchuv => new Lchuv(grey),
            ColourSpace.Hsluv => new Hsluv(grey),
            ColourSpace.Hpluv => new Hpluv(grey),
            ColourSpace.Ypbpr => new Ypbpr(grey),
            ColourSpace.Ycbcr => new Ycbcr(grey),
            ColourSpace.Ycgco => new Ycgco(grey),
            ColourSpace.Yuv => new Yuv(grey),
            ColourSpace.Yiq => new Yiq(grey),
            ColourSpace.Ydbdr => new Ydbdr(grey),
            ColourSpace.Tsl => new Tsl(grey),
            ColourSpace.Xyb => new Xyb(grey),
            ColourSpace.Lms => new Lms(grey),
            ColourSpace.Ipt => new Ipt(grey),
            ColourSpace.Ictcp => new Ictcp(grey),
            ColourSpace.Jzazbz => new Jzazbz(grey),
            ColourSpace.Jzczhz => new Jzczhz(grey),
            ColourSpace.Oklab => new Oklab(grey),
            ColourSpace.Oklch => new Oklch(grey),
            ColourSpace.Okhsv => new Okhsv(grey),
            ColourSpace.Okhsl => new Okhsl(grey),
            ColourSpace.Okhwb => new Okhwb(grey),
            ColourSpace.Oklrab => new Oklrab(grey),
            ColourSpace.Oklrch => new Oklrch(grey),
            ColourSpace.Cam02 => new Cam02(grey, config.Cam),
            ColourSpace.Cam16 => new Cam16(grey, config.Cam),
            ColourSpace.Hct => new Hct(grey),
            ColourSpace.Munsell => new Munsell(grey),
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }
    
    /*
     * evaluation method switch expressions are arranged as follows:
     * - first item     = target space is the initial space, simply return initial representation
     * - middle items   = reverse transforms to the target space; only the immediate transforms
     * - default item   = forward transform from a base space
     * -----------------
     * only need to consider the transforms relative to the target-space, as subsequent transforms are handled recursively
     * e.g. for target-space RGB...
     * - starting at HSL:
     *   - transforms: HSL ==reverse==> HSB ==reverse==> RGB
     *   - only need to specify: HSB ==reverse==> RGB
     *   - function: Hsb.ToRgb()
     * - starting at LAB:
     *   - LAB ==reverse==> XYZ ==forward==> RGB Linear ==forward==> RGB
     *   - only need to specify: RGB Linear ==forward==> RGB
     *   - function: Rgb.FromRgbLinear()
     */

    private Rgb EvaluateRgb()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Rgb => (Rgb)SourceRepresentation,
            ColourSpace.Hsb => Hsb.ToRgb(Hsb),
            ColourSpace.Hsl => Hsb.ToRgb(Hsb),
            ColourSpace.Hwb => Hsb.ToRgb(Hsb),
            ColourSpace.Hsi => Hsi.ToRgb(Hsi),
            ColourSpace.Ypbpr => Ypbpr.ToRgb(Ypbpr, Configuration.Ybr),
            ColourSpace.Ycbcr => Ypbpr.ToRgb(Ypbpr, Configuration.Ybr),
            ColourSpace.Ycgco => Ycgco.ToRgb(Ycgco),
            ColourSpace.Yuv => Yuv.ToRgb(Yuv),
            ColourSpace.Yiq => Yuv.ToRgb(Yuv),
            ColourSpace.Ydbdr => Yuv.ToRgb(Yuv),
            ColourSpace.Tsl => Tsl.ToRgb(Tsl, Configuration.Ybr),
            _ => Rgb.FromRgbLinear(RgbLinear, Configuration.Rgb, Configuration.DynamicRange)
        };
    }
    
    private RgbLinear EvaluateRgbLinear()
    {
        return SourceColourSpace switch
        {
            ColourSpace.RgbLinear => (RgbLinear)SourceRepresentation,
            ColourSpace.Rgb => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Hsb => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Hsl => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Hwb => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Hsi => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Ypbpr => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Ycbcr => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Ycgco => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Yuv => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Yiq => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Ydbdr => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Tsl => Rgb.ToRgbLinear(Rgb, Configuration.Rgb, Configuration.DynamicRange),
            ColourSpace.Xyb => Xyb.ToRgbLinear(Xyb),
            _ => RgbLinear.FromXyz(Xyz, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor)
        };
    }

    private Hsb EvaluateHsb()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Hsb => (Hsb)SourceRepresentation,
            ColourSpace.Hsl => Hsl.ToHsb(Hsl),
            ColourSpace.Hwb => Hwb.ToHsb(Hwb),
            _ => Hsb.FromRgb(Rgb)
        };
    }

    private Hsl EvaluateHsl()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Hsl => (Hsl)SourceRepresentation,
            _ => Hsl.FromHsb(Hsb)
        };
    }

    private Hwb EvaluateHwb()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Hwb => (Hwb)SourceRepresentation,
            _ => Hwb.FromHsb(Hsb)
        };
    }
    
    private Hsi EvaluateHsi()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Hsi => (Hsi)SourceRepresentation,
            _ => Hsi.FromRgb(Rgb)
        };
    }
    
    private Xyz EvaluateXyz()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Xyz => (Xyz)SourceRepresentation,
            ColourSpace.Rgb => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.RgbLinear => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Hsb => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Hsl => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Hwb => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Hsi => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Xyy => Xyy.ToXyz(Xyy),
            ColourSpace.Wxy => Xyy.ToXyz(Xyy),
            ColourSpace.Lab => Lab.ToXyz(Lab, Configuration.Xyz.WhitePoint),
            ColourSpace.Lchab => Lab.ToXyz(Lab, Configuration.Xyz.WhitePoint),
            ColourSpace.Luv => Luv.ToXyz(Luv, Configuration.Xyz.WhitePoint),
            ColourSpace.Lchuv => Luv.ToXyz(Luv, Configuration.Xyz.WhitePoint),
            ColourSpace.Hsluv => Luv.ToXyz(Luv, Configuration.Xyz.WhitePoint),
            ColourSpace.Hpluv => Luv.ToXyz(Luv, Configuration.Xyz.WhitePoint),
            ColourSpace.Ypbpr => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Ycbcr => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Ycgco => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Yuv => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Yiq => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Ydbdr => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Tsl => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Xyb => RgbLinear.ToXyz(RgbLinear, Configuration.Rgb, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Lms => Lms.ToXyz(Lms, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Ipt => Ipt.ToXyz(Ipt, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Ictcp => Ictcp.ToXyz(Ictcp, Configuration.Xyz.ChromaticAdaptor, Configuration.DynamicRange),
            ColourSpace.Jzazbz => Jzazbz.ToXyz(Jzazbz, Configuration.Xyz.ChromaticAdaptor, Configuration.DynamicRange),
            ColourSpace.Jzczhz => Jzazbz.ToXyz(Jzazbz, Configuration.Xyz.ChromaticAdaptor, Configuration.DynamicRange),
            ColourSpace.Oklab => Oklab.ToXyz(Oklab, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Oklch => Oklab.ToXyz(Oklab, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Okhsv => Oklab.ToXyz(Oklab, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Okhsl => Oklab.ToXyz(Oklab, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Okhwb => Oklab.ToXyz(Oklab, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Oklrab => Oklab.ToXyz(Oklab, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Oklrch => Oklab.ToXyz(Oklab, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Cam02 => Cam02.ToXyz(Cam02, Configuration.Cam, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Cam16 => Cam16.ToXyz(Cam16, Configuration.Cam, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Hct => Hct.ToXyz(Hct, Configuration.Xyz.ChromaticAdaptor),
            ColourSpace.Munsell => Xyy.ToXyz(Xyy),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Xyy EvaluateXyy()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Xyy => (Xyy)SourceRepresentation,
            ColourSpace.Wxy => Wxy.ToXyy(Wxy, Configuration.Xyz.SpectralBoundary),
            ColourSpace.Munsell => Munsell.ToXyy(Munsell, Configuration.Xyz.ChromaticAdaptor),
            _ => Xyy.FromXyz(Xyz)
        };
    }
    
    private Wxy EvaluateWxy()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Wxy => (Wxy)SourceRepresentation,
            _ => Wxy.FromXyy(Xyy, Configuration.Xyz.SpectralBoundary)
        };
    }

    private Lab EvaluateLab()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Lab => (Lab)SourceRepresentation,
            ColourSpace.Lchab => Lchab.ToLab(Lchab),
            _ => Lab.FromXyz(Xyz)
        };
    }

    private Lchab EvaluateLchab()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Lchab => (Lchab)SourceRepresentation,
            _ => Lchab.FromLab(Lab)
        };
    }

    private Luv EvaluateLuv()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Luv => (Luv)SourceRepresentation,
            ColourSpace.Lchuv => Lchuv.ToLuv(Lchuv),
            ColourSpace.Hsluv => Lchuv.ToLuv(Lchuv),
            ColourSpace.Hpluv => Lchuv.ToLuv(Lchuv),
            _ => Luv.FromXyz(Xyz)
        };
    }

    private Lchuv EvaluateLchuv()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Lchuv => (Lchuv)SourceRepresentation,
            ColourSpace.Hsluv => Hsluv.ToLchuv(Hsluv),
            ColourSpace.Hpluv => Hpluv.ToLchuv(Hpluv),
            _ => Lchuv.FromLuv(Luv)
        };
    }

    private Hsluv EvaluateHsluv()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Hsluv => (Hsluv)SourceRepresentation,
            _ => Hsluv.FromLchuv(Lchuv)
        };
    }

    private Hpluv EvaluateHpluv()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Hpluv => (Hpluv)SourceRepresentation,
            _ => Hpluv.FromLchuv(Lchuv)
        };
    }
    
    private Ypbpr EvaluateYpbpr()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Ypbpr => (Ypbpr)SourceRepresentation,
            ColourSpace.Ycbcr => Ycbcr.ToYpbpr(Ycbcr, Configuration.Ybr),
            _ => Ypbpr.FromRgb(Rgb, Configuration.Ybr)
        };
    }
    
    private Ycbcr EvaluateYcbcr()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Ycbcr => (Ycbcr)SourceRepresentation,
            _ => Ycbcr.FromYpbpr(Ypbpr, Configuration.Ybr)
        };
    }
    
    private Ycgco EvaluateYcgco()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Ycgco => (Ycgco)SourceRepresentation,
            _ => Ycgco.FromRgb(Rgb)
        };
    }
    
    private Yuv EvaluateYuv()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Yuv => (Yuv)SourceRepresentation,
            ColourSpace.Yiq => Yiq.ToYuv(Yiq),
            ColourSpace.Ydbdr => Ydbdr.ToYuv(Ydbdr),
            _ => Yuv.FromRgb(Rgb)
        };
    }
    
    private Yiq EvaluateYiq()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Yiq => (Yiq)SourceRepresentation,
            _ => Yiq.FromYuv(Yuv)
        };
    }
    
    private Ydbdr EvaluateYdbdr()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Ydbdr => (Ydbdr)SourceRepresentation,
            _ => Ydbdr.FromYuv(Yuv)
        };
    }
    
    private Tsl EvaluateTsl()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Tsl => (Tsl)SourceRepresentation,
            _ => Tsl.FromRgb(Rgb, Configuration.Ybr)
        };
    }
    
    private Xyb EvaluateXyb()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Xyb => (Xyb)SourceRepresentation,
            _ => Xyb.FromRgbLinear(RgbLinear)
        };
    }
    
    private Lms EvaluateLms()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Lms => (Lms)SourceRepresentation,
            _ => Lms.FromXyz(Xyz, Configuration.Xyz.ChromaticAdaptor)
        };
    }
    
    private Ipt EvaluateIpt()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Ipt => (Ipt)SourceRepresentation,
            _ => Ipt.FromXyz(Xyz, Configuration.Xyz.ChromaticAdaptor)
        };
    }

    private Ictcp EvaluateIctcp()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Ictcp => (Ictcp)SourceRepresentation,
            _ => Ictcp.FromXyz(Xyz, Configuration.Xyz.ChromaticAdaptor, Configuration.DynamicRange)
        };
    }

    private Jzazbz EvaluateJzazbz()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Jzazbz => (Jzazbz)SourceRepresentation,
            ColourSpace.Jzczhz => Jzczhz.ToJzazbz(Jzczhz),
            _ => Jzazbz.FromXyz(Xyz, Configuration.Xyz.ChromaticAdaptor, Configuration.DynamicRange)
        };
    }

    private Jzczhz EvaluateJzczhz()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Jzczhz => (Jzczhz)SourceRepresentation,
            _ => Jzczhz.FromJzazbz(Jzazbz)
        };
    }

    private Oklab EvaluateOklab()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Oklab => (Oklab)SourceRepresentation,
            ColourSpace.Oklch => Oklch.ToOklab(Oklch),
            ColourSpace.Okhsv => Okhsv.ToOklab(Okhsv, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Okhsl => Okhsl.ToOklab(Okhsl, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Okhwb => Okhsv.ToOklab(Okhsv, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb),
            ColourSpace.Oklrab => Oklrab.ToOklab(Oklrab),
            ColourSpace.Oklrch => Oklrab.ToOklab(Oklrab),
            _ => Oklab.FromXyz(Xyz, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb)
        };
    }

    private Oklch EvaluateOklch()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Oklch => (Oklch)SourceRepresentation,
            _ => Oklch.FromOklab(Oklab)
        };
    }
    
    private Okhsv EvaluateOkhsv()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Okhsv => (Okhsv)SourceRepresentation,
            ColourSpace.Okhwb => Okhwb.ToOkhsv(Okhwb),
            _ => Okhsv.FromOklab(Oklab, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb)
        };
    }
    
    private Okhsl EvaluateOkhsl()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Okhsl => (Okhsl)SourceRepresentation,
            _ => Okhsl.FromOklab(Oklab, Configuration.Xyz.ChromaticAdaptor, Configuration.Rgb)
        };
    }
    
    private Okhwb EvaluateOkhwb()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Okhwb => (Okhwb)SourceRepresentation,
            _ => Okhwb.FromOkhsv(Okhsv)
        };
    }
    
    private Oklrab EvaluateOklrab()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Oklrab => (Oklrab)SourceRepresentation,
            ColourSpace.Oklrch => Oklrch.ToOklrab(Oklrch),
            _ => Oklrab.FromOklab(Oklab)
        };
    }

    private Oklrch EvaluateOklrch()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Oklrch => (Oklrch)SourceRepresentation,
            _ => Oklrch.FromOklrab(Oklrab)
        };
    }
    
    private Cam02 EvaluateCam02()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Cam02 => (Cam02)SourceRepresentation,
            _ => Cam02.FromXyz(Xyz, Configuration.Cam, Configuration.Xyz.ChromaticAdaptor)
        };
    }
    
    private Cam16 EvaluateCam16()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Cam16 => (Cam16)SourceRepresentation,
            _ => Cam16.FromXyz(Xyz, Configuration.Cam, Configuration.Xyz.ChromaticAdaptor)
        };
    }
    
    private Hct EvaluateHct()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Hct => (Hct)SourceRepresentation,
            _ => Hct.FromXyz(Xyz, Configuration.Xyz.ChromaticAdaptor)
        };
    }
    
    private Munsell EvaluateMunsell()
    {
        return SourceColourSpace switch
        {
            ColourSpace.Munsell => (Munsell)SourceRepresentation,
            _ => Munsell.FromXyy(Xyy, Configuration.Xyz.ChromaticAdaptor)
        };
    }
}