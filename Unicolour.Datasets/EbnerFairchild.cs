namespace Wacton.Unicolour.Datasets;

public static class EbnerFairchild
{
    private static readonly Xyz WhiteXyz = new(0.9501, 1.0000, 1.0881);
    private static readonly XyzConfiguration XyzConfig = new(new WhitePoint(WhiteXyz.X * 100, WhiteXyz.Y * 100, WhiteXyz.Z * 100));
    private static readonly Configuration Config = new(RgbConfiguration.StandardRgb, XyzConfig);
    public static readonly Unicolour White = new(ColourSpace.Xyz, Config, WhiteXyz.Triplet.Tuple);
    
    public static readonly Unicolour Hue0Ref = new(ColourSpace.Xyz, Config, 0.4092, 0.2812, 0.3060);
    public static readonly List<Unicolour> AllHue0 = new()
    {
        Hue0Ref,
        new(ColourSpace.Xyz, Config, 0.024951, 0.019086, 0.020329),	
        new(ColourSpace.Xyz, Config, 0.033296, 0.019086, 0.021063),	
        new(ColourSpace.Xyz, Config, 0.057386, 0.029891, 0.032447),	
        new(ColourSpace.Xyz, Config, 0.073706, 0.062359, 0.070514),	
        new(ColourSpace.Xyz, Config, 0.090418, 0.062359, 0.068901),	
        new(ColourSpace.Xyz, Config, 0.109443, 0.062359, 0.067881),	
        new(ColourSpace.Xyz, Config, 0.120597, 0.062359, 0.066805),	
        new(ColourSpace.Xyz, Config, 0.216475, 0.112510, 0.123602),	
        new(ColourSpace.Xyz, Config, 0.204172, 0.184187, 0.201965),	
        new(ColourSpace.Xyz, Config, 0.236417, 0.184187, 0.201739),	
        new(ColourSpace.Xyz, Config, 0.271865, 0.184187, 0.195653),	
        new(ColourSpace.Xyz, Config, 0.310729, 0.184187, 0.197150),	
        new(ColourSpace.Xyz, Config, 0.355801, 0.184187, 0.181154),	
        new(ColourSpace.Xyz, Config, 0.522309, 0.281233, 0.306758),	
        new(ColourSpace.Xyz, Config, 0.436118, 0.407494, 0.442210),	
        new(ColourSpace.Xyz, Config, 0.488989, 0.407494, 0.448546),	
        new(ColourSpace.Xyz, Config, 0.546008, 0.407494, 0.442074),	
        new(ColourSpace.Xyz, Config, 0.594653, 0.407494, 0.450086),	
        new(ColourSpace.Xyz, Config, 0.691717, 0.566813, 0.619143),	
        new(ColourSpace.Xyz, Config, 0.803849, 0.763034, 0.836166)
    };
    
    public static readonly Unicolour Hue24Ref = new(ColourSpace.Xyz, Config, 0.3953, 0.2812, 0.1845);
    public static readonly List<Unicolour> AllHue24 = new()
    {
        Hue24Ref,
        new(ColourSpace.Xyz, Config, 0.024364, 0.019086, 0.014688),	
        new(ColourSpace.Xyz, Config, 0.031680, 0.019086, 0.009391),	
        new(ColourSpace.Xyz, Config, 0.055818, 0.029891, 0.011895),	
        new(ColourSpace.Xyz, Config, 0.072650, 0.062359, 0.054620),	
        new(ColourSpace.Xyz, Config, 0.087473, 0.062359, 0.041462),	
        new(ColourSpace.Xyz, Config, 0.103312, 0.062359, 0.028546),	
        new(ColourSpace.Xyz, Config, 0.116178, 0.062359, 0.021270),	
        new(ColourSpace.Xyz, Config, 0.208807, 0.112510, 0.037728),	
        new(ColourSpace.Xyz, Config, 0.201146, 0.184187, 0.167921),	
        new(ColourSpace.Xyz, Config, 0.230654, 0.184187, 0.142768),	
        new(ColourSpace.Xyz, Config, 0.263119, 0.184187, 0.121097),	
        new(ColourSpace.Xyz, Config, 0.294922, 0.184187, 0.091904),	
        new(ColourSpace.Xyz, Config, 0.337924, 0.184187, 0.056569),	
        new(ColourSpace.Xyz, Config, 0.419121, 0.229298, 0.063588),	
        new(ColourSpace.Xyz, Config, 0.527130, 0.340472, 0.186420),	
        new(ColourSpace.Xyz, Config, 0.431587, 0.407494, 0.390086),	
        new(ColourSpace.Xyz, Config, 0.478373, 0.407494, 0.337280),	
        new(ColourSpace.Xyz, Config, 0.528667, 0.407494, 0.290411),	
        new(ColourSpace.Xyz, Config, 0.665260, 0.566813, 0.457942),	
        new(ColourSpace.Xyz, Config, 0.790894, 0.763034, 0.742766)
    };
    
    public static readonly Unicolour Hue48Ref = new(ColourSpace.Xyz, Config, 0.3577, 0.2812, 0.1125);
    public static readonly List<Unicolour> AllHue48 = new()
    {
        Hue48Ref,
        new(ColourSpace.Xyz, Config, 0.036781, 0.029891, 0.014811),	
        new(ColourSpace.Xyz, Config, 0.043912, 0.029891, 0.004572),	
        new(ColourSpace.Xyz, Config, 0.072096, 0.062359, 0.036436),	
        new(ColourSpace.Xyz, Config, 0.086369, 0.062359, 0.016378),	
        new(ColourSpace.Xyz, Config, 0.093726, 0.062359, 0.007023),	
        new(ColourSpace.Xyz, Config, 0.171277, 0.112510, 0.012299),	
        new(ColourSpace.Xyz, Config, 0.159324, 0.145417, 0.098473),	
        new(ColourSpace.Xyz, Config, 0.184248, 0.145417, 0.057809),	
        new(ColourSpace.Xyz, Config, 0.208121, 0.145417, 0.028115),	
        new(ColourSpace.Xyz, Config, 0.272709, 0.184187, 0.017329),	
        new(ColourSpace.Xyz, Config, 0.300809, 0.281233, 0.212298),	
        new(ColourSpace.Xyz, Config, 0.337425, 0.281233, 0.140395),	
        new(ColourSpace.Xyz, Config, 0.371325, 0.281233, 0.082291),	
        new(ColourSpace.Xyz, Config, 0.389601, 0.281233, 0.035885),	
        new(ColourSpace.Xyz, Config, 0.409676, 0.281233, 0.022750),	
        new(ColourSpace.Xyz, Config, 0.529760, 0.407494, 0.117200),	
        new(ColourSpace.Xyz, Config, 0.501955, 0.482781, 0.380164),	
        new(ColourSpace.Xyz, Config, 0.545519, 0.482781, 0.261853),	
        new(ColourSpace.Xyz, Config, 0.633032, 0.566813, 0.326901),	
        new(ColourSpace.Xyz, Config, 0.770216, 0.763034, 0.651076)
    };
    
    public static readonly Unicolour Hue72Ref = new(ColourSpace.Xyz, Config, 0.5151, 0.4828, 0.1778);
    public static readonly List<Unicolour> AllHue72 = new()
    {
        Hue72Ref,
        new(ColourSpace.Xyz, Config, 0.033197, 0.029891, 0.011480),	
        new(ColourSpace.Xyz, Config, 0.035138, 0.029891, 0.006227),	
        new(ColourSpace.Xyz, Config, 0.066846, 0.062359, 0.030931),	
        new(ColourSpace.Xyz, Config, 0.074603, 0.062359, 0.011325),	
        new(ColourSpace.Xyz, Config, 0.134731, 0.112510, 0.018616),	
        new(ColourSpace.Xyz, Config, 0.148686, 0.145417, 0.087303),	
        new(ColourSpace.Xyz, Config, 0.163904, 0.145417, 0.043255),	
        new(ColourSpace.Xyz, Config, 0.217903, 0.184187, 0.029308),	
        new(ColourSpace.Xyz, Config, 0.285178, 0.281233, 0.192981),	
        new(ColourSpace.Xyz, Config, 0.304615, 0.281233, 0.112259),	
        new(ColourSpace.Xyz, Config, 0.323158, 0.281233, 0.057361),	
        new(ColourSpace.Xyz, Config, 0.331870, 0.281233, 0.042045),	
        new(ColourSpace.Xyz, Config, 0.460119, 0.407494, 0.056175),	
        new(ColourSpace.Xyz, Config, 0.483113, 0.482781, 0.358235),	
        new(ColourSpace.Xyz, Config, 0.504509, 0.482781, 0.228894),	
        new(ColourSpace.Xyz, Config, 0.525746, 0.482781, 0.134945),	
        new(ColourSpace.Xyz, Config, 0.546262, 0.482781, 0.070769),	
        new(ColourSpace.Xyz, Config, 0.616913, 0.566813, 0.164547),	
        new(ColourSpace.Xyz, Config, 0.678322, 0.660070, 0.333101),	
        new(ColourSpace.Xyz, Config, 0.754525, 0.763034, 0.541791)
    };
    
    public static readonly Unicolour Hue96Ref = new(ColourSpace.Xyz, Config, 0.5184, 0.5668, 0.2112);
    public static readonly List<Unicolour> AllHue96 = new()
    {
        Hue96Ref,
        new(ColourSpace.Xyz, Config, 0.028909, 0.029891, 0.010142),	
        new(ColourSpace.Xyz, Config, 0.059861, 0.062359, 0.028395),	
        new(ColourSpace.Xyz, Config, 0.061870, 0.062359, 0.010384),	
        new(ColourSpace.Xyz, Config, 0.108221, 0.112510, 0.017582),	
        new(ColourSpace.Xyz, Config, 0.138453, 0.145417, 0.084043),	
        new(ColourSpace.Xyz, Config, 0.140725, 0.145417, 0.037708),	
        new(ColourSpace.Xyz, Config, 0.174718, 0.184187, 0.027642),	
        new(ColourSpace.Xyz, Config, 0.266956, 0.281233, 0.186195),	
        new(ColourSpace.Xyz, Config, 0.267106, 0.281233, 0.102615),	
        new(ColourSpace.Xyz, Config, 0.265222, 0.281233, 0.048764),	
        new(ColourSpace.Xyz, Config, 0.378172, 0.407494, 0.060499),	
        new(ColourSpace.Xyz, Config, 0.452991, 0.482781, 0.349471),	
        new(ColourSpace.Xyz, Config, 0.448363, 0.482781, 0.217887),	
        new(ColourSpace.Xyz, Config, 0.444752, 0.482781, 0.124260),	
        new(ColourSpace.Xyz, Config, 0.512315, 0.566813, 0.085942),	
        new(ColourSpace.Xyz, Config, 0.709748, 0.763034, 0.589282),	
        new(ColourSpace.Xyz, Config, 0.701174, 0.763034, 0.398412),	
        new(ColourSpace.Xyz, Config, 0.698154, 0.763034, 0.253302),	
        new(ColourSpace.Xyz, Config, 0.705383, 0.763034, 0.148054),	
        new(ColourSpace.Xyz, Config, 0.704427, 0.763034, 0.112490)
    };
    
    public static readonly Unicolour Hue120Ref = new(ColourSpace.Xyz, Config, 0.3765, 0.4828, 0.1994);
    public static readonly List<Unicolour> AllHue120 = new()
    {
        Hue120Ref,
        new(ColourSpace.Xyz, Config, 0.023295, 0.029891, 0.012151),	
        new(ColourSpace.Xyz, Config, 0.050776, 0.062359, 0.032350),	
        new(ColourSpace.Xyz, Config, 0.045813, 0.062359, 0.010648),	
        new(ColourSpace.Xyz, Config, 0.123217, 0.145417, 0.091954),	
        new(ColourSpace.Xyz, Config, 0.111245, 0.145417, 0.045894),	
        new(ColourSpace.Xyz, Config, 0.131275, 0.184187, 0.025870),	
        new(ColourSpace.Xyz, Config, 0.242342, 0.281233, 0.201576),	
        new(ColourSpace.Xyz, Config, 0.222813, 0.281233, 0.120243),	
        new(ColourSpace.Xyz, Config, 0.204891, 0.281233, 0.064150),	
        new(ColourSpace.Xyz, Config, 0.200979, 0.281233, 0.040060),	
        new(ColourSpace.Xyz, Config, 0.420356, 0.482781, 0.376153),	
        new(ColourSpace.Xyz, Config, 0.392198, 0.482781, 0.248400),	
        new(ColourSpace.Xyz, Config, 0.365407, 0.482781, 0.153761),	
        new(ColourSpace.Xyz, Config, 0.344596, 0.482781, 0.084394),	
        new(ColourSpace.Xyz, Config, 0.400128, 0.566813, 0.081820),	
        new(ColourSpace.Xyz, Config, 0.526749, 0.763034, 0.113140),	
        new(ColourSpace.Xyz, Config, 0.672532, 0.763034, 0.625084),	
        new(ColourSpace.Xyz, Config, 0.628152, 0.763034, 0.449196),	
        new(ColourSpace.Xyz, Config, 0.586004, 0.763034, 0.309764),	
        new(ColourSpace.Xyz, Config, 0.555877, 0.763034, 0.194968)
    };
    
    public static readonly Unicolour Hue144Ref = new(ColourSpace.Xyz, Config, 0.2873, 0.4828, 0.2109);
    public static readonly List<Unicolour> AllHue144 = new()
    {
        Hue144Ref,
        new(ColourSpace.Xyz, Config, 0.020570, 0.029891, 0.016832),	
        new(ColourSpace.Xyz, Config, 0.013952, 0.029891, 0.008032),	
        new(ColourSpace.Xyz, Config, 0.086184, 0.112510, 0.085273),	
        new(ColourSpace.Xyz, Config, 0.069267, 0.112510, 0.053448),	
        new(ColourSpace.Xyz, Config, 0.055532, 0.112510, 0.028928),	
        new(ColourSpace.Xyz, Config, 0.082777, 0.184187, 0.041966),	
        new(ColourSpace.Xyz, Config, 0.228649, 0.281233, 0.234216),	
        new(ColourSpace.Xyz, Config, 0.194735, 0.281233, 0.171960),	
        new(ColourSpace.Xyz, Config, 0.163555, 0.281233, 0.124344),	
        new(ColourSpace.Xyz, Config, 0.138615, 0.281233, 0.079293),	
        new(ColourSpace.Xyz, Config, 0.126312, 0.281233, 0.063504),	
        new(ColourSpace.Xyz, Config, 0.281591, 0.566813, 0.162474),	
        new(ColourSpace.Xyz, Config, 0.252804, 0.566813, 0.129911),	
        new(ColourSpace.Xyz, Config, 0.476451, 0.566813, 0.499746),	
        new(ColourSpace.Xyz, Config, 0.419222, 0.566813, 0.398854),	
        new(ColourSpace.Xyz, Config, 0.372596, 0.566813, 0.293883),	
        new(ColourSpace.Xyz, Config, 0.321493, 0.566813, 0.231624),	
        new(ColourSpace.Xyz, Config, 0.295765, 0.660070, 0.147727),	
        new(ColourSpace.Xyz, Config, 0.476799, 0.763034, 0.351210),	
        new(ColourSpace.Xyz, Config, 0.701190, 0.876183, 0.690486)
    };
    
    public static readonly Unicolour Hue168Ref = new(ColourSpace.Xyz, Config, 0.3076, 0.4828, 0.4277);
    public static readonly List<Unicolour> AllHue168 = new()
    {
        Hue168Ref,
        new(ColourSpace.Xyz, Config, 0.021080, 0.029891, 0.027904),	
        new(ColourSpace.Xyz, Config, 0.016702, 0.029891, 0.023311),	
        new(ColourSpace.Xyz, Config, 0.034115, 0.062359, 0.051996),	
        new(ColourSpace.Xyz, Config, 0.088419, 0.112510, 0.113399),	
        new(ColourSpace.Xyz, Config, 0.072572, 0.112510, 0.098606),	
        new(ColourSpace.Xyz, Config, 0.061947, 0.112510, 0.093344),	
        new(ColourSpace.Xyz, Config, 0.100632, 0.184187, 0.151057),	
        new(ColourSpace.Xyz, Config, 0.232833, 0.281233, 0.285625),	
        new(ColourSpace.Xyz, Config, 0.201319, 0.281233, 0.269643),	
        new(ColourSpace.Xyz, Config, 0.173352, 0.281233, 0.244685),	
        new(ColourSpace.Xyz, Config, 0.152558, 0.281233, 0.232349),	
        new(ColourSpace.Xyz, Config, 0.221968, 0.407494, 0.339779),	
        new(ColourSpace.Xyz, Config, 0.483497, 0.566813, 0.579764),	
        new(ColourSpace.Xyz, Config, 0.432329, 0.566813, 0.544056),	
        new(ColourSpace.Xyz, Config, 0.385515, 0.566813, 0.501932),	
        new(ColourSpace.Xyz, Config, 0.341577, 0.566813, 0.470353),	
        new(ColourSpace.Xyz, Config, 0.308994, 0.566813, 0.445270),	
        new(ColourSpace.Xyz, Config, 0.359901, 0.660070, 0.507477),	
        new(ColourSpace.Xyz, Config, 0.490016, 0.763034, 0.638159),	
        new(ColourSpace.Xyz, Config, 0.622486, 0.829670, 0.763862)
    };
    
    public static readonly Unicolour Hue192Ref = new(ColourSpace.Xyz, Config, 0.3488, 0.4828, 0.6019);
    public static readonly List<Unicolour> AllHue192 = new()
    {
        Hue192Ref,
        new(ColourSpace.Xyz, Config, 0.023453, 0.029891, 0.037174),	
        new(ColourSpace.Xyz, Config, 0.019260, 0.029891, 0.044245),	
        new(ColourSpace.Xyz, Config, 0.040173, 0.062359, 0.096191),	
        new(ColourSpace.Xyz, Config, 0.094684, 0.112510, 0.133777),	
        new(ColourSpace.Xyz, Config, 0.083988, 0.112510, 0.152001),	
        new(ColourSpace.Xyz, Config, 0.073638, 0.112510, 0.165584),	
        new(ColourSpace.Xyz, Config, 0.116981, 0.184187, 0.255249),	
        new(ColourSpace.Xyz, Config, 0.243887, 0.281233, 0.319199),	
        new(ColourSpace.Xyz, Config, 0.222844, 0.281233, 0.345849),	
        new(ColourSpace.Xyz, Config, 0.202715, 0.281233, 0.368170),	
        new(ColourSpace.Xyz, Config, 0.182763, 0.281233, 0.373909),	
        new(ColourSpace.Xyz, Config, 0.254976, 0.407494, 0.528133),	
        new(ColourSpace.Xyz, Config, 0.501257, 0.566813, 0.640076),	
        new(ColourSpace.Xyz, Config, 0.465487, 0.566813, 0.660339),	
        new(ColourSpace.Xyz, Config, 0.431626, 0.566813, 0.683981),	
        new(ColourSpace.Xyz, Config, 0.398701, 0.566813, 0.692512),	
        new(ColourSpace.Xyz, Config, 0.352616, 0.566813, 0.704797),	
        new(ColourSpace.Xyz, Config, 0.460800, 0.741641, 0.881726),	
        new(ColourSpace.Xyz, Config, 0.582443, 0.807044, 0.939569),	
        new(ColourSpace.Xyz, Config, 0.714332, 0.876183, 0.999575)
    };
    
    public static readonly Unicolour Hue216Ref = new(ColourSpace.Xyz, Config, 0.3787, 0.4828, 0.7231);
    public static readonly List<Unicolour> AllHue216 = new()
    {
        Hue216Ref,
        new(ColourSpace.Xyz, Config, 0.024666, 0.029891, 0.044851),	
        new(ColourSpace.Xyz, Config, 0.022448, 0.029891, 0.059111),	
        new(ColourSpace.Xyz, Config, 0.046165, 0.062359, 0.120332),	
        new(ColourSpace.Xyz, Config, 0.097400, 0.112510, 0.149949),	
        new(ColourSpace.Xyz, Config, 0.088882, 0.112510, 0.183141),	
        new(ColourSpace.Xyz, Config, 0.081128, 0.112510, 0.214127),	
        new(ColourSpace.Xyz, Config, 0.132261, 0.184187, 0.345513),	
        new(ColourSpace.Xyz, Config, 0.248999, 0.281233, 0.353653),	
        new(ColourSpace.Xyz, Config, 0.231907, 0.281233, 0.407240),	
        new(ColourSpace.Xyz, Config, 0.216271, 0.281233, 0.469210),	
        new(ColourSpace.Xyz, Config, 0.198433, 0.281233, 0.504344),	
        new(ColourSpace.Xyz, Config, 0.282041, 0.407494, 0.690229),	
        new(ColourSpace.Xyz, Config, 0.508535, 0.566813, 0.688802),	
        new(ColourSpace.Xyz, Config, 0.478889, 0.566813, 0.762798),	
        new(ColourSpace.Xyz, Config, 0.446584, 0.566813, 0.820460),	
        new(ColourSpace.Xyz, Config, 0.416195, 0.566813, 0.881993),	
        new(ColourSpace.Xyz, Config, 0.390704, 0.566813, 0.944482),	
        new(ColourSpace.Xyz, Config, 0.498666, 0.660070, 1.002007),	
        new(ColourSpace.Xyz, Config, 0.629735, 0.763034, 1.030157),	
        new(ColourSpace.Xyz, Config, 0.780945, 0.876183, 1.053757)
    };

    public static readonly Unicolour Hue240Ref = new(ColourSpace.Xyz, Config, 0.3421, 0.4075, 0.7197);
    public static readonly List<Unicolour> AllHue240 = new()
    {
        Hue240Ref,
        new(ColourSpace.Xyz, Config, 0.026197, 0.029891, 0.049052),	
        new(ColourSpace.Xyz, Config, 0.024789, 0.029891, 0.069067),	
        new(ColourSpace.Xyz, Config, 0.050943, 0.062359, 0.145770),	
        new(ColourSpace.Xyz, Config, 0.101162, 0.112510, 0.159858),	
        new(ColourSpace.Xyz, Config, 0.096673, 0.112510, 0.206324),	
        new(ColourSpace.Xyz, Config, 0.092055, 0.112510, 0.260341),	
        new(ColourSpace.Xyz, Config, 0.148942, 0.184187, 0.428274),	
        new(ColourSpace.Xyz, Config, 0.255352, 0.281233, 0.371142),	
        new(ColourSpace.Xyz, Config, 0.244632, 0.281233, 0.446614),	
        new(ColourSpace.Xyz, Config, 0.234074, 0.281233, 0.531310),	
        new(ColourSpace.Xyz, Config, 0.222932, 0.281233, 0.623583),	
        new(ColourSpace.Xyz, Config, 0.319264, 0.407494, 0.921437),	
        new(ColourSpace.Xyz, Config, 0.441905, 0.482781, 0.618135),	
        new(ColourSpace.Xyz, Config, 0.423672, 0.482781, 0.717357),	
        new(ColourSpace.Xyz, Config, 0.407141, 0.482781, 0.829579),	
        new(ColourSpace.Xyz, Config, 0.392461, 0.482781, 0.956828),	
        new(ColourSpace.Xyz, Config, 0.472962, 0.566813, 1.003071),	
        new(ColourSpace.Xyz, Config, 0.572536, 0.660070, 1.015921),	
        new(ColourSpace.Xyz, Config, 0.679355, 0.763034, 1.034360)
    };
    
    public static readonly Unicolour Hue264Ref = new(ColourSpace.Xyz, Config, 0.3149, 0.3405, 0.6623);
    public static readonly List<Unicolour> AllHue264 = new()
    {
        Hue264Ref,
        new(ColourSpace.Xyz, Config, 0.027982, 0.029891, 0.086685),	
        new(ColourSpace.Xyz, Config, 0.041208, 0.044155, 0.071309),	
        new(ColourSpace.Xyz, Config, 0.041612, 0.044155, 0.101433),	
        new(ColourSpace.Xyz, Config, 0.060966, 0.062359, 0.187302),	
        new(ColourSpace.Xyz, Config, 0.105606, 0.112510, 0.164317),	
        new(ColourSpace.Xyz, Config, 0.104042, 0.112510, 0.214695),	
        new(ColourSpace.Xyz, Config, 0.104747, 0.112510, 0.275383),	
        new(ColourSpace.Xyz, Config, 0.105836, 0.112510, 0.338709),	
        new(ColourSpace.Xyz, Config, 0.181547, 0.184187, 0.553577),	
        new(ColourSpace.Xyz, Config, 0.262553, 0.281233, 0.380059),	
        new(ColourSpace.Xyz, Config, 0.258303, 0.281233, 0.465439),	
        new(ColourSpace.Xyz, Config, 0.259521, 0.281233, 0.566217),	
        new(ColourSpace.Xyz, Config, 0.267324, 0.281233, 0.680523),	
        new(ColourSpace.Xyz, Config, 0.280301, 0.281233, 0.844180),	
        new(ColourSpace.Xyz, Config, 0.338060, 0.340472, 0.968792),	
        new(ColourSpace.Xyz, Config, 0.424315, 0.451644, 0.999617),	
        new(ColourSpace.Xyz, Config, 0.528874, 0.566813, 0.731468),	
        new(ColourSpace.Xyz, Config, 0.523387, 0.566813, 0.862998),	
        new(ColourSpace.Xyz, Config, 0.524765, 0.566813, 1.013278),	
        new(ColourSpace.Xyz, Config, 0.708398, 0.763034, 1.029002)
    };
    
    public static readonly Unicolour Hue288Ref = new(ColourSpace.Xyz, Config, 0.2051, 0.1842, 0.5713);
    public static readonly List<Unicolour> AllHue288 = new()
    {
        Hue288Ref,
        new(ColourSpace.Xyz, Config, 0.030398, 0.029891, 0.061233),	
        new(ColourSpace.Xyz, Config, 0.033574, 0.029891, 0.101548),	
        new(ColourSpace.Xyz, Config, 0.037776, 0.029891, 0.140607),	
        new(ColourSpace.Xyz, Config, 0.081239, 0.062359, 0.289466),	
        new(ColourSpace.Xyz, Config, 0.083832, 0.084984, 0.147224),	
        new(ColourSpace.Xyz, Config, 0.088700, 0.084984, 0.218435),	
        new(ColourSpace.Xyz, Config, 0.097338, 0.084984, 0.303378),	
        new(ColourSpace.Xyz, Config, 0.111523, 0.084984, 0.393449),	
        new(ColourSpace.Xyz, Config, 0.150163, 0.112510, 0.504059),	
        new(ColourSpace.Xyz, Config, 0.179346, 0.184187, 0.289351),	
        new(ColourSpace.Xyz, Config, 0.184058, 0.184187, 0.401114),	
        new(ColourSpace.Xyz, Config, 0.198420, 0.184187, 0.527573),	
        new(ColourSpace.Xyz, Config, 0.218326, 0.184187, 0.664797),	
        new(ColourSpace.Xyz, Config, 0.244740, 0.184187, 0.834294),	
        new(ColourSpace.Xyz, Config, 0.323931, 0.281233, 0.951349),	
        new(ColourSpace.Xyz, Config, 0.325501, 0.340472, 0.502969),	
        new(ColourSpace.Xyz, Config, 0.331968, 0.340472, 0.662406),	
        new(ColourSpace.Xyz, Config, 0.353672, 0.340472, 0.839130),	
        new(ColourSpace.Xyz, Config, 0.368082, 0.340472, 0.969789),	
        new(ColourSpace.Xyz, Config, 0.456537, 0.451644, 1.003590),	
        new(ColourSpace.Xyz, Config, 0.538261, 0.566813, 0.800104),	
        new(ColourSpace.Xyz, Config, 0.553271, 0.566813, 1.012963),	
        new(ColourSpace.Xyz, Config, 0.723061, 0.763034, 1.051867)
    };
    
    public static readonly Unicolour Hue312Ref = new(ColourSpace.Xyz, Config, 0.2765, 0.1842, 0.6201);
    public static readonly List<Unicolour> AllHue312 = new()
    {
        Hue312Ref,
        new(ColourSpace.Xyz, Config, 0.037048, 0.029891, 0.059648),	
        new(ColourSpace.Xyz, Config, 0.047540, 0.029891, 0.097663),	
        new(ColourSpace.Xyz, Config, 0.060374, 0.029891, 0.146405),	
        new(ColourSpace.Xyz, Config, 0.073117, 0.029891, 0.197734),	
        new(ColourSpace.Xyz, Config, 0.151352, 0.062359, 0.427145),	
        new(ColourSpace.Xyz, Config, 0.190702, 0.084984, 0.520012),	
        new(ColourSpace.Xyz, Config, 0.096873, 0.084984, 0.146268),	
        new(ColourSpace.Xyz, Config, 0.114333, 0.084984, 0.220239),	
        new(ColourSpace.Xyz, Config, 0.136517, 0.084984, 0.303894),	
        new(ColourSpace.Xyz, Config, 0.161808, 0.084984, 0.403975),	
        new(ColourSpace.Xyz, Config, 0.272723, 0.112510, 0.781810),	
        new(ColourSpace.Xyz, Config, 0.340040, 0.184187, 0.857156),	
        new(ColourSpace.Xyz, Config, 0.199887, 0.184187, 0.292578),	
        new(ColourSpace.Xyz, Config, 0.226043, 0.184187, 0.412528),	
        new(ColourSpace.Xyz, Config, 0.260220, 0.184187, 0.539744),	
        new(ColourSpace.Xyz, Config, 0.296726, 0.184187, 0.693691),	
        new(ColourSpace.Xyz, Config, 0.427495, 0.281233, 0.953079),	
        new(ColourSpace.Xyz, Config, 0.358782, 0.340472, 0.511040),	
        new(ColourSpace.Xyz, Config, 0.397286, 0.340472, 0.681086),	
        new(ColourSpace.Xyz, Config, 0.445219, 0.340472, 0.862127),	
        new(ColourSpace.Xyz, Config, 0.520133, 0.407494, 0.999868),	
        new(ColourSpace.Xyz, Config, 0.584479, 0.566813, 0.818948),	
        new(ColourSpace.Xyz, Config, 0.633425, 0.566813, 1.040201),	
        new(ColourSpace.Xyz, Config, 0.774496, 0.763034, 1.068820)
    };
    
    public static readonly Unicolour Hue336Ref = new(ColourSpace.Xyz, Config, 0.4562, 0.2812, 0.5520);
    public static readonly List<Unicolour> AllHue336 = new()
    {
        Hue336Ref,
        new(ColourSpace.Xyz, Config, 0.059792, 0.029891, 0.067983),	
        new(ColourSpace.Xyz, Config, 0.056611, 0.044155, 0.064892),	
        new(ColourSpace.Xyz, Config, 0.074487, 0.044155, 0.084184),	
        new(ColourSpace.Xyz, Config, 0.125371, 0.062359, 0.148021),	
        new(ColourSpace.Xyz, Config, 0.168870, 0.145417, 0.198849),	
        new(ColourSpace.Xyz, Config, 0.205757, 0.145417, 0.232471),	
        new(ColourSpace.Xyz, Config, 0.246615, 0.145417, 0.276573),	
        new(ColourSpace.Xyz, Config, 0.290646, 0.145417, 0.339618),	
        new(ColourSpace.Xyz, Config, 0.371009, 0.184187, 0.443260),	
        new(ColourSpace.Xyz, Config, 0.553607, 0.281233, 0.752024),	
        new(ColourSpace.Xyz, Config, 0.314203, 0.281233, 0.368351),	
        new(ColourSpace.Xyz, Config, 0.368426, 0.281233, 0.424991),	
        new(ColourSpace.Xyz, Config, 0.427739, 0.281233, 0.492386),	
        new(ColourSpace.Xyz, Config, 0.488039, 0.281233, 0.601262),	
        new(ColourSpace.Xyz, Config, 0.650875, 0.407494, 0.827335),	
        new(ColourSpace.Xyz, Config, 0.524811, 0.482781, 0.617910),	
        new(ColourSpace.Xyz, Config, 0.598114, 0.482781, 0.713579),	
        new(ColourSpace.Xyz, Config, 0.673205, 0.482781, 0.848059),	
        new(ColourSpace.Xyz, Config, 0.725461, 0.566813, 0.913143),	
        new(ColourSpace.Xyz, Config, 0.825715, 0.763034, 0.988142)
    };

    public static IEnumerable<Unicolour> All => new List<Unicolour>()
        .Concat(AllHue0)
        .Concat(AllHue24)
        .Concat(AllHue48)
        .Concat(AllHue72)
        .Concat(AllHue96)
        .Concat(AllHue120)
        .Concat(AllHue144)
        .Concat(AllHue168)
        .Concat(AllHue192)
        .Concat(AllHue216)
        .Concat(AllHue240)
        .Concat(AllHue264)
        .Concat(AllHue288)
        .Concat(AllHue312)
        .Concat(AllHue336);
}