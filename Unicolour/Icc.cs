namespace Wacton.Unicolour;

using System.Text;

public static class Icc
{
    private static readonly FileInfo Fogra39 = new("Coated_Fogra39L_VIGC_300.icc");
    private static readonly FileInfo Fogra55 = new("Ref-ECG-CMYKOGV_FOGRA55_TAC300.icc");
    private static readonly ASCIIEncoding AsciiEncoding = new();
    
    public static void Execute()
    {
        Parse(Fogra39);
        Parse(Fogra55);
    }

    private static void Parse(FileInfo fileInfo)
    {
        using var stream = fileInfo.OpenRead();
        var profileSize = GetUInt32(stream); // bytes 0 - 3
        var preferredCmmType = GetFourByteSignature(stream); // bytes 4 - 7
        var profileVersion = GetProfileVersion(stream); // bytes 8 - 11
        var profileClass = GetFourByteSignature(stream); // bytes 12 - 15
        var dataColourSpace = GetFourByteSignature(stream); // bytes 16 - 20
        var pcs = GetFourByteSignature(stream); // bytes 21 - 23
        var dateTime = GetDateTime(stream); // bytes 24 - 35
        var profileFileSignature = GetFourByteSignature(stream); // bytes 36 - 39
        
        if (profileFileSignature != "acsp") throw new InvalidDataException("Profile does not have 'acsp' as the profile file signature");

        var primaryPlatform = GetFourByteSignature(stream); // bytes 40 - 43
        var profileFlags = GetProfileFlags(stream); // bytes 44 - 47
        var deviceManufacturer = GetFourByteSignature(stream); // bytes 48 - 51
        var deviceModel = GetFourByteSignature(stream); // bytes 52 - 55
        var deviceAttributes = GetDeviceAttributes(stream); // bytes 56 - 63
        var renderingIntent = GetRenderingIntent(stream); // bytes 64 - 67
        var pcsIlluminant = GetPcsIlluminant(stream); // bytes 68 - 79
        var profileCreator = GetFourByteSignature(stream); // bytes 80 - 83
        var profileId = GetProfileId(stream); // bytes 84 - 99
        var spectralPcs = GetFourByteSignature(stream); // bytes 100 - 103
        var spectralPcsWavelengthRange = GetSpectralRange(stream); // bytes 104 - 109
        var bispectralPcsWavelengthRange = GetSpectralRange(stream); // bytes 110 - 115
        var mcsSignature = GetFourByteSignature(stream); // bytes 116 - 119
        var profileSubclass = GetFourByteSignature(stream); // bytes 120 - 123
        var reserved = GetUInt32(stream); // bytes 124 - 127

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"[bytes 000 - 003] Profile size: {profileSize}");
        stringBuilder.AppendLine($"[bytes 004 - 007] Preferred CMM type: {preferredCmmType}");
        stringBuilder.AppendLine($"[bytes 008 - 011] Profile version: {profileVersion}");
        stringBuilder.AppendLine($"[bytes 012 - 015] Profile class: {profileClass}");
        stringBuilder.AppendLine($"[bytes 016 - 019] Data colour space: {dataColourSpace}");
        stringBuilder.AppendLine($"[bytes 020 - 023] PCS: {pcs}");
        stringBuilder.AppendLine($"[bytes 024 - 035] Creation date and time: {dateTime}");
        stringBuilder.AppendLine($"[bytes 036 - 039] Profile file signature: {profileFileSignature}");
        stringBuilder.AppendLine($"[bytes 040 - 043] Primary platform: {primaryPlatform}");
        stringBuilder.AppendLine($"[bytes 044 - 047] Profile flags: {profileFlags}");
        stringBuilder.AppendLine($"[bytes 048 - 051] Device manufacturer: {deviceManufacturer}");
        stringBuilder.AppendLine($"[bytes 052 - 055] Device model: {deviceModel}");
        stringBuilder.AppendLine($"[bytes 056 - 063] Device attributes: {deviceAttributes}");
        stringBuilder.AppendLine($"[bytes 064 - 067] Rendering intent: {renderingIntent}");
        stringBuilder.AppendLine($"[bytes 068 - 079] PCS illuminant: {pcsIlluminant}");
        stringBuilder.AppendLine($"[bytes 080 - 083] Profile creator: {profileCreator}");
        stringBuilder.AppendLine($"[bytes 084 - 099] Profile ID: {profileId}");
        stringBuilder.AppendLine($"[bytes 100 - 103] Spectral PCS: {spectralPcs}");
        stringBuilder.AppendLine($"[bytes 104 - 109] Spectral PCS wavelength range: {spectralPcsWavelengthRange}");
        stringBuilder.AppendLine($"[bytes 110 - 115] Bi-spectral PCS wavelength range: {bispectralPcsWavelengthRange}");
        stringBuilder.AppendLine($"[bytes 116 - 119] MCS signature: {mcsSignature}");
        stringBuilder.AppendLine($"[bytes 120 - 123] Profile/device sub-class: {profileSubclass}");
        stringBuilder.AppendLine($"[bytes 124 - 127] Reserved: {reserved}");
        Console.WriteLine(stringBuilder.ToString());
    }
    
    private static string GetProfileVersion(Stream stream)
    {
        var bytes = ReadBytes(stream, fieldLength: 4);
        var majorByte = bytes[0];
        var minorByte = (bytes[1] >> 4) & 0b00001111; // first-half of second byte
        var bugFixByte = bytes[1] & 0b00001111; // second-half of second byte
        // TODO: remaining bytes are sub-class major and sub-class minor, 0 when not associated with a profile
        return $"{majorByte}.{minorByte}.{bugFixByte}";
    }

    private static DateTime GetDateTime(Stream stream)
    {
        var year = GetUInt16(stream);
        var month = GetUInt16(stream);
        var day = GetUInt16(stream);
        var hour = GetUInt16(stream);
        var minute = GetUInt16(stream);
        var second = GetUInt16(stream);
        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
    }

    private static string GetProfileFlags(Stream stream)
    {
        var cmmHints = ReadBytes(stream, fieldLength: 2);
        var iccFlags = ReadBytes(stream, fieldLength: 2);
        var binaryInfo = $"CMM hints {ToBinaryString(cmmHints)}, " +
                         $"ICC flags {ToBinaryString(iccFlags)}";
        
        // first byte of ICC flags is unused but reserved
        var flags = new List<string>
        {
            $"embedded profile = {(iccFlags[1] & 0b0001) != 0}",
            $"use with embedded data only = {(iccFlags[1] & 0b0010) != 0}",
            $"MSC channels are subset = {(iccFlags[1] & 0b0100) != 0}",
            $"extended encoding range = {(iccFlags[1] & 0b1000) != 0}",
        };
        
        return $"{binaryInfo} ({string.Join(", ", flags)})";
    }
    
    private static string GetDeviceAttributes(Stream stream)
    {
        var bytes = ReadBytes(stream, fieldLength: 8);

        var binaryInfo = $"Attributes {ToBinaryString(bytes[0])}";
        var attributes = new List<string>
        {
            (bytes[0] & 0b00000001) == 0 ? "reflective" : "transparency",
            (bytes[0] & 0b00000010) == 0 ? "glossy" : "matte",
            (bytes[0] & 0b00000100) == 0 ? "positive media polarity" : "negative media polarity",
            (bytes[0] & 0b00001000) == 0 ? "colour media" : "black and white media",
            (bytes[0] & 0b00010000) == 0 ? "paper/paperboard" : "non-paper-based",
            (bytes[0] & 0b00100000) == 0 ? "non-textured" : "textured",
            (bytes[0] & 0b00100000) == 0 ? "isotropic" : "non-isotropic",
            (bytes[0] & 0b01000000) == 0 ? "non self-luminous" : "self-luminous"
        };
        // bits 8 - 31 unused but reserved
        // bits 32 - 64 not defined by ICC (vendor-specific)
        return $"{binaryInfo} ({string.Join(", ", attributes)})";
    }
    
    private static string GetRenderingIntent(Stream stream)
    {
        var value = GetUInt32(stream);
        return value switch
        {
            0 => "perceptual",
            1 => "media-relative colorimetric",
            2 => "saturation",
            3 => "ICC-absolute colorimetric",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static (double, double, double) GetPcsIlluminant(Stream stream)
    {
        var x = GetS15Fixed16Number(stream);
        var y = GetS15Fixed16Number(stream);
        var z = GetS15Fixed16Number(stream);
        return (x, y, z);
    }
    
    private static (double, double) GetProfileId(Stream stream)
    {
        var md5Part1 = GetUInt64(stream);
        var md5Part2 = GetUInt64(stream);
        return (md5Part1, md5Part2);
    }
    
    // TODO: spectral range should return floats or doubles
    // but it's not so straightforward to take 2 bytes and convert them to a floating-point "half" precision value
    // would need to assemble my own single-precision value after extracting sign, exponent, mantissa
    // though possible in .NET 8 (https://learn.microsoft.com/en-us/dotnet/api/system.bitconverter.tohalf?view=net-8.0)
    private static (int, int, int) GetSpectralRange(Stream stream)
    {
        var start = GetUInt16(stream);
        var end = GetUInt16(stream);
        var steps = GetUInt16(stream);
        return (start, end, steps);
    }
    
    private static string GetFourByteSignature(Stream stream)
    {
        const int signatureLength = 4;
        var bytes = ReadBytes(stream, signatureLength);
        return AsciiEncoding.GetString(bytes, 0, signatureLength);
    }
    
    // TODO: handle bit conversion manually (like s15Fixed16Number) instead of worrying about endianness?
    private static ushort GetUInt16(Stream stream)
    {
        var bytes = ReadBytes(stream, fieldLength: 2);
        
        // ICC profiles are encoded as big-endian
        // order bytes accordingly when passing to .NET to parse as a whole
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        
        return BitConverter.ToUInt16(bytes, 0);
    }
    
    // TODO: handle bit conversion manually (like s15Fixed16Number) instead of worrying about endianness?
    private static uint GetUInt32(Stream stream)
    {
        var bytes = ReadBytes(stream, fieldLength: 4);
        
        // ICC profiles are encoded as big-endian
        // order bytes accordingly when passing to .NET to parse as a whole
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        
        return BitConverter.ToUInt32(bytes, 0);
    }
    
    // TODO: handle bit conversion manually (like s15Fixed16Number) instead of worrying about endianness?
    private static ulong GetUInt64(Stream stream)
    {
        var bytes = ReadBytes(stream, fieldLength: 8);
        
        // ICC profiles are encoded as big-endian
        // order bytes accordingly when passing to .NET to parse as a whole
        if (BitConverter.IsLittleEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }
        
        return BitConverter.ToUInt64(bytes, 0);
    }
    
    private static double GetS15Fixed16Number(Stream stream)
    {
        var bytes = ReadBytes(stream, fieldLength: 4);

        var isNegative = (bytes[0] & 0b10000000) != 0;

        // shift all appropriate first byte bits 8 to the left (creating 2-byte value) and combine with next byte
        var integer = ((bytes[0] & 0b011111111) << 8) | bytes[1];
        var fraction = ((bytes[2] & 0b111111111) << 8) | bytes[3];
        var number = integer + fraction / 65536.0;
        return isNegative ? -number : number;
    }
    
    private static byte[] ReadBytes(Stream stream, int fieldLength)
    {
        var buffer = new byte[fieldLength];
        var bytesRead = stream.Read(buffer, 0, fieldLength);
        if (bytesRead < fieldLength) throw new InvalidDataException("Could not read enough bytes for field");
        return buffer;
    }
    
    private static string ToBinaryString(IEnumerable<byte> bytes) => string.Join(" ", bytes.Select(ToBinaryString));
    private static string ToBinaryString(byte b) => Convert.ToString(b, 2).PadLeft(8, '0');
}