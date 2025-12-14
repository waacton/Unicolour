using Wacton.Unicolour.Datasets;

namespace Wacton.Unicolour.Example.Web;

internal static class Utils
{
    internal static readonly Unicolour Dark = new("404046");
    internal static readonly Unicolour Light = new("e8e8ff");
    
    internal static string ToCss(Unicolour colour, double alpha)
    {
        if (HasConversionError(colour))
        {
            return "transparent";
        }

        var (r, g, b) = colour.Rgb.ConstrainedTriplet;
        return $"rgb({r * 100}% {g * 100}% {b * 100}% / {alpha}%)";
    }

    internal static bool HasConversionError(Unicolour colour)
    {
        // if the constrained hex has no value, RGB is invalid (likely a NaN during conversion)
        return colour.Rgb.Byte255.ConstrainedHex == "-";
    }

    internal static string TextOnColourCssClass(Unicolour colour)
    {
        if (HasConversionError(colour)) return "light-text-with-contrast";
        var inGamut = colour.MapToRgbGamut(GamutMap.RgbClipping);
        return inGamut.Contrast(Light) > inGamut.Contrast(Dark) 
            ? "light-text-with-contrast" 
            : "dark-text-with-contrast";
    }

    internal static readonly Dictionary<ColourSpace, Range[]> SpaceToRange = new()
    {
        { ColourSpace.Rgb255, [new(0, 255), new(0, 255), new(0, 255)] },
        { ColourSpace.Rgb, [new(0, 1), new(0, 1), new(0, 1)] },
        { ColourSpace.RgbLinear, [new(0, 1), new(0, 1), new(0, 1)] },
        { ColourSpace.Hsb, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Hsl, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Hwb, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Hsi, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Xyz, [new(0, 1.1), new(0, 1.1), new(0, 1.1)] },
        { ColourSpace.Xyy, [new(0, 1), new(0, 1), new(0, 1)] },
        { ColourSpace.Wxy, [new(360, 700), new(0, 1), new(0, 1)] },
        { ColourSpace.Lab, [new(0, 100), new(-128, 128), new(-128, 128)] },
        { ColourSpace.Lchab, [new(0, 100), new(0, 230), new(0, 360)] },
        { ColourSpace.Luv, [new(0, 100), new(-100, 100), new(-100, 100)] },
        { ColourSpace.Lchuv, [new(0, 100), new(0, 230), new(0, 360)] },
        { ColourSpace.Hsluv, [new(0, 360), new(0, 100), new(0, 100)] },
        { ColourSpace.Hpluv, [new(0, 360), new(0, 100), new(0, 100)] },
        { ColourSpace.Ypbpr, [new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)] },
        { ColourSpace.Ycbcr, [new(0, 255), new(0, 255), new(0, 255)] },
        { ColourSpace.Ycgco, [new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)] },
        { ColourSpace.Yuv, [new(0, 1), new(-0.44, 0.44), new(-0.62, 0.62)] },
        { ColourSpace.Yiq, [new(0, 1), new(-0.60, 0.60), new(-0.53, 0.53)] },
        { ColourSpace.Ydbdr, [new(0, 1), new(-1.333, 1.333), new(-1.333, 1.333)] },
        { ColourSpace.Tsl, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Xyb, [new(-0.03, 0.03), new(0, 1.0), new(-0.4, 0.4)] },
        { ColourSpace.Lms, [new(0, 1), new(0, 1), new(0, 1)] },
        { ColourSpace.Ipt, [new(0, 1), new(-0.75, 0.75), new(-0.75, 0.75)] },
        { ColourSpace.Ictcp, [new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)] },
        { ColourSpace.Jzazbz, [new(0, 1), new(-0.21, 0.21), new(-0.21, 0.21)] },
        { ColourSpace.Jzczhz, [new(0, 1), new(0, 0.26), new(0, 360)] },
        { ColourSpace.Oklab, [new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)] },
        { ColourSpace.Oklch, [new(0, 1), new(0, 0.5), new(0, 360)] },
        { ColourSpace.Okhsv, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Okhsl, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Okhwb, [new(0, 360), new(0, 1), new(0, 1)] },
        { ColourSpace.Oklrab, [new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)] },
        { ColourSpace.Oklrch, [new(0, 1), new(0, 0.5), new(0, 360)] },
        { ColourSpace.Cam02, [new(0, 100), new(-50, 50), new(-50, 50)] },
        { ColourSpace.Cam16, [new(0, 100), new(-50, 50), new(-50, 50)] },
        { ColourSpace.Hct, [new(0, 360), new(0, 120), new(0, 100)] },
        { ColourSpace.Munsell, [new(0, 360), new(1, 10), new(0, 26)] }
    };

    internal static readonly Dictionary<ColourSpace, string[]> SpaceToAxes = new()
    {
        { ColourSpace.Rgb255, ["R", "G", "B"] },
        { ColourSpace.Rgb, ["R", "G", "B"] },
        { ColourSpace.RgbLinear, ["R", "G", "B"] },
        { ColourSpace.Hsb, ["H", "S", "B"] },
        { ColourSpace.Hsl, ["H", "S", "L"] },
        { ColourSpace.Hwb, ["H", "W", "B"] },
        { ColourSpace.Hsi, ["H", "S", "I"] },
        { ColourSpace.Xyz, ["X", "Y", "Z"] },
        { ColourSpace.Xyy, ["x", "y", "Y"] },
        { ColourSpace.Wxy, ["W", "X", "Y"] },
        { ColourSpace.Lab, ["L", "A", "B"] },
        { ColourSpace.Lchab, ["L", "C", "H"] },
        { ColourSpace.Luv, ["L", "U", "V"] },
        { ColourSpace.Lchuv, ["L", "C", "H"] },
        { ColourSpace.Hsluv, ["H", "S", "L"] },
        { ColourSpace.Hpluv, ["H", "S", "L"] },
        { ColourSpace.Ypbpr, ["Y", "Pb", "Pr"] },
        { ColourSpace.Ycbcr, ["Y", "Cb", "Cr"] },
        { ColourSpace.Ycgco, ["Y", "Cg", "Co"] },
        { ColourSpace.Yuv, ["Y", "U", "V"] },
        { ColourSpace.Yiq, ["Y", "I", "Q"] },
        { ColourSpace.Ydbdr, ["Y", "Db", "Dr"] },
        { ColourSpace.Tsl, ["T", "S", "L"] },
        { ColourSpace.Xyb, ["X", "Y", "B"] },
        { ColourSpace.Lms, ["L", "M", "S"] },
        { ColourSpace.Ipt, ["I", "P", "T"] },
        { ColourSpace.Ictcp, ["I", "Ct", "Cp"] },
        { ColourSpace.Jzazbz, ["J", "A", "B"] },
        { ColourSpace.Jzczhz, ["J", "C", "H"] },
        { ColourSpace.Oklab, ["L", "A", "B"] },
        { ColourSpace.Oklch, ["L", "C", "H"] },
        { ColourSpace.Okhsv, ["H", "S", "V"] },
        { ColourSpace.Okhsl, ["H", "S", "L"] },
        { ColourSpace.Okhwb, ["H", "W", "B"] },
        { ColourSpace.Oklrab, ["L", "A", "B"] },
        { ColourSpace.Oklrch, ["L", "C", "H"] },
        { ColourSpace.Cam02, ["J", "A", "B"] },
        { ColourSpace.Cam16, ["J", "A", "B"] },
        { ColourSpace.Hct, ["H", "C", "T"] },
        { ColourSpace.Munsell, ["H", "V", "C"] }
    };

    internal static readonly Dictionary<Pigment, string> PigmentToName = new()
    {
        { ArtistPaint.BoneBlack, "Bone Black" },
        { ArtistPaint.TitaniumWhite, "Titanium White" },
        { ArtistPaint.BismuthVanadateYellow, "Bismuth Vanadate Yellow" },
        { ArtistPaint.HansaYellowOpaque, "Hansa Yellow" },
        { ArtistPaint.DiarylideYellow, "Diarylide Yellow" },
        { ArtistPaint.CadmiumOrange, "Cadmium Orange" },
        { ArtistPaint.PyrroleOrange, "Pyrrole Orange" },
        { ArtistPaint.CadmiumRedLight, "Cadmium Red Light" },
        { ArtistPaint.PyrroleRed, "Pyrrole Red" },
        { ArtistPaint.QuinacridoneRed, "Quinacridone Red" },
        { ArtistPaint.QuinacridoneMagenta, "Quinacridone Magenta" },
        { ArtistPaint.DioxazinePurple, "Dioxazine Purple" },
        { ArtistPaint.PhthaloBlueRedShade, "Phthalo Blue (Red Shade)" },
        { ArtistPaint.PhthaloBlueGreenShade, "Phthalo Blue (Green Shade)" },
        { ArtistPaint.UltramarineBlue, "Ultramarine Blue" },
        { ArtistPaint.CobaltBlue, "Cobalt Blue" },
        { ArtistPaint.CeruleanBlueChromium, "Cerulean Blue Chromium" },
        { ArtistPaint.PhthaloGreenBlueShade, "Phthalo Green (Blue Shade)" },
        { ArtistPaint.PhthaloGreenYellowShade, "Phthalo Green (Yellow Shade)" }
    };

    internal static readonly Dictionary<Pigment, Unicolour> PigmentToColour = new()
    {
        { ArtistPaint.BoneBlack, GetSinglePigmentColour(ArtistPaint.BoneBlack) },
        { ArtistPaint.TitaniumWhite, GetSinglePigmentColour(ArtistPaint.TitaniumWhite) },
        { ArtistPaint.BismuthVanadateYellow, GetSinglePigmentColour(ArtistPaint.BismuthVanadateYellow) },
        { ArtistPaint.HansaYellowOpaque, GetSinglePigmentColour(ArtistPaint.HansaYellowOpaque) },
        { ArtistPaint.DiarylideYellow, GetSinglePigmentColour(ArtistPaint.DiarylideYellow) },
        { ArtistPaint.CadmiumOrange, GetSinglePigmentColour(ArtistPaint.CadmiumOrange) },
        { ArtistPaint.PyrroleOrange, GetSinglePigmentColour(ArtistPaint.PyrroleOrange) },
        { ArtistPaint.CadmiumRedLight, GetSinglePigmentColour(ArtistPaint.CadmiumRedLight) },
        { ArtistPaint.PyrroleRed, GetSinglePigmentColour(ArtistPaint.PyrroleRed) },
        { ArtistPaint.QuinacridoneRed, GetSinglePigmentColour(ArtistPaint.QuinacridoneRed) },
        { ArtistPaint.QuinacridoneMagenta, GetSinglePigmentColour(ArtistPaint.QuinacridoneMagenta) },
        { ArtistPaint.DioxazinePurple, GetSinglePigmentColour(ArtistPaint.DioxazinePurple) },
        { ArtistPaint.PhthaloBlueRedShade, GetSinglePigmentColour(ArtistPaint.PhthaloBlueRedShade) },
        { ArtistPaint.PhthaloBlueGreenShade, GetSinglePigmentColour(ArtistPaint.PhthaloBlueGreenShade) },
        { ArtistPaint.UltramarineBlue, GetSinglePigmentColour(ArtistPaint.UltramarineBlue) },
        { ArtistPaint.CobaltBlue, GetSinglePigmentColour(ArtistPaint.CobaltBlue) },
        { ArtistPaint.CeruleanBlueChromium, GetSinglePigmentColour(ArtistPaint.CeruleanBlueChromium) },
        { ArtistPaint.PhthaloGreenBlueShade, GetSinglePigmentColour(ArtistPaint.PhthaloGreenBlueShade) },
        { ArtistPaint.PhthaloGreenYellowShade, GetSinglePigmentColour(ArtistPaint.PhthaloGreenYellowShade) },
    };
    
    private static Unicolour GetSinglePigmentColour(Pigment pigment) => new Unicolour([pigment], [1]).MapToRgbGamut(GamutMap.RgbClipping);
        
    internal static string[] IccAxes(string space)
    {
        return space switch
        {
            "XYZ " => ["X", "Y", "Z"],
            "Lab " => ["L", "A", "B"],
            "Luv " => ["L", "U", "V"],
            "YCbr" => ["Y", "Cb", "Cr"],
            "Yxy " => ["Y", "x", "y"],
            "RGB " => ["R", "G", "B"],
            "GRAY" => ["K"],
            "HSV " => ["H", "S", "V"],
            "HLS " => ["H", "L", "S"],
            "CMYK" => ["C", "M", "Y", "K"],
            "CMY " => ["C", "M", "Y"],
            "2CLR" => ["C1", "C2"],
            "3CLR" => ["C1", "C2", "C3"],
            "4CLR" => ["C1", "C2", "C3", "C4"],
            "5CLR" => ["C1", "C2", "C3", "C4", "C5"],
            "6CLR" => ["C1", "C2", "C3", "C4", "C5", "C6"],
            "7CLR" => ["C1", "C2", "C3", "C4", "C5", "C6", "C7"],
            "8CLR" => ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8"],
            "9CLR" => ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9"],
            "ACLR" => ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10"],
            "BCLR" => ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11"],
            "CCLR" => ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11", "C12"],
            "DCLR" => ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11", "C12", "C13"],
            "ECLR" => ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11", "C12", "C13", "C14"],
            "FCLR" => ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11", "C12", "C13", "C14", "C15"],
            _ => []
        };
    }
}