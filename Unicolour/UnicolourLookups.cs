namespace Wacton.Unicolour;

internal enum ColourSpace { Rgb, RgbLinear, Rgb255, Hsb, Hsl, Hwb, Xyz, Xyy, Lab, Lchab, Luv, Lchuv, Hsluv, Hpluv, Cam16, Ictcp, Jzazbz, Jzczhz, Oklab, Oklch }

public partial class Unicolour
{
    /*
     * ColourSpace information needs to be held outwith ColourRepresentation object
     * otherwise the ColourRepresentation needs to be evaluated just to obtain the ColourSpace it represents
     */
    private static ColourSpace GetSpace<T>(T representation) where T : ColourRepresentation => TypeToSpace[representation.GetType()];
    private static readonly Dictionary<Type, ColourSpace> TypeToSpace = new()
    {
        { typeof(Rgb), ColourSpace.Rgb },
        { typeof(RgbLinear), ColourSpace.RgbLinear },
        { typeof(Rgb255), ColourSpace.Rgb255 },
        { typeof(Hsb), ColourSpace.Hsb },
        { typeof(Hsl), ColourSpace.Hsl },
        { typeof(Hwb), ColourSpace.Hwb },
        { typeof(Xyz), ColourSpace.Xyz },
        { typeof(Xyy), ColourSpace.Xyy },
        { typeof(Lab), ColourSpace.Lab },
        { typeof(Lchab), ColourSpace.Lchab },
        { typeof(Luv), ColourSpace.Luv },
        { typeof(Lchuv), ColourSpace.Lchuv },
        { typeof(Hsluv), ColourSpace.Hsluv },
        { typeof(Hpluv), ColourSpace.Hpluv },
        { typeof(Cam16), ColourSpace.Cam16 },
        { typeof(Ictcp), ColourSpace.Ictcp },
        { typeof(Jzazbz), ColourSpace.Jzazbz },
        { typeof(Jzczhz), ColourSpace.Jzczhz },
        { typeof(Oklab), ColourSpace.Oklab },
        { typeof(Oklch), ColourSpace.Oklch }
    };

    internal List<ColourRepresentation> GetRepresentations(List<ColourSpace> colourSpaces) => colourSpaces.Select(GetRepresentation).ToList();
    internal ColourRepresentation GetRepresentation(ColourSpace colourSpace)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => Rgb,
            ColourSpace.RgbLinear => Rgb.Linear,
            ColourSpace.Rgb255 => Rgb.Byte255,
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
            ColourSpace.Cam16 => Cam16,
            ColourSpace.Ictcp => Ictcp,
            ColourSpace.Jzazbz => Jzazbz,
            ColourSpace.Jzczhz => Jzczhz,
            ColourSpace.Oklab => Oklab,
            ColourSpace.Oklch => Oklch,
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }

    /*
     * getting a value will trigger a chain of gets and conversions if the intermediary values have not been calculated yet
     * e.g. if Unicolour is created from RGB, and the first request is for LAB:
     * - Get(ColourSpace.Lab); lab is null, execute: lab = Lab.FromXyz(Xyz, Config)
     * - Get(ColourSpace.Xyz); xyz is null, execute: xyz = Rgb.ToXyz(Rgb, Config)
     * - Get(ColourSpace.Rgb); rgb is not null, return value
     * - xyz is evaluated from rgb and stored
     * - lab is evaluated from xyz and stored
     */
    private T Get<T>(ColourSpace targetSpace) where T : ColourRepresentation
    {
        var backingRepresentation = GetBackingField(targetSpace);
        if (backingRepresentation == null)
        {
            SetBackingField(targetSpace);
            backingRepresentation = GetBackingField(targetSpace);
        }

        return (backingRepresentation as T)!;
    }

    private ColourRepresentation? GetBackingField(ColourSpace colourSpace)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => rgb,
            ColourSpace.Hsb => hsb,
            ColourSpace.Hsl => hsl,
            ColourSpace.Hwb => hwb,
            ColourSpace.Xyz => xyz,
            ColourSpace.Xyy => xyy,
            ColourSpace.Lab => lab,
            ColourSpace.Lchab => lchab,
            ColourSpace.Luv => luv,
            ColourSpace.Lchuv => lchuv,
            ColourSpace.Hsluv => hsluv,
            ColourSpace.Hpluv => hpluv,
            ColourSpace.Cam16 => cam16,
            ColourSpace.Ictcp => ictcp,
            ColourSpace.Jzazbz => jzazbz,
            ColourSpace.Jzczhz => jzczhz,
            ColourSpace.Oklab => oklab,
            ColourSpace.Oklch => oklch,
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }

    private void SetBackingField(ColourSpace targetSpace)
    {
        Action setField = targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = EvaluateRgb(),
            ColourSpace.Hsb => () => hsb = EvaluateHsb(),
            ColourSpace.Hsl => () => hsl = EvaluateHsl(),
            ColourSpace.Hwb => () => hwb = EvaluateHwb(),
            ColourSpace.Xyz => () => xyz = EvaluateXyz(),
            ColourSpace.Xyy => () => xyy = EvaluateXyy(),
            ColourSpace.Lab => () => lab = EvaluateLab(),
            ColourSpace.Lchab => () => lchab = EvaluateLchab(),
            ColourSpace.Luv => () => luv = EvaluateLuv(),
            ColourSpace.Lchuv => () => lchuv = EvaluateLchuv(),
            ColourSpace.Hsluv => () => hsluv = EvaluateHsluv(),
            ColourSpace.Hpluv => () => hpluv = EvaluateHpluv(),
            ColourSpace.Cam16 => () => cam16 = EvaluateCam16(),
            ColourSpace.Ictcp => () => ictcp = EvaluateIctcp(),
            ColourSpace.Jzazbz => () => jzazbz = EvaluateJzazbz(),
            ColourSpace.Jzczhz => () => jzczhz = EvaluateJzczhz(),
            ColourSpace.Oklab => () => oklab = EvaluateOklab(),
            ColourSpace.Oklch => () => oklch = EvaluateOklch(),
            _ => throw new ArgumentOutOfRangeException(nameof(targetSpace), targetSpace, null)
        };

        setField();
    }
    
    /*
     * evaluation method switch expressions are arranged as follows:
     * - first item     = target space is the initial space, simply return initial representation
     * - middle items   = reverse transforms to the target space; only the immediate transforms (e.g. RGB <- HSB <- HSL)
     * - default item   = forward transform from a base space
     * -----------------
     * only need to consider the immediate transforms, as subsequent transforms are handled recursively
     * e.g. if target is RGB:
     * reverse starting at HSL = RGB <- HSB <- HSL; use RGB <- HSB [Hsb.ToRgb]
     * forward starting at LAB = LAB -> XYZ -> RGB; use XYZ -> RGB [Rgb.FromXyz]
     */

    private Rgb EvaluateRgb()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Rgb => (Rgb)InitialRepresentation,
            ColourSpace.Hsb => Hsb.ToRgb(Hsb, Config.Rgb),
            ColourSpace.Hsl => Hsb.ToRgb(Hsb, Config.Rgb),
            ColourSpace.Hwb => Hsb.ToRgb(Hsb, Config.Rgb),
            _ => Rgb.FromXyz(Xyz, Config.Rgb, Config.Xyz)
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
            ColourSpace.Rgb => Rgb.ToXyz(Rgb, Config.Rgb, Config.Xyz),
            ColourSpace.Hsb => Rgb.ToXyz(Rgb, Config.Rgb, Config.Xyz),
            ColourSpace.Hsl => Rgb.ToXyz(Rgb, Config.Rgb, Config.Xyz),
            ColourSpace.Hwb => Rgb.ToXyz(Rgb, Config.Rgb, Config.Xyz),
            ColourSpace.Xyy => Xyy.ToXyz(Xyy),
            ColourSpace.Lab => Lab.ToXyz(Lab, Config.Xyz),
            ColourSpace.Lchab => Lab.ToXyz(Lab, Config.Xyz),
            ColourSpace.Luv => Luv.ToXyz(Luv, Config.Xyz),
            ColourSpace.Lchuv => Luv.ToXyz(Luv, Config.Xyz),
            ColourSpace.Hsluv => Luv.ToXyz(Luv, Config.Xyz),
            ColourSpace.Hpluv => Luv.ToXyz(Luv, Config.Xyz),
            ColourSpace.Cam16 => Cam16.ToXyz(Cam16, Config.Cam16, Config.Xyz),
            ColourSpace.Ictcp => Ictcp.ToXyz(Ictcp, Config.IctcpScalar, Config.Xyz),
            ColourSpace.Jzazbz => Jzazbz.ToXyz(Jzazbz, Config.JzazbzScalar, Config.Xyz),
            ColourSpace.Jzczhz => Jzazbz.ToXyz(Jzazbz, Config.JzazbzScalar, Config.Xyz),
            ColourSpace.Oklab => Oklab.ToXyz(Oklab, Config.Xyz),
            ColourSpace.Oklch => Oklab.ToXyz(Oklab, Config.Xyz),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Xyy EvaluateXyy()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Xyy => (Xyy)InitialRepresentation,
            _ => Xyy.FromXyz(Xyz, Config.Xyz)
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
    
    private Cam16 EvaluateCam16()
    {
        return InitialColourSpace switch
        {
            ColourSpace.Cam16 => (Cam16)InitialRepresentation,
            _ => Cam16.FromXyz(Xyz, Config.Cam16, Config.Xyz)
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
}