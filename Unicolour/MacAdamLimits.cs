namespace Wacton.Unicolour;

internal static class MacAdamLimits
{
    internal static readonly Configuration Config = Configuration.Default;

    // https://doi.org/10.2307/1420820 Table II(3.7) - D65 values
    private static readonly Lazy<Dictionary<double, Chromaticity[]>> Limits = new(() =>
        new Dictionary<double, Chromaticity[]>
        {
            {
                10, new Chromaticity[]
                {
                    new(0.1346, 0.0747), new(0.0990, 0.1607), new(0.0751, 0.2403), new(0.0391, 0.4074),
                    new(0.0211, 0.5490), new(0.0177, 0.6693), new(0.0344, 0.7732), new(0.0516, 0.8055),
                    new(0.0727, 0.8223), new(0.0959, 0.8261), new(0.1188, 0.8213), new(0.7035, 0.2965),
                    new(0.6832, 0.2853), new(0.6470, 0.2653), new(0.5517, 0.2132), new(0.5309, 0.2019),
                    new(0.4346, 0.1504), new(0.3999, 0.1324), new(0.3549, 0.1101), new(0.3207, 0.0945),
                    new(0.2989, 0.0857), new(0.2852, 0.0808), new(0.2660, 0.0755), new(0.2186, 0.0707)
                }
            },
            {
                20, new Chromaticity[]
                {
                    new(0.1268, 0.1365), new(0.1081, 0.1984), new(0.0894, 0.2766), new(0.0660, 0.4074),
                    new(0.0549, 0.4971), new(0.0479, 0.6227), new(0.0565, 0.7312), new(0.0927, 0.8005),
                    new(0.1289, 0.8078), new(0.1479, 0.8026), new(0.1664, 0.7941), new(0.6708, 0.3289),
                    new(0.6591, 0.3213), new(0.5988, 0.2820), new(0.5514, 0.2513), new(0.5018, 0.2197),
                    new(0.4502, 0.1874), new(0.4045, 0.1601), new(0.3762, 0.1443), new(0.3440, 0.1284),
                    new(0.3185, 0.1196), new(0.2935, 0.1164), new(0.2528, 0.1189), new(0.2205, 0.1229)
                }
            },
            {
                30, new Chromaticity[]
                {
                    new(0.1282, 0.1889), new(0.1067, 0.3003), new(0.0990, 0.3535), new(0.0929, 0.4041),
                    new(0.0846, 0.5028), new(0.0819, 0.6020), new(0.0836, 0.6491), new(0.1004, 0.7433),
                    new(0.1481, 0.7857), new(0.1799, 0.7787), new(0.2119, 0.7609), new(0.6368, 0.3628),
                    new(0.6281, 0.3561), new(0.5682, 0.3098), new(0.5271, 0.2784), new(0.4977, 0.2562),
                    new(0.4504, 0.2212), new(0.4219, 0.2008), new(0.3999, 0.1859), new(0.3801, 0.1732),
                    new(0.3491, 0.1574), new(0.3350, 0.1536), new(0.3190, 0.1526), new(0.2021, 0.1732)
                }
            },
            {
                40, new Chromaticity[]
                {
                    new(0.1360, 0.2324), new(0.1266, 0.3030), new(0.1219, 0.3504), new(0.1183, 0.3985),
                    new(0.1155, 0.4509), new(0.1141, 0.5055), new(0.1312, 0.7047), new(0.1516, 0.7454),
                    new(0.1853, 0.7580), new(0.2129, 0.7510), new(0.2415, 0.7344), new(0.6041, 0.3954),
                    new(0.5969, 0.3888), new(0.5524, 0.3484), new(0.5257, 0.3244), new(0.4980, 0.2997),
                    new(0.4598, 0.2661), new(0.3696, 0.1949), new(0.3603, 0.1898), new(0.3501, 0.1859),
                    new(0.3375, 0.1841), new(0.2581, 0.2001), new(0.2220, 0.2095), new(0.1771, 0.2214)
                }
            },
            {
                50, new Chromaticity[]
                {
                    new(0.1491, 0.2679), new(0.1441, 0.3511), new(0.1429, 0.4025), new(0.1429, 0.4479),
                    new(0.1472, 0.5522), new(0.1548, 0.6201), new(0.1621, 0.6570), new(0.1790, 0.7035),
                    new(0.1929, 0.7201), new(0.2114, 0.7277), new(0.2991, 0.6851), new(0.5731, 0.4262),
                    new(0.5668, 0.4195), new(0.5492, 0.4009), new(0.4795, 0.3281), new(0.4514, 0.2994),
                    new(0.4113, 0.2600), new(0.3897, 0.2401), new(0.3509, 0.2139), new(0.3391, 0.2126),
                    new(0.3211, 0.2155), new(0.3042, 0.2200), new(0.2466, 0.2374), new(0.2041, 0.2507)
                }
            },
            {
                60, new Chromaticity[]
                {
                    new(0.1674, 0.2959), new(0.1677, 0.3520), new(0.1700, 0.4130), new(0.1749, 0.4782),
                    new(0.1801, 0.5257), new(0.1873, 0.5730), new(0.1994, 0.6257), new(0.2088, 0.6523),
                    new(0.2506, 0.6927), new(0.2703, 0.6900), new(0.2930, 0.6798), new(0.5435, 0.4552),
                    new(0.5379, 0.4483), new(0.4775, 0.3751), new(0.4522, 0.3450), new(0.4138, 0.3005),
                    new(0.3611, 0.2472), new(0.3497, 0.2405), new(0.3395, 0.2388), new(0.3195, 0.2429),
                    new(0.2963, 0.2505), new(0.2701, 0.2595), new(0.2270, 0.2747), new(0.2037, 0.2830)
                }
            },
            {
                70, new Chromaticity[]
                {
                    new(0.1916, 0.3164), new(0.1958, 0.3656), new(0.2003, 0.4049), new(0.2065, 0.4485),
                    new(0.2150, 0.4963), new(0.2221, 0.5295), new(0.2298, 0.5597), new(0.2402, 0.5918),
                    new(0.2550, 0.6237), new(0.2784, 0.6484), new(0.3000, 0.6521), new(0.5148, 0.4825),
                    new(0.5097, 0.4753), new(0.4776, 0.4304), new(0.4508, 0.3933), new(0.4192, 0.3505),
                    new(0.4005, 0.3259), new(0.3706, 0.2890), new(0.3663, 0.2842), new(0.3517, 0.2699),
                    new(0.3364, 0.2634), new(0.3194, 0.2671), new(0.3007, 0.2739), new(0.2664, 0.2872)
                }
            },
            {
                80, new Chromaticity[]
                {
                    new(0.2232, 0.3290), new(0.2404, 0.4145), new(0.2496, 0.4504), new(0.2583, 0.4801),
                    new(0.2760, 0.5308), new(0.3023, 0.5809), new(0.3092, 0.5892), new(0.3318, 0.6041),
                    new(0.3515, 0.6048), new(0.3679, 0.5995), new(0.4080, 0.5750), new(0.4858, 0.5081),
                    new(0.4811, 0.5005), new(0.4634, 0.4719), new(0.4514, 0.4526), new(0.4299, 0.4158),
                    new(0.4001, 0.3720), new(0.3732, 0.3319), new(0.3603, 0.3139), new(0.3500, 0.3009),
                    new(0.3307, 0.2866), new(0.2730, 0.3080), new(0.2519, 0.3169), new(0.2400, 0.3219)
                }
            },
            {
                90, new Chromaticity[]
                {
                    new(0.2639, 0.3331), new(0.2801, 0.3832), new(0.2864, 0.4008), new(0.3059, 0.4486),
                    new(0.3182, 0.4746), new(0.3317, 0.4994), new(0.3513, 0.5278), new(0.3657, 0.5421),
                    new(0.3946, 0.5537), new(0.4126, 0.5510), new(0.4354, 0.5406), new(0.4530, 0.5293),
                    new(0.4486, 0.5210), new(0.4444, 0.5131), new(0.4325, 0.4906), new(0.4215, 0.4700),
                    new(0.3990, 0.4284), new(0.3749, 0.3849), new(0.3504, 0.3431), new(0.3349, 0.3196),
                    new(0.3217, 0.3084), new(0.3099, 0.3124), new(0.2852, 0.3235), new(0.2711, 0.3299)
                }
            },
            {
                95, new Chromaticity[]
                {
                    new(0.2875, 0.3320), new(0.2949, 0.3513), new(0.3067, 0.3800), new(0.3230, 0.4150),
                    new(0.3368, 0.4415), new(0.3508, 0.4654), new(0.3644, 0.4856), new(0.3765, 0.5007),
                    new(0.3887, 0.5126), new(0.4003, 0.5206), new(0.4108, 0.5251), new(0.4281, 0.5268),
                    new(0.4204, 0.5109), new(0.4132, 0.4959), new(0.4031, 0.4751), new(0.3697, 0.4076),
                    new(0.3498, 0.3692), new(0.3401, 0.3513), new(0.3295, 0.3331), new(0.3167, 0.3189),
                    new(0.3148, 0.3195), new(0.3103, 0.3214), new(0.3006, 0.3259), new(0.2900, 0.3308)
                }
            }
        }
    );

    private static readonly Lazy<Chromaticity[]> LimitsMin = new(LimitsAtZero);
    private static readonly Lazy<Chromaticity[]> LimitsMax = new(() => Enumerable.Range(0, 24).Select(_ => Config.Xyz.WhiteChromaticity).ToArray()); // every limit point converges to white

    internal static bool IsInLimits(Unicolour colour)
    {
        if (colour.Configuration.Xyz.Illuminant != Config.Xyz.Illuminant 
            || colour.Configuration.Xyz.Observer != Config.Xyz.Observer)
        {
            colour = colour.ConvertToConfiguration(Config);
        }
        
        if (colour.Xyy.UseAsNaN || colour.Xyz.UseAsNaN) return false;

        var (x, y, luminance) = colour.Xyy;
        if (luminance >= 1)
        {
            var sameX = (x - colour.Configuration.Xyz.WhiteChromaticity.X).IsEffectivelyZero();
            var sameY = (y - colour.Configuration.Xyz.WhiteChromaticity.Y).IsEffectivelyZero();
            return sameX && sameY;
        }
        
        var boundary = new Boundary(new Lazy<Chromaticity[]>(() => Get(luminance)));
        return boundary.Contains(colour.Chromaticity);
    }
    
    internal static Chromaticity[] Get(double y)
    {
        y = y.Clamp(0, 1) * 100;
        return y switch
        {
            0 => LimitsMin.Value,
            100 => LimitsMax.Value,
            _ => Limits.Value.TryGetValue(y, out var limits) ? limits : GetInterpolated(y)
        };
    }

    // naive but intuitive implementation
    // a future improvement would be to implement the approach at https://www.researchgate.net/publication/39435417_A_new_algorithm_for_calculating_the_MacAdam_limits_for_any_luminance_factor_hue_angle_and_illuminant
    // and calculate limits by systematically searching all optimal colours
    private static Chromaticity[] GetInterpolated(double y)
    {
        var ys = new[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 100 };
        var lowerY = ys.Last(x => x <= y);
        var upperY = ys.First(x => x >= y);
        var distance = (y - lowerY) / (upperY - lowerY);
        
        var result = new List<Chromaticity>();
        for (var i = 0; i < 24; i++)
        {
            var lower = Lookup(lowerY);
            var upper = Lookup(upperY);
            var interpolatedX = Interpolation.Linear(lower[i].X, upper[i].X, distance);
            var interpolatedY = Interpolation.Linear(lower[i].Y, upper[i].Y, distance);
            result.Add(new(interpolatedX, interpolatedY));
        }

        return result.ToArray();

        Chromaticity[] Lookup(double knownY)
        {
            return knownY switch
            {
                0 => LimitsMin.Value,
                100 => LimitsMax.Value,
                _ => Limits.Value[knownY]
            };
        }
    }
    
    // NOTE: could precalculate this for a performance improvement, though not expecting it to be used much
    private static Chromaticity[] LimitsAtZero()
    {
        var spectralBoundary = Config.Xyz.SpectralBoundary;
        var zeroPoints = new List<Chromaticity>();
        for (var i = 0; i < 24; i++) 
        {
            // uses each pair of 20 -> 10 macadam points to extrapolate the spectral boundary intersect
            var intersects = spectralBoundary.GetIntersects(sample: Limits.Value[10][i], reference: Limits.Value[20][i]);
            zeroPoints.Add(intersects!.Value.near.Point);
        }
        
        // extrapolation doesn't quite converge to pure red and blue wavelengths
        // (the "corners" of the chromaticity diagram)
        // so replace specific extrapolated values with pure red and blue
        // to give the largest possible area of the chromaticity diagram 
        var red = spectralBoundary.GetChromaticity(SpectralBoundary.MaxWavelength, purity: 1.0);
        var blue = spectralBoundary.GetChromaticity(SpectralBoundary.MinWavelength, purity: 1.0);
        zeroPoints[11] = red;
        zeroPoints[23] = blue;
        return zeroPoints.ToArray();
    }
}

