namespace Wacton.Unicolour.Datasets;

public static class Macbeth
{
    /*
     * table at https://xritephoto.com/documents/literature/en/ColorData-1p_EN.pdf references LAB using D50
     * but their LAB values appear to be a bit off, unclear why (perhaps an unusual chromatic adaptation method?)
     * ----------
     * there is also https://poynton.ca/notes/color/GretagMacbeth-ColorChecker.html containing apparent Illuminant C xyY data
     * which is even further removed from the manufacturer's sRGB values
     */
    private static readonly XyzConfiguration XyzConfig = new(WhitePoint.From(Illuminant.D50));
    private static readonly Configuration Config = new(RgbConfiguration.StandardRgb, XyzConfig);
    
    public static readonly Unicolour DarkSkin = new(ColourSpace.Rgb255, Config, 115, 82, 68);
    public static readonly Unicolour LightSkin = new(ColourSpace.Rgb255, Config, 194, 150, 130);
    public static readonly Unicolour BlueSky = new(ColourSpace.Rgb255, Config, 98, 122, 157);
    public static readonly Unicolour Foliage = new(ColourSpace.Rgb255, Config, 87, 108, 67);
    public static readonly Unicolour BlueFlower = new(ColourSpace.Rgb255, Config, 133, 128, 177);
    public static readonly Unicolour BluishGreen = new(ColourSpace.Rgb255, Config, 103, 189, 170);
    public static readonly Unicolour Orange = new(ColourSpace.Rgb255, Config, 214, 126, 44);
    public static readonly Unicolour PurplishBlue = new(ColourSpace.Rgb255, Config, 80, 91, 166);
    public static readonly Unicolour ModerateRed = new(ColourSpace.Rgb255, Config, 193, 90, 99);
    public static readonly Unicolour Purple = new(ColourSpace.Rgb255, Config, 94, 60, 108);
    public static readonly Unicolour YellowGreen = new(ColourSpace.Rgb255, Config, 157, 188, 64);
    public static readonly Unicolour OrangeYellow = new(ColourSpace.Rgb255, Config, 224, 163, 46);
    public static readonly Unicolour Blue = new(ColourSpace.Rgb255, Config, 56, 61, 150);
    public static readonly Unicolour Green = new(ColourSpace.Rgb255, Config, 70, 148, 73);
    public static readonly Unicolour Red = new(ColourSpace.Rgb255, Config, 175, 54, 60);
    public static readonly Unicolour Yellow = new(ColourSpace.Rgb255, Config, 231, 199, 31);
    public static readonly Unicolour Magenta = new(ColourSpace.Rgb255, Config, 187, 86, 149);
    public static readonly Unicolour Cyan = new(ColourSpace.Rgb255, Config, 8, 133, 161);
    public static readonly Unicolour White = new(ColourSpace.Rgb255, Config, 243, 243, 242);
    public static readonly Unicolour Neutral8 = new(ColourSpace.Rgb255, Config, 200, 200, 200);
    public static readonly Unicolour Neutral65 = new(ColourSpace.Rgb255, Config, 160, 160, 160);
    public static readonly Unicolour Neutral5 = new(ColourSpace.Rgb255, Config, 122, 122, 121);
    public static readonly Unicolour Neutral35 = new(ColourSpace.Rgb255, Config, 85, 85, 85);
    public static readonly Unicolour Black = new(ColourSpace.Rgb255, Config, 52, 52, 52);
    
    public static readonly List<Unicolour> Natural = new() { DarkSkin, LightSkin, BlueSky, Foliage, BlueFlower, BluishGreen };
    public static readonly List<Unicolour> Miscellaneous = new() { Orange, PurplishBlue, ModerateRed, Purple, YellowGreen, OrangeYellow };
    public static readonly List<Unicolour> PrimaryAndSecondary = new() { Blue, Green, Red, Yellow, Magenta, Cyan };
    public static readonly List<Unicolour> Greyscale = new() { White, Neutral8, Neutral65, Neutral5, Neutral35, Black };

    public static IEnumerable<Unicolour> All => new List<Unicolour>()
        .Concat(Natural)
        .Concat(Miscellaneous)
        .Concat(PrimaryAndSecondary)
        .Concat(Greyscale);
}