namespace Wacton.Unicolour.Example.Web;

internal static class Configs
{
    internal static readonly List<RgbConfiguration> Rgb =
    [
        RgbConfiguration.StandardRgb,
        RgbConfiguration.DisplayP3,
        RgbConfiguration.Rec2020,
        RgbConfiguration.A98,
        RgbConfiguration.ProPhoto,
        RgbConfiguration.Aces20651,
        RgbConfiguration.Acescg,
        RgbConfiguration.Acescct,
        RgbConfiguration.Acescc,
        RgbConfiguration.Rec601Line625,
        RgbConfiguration.Rec601Line525,
        RgbConfiguration.Rec709,
        RgbConfiguration.XvYcc,
        RgbConfiguration.Pal,
        RgbConfiguration.PalM,
        RgbConfiguration.Pal625,
        RgbConfiguration.Pal525,
        RgbConfiguration.Ntsc,
        RgbConfiguration.NtscSmpteC,
        RgbConfiguration.Ntsc525,
        RgbConfiguration.Secam,
        RgbConfiguration.Secam625
    ];
    
    internal static readonly XyzConfiguration ADegree2 = new(Illuminant.A, Observer.Degree2, "A (2°)");
    internal static readonly XyzConfiguration CDegree2 = new(Illuminant.C, Observer.Degree2, "C (2°)");
    internal static readonly XyzConfiguration D50Degree2 = new(Illuminant.D50, Observer.Degree2, "D50 (2°)");
    internal static readonly XyzConfiguration D55Degree2 = new(Illuminant.D55, Observer.Degree2, "D55 (2°)");
    internal static readonly XyzConfiguration D65Degree2 = new(Illuminant.D65, Observer.Degree2, "D65 (2°)");
    internal static readonly XyzConfiguration D75Degree2 = new(Illuminant.D75, Observer.Degree2, "D75 (2°)");
    internal static readonly XyzConfiguration EDegree2 = new(Illuminant.E, Observer.Degree2, "E (2°)");
    internal static readonly XyzConfiguration F2Degree2 = new(Illuminant.F2, Observer.Degree2, "F2 (2°)");
    internal static readonly XyzConfiguration F7Degree2 = new(Illuminant.F7, Observer.Degree2, "F7 (2°)");
    internal static readonly XyzConfiguration F11Degree2 = new(Illuminant.F11, Observer.Degree2, "F11 (2°)");
    internal static readonly XyzConfiguration ADegree10 = new(Illuminant.A, Observer.Degree10, "A (10°)");
    internal static readonly XyzConfiguration CDegree10 = new(Illuminant.C, Observer.Degree10, "C (10°)");
    internal static readonly XyzConfiguration D50Degree10 = new(Illuminant.D50, Observer.Degree10, "D50 (10°)");
    internal static readonly XyzConfiguration D55Degree10 = new(Illuminant.D55, Observer.Degree10, "D55 (10°)");
    internal static readonly XyzConfiguration D65Degree10 = new(Illuminant.D65, Observer.Degree10, "D65 (10°)");
    internal static readonly XyzConfiguration D75Degree10 = new(Illuminant.D75, Observer.Degree10, "D75 (10°)");
    internal static readonly XyzConfiguration EDegree10 = new(Illuminant.E, Observer.Degree10, "E (10°)");
    internal static readonly XyzConfiguration F2Degree10 = new(Illuminant.F2, Observer.Degree10, "F2 (10°)");
    internal static readonly XyzConfiguration F7Degree10 = new(Illuminant.F7, Observer.Degree10, "F7 (10°)");
    internal static readonly XyzConfiguration F11Degree10 = new(Illuminant.F11, Observer.Degree10, "F11 (10°)");

    internal static List<XyzConfiguration> Xyz =
    [
        ADegree2,
        CDegree2,
        D50Degree2,
        D55Degree2,
        D65Degree2,
        D75Degree2,
        EDegree2,
        F2Degree2,
        F7Degree2,
        F11Degree2,
        ADegree10,
        CDegree10,
        D50Degree10,
        D55Degree10,
        D65Degree10,
        D75Degree10,
        EDegree10,
        F2Degree10,
        F7Degree10,
        F11Degree10
    ];
}