namespace Wacton.Unicolour.Datasets;

public static class EbnerFairchild
{
    private static readonly Xyz WhiteXyz = new(0.9501, 1.0000, 1.0881);
    private static readonly XyzConfiguration XyzConfig = new(new WhitePoint(WhiteXyz.X * 100, WhiteXyz.Y * 100, WhiteXyz.Z * 100));
    private static readonly Configuration Config = new(RgbConfiguration.StandardRgb, XyzConfig);
    public static readonly Unicolour White = Unicolour.FromXyz(Config, WhiteXyz.Triplet.Tuple);
    
    public static readonly Unicolour Hue0Ref = Unicolour.FromXyz(Config, 0.4092, 0.2812, 0.3060);
    public static readonly List<Unicolour> AllHue0 = new()
    {
        Hue0Ref,
        Unicolour.FromXyz(Config, 0.024951, 0.019086, 0.020329),	
        Unicolour.FromXyz(Config, 0.033296, 0.019086, 0.021063),	
        Unicolour.FromXyz(Config, 0.057386, 0.029891, 0.032447),	
        Unicolour.FromXyz(Config, 0.073706, 0.062359, 0.070514),	
        Unicolour.FromXyz(Config, 0.090418, 0.062359, 0.068901),	
        Unicolour.FromXyz(Config, 0.109443, 0.062359, 0.067881),	
        Unicolour.FromXyz(Config, 0.120597, 0.062359, 0.066805),	
        Unicolour.FromXyz(Config, 0.216475, 0.112510, 0.123602),	
        Unicolour.FromXyz(Config, 0.204172, 0.184187, 0.201965),	
        Unicolour.FromXyz(Config, 0.236417, 0.184187, 0.201739),	
        Unicolour.FromXyz(Config, 0.271865, 0.184187, 0.195653),	
        Unicolour.FromXyz(Config, 0.310729, 0.184187, 0.197150),	
        Unicolour.FromXyz(Config, 0.355801, 0.184187, 0.181154),	
        Unicolour.FromXyz(Config, 0.522309, 0.281233, 0.306758),	
        Unicolour.FromXyz(Config, 0.436118, 0.407494, 0.442210),	
        Unicolour.FromXyz(Config, 0.488989, 0.407494, 0.448546),	
        Unicolour.FromXyz(Config, 0.546008, 0.407494, 0.442074),	
        Unicolour.FromXyz(Config, 0.594653, 0.407494, 0.450086),	
        Unicolour.FromXyz(Config, 0.691717, 0.566813, 0.619143),	
        Unicolour.FromXyz(Config, 0.803849, 0.763034, 0.836166)
    };
    
    public static readonly Unicolour Hue24Ref = Unicolour.FromXyz(Config, 0.3953, 0.2812, 0.1845);
    public static readonly List<Unicolour> AllHue24 = new()
    {
        Hue24Ref,
        Unicolour.FromXyz(Config, 0.024364, 0.019086, 0.014688),	
        Unicolour.FromXyz(Config, 0.031680, 0.019086, 0.009391),	
        Unicolour.FromXyz(Config, 0.055818, 0.029891, 0.011895),	
        Unicolour.FromXyz(Config, 0.072650, 0.062359, 0.054620),	
        Unicolour.FromXyz(Config, 0.087473, 0.062359, 0.041462),	
        Unicolour.FromXyz(Config, 0.103312, 0.062359, 0.028546),	
        Unicolour.FromXyz(Config, 0.116178, 0.062359, 0.021270),	
        Unicolour.FromXyz(Config, 0.208807, 0.112510, 0.037728),	
        Unicolour.FromXyz(Config, 0.201146, 0.184187, 0.167921),	
        Unicolour.FromXyz(Config, 0.230654, 0.184187, 0.142768),	
        Unicolour.FromXyz(Config, 0.263119, 0.184187, 0.121097),	
        Unicolour.FromXyz(Config, 0.294922, 0.184187, 0.091904),	
        Unicolour.FromXyz(Config, 0.337924, 0.184187, 0.056569),	
        Unicolour.FromXyz(Config, 0.419121, 0.229298, 0.063588),	
        Unicolour.FromXyz(Config, 0.527130, 0.340472, 0.186420),	
        Unicolour.FromXyz(Config, 0.431587, 0.407494, 0.390086),	
        Unicolour.FromXyz(Config, 0.478373, 0.407494, 0.337280),	
        Unicolour.FromXyz(Config, 0.528667, 0.407494, 0.290411),	
        Unicolour.FromXyz(Config, 0.665260, 0.566813, 0.457942),	
        Unicolour.FromXyz(Config, 0.790894, 0.763034, 0.742766)
    };
    
    public static readonly Unicolour Hue48Ref = Unicolour.FromXyz(Config, 0.3577, 0.2812, 0.1125);
    public static readonly List<Unicolour> AllHue48 = new()
    {
        Hue48Ref,
        Unicolour.FromXyz(Config, 0.036781, 0.029891, 0.014811),	
        Unicolour.FromXyz(Config, 0.043912, 0.029891, 0.004572),	
        Unicolour.FromXyz(Config, 0.072096, 0.062359, 0.036436),	
        Unicolour.FromXyz(Config, 0.086369, 0.062359, 0.016378),	
        Unicolour.FromXyz(Config, 0.093726, 0.062359, 0.007023),	
        Unicolour.FromXyz(Config, 0.171277, 0.112510, 0.012299),	
        Unicolour.FromXyz(Config, 0.159324, 0.145417, 0.098473),	
        Unicolour.FromXyz(Config, 0.184248, 0.145417, 0.057809),	
        Unicolour.FromXyz(Config, 0.208121, 0.145417, 0.028115),	
        Unicolour.FromXyz(Config, 0.272709, 0.184187, 0.017329),	
        Unicolour.FromXyz(Config, 0.300809, 0.281233, 0.212298),	
        Unicolour.FromXyz(Config, 0.337425, 0.281233, 0.140395),	
        Unicolour.FromXyz(Config, 0.371325, 0.281233, 0.082291),	
        Unicolour.FromXyz(Config, 0.389601, 0.281233, 0.035885),	
        Unicolour.FromXyz(Config, 0.409676, 0.281233, 0.022750),	
        Unicolour.FromXyz(Config, 0.529760, 0.407494, 0.117200),	
        Unicolour.FromXyz(Config, 0.501955, 0.482781, 0.380164),	
        Unicolour.FromXyz(Config, 0.545519, 0.482781, 0.261853),	
        Unicolour.FromXyz(Config, 0.633032, 0.566813, 0.326901),	
        Unicolour.FromXyz(Config, 0.770216, 0.763034, 0.651076)
    };
    
    public static readonly Unicolour Hue72Ref = Unicolour.FromXyz(Config, 0.5151, 0.4828, 0.1778);
    public static readonly List<Unicolour> AllHue72 = new()
    {
        Hue72Ref,
        Unicolour.FromXyz(Config, 0.033197, 0.029891, 0.011480),	
        Unicolour.FromXyz(Config, 0.035138, 0.029891, 0.006227),	
        Unicolour.FromXyz(Config, 0.066846, 0.062359, 0.030931),	
        Unicolour.FromXyz(Config, 0.074603, 0.062359, 0.011325),	
        Unicolour.FromXyz(Config, 0.134731, 0.112510, 0.018616),	
        Unicolour.FromXyz(Config, 0.148686, 0.145417, 0.087303),	
        Unicolour.FromXyz(Config, 0.163904, 0.145417, 0.043255),	
        Unicolour.FromXyz(Config, 0.217903, 0.184187, 0.029308),	
        Unicolour.FromXyz(Config, 0.285178, 0.281233, 0.192981),	
        Unicolour.FromXyz(Config, 0.304615, 0.281233, 0.112259),	
        Unicolour.FromXyz(Config, 0.323158, 0.281233, 0.057361),	
        Unicolour.FromXyz(Config, 0.331870, 0.281233, 0.042045),	
        Unicolour.FromXyz(Config, 0.460119, 0.407494, 0.056175),	
        Unicolour.FromXyz(Config, 0.483113, 0.482781, 0.358235),	
        Unicolour.FromXyz(Config, 0.504509, 0.482781, 0.228894),	
        Unicolour.FromXyz(Config, 0.525746, 0.482781, 0.134945),	
        Unicolour.FromXyz(Config, 0.546262, 0.482781, 0.070769),	
        Unicolour.FromXyz(Config, 0.616913, 0.566813, 0.164547),	
        Unicolour.FromXyz(Config, 0.678322, 0.660070, 0.333101),	
        Unicolour.FromXyz(Config, 0.754525, 0.763034, 0.541791)
    };
    
    public static readonly Unicolour Hue96Ref = Unicolour.FromXyz(Config, 0.5184, 0.5668, 0.2112);
    public static readonly List<Unicolour> AllHue96 = new()
    {
        Hue96Ref,
        Unicolour.FromXyz(Config, 0.028909, 0.029891, 0.010142),	
        Unicolour.FromXyz(Config, 0.059861, 0.062359, 0.028395),	
        Unicolour.FromXyz(Config, 0.061870, 0.062359, 0.010384),	
        Unicolour.FromXyz(Config, 0.108221, 0.112510, 0.017582),	
        Unicolour.FromXyz(Config, 0.138453, 0.145417, 0.084043),	
        Unicolour.FromXyz(Config, 0.140725, 0.145417, 0.037708),	
        Unicolour.FromXyz(Config, 0.174718, 0.184187, 0.027642),	
        Unicolour.FromXyz(Config, 0.266956, 0.281233, 0.186195),	
        Unicolour.FromXyz(Config, 0.267106, 0.281233, 0.102615),	
        Unicolour.FromXyz(Config, 0.265222, 0.281233, 0.048764),	
        Unicolour.FromXyz(Config, 0.378172, 0.407494, 0.060499),	
        Unicolour.FromXyz(Config, 0.452991, 0.482781, 0.349471),	
        Unicolour.FromXyz(Config, 0.448363, 0.482781, 0.217887),	
        Unicolour.FromXyz(Config, 0.444752, 0.482781, 0.124260),	
        Unicolour.FromXyz(Config, 0.512315, 0.566813, 0.085942),	
        Unicolour.FromXyz(Config, 0.709748, 0.763034, 0.589282),	
        Unicolour.FromXyz(Config, 0.701174, 0.763034, 0.398412),	
        Unicolour.FromXyz(Config, 0.698154, 0.763034, 0.253302),	
        Unicolour.FromXyz(Config, 0.705383, 0.763034, 0.148054),	
        Unicolour.FromXyz(Config, 0.704427, 0.763034, 0.112490)
    };
    
    public static readonly Unicolour Hue120Ref = Unicolour.FromXyz(Config, 0.3765, 0.4828, 0.1994);
    public static readonly List<Unicolour> AllHue120 = new()
    {
        Hue120Ref,
        Unicolour.FromXyz(Config, 0.023295, 0.029891, 0.012151),	
        Unicolour.FromXyz(Config, 0.050776, 0.062359, 0.032350),	
        Unicolour.FromXyz(Config, 0.045813, 0.062359, 0.010648),	
        Unicolour.FromXyz(Config, 0.123217, 0.145417, 0.091954),	
        Unicolour.FromXyz(Config, 0.111245, 0.145417, 0.045894),	
        Unicolour.FromXyz(Config, 0.131275, 0.184187, 0.025870),	
        Unicolour.FromXyz(Config, 0.242342, 0.281233, 0.201576),	
        Unicolour.FromXyz(Config, 0.222813, 0.281233, 0.120243),	
        Unicolour.FromXyz(Config, 0.204891, 0.281233, 0.064150),	
        Unicolour.FromXyz(Config, 0.200979, 0.281233, 0.040060),	
        Unicolour.FromXyz(Config, 0.420356, 0.482781, 0.376153),	
        Unicolour.FromXyz(Config, 0.392198, 0.482781, 0.248400),	
        Unicolour.FromXyz(Config, 0.365407, 0.482781, 0.153761),	
        Unicolour.FromXyz(Config, 0.344596, 0.482781, 0.084394),	
        Unicolour.FromXyz(Config, 0.400128, 0.566813, 0.081820),	
        Unicolour.FromXyz(Config, 0.526749, 0.763034, 0.113140),	
        Unicolour.FromXyz(Config, 0.672532, 0.763034, 0.625084),	
        Unicolour.FromXyz(Config, 0.628152, 0.763034, 0.449196),	
        Unicolour.FromXyz(Config, 0.586004, 0.763034, 0.309764),	
        Unicolour.FromXyz(Config, 0.555877, 0.763034, 0.194968)
    };
    
    public static readonly Unicolour Hue144Ref = Unicolour.FromXyz(Config, 0.2873, 0.4828, 0.2109);
    public static readonly List<Unicolour> AllHue144 = new()
    {
        Hue144Ref,
        Unicolour.FromXyz(Config, 0.020570, 0.029891, 0.016832),	
        Unicolour.FromXyz(Config, 0.013952, 0.029891, 0.008032),	
        Unicolour.FromXyz(Config, 0.086184, 0.112510, 0.085273),	
        Unicolour.FromXyz(Config, 0.069267, 0.112510, 0.053448),	
        Unicolour.FromXyz(Config, 0.055532, 0.112510, 0.028928),	
        Unicolour.FromXyz(Config, 0.082777, 0.184187, 0.041966),	
        Unicolour.FromXyz(Config, 0.228649, 0.281233, 0.234216),	
        Unicolour.FromXyz(Config, 0.194735, 0.281233, 0.171960),	
        Unicolour.FromXyz(Config, 0.163555, 0.281233, 0.124344),	
        Unicolour.FromXyz(Config, 0.138615, 0.281233, 0.079293),	
        Unicolour.FromXyz(Config, 0.126312, 0.281233, 0.063504),	
        Unicolour.FromXyz(Config, 0.281591, 0.566813, 0.162474),	
        Unicolour.FromXyz(Config, 0.252804, 0.566813, 0.129911),	
        Unicolour.FromXyz(Config, 0.476451, 0.566813, 0.499746),	
        Unicolour.FromXyz(Config, 0.419222, 0.566813, 0.398854),	
        Unicolour.FromXyz(Config, 0.372596, 0.566813, 0.293883),	
        Unicolour.FromXyz(Config, 0.321493, 0.566813, 0.231624),	
        Unicolour.FromXyz(Config, 0.295765, 0.660070, 0.147727),	
        Unicolour.FromXyz(Config, 0.476799, 0.763034, 0.351210),	
        Unicolour.FromXyz(Config, 0.701190, 0.876183, 0.690486)
    };
    
    public static readonly Unicolour Hue168Ref = Unicolour.FromXyz(Config, 0.3076, 0.4828, 0.4277);
    public static readonly List<Unicolour> AllHue168 = new()
    {
        Hue168Ref,
        Unicolour.FromXyz(Config, 0.021080, 0.029891, 0.027904),	
        Unicolour.FromXyz(Config, 0.016702, 0.029891, 0.023311),	
        Unicolour.FromXyz(Config, 0.034115, 0.062359, 0.051996),	
        Unicolour.FromXyz(Config, 0.088419, 0.112510, 0.113399),	
        Unicolour.FromXyz(Config, 0.072572, 0.112510, 0.098606),	
        Unicolour.FromXyz(Config, 0.061947, 0.112510, 0.093344),	
        Unicolour.FromXyz(Config, 0.100632, 0.184187, 0.151057),	
        Unicolour.FromXyz(Config, 0.232833, 0.281233, 0.285625),	
        Unicolour.FromXyz(Config, 0.201319, 0.281233, 0.269643),	
        Unicolour.FromXyz(Config, 0.173352, 0.281233, 0.244685),	
        Unicolour.FromXyz(Config, 0.152558, 0.281233, 0.232349),	
        Unicolour.FromXyz(Config, 0.221968, 0.407494, 0.339779),	
        Unicolour.FromXyz(Config, 0.483497, 0.566813, 0.579764),	
        Unicolour.FromXyz(Config, 0.432329, 0.566813, 0.544056),	
        Unicolour.FromXyz(Config, 0.385515, 0.566813, 0.501932),	
        Unicolour.FromXyz(Config, 0.341577, 0.566813, 0.470353),	
        Unicolour.FromXyz(Config, 0.308994, 0.566813, 0.445270),	
        Unicolour.FromXyz(Config, 0.359901, 0.660070, 0.507477),	
        Unicolour.FromXyz(Config, 0.490016, 0.763034, 0.638159),	
        Unicolour.FromXyz(Config, 0.622486, 0.829670, 0.763862)
    };
    
    public static readonly Unicolour Hue192Ref = Unicolour.FromXyz(Config, 0.3488, 0.4828, 0.6019);
    public static readonly List<Unicolour> AllHue192 = new()
    {
        Hue192Ref,
        Unicolour.FromXyz(Config, 0.023453, 0.029891, 0.037174),	
        Unicolour.FromXyz(Config, 0.019260, 0.029891, 0.044245),	
        Unicolour.FromXyz(Config, 0.040173, 0.062359, 0.096191),	
        Unicolour.FromXyz(Config, 0.094684, 0.112510, 0.133777),	
        Unicolour.FromXyz(Config, 0.083988, 0.112510, 0.152001),	
        Unicolour.FromXyz(Config, 0.073638, 0.112510, 0.165584),	
        Unicolour.FromXyz(Config, 0.116981, 0.184187, 0.255249),	
        Unicolour.FromXyz(Config, 0.243887, 0.281233, 0.319199),	
        Unicolour.FromXyz(Config, 0.222844, 0.281233, 0.345849),	
        Unicolour.FromXyz(Config, 0.202715, 0.281233, 0.368170),	
        Unicolour.FromXyz(Config, 0.182763, 0.281233, 0.373909),	
        Unicolour.FromXyz(Config, 0.254976, 0.407494, 0.528133),	
        Unicolour.FromXyz(Config, 0.501257, 0.566813, 0.640076),	
        Unicolour.FromXyz(Config, 0.465487, 0.566813, 0.660339),	
        Unicolour.FromXyz(Config, 0.431626, 0.566813, 0.683981),	
        Unicolour.FromXyz(Config, 0.398701, 0.566813, 0.692512),	
        Unicolour.FromXyz(Config, 0.352616, 0.566813, 0.704797),	
        Unicolour.FromXyz(Config, 0.460800, 0.741641, 0.881726),	
        Unicolour.FromXyz(Config, 0.582443, 0.807044, 0.939569),	
        Unicolour.FromXyz(Config, 0.714332, 0.876183, 0.999575)
    };
    
    public static readonly Unicolour Hue216Ref = Unicolour.FromXyz(Config, 0.3787, 0.4828, 0.7231);
    public static readonly List<Unicolour> AllHue216 = new()
    {
        Hue216Ref,
        Unicolour.FromXyz(Config, 0.024666, 0.029891, 0.044851),	
        Unicolour.FromXyz(Config, 0.022448, 0.029891, 0.059111),	
        Unicolour.FromXyz(Config, 0.046165, 0.062359, 0.120332),	
        Unicolour.FromXyz(Config, 0.097400, 0.112510, 0.149949),	
        Unicolour.FromXyz(Config, 0.088882, 0.112510, 0.183141),	
        Unicolour.FromXyz(Config, 0.081128, 0.112510, 0.214127),	
        Unicolour.FromXyz(Config, 0.132261, 0.184187, 0.345513),	
        Unicolour.FromXyz(Config, 0.248999, 0.281233, 0.353653),	
        Unicolour.FromXyz(Config, 0.231907, 0.281233, 0.407240),	
        Unicolour.FromXyz(Config, 0.216271, 0.281233, 0.469210),	
        Unicolour.FromXyz(Config, 0.198433, 0.281233, 0.504344),	
        Unicolour.FromXyz(Config, 0.282041, 0.407494, 0.690229),	
        Unicolour.FromXyz(Config, 0.508535, 0.566813, 0.688802),	
        Unicolour.FromXyz(Config, 0.478889, 0.566813, 0.762798),	
        Unicolour.FromXyz(Config, 0.446584, 0.566813, 0.820460),	
        Unicolour.FromXyz(Config, 0.416195, 0.566813, 0.881993),	
        Unicolour.FromXyz(Config, 0.390704, 0.566813, 0.944482),	
        Unicolour.FromXyz(Config, 0.498666, 0.660070, 1.002007),	
        Unicolour.FromXyz(Config, 0.629735, 0.763034, 1.030157),	
        Unicolour.FromXyz(Config, 0.780945, 0.876183, 1.053757)
    };

    public static readonly Unicolour Hue240Ref = Unicolour.FromXyz(Config, 0.3421, 0.4075, 0.7197);
    public static readonly List<Unicolour> AllHue240 = new()
    {
        Hue240Ref,
        Unicolour.FromXyz(Config, 0.026197, 0.029891, 0.049052),	
        Unicolour.FromXyz(Config, 0.024789, 0.029891, 0.069067),	
        Unicolour.FromXyz(Config, 0.050943, 0.062359, 0.145770),	
        Unicolour.FromXyz(Config, 0.101162, 0.112510, 0.159858),	
        Unicolour.FromXyz(Config, 0.096673, 0.112510, 0.206324),	
        Unicolour.FromXyz(Config, 0.092055, 0.112510, 0.260341),	
        Unicolour.FromXyz(Config, 0.148942, 0.184187, 0.428274),	
        Unicolour.FromXyz(Config, 0.255352, 0.281233, 0.371142),	
        Unicolour.FromXyz(Config, 0.244632, 0.281233, 0.446614),	
        Unicolour.FromXyz(Config, 0.234074, 0.281233, 0.531310),	
        Unicolour.FromXyz(Config, 0.222932, 0.281233, 0.623583),	
        Unicolour.FromXyz(Config, 0.319264, 0.407494, 0.921437),	
        Unicolour.FromXyz(Config, 0.441905, 0.482781, 0.618135),	
        Unicolour.FromXyz(Config, 0.423672, 0.482781, 0.717357),	
        Unicolour.FromXyz(Config, 0.407141, 0.482781, 0.829579),	
        Unicolour.FromXyz(Config, 0.392461, 0.482781, 0.956828),	
        Unicolour.FromXyz(Config, 0.472962, 0.566813, 1.003071),	
        Unicolour.FromXyz(Config, 0.572536, 0.660070, 1.015921),	
        Unicolour.FromXyz(Config, 0.679355, 0.763034, 1.034360)
    };
    
    public static readonly Unicolour Hue264Ref = Unicolour.FromXyz(Config, 0.3149, 0.3405, 0.6623);
    public static readonly List<Unicolour> AllHue264 = new()
    {
        Hue264Ref,
        Unicolour.FromXyz(Config, 0.027982, 0.029891, 0.086685),	
        Unicolour.FromXyz(Config, 0.041208, 0.044155, 0.071309),	
        Unicolour.FromXyz(Config, 0.041612, 0.044155, 0.101433),	
        Unicolour.FromXyz(Config, 0.060966, 0.062359, 0.187302),	
        Unicolour.FromXyz(Config, 0.105606, 0.112510, 0.164317),	
        Unicolour.FromXyz(Config, 0.104042, 0.112510, 0.214695),	
        Unicolour.FromXyz(Config, 0.104747, 0.112510, 0.275383),	
        Unicolour.FromXyz(Config, 0.105836, 0.112510, 0.338709),	
        Unicolour.FromXyz(Config, 0.181547, 0.184187, 0.553577),	
        Unicolour.FromXyz(Config, 0.262553, 0.281233, 0.380059),	
        Unicolour.FromXyz(Config, 0.258303, 0.281233, 0.465439),	
        Unicolour.FromXyz(Config, 0.259521, 0.281233, 0.566217),	
        Unicolour.FromXyz(Config, 0.267324, 0.281233, 0.680523),	
        Unicolour.FromXyz(Config, 0.280301, 0.281233, 0.844180),	
        Unicolour.FromXyz(Config, 0.338060, 0.340472, 0.968792),	
        Unicolour.FromXyz(Config, 0.424315, 0.451644, 0.999617),	
        Unicolour.FromXyz(Config, 0.528874, 0.566813, 0.731468),	
        Unicolour.FromXyz(Config, 0.523387, 0.566813, 0.862998),	
        Unicolour.FromXyz(Config, 0.524765, 0.566813, 1.013278),	
        Unicolour.FromXyz(Config, 0.708398, 0.763034, 1.029002)
    };
    
    public static readonly Unicolour Hue288Ref = Unicolour.FromXyz(Config, 0.2051, 0.1842, 0.5713);
    public static readonly List<Unicolour> AllHue288 = new()
    {
        Hue288Ref,
        Unicolour.FromXyz(Config, 0.030398, 0.029891, 0.061233),	
        Unicolour.FromXyz(Config, 0.033574, 0.029891, 0.101548),	
        Unicolour.FromXyz(Config, 0.037776, 0.029891, 0.140607),	
        Unicolour.FromXyz(Config, 0.081239, 0.062359, 0.289466),	
        Unicolour.FromXyz(Config, 0.083832, 0.084984, 0.147224),	
        Unicolour.FromXyz(Config, 0.088700, 0.084984, 0.218435),	
        Unicolour.FromXyz(Config, 0.097338, 0.084984, 0.303378),	
        Unicolour.FromXyz(Config, 0.111523, 0.084984, 0.393449),	
        Unicolour.FromXyz(Config, 0.150163, 0.112510, 0.504059),	
        Unicolour.FromXyz(Config, 0.179346, 0.184187, 0.289351),	
        Unicolour.FromXyz(Config, 0.184058, 0.184187, 0.401114),	
        Unicolour.FromXyz(Config, 0.198420, 0.184187, 0.527573),	
        Unicolour.FromXyz(Config, 0.218326, 0.184187, 0.664797),	
        Unicolour.FromXyz(Config, 0.244740, 0.184187, 0.834294),	
        Unicolour.FromXyz(Config, 0.323931, 0.281233, 0.951349),	
        Unicolour.FromXyz(Config, 0.325501, 0.340472, 0.502969),	
        Unicolour.FromXyz(Config, 0.331968, 0.340472, 0.662406),	
        Unicolour.FromXyz(Config, 0.353672, 0.340472, 0.839130),	
        Unicolour.FromXyz(Config, 0.368082, 0.340472, 0.969789),	
        Unicolour.FromXyz(Config, 0.456537, 0.451644, 1.003590),	
        Unicolour.FromXyz(Config, 0.538261, 0.566813, 0.800104),	
        Unicolour.FromXyz(Config, 0.553271, 0.566813, 1.012963),	
        Unicolour.FromXyz(Config, 0.723061, 0.763034, 1.051867)
    };
    
    public static readonly Unicolour Hue312Ref = Unicolour.FromXyz(Config, 0.2765, 0.1842, 0.6201);
    public static readonly List<Unicolour> AllHue312 = new()
    {
        Hue312Ref,
        Unicolour.FromXyz(Config, 0.037048, 0.029891, 0.059648),	
        Unicolour.FromXyz(Config, 0.047540, 0.029891, 0.097663),	
        Unicolour.FromXyz(Config, 0.060374, 0.029891, 0.146405),	
        Unicolour.FromXyz(Config, 0.073117, 0.029891, 0.197734),	
        Unicolour.FromXyz(Config, 0.151352, 0.062359, 0.427145),	
        Unicolour.FromXyz(Config, 0.190702, 0.084984, 0.520012),	
        Unicolour.FromXyz(Config, 0.096873, 0.084984, 0.146268),	
        Unicolour.FromXyz(Config, 0.114333, 0.084984, 0.220239),	
        Unicolour.FromXyz(Config, 0.136517, 0.084984, 0.303894),	
        Unicolour.FromXyz(Config, 0.161808, 0.084984, 0.403975),	
        Unicolour.FromXyz(Config, 0.272723, 0.112510, 0.781810),	
        Unicolour.FromXyz(Config, 0.340040, 0.184187, 0.857156),	
        Unicolour.FromXyz(Config, 0.199887, 0.184187, 0.292578),	
        Unicolour.FromXyz(Config, 0.226043, 0.184187, 0.412528),	
        Unicolour.FromXyz(Config, 0.260220, 0.184187, 0.539744),	
        Unicolour.FromXyz(Config, 0.296726, 0.184187, 0.693691),	
        Unicolour.FromXyz(Config, 0.427495, 0.281233, 0.953079),	
        Unicolour.FromXyz(Config, 0.358782, 0.340472, 0.511040),	
        Unicolour.FromXyz(Config, 0.397286, 0.340472, 0.681086),	
        Unicolour.FromXyz(Config, 0.445219, 0.340472, 0.862127),	
        Unicolour.FromXyz(Config, 0.520133, 0.407494, 0.999868),	
        Unicolour.FromXyz(Config, 0.584479, 0.566813, 0.818948),	
        Unicolour.FromXyz(Config, 0.633425, 0.566813, 1.040201),	
        Unicolour.FromXyz(Config, 0.774496, 0.763034, 1.068820)
    };
    
    public static readonly Unicolour Hue336Ref = Unicolour.FromXyz(Config, 0.4562, 0.2812, 0.5520);
    public static readonly List<Unicolour> AllHue336 = new()
    {
        Hue336Ref,
        Unicolour.FromXyz(Config, 0.059792, 0.029891, 0.067983),	
        Unicolour.FromXyz(Config, 0.056611, 0.044155, 0.064892),	
        Unicolour.FromXyz(Config, 0.074487, 0.044155, 0.084184),	
        Unicolour.FromXyz(Config, 0.125371, 0.062359, 0.148021),	
        Unicolour.FromXyz(Config, 0.168870, 0.145417, 0.198849),	
        Unicolour.FromXyz(Config, 0.205757, 0.145417, 0.232471),	
        Unicolour.FromXyz(Config, 0.246615, 0.145417, 0.276573),	
        Unicolour.FromXyz(Config, 0.290646, 0.145417, 0.339618),	
        Unicolour.FromXyz(Config, 0.371009, 0.184187, 0.443260),	
        Unicolour.FromXyz(Config, 0.553607, 0.281233, 0.752024),	
        Unicolour.FromXyz(Config, 0.314203, 0.281233, 0.368351),	
        Unicolour.FromXyz(Config, 0.368426, 0.281233, 0.424991),	
        Unicolour.FromXyz(Config, 0.427739, 0.281233, 0.492386),	
        Unicolour.FromXyz(Config, 0.488039, 0.281233, 0.601262),	
        Unicolour.FromXyz(Config, 0.650875, 0.407494, 0.827335),	
        Unicolour.FromXyz(Config, 0.524811, 0.482781, 0.617910),	
        Unicolour.FromXyz(Config, 0.598114, 0.482781, 0.713579),	
        Unicolour.FromXyz(Config, 0.673205, 0.482781, 0.848059),	
        Unicolour.FromXyz(Config, 0.725461, 0.566813, 0.913143),	
        Unicolour.FromXyz(Config, 0.825715, 0.763034, 0.988142)
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