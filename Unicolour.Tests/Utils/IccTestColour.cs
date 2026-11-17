using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour.Tests.Utils;

public record IccTestColour(Profile Profile, IccTransform Transform, Intent Intent, double[] Input, double[] Output)
{
    public override string ToString() => $"{Transform}, {Intent}, [{string.Join(", ", Input)}] -> [{string.Join(", ", Output)}]";
}

public enum IccTransform { ToPcs, ToDevice }
