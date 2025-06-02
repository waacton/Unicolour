namespace Wacton.Unicolour.Icc;

public record Header
{
    /* v2+ */
    public uint ProfileSize { get; }
    public string PreferredCmmType { get; }
    public Version ProfileVersion { get; }
    public string ProfileClass { get; }
    public string DataColourSpace { get; }  // colour space on the A side of the transform
    public string Pcs { get; }              // colour space on the B side of the transform
    public DateTime DateTime { get; }
    public string ProfileFileSignature { get; }
    public string PrimaryPlatform { get; }
    public string[] ProfileFlags { get; }
    public string DeviceManufacturer { get; }
    public string DeviceModel { get; }
    public string[] DeviceAttributes { get; }
    public Intent Intent { get; }
    public (double x, double y, double z) PcsIlluminant { get; }
    public string ProfileCreator { get; }
    
    /* v4+ */
    public byte[] ProfileId { get; }
    
    /* v5+ / iccMAX */
    /* don't care for now, low priority, expect quite difficult to integrate (especially reflectance or transmittance spectra) */
    
    // public string SpectralPcs { get; }
    // public byte[] SpectralPcsWavelengthRange { get; }
    // public byte[] BispectralPcsWavelengthRange { get; }
    // public string McsSignature { get; }
    // public string ProfileSubclass { get; }
    // public uint Reserved { get; }
    
    internal Header(Stream stream)
    {
        // NOTE: order of assignments is critical, each call advances the stream position
        // v2+
        ProfileSize = stream.ReadUInt32();                          // bytes 0 - 3
        PreferredCmmType = stream.ReadSignature();                  // bytes 4 - 7
        ProfileVersion = stream.ReadVersion();                      // bytes 8 - 11
        ProfileClass = stream.ReadSignature();                      // bytes 12 - 15
        DataColourSpace = stream.ReadSignature();                   // bytes 16 - 19
        Pcs = stream.ReadSignature();                               // bytes 20 - 23
        DateTime = stream.ReadDateTime();                           // bytes 24 - 35
        ProfileFileSignature = stream.ReadSignature();              // bytes 36 - 39
        PrimaryPlatform = stream.ReadSignature();                   // bytes 40 - 43
        ProfileFlags = stream.ReadProfileFlags();                   // bytes 44 - 47
        DeviceManufacturer = stream.ReadSignature();                // bytes 48 - 51
        DeviceModel = stream.ReadSignature();                       // bytes 52 - 55
        DeviceAttributes = stream.ReadDeviceAttributes();           // bytes 56 - 63
        Intent = (Intent)stream.ReadUInt32();                       // bytes 64 - 67
        PcsIlluminant = stream.ReadXyzNumber();                     // bytes 68 - 79
        ProfileCreator = stream.ReadSignature();                    // bytes 80 - 83
        
        // v4+
        ProfileId = stream.ReadBytes(16);                           // bytes 84 - 99
        
        // v5+ / iccMAX
        // SpectralPcs = stream.ReadSignature();                       // bytes 100 - 103
        // SpectralPcsWavelengthRange = stream.ReadSpectralRange();    // bytes 104 - 109
        // BispectralPcsWavelengthRange = stream.ReadSpectralRange();  // bytes 110 - 115
        // McsSignature = stream.ReadSignature();                      // bytes 116 - 119
        // ProfileSubclass = stream.ReadSignature();                   // bytes 120 - 123
        // Reserved = stream.ReadUInt32();                             // bytes 124 - 127
    }
    
    public override string ToString() => $"v{ProfileVersion}, {ProfileClass}, {Intent}, {DataColourSpace} device, {Pcs} PCS";
}

