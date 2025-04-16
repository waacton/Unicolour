using System.Text;

namespace Wacton.Unicolour.Icc;

internal static class DataTypes
{
    private static readonly ASCIIEncoding AsciiEncoding = new();
    
    internal static string ReadSignature(this Stream stream)
    {
        var bytes = stream.ReadBytes(4);
        return AsciiEncoding.GetString(bytes, 0, bytes.Length);
    }
    
    // v5+ / iccMAX:
    // bytes[2] = sub-class major, bytes[3] = sub-class minor
    internal static Version ReadVersion(this Stream stream)
    {
        var bytes = stream.ReadBytes(4);
        var major = bytes[0];
        var minor = (bytes[1] >> 4) & 0b00001111;   // first-half of second byte
        var bugFix = bytes[1] & 0b00001111;         // second-half of second byte
        return new Version(major, minor, bugFix);
    }

    internal static DateTime ReadDateTime(this Stream stream)
    {
        var year = stream.ReadUInt16();
        var month = stream.ReadUInt16();
        var day = stream.ReadUInt16();
        var hour = stream.ReadUInt16();
        var minute = stream.ReadUInt16();
        var second = stream.ReadUInt16();
        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
    }

    internal const string NotEmbedded = "not embedded";
    internal const string Embedded = "embedded";
    internal const string Independent = "independent";
    internal const string NotIndependent = "not independent";
    internal static string[] ReadProfileFlags(this Stream stream)
    {
        // first 16 bits are CMM hints, second 16 bits reserved for ICC
        var flags = stream.ReadUInt32();
        return new[]
        {
            (flags & 0b0001) == 0 ? NotEmbedded : Embedded,
            (flags & 0b0010) == 0 ? Independent : NotIndependent
            
            // v5+ / iccMAX:
            // (flags & 0b0100) == 0 ? "MSC subset" : "not MSC subset",
            // (flags & 0b1000) == 0 ? "extended encoding range" : "not extended encoding range"
        };
    }
    
    internal const string Reflective = "reflective";
    internal const string Transparency = "transparency";
    internal const string Glossy = "glossy";
    internal const string Matte = "matte";
    internal const string Positive = "positive media polarity";
    internal const string Negative = "negative media polarity";
    internal const string Colour = "colour media";
    internal const string BlackAndWhite = "black and white media";

    internal static string[] ReadDeviceAttributes(this Stream stream)
    {
        // first 32 bits are vendor specific, second 32 bits reserved for ICC
        var flags = stream.ReadUInt64();
        return new[]
        {
            (flags & 0b00000001) == 0 ? Reflective : Transparency,
            (flags & 0b00000010) == 0 ? Glossy : Matte,
            (flags & 0b00000100) == 0 ? Positive : Negative,
            (flags & 0b00001000) == 0 ? Colour : BlackAndWhite
            
            // v5+ / iccMAX:
            // (flags & 0b00010000) == 0 ? "paper/paperboard" : "non-paper-based",
            // (flags & 0b00100000) == 0 ? "non-textured" : "textured",
            // (flags & 0b01000000) == 0 ? "isotropic" : "non-isotropic",
            // (flags & 0b10000000) == 0 ? "non self-luminous" : "self-luminous"
        };
    }

    // v5+ / iccMAX
    // should be (float, float, int)
    // but it's not so straightforward to take 2 bytes and convert them to a floating-point "half" precision value
    // would need to assemble my own single-precision value after extracting sign, exponent, mantissa
    // sadly much more easily possible in .NET 8 (https://learn.microsoft.com/en-us/dotnet/api/system.bitconverter.tohalf?view=net-8.0)
    // internal static byte[] ReadSpectralRange(this Stream stream)
    // {
    //     return stream.ReadBytes(6);
    // }
    
    internal static (double x, double y, double z) ReadXyzNumber(this Stream stream)
    {
        var x = stream.ReadS15Fixed16();
        var y = stream.ReadS15Fixed16();
        var z = stream.ReadS15Fixed16();
        return (x, y, z);
    }
    
    internal static XyzType ReadXyzType(this Stream stream)
    {
        stream.ReadSignature();
        stream.ReadBytes(4); // reserved
        var (x, y, z) = stream.ReadXyzNumber();
        return new XyzType(x, y, z);
    }
    
    internal static double[] ToArray(this (double x, double y, double z) tuple)
    {
        return new [] { tuple.x, tuple.y, tuple.z };
    }
}

// the ICC specification distinguishes between "XyzNumber" and "XyzType"
// but the main benefit for this implementation is that it's a reference type
// and easily handled as null if the tag is not present
// (as opposed to default values of 0, which can be misconstrued as existing and set to zero)
internal record XyzType(double x, double y, double z)
{
    internal double x { get; } = x;
    internal double y { get; } = y;
    internal double z { get; } = z;
    
    internal double[] ToArray() => new [] { x, y, z };
    internal (double x, double y, double z) ToTuple() => (x, y, z);
}