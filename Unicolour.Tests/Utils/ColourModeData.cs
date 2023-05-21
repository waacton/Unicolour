namespace Wacton.Unicolour.Tests.Utils;

using System.Collections.Generic;
using System.Linq;

internal class ColourModeData
{
    private readonly Dictionary<ColourSpace, ColourMode> modes;
    private readonly Dictionary<ColourSpace, bool> greyscale;
    private readonly Dictionary<ColourSpace, bool> hued;
    private readonly Dictionary<ColourSpace, bool> nan;
    
    internal ColourModeData(Unicolour unicolour)
    {
        var all = AssertUtils.AllColourSpaces.ToDictionary(x => x, unicolour.GetRepresentation);
        modes = all.ToDictionary(x => x.Key, x => x.Value.ColourMode);
        greyscale = all.ToDictionary(x => x.Key, x => x.Value.IsEffectivelyGreyscale);
        hued = all.ToDictionary(x => x.Key, x => x.Value.IsEffectivelyHued);
        nan = all.ToDictionary(x => x.Key, x => x.Value.IsEffectivelyNaN);
    }

    public List<ColourMode> Modes(List<ColourSpace> spaces) => Filter(modes, spaces);
    public List<bool> Greyscale(List<ColourSpace> spaces) => Filter(greyscale, spaces);
    public List<bool> Hued(List<ColourSpace> spaces) => Filter(hued, spaces);
    public List<bool> NaN(List<ColourSpace> spaces) => Filter(nan, spaces);
        
    private static List<T> Filter<T>(Dictionary<ColourSpace, T> dictionary, List<ColourSpace> spaces)
    {
        return dictionary.Where(x => spaces.Contains(x.Key)).Select(x => x.Value).ToList();
    }
}