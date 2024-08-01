using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests.Utils;

public record IccTestColour(Profile Profile, IccTransform Transform, string Source, Intent Intent, double[] Input, double[] Output)
{
    public override string ToString() => $"{Source}, {Transform}, {Intent}, [{string.Join(", ", Input)}] -> [{string.Join(", ", Output)}]";
}

public enum IccTransform { ToPcs, ToDevice }
