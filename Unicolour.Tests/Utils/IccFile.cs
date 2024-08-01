using System.Collections.Generic;
using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests.Utils;

public record IccFile(string Id, string Name, int DeviceChannels)
{
    internal const string DataFolderName = "Data/ICC";

    internal static IccFile Fogra39 = new(nameof(Fogra39), "Coated_Fogra39L_VIGC_300", 4);              // v2 CMYK -> LAB, prtr, (lut16Type)
    internal static IccFile Fogra55 = new(nameof(Fogra55), "Ref-ECG-CMYKOGV_FOGRA55_TAC300", 7);        // v2 7CLR -> LAB, prtr, (lut16Type)
    internal static IccFile Swop2006 = new(nameof(Swop2006), "SWOP2006_Coated5v2", 4);                  // v2 CMYK -> LAB, prtr, (lut16Type)
    internal static IccFile Swop2013 = new(nameof(Swop2013), "SWOP2013C3_CRPC5", 4);                    // v4 CMYK -> LAB, prtr, (lutAToBType)
    internal static IccFile JapanColor2011 = new(nameof(JapanColor2011), "JapanColor2011Coated", 4);    // v2 CMYK -> LAB, prtr, (lut8Type)
    internal static IccFile Cgats21 = new(nameof(Cgats21), "CGATS21_CRPC7", 4);                         // v4 CMYK -> LAB, prtr, (lutAToBType)
    internal static IccFile Prmg = new(nameof(Prmg), "PRMG_v2.0.1_MR", 4);                              // v4 CMYK -> LAB, prtr, (lutAToBType)
    internal static IccFile StandardRgbV2 = new(nameof(StandardRgbV2), "sRGB2014", 3);                  // v2 RGB -> XYZ,  mntr
    
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
        { nameof(Cgats21), Cgats21 },
        { nameof(Prmg), Prmg },
        { nameof(StandardRgbV2), StandardRgbV2 }
    };

    public override string ToString() => Id;
}