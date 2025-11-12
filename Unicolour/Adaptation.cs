namespace Wacton.Unicolour;

// http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
public static class Adaptation
{
    // HPE for LMS conversion is the matrix originally used in the von Kries transformation for chromatic adaptation
    // doesn't matter that it's D65-relative; during adaptation both source & destination use D65 as a common relative space
    public static readonly double[,] VonKries = Lms.HuntPointerEstevez.Data;
    
    // Bradford is a "spectrally sharpened" transformation matrix, adjusting L and M cone responses to make them more distinct
    // designed to produce better adaptation results, not designed for actual LMS conversion
    public static readonly double[,] Bradford =
    {
        { +0.8951000, +0.2664000, -0.1614000 },
        { -0.7502000, +1.7135000, +0.0367000 },
        { +0.0389000, -0.0685000, +1.0296000 }
    };
    
    public static readonly double[,] XyzScaling =
    {
        { 1.0000000, 0.0000000, 0.0000000 },
        { 0.0000000, 1.0000000, 0.0000000 },
        { 0.0000000, 0.0000000, 1.0000000 }
    };

    internal static Xyz WhitePoint(Xyz sourceXyz, WhitePoint sourceWhitePoint, WhitePoint destinationWhitePoint, Matrix adaptationMatrix)
    {
        if (sourceWhitePoint == destinationWhitePoint)
        {
            return sourceXyz;
        }
        
        var sourceXyzMatrix = Matrix.From(sourceXyz);
        var (x, y, z) = WhitePoint(sourceXyzMatrix, sourceWhitePoint, destinationWhitePoint, adaptationMatrix).ToTriplet();
        return new Xyz(x, y, z);
    }
    
    internal static Matrix WhitePoint(Matrix sourceXyz, WhitePoint sourceWhitePoint, WhitePoint destinationWhitePoint, Matrix adaptationMatrix)
    {
        return sourceWhitePoint == destinationWhitePoint 
            ? sourceXyz 
            : M(sourceWhitePoint, destinationWhitePoint, adaptationMatrix).Multiply(sourceXyz);
    }
    
    internal static Matrix M(WhitePoint sourceWhitePoint, WhitePoint destinationWhitePoint, Matrix adaptationMatrix)
    {
        var sourceWhite = sourceWhitePoint.AsXyzMatrix();
        var destinationWhite = destinationWhitePoint.AsXyzMatrix();

        var sourceLms = adaptationMatrix.Multiply(sourceWhite).ToTriplet();
        var destinationLms = adaptationMatrix.Multiply(destinationWhite).ToTriplet();

        var lmsRatios = new Matrix(new[,]
        {
            { destinationLms.First / sourceLms.First, 0, 0 },
            { 0, destinationLms.Second / sourceLms.Second, 0 },
            { 0, 0, destinationLms.Third / sourceLms.Third }
        });

        return adaptationMatrix.Inverse().Multiply(lmsRatios).Multiply(adaptationMatrix);
    }
}