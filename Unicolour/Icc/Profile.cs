using System.Security.Cryptography;

namespace Wacton.Unicolour.Icc;

public class Profile
{
    public FileInfo FileInfo { get; }
    public Header Header { get; }
    public Tags Tags { get; }

    internal Transform Transform { get; }
    
    public Profile(string filePath)
    {
        FileInfo = new FileInfo(filePath);
        if (FileInfo.Length < 128)
        {
            throw new ArgumentException($"[{FileInfo}] does not contain enough bytes to be an ICC profile");
        }

        try
        {
            Header = Header.FromFile(FileInfo);
            Tags = Tags.FromFile(FileInfo);
        }
        catch (Exception e)
        {
            throw new ArgumentException($"[{FileInfo}] could not be parsed as ICC data", e);
        }

        Transform = GetTransform();
    }
    
    internal Xyz ToXyz(double[] deviceValues, XyzConfiguration xyzConfig, Intent intent)
    {
        // NOTE: iccMAX allows "profile connection conditions" (customToStandardPCC, standardToCustomPCC)
        // but if it is ever implemented, it probably doesn't change this device-to-"StandardD50" calculation
        var xyzD50 = Transform.ToXyz(deviceValues, intent);
        var xyzD50Matrix = new Matrix(new[,] { { xyzD50[0] }, { xyzD50[1] }, { xyzD50[2] } });
        var (x, y, z) = Adaptation.WhitePoint(xyzD50Matrix, Transform.XyzD50.WhitePoint, xyzConfig.WhitePoint).ToTriplet().Tuple;
        return new Xyz(x, y, z);
    }
    
    internal double[] FromXyz(Xyz xyz, XyzConfiguration xyzConfig, Intent intent)
    {
        // NOTE: iccMAX allows "profile connection conditions" (customToStandardPCC, standardToCustomPCC)
        // but if it is ever implemented, it probably doesn't change this "StandardD50"-to-device calculation
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var xyzD50 = Adaptation.WhitePoint(xyzMatrix, xyzConfig.WhitePoint, Transform.XyzD50.WhitePoint).ToTriplet().ToArray();
        return Transform.FromXyz(xyzD50, intent);
    }
    
    private static readonly int[] indexesToZeroForHash = { 44, 45, 46, 47, 64, 65, 66, 67, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
    public byte[] CalculateProfileId()
    {
        var bytes = File.ReadAllBytes(FileInfo.FullName);
        foreach (var index in indexesToZeroForHash)
        {
            bytes[index] = 0;
        }
        
        var md5 = MD5.Create();
        return md5.ComputeHash(bytes);
    }
    
    internal void ErrorIfUnsupported()
    {
        if (Header.ProfileFileSignature != Signatures.Profile)
        {
            throw new ArgumentException($"[{FileInfo}] signature is incorrect: expected [{Signatures.Profile}] but was [{Header.ProfileFileSignature}]");
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
        throw new ArgumentException($"[{FileInfo}] is not supported: expected [{expected}] but was [{actual}]");
    }
    
    private void ErrorIfUnsupportedTransform()
    {
        var isTransformSupported = Transform is TransformAToB or TransformTrcMatrix or TransformTrcGrey;
        
        if (isTransformSupported) return;
        const string expected = $"transform {nameof(TransformAToB)} or {nameof(TransformTrcMatrix)} or {nameof(TransformTrcGrey)}";
        var actual = $"transform {Transform.GetType().Name}";
        throw new ArgumentException($"[{FileInfo}] is not supported: expected [{expected}] but was [{actual}]");
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

    public override string ToString() => $"{FileInfo} · {Header} · {Tags.Count} tags";
}
