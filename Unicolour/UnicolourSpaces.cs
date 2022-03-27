namespace Wacton.Unicolour;

public partial class Unicolour
{
    private enum ColourSpace { Rgb, Hsb, Hsl, Xyz, Lab }

    private readonly Dictionary<ColourSpace, Dictionary<ColourSpace, Action>> conversions = new();

    private void SetupConversions()
    {
        conversions.Add(ColourSpace.Rgb, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.RgbToXyz(Rgb, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)}
        });
        
        conversions.Add(ColourSpace.Hsb, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.HsbToRgb(Hsb, Config)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.RgbToXyz(Rgb, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)}
        });
        
        conversions.Add(ColourSpace.Hsl, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.HsbToRgb(Hsb, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.HslToHsb(Hsl)},
            {ColourSpace.Xyz, () => xyz = Conversion.RgbToXyz(Rgb, Config)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)}
        });
        
        conversions.Add(ColourSpace.Xyz, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.XyzToRgb(Xyz, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Lab, () => lab = Conversion.XyzToLab(Xyz, Config)}
        });
        
        conversions.Add(ColourSpace.Lab, new Dictionary<ColourSpace, Action>
        {
            {ColourSpace.Rgb, () => rgb = Conversion.XyzToRgb(Xyz, Config)},
            {ColourSpace.Hsb, () => hsb = Conversion.RgbToHsb(Rgb)},
            {ColourSpace.Hsl, () => hsl = Conversion.HsbToHsl(Hsb)},
            {ColourSpace.Xyz, () => xyz = Conversion.LabToXyz(Lab, Config)}
        });
    }

    private T Get<T>(Func<T> get, ColourSpace targetSpace)
    {
        if (get() == null) conversions[initialSpace][targetSpace]();
        return get();
    }
}