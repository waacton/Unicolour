namespace Wacton.Unicolour.Icc;

internal static class Signatures
{
    internal const string Profile = "acsp";
    
    internal const string Cmyk = "CMYK";
    internal const string Clr7 = "7CLR";
    internal const string Rgb = "RGB ";
    internal const string Grey = "GRAY";
    
    internal const string Lab = "Lab ";
    internal const string Xyz = "XYZ ";

    internal const string Input = "scnr";
    internal const string Display = "mntr";
    internal const string Output = "prtr";
    internal const string ColourSpace = "spac";

    internal const string AToB0 = "A2B0";
    internal const string AToB1 = "A2B1";
    internal const string AToB2 = "A2B2";
    internal const string BToA0 = "B2A0";
    internal const string BToA1 = "B2A1";
    internal const string BToA2 = "B2A2";
    
    internal const string DToB0 = "D2B0";
    internal const string DToB1 = "D2B1";
    internal const string DToB2 = "D2B2";
    internal const string DToB3 = "D2B3";
    internal const string BToD0 = "B2D0";
    internal const string BToD1 = "B2D1";
    internal const string BToD2 = "B2D2";
    internal const string BToD3 = "B2D3";

    internal const string RedMatrixColumn = "rXYZ";
    internal const string GreenMatrixColumn = "gXYZ";
    internal const string BlueMatrixColumn = "bXYZ";
    internal const string RedTrc = "rTRC";
    internal const string GreenTrc = "gTRC";
    internal const string BlueTrc = "bTRC";
    internal const string GreyTrc = "kTRC";

    internal const string MultiFunctionTable1Byte = "mft1";
    internal const string MultiFunctionTable2Byte = "mft2";
    internal const string MultiFunctionAToB = "mAB ";
    internal const string MultiFunctionBToA = "mBA ";

    internal const string Curve = "curv";
    internal const string ParametricCurve = "para";

    internal const string MediaWhitePoint = "wtpt";

    internal const string Null = "\0\0\0\0";
}