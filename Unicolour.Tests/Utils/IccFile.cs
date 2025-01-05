using System.Collections.Generic;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests.Utils;

public record IccFile(string Id, string Name)
{
    internal const string DataFolderName = "Data/ICC";

    internal static IccFile Fogra39 = new(nameof(Fogra39), "Coated_Fogra39L_VIGC_300");               // v2 CMYK -> LAB, prtr, lut16
    internal static IccFile Fogra55 = new(nameof(Fogra55), "Ref-ECG-CMYKOGV_FOGRA55_TAC300");         // v2 7CLR -> LAB, prtr, lut16
    internal static IccFile Swop2006 = new(nameof(Swop2006), "SWOP2006_Coated5v2");                   // v2 CMYK -> LAB, prtr, lut16
    internal static IccFile Swop2013 = new(nameof(Swop2013), "SWOP2013C3_CRPC5");                     // v4 CMYK -> LAB, prtr, lutAToB: B-CLUT-A
    internal static IccFile JapanColor2011 = new(nameof(JapanColor2011), "JapanColor2011Coated");     // v2 CMYK -> LAB, prtr, lut8 (A2B)
    internal static IccFile JapanColor2003 = new(nameof(JapanColor2003), "JapanColor2003WebCoated");  // v2 CMYK -> LAB, prtr, lut8 (B2A)
    internal static IccFile Cgats21 = new(nameof(Cgats21), "CGATS21_CRPC7");                          // v4 CMYK -> LAB, prtr, lutAToB: B-CLUT-A
    internal static IccFile Prmg = new(nameof(Prmg), "PRMG_v2.0.1_MR");                               // v4 CMYK -> LAB, prtr, lutAToB: B-CLUT-A
    internal static IccFile RommRgb = new(nameof(RommRgb), "ISO22028-2_ROMM-RGB");                    // v4 RGB -> XYZ,  spac, lutAToB: B-Matrix-M [only intent 0]
    internal static IccFile StandardRgbV4 = new(nameof(StandardRgbV4), "sRGB_v4_ICC_preference");     // v4 RGB -> LAB,  spac, lutAToB: B-Matrix-M-CLUT-A [only intent 0 & 1]
    internal static IccFile StandardRgbV2 = new(nameof(StandardRgbV2), "sRGB2014");                   // v2 RGB -> XYZ,  mntr, TRC matrix
    internal static IccFile StandardRgbGreyV4 = new(nameof(StandardRgbGreyV4), "sGrey-v4");           // v4 GRAY -> XYZ, mntr, TRC grey
    internal static IccFile D65Xyz = new(nameof(D65Xyz), "D65_XYZ");                                  // v2 RGB -> XYZ,  mntr, TRC matrix
    internal static IccFile CxMonitorWeird = new(nameof(CxMonitorWeird), "CX_Monitor_weird");         // v2 RGB -> LAB,  mntr, lut16 [only intent 0] & TRC matrix [unused]
    internal static IccFile CxCmykProof = new(nameof(CxCmykProof), "CX_CMYK_ProofTest_PCS_RGB");      // v2 CMYK -> LAB, prtr, lut16 & TRC grey [unused]
    internal static IccFile CxScannerGrey = new(nameof(CxScannerGrey), "CX_scnr_RGB-_Gray");          // v2 RGB -> LAB,  scnr, lut16 [A2B only, no B2A]
    internal static IccFile CxHue45Abstract = new(nameof(CxHue45Abstract), "CX_Hue+45_abst");         // v2 LAB -> LAB,  abst, lut16 [A2B only, no B2A]
    internal static IccFile HackCxCmykKtrc = new(nameof(HackCxCmykKtrc), "Hack_(CX_CMYK_ktrc_only)"); // v2 GRAY -> LAB, prtr, TRC grey ℹ️ hacked variant of CxCmykProof ℹ️
    
    internal string Path => System.IO.Path.Combine(DataFolderName, $"{Name}.icc");
    
    private static readonly Dictionary<IccFile, Profile> ProfileCache = new();
    internal Profile GetProfile()
    {
        if (ProfileCache.TryGetValue(this, out var cachedProfile))
        {
            return cachedProfile;
        }
        
        var profile = new Profile(Path);
        ProfileCache[this] = profile;
        return profile;
    }

    internal static readonly Dictionary<string, IccFile> Lookup = new()
    {
        { nameof(Fogra39), Fogra39 },
        { nameof(Fogra55), Fogra55 },
        { nameof(Swop2006), Swop2006 },
        { nameof(Swop2013), Swop2013 },
        { nameof(JapanColor2011), JapanColor2011 },
        { nameof(JapanColor2003), JapanColor2003 },
        { nameof(Cgats21), Cgats21 },
        { nameof(Prmg), Prmg },
        { nameof(RommRgb), RommRgb },
        { nameof(StandardRgbV4), StandardRgbV4 },
        { nameof(StandardRgbV2), StandardRgbV2 },
        { nameof(StandardRgbGreyV4), StandardRgbGreyV4 },
        { nameof(D65Xyz), D65Xyz }
    };
    
    internal static int GetDeviceChannels(IccFile iccFile)
    {
        return iccFile.GetProfile().Header.DataColourSpace switch
        {
            Signatures.Cmyk => 4,
            Signatures.Clr7 => 7,
            Signatures.Grey => 1,
            _ => 3
        };
    }

    public override string ToString() => Id;
}