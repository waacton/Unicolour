using System;
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
        const int channels = 15;
        var lutsAB = CreateLutsBCurvesOnly(isAToB: true, channels);        
        var lutsBA = CreateLutsBCurvesOnly(isAToB: false, channels);
        Assert.That(lutsAB.ToString(), Is.EqualTo($"B [x{channels}]"));
        Assert.That(lutsBA.ToString(), Is.EqualTo($"B [x{channels}]"));

        var inputs = Enumerable.Range(0, channels).Select(_ => 0.5).ToArray();
        var outputs = lutsAB.Apply(inputs);
        var roundtrip = lutsBA.Apply(outputs);
        Assert.That(roundtrip, Is.EqualTo(inputs).Within(0.00005));
    }

    private static Luts CreateLutsBCurvesOnly(bool isAToB, int channels)
    {
        var bCurvesToPcs = Enumerable.Range(1, channels).Select(value => GenerateTableCurve(power: 1 + value / (double)channels, isAToB)).ToList();
        return new Luts(ACurves: null, Clut: null, MCurves: null, Matrices: null, bCurvesToPcs, isAToB ? LutType.LutAB : LutType.LutBA, isAToB);
    }

    private static Curve GenerateTableCurve(double power, bool isAToB)
    {
        var table = new double[100];
        for (var i = 0; i < table.Length; i++)
        {
            table[i] = isAToB ? Math.Pow(i, power) : Math.Pow(i,  1 / power);
        }

        table = table.Select(x => x / table.Max()).ToArray();
        return new TableCurve(table);
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
        Assert.DoesNotThrow(() => modifiedProfile.ToXyzStandardD50([0.2, 0.4, 0.6, 0.8], Intent.Perceptual));
        Assert.DoesNotThrow(() => modifiedProfile.FromStandardXyzD50([0.25, 0.5, 0.75], Intent.Perceptual));
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