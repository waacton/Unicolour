namespace Wacton.Unicolour.Icc;

internal static class NumberTypes
{
    /*
     * parsing unsigned numbers is easily done with BitConverter.ToUInt16() etc.
     * however, would need to be careful: ICC profiles are encoding as big-endian
     * so would always need to check the computer's architecture using BitConverter.IsLittleEndian
     * and reverse the byte array before converting it
     * to reduce risk of issues due to endianness, these functions build numbers by bitshifting
     * which is less readable but precisely controlled
     * ----------
     * also note: .NET Standard 2.1 has `BinaryPrimitives.ReadUInt32BigEndian()` 🥺
     */
    
    internal static ushort ReadUInt16(this Stream stream)
    {
        var bytes = stream.ReadBytes(2);
        var integer = (bytes[0] << 8) | bytes[1];
        return (ushort)integer;
    }
    
    internal static uint ReadUInt32(this Stream stream)
    {
        var bytes = stream.ReadBytes(4);
        var integer = (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
        return (uint)integer;
    } 
    
    internal static ulong ReadUInt64(this Stream stream)
    {
        var bytes = stream.ReadBytes(8).Select(x => (long)x).ToArray();
        var integer = (bytes[0] << 56) | (bytes[1] << 48) | (bytes[2] << 40) | (bytes[3] << 32) | 
                      (bytes[4] << 24) | (bytes[5] << 16) | (bytes[6] << 8) | bytes[7];
        return (ulong)integer;
    }
    
    internal static byte ReadUInt8(this Stream stream)
    {
        return (byte)stream.ReadByte();
    }
    
    internal static double ReadS15Fixed16(this Stream stream)
    {
        var bytes = stream.ReadBytes(4);
        // shift first byte bits 8 to the left to create 16-bit value and combine with next byte
        var integer = (short)((bytes[0] << 8) | bytes[1]);
        var fraction = (double)((bytes[2] << 8) | bytes[3]) / 65536.0;
        return integer + fraction;
    }
    
    internal static byte[] ReadBytes(this Stream stream, int count)
    {
        var buffer = new byte[count];
        var bytesRead = stream.Read(buffer, 0, count);
        if (bytesRead < count) throw new InvalidDataException("Not enough bytes in stream");
        return buffer;
    }
    
    internal static T[] ReadArray<T>(this Stream stream, Func<Stream, T> readNext, int count)
    {
        var numbers = new List<T>();
        while (numbers.Count < count)
        {
            var number = readNext(stream);
            numbers.Add(number);
        }

        return numbers.ToArray();
    }
}