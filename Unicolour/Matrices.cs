namespace Wacton.Unicolour;

internal static class Matrices
{
    private static readonly Matrix Bradford = new(new[,]
    {
        {+0.8951, +0.2664, -0.1614},
        {-0.7502, +1.7135, +0.0367},
        {+0.0389, -0.0685, +1.0296}
    });
    
    public static readonly Matrix IctcpM1 = new(new[,]
    {
        { 0.3593, 0.6976, -0.0359 },
        { -0.1921, 1.1005, 0.0754 },
        { 0.0071, 0.0748, 0.8433 }
    });
    
    public static readonly Matrix IctcpM2 = new Matrix(new double[,]
    {
        { 2048, 2048, 0 },
        { 6610, -13613, 7003 },
        { 17933, -17390, -543 }
    }).Scalar(x => x / 4096.0);
    
    public static readonly Matrix JzazbzM1 = new(new[,]
    {
        {+0.41478972, +0.579999, +0.0146480},
        {-0.2015100, +1.120649, +0.0531008},
        {-0.0166008, +0.264800, +0.6684799}
    });
    
    public static readonly Matrix JzazbzM2 = new(new[,]
    {
        {+0.5, +0.5, 0},
        {+3.524000, -4.066708, +0.542708},
        {+0.199076, +1.096799, -1.295875}
    });

    public static readonly Matrix OklabM1 = new(new[,]
    {
        {+0.8189330101, +0.3618667424, -0.1288597137},
        {+0.0329845436, +0.9293118715, +0.0361456387},
        {+0.0482003018, +0.2643662691, +0.6338517070}
    });
    
    public static readonly Matrix OklabM2 = new(new[,]
    {
        {+0.2104542553, +0.7936177850, -0.0040720468},
        {+1.9779984951, -2.4285922050, +0.4505937099},
        {+0.0259040371, +0.7827717662, -0.8086757660}
    });

    // see http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
    public static Matrix RgbToXyzMatrix(RgbConfiguration rgbConfig, XyzConfiguration xyzConfig)
    {
        var cr = rgbConfig.ChromaticityR;
        var cg = rgbConfig.ChromaticityG;
        var cb = rgbConfig.ChromaticityB;

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
        
        var sourceWhite = ReferenceWhiteMatrix(rgbConfig.WhitePoint);
        
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

        var adaptedMatrix = AdaptForWhitePoint(matrix, rgbConfig.WhitePoint, xyzConfig.WhitePoint);
        return adaptedMatrix;
    }

    public static Matrix AdaptForWhitePoint(Matrix matrix, WhitePoint sourceWhitePoint, WhitePoint destinationWhitePoint)
    {
        if (sourceWhitePoint == destinationWhitePoint)
        {
            return matrix;
        }
        
        var adaptedBradford = AdaptedBradfordMatrix(sourceWhitePoint, destinationWhitePoint);
        return adaptedBradford.Multiply(matrix);
    }

    private static Matrix ReferenceWhiteMatrix(WhitePoint whitePoint)
    {
        var (x, y, z) = whitePoint;
        return Matrix.FromTriplet(new(x, y, z)).Scalar(value => value / 100.0);
    }

    // see http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
    private static Matrix AdaptedBradfordMatrix(WhitePoint sourceWhitePoint, WhitePoint destinationWhitePoint)
    {
        var sourceWhite = ReferenceWhiteMatrix(sourceWhitePoint);
        var destinationWhite = ReferenceWhiteMatrix(destinationWhitePoint);

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