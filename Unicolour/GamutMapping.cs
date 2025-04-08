namespace Wacton.Unicolour;

internal static class GamutMapping
{
    internal static Unicolour ToRgbGamut(Unicolour colour, GamutMap gamutMap)
    {
        if (colour.IsInRgbGamut)
        {
            return colour.Clone();
        }

        // could do some early checks for unusual values (NaN, infinity, etc)
        // but don't want to add more edge cases to the algorithms
        // which should end before too long anyway
        return gamutMap switch
        {
            GamutMap.RgbClipping => RgbClipping(colour),
            GamutMap.OklchChromaReduction => OklchChromaReduction(colour),
            GamutMap.WxyPurityReduction => WxyPurityReduction(colour),
            _ => throw new ArgumentOutOfRangeException(nameof(gamutMap), gamutMap, null)
        };
    }

    internal static Unicolour ToPointerGamut(Unicolour colour)
    {
        var config = colour.Configuration;
        var alpha = colour.Alpha.A;
        var lchab = colour.Lchab;
        
        if (colour.IsInPointerGamut)
        {
            return colour.Clone();
        }

        if (lchab.UseAsNaN)
        {
            return new Unicolour(config, ColourSpace.Xyz, double.NaN, double.NaN, double.NaN, alpha);
        }
        
        var (l, _, h) = colour.ConvertToConfiguration(PointerGamut.Config.Value).Lchab;
        l = l.Clamp(PointerGamut.MinL, PointerGamut.MaxL);

        var gamutBoundaryColour = lchab.UseAsGreyscale
            ? new Unicolour(PointerGamut.Config.Value, ColourSpace.Lab, l, 0, 0, alpha)
            : new Unicolour(PointerGamut.Config.Value, ColourSpace.Lchab, l, PointerGamut.GetMaxC(l, h), h, alpha);

        return gamutBoundaryColour.ConvertToConfiguration(config);
    }

    private static Unicolour RgbClipping(Unicolour colour)
    {
        return new Unicolour(colour.Configuration, ColourSpace.Rgb, colour.Rgb.ConstrainedTuple, colour.Alpha.A);
    }
    
    /*
     * adapted from https://www.w3.org/TR/css-color-4/#css-gamut-mapping & https://www.w3.org/TR/css-color-4/#binsearch
     * the pseudocode doesn't appear to handle the edge case scenario where:
     * a) origin colour OKLCH chroma < epsilon
     * b) origin colour destination (RGB here) is out-of-gamut
     * e.g. OKLCH (0.99999, 0, 0) --> RGB (1.00010, 0.99998, 0.99974)
     * - the search never executes since chroma = 0 (min 0 - max 0 < epsilon 0.0001)
     * - even if the search did execute, would not return clipped variant since ΔE is *too small*, and min never changes from 0
     * so need to clip if the mapped colour is somehow out-of-gamut (i.e. not processed)
     */
    private static Unicolour OklchChromaReduction(Unicolour colour)
    {
        var config = colour.Configuration;
        var alpha = colour.Alpha.A;
        
        var oklch = colour.Oklch;
        if (oklch.L >= 1.0) return new Unicolour(config, ColourSpace.Rgb, 1, 1, 1, alpha);
        if (oklch.L <= 0.0) return new Unicolour(config, ColourSpace.Rgb, 0, 0, 0, alpha);

        const double jnd = 0.02;
        const double epsilon = 0.0001;
        var minChroma = 0.0;
        var maxChroma = oklch.C;
        var minChromaInGamut = true;

        // iteration count ensures the while loop doesn't get stuck in an endless cycle if bad input is provided
        // e.g. double.Epsilon
        var iterations = 0;
        Unicolour? current = null;
        bool HasChromaConverged() => maxChroma - minChroma <= epsilon;
        while (!HasChromaConverged() && iterations < 1000)
        {
            iterations++;

            var chroma = (minChroma + maxChroma) / 2.0;
            current = new Unicolour(config, ColourSpace.Oklch, oklch.L, chroma, oklch.H, alpha);
            
            if (minChromaInGamut && current.Rgb.IsInGamut)
            {
                minChroma = chroma;
                continue;
            }

            var clipped = RgbClipping(current);
            var deltaE = clipped.Difference(current, DeltaE.Ok);
            
            var isNoticeableDifference = deltaE >= jnd;
            if (isNoticeableDifference)
            {
                maxChroma = chroma;
            }
            else
            {
                // not clear to me why a clipped colour must have ΔE from "current" colour between 0.0199 - 0.02
                // effectively: only returning clipped when ΔE == JND, but continue if the non-noticeable ΔE is *too small*
                // but I assume it's something to do with this comment about intersecting shallow and concave gamut boundaries
                // https://github.com/w3c/csswg-drafts/issues/7653#issuecomment-1489096489
                var isUnnoticeableDifferenceLargeEnough = jnd - deltaE < epsilon;
                if (isUnnoticeableDifferenceLargeEnough)
                {
                    return clipped;
                }
                
                minChromaInGamut = false;
                minChroma = chroma;
            }
        }

        // in case while loop never executes (e.g. Oklch.C == 0)
        current ??= new Unicolour(config, ColourSpace.Oklch, oklch.Tuple, alpha);
        
        // it's possible for the "current" colour to still be out of RGB gamut, either because:
        // a) the original OKLCH was not processed (chroma too low) and was already out of RGB gamut
        // b) the algorithm converged on an OKLCH that is out of RGB gamut (happens ~5% of the time for me with using random OKLCH inputs)
        return current.IsInRgbGamut ? current : RgbClipping(current);
    }
    
    private static Unicolour WxyPurityReduction(Unicolour colour)
    {
        var config = colour.Configuration;
        var alpha = colour.Alpha.A;

        var (w, x, y) = colour.Wxy;
        x = x.Clamp(0.0, 1.0); // no point starting with purity outwith 0 - 100%
        y = y.Clamp(0.0, 1.0); // luminance also needs to be bound for a sensible result 
        var current = new Unicolour(config, ColourSpace.Wxy, w, x, y, alpha);
        while (!current.IsInRgbGamut && x > 0)
        {
            x -= 0.001; // at most 1000 iterations from purity of 1 to 0
            current = new Unicolour(config, ColourSpace.Wxy, w, x, y, alpha);
        }
        
        // if purity has been reduced from 100% to 0% and no colours have been found to be in gamut
        // then there is no solution that is in gamut
        return current.IsInRgbGamut ? current : new Unicolour(config, ColourSpace.Wxy, double.NaN, double.NaN, double.NaN, alpha);
    }
}