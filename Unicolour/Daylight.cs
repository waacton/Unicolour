namespace Wacton.Unicolour;

internal static class Daylight
{
    internal static Spd GetSpd(double cct)
    {
        var wavelengths = SpectraComponents.Keys;
        var dictionary = wavelengths.ToDictionary(wavelength => wavelength, wavelength => SpectralPower(wavelength, cct));
        return new Spd(dictionary);
    }
    
    internal static Chromaticity GetChromaticity(double cct)
    {
        if (cct is < 4000 or > 25000) return new(double.NaN, double.NaN);
        var xd = cct < 7000
            ? -4.6070e9 / Math.Pow(cct, 3) + 2.9678e6 / Math.Pow(cct, 2) + 0.09911e3 / cct + 0.244063
            : -2.0064e9 / Math.Pow(cct, 3) + 1.9018e6 / Math.Pow(cct, 2) + 0.24748e3 / cct + 0.237040;
        var yd = -3.000 * Math.Pow(xd, 2) + 2.870 * xd - 0.275;
        return new(xd, yd);
    }
    
    private static double SpectralPower(int wavelength, double cct)
    {
        var (xd, yd) = GetChromaticity(cct);
        var m1 = (-1.3515 - 1.7703 * xd + 5.9114 * yd) / (0.0241 + 0.2562 * xd - 0.7341 * yd);
        var m2 = (0.0300 - 31.4424 * xd + 30.0717 * yd) / (0.0241 + 0.2562 * xd - 0.7341 * yd);
        var s0 = SpectraComponents[wavelength].s0;
        var s1 = SpectraComponents[wavelength].s1;
        var s2 = SpectraComponents[wavelength].s2;
        return s0 + m1 * s1 + m2 * s2;
    }

    private static readonly Dictionary<int, (double s0, double s1, double s2)> SpectraComponents = new()
    {
        { 300, (0.04, 0.02, 0) },
        { 305, (3.02, 2.26, 1) },
        { 310, (6, 4.5, 2) },
        { 315, (17.8, 13.45, 3) },
        { 320, (29.6, 22.4, 4) },
        { 325, (42.45, 32.2, 6.25) },
        { 330, (55.3, 42, 8.5) },
        { 335, (56.3, 41.3, 8.15) },
        { 340, (57.3, 40.6, 7.8) },
        { 345, (59.55, 41.1, 7.25) },
        { 350, (61.8, 41.6, 6.7) },
        { 355, (61.65, 39.8, 6) },
        { 360, (61.5, 38, 5.3) },
        { 365, (65.15, 40.2, 5.7) },
        { 370, (68.8, 42.4, 6.1) },
        { 375, (66.1, 40.45, 4.55) },
        { 380, (63.4, 38.5, 3) },
        { 385, (64.6, 36.75, 2.1) },
        { 390, (65.8, 35, 1.2) },
        { 395, (80.3, 39.2, 0.05) },
        { 400, (94.8, 43.4, -1.1) },
        { 405, (99.8, 44.85, -0.8) },
        { 410, (104.8, 46.3, -0.5) },
        { 415, (105.35, 45.1, -0.6) },
        { 420, (105.9, 43.9, -0.7) },
        { 425, (101.35, 40.5, -0.95) },
        { 430, (96.8, 37.1, -1.2) },
        { 435, (105.35, 36.9, -1.9) },
        { 440, (113.9, 36.7, -2.6) },
        { 445, (119.75, 36.3, -2.75) },
        { 450, (125.6, 35.9, -2.9) },
        { 455, (125.55, 34.25, -2.85) },
        { 460, (125.5, 32.6, -2.8) },
        { 465, (123.4, 30.25, -2.7) },
        { 470, (121.3, 27.9, -2.6) },
        { 475, (121.3, 26.1, -2.6) },
        { 480, (121.3, 24.3, -2.6) },
        { 485, (117.4, 22.2, -2.2) },
        { 490, (113.5, 20.1, -1.8) },
        { 495, (113.3, 18.15, -1.65) },
        { 500, (113.1, 16.2, -1.5) },
        { 505, (111.95, 14.7, -1.4) },
        { 510, (110.8, 13.2, -1.3) },
        { 515, (108.65, 10.9, -1.25) },
        { 520, (106.5, 8.6, -1.2) },
        { 525, (107.65, 7.35, -1.1) },
        { 530, (108.8, 6.1, -1) },
        { 535, (107.05, 5.15, -0.75) },
        { 540, (105.3, 4.2, -0.5) },
        { 545, (104.85, 3.05, -0.4) },
        { 550, (104.4, 1.9, -0.3) },
        { 555, (102.2, 0.95, -0.15) },
        { 560, (100.0, 0, 0) },
        { 565, (98, -0.8, 0.1) },
        { 570, (96, -1.6, 0.2) },
        { 575, (95.55, -2.55, 0.35) },
        { 580, (95.1, -3.5, 0.5) },
        { 585, (92.1, -3.5, 1.3) },
        { 590, (89.1, -3.5, 2.1) },
        { 595, (89.8, -4.65, 2.65) },
        { 600, (90.5, -5.8, 3.2) },
        { 605, (90.4, -6.5, 3.65) },
        { 610, (90.3, -7.2, 4.1) },
        { 615, (89.35, -7.9, 4.4) },
        { 620, (88.4, -8.6, 4.7) },
        { 625, (86.2, -9.05, 4.9) },
        { 630, (84, -9.5, 5.1) },
        { 635, (84.55, -10.2, 5.9) },
        { 640, (85.1, -10.9, 6.7) },
        { 645, (83.5, -10.8, 7) },
        { 650, (81.9, -10.7, 7.3) },
        { 655, (82.25, -11.35, 7.95) },
        { 660, (82.6, -12, 8.6) },
        { 665, (83.75, -13, 9.2) },
        { 670, (84.9, -14, 9.8) },
        { 675, (83.1, -13.8, 10) },
        { 680, (81.3, -13.6, 10.2) },
        { 685, (76.6, -12.8, 9.25) },
        { 690, (71.9, -12, 8.3) },
        { 695, (73.1, -12.65, 8.95) },
        { 700, (74.3, -13.3, 9.6) },
        { 705, (75.35, -13.1, 9.05) },
        { 710, (76.4, -12.9, 8.5) },
        { 715, (69.85, -11.75, 7.75) },
        { 720, (63.3, -10.6, 7) },
        { 725, (67.5, -11.1, 7.3) },
        { 730, (71.7, -11.6, 7.6) },
        { 735, (74.35, -11.9, 7.8) },
        { 740, (77, -12.2, 8) },
        { 745, (71.1, -11.2, 7.35) },
        { 750, (65.2, -10.2, 6.7) },
        { 755, (56.45, -9, 5.95) },
        { 760, (47.7, -7.8, 5.2) },
        { 765, (58.15, -9.5, 6.3) },
        { 770, (68.6, -11.2, 7.4) },
        { 775, (66.8, -10.8, 7.1) },
        { 780, (65, -10.4, 6.8) },
        { 785, (65.5, -10.5, 6.9) },
        { 790, (66, -10.6, 7) },
        { 795, (63.5, -10.15, 6.7) },
        { 800, (61, -9.7, 6.4) },
        { 805, (57.15, -9, 5.95) },
        { 810, (53.3, -8.3, 5.5) },
        { 815, (56.1, -8.8, 5.8) },
        { 820, (58.9, -9.3, 6.1) },
        { 825, (60.4, -9.55, 6.3) },
        { 830, (61.9, -9.8, 6.5) }
    };
}