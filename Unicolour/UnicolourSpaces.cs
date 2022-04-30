namespace Wacton.Unicolour;

public partial class Unicolour
{
    private enum ColourSpace { Rgb, Hsb, Hsl, Xyz, Lab, Lchab, Luv, Lchuv, Oklab, Oklch }

    private readonly Dictionary<ColourSpace, Dictionary<ColourSpace, Action>> conversions = new();

    private void SetupConversions()
    {
        conversions.Add(ColourSpace.Rgb, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.RgbToXyz(Rgb, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)},
            {ColourSpace.Lchab, () => lchab = Conversion.LabToLchab(Lab)},
            {ColourSpace.Luv, () => luv = Conversion.XyzToLuv(Xyz, Config)},
            {ColourSpace.Lchuv, () => lchuv = Conversion.LuvToLchuv(Luv)},
            {ColourSpace.Oklab, () => oklab = Conversion.XyzToOklab(Xyz, Config)},
            {ColourSpace.Oklch, () => oklch = Conversion.OklabToOklch(Oklab)}
        });
        
        conversions.Add(ColourSpace.Hsb, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.HsbToRgb(Hsb, Config)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.RgbToXyz(Rgb, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)},
            {ColourSpace.Lchab, () => lchab = Conversion.LabToLchab(Lab)},
            {ColourSpace.Luv, () => luv = Conversion.XyzToLuv(Xyz, Config)},
            {ColourSpace.Lchuv, () => lchuv = Conversion.LuvToLchuv(Luv)},
            {ColourSpace.Oklab, () => oklab = Conversion.XyzToOklab(Xyz, Config)},
            {ColourSpace.Oklch, () => oklch = Conversion.OklabToOklch(Oklab)}
        });
        
        conversions.Add(ColourSpace.Hsl, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.HsbToRgb(Hsb, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.HslToHsb(Hsl)},
            {ColourSpace.Xyz, () => xyz = Conversion.RgbToXyz(Rgb, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)},
            {ColourSpace.Lchab, () => lchab = Conversion.LabToLchab(Lab)},
            {ColourSpace.Luv, () => luv = Conversion.XyzToLuv(Xyz, Config)},
            {ColourSpace.Lchuv, () => lchuv = Conversion.LuvToLchuv(Luv)},
            {ColourSpace.Oklab, () => oklab = Conversion.XyzToOklab(Xyz, Config)},
            {ColourSpace.Oklch, () => oklch = Conversion.OklabToOklch(Oklab)}
        });
        
        conversions.Add(ColourSpace.Xyz, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.XyzToRgb(Xyz, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)},
            {ColourSpace.Lchab, () => lchab = Conversion.LabToLchab(Lab)},
            {ColourSpace.Luv, () => luv = Conversion.XyzToLuv(Xyz, Config)},
            {ColourSpace.Lchuv, () => lchuv = Conversion.LuvToLchuv(Luv)},
            {ColourSpace.Oklab, () => oklab = Conversion.XyzToOklab(Xyz, Config)},
            {ColourSpace.Oklch, () => oklch = Conversion.OklabToOklch(Oklab)}
        });
        
        conversions.Add(ColourSpace.Lab, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.XyzToRgb(Xyz, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.LabToXyz(Lab, Config)},
            {ColourSpace.Lchab, () => lchab = Conversion.LabToLchab(Lab)},
            {ColourSpace.Luv, () => luv = Conversion.XyzToLuv(Xyz, Config)},
            {ColourSpace.Lchuv, () => lchuv = Conversion.LuvToLchuv(Luv)},
            {ColourSpace.Oklab, () => oklab = Conversion.XyzToOklab(Xyz, Config)},
            {ColourSpace.Oklch, () => oklch = Conversion.OklabToOklch(Oklab)}
        });
        
        conversions.Add(ColourSpace.Lchab, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.XyzToRgb(Xyz, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.LabToXyz(Lab, Config)},
            {ColourSpace.Lab, () => lab = Conversion.LchabToLab(Lchab)},
            {ColourSpace.Luv, () => luv = Conversion.XyzToLuv(Xyz, Config)},
            {ColourSpace.Lchuv, () => lchuv = Conversion.LuvToLchuv(Luv)},
            {ColourSpace.Oklab, () => oklab = Conversion.XyzToOklab(Xyz, Config)},
            {ColourSpace.Oklch, () => oklch = Conversion.OklabToOklch(Oklab)}
        });
        
        conversions.Add(ColourSpace.Luv, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.XyzToRgb(Xyz, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.LuvToXyz(Luv, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)},
            {ColourSpace.Lchab, () => lchab = Conversion.LabToLchab(Lab)},
            {ColourSpace.Lchuv, () => lchuv = Conversion.LuvToLchuv(Luv)},
            {ColourSpace.Oklab, () => oklab = Conversion.XyzToOklab(Xyz, Config)},
            {ColourSpace.Oklch, () => oklch = Conversion.OklabToOklch(Oklab)}
        });
        
        conversions.Add(ColourSpace.Lchuv, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.XyzToRgb(Xyz, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.LuvToXyz(Luv, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)},
            {ColourSpace.Lchab, () => lchab = Conversion.LabToLchab(Lab)},
            {ColourSpace.Luv, () => luv = Conversion.LchuvToLuv(Lchuv)},
            {ColourSpace.Oklab, () => oklab = Conversion.XyzToOklab(Xyz, Config)},
            {ColourSpace.Oklch, () => oklch = Conversion.OklabToOklch(Oklab)}
        });
        
        conversions.Add(ColourSpace.Oklab, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.XyzToRgb(Xyz, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.OklabToXyz(Oklab, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)},
            {ColourSpace.Lchab, () => lchab = Conversion.LabToLchab(Lab)},
            {ColourSpace.Luv, () => luv = Conversion.XyzToLuv(Xyz, Config)},
            {ColourSpace.Lchuv, () => lchuv = Conversion.LuvToLchuv(Luv)},
            {ColourSpace.Oklch, () => oklch = Conversion.OklabToOklch(Oklab)}
        });
        
        conversions.Add(ColourSpace.Oklch, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.XyzToRgb(Xyz, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.OklabToXyz(Oklab, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)},
            {ColourSpace.Lchab, () => lchab = Conversion.LabToLchab(Lab)},
            {ColourSpace.Luv, () => luv = Conversion.XyzToLuv(Xyz, Config)},
            {ColourSpace.Lchuv, () => lchuv = Conversion.LuvToLchuv(Luv)},
            {ColourSpace.Oklab, () => oklab = Conversion.OklchToOklab(Oklch)}
        });
    }

    private T Get<T>(Func<T> get, ColourSpace targetSpace)
    {
        if (get() == null) conversions[initialSpace][targetSpace]();
        return get();
    }
}