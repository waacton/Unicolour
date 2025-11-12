namespace Wacton.Unicolour.Icc;

public class Tags : List<Tag>
{
    internal Lazy<Luts?> AToB0 { get; }
    internal Lazy<Luts?> AToB1 { get; }
    internal Lazy<Luts?> AToB2 { get; }
    internal Lazy<Luts?> BToA0 { get; }
    internal Lazy<Luts?> BToA1 { get; }
    internal Lazy<Luts?> BToA2 { get; }

    internal Lazy<XyzType?> RedMatrixColumn { get; }
    internal Lazy<XyzType?> GreenMatrixColumn { get; }
    internal Lazy<XyzType?> BlueMatrixColumn { get; }
    internal Lazy<Curve?> RedTrc { get; }
    internal Lazy<Curve?> GreenTrc { get; }
    internal Lazy<Curve?> BlueTrc { get; }
    
    internal Lazy<Curve?> GreyTrc { get; }
    
    internal Lazy<XyzType?> MediaWhite { get; }
    
    internal Tags(Stream stream)
    {
        /* initialisation of tags just gathers the raw byte data, ready for parsing later on when needed */
        stream.Seek(128, SeekOrigin.Begin);         // tag table begins at byte 128
        var tagCount = stream.ReadUInt32();         // bytes 0 - 3
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
            Add(tag);
        }

        /* tags used in transforms have their data parsed lazily as needed */
        AToB0 = new Lazy<Luts?>(() => Read(Signatures.AToB0, Luts.AToBFromStream));
        AToB2 = new Lazy<Luts?>(() => Read(Signatures.AToB2, Luts.AToBFromStream));
        AToB1 = new Lazy<Luts?>(() => Read(Signatures.AToB1, Luts.AToBFromStream));
        BToA0 = new Lazy<Luts?>(() => Read(Signatures.BToA0, Luts.BToAFromStream));
        BToA1 = new Lazy<Luts?>(() => Read(Signatures.BToA1, Luts.BToAFromStream));
        BToA2 = new Lazy<Luts?>(() => Read(Signatures.BToA2, Luts.BToAFromStream));
        RedMatrixColumn = new Lazy<XyzType?>(() => Read(Signatures.RedMatrixColumn, DataTypes.ReadXyzType ));
        GreenMatrixColumn = new Lazy<XyzType?>(() => Read(Signatures.GreenMatrixColumn, DataTypes.ReadXyzType));
        BlueMatrixColumn = new Lazy<XyzType?>(() => Read(Signatures.BlueMatrixColumn, DataTypes.ReadXyzType));
        RedTrc = new Lazy<Curve?>(() => Read(Signatures.RedTrc, Curve.FromStream));
        GreenTrc = new Lazy<Curve?>(() => Read(Signatures.GreenTrc, Curve.FromStream));
        BlueTrc = new Lazy<Curve?>(() => Read(Signatures.BlueTrc, Curve.FromStream));
        GreyTrc = new Lazy<Curve?>(() => Read(Signatures.GreyTrc, Curve.FromStream));
        MediaWhite = new Lazy<XyzType?>(() => Read(Signatures.MediaWhitePoint, DataTypes.ReadXyzType));
    }
    
    internal bool Has(string signature) => this.Any(x => x.Signature == signature);
    internal bool HasAll(params string[] signatures) => signatures.All(Has);
    internal bool HasAny(params string[] signatures) => signatures.Any(Has);

    private T? Read<T>(string signature, Func<Stream, T> read)
    {
        var tag = this.SingleOrDefault(x => x.Signature == signature);
        if (tag == null) return default;
        using var stream = new MemoryStream(tag.Data);
        var result = read(stream);
        return result;
    }
}