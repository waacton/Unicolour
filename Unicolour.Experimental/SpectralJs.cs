using System;
using System.Collections.Generic;
using System.Linq;

namespace Wacton.Unicolour.Experimental;

public static class SpectralJs
{
    /*
     * Spectral.js approximates a colour's reflectance (i.e. pigment composition)
     * by using 7 predefined reflectances as references, and calculating an average reflectance at each wavelength
     * (reflectance curves for RGB colours are generated using http://scottburns.us/reflectance-curves-from-srgb-2/)
     * ----------
     * Unicolour instead directly generates reflectance for each input colour using Scott Burns' approach
     * which sacrifices the speed of using predefined reference reflectances for greater accuracy
     * ----------
     * Unicolour also extends Spectral.js functionality
     * so that it can mix more than 2 colours together
     */
    public static Unicolour Mix(Unicolour[] colours, double[] weights)
    {
        // weight -> concentration calculation is the same as Pigment.GetReflectance()
        weights = weights.Select(x => Math.Max(x, 0.0)).ToArray();
        var totalWeight = weights.Sum();
        if (totalWeight == 0.0)
        {
            return new Unicolour(ColourSpace.Xyz, double.NaN, double.NaN, double.NaN);
        }
        
        var concentrations = weights.Select(x => x / totalWeight).ToArray();
        var pigments = colours.Select(PigmentGenerator.From).ToArray();
        return Mix(pigments, concentrations);
    }
    
    // Spectral.js is fundamentally just an adjustment of requested concentrations
    // based on luminance and a curve of unknown origin (hence experimental)
    private static Unicolour Mix(Pigment[] pigments, double[] concentrations)
    {
        var ls = new double[pigments.Length];
        for (var wavelengthIndex = 0; wavelengthIndex < PigmentGenerator.Wavelengths; wavelengthIndex++)
        {
            var wavelength = PigmentGenerator.Start + wavelengthIndex * PigmentGenerator.Interval;
            var yBar = Observer.Degree2.ColourMatchY(wavelength);
            
            // must be single-constant pigments
            // which is enforced by the pigments passed to this method being generated from RGB
            for (var pigmentIndex = 0; pigmentIndex < ls.Length; pigmentIndex++)
            {
                ls[pigmentIndex] += pigments[pigmentIndex].R![wavelength] * yBar;
            }
        }

        var t = new double[pigments.Length];
        for (var pigmentIndex = 0; pigmentIndex < pigments.Length; pigmentIndex++)
        {
            t[pigmentIndex] = ls[pigmentIndex] * Math.Pow(concentrations[pigmentIndex], 2);
        }

        var modifiedConcentrations = t.Select(x => x / t.Max()).ToArray();
        return new Unicolour(pigments, modifiedConcentrations);
    }
    
    public static IEnumerable<Unicolour> Palette(Unicolour start, Unicolour end, int count)
    {
        var colours = new[] { start, end };
        count = Math.Max(count, 0);

        var palette = new List<Unicolour>();
        for (var i = 0; i < count; i++)
        {
            var distance = count == 1 ? 0.5 : i / (double)(count - 1);
            var weights = new[] { 1 - distance, distance };
            palette.Add(Mix(colours, weights));
        }

        return palette;
    }
}