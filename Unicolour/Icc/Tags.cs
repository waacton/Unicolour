namespace Wacton.Unicolour.Icc;

public class Tags : List<Tag>
{
    internal Lazy<Luts> AToB0 { get; }
    internal Lazy<Luts> AToB1 { get; }
    internal Lazy<Luts> AToB2 { get; }
    internal Lazy<Luts> BToA0 { get; }
    internal Lazy<Luts> BToA1 { get; }
    internal Lazy<Luts> BToA2 { get; }
    internal Lazy<(double x, double y, double z)> MediaWhite { get; }

    private Tags()
    {
        AToB0 = new Lazy<Luts>(() => Read(Signatures.AToB0, Luts.FromStream));
        AToB1 = new Lazy<Luts>(() => Read(Signatures.AToB1, Luts.FromStream));
        AToB2 = new Lazy<Luts>(() => Read(Signatures.AToB2, Luts.FromStream));
        BToA0 = new Lazy<Luts>(() => Read(Signatures.BToA0, Luts.FromStream));
        BToA1 = new Lazy<Luts>(() => Read(Signatures.BToA1, Luts.FromStream));
        BToA2 = new Lazy<Luts>(() => Read(Signatures.BToA2, Luts.FromStream));
        MediaWhite = new Lazy<(double x, double y, double z)>(() => Read(Signatures.MediaWhitePoint, DataTypes.ReadXyzType));
    }

    internal Luts GetLuts(string signature)
    {
        return signature switch
        {
            Signatures.AToB0 => AToB0.Value,
            Signatures.AToB1 => AToB1.Value,
            Signatures.AToB2 => AToB2.Value,
            Signatures.BToA0 => BToA0.Value,
            Signatures.BToA1 => BToA1.Value,
            Signatures.BToA2 => BToA2.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(signature), signature, null)
        };
    }

    private T Read<T>(string signature, Func<Stream, T> read)
    {
        var tag = this.Single(x => x.Signature == signature);
        using var stream = new MemoryStream(tag.Data);
        var result = read(stream);
        return result;
    }
    
    internal static Tags FromFile(FileInfo fileInfo)
    {
        using var stream = fileInfo.OpenRead();
        stream.Seek(128, SeekOrigin.Begin); // tag table begins at byte 128

        var tagCount = stream.ReadUInt32();         // bytes 0 - 3

        var tags = new Tags();
        for (var i = 0; i < tagCount; i++)
        {
            var signature = stream.ReadSignature(); // bytes 4 - 7 (and repeating)
            var offset = stream.ReadUInt32();       // bytes 8 - 11 (and repeating)
            var size = stream.ReadUInt32();         // bytes 12 - 15 (and repeating)

            // override the stream's position to read the data
            // then restore the position for the next tag
            var streamPosition = stream.Position;
            stream.Seek(offset, SeekOrigin.Begin);
            var data = stream.ReadBytes((int)size);
            stream.Seek(streamPosition, SeekOrigin.Begin);
            
            var tag = new Tag(signature, offset, size, data);
            tags.Add(tag);
        }

        return tags;
    }
}