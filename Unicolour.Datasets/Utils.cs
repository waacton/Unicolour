namespace Wacton.Unicolour.Datasets;

internal static class Utils
{
    internal static Unicolour? FromName(string name, Dictionary<string, Unicolour> lookup)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        
        // first try to use the name as though it is the exact text used in the spec
        var lowercase = name.ToLower();
        lookup.TryGetValue(lowercase, out var value);
        if (value != null)
        {
            return value;
        }

        // if that doesn't match, sanitise both name and keys to find potential matches
        // by removing all whitespace (including line separators) and any punctuation that appears in the keys
        var sanitisedName = Sanitise(lowercase);
        var potentialKeys = lookup.Keys.Where(x => Sanitise(x) == sanitisedName).ToList();
        return potentialKeys.Any() ? lookup[potentialKeys.First()] : null;

        string Sanitise(string text)
        {
            var noWhitespace = string.Concat(text.Where(x => !char.IsWhiteSpace(x)));
            return noWhitespace.Replace("/", string.Empty).Replace("'", string.Empty);
        }
    }
}