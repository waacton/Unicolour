﻿namespace Wacton.Unicolour;

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
            ColourSpace.Xyz => Xyz,
            ColourSpace.Xyy => Xyy,
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
            ColourSpace.Ipt => Ipt,
            ColourSpace.Ictcp => Ictcp,
            ColourSpace.Jzazbz => Jzazbz,
            ColourSpace.Jzczhz => Jzczhz,
            ColourSpace.Oklab => Oklab,
            ColourSpace.Oklch => Oklch,
            ColourSpace.Cam02 => Cam02,
            ColourSpace.Cam16 => Cam16,
            ColourSpace.Hct => Hct,
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }
    
    private static ColourRepresentation CreateRepresentation(
        ColourSpace colourSpace, double first, double second, double third, 
        Configuration config, ColourHeritage heritage)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => new Rgb(first, second, third, heritage),
            ColourSpace.RgbLinear => new RgbLinear(first, second, third, heritage),
            ColourSpace.Hsb => new Hsb(first, second, third, heritage),
            ColourSpace.Hsl => new Hsl(first, second, third, heritage),
            ColourSpace.Hwb => new Hwb(first, second, third, heritage),
            ColourSpace.Xyz => new Xyz(first, second, third, heritage),
            ColourSpace.Xyy => new Xyy(first, second, third, heritage),
            ColourSpace.Lab => new Lab(first, second, third, heritage),
            ColourSpace.Lchab => new Lchab(first, second, third, heritage),
            ColourSpace.Luv => new Luv(first, second, third, heritage),
            ColourSpace.Lchuv => new Lchuv(first, second, third, heritage),
            ColourSpace.Hsluv => new Hsluv(first, second, third, heritage),
            ColourSpace.Hpluv => new Hpluv(first, second, third, heritage),
            ColourSpace.Ypbpr => new Ypbpr(first, second, third, heritage),
            ColourSpace.Ycbcr => new Ycbcr(first, second, third, heritage),
            ColourSpace.Ycgco => new Ycgco(first, second, third, heritage),
            ColourSpace.Yuv => new Yuv(first, second, third, heritage),
            ColourSpace.Yiq => new Yiq(first, second, third, heritage),
            ColourSpace.Ydbdr => new Ydbdr(first, second, third, heritage),
            ColourSpace.Ipt => new Ipt(first, second, third, heritage),
            ColourSpace.Ictcp => new Ictcp(first, second, third, heritage),
            ColourSpace.Jzazbz => new Jzazbz(first, second, third, heritage),
            ColourSpace.Jzczhz => new Jzczhz(first, second, third, heritage),
            ColourSpace.Oklab => new Oklab(first, second, third, heritage),
            ColourSpace.Oklch => new Oklch(first, second, third, heritage),
            ColourSpace.Cam02 => new Cam02(new Cam.Ucs(first, second, third), config.Cam, heritage),
            ColourSpace.Cam16 => new Cam16(new Cam.Ucs(first, second, third), config.Cam, heritage),
            ColourSpace.Hct => new Hct(first, second, third, heritage),
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
        return InitialColourSpace switch
        {
            ColourSpace.Rgb => (Rgb)InitialRepresentation,
            ColourSpace.Hsb => Hsb.ToRgb(Hsb),
            ColourSpace.Hsl => Hsb.ToRgb(Hsb),
            ColourSpace.Hwb => Hsb.ToRgb(Hsb),
            ColourSpace.Ypbpr => Ypbpr.ToRgb(Ypbpr, Config.Ybr),
            ColourSpace.Ycbcr => Ypbpr.ToRgb(Ypbpr, Config.Ybr),
            ColourSpace.Ycgco => Ycgco.ToRgb(Ycgco),
            ColourSpace.Yuv => Yuv.ToRgb(Yuv),
            ColourSpace.Yiq => Yuv.ToRgb(Yuv),
            ColourSpace.Ydbdr => Yuv.ToRgb(Yuv),
            _ => Rgb.FromRgbLinear(RgbLinear, Config.Rgb)
        };
    }
    
    private RgbLinear EvaluateRgbLinear()
    {
        return InitialColourSpace switch
        {
            ColourSpace.RgbLinear => (RgbLinear)InitialRepresentation,
            ColourSpace.Rgb => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            ColourSpace.Hsb => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            ColourSpace.Hsl => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            ColourSpace.Hwb => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            ColourSpace.Ypbpr => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            ColourSpace.Ycbcr => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            ColourSpace.Ycgco => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            ColourSpace.Yuv => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            ColourSpace.Yiq => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            ColourSpace.Ydbdr => Rgb.ToRgbLinear(Rgb, Config.Rgb),
            _ => RgbLinear.FromXyz(Xyz, Config.Rgb, Config.Xyz)
        };
    }

    private Hsb EvaluateHsb()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Hsb => (Hsb)InitialRepresentation,
            ColourSpace.Hsl => Hsl.ToHsb(Hsl),
            ColourSpace.Hwb => Hwb.ToHsb(Hwb),
            _ => Hsb.FromRgb(Rgb)
        };
    }

    private Hsl EvaluateHsl()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Hsl => (Hsl)InitialRepresentation,
            _ => Hsl.FromHsb(Hsb)
        };
    }

    private Hwb EvaluateHwb()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Hwb => (Hwb)InitialRepresentation,
            _ => Hwb.FromHsb(Hsb)
        };
    }

    private Xyz EvaluateXyz()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Xyz => (Xyz)InitialRepresentation,
            ColourSpace.Rgb => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.RgbLinear => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Hsb => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Hsl => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Hwb => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Xyy => Xyy.ToXyz(Xyy),
            ColourSpace.Lab => Lab.ToXyz(Lab, Config.Xyz),
            ColourSpace.Lchab => Lab.ToXyz(Lab, Config.Xyz),
            ColourSpace.Luv => Luv.ToXyz(Luv, Config.Xyz),
            ColourSpace.Lchuv => Luv.ToXyz(Luv, Config.Xyz),
            ColourSpace.Hsluv => Luv.ToXyz(Luv, Config.Xyz),
            ColourSpace.Hpluv => Luv.ToXyz(Luv, Config.Xyz),
            ColourSpace.Ypbpr => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Ycbcr => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Ycgco => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Yuv => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Yiq => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Ydbdr => RgbLinear.ToXyz(RgbLinear, Config.Rgb, Config.Xyz),
            ColourSpace.Ipt => Ipt.ToXyz(Ipt, Config.Xyz),
            ColourSpace.Ictcp => Ictcp.ToXyz(Ictcp, Config.IctcpScalar, Config.Xyz),
            ColourSpace.Jzazbz => Jzazbz.ToXyz(Jzazbz, Config.JzazbzScalar, Config.Xyz),
            ColourSpace.Jzczhz => Jzazbz.ToXyz(Jzazbz, Config.JzazbzScalar, Config.Xyz),
            ColourSpace.Oklab => Oklab.ToXyz(Oklab, Config.Xyz),
            ColourSpace.Oklch => Oklab.ToXyz(Oklab, Config.Xyz),
            ColourSpace.Cam02 => Cam02.ToXyz(Cam02, Config.Cam, Config.Xyz),
            ColourSpace.Cam16 => Cam16.ToXyz(Cam16, Config.Cam, Config.Xyz),
            ColourSpace.Hct => Hct.ToXyz(Hct, Config.Xyz),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Xyy EvaluateXyy()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Xyy => (Xyy)InitialRepresentation,
            _ => Xyy.FromXyz(Xyz, Config.Xyz.WhiteChromaticity)
        };
    }

    private Lab EvaluateLab()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Lab => (Lab)InitialRepresentation,
            ColourSpace.Lchab => Lchab.ToLab(Lchab),
            _ => Lab.FromXyz(Xyz, Config.Xyz)
        };
    }

    private Lchab EvaluateLchab()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Lchab => (Lchab)InitialRepresentation,
            _ => Lchab.FromLab(Lab)
        };
    }

    private Luv EvaluateLuv()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Luv => (Luv)InitialRepresentation,
            ColourSpace.Lchuv => Lchuv.ToLuv(Lchuv),
            ColourSpace.Hsluv => Lchuv.ToLuv(Lchuv),
            ColourSpace.Hpluv => Lchuv.ToLuv(Lchuv),
            _ => Luv.FromXyz(Xyz, Config.Xyz)
        };
    }

    private Lchuv EvaluateLchuv()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Lchuv => (Lchuv)InitialRepresentation,
            ColourSpace.Hsluv => Hsluv.ToLchuv(Hsluv),
            ColourSpace.Hpluv => Hpluv.ToLchuv(Hpluv),
            _ => Lchuv.FromLuv(Luv)
        };
    }

    private Hsluv EvaluateHsluv()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Hsluv => (Hsluv)InitialRepresentation,
            _ => Hsluv.FromLchuv(Lchuv)
        };
    }

    private Hpluv EvaluateHpluv()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Hpluv => (Hpluv)InitialRepresentation,
            _ => Hpluv.FromLchuv(Lchuv)
        };
    }
    
    private Ypbpr EvaluateYpbpr()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Ypbpr => (Ypbpr)InitialRepresentation,
            ColourSpace.Ycbcr => Ycbcr.ToYpbpr(Ycbcr, Config.Ybr),
            _ => Ypbpr.FromRgb(Rgb, Config.Ybr)
        };
    }
    
    private Ycbcr EvaluateYcbcr()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Ycbcr => (Ycbcr)InitialRepresentation,
            _ => Ycbcr.FromYpbpr(Ypbpr, Config.Ybr)
        };
    }
    
    private Ycgco EvaluateYcgco()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Ycgco => (Ycgco)InitialRepresentation,
            _ => Ycgco.FromRgb(Rgb)
        };
    }
    
    private Yuv EvaluateYuv()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Yuv => (Yuv)InitialRepresentation,
            ColourSpace.Yiq => Yiq.ToYuv(Yiq),
            ColourSpace.Ydbdr => Ydbdr.ToYuv(Ydbdr),
            _ => Yuv.FromRgb(Rgb)
        };
    }
    
    private Yiq EvaluateYiq()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Yiq => (Yiq)InitialRepresentation,
            _ => Yiq.FromYuv(Yuv)
        };
    }
    
    private Ydbdr EvaluateYdbdr()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Ydbdr => (Ydbdr)InitialRepresentation,
            _ => Ydbdr.FromYuv(Yuv)
        };
    }
    
    private Ipt EvaluateIpt()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Ipt => (Ipt)InitialRepresentation,
            _ => Ipt.FromXyz(Xyz, Config.Xyz)
        };
    }

    private Ictcp EvaluateIctcp()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Ictcp => (Ictcp)InitialRepresentation,
            _ => Ictcp.FromXyz(Xyz, Config.IctcpScalar, Config.Xyz)
        };
    }

    private Jzazbz EvaluateJzazbz()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Jzazbz => (Jzazbz)InitialRepresentation,
            ColourSpace.Jzczhz => Jzczhz.ToJzazbz(Jzczhz),
            _ => Jzazbz.FromXyz(Xyz, Config.JzazbzScalar, Config.Xyz)
        };
    }

    private Jzczhz EvaluateJzczhz()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Jzczhz => (Jzczhz)InitialRepresentation,
            _ => Jzczhz.FromJzazbz(Jzazbz)
        };
    }

    private Oklab EvaluateOklab()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Oklab => (Oklab)InitialRepresentation,
            ColourSpace.Oklch => Oklch.ToOklab(Oklch),
            _ => Oklab.FromXyz(Xyz, Config.Xyz)
        };
    }

    private Oklch EvaluateOklch()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Oklch => (Oklch)InitialRepresentation,
            _ => Oklch.FromOklab(Oklab)
        };
    }
    
    private Cam02 EvaluateCam02()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Cam02 => (Cam02)InitialRepresentation,
            _ => Cam02.FromXyz(Xyz, Config.Cam, Config.Xyz)
        };
    }
    
    private Cam16 EvaluateCam16()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Cam16 => (Cam16)InitialRepresentation,
            _ => Cam16.FromXyz(Xyz, Config.Cam, Config.Xyz)
        };
    }
    
    private Hct EvaluateHct()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Hct => (Hct)InitialRepresentation,
            _ => Hct.FromXyz(Xyz, Config.Xyz)
        };
    }
}