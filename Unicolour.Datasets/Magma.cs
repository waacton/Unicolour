namespace Wacton.Unicolour.Datasets;

// https://bids.github.io/colormap/
public class Magma : Colourmap
{
    internal Magma()
    {
    }
    
    public override Unicolour Map(double x) => InterpolateLookup(Lookup, x);
    public override string ToString() => nameof(Magma);
    
    public static readonly Unicolour[] Lookup = 
    {
        new(Config, ColourSpace.Rgb, 0.001462, 0.000466, 0.013866),
        new(Config, ColourSpace.Rgb, 0.002258, 0.001295, 0.018331),
        new(Config, ColourSpace.Rgb, 0.003279, 0.002305, 0.023708),
        new(Config, ColourSpace.Rgb, 0.004512, 0.00349, 0.029965),
        new(Config, ColourSpace.Rgb, 0.00595, 0.004843, 0.03713),
        new(Config, ColourSpace.Rgb, 0.007588, 0.006356, 0.044973),
        new(Config, ColourSpace.Rgb, 0.009426, 0.008022, 0.052844),
        new(Config, ColourSpace.Rgb, 0.011465, 0.009828, 0.06075),
        new(Config, ColourSpace.Rgb, 0.013708, 0.011771, 0.068667),
        new(Config, ColourSpace.Rgb, 0.016156, 0.01384, 0.076603),
        new(Config, ColourSpace.Rgb, 0.018815, 0.016026, 0.084584),
        new(Config, ColourSpace.Rgb, 0.021692, 0.01832, 0.09261),
        new(Config, ColourSpace.Rgb, 0.024792, 0.020715, 0.100676),
        new(Config, ColourSpace.Rgb, 0.028123, 0.023201, 0.108787),
        new(Config, ColourSpace.Rgb, 0.031696, 0.025765, 0.116965),
        new(Config, ColourSpace.Rgb, 0.03552, 0.028397, 0.125209),
        new(Config, ColourSpace.Rgb, 0.039608, 0.03109, 0.133515),
        new(Config, ColourSpace.Rgb, 0.04383, 0.03383, 0.141886),
        new(Config, ColourSpace.Rgb, 0.048062, 0.036607, 0.150327),
        new(Config, ColourSpace.Rgb, 0.05232, 0.039407, 0.158841),
        new(Config, ColourSpace.Rgb, 0.056615, 0.04216, 0.167446),
        new(Config, ColourSpace.Rgb, 0.060949, 0.044794, 0.176129),
        new(Config, ColourSpace.Rgb, 0.06533, 0.047318, 0.184892),
        new(Config, ColourSpace.Rgb, 0.069764, 0.049726, 0.193735),
        new(Config, ColourSpace.Rgb, 0.074257, 0.052017, 0.20266),
        new(Config, ColourSpace.Rgb, 0.078815, 0.054184, 0.211667),
        new(Config, ColourSpace.Rgb, 0.083446, 0.056225, 0.220755),
        new(Config, ColourSpace.Rgb, 0.088155, 0.058133, 0.229922),
        new(Config, ColourSpace.Rgb, 0.092949, 0.059904, 0.239164),
        new(Config, ColourSpace.Rgb, 0.097833, 0.061531, 0.248477),
        new(Config, ColourSpace.Rgb, 0.102815, 0.06301, 0.257854),
        new(Config, ColourSpace.Rgb, 0.107899, 0.064335, 0.267289),
        new(Config, ColourSpace.Rgb, 0.113094, 0.065492, 0.276784),
        new(Config, ColourSpace.Rgb, 0.118405, 0.066479, 0.286321),
        new(Config, ColourSpace.Rgb, 0.123833, 0.067295, 0.295879),
        new(Config, ColourSpace.Rgb, 0.12938, 0.067935, 0.305443),
        new(Config, ColourSpace.Rgb, 0.135053, 0.068391, 0.315),
        new(Config, ColourSpace.Rgb, 0.140858, 0.068654, 0.324538),
        new(Config, ColourSpace.Rgb, 0.146785, 0.068738, 0.334011),
        new(Config, ColourSpace.Rgb, 0.152839, 0.068637, 0.343404),
        new(Config, ColourSpace.Rgb, 0.159018, 0.068354, 0.352688),
        new(Config, ColourSpace.Rgb, 0.165308, 0.067911, 0.361816),
        new(Config, ColourSpace.Rgb, 0.171713, 0.067305, 0.370771),
        new(Config, ColourSpace.Rgb, 0.178212, 0.066576, 0.379497),
        new(Config, ColourSpace.Rgb, 0.184801, 0.065732, 0.387973),
        new(Config, ColourSpace.Rgb, 0.19146, 0.064818, 0.396152),
        new(Config, ColourSpace.Rgb, 0.198177, 0.063862, 0.404009),
        new(Config, ColourSpace.Rgb, 0.204935, 0.062907, 0.411514),
        new(Config, ColourSpace.Rgb, 0.211718, 0.061992, 0.418647),
        new(Config, ColourSpace.Rgb, 0.218512, 0.061158, 0.425392),
        new(Config, ColourSpace.Rgb, 0.225302, 0.060445, 0.431742),
        new(Config, ColourSpace.Rgb, 0.232077, 0.059889, 0.437695),
        new(Config, ColourSpace.Rgb, 0.238826, 0.059517, 0.443256),
        new(Config, ColourSpace.Rgb, 0.245543, 0.059352, 0.448436),
        new(Config, ColourSpace.Rgb, 0.25222, 0.059415, 0.453248),
        new(Config, ColourSpace.Rgb, 0.258857, 0.059706, 0.45771),
        new(Config, ColourSpace.Rgb, 0.265447, 0.060237, 0.46184),
        new(Config, ColourSpace.Rgb, 0.271994, 0.060994, 0.46566),
        new(Config, ColourSpace.Rgb, 0.278493, 0.061978, 0.46919),
        new(Config, ColourSpace.Rgb, 0.284951, 0.063168, 0.472451),
        new(Config, ColourSpace.Rgb, 0.291366, 0.064553, 0.475462),
        new(Config, ColourSpace.Rgb, 0.29774, 0.066117, 0.478243),
        new(Config, ColourSpace.Rgb, 0.304081, 0.067835, 0.480812),
        new(Config, ColourSpace.Rgb, 0.310382, 0.069702, 0.483186),
        new(Config, ColourSpace.Rgb, 0.316654, 0.07169, 0.48538),
        new(Config, ColourSpace.Rgb, 0.322899, 0.073782, 0.487408),
        new(Config, ColourSpace.Rgb, 0.329114, 0.075972, 0.489287),
        new(Config, ColourSpace.Rgb, 0.335308, 0.078236, 0.491024),
        new(Config, ColourSpace.Rgb, 0.341482, 0.080564, 0.492631),
        new(Config, ColourSpace.Rgb, 0.347636, 0.082946, 0.494121),
        new(Config, ColourSpace.Rgb, 0.353773, 0.085373, 0.495501),
        new(Config, ColourSpace.Rgb, 0.359898, 0.087831, 0.496778),
        new(Config, ColourSpace.Rgb, 0.366012, 0.090314, 0.49796),
        new(Config, ColourSpace.Rgb, 0.372116, 0.092816, 0.499053),
        new(Config, ColourSpace.Rgb, 0.378211, 0.095332, 0.500067),
        new(Config, ColourSpace.Rgb, 0.384299, 0.097855, 0.501002),
        new(Config, ColourSpace.Rgb, 0.390384, 0.100379, 0.501864),
        new(Config, ColourSpace.Rgb, 0.396467, 0.102902, 0.502658),
        new(Config, ColourSpace.Rgb, 0.402548, 0.10542, 0.503386),
        new(Config, ColourSpace.Rgb, 0.408629, 0.10793, 0.504052),
        new(Config, ColourSpace.Rgb, 0.414709, 0.110431, 0.504662),
        new(Config, ColourSpace.Rgb, 0.420791, 0.11292, 0.505215),
        new(Config, ColourSpace.Rgb, 0.426877, 0.115395, 0.505714),
        new(Config, ColourSpace.Rgb, 0.432967, 0.117855, 0.50616),
        new(Config, ColourSpace.Rgb, 0.439062, 0.120298, 0.506555),
        new(Config, ColourSpace.Rgb, 0.445163, 0.122724, 0.506901),
        new(Config, ColourSpace.Rgb, 0.451271, 0.125132, 0.507198),
        new(Config, ColourSpace.Rgb, 0.457386, 0.127522, 0.507448),
        new(Config, ColourSpace.Rgb, 0.463508, 0.129893, 0.507652),
        new(Config, ColourSpace.Rgb, 0.46964, 0.132245, 0.507809),
        new(Config, ColourSpace.Rgb, 0.47578, 0.134577, 0.507921),
        new(Config, ColourSpace.Rgb, 0.481929, 0.136891, 0.507989),
        new(Config, ColourSpace.Rgb, 0.488088, 0.139186, 0.508011),
        new(Config, ColourSpace.Rgb, 0.494258, 0.141462, 0.507988),
        new(Config, ColourSpace.Rgb, 0.500438, 0.143719, 0.50792),
        new(Config, ColourSpace.Rgb, 0.506629, 0.145958, 0.507806),
        new(Config, ColourSpace.Rgb, 0.512831, 0.148179, 0.507648),
        new(Config, ColourSpace.Rgb, 0.519045, 0.150383, 0.507443),
        new(Config, ColourSpace.Rgb, 0.52527, 0.152569, 0.507192),
        new(Config, ColourSpace.Rgb, 0.531507, 0.154739, 0.506895),
        new(Config, ColourSpace.Rgb, 0.537755, 0.156894, 0.506551),
        new(Config, ColourSpace.Rgb, 0.544015, 0.159033, 0.506159),
        new(Config, ColourSpace.Rgb, 0.550287, 0.161158, 0.505719),
        new(Config, ColourSpace.Rgb, 0.556571, 0.163269, 0.50523),
        new(Config, ColourSpace.Rgb, 0.562866, 0.165368, 0.504692),
        new(Config, ColourSpace.Rgb, 0.569172, 0.167454, 0.504105),
        new(Config, ColourSpace.Rgb, 0.57549, 0.16953, 0.503466),
        new(Config, ColourSpace.Rgb, 0.581819, 0.171596, 0.502777),
        new(Config, ColourSpace.Rgb, 0.588158, 0.173652, 0.502035),
        new(Config, ColourSpace.Rgb, 0.594508, 0.175701, 0.501241),
        new(Config, ColourSpace.Rgb, 0.600868, 0.177743, 0.500394),
        new(Config, ColourSpace.Rgb, 0.607238, 0.179779, 0.499492),
        new(Config, ColourSpace.Rgb, 0.613617, 0.181811, 0.498536),
        new(Config, ColourSpace.Rgb, 0.620005, 0.18384, 0.497524),
        new(Config, ColourSpace.Rgb, 0.626401, 0.185867, 0.496456),
        new(Config, ColourSpace.Rgb, 0.632805, 0.187893, 0.495332),
        new(Config, ColourSpace.Rgb, 0.639216, 0.189921, 0.49415),
        new(Config, ColourSpace.Rgb, 0.645633, 0.191952, 0.49291),
        new(Config, ColourSpace.Rgb, 0.652056, 0.193986, 0.491611),
        new(Config, ColourSpace.Rgb, 0.658483, 0.196027, 0.490253),
        new(Config, ColourSpace.Rgb, 0.664915, 0.198075, 0.488836),
        new(Config, ColourSpace.Rgb, 0.671349, 0.200133, 0.487358),
        new(Config, ColourSpace.Rgb, 0.677786, 0.202203, 0.485819),
        new(Config, ColourSpace.Rgb, 0.684224, 0.204286, 0.484219),
        new(Config, ColourSpace.Rgb, 0.690661, 0.206384, 0.482558),
        new(Config, ColourSpace.Rgb, 0.697098, 0.208501, 0.480835),
        new(Config, ColourSpace.Rgb, 0.703532, 0.210638, 0.479049),
        new(Config, ColourSpace.Rgb, 0.709962, 0.212797, 0.477201),
        new(Config, ColourSpace.Rgb, 0.716387, 0.214982, 0.47529),
        new(Config, ColourSpace.Rgb, 0.722805, 0.217194, 0.473316),
        new(Config, ColourSpace.Rgb, 0.729216, 0.219437, 0.471279),
        new(Config, ColourSpace.Rgb, 0.735616, 0.221713, 0.46918),
        new(Config, ColourSpace.Rgb, 0.742004, 0.224025, 0.467018),
        new(Config, ColourSpace.Rgb, 0.748378, 0.226377, 0.464794),
        new(Config, ColourSpace.Rgb, 0.754737, 0.228772, 0.462509),
        new(Config, ColourSpace.Rgb, 0.761077, 0.231214, 0.460162),
        new(Config, ColourSpace.Rgb, 0.767398, 0.233705, 0.457755),
        new(Config, ColourSpace.Rgb, 0.773695, 0.236249, 0.455289),
        new(Config, ColourSpace.Rgb, 0.779968, 0.238851, 0.452765),
        new(Config, ColourSpace.Rgb, 0.786212, 0.241514, 0.450184),
        new(Config, ColourSpace.Rgb, 0.792427, 0.244242, 0.447543),
        new(Config, ColourSpace.Rgb, 0.798608, 0.24704, 0.444848),
        new(Config, ColourSpace.Rgb, 0.804752, 0.249911, 0.442102),
        new(Config, ColourSpace.Rgb, 0.810855, 0.252861, 0.439305),
        new(Config, ColourSpace.Rgb, 0.816914, 0.255895, 0.436461),
        new(Config, ColourSpace.Rgb, 0.822926, 0.259016, 0.433573),
        new(Config, ColourSpace.Rgb, 0.828886, 0.262229, 0.430644),
        new(Config, ColourSpace.Rgb, 0.834791, 0.26554, 0.427671),
        new(Config, ColourSpace.Rgb, 0.840636, 0.268953, 0.424666),
        new(Config, ColourSpace.Rgb, 0.846416, 0.272473, 0.421631),
        new(Config, ColourSpace.Rgb, 0.852126, 0.276106, 0.418573),
        new(Config, ColourSpace.Rgb, 0.857763, 0.279857, 0.415496),
        new(Config, ColourSpace.Rgb, 0.86332, 0.283729, 0.412403),
        new(Config, ColourSpace.Rgb, 0.868793, 0.287728, 0.409303),
        new(Config, ColourSpace.Rgb, 0.874176, 0.291859, 0.406205),
        new(Config, ColourSpace.Rgb, 0.879464, 0.296125, 0.403118),
        new(Config, ColourSpace.Rgb, 0.884651, 0.30053, 0.400047),
        new(Config, ColourSpace.Rgb, 0.889731, 0.305079, 0.397002),
        new(Config, ColourSpace.Rgb, 0.8947, 0.309773, 0.393995),
        new(Config, ColourSpace.Rgb, 0.899552, 0.314616, 0.391037),
        new(Config, ColourSpace.Rgb, 0.904281, 0.31961, 0.388137),
        new(Config, ColourSpace.Rgb, 0.908884, 0.324755, 0.385308),
        new(Config, ColourSpace.Rgb, 0.913354, 0.330052, 0.382563),
        new(Config, ColourSpace.Rgb, 0.917689, 0.3355, 0.379915),
        new(Config, ColourSpace.Rgb, 0.921884, 0.341098, 0.377376),
        new(Config, ColourSpace.Rgb, 0.925937, 0.346844, 0.374959),
        new(Config, ColourSpace.Rgb, 0.929845, 0.352734, 0.372677),
        new(Config, ColourSpace.Rgb, 0.933606, 0.358764, 0.370541),
        new(Config, ColourSpace.Rgb, 0.937221, 0.364929, 0.368567),
        new(Config, ColourSpace.Rgb, 0.940687, 0.371224, 0.366762),
        new(Config, ColourSpace.Rgb, 0.944006, 0.377643, 0.365136),
        new(Config, ColourSpace.Rgb, 0.94718, 0.384178, 0.363701),
        new(Config, ColourSpace.Rgb, 0.95021, 0.39082, 0.362468),
        new(Config, ColourSpace.Rgb, 0.953099, 0.397563, 0.361438),
        new(Config, ColourSpace.Rgb, 0.955849, 0.4044, 0.360619),
        new(Config, ColourSpace.Rgb, 0.958464, 0.411324, 0.360014),
        new(Config, ColourSpace.Rgb, 0.960949, 0.418323, 0.35963),
        new(Config, ColourSpace.Rgb, 0.96331, 0.42539, 0.359469),
        new(Config, ColourSpace.Rgb, 0.965549, 0.432519, 0.359529),
        new(Config, ColourSpace.Rgb, 0.967671, 0.439703, 0.35981),
        new(Config, ColourSpace.Rgb, 0.96968, 0.446936, 0.360311),
        new(Config, ColourSpace.Rgb, 0.971582, 0.45421, 0.36103),
        new(Config, ColourSpace.Rgb, 0.973381, 0.46152, 0.361965),
        new(Config, ColourSpace.Rgb, 0.975082, 0.468861, 0.363111),
        new(Config, ColourSpace.Rgb, 0.97669, 0.476226, 0.364466),
        new(Config, ColourSpace.Rgb, 0.97821, 0.483612, 0.366025),
        new(Config, ColourSpace.Rgb, 0.979645, 0.491014, 0.367783),
        new(Config, ColourSpace.Rgb, 0.981, 0.498428, 0.369734),
        new(Config, ColourSpace.Rgb, 0.982279, 0.505851, 0.371874),
        new(Config, ColourSpace.Rgb, 0.983485, 0.51328, 0.374198),
        new(Config, ColourSpace.Rgb, 0.984622, 0.520713, 0.376698),
        new(Config, ColourSpace.Rgb, 0.985693, 0.528148, 0.379371),
        new(Config, ColourSpace.Rgb, 0.9867, 0.535582, 0.38221),
        new(Config, ColourSpace.Rgb, 0.987646, 0.543015, 0.38521),
        new(Config, ColourSpace.Rgb, 0.988533, 0.550446, 0.388365),
        new(Config, ColourSpace.Rgb, 0.989363, 0.557873, 0.391671),
        new(Config, ColourSpace.Rgb, 0.990138, 0.565296, 0.395122),
        new(Config, ColourSpace.Rgb, 0.990871, 0.572706, 0.398714),
        new(Config, ColourSpace.Rgb, 0.991558, 0.580107, 0.402441),
        new(Config, ColourSpace.Rgb, 0.992196, 0.587502, 0.406299),
        new(Config, ColourSpace.Rgb, 0.992785, 0.594891, 0.410283),
        new(Config, ColourSpace.Rgb, 0.993326, 0.602275, 0.41439),
        new(Config, ColourSpace.Rgb, 0.993834, 0.609644, 0.418613),
        new(Config, ColourSpace.Rgb, 0.994309, 0.616999, 0.42295),
        new(Config, ColourSpace.Rgb, 0.994738, 0.62435, 0.427397),
        new(Config, ColourSpace.Rgb, 0.995122, 0.631696, 0.431951),
        new(Config, ColourSpace.Rgb, 0.99548, 0.639027, 0.436607),
        new(Config, ColourSpace.Rgb, 0.99581, 0.646344, 0.441361),
        new(Config, ColourSpace.Rgb, 0.996096, 0.653659, 0.446213),
        new(Config, ColourSpace.Rgb, 0.996341, 0.660969, 0.45116),
        new(Config, ColourSpace.Rgb, 0.99658, 0.668256, 0.456192),
        new(Config, ColourSpace.Rgb, 0.996775, 0.675541, 0.461314),
        new(Config, ColourSpace.Rgb, 0.996925, 0.682828, 0.466526),
        new(Config, ColourSpace.Rgb, 0.997077, 0.690088, 0.471811),
        new(Config, ColourSpace.Rgb, 0.997186, 0.697349, 0.477182),
        new(Config, ColourSpace.Rgb, 0.997254, 0.704611, 0.482635),
        new(Config, ColourSpace.Rgb, 0.997325, 0.711848, 0.488154),
        new(Config, ColourSpace.Rgb, 0.997351, 0.719089, 0.493755),
        new(Config, ColourSpace.Rgb, 0.997351, 0.726324, 0.499428),
        new(Config, ColourSpace.Rgb, 0.997341, 0.733545, 0.505167),
        new(Config, ColourSpace.Rgb, 0.997285, 0.740772, 0.510983),
        new(Config, ColourSpace.Rgb, 0.997228, 0.747981, 0.516859),
        new(Config, ColourSpace.Rgb, 0.997138, 0.75519, 0.522806),
        new(Config, ColourSpace.Rgb, 0.997019, 0.762398, 0.528821),
        new(Config, ColourSpace.Rgb, 0.996898, 0.769591, 0.534892),
        new(Config, ColourSpace.Rgb, 0.996727, 0.776795, 0.541039),
        new(Config, ColourSpace.Rgb, 0.996571, 0.783977, 0.547233),
        new(Config, ColourSpace.Rgb, 0.996369, 0.791167, 0.553499),
        new(Config, ColourSpace.Rgb, 0.996162, 0.798348, 0.55982),
        new(Config, ColourSpace.Rgb, 0.995932, 0.805527, 0.566202),
        new(Config, ColourSpace.Rgb, 0.99568, 0.812706, 0.572645),
        new(Config, ColourSpace.Rgb, 0.995424, 0.819875, 0.57914),
        new(Config, ColourSpace.Rgb, 0.995131, 0.827052, 0.585701),
        new(Config, ColourSpace.Rgb, 0.994851, 0.834213, 0.592307),
        new(Config, ColourSpace.Rgb, 0.994524, 0.841387, 0.598983),
        new(Config, ColourSpace.Rgb, 0.994222, 0.84854, 0.605696),
        new(Config, ColourSpace.Rgb, 0.993866, 0.855711, 0.612482),
        new(Config, ColourSpace.Rgb, 0.993545, 0.862859, 0.619299),
        new(Config, ColourSpace.Rgb, 0.99317, 0.870024, 0.626189),
        new(Config, ColourSpace.Rgb, 0.992831, 0.877168, 0.633109),
        new(Config, ColourSpace.Rgb, 0.99244, 0.88433, 0.640099),
        new(Config, ColourSpace.Rgb, 0.992089, 0.89147, 0.647116),
        new(Config, ColourSpace.Rgb, 0.991688, 0.898627, 0.654202),
        new(Config, ColourSpace.Rgb, 0.991332, 0.905763, 0.661309),
        new(Config, ColourSpace.Rgb, 0.99093, 0.912915, 0.668481),
        new(Config, ColourSpace.Rgb, 0.99057, 0.920049, 0.675675),
        new(Config, ColourSpace.Rgb, 0.990175, 0.927196, 0.682926),
        new(Config, ColourSpace.Rgb, 0.989815, 0.934329, 0.690198),
        new(Config, ColourSpace.Rgb, 0.989434, 0.94147, 0.697519),
        new(Config, ColourSpace.Rgb, 0.989077, 0.948604, 0.704863),
        new(Config, ColourSpace.Rgb, 0.988717, 0.955742, 0.712242),
        new(Config, ColourSpace.Rgb, 0.988367, 0.962878, 0.719649),
        new(Config, ColourSpace.Rgb, 0.988033, 0.970012, 0.727077),
        new(Config, ColourSpace.Rgb, 0.987691, 0.977154, 0.734536),
        new(Config, ColourSpace.Rgb, 0.987387, 0.984288, 0.742002),
        new(Config, ColourSpace.Rgb, 0.987053, 0.991438, 0.749504)
    };
}