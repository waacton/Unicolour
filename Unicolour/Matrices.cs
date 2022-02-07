namespace Wacton.Unicolour;

internal static class Matrices
{
    private static readonly Matrix Bradford = new(new[,]
    {
        {0.8951, 0.2664, -0.1614},
        {-0.7502, 1.7135, 0.0367},
        {0.0389, -0.0685, 1.0296}
    });
    
    // based on http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
    public static Matrix RgbToXyzMatrix(Configuration config)
    {
        var cr = config.ChromaticityR;
        var cg = config.ChromaticityG;
        var cb = config.ChromaticityB;
        var rgbIlluminant = config.RgbIlluminant;
        var xyzIlluminant = config.XyzIlluminant;
        var observer = config.Observer;
        
        double X(Chromaticity c) => c.X / c.Y;
        double Y(Chromaticity c) => 1;
        double Z(Chromaticity c) => (1 - c.X - c.Y) / c.Y;

        var (xr, yr, zr) = (X(cr), Y(cr), Z(cr));
        var (xg, yg, zg) = (X(cg), Y(cg), Z(cg));
        var (xb, yb, zb) = (X(cb), Y(cb), Z(cb));
    
        var fromPrimaries = new Matrix(new[,]
        {
            {xr, xg, xb},
            {yr, yg, yb},
            {zr, zg, zb}
        });
        
        var sourceWhite = ReferenceWhiteMatrix(rgbIlluminant, observer);
        
        var s = fromPrimaries.Inverse().Multiply(sourceWhite);
        var sr = s[0, 0];
        var sg = s[1, 0];
        var sb = s[2, 0];

        var matrix = new Matrix(new[,]
        {
            {sr * xr, sg * xg, sb * xb},
            {sr * yr, sg * yg, sb * yb},
            {sr * zr, sg * zg, sb * zb}
        });

        if (rgbIlluminant == xyzIlluminant)
        {
            return matrix;
        }
    
        var adaptedBradford = AdaptedBradfordMatrix(rgbIlluminant, xyzIlluminant, observer);
        return adaptedBradford.Multiply(matrix);
    }
    
    private static Matrix ReferenceWhiteMatrix(Illuminant illuminant, Observer observer)
    {
        var (x, y, z) = Illuminants.ReferenceWhite(illuminant, observer);
        return new Matrix(new[,]
        {
            {x / 100.0},
            {y / 100.0},
            {z / 100.0}
        });
    }

    // based on http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
    private static Matrix AdaptedBradfordMatrix(Illuminant source, Illuminant destination, Observer observer)
    {
        var sourceWhite = ReferenceWhiteMatrix(source, observer);
        var destinationWhite = ReferenceWhiteMatrix(destination, observer);

        var sourceLms = Bradford.Multiply(sourceWhite);
        var destinationLms = Bradford.Multiply(destinationWhite);
        
        var lmsRatios = new Matrix(new[,]
        {
            {destinationLms[0, 0] / sourceLms[0, 0], 0, 0},
            {0, destinationLms[1, 0] / sourceLms[1, 0], 0},
            {0, 0, destinationLms[2, 0] / sourceLms[2, 0]}
        });

        var inverseBradford = Bradford.Inverse();
        var adaptedBradfordMatrix = inverseBradford.Multiply(lmsRatios).Multiply(Bradford);
        return adaptedBradfordMatrix;
    }
}