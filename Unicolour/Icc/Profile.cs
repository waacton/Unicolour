using System.Security.Cryptography;

namespace Wacton.Unicolour.Icc;

public class Profile
{
    private static readonly double[] RefWhite = { 0.9642, 1.0000, 0.8249 };
    private static readonly double[] RefBlack = { 0.00336, 0.0034731, 0.00287 };
    internal static readonly XyzConfiguration XyzD50 = new(WhitePoint.FromXyz(new Xyz(RefWhite[0], RefWhite[1], RefWhite[2])), "ICC XYZ D50");
    
    public FileInfo FileInfo { get; }
    public Header Header { get; }
    public Tags Tags { get; }
    
    private double[] mediaWhite => Tags.MediaWhite.Value.ToArray();
    
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
    }
    
    internal Xyz ToXyz(double[] deviceValues, XyzConfiguration xyzConfig, Intent intent)
    {
        var xyzD50 = ToXyzStandardD50(deviceValues, intent);
        var xyzD50Matrix = new Matrix(new[,] { { xyzD50[0] }, { xyzD50[1] }, { xyzD50[2] } });
        var (x, y, z) = Adaptation.WhitePoint(xyzD50Matrix, XyzD50.WhitePoint, xyzConfig.WhitePoint).ToTriplet().Tuple;
        return new Xyz(x, y, z);
    }
    
    internal double[] FromXyz(Xyz xyz, XyzConfiguration xyzConfig, Intent intent)
    {
        var xyzMatrix = Matrix.FromTriplet(xyz.Triplet);
        var xyzD50 = Adaptation.WhitePoint(xyzMatrix, xyzConfig.WhitePoint, XyzD50.WhitePoint).ToTriplet().ToArray();
        return FromStandardXyzD50(xyzD50, intent);
    }
    
    // NOTE: iccMAX allows "profile connection conditions" (customToStandardPCC, standardToCustomPCC)
    // but if it is ever implemented, it probably doesn't change this device-to-"StandardD50" calculation
    internal double[] ToXyzStandardD50(double[] deviceValues, Intent intent)
    {
        var luts = Tags.GetLuts(intent, isDeviceToPcs: true);
        var pcsValues = luts.Apply(deviceValues);
        return Header.Pcs == Signatures.Lab 
            ? Convert.IccLabToXyz(pcsValues, intent, luts.LutType, RefBlack, RefWhite, mediaWhite, Header.ProfileVersion.Major) 
            : Convert.IccXyzToXyz(pcsValues, intent, RefWhite, mediaWhite);
    }
    
    // NOTE: iccMAX allows "profile connection conditions" (customToStandardPCC, standardToCustomPCC)
    // but if it is ever implemented, it probably doesn't change this "StandardD50"-to-device calculation
    internal double[] FromStandardXyzD50(double[] xyzD50, Intent intent)
    {
        var luts = Tags.GetLuts(intent, isDeviceToPcs: false);
        var pcsValues = Header.Pcs == Signatures.Lab 
            ? Convert.XyzToIccLab(xyzD50, intent, luts.LutType, RefBlack, RefWhite, mediaWhite, Header.ProfileVersion.Major) 
            : Convert.XyzToIccXyz(xyzD50, intent, RefWhite, mediaWhite);

        return luts.Apply(pcsValues);
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
    
    internal void ErrorIfUnsupported(Intent intent)
    {
        if (Header.ProfileFileSignature != Signatures.Profile)
        {
            throw new ArgumentException($"[{FileInfo}] signature is incorrect: expected [{Signatures.Profile}] but was [{Header.ProfileFileSignature}]");
        }
        
        ErrorIfUnsupportedHeader();
        ErrorIfUnsupportedIntent(intent);
    }

    private void ErrorIfUnsupportedHeader()
    {
        var isHeaderSupported = Header is
        {
            Pcs: Signatures.Lab or Signatures.Xyz,
            ProfileClass: Signatures.Output or Signatures.ColourSpace
        };
        
        if (isHeaderSupported) return;
        const string expected = $"PCS {Signatures.Lab} or {Signatures.Xyz}, Profile class {Signatures.Output} or {Signatures.ColourSpace}";
        var actual = $"PCS {Header.Pcs}, Class {Header.ProfileClass}";
        throw new ArgumentException($"[{FileInfo}] is not supported: expected [{expected}] but was [{actual}]");
    }

    private void ErrorIfUnsupportedIntent(Intent intent)
    {
        // AToB0 is used as a fallback regardless of intent
        var requiredSignatures = new List<string> { Signatures.AToB0, Signatures.BToA0 };
        if (intent == Intent.AbsoluteColorimetric)
        {
            requiredSignatures.Add(Signatures.MediaWhitePoint);
        }
        
        var signatures = Tags.Select(x => x.Signature).ToList();
        var missing = new List<string>();
        foreach (var requiredSignature in requiredSignatures)
        {
            var isMissing = !signatures.Contains(requiredSignature);
            if (isMissing)
            {
                missing.Add(requiredSignature);
            }
        }

        if (!missing.Any()) return;
        throw new ArgumentException($"[{FileInfo}] with intent [{intent}] is not supported: missing required tags [{string.Join(" · ", missing)}]");
    }

    public override string ToString() => $"{FileInfo} · {Header} · {Tags.Count} tags";
}
