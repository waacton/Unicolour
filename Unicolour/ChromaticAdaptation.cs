namespace Wacton.Unicolour;

// http://www.brucelindbloom.com/index.html?Eqn_ChromAdapt.html
public class ChromaticAdaptation
{
    // HPE for LMS conversion is the matrix originally used in the von Kries transformation for chromatic adaptation
    // doesn't matter that it's D65-relative; during adaptation both source & destination use D65 as a common relative space
    public static readonly ChromaticAdaptation VonKries = new(Lms.HuntPointerEstevez.Data, nameof(VonKries));
    
    // Bradford is a "spectrally sharpened" transformation matrix, adjusting L and M cone responses to make them more distinct
    // designed to produce better adaptation results, not designed for actual LMS conversion
    public static readonly ChromaticAdaptation Bradford = new(new[,]
    {
        { +0.8951000, +0.2664000, -0.1614000 },
        { -0.7502000, +1.7135000, +0.0367000 },
        { +0.0389000, -0.0685000, +1.0296000 }
    }, nameof(Bradford));
    
    public static readonly ChromaticAdaptation XyzScaling = new(new[,]
    {
        { 1.0000000, 0.0000000, 0.0000000 },
        { 0.0000000, 1.0000000, 0.0000000 },
        { 0.0000000, 0.0000000, 1.0000000 }
    }, nameof(XyzScaling));
    
    internal Matrix AdaptationMatrix { get; }
    public string Name { get; }
    
    internal ChromaticAdaptation(double[,] adaptation, string name = Utils.Unnamed)
    {
        AdaptationMatrix = GetAdaptationMatrix(adaptation);
        Name = name;
    }
    
    internal Xyz Transform(Xyz sourceXyz, WhitePoint destinationWhitePoint)
    {
        if (sourceXyz.WhitePoint == destinationWhitePoint)
        {
            return sourceXyz;
        }
        
        var sourceXyzMatrix = Matrix.From(sourceXyz);
        var transformMatrix = GetTransformMatrix(sourceXyz.WhitePoint, destinationWhitePoint);
        var destinationXyzMatrix = transformMatrix.Multiply(sourceXyzMatrix);
        return new Xyz(destinationXyzMatrix.ToTriplet(), destinationWhitePoint, sourceXyz.Limitation);
    }
    
    internal Matrix GetTransformMatrix(WhitePoint sourceWhitePoint, WhitePoint destinationWhitePoint)
    {
        var sourceWhite = Matrix.From(sourceWhitePoint.Triplet);
        var destinationWhite = Matrix.From(destinationWhitePoint.Triplet);

        var sourceLms = AdaptationMatrix.Multiply(sourceWhite).ToTriplet();
        var destinationLms = AdaptationMatrix.Multiply(destinationWhite).ToTriplet();

        var lmsRatios = new Matrix(new[,]
        {
            { destinationLms.First / sourceLms.First, 0, 0 },
            { 0, destinationLms.Second / sourceLms.Second, 0 },
            { 0, 0, destinationLms.Third / sourceLms.Third }
        });

        return AdaptationMatrix.Inverse().Multiply(lmsRatios).Multiply(AdaptationMatrix);
    }
    
    private static Matrix GetAdaptationMatrix(double[,] adaptation)
    {
        const int rows = 3;
        const int cols = 3;
        
        var maxRow = adaptation.GetLength(0);
        var maxCol = adaptation.GetLength(1);
        if (maxRow == rows && maxCol == cols)
        {
            return new Matrix(adaptation);
        }

        var data = new double[rows, cols];
        for (var row = 0; row < rows; row++)
        {
            var hasRow = maxRow >= row + 1;
            for (var col = 0; col < cols; col++)
            {
                var hasCol = maxCol >= col + 1;
                data[row, col] = hasRow && hasCol ? adaptation[row, col] : double.NaN;
            }
        }
        
        return new Matrix(data);
    }

    public override string ToString() => Name;
}