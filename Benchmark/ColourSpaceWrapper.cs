extern alias LocalProject;
extern alias NuGetPackage;

using Local = LocalProject::Wacton.Unicolour;
using NuGet = NuGetPackage::Wacton.Unicolour;

namespace Benchmark;

public enum ColourSpaceWrapper
{
    Rgb,
    Rgb255,
    RgbLinear,
    Hsb,
    Hsl,
    Hwb,
    Hsi,
    Xyz,
    Xyy,
    Wxy,
    Lab,
    Lchab,
    Luv,
    Lchuv,
    Hsluv,
    Hpluv,
    Ypbpr,
    Ycbcr,
    Ycgco,
    Yuv,
    Yiq,
    Ydbdr,
    Tsl,
    Xyb,
    Lms,
    Ipt,
    Ictcp,
    Jzazbz,
    Jzczhz,
    Oklab,
    Oklch,
    Okhsv,
    Okhsl,
    Okhwb,
    Oklrab,
    Oklrch,
    Cam02,
    Cam16,
    Hct,
    Munsell
}

public static class ColourSpaceExtensions
{
    public static NuGet.ColourSpace ToNuGet(this ColourSpaceWrapper colourSpace)
    {
        return colourSpace switch
        {
            ColourSpaceWrapper.Rgb => NuGet.ColourSpace.Rgb,
            ColourSpaceWrapper.Rgb255 => NuGet.ColourSpace.Rgb255,
            ColourSpaceWrapper.RgbLinear => NuGet.ColourSpace.RgbLinear,
            ColourSpaceWrapper.Hsb => NuGet.ColourSpace.Hsb,
            ColourSpaceWrapper.Hsl => NuGet.ColourSpace.Hsl,
            ColourSpaceWrapper.Hwb => NuGet.ColourSpace.Hwb,
            ColourSpaceWrapper.Hsi => NuGet.ColourSpace.Hsi,
            ColourSpaceWrapper.Xyz => NuGet.ColourSpace.Xyz,
            ColourSpaceWrapper.Xyy => NuGet.ColourSpace.Xyy,
            ColourSpaceWrapper.Wxy => NuGet.ColourSpace.Wxy,
            ColourSpaceWrapper.Lab => NuGet.ColourSpace.Lab,
            ColourSpaceWrapper.Lchab => NuGet.ColourSpace.Lchab,
            ColourSpaceWrapper.Luv => NuGet.ColourSpace.Luv,
            ColourSpaceWrapper.Lchuv => NuGet.ColourSpace.Lchuv,
            ColourSpaceWrapper.Hsluv => NuGet.ColourSpace.Hsluv,
            ColourSpaceWrapper.Hpluv => NuGet.ColourSpace.Hpluv,
            ColourSpaceWrapper.Ypbpr => NuGet.ColourSpace.Ypbpr,
            ColourSpaceWrapper.Ycbcr => NuGet.ColourSpace.Ycbcr,
            ColourSpaceWrapper.Ycgco => NuGet.ColourSpace.Ycgco,
            ColourSpaceWrapper.Yuv => NuGet.ColourSpace.Yuv,
            ColourSpaceWrapper.Yiq => NuGet.ColourSpace.Yiq,
            ColourSpaceWrapper.Ydbdr => NuGet.ColourSpace.Ydbdr,
            ColourSpaceWrapper.Tsl => NuGet.ColourSpace.Tsl,
            ColourSpaceWrapper.Xyb => NuGet.ColourSpace.Xyb,
            ColourSpaceWrapper.Lms => NuGet.ColourSpace.Lms,
            ColourSpaceWrapper.Ipt => NuGet.ColourSpace.Ipt,
            ColourSpaceWrapper.Ictcp => NuGet.ColourSpace.Ictcp,
            ColourSpaceWrapper.Jzazbz => NuGet.ColourSpace.Jzazbz,
            ColourSpaceWrapper.Jzczhz => NuGet.ColourSpace.Jzczhz,
            ColourSpaceWrapper.Oklab => NuGet.ColourSpace.Oklab,
            ColourSpaceWrapper.Oklch => NuGet.ColourSpace.Oklch,
            ColourSpaceWrapper.Okhsv => NuGet.ColourSpace.Okhsv,
            ColourSpaceWrapper.Okhsl => NuGet.ColourSpace.Okhsl,
            ColourSpaceWrapper.Okhwb => NuGet.ColourSpace.Okhwb,
            ColourSpaceWrapper.Oklrab => NuGet.ColourSpace.Oklrab,
            ColourSpaceWrapper.Oklrch => NuGet.ColourSpace.Oklrch,
            ColourSpaceWrapper.Cam02 => NuGet.ColourSpace.Cam02,
            ColourSpaceWrapper.Cam16 => NuGet.ColourSpace.Cam16,
            ColourSpaceWrapper.Hct => NuGet.ColourSpace.Hct,
            ColourSpaceWrapper.Munsell => NuGet.ColourSpace.Munsell,
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }
    
    public static Local.ColourSpace ToLocal(this ColourSpaceWrapper colourSpace)
    {
        return colourSpace switch
        {
            ColourSpaceWrapper.Rgb => Local.ColourSpace.Rgb,
            ColourSpaceWrapper.Rgb255 => Local.ColourSpace.Rgb255,
            ColourSpaceWrapper.RgbLinear => Local.ColourSpace.RgbLinear,
            ColourSpaceWrapper.Hsb => Local.ColourSpace.Hsb,
            ColourSpaceWrapper.Hsl => Local.ColourSpace.Hsl,
            ColourSpaceWrapper.Hwb => Local.ColourSpace.Hwb,
            ColourSpaceWrapper.Hsi => Local.ColourSpace.Hsi,
            ColourSpaceWrapper.Xyz => Local.ColourSpace.Xyz,
            ColourSpaceWrapper.Xyy => Local.ColourSpace.Xyy,
            ColourSpaceWrapper.Wxy => Local.ColourSpace.Wxy,
            ColourSpaceWrapper.Lab => Local.ColourSpace.Lab,
            ColourSpaceWrapper.Lchab => Local.ColourSpace.Lchab,
            ColourSpaceWrapper.Luv => Local.ColourSpace.Luv,
            ColourSpaceWrapper.Lchuv => Local.ColourSpace.Lchuv,
            ColourSpaceWrapper.Hsluv => Local.ColourSpace.Hsluv,
            ColourSpaceWrapper.Hpluv => Local.ColourSpace.Hpluv,
            ColourSpaceWrapper.Ypbpr => Local.ColourSpace.Ypbpr,
            ColourSpaceWrapper.Ycbcr => Local.ColourSpace.Ycbcr,
            ColourSpaceWrapper.Ycgco => Local.ColourSpace.Ycgco,
            ColourSpaceWrapper.Yuv => Local.ColourSpace.Yuv,
            ColourSpaceWrapper.Yiq => Local.ColourSpace.Yiq,
            ColourSpaceWrapper.Ydbdr => Local.ColourSpace.Ydbdr,
            ColourSpaceWrapper.Tsl => Local.ColourSpace.Tsl,
            ColourSpaceWrapper.Xyb => Local.ColourSpace.Xyb,
            ColourSpaceWrapper.Lms => Local.ColourSpace.Lms,
            ColourSpaceWrapper.Ipt => Local.ColourSpace.Ipt,
            ColourSpaceWrapper.Ictcp => Local.ColourSpace.Ictcp,
            ColourSpaceWrapper.Jzazbz => Local.ColourSpace.Jzazbz,
            ColourSpaceWrapper.Jzczhz => Local.ColourSpace.Jzczhz,
            ColourSpaceWrapper.Oklab => Local.ColourSpace.Oklab,
            ColourSpaceWrapper.Oklch => Local.ColourSpace.Oklch,
            ColourSpaceWrapper.Okhsv => Local.ColourSpace.Okhsv,
            ColourSpaceWrapper.Okhsl => Local.ColourSpace.Okhsl,
            ColourSpaceWrapper.Okhwb => Local.ColourSpace.Okhwb,
            ColourSpaceWrapper.Oklrab => Local.ColourSpace.Oklrab,
            ColourSpaceWrapper.Oklrch => Local.ColourSpace.Oklrch,
            ColourSpaceWrapper.Cam02 => Local.ColourSpace.Cam02,
            ColourSpaceWrapper.Cam16 => Local.ColourSpace.Cam16,
            ColourSpaceWrapper.Hct => Local.ColourSpace.Hct,
            ColourSpaceWrapper.Munsell => Local.ColourSpace.Munsell,
            _ => throw new ArgumentOutOfRangeException(nameof(colourSpace), colourSpace, null)
        };
    }
}