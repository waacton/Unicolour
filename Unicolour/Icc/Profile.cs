using System.Security.Cryptography;

namespace Wacton.Unicolour.Icc;

public class Profile
{
    public string Name { get; }
    public long Length { get; }
    public Header Header { get; }
    public Tags Tags { get; }
    internal Transform Transform { get; }
    
    // note that this constructor is creating a stream without `using`, instead the private constructor closes the stream
    // not best practice, but it makes code reuse much easier, and `closeStream` is only triggered here and not exposed
    public Profile(string filePath, string? name = null) : this(File.OpenRead(filePath), name ?? Path.GetFileNameWithoutExtension(filePath), closeStream: true) {}
    public Profile(byte[] bytes, string name = "from bytes") : this(new MemoryStream(bytes), name) {}
    public Profile(Stream stream, string name = "from stream") : this(stream, name, closeStream: false) {}
    private Profile(Stream stream, string name, bool closeStream)
    {
        try
        {
            Name = name;
            Length = stream.Length;
        
            if (Length < 128)
            {
                throw new ArgumentException($"[{Name}] does not contain enough bytes to be an ICC profile");
            }

            try
            {
                Header = new Header(stream);
                Tags = new Tags(stream);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"[{Name}] could not be parsed as ICC data", e);
            }
            
            Transform = GetTransform();
        }
        finally
        {
            // not best practice, see comment above constructors - only used when file path provided
            if (closeStream) stream.Dispose();
        }
    }
    
    internal Xyz ToXyz(double[] deviceValues, XyzConfiguration xyzConfig, Intent intent)
    {
        // NOTE: iccMAX allows "profile connection conditions" (customToStandardPCC, standardToCustomPCC)
        // but if it is ever implemented, it probably doesn't change this device-to-"StandardD50" calculation
        var xyzD50 = Transform.ToXyz(deviceValues, intent);
        var xyz = new Xyz(xyzD50[0], xyzD50[1], xyzD50[2]);
        return Adaptation.WhitePoint(xyz, Transform.XyzD50.WhitePoint, xyzConfig.WhitePoint, xyzConfig.AdaptationMatrix);
    }
    
    internal double[] FromXyz(Xyz xyz, XyzConfiguration xyzConfig, Intent intent)
    {
        // NOTE: iccMAX allows "profile connection conditions" (customToStandardPCC, standardToCustomPCC)
        // but if it is ever implemented, it probably doesn't change this "StandardD50"-to-device calculation
        var xyzD50 = Adaptation.WhitePoint(xyz, xyzConfig.WhitePoint, Transform.XyzD50.WhitePoint, xyzConfig.AdaptationMatrix).ToArray();
        return Transform.FromXyz(xyzD50, intent);
    }
    
    internal void ErrorIfUnsupported()
    {
        if (Header.ProfileFileSignature != Signatures.Profile)
        {
            throw new ArgumentException($"[{Name}] signature is incorrect: expected [{Signatures.Profile}] but was [{Header.ProfileFileSignature}]");
        }
        
        ErrorIfUnsupportedHeader();
        ErrorIfUnsupportedTransform();
    }

    private void ErrorIfUnsupportedHeader()
    {
        var isHeaderSupported = Header is
        {
            ProfileClass: Signatures.Input or Signatures.Display or Signatures.Output or Signatures.ColourSpace
        };
        
        if (isHeaderSupported) return;
        const string expected = $"profile class {Signatures.Input} or {Signatures.Display} or {Signatures.Output} or {Signatures.ColourSpace}";
        var actual = $"profile class {Header.ProfileClass}";
        throw new ArgumentException($"[{Name}] is not supported: expected [{expected}] but was [{actual}]");
    }
    
    private void ErrorIfUnsupportedTransform()
    {
        var isTransformSupported = Transform is TransformAToB or TransformTrcMatrix or TransformTrcGrey;
        
        if (isTransformSupported) return;
        const string expected = $"transform {nameof(TransformAToB)} or {nameof(TransformTrcMatrix)} or {nameof(TransformTrcGrey)}";
        var actual = $"transform {Transform.GetType().Name}";
        throw new ArgumentException($"[{Name}] is not supported: expected [{expected}] but was [{actual}]");
    }
    
    /*
     * transform tag precedence for input, display, output, or colour space profile types
     * 1) use BToD* and DToB* if present, except where not needed [v5+ / iccMax also has BToD3 and DToB3 for absolute]
     * 2) use BToA* and AToB* if present, when tag in 1) is not used
     * 3) use BToA0 and AToB0 if present, when tags in 1), 2) are not used
     * 4) use TRCs when tags in 1), 2), 3) are not used
     * ----------
     * device link and abstract profile types precedence is 1) DToB0 2) AToB0, but these are not currently supported
     */
    private Transform GetTransform()
    {
        if (Tags.HasAny(Signatures.DToB0, Signatures.DToB1, Signatures.DToB2, Signatures.DToB3))
        {
            return new TransformDToB(Header, Tags);
        }
        
        if (Tags.Has(Signatures.AToB0)) // AToB0 is used as a fallback regardless of intent when AToB1 or AToB2 is missing
        {
            return new TransformAToB(Header, Tags);
        }

        if (Tags.HasAll(Signatures.RedTrc, Signatures.GreenTrc, Signatures.BlueTrc, Signatures.RedMatrixColumn, Signatures.GreenMatrixColumn, Signatures.BlueMatrixColumn))
        {
            return new TransformTrcMatrix(Header, Tags);
        }

        if (Tags.Has(Signatures.GreyTrc))
        {
            return new TransformTrcGrey(Header, Tags);
        }

        return new TransformNone(Header, Tags);
    }
    
    private static readonly int[] IndexesToZeroForHash = { 44, 45, 46, 47, 64, 65, 66, 67, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
    public static byte[] CalculateId(byte[] bytes)
    {
        foreach (var index in IndexesToZeroForHash)
        {
            bytes[index] = 0;
        }
        
        var md5 = MD5.Create();
        return md5.ComputeHash(bytes);
    }

    public override string ToString() => $"{Name} · {Header} · {Tags.Count} tags";
}
