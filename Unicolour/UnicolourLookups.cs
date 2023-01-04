namespace Wacton.Unicolour;

internal enum ColourSpace { Rgb, RgbLinear, Rgb255, Hsb, Hsl, Xyz, Xyy, Lab, Lchab, Luv, Lchuv, Hsluv, Hpluv, Jzazbz, Jzczhz, Oklab, Oklch }

public partial class Unicolour
{
    internal IEnumerable<ColourRepresentation> AllRepresentations => new List<ColourRepresentation>
        {Rgb, Hsb, Hsl, Xyz, Xyy, Lab, Lchab, Luv, Lchuv, Hsluv, Hpluv, Jzazbz, Jzczhz, Oklab, Oklch};

    internal ColourRepresentation Representation(ColourSpace colourSpace) => AllRepresentations.Single(x => x.ColourSpace == colourSpace);
    internal ColourRepresentation InitialRepresentation() => Representation(initialColourSpace);
    
    private void SetInitialRepresentation(ColourRepresentation colourRepresentation)
    {
        switch (colourRepresentation)
        {
            case Rgb rgbColour: rgb = rgbColour; break;
            case Hsb hsbColour: hsb = hsbColour; break;
            case Hsl hslColour: hsl = hslColour; break;
            case Xyz xyzColour: xyz = xyzColour; break;
            case Xyy xyyColour: xyy = xyyColour; break;
            case Lab labColour: lab = labColour; break;
            case Lchab lchabColour: lchab = lchabColour; break;
            case Luv luvColour: luv = luvColour; break;
            case Lchuv lchuvColour: lchuv = lchuvColour; break;
            case Hsluv hsluvColour: hsluv = hsluvColour; break;
            case Hpluv hpluvColour: hpluv = hpluvColour; break;
            case Jzazbz jzazbzColour: jzazbz = jzazbzColour; break;
            case Jzczhz jzczhzColour: jzczhz = jzczhzColour; break;
            case Oklab oklabColour: oklab = oklabColour; break;
            case Oklch oklchColour: oklch = oklchColour; break;
            default: throw new ArgumentOutOfRangeException(nameof(colourRepresentation));
        }
    }
    
    /*
     * getting a value will trigger a chain of gets and conversions if the intermediary values have not been calculated yet
     * e.g. if Unicolour is created from RGB, and the first request is for LAB:
     * - Get(ColourSpace.Lab); lab is null, execute: lab = Conversion.XyzToLab(Xyz, Config)
     * - Get(ColourSpace.Xyz); xyz is null, execute: xyz = Conversion.RgbToXyz(Rgb, Config)
     * - Get(ColourSpace.Rgb); rgb is not null, return value
     * - xyz is evaluated from rgb and stored
     * - lab is evaluated from xyz and stored
     */
    private T Get<T>(ColourSpace targetSpace) where T : ColourRepresentation
    {
        var colourRepresentation = GetBackingRepresentation(targetSpace);
        if (colourRepresentation == null)
        {
            SetBackingRepresentation(targetSpace);
            colourRepresentation = GetBackingRepresentation(targetSpace);
        }

        return (colourRepresentation as T)!;
    }
    
    private ColourRepresentation? GetBackingRepresentation(ColourSpace colourSpace)
    {
        return colourSpace switch
        {
            ColourSpace.Rgb => rgb,
            ColourSpace.Hsb => hsb,
            ColourSpace.Hsl => hsl,
            ColourSpace.Xyz => xyz,
            ColourSpace.Xyy => xyy,
            ColourSpace.Lab => lab,
            ColourSpace.Lchab => lchab,
            ColourSpace.Luv => luv,
            ColourSpace.Lchuv => lchuv,
            ColourSpace.Hsluv => hsluv,
            ColourSpace.Hpluv => hpluv,
            ColourSpace.Jzazbz => jzazbz,
            ColourSpace.Jzczhz => jzczhz,
            ColourSpace.Oklab => oklab,
            ColourSpace.Oklch => oklch,
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }

    private void SetBackingRepresentation(ColourSpace targetSpace)
    {
        var setValueAction = initialColourSpace switch
        {
            ColourSpace.Rgb => SetFromRgb(targetSpace),
            ColourSpace.Hsb => SetFromHsb(targetSpace),
            ColourSpace.Hsl => SetFromHsl(targetSpace),
            ColourSpace.Xyz => SetFromXyz(targetSpace),
            ColourSpace.Xyy => SetFromXyy(targetSpace),
            ColourSpace.Lab => SetFromLab(targetSpace),
            ColourSpace.Lchab => SetFromLchab(targetSpace),
            ColourSpace.Luv => SetFromLuv(targetSpace),
            ColourSpace.Lchuv => SetFromLchuv(targetSpace),
            ColourSpace.Hsluv => SetFromHsluv(targetSpace),
            ColourSpace.Hpluv => SetFromHpluv(targetSpace),
            ColourSpace.Jzazbz => SetFromJzazbz(targetSpace),
            ColourSpace.Jzczhz => SetFromJzczhz(targetSpace),
            ColourSpace.Oklab => SetFromOklab(targetSpace),
            ColourSpace.Oklch => SetFromOklch(targetSpace),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        setValueAction();
    }
    
    private Action SetFromRgb(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            // ColourSpace.Rgb => () => { },
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.RgbToXyz(Rgb, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Action SetFromHsb(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.HsbToRgb(Hsb, Config),
            // ColourSpace.Hsb => () => { },
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.RgbToXyz(Rgb, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromHsl(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.HsbToRgb(Hsb, Config),
            ColourSpace.Hsb => () => hsb = Conversion.HslToHsb(Hsl),
            // ColourSpace.Hsl => () => { },
            ColourSpace.Xyz => () => xyz = Conversion.RgbToXyz(Rgb, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromXyz(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            // ColourSpace.Xyz => () => { },
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromXyy(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.XyyToXyz(Xyy),
            // ColourSpace.Xyy => () => { },
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromLab(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.LabToXyz(Lab, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            // ColourSpace.Lab => () => { },
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromLchab(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.LabToXyz(Lab, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.LchabToLab(Lchab),
            // ColourSpace.Lchab => () => { },
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromLuv(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.LuvToXyz(Luv, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            // ColourSpace.Luv => () => { },
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromLchuv(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.LuvToXyz(Luv, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.LchuvToLuv(Lchuv),
            // ColourSpace.Lchuv => () => { },
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromHsluv(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.LuvToXyz(Luv, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.LchuvToLuv(Lchuv),
            ColourSpace.Lchuv => () => lchuv = Conversion.HsluvToLchuv(Hsluv),
            // ColourSpace.Hsluv => () => { },
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromHpluv(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.LuvToXyz(Luv, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.LchuvToLuv(Lchuv),
            ColourSpace.Lchuv => () => lchuv = Conversion.HpluvToLchuv(Hpluv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            // ColourSpace.Hpluv => () => { },
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromJzazbz(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.JzazbzToXyz(Jzazbz, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            // ColourSpace.Jzazbz => () => { },
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromJzczhz(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.JzazbzToXyz(Jzazbz, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.JzczhzToJzazbz(Jzczhz),
            // ColourSpace.Jzczhz => () => { },
            ColourSpace.Oklab => () => oklab = Conversion.XyzToOklab(Xyz, Config),
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromOklab(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.OklabToXyz(Oklab, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            // ColourSpace.Oklab => () => { },
            ColourSpace.Oklch => () => oklch = Conversion.OklabToOklch(Oklab),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Action SetFromOklch(ColourSpace targetSpace)
    {
        return targetSpace switch
        {
            ColourSpace.Rgb => () => rgb = Conversion.XyzToRgb(Xyz, Config),
            ColourSpace.Hsb => () => hsb = Conversion.RgbToHsb(Rgb),
            ColourSpace.Hsl => () => hsl = Conversion.HsbToHsl(Hsb),
            ColourSpace.Xyz => () => xyz = Conversion.OklabToXyz(Oklab, Config),
            ColourSpace.Xyy => () => xyy = Conversion.XyzToXyy(Xyz, Config),
            ColourSpace.Lab => () => lab = Conversion.XyzToLab(Xyz, Config),
            ColourSpace.Lchab => () => lchab = Conversion.LabToLchab(Lab),
            ColourSpace.Luv => () => luv = Conversion.XyzToLuv(Xyz, Config),
            ColourSpace.Lchuv => () => lchuv = Conversion.LuvToLchuv(Luv),
            ColourSpace.Hsluv => () => hsluv = Conversion.LchuvToHsluv(Lchuv),
            ColourSpace.Hpluv => () => hpluv = Conversion.LchuvToHpluv(Lchuv),
            ColourSpace.Jzazbz => () => jzazbz = Conversion.XyzToJzazbz(Xyz, Config),
            ColourSpace.Jzczhz => () => jzczhz = Conversion.JzazbzToJzczhz(Jzazbz),
            ColourSpace.Oklab => () => oklab = Conversion.OklchToOklab(Oklch),
            // ColourSpace.Oklch => () => { },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}