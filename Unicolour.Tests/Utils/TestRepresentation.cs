namespace Wacton.Unicolour.Tests.Utils;

internal record TestRepresentation : ColourRepresentation
{
    internal TestRepresentation(double first, double second, double third, ColourMode colourMode) : base(first, second, third, colourMode) { }

    private ColourSpace colourSpace;
    internal void OverrideColourSpace(ColourSpace colourSpaceOverride)
    {
        colourSpace = colourSpaceOverride;
    }

    internal override ColourSpace ColourSpace => colourSpace;
    protected override int? HueIndex => null;
    internal override bool IsGreyscale => false;
    protected override string FirstString => string.Empty;
    protected override string SecondString => string.Empty;
    protected override string ThirdString => string.Empty;
}