﻿namespace Wacton.Unicolour;

public record Jzczhz : ColourRepresentation
{
    internal override ColourSpace ColourSpace => ColourSpace.Jzczhz;
    protected override int? HueIndex => 2;
    public double J => First;
    public double C => Second;
    public double H => Third;
    public double ConstrainedH => ConstrainedThird;
    protected override double ConstrainedThird => H.Modulo(360.0);
    
    // I'm assuming JCH has the same greyscale behaviour as LCH, i.e. greyscale = no chroma, no lightness, or full lightness
    // (paper says lightness J is 0 - 1 but seems like it's a scaling of their plot of Rec.2020 gamut - in my tests maxes out after ~0.17)
    internal override bool IsGreyscale => C <= 0.0 || J is <= 0.0 or >= 1.0;

    public Jzczhz(double j, double c, double h) : this(j, c, h, ColourMode.Unset) {}
    internal Jzczhz(double j, double c, double h, ColourMode colourMode) : base(j, c, h, colourMode) {}

    protected override string FirstString => $"{J:F3}";
    protected override string SecondString => $"{C:F3}";
    protected override string ThirdString => IsEffectivelyHued ? $"{H:F1}°" : "—°";
    public override string ToString() => base.ToString();
}