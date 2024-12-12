namespace Wacton.Unicolour.Icc;

public class Tags : List<Tag>
{
    internal Lazy<Luts?> AToB0 { get; }
    internal Lazy<Luts?> AToB1 { get; }
    internal Lazy<Luts?> AToB2 { get; }
    internal Lazy<Luts?> BToA0 { get; }
    internal Lazy<Luts?> BToA1 { get; }
    internal Lazy<Luts?> BToA2 { get; }
    internal Lazy<(double x, double y, double z)> MediaWhite { get; }

    private Tags()
    {
        AToB0 = new Lazy<Luts?>(() => Read(Signatures.AToB0, Luts.AToBFromStream));
        AToB2 = new Lazy<Luts?>(() => Read(Signatures.AToB2, Luts.AToBFromStream));
        AToB1 = new Lazy<Luts?>(() => Read(Signatures.AToB1, Luts.AToBFromStream));
        BToA0 = new Lazy<Luts?>(() => Read(Signatures.BToA0, Luts.BToAFromStream));
        BToA1 = new Lazy<Luts?>(() => Read(Signatures.BToA1, Luts.BToAFromStream));
        BToA2 = new Lazy<Luts?>(() => Read(Signatures.BToA2, Luts.BToAFromStream));
        MediaWhite = new Lazy<(double x, double y, double z)>(() => Read(Signatures.MediaWhitePoint, DataTypes.ReadXyzType));
    }
    
    internal Luts GetLuts(Intent intent, bool isDeviceToPcs)
    {
        /*
         * LUT tag precedence
         * 1) use BToD* and DToB* if present, except where not needed [v5+ / iccMax also has BToD3 and DToB3 for absolute]
         * 2) use BToA* and AToB* if present, when tag in 1) is not used
         * 3) use BToA0 and AToB0 if present, when tags in 1), 2) are not used
         * 4) use TRCs when tags in 1), 2), 3) are not used
         */
        return isDeviceToPcs
            ? intent switch
            {
                Intent.Perceptual => AToB0.Value!,
                Intent.RelativeColorimetric => AToB1.Value ?? AToB0.Value!,
                Intent.Saturation => AToB2.Value ?? AToB0.Value!,
                Intent.AbsoluteColorimetric => AToB1.Value ?? AToB0.Value!,
                _ => throw new ArgumentOutOfRangeException(nameof(intent), intent, null)
            }
            : intent switch
            {
                Intent.Perceptual => BToA0.Value!,
                Intent.RelativeColorimetric => BToA1.Value ?? BToA0.Value!,
                Intent.Saturation => BToA2.Value ?? BToA0.Value!,
                Intent.AbsoluteColorimetric => BToA1.Value ?? BToA0.Value!,
                _ => throw new ArgumentOutOfRangeException(nameof(intent), intent, null)
            };
    }

    private T? Read<T>(string signature, Func<Stream, T> read)
    {
        var tag = this.SingleOrDefault(x => x.Signature == signature);
        if (tag == null) return default;
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