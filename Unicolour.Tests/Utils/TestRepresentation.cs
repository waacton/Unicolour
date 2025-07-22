namespace Wacton.Unicolour.Tests.Utils;

internal record TestRepresentation : ColourRepresentation
{
    internal TestRepresentation(double first, double second, double third, ColourHeritage heritage) : base(first, second, third, heritage) { }
    
    protected override int? HueIndex => null;
    internal override bool IsGreyscale => false;
    protected override string String => string.Empty;
}