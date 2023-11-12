namespace Wacton.Unicolour.Datasets;

public static class Css
{
    /*
     * NOTE: cannot currently provide Unicolours that behave exactly to the CSS specification
     * - CSS 'lab' & 'lch' uses D50 illuminant
     * - CSS 'xyz' uses D65 illuminant
     * - Unicolour uses illuminant from XyzConfig for both LAB & XYZ, so by default LAB values will appear different
     * could add dedicated `LabConfiguration` easily enough if it became desirable,
     * but until then it's easy enough to use `.ConvertToConfiguration()` to get the expected values
     * (see PremultipliedAlphaTests.cs for examples)
     */
    private static readonly Configuration Config = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65);

    public static readonly Unicolour AliceBlue = new(Config, "#F0F8FF");
    public static readonly Unicolour AntiqueWhite = new(Config, "#FAEBD7");
    public static readonly Unicolour Aqua = new(Config, "#00FFFF");
    public static readonly Unicolour Aquamarine = new(Config, "#7FFFD4");
    public static readonly Unicolour Azure = new(Config, "#F0FFFF");
    public static readonly Unicolour Beige = new(Config, "#F5F5DC");
    public static readonly Unicolour Bisque = new(Config, "#FFE4C4");
    public static readonly Unicolour Black = new(Config, "#000000");
    public static readonly Unicolour BlanchedAlmond = new(Config, "#FFEBCD");
    public static readonly Unicolour Blue = new(Config, "#0000FF");
    public static readonly Unicolour BlueViolet = new(Config, "#8A2BE2");
    public static readonly Unicolour Brown = new(Config, "#A52A2A");
    public static readonly Unicolour Burlywood = new(Config, "#DEB887");
    public static readonly Unicolour CadetBlue = new(Config, "#5F9EA0");
    public static readonly Unicolour Chartreuse = new(Config, "#7FFF00");
    public static readonly Unicolour Chocolate = new(Config, "#D2691E");
    public static readonly Unicolour Coral = new(Config, "#FF7F50");
    public static readonly Unicolour CornflowerBlue = new(Config, "#6495ED");
    public static readonly Unicolour Cornsilk = new(Config, "#FFF8DC");
    public static readonly Unicolour Crimson = new(Config, "#DC143C");
    public static readonly Unicolour Cyan = new(Config, "#00FFFF");
    public static readonly Unicolour DarkBlue = new(Config, "#00008B");
    public static readonly Unicolour DarkCyan = new(Config, "#008B8B");
    public static readonly Unicolour DarkGoldenrod = new(Config, "#B8860B");
    public static readonly Unicolour DarkGray = new(Config, "#A9A9A9");
    public static readonly Unicolour DarkGreen = new(Config, "#006400");
    public static readonly Unicolour DarkGrey = new(Config, "#A9A9A9");
    public static readonly Unicolour DarkKhaki = new(Config, "#BDB76B");
    public static readonly Unicolour DarkMagenta = new(Config, "#8B008B");
    public static readonly Unicolour DarkOliveGreen = new(Config, "#556B2F");
    public static readonly Unicolour DarkOrange = new(Config, "#FF8C00");
    public static readonly Unicolour DarkOrchid = new(Config, "#9932CC");
    public static readonly Unicolour DarkRed = new(Config, "#8B0000");
    public static readonly Unicolour DarkSalmon = new(Config, "#E9967A");
    public static readonly Unicolour DarkSeaGreen = new(Config, "#8FBC8F");
    public static readonly Unicolour DarkSlateBlue = new(Config, "#483D8B");
    public static readonly Unicolour DarkSlateGray = new(Config, "#2F4F4F");
    public static readonly Unicolour DarkSlateGrey = new(Config, "#2F4F4F");
    public static readonly Unicolour DarkTurquoise = new(Config, "#00CED1");
    public static readonly Unicolour DarkViolet = new(Config, "#9400D3");
    public static readonly Unicolour DeepPink = new(Config, "#FF1493");
    public static readonly Unicolour DeepSkyBlue = new(Config, "#00BFFF");
    public static readonly Unicolour DimGray = new(Config, "#696969");
    public static readonly Unicolour DimGrey = new(Config, "#696969");
    public static readonly Unicolour DodgerBlue = new(Config, "#1E90FF");
    public static readonly Unicolour FireBrick = new(Config, "#B22222");
    public static readonly Unicolour FloralWhite = new(Config, "#FFFAF0");
    public static readonly Unicolour ForestGreen = new(Config, "#228B22");
    public static readonly Unicolour Fuchsia = new(Config, "#FF00FF");
    public static readonly Unicolour Gainsboro = new(Config, "#DCDCDC");
    public static readonly Unicolour GhostWhite = new(Config, "#F8F8FF");
    public static readonly Unicolour Gold = new(Config, "#FFD700");
    public static readonly Unicolour Goldenrod = new(Config, "#DAA520");
    public static readonly Unicolour Gray = new(Config, "#808080");
    public static readonly Unicolour Green = new(Config, "#008000");
    public static readonly Unicolour GreenYellow = new(Config, "#ADFF2F");
    public static readonly Unicolour Grey = new(Config, "#808080");
    public static readonly Unicolour Honeydew = new(Config, "#F0FFF0");
    public static readonly Unicolour HotPink = new(Config, "#FF69B4");
    public static readonly Unicolour IndianRed = new(Config, "#CD5C5C");
    public static readonly Unicolour Indigo = new(Config, "#4B0082");
    public static readonly Unicolour Ivory = new(Config, "#FFFFF0");
    public static readonly Unicolour Khaki = new(Config, "#F0E68C");
    public static readonly Unicolour Lavender = new(Config, "#E6E6FA");
    public static readonly Unicolour LavenderBlush = new(Config, "#FFF0F5");
    public static readonly Unicolour LawnGreen = new(Config, "#7CFC00");
    public static readonly Unicolour LemonChiffon = new(Config, "#FFFACD");
    public static readonly Unicolour LightBlue = new(Config, "#ADD8E6");
    public static readonly Unicolour LightCoral = new(Config, "#F08080");
    public static readonly Unicolour LightCyan = new(Config, "#E0FFFF");
    public static readonly Unicolour LightGoldenrodYellow = new(Config, "#FAFAD2");
    public static readonly Unicolour LightGray = new(Config, "#D3D3D3");
    public static readonly Unicolour LightGreen = new(Config, "#90EE90");
    public static readonly Unicolour LightGrey = new(Config, "#D3D3D3");
    public static readonly Unicolour LightPink = new(Config, "#FFB6C1");
    public static readonly Unicolour LightSalmon = new(Config, "#FFA07A");
    public static readonly Unicolour LightSeaGreen = new(Config, "#20B2AA");
    public static readonly Unicolour LightSkyBlue = new(Config, "#87CEFA");
    public static readonly Unicolour LightSlateGray = new(Config, "#778899");
    public static readonly Unicolour LightSlateGrey = new(Config, "#778899");
    public static readonly Unicolour LightSteelBlue = new(Config, "#B0C4DE");
    public static readonly Unicolour LightYellow = new(Config, "#FFFFE0");
    public static readonly Unicolour Lime = new(Config, "#00FF00");
    public static readonly Unicolour LimeGreen = new(Config, "#32CD32");
    public static readonly Unicolour Linen = new(Config, "#FAF0E6");
    public static readonly Unicolour Magenta = new(Config, "#FF00FF");
    public static readonly Unicolour Maroon = new(Config, "#800000");
    public static readonly Unicolour MediumAquamarine = new(Config, "#66CDAA");
    public static readonly Unicolour MediumBlue = new(Config, "#0000CD");
    public static readonly Unicolour MediumOrchid = new(Config, "#BA55D3");
    public static readonly Unicolour MediumPurple = new(Config, "#9370DB");
    public static readonly Unicolour MediumSeaGreen = new(Config, "#3CB371");
    public static readonly Unicolour MediumSlateBlue = new(Config, "#7B68EE");
    public static readonly Unicolour MediumSpringGreen = new(Config, "#00FA9A");
    public static readonly Unicolour MediumTurquoise = new(Config, "#48D1CC");
    public static readonly Unicolour MediumVioletRed = new(Config, "#C71585");
    public static readonly Unicolour MidnightBlue = new(Config, "#191970");
    public static readonly Unicolour MintCream = new(Config, "#F5FFFA");
    public static readonly Unicolour MistyRose = new(Config, "#FFE4E1");
    public static readonly Unicolour Moccasin = new(Config, "#FFE4B5");
    public static readonly Unicolour NavajoWhite = new(Config, "#FFDEAD");
    public static readonly Unicolour Navy = new(Config, "#000080");
    public static readonly Unicolour OldLace = new(Config, "#FDF5E6");
    public static readonly Unicolour Olive = new(Config, "#808000");
    public static readonly Unicolour OliveDrab = new(Config, "#6B8E23");
    public static readonly Unicolour Orange = new(Config, "#FFA500");
    public static readonly Unicolour OrangeRed = new(Config, "#FF4500");
    public static readonly Unicolour Orchid = new(Config, "#DA70D6");
    public static readonly Unicolour PaleGoldenrod = new(Config, "#EEE8AA");
    public static readonly Unicolour PaleGreen = new(Config, "#98FB98");
    public static readonly Unicolour PaleTurquoise = new(Config, "#AFEEEE");
    public static readonly Unicolour PaleVioletRed = new(Config, "#DB7093");
    public static readonly Unicolour PapayaWhip = new(Config, "#FFEFD5");
    public static readonly Unicolour PeachPuff = new(Config, "#FFDAB9");
    public static readonly Unicolour Peru = new(Config, "#CD853F");
    public static readonly Unicolour Pink = new(Config, "#FFC0CB");
    public static readonly Unicolour Plum = new(Config, "#DDA0DD");
    public static readonly Unicolour PowderBlue = new(Config, "#B0E0E6");
    public static readonly Unicolour Purple = new(Config, "#800080");
    public static readonly Unicolour RebeccaPurple = new(Config, "#663399");
    public static readonly Unicolour Red = new(Config, "#FF0000");
    public static readonly Unicolour RosyBrown = new(Config, "#BC8F8F");
    public static readonly Unicolour RoyalBlue = new(Config, "#4169E1");
    public static readonly Unicolour SaddleBrown = new(Config, "#8B4513");
    public static readonly Unicolour Salmon = new(Config, "#FA8072");
    public static readonly Unicolour SandyBrown = new(Config, "#F4A460");
    public static readonly Unicolour SeaGreen = new(Config, "#2E8B57");
    public static readonly Unicolour Seashell = new(Config, "#FFF5EE");
    public static readonly Unicolour Sienna = new(Config, "#A0522D");
    public static readonly Unicolour Silver = new(Config, "#C0C0C0");
    public static readonly Unicolour SkyBlue = new(Config, "#87CEEB");
    public static readonly Unicolour SlateBlue = new(Config, "#6A5ACD");
    public static readonly Unicolour SlateGray = new(Config, "#708090");
    public static readonly Unicolour SlateGrey = new(Config, "#708090");
    public static readonly Unicolour Snow = new(Config, "#FFFAFA");
    public static readonly Unicolour SpringGreen = new(Config, "#00FF7F");
    public static readonly Unicolour SteelBlue = new(Config, "#4682B4");
    public static readonly Unicolour Tan = new(Config, "#D2B48C");
    public static readonly Unicolour Teal = new(Config, "#008080");
    public static readonly Unicolour Thistle = new(Config, "#D8BFD8");
    public static readonly Unicolour Tomato = new(Config, "#FF6347");
    public static readonly Unicolour Turquoise = new(Config, "#40E0D0");
    public static readonly Unicolour Violet = new(Config, "#EE82EE");
    public static readonly Unicolour Wheat = new(Config, "#F5DEB3");
    public static readonly Unicolour White = new(Config, "#FFFFFF");
    public static readonly Unicolour WhiteSmoke = new(Config, "#F5F5F5");
    public static readonly Unicolour Yellow = new(Config, "#FFFF00");
    public static readonly Unicolour YellowGreen = new(Config, "#9ACD32");

    // TODO: hex constructor that takes hex with 0-1 alpha
    // (will need to handle alpha present in both hex string and alpha param)
    public static readonly Unicolour Transparent = new(Config, "#00000000");
    
    public static IEnumerable<Unicolour> All => new List<Unicolour>
    {
        AliceBlue, AntiqueWhite, Aqua, Aquamarine, Azure,
        Beige, Bisque, Black, BlanchedAlmond, Blue, BlueViolet, Brown, Burlywood,
        CadetBlue, Chartreuse, Chocolate, Coral, CornflowerBlue, Cornsilk, Crimson, Cyan,
        DarkBlue, DarkCyan, DarkGoldenrod, DarkGray, DarkGreen, DarkGrey, DarkKhaki, DarkMagenta, DarkOliveGreen, DarkOrange, DarkOrchid, DarkRed, DarkSalmon, DarkSeaGreen, DarkSlateBlue, DarkSlateGray, DarkSlateGrey, DarkTurquoise, DarkViolet, DeepPink, DeepSkyBlue, DimGray, DimGrey, DodgerBlue,
        FireBrick, FloralWhite, ForestGreen, Fuchsia,
        Gainsboro, GhostWhite, Gold, Goldenrod, Gray, Green, GreenYellow, Grey,
        Honeydew, HotPink,
        IndianRed, Indigo, Ivory,
        Khaki,
        Lavender, LavenderBlush, LawnGreen, LemonChiffon, LightBlue, LightCoral, LightCyan, LightGoldenrodYellow, LightGray, LightGreen, LightGrey, LightPink, LightSalmon, LightSeaGreen, LightSkyBlue, LightSlateGray, LightSlateGrey, LightSteelBlue, LightYellow, Lime, LimeGreen, Linen,
        Magenta, Maroon, MediumAquamarine, MediumBlue, MediumOrchid, MediumPurple, MediumSeaGreen, MediumSlateBlue, MediumSpringGreen, MediumTurquoise, MediumVioletRed, MidnightBlue, MintCream, MistyRose, Moccasin,
        NavajoWhite, Navy,
        OldLace, Olive, OliveDrab, Orange, OrangeRed, Orchid,
        PaleGoldenrod, PaleGreen, PaleTurquoise, PaleVioletRed, PapayaWhip, PeachPuff, Peru, Pink, Plum, PowderBlue, Purple,
        RebeccaPurple, Red, RosyBrown, RoyalBlue,
        SaddleBrown, Salmon, SandyBrown, SeaGreen, Seashell, Sienna, Silver, SkyBlue, SlateBlue, SlateGray, SlateGrey, Snow, SpringGreen, SteelBlue,
        Tan, Teal, Thistle, Tomato, Turquoise,
        Violet,
        Wheat, White, WhiteSmoke,
        Yellow, YellowGreen
    };
    
    public static Unicolour? FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        var key = string.Concat(name.Where(x => !char.IsWhiteSpace(x))).ToLower();
        return Lookup.TryGetValue(key, out Unicolour? value) ? value : null;
    }
    
    private static readonly Dictionary<string, Unicolour> Lookup = new()
    {
        { nameof(AliceBlue).ToLower(), AliceBlue },
        { nameof(AntiqueWhite).ToLower(), AntiqueWhite },
        { nameof(Aqua).ToLower(), Aqua },
        { nameof(Aquamarine).ToLower(), Aquamarine },
        { nameof(Azure).ToLower(), Azure },
        { nameof(Beige).ToLower(), Beige },
        { nameof(Bisque).ToLower(), Bisque },
        { nameof(Black).ToLower(), Black },
        { nameof(BlanchedAlmond).ToLower(), BlanchedAlmond },
        { nameof(Blue).ToLower(), Blue },
        { nameof(BlueViolet).ToLower(), BlueViolet },
        { nameof(Brown).ToLower(), Brown },
        { nameof(Burlywood).ToLower(), Burlywood },
        { nameof(CadetBlue).ToLower(), CadetBlue },
        { nameof(Chartreuse).ToLower(), Chartreuse },
        { nameof(Chocolate).ToLower(), Chocolate },
        { nameof(Coral).ToLower(), Coral },
        { nameof(CornflowerBlue).ToLower(), CornflowerBlue },
        { nameof(Cornsilk).ToLower(), Cornsilk },
        { nameof(Crimson).ToLower(), Crimson },
        { nameof(Cyan).ToLower(), Cyan },
        { nameof(DarkBlue).ToLower(), DarkBlue },
        { nameof(DarkCyan).ToLower(), DarkCyan },
        { nameof(DarkGoldenrod).ToLower(), DarkGoldenrod },
        { nameof(DarkGray).ToLower(), DarkGray },
        { nameof(DarkGreen).ToLower(), DarkGreen },
        { nameof(DarkGrey).ToLower(), DarkGrey },
        { nameof(DarkKhaki).ToLower(), DarkKhaki },
        { nameof(DarkMagenta).ToLower(), DarkMagenta },
        { nameof(DarkOliveGreen).ToLower(), DarkOliveGreen },
        { nameof(DarkOrange).ToLower(), DarkOrange },
        { nameof(DarkOrchid).ToLower(), DarkOrchid },
        { nameof(DarkRed).ToLower(), DarkRed },
        { nameof(DarkSalmon).ToLower(), DarkSalmon },
        { nameof(DarkSeaGreen).ToLower(), DarkSeaGreen },
        { nameof(DarkSlateBlue).ToLower(), DarkSlateBlue },
        { nameof(DarkSlateGray).ToLower(), DarkSlateGray },
        { nameof(DarkSlateGrey).ToLower(), DarkSlateGrey },
        { nameof(DarkTurquoise).ToLower(), DarkTurquoise },
        { nameof(DarkViolet).ToLower(), DarkViolet },
        { nameof(DeepPink).ToLower(), DeepPink },
        { nameof(DeepSkyBlue).ToLower(), DeepSkyBlue },
        { nameof(DimGray).ToLower(), DimGray },
        { nameof(DimGrey).ToLower(), DimGrey },
        { nameof(DodgerBlue).ToLower(), DodgerBlue },
        { nameof(FireBrick).ToLower(), FireBrick },
        { nameof(FloralWhite).ToLower(), FloralWhite },
        { nameof(ForestGreen).ToLower(), ForestGreen },
        { nameof(Fuchsia).ToLower(), Fuchsia },
        { nameof(Gainsboro).ToLower(), Gainsboro },
        { nameof(GhostWhite).ToLower(), GhostWhite },
        { nameof(Gold).ToLower(), Gold },
        { nameof(Goldenrod).ToLower(), Goldenrod },
        { nameof(Gray).ToLower(), Gray },
        { nameof(Green).ToLower(), Green },
        { nameof(GreenYellow).ToLower(), GreenYellow },
        { nameof(Grey).ToLower(), Grey },
        { nameof(Honeydew).ToLower(), Honeydew },
        { nameof(HotPink).ToLower(), HotPink },
        { nameof(IndianRed).ToLower(), IndianRed },
        { nameof(Indigo).ToLower(), Indigo },
        { nameof(Ivory).ToLower(), Ivory },
        { nameof(Khaki).ToLower(), Khaki },
        { nameof(Lavender).ToLower(), Lavender },
        { nameof(LavenderBlush).ToLower(), LavenderBlush },
        { nameof(LawnGreen).ToLower(), LawnGreen },
        { nameof(LemonChiffon).ToLower(), LemonChiffon },
        { nameof(LightBlue).ToLower(), LightBlue },
        { nameof(LightCoral).ToLower(), LightCoral },
        { nameof(LightCyan).ToLower(), LightCyan },
        { nameof(LightGoldenrodYellow).ToLower(), LightGoldenrodYellow },
        { nameof(LightGray).ToLower(), LightGray },
        { nameof(LightGreen).ToLower(), LightGreen },
        { nameof(LightGrey).ToLower(), LightGrey },
        { nameof(LightPink).ToLower(), LightPink },
        { nameof(LightSalmon).ToLower(), LightSalmon },
        { nameof(LightSeaGreen).ToLower(), LightSeaGreen },
        { nameof(LightSkyBlue).ToLower(), LightSkyBlue },
        { nameof(LightSlateGray).ToLower(), LightSlateGray },
        { nameof(LightSlateGrey).ToLower(), LightSlateGrey },
        { nameof(LightSteelBlue).ToLower(), LightSteelBlue },
        { nameof(LightYellow).ToLower(), LightYellow },
        { nameof(Lime).ToLower(), Lime },
        { nameof(LimeGreen).ToLower(), LimeGreen },
        { nameof(Linen).ToLower(), Linen },
        { nameof(Magenta).ToLower(), Magenta },
        { nameof(Maroon).ToLower(), Maroon },
        { nameof(MediumAquamarine).ToLower(), MediumAquamarine },
        { nameof(MediumBlue).ToLower(), MediumBlue },
        { nameof(MediumOrchid).ToLower(), MediumOrchid },
        { nameof(MediumPurple).ToLower(), MediumPurple },
        { nameof(MediumSeaGreen).ToLower(), MediumSeaGreen },
        { nameof(MediumSlateBlue).ToLower(), MediumSlateBlue },
        { nameof(MediumSpringGreen).ToLower(), MediumSpringGreen },
        { nameof(MediumTurquoise).ToLower(), MediumTurquoise },
        { nameof(MediumVioletRed).ToLower(), MediumVioletRed },
        { nameof(MidnightBlue).ToLower(), MidnightBlue },
        { nameof(MintCream).ToLower(), MintCream },
        { nameof(MistyRose).ToLower(), MistyRose },
        { nameof(Moccasin).ToLower(), Moccasin },
        { nameof(NavajoWhite).ToLower(), NavajoWhite },
        { nameof(Navy).ToLower(), Navy },
        { nameof(OldLace).ToLower(), OldLace },
        { nameof(Olive).ToLower(), Olive },
        { nameof(OliveDrab).ToLower(), OliveDrab },
        { nameof(Orange).ToLower(), Orange },
        { nameof(OrangeRed).ToLower(), OrangeRed },
        { nameof(Orchid).ToLower(), Orchid },
        { nameof(PaleGoldenrod).ToLower(), PaleGoldenrod },
        { nameof(PaleGreen).ToLower(), PaleGreen },
        { nameof(PaleTurquoise).ToLower(), PaleTurquoise },
        { nameof(PaleVioletRed).ToLower(), PaleVioletRed },
        { nameof(PapayaWhip).ToLower(), PapayaWhip },
        { nameof(PeachPuff).ToLower(), PeachPuff },
        { nameof(Peru).ToLower(), Peru },
        { nameof(Pink).ToLower(), Pink },
        { nameof(Plum).ToLower(), Plum },
        { nameof(PowderBlue).ToLower(), PowderBlue },
        { nameof(Purple).ToLower(), Purple },
        { nameof(RebeccaPurple).ToLower(), RebeccaPurple },
        { nameof(Red).ToLower(), Red },
        { nameof(RosyBrown).ToLower(), RosyBrown },
        { nameof(RoyalBlue).ToLower(), RoyalBlue },
        { nameof(SaddleBrown).ToLower(), SaddleBrown },
        { nameof(Salmon).ToLower(), Salmon },
        { nameof(SandyBrown).ToLower(), SandyBrown },
        { nameof(SeaGreen).ToLower(), SeaGreen },
        { nameof(Seashell).ToLower(), Seashell },
        { nameof(Sienna).ToLower(), Sienna },
        { nameof(Silver).ToLower(), Silver },
        { nameof(SkyBlue).ToLower(), SkyBlue },
        { nameof(SlateBlue).ToLower(), SlateBlue },
        { nameof(SlateGray).ToLower(), SlateGray },
        { nameof(SlateGrey).ToLower(), SlateGrey },
        { nameof(Snow).ToLower(), Snow },
        { nameof(SpringGreen).ToLower(), SpringGreen },
        { nameof(SteelBlue).ToLower(), SteelBlue },
        { nameof(Tan).ToLower(), Tan },
        { nameof(Teal).ToLower(), Teal },
        { nameof(Thistle).ToLower(), Thistle },
        { nameof(Tomato).ToLower(), Tomato },
        { nameof(Turquoise).ToLower(), Turquoise },
        { nameof(Violet).ToLower(), Violet },
        { nameof(Wheat).ToLower(), Wheat },
        { nameof(White).ToLower(), White },
        { nameof(WhiteSmoke).ToLower(), WhiteSmoke },
        { nameof(Yellow).ToLower(), Yellow },
        { nameof(YellowGreen).ToLower(), YellowGreen },
        { nameof(Transparent).ToLower(), Transparent }
    };
}