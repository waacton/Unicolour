namespace Wacton.Unicolour;

internal record ColourDescription(string description)
{
    private readonly string description = description;

    internal static readonly ColourDescription NotApplicable = new("-"); 
    internal static readonly ColourDescription Black = new(nameof(Black)); 
    internal static readonly ColourDescription Shadow = new(nameof(Shadow)); 
    internal static readonly ColourDescription Dark = new(nameof(Dark)); 
    internal static readonly ColourDescription Pure = new(nameof(Pure)); 
    internal static readonly ColourDescription Light = new(nameof(Light)); 
    internal static readonly ColourDescription Pale = new(nameof(Pale)); 
    internal static readonly ColourDescription White = new(nameof(White));
    internal static readonly ColourDescription Grey = new(nameof(Grey)); 
    internal static readonly ColourDescription Faint = new(nameof(Faint)); 
    internal static readonly ColourDescription Weak = new(nameof(Weak)); 
    internal static readonly ColourDescription Mild = new(nameof(Mild)); 
    internal static readonly ColourDescription Strong = new(nameof(Strong)); 
    internal static readonly ColourDescription Vibrant = new(nameof(Vibrant));
    internal static readonly ColourDescription Red = new(nameof(Red)); 
    internal static readonly ColourDescription Orange = new(nameof(Orange)); 
    internal static readonly ColourDescription Yellow = new(nameof(Yellow)); 
    internal static readonly ColourDescription Chartreuse = new(nameof(Chartreuse)); 
    internal static readonly ColourDescription Green = new(nameof(Green)); 
    internal static readonly ColourDescription Mint = new(nameof(Mint)); 
    internal static readonly ColourDescription Cyan = new(nameof(Cyan)); 
    internal static readonly ColourDescription Azure = new(nameof(Azure)); 
    internal static readonly ColourDescription Blue = new(nameof(Blue)); 
    internal static readonly ColourDescription Violet = new(nameof(Violet)); 
    internal static readonly ColourDescription Magenta = new(nameof(Magenta)); 
    internal static readonly ColourDescription Rose = new(nameof(Rose));

    internal static readonly List<ColourDescription> Lightnesses = new() { Shadow, Dark, Pure, Light, Pale };
    internal static readonly List<ColourDescription> Saturations = new() { Faint, Weak, Mild, Strong, Vibrant };
    internal static readonly List<ColourDescription> Hues = new() { Red, Orange, Yellow, Chartreuse, Green, Mint, Cyan, Azure, Blue, Violet, Magenta, Rose };
    internal static readonly List<ColourDescription> Greyscales = new() { Black, Grey, White };

    public override string ToString() => description.ToLower();
}