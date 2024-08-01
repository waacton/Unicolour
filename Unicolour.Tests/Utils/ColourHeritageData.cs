using System.Collections.Generic;
using System.Linq;

namespace Wacton.Unicolour.Tests.Utils;

internal class ColourHeritageData
{
    private readonly Dictionary<ColourSpace, ColourHeritage> heritages;
    private readonly Dictionary<ColourSpace, bool> greyscale;
    private readonly Dictionary<ColourSpace, bool> hued;
    private readonly Dictionary<ColourSpace, bool> nan;
    
    internal ColourHeritageData(Unicolour unicolour)
    {
        var all = TestUtils.AllColourSpaces.ToDictionary(x => x, unicolour.GetRepresentation);
        heritages = all.ToDictionary(x => x.Key, x => x.Value.Heritage);
        greyscale = all.ToDictionary(x => x.Key, x => x.Value.UseAsGreyscale);
        hued = all.ToDictionary(x => x.Key, x => x.Value.UseAsHued);
        nan = all.ToDictionary(x => x.Key, x => x.Value.UseAsNaN);
    }

    public List<ColourHeritage> Heritages(List<ColourSpace> spaces) => Filter(heritages, spaces);
    public List<bool> UseAsGreyscale(List<ColourSpace> spaces) => Filter(greyscale, spaces);
    public List<bool> UseAsHued(List<ColourSpace> spaces) => Filter(hued, spaces);
    public List<bool> UseAsNaN(List<ColourSpace> spaces) => Filter(nan, spaces);
        
    private static List<T> Filter<T>(Dictionary<ColourSpace, T> dictionary, List<ColourSpace> spaces)
    {
        return dictionary.Where(x => spaces.Contains(x.Key)).Select(x => x.Value).ToList();
    }
}