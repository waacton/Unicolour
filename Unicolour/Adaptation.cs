namespace Wacton.Unicolour;

// http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
public static class Adaptation
{
    public static readonly double[,] Bradford =
    {
        { +0.8951000, +0.2664000, -0.1614000 },
        { -0.7502000, +1.7135000, +0.0367000 },
        { +0.0389000, -0.0685000, +1.0296000 }
    };
    
    public static readonly double[,] VonKries = // aka Hunt-Pointer-Estevez
    {
        { +0.4002400, +0.7076000, -0.0808100 },
        { -0.2263000, +1.1653200, +0.0457000 },
        { +0.0000000, +0.0000000, +0.9182200 }
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