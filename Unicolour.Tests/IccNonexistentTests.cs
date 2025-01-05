using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Wacton.Unicolour.Icc;
using Wacton.Unicolour.Tests.Utils;

namespace Wacton.Unicolour.Tests;

/*
 * tests behaviour that I can't find real ICC profiles for
 * so are implemented as part of the specification but with no data for validation
 * delete if suitable profiles are ever found
 */
public class IccNonexistentTests
{
    /* not found a profile that uses only B-curves in LutAToB or LutBToA */
    [Test]
    public void BCurvesOnly()
    {
        const string path = "b_curves_only.icc";
        WriteProfileWithBCurvesOnly(path);
        var profile = new Profile(path);

        Assert.That(profile.Tags.AToB0.Value!.ToString(), Is.EqualTo("B [x3]"));
        Assert.That(profile.Tags.BToA0.Value!.ToString(), Is.EqualTo("B [x3]"));

        double[] deviceValues = [0.25, 0.5, 0.75];
        var xyz = profile.Transform.ToXyz(deviceValues, Intent.Perceptual);
        var roundtrip = profile.Transform.FromXyz(xyz, Intent.Perceptual);
        Assert.That(roundtrip, Is.EqualTo(deviceValues));
    }
    
    private static void WriteProfileWithBCurvesOnly(string modifiedPath)
    {
        var profile = IccFile.RommRgb.GetProfile();
        var a2b0 = profile.Tags.Single(x => x.Signature == Signatures.AToB0);
        var b2a0 = profile.Tags.Single(x => x.Signature == Signatures.BToA0);
     
        // set the offset of every LUT element except B curves to 0, indicating that they are not present
        // (see Luts.ReadTablesAB for details)
        
        var bytes = File.ReadAllBytes(profile.FileInfo.FullName);
        byte[] bytesToZero =
        [
            16, 17, 18, 19, // matrix offset
            20, 21, 22, 23, // M curves offset
            24, 25, 26, 27, // CLUT offset
            28, 29, 30, 31  // A curves offset
        ];

        foreach (var i in bytesToZero)
        {
            bytes[a2b0.Offset + i] = 0;
            bytes[b2a0.Offset + i] = 0;
        }
        
        File.WriteAllBytes(modifiedPath, bytes);
    }
    
    /* not found a profile that uses an 8-bit CLUT in LutAToB or LutBToA */
    [Test]
    public void Clut8Bit()
    {
        var profile = IccFile.Swop2013.GetProfile();
        var a2b0 = profile.Tags.Single(x => x.Signature == Signatures.AToB0);
        var b2a0 = profile.Tags.Single(x => x.Signature == Signatures.BToA0);
        
        // CLUT data start at byte 80, byte 16 is the precision
        // force clut precision to be 8-bit (1 = 8-bit, 2 = 16-bit)
        var bytes = File.ReadAllBytes(profile.FileInfo.FullName);
        bytes[a2b0.Offset + 80 + 16] = 1; 
        bytes[b2a0.Offset + 80 + 16] = 1;
        
        const string modifiedName = "modified_clut_precision.icc";
        File.WriteAllBytes(modifiedName, bytes);

        var modifiedProfile = new Profile(modifiedName);
        Assert.DoesNotThrow(() => modifiedProfile.Transform.ToXyz([0.2, 0.4, 0.6, 0.8], Intent.Perceptual));
        Assert.DoesNotThrow(() => modifiedProfile.Transform.FromXyz([0.25, 0.5, 0.75], Intent.Perceptual));
        File.Delete(modifiedName);
    }
    
    /* not found a profile that uses a table curve ('curv') consisting of a single value */
    [Test]
    public void CurveWithSingleEntry()
    {
        var curveType = typeof(Curve);
        var privateReadCurve = curveType.GetMethod("ReadCurve", BindingFlags.NonPublic | BindingFlags.Static)!;
        
        const byte gamma = 2;
        var stream = new MemoryStream([
            0, 0, 0, 0, // ignored (signature)
            0, 0, 0, 0, // ignored (reserved)
            0, 0, 0, 1, // number of entries (uInt32Number)
            gamma, 0    // gamma (u8Fixed8Number)
        ]);
        
        var parametricCurve = (ParametricCurve)privateReadCurve.Invoke(null, [stream])!; // null parameter because it is static
        
        // curve function is y = x ^ gamma
        var result = parametricCurve.Lookup(8);
        Assert.That(result, Is.EqualTo(64));
    }
    
    /* not found a profile that uses a parametric curve ('para') encoded as 0002h (IEC 61966‐3) */
    [Test]
    public void ParametricCurveWithEncodedValue2()
    {
        var curveType = typeof(Curve);
        var privateReadParametricCurve = curveType.GetMethod("ReadParametricCurve", BindingFlags.NonPublic | BindingFlags.Static)!;
        
        const byte g = 1;
        const byte a = 2;
        const byte b = 3;
        const byte c = 4;
        var stream = new MemoryStream([
            0, 0, 0, 0, // ignored (signature)
            0, 0, 0, 0, // ignored (reserved)
            0, 2,       // function type (uInt16Number)
            0, 0,       // ignored (reserved)
            0, g, 0, 0, // g (s15Fixed16Number)
            0, a, 0, 0, // a (s15Fixed16Number)
            0, b, 0, 0, // b (s15Fixed16Number)
            0, c, 0, 0  // c (s15Fixed16Number)
        ]);
        
        var parametricCurve = (ParametricCurve)privateReadParametricCurve.Invoke(null, [stream])!; // null parameter because it is static
        
        // curve function is y = (ax + b) ^ g + c
        var resultPositive = parametricCurve.Lookup(8);
        Assert.That(resultPositive, Is.EqualTo(23));

        // curve function is y = c
        var resultNegative = parametricCurve.Lookup(-8);
        Assert.That(resultNegative, Is.EqualTo(4));
    }
}