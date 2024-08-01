namespace Wacton.Unicolour.Icc;

public record Header
{
    /* v2+ */
    public uint ProfileSize { get; private set; }
    public string PreferredCmmType { get; private set; } = null!;
    public Version ProfileVersion { get; private set; } = null!;
    public string ProfileClass { get; private set; } = null!;
    public string DataColourSpace { get; private set; } = null!;    // colour space on the A side of the transform
    public string Pcs { get; private set; } = null!;                // colour space on the B side of the transform
    public DateTime DateTime { get; private set; }
    public string ProfileFileSignature { get; private set; } = null!;
    public string PrimaryPlatform { get; private set; } = null!;
    public string[] ProfileFlags { get; private set; } = null!;
    public string DeviceManufacturer { get; private set; } = null!;
    public string DeviceModel { get; private set; } = null!;
    public string[] DeviceAttributes { get; private set; } = null!;
    public Intent Intent { get; private set; } = Intent.Unspecified;
    public (double x, double y, double z) PcsIlluminant { get; private set; }
    public string ProfileCreator { get; private set; } = null!;
    
    /* v4+ */
    public byte[] ProfileId { get; private set; } = null!;
    
    /* v5+ / iccMAX */
    /* don't care for now, low priority, expect quite difficult to integrate (especially reflectance or transmittance spectra) */
    
    // public string SpectralPcs { get; private set; } = null!;
    // public byte[] SpectralPcsWavelengthRange { get; private set; }
    // public byte[] BispectralPcsWavelengthRange { get; private set; }
    // public string McsSignature { get; private set; } = null!;
    // public string ProfileSubclass { get; private set; } = null!;
    // public uint Reserved { get; private set; }
    
    internal static Header FromFile(FileInfo fileInfo)
    {
        using var stream = fileInfo.OpenRead();
        
        // NOTE: order of assignments is critical, each call advances the stream position
        return new Header
        {
            // v2+
            ProfileSize = stream.ReadUInt32(),                          // bytes 0 - 3
            PreferredCmmType = stream.ReadSignature(),                  // bytes 4 - 7
            ProfileVersion = stream.ReadVersion(),                      // bytes 8 - 11
            ProfileClass = stream.ReadSignature(),                      // bytes 12 - 15
            DataColourSpace = stream.ReadSignature(),                   // bytes 16 - 19
            Pcs = stream.ReadSignature(),                               // bytes 20 - 23
            DateTime = stream.ReadDateTime(),                           // bytes 24 - 35
            ProfileFileSignature = stream.ReadSignature(),              // bytes 36 - 39
            PrimaryPlatform = stream.ReadSignature(),                   // bytes 40 - 43
            ProfileFlags = stream.ReadProfileFlags(),                   // bytes 44 - 47
            DeviceManufacturer = stream.ReadSignature(),                // bytes 48 - 51
            DeviceModel = stream.ReadSignature(),                       // bytes 52 - 55
            DeviceAttributes = stream.ReadDeviceAttributes(),           // bytes 56 - 63
            Intent = (Intent)stream.ReadUInt32(),                       // bytes 64 - 67
            PcsIlluminant = stream.ReadXyzNumber(),                     // bytes 68 - 79
            ProfileCreator = stream.ReadSignature(),                    // bytes 80 - 83
            
            // v4+
            ProfileId = stream.ReadBytes(16)                            // bytes 84 - 99
            
            // v5+ / iccMAX
            // SpectralPcs = stream.ReadSignature(),                       // bytes 100 - 103
            // SpectralPcsWavelengthRange = stream.ReadSpectralRange(),    // bytes 104 - 109
            // BispectralPcsWavelengthRange = stream.ReadSpectralRange(),  // bytes 110 - 115
            // McsSignature = stream.ReadSignature(),                      // bytes 116 - 119
            // ProfileSubclass = stream.ReadSignature(),                   // bytes 120 - 123
            // Reserved = stream.ReadUInt32()                              // bytes 124 - 127
        };
    }
    
    public override string ToString() => $"v{ProfileVersion}, {ProfileClass}, {Intent}, {DataColourSpace} device, {Pcs} PCS";
}

