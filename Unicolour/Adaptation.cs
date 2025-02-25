namespace Wacton.Unicolour;

internal static class Adaptation
{
    internal static readonly Matrix Bradford = new(new[,]
    {
        { +0.8951, +0.2664, -0.1614 },
        { -0.7502, +1.7135, +0.0367 },
        { +0.0389, -0.0685, +1.0296 }
    });

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

    // http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
    internal static Matrix WhitePoint(Matrix sourceXyz, WhitePoint sourceWhitePoint, WhitePoint destinationWhitePoint, Matrix adaptationMatrix)
    {
        return sourceWhitePoint == destinationWhitePoint 
            ? sourceXyz 
            : M(sourceWhitePoint, destinationWhitePoint, adaptationMatrix).Multiply(sourceXyz);
    }
    
    private static Matrix M(WhitePoint sourceWhitePoint, WhitePoint destinationWhitePoint, Matrix adaptationMatrix)
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