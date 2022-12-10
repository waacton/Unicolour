namespace Wacton.Unicolour;

internal record ColourMode(string description)
{
    private readonly string description = description;
    
    internal static readonly ColourMode Unset = new(nameof(Unset));
    internal static readonly ColourMode NoExplicitBehaviour = new(nameof(NoExplicitBehaviour));
    internal static readonly ColourMode ExplicitNaN = new(nameof(ExplicitNaN));
    internal static readonly ColourMode ExplicitHue = new(nameof(ExplicitHue));
    internal static readonly ColourMode ExplicitGreyscale = new(nameof(ExplicitGreyscale));
    
    internal static ColourMode Default(int? hueIndex) => hueIndex != null ? ExplicitHue : NoExplicitBehaviour;
    
    internal static ColourMode FromRepresentation(ColourRepresentation previous)
    {
        // don't propagate ExplicitHue, it's only used for the initial representation
        if (previous.IsEffectivelyNaN) return ExplicitNaN;
        if (previous.IsEffectivelyGreyscale) return ExplicitGreyscale; 
        return NoExplicitBehaviour;
    }

    public override string ToString() => description;
}