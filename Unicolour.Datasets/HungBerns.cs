﻿namespace Wacton.Unicolour.Datasets;

using Wacton.Unicolour;

// https://zenodo.org/record/3367463
public static class HungBerns
{
    public static readonly Unicolour Red25 = Unicolour.FromXyz(0.3603, 0.309, 0.2448);
    public static readonly Unicolour Red50 = Unicolour.FromXyz(0.4195, 0.309, 0.1528);
    public static readonly Unicolour Red75 = Unicolour.FromXyz(0.4855, 0.309, 0.0919);
    public static readonly Unicolour RedRef = Unicolour.FromXyz(0.5445, 0.309, 0.0254);
    public static readonly Unicolour RedYellow25 = Unicolour.FromXyz(0.6078, 0.6138, 0.5257);
    public static readonly Unicolour RedYellow50 = Unicolour.FromXyz(0.6303, 0.6138, 0.3573);
    public static readonly Unicolour RedYellow75 = Unicolour.FromXyz(0.6502, 0.6138, 0.2064);
    public static readonly Unicolour RedYellowRef = Unicolour.FromXyz(0.6822, 0.6138, 0.0791);
    public static readonly Unicolour Yellow25 = Unicolour.FromXyz(0.9375, 1.0041, 0.8927);
    public static readonly Unicolour Yellow50 = Unicolour.FromXyz(0.9117, 1.0041, 0.6245);
    public static readonly Unicolour Yellow75 = Unicolour.FromXyz(0.88, 1.0041, 0.3811);
    public static readonly Unicolour YellowRef = Unicolour.FromXyz(0.8615, 1.0041, 0.1492);
    public static readonly Unicolour YellowGreen25 = Unicolour.FromXyz(0.8407, 0.9472, 0.855);
    public static readonly Unicolour YellowGreen50 = Unicolour.FromXyz(0.7772, 0.9472, 0.5931);
    public static readonly Unicolour YellowGreen75 = Unicolour.FromXyz(0.7161, 0.9472, 0.3622);
    public static readonly Unicolour YellowGreenRef = Unicolour.FromXyz(0.6562, 0.9472, 0.1528);
    public static readonly Unicolour Green25 = Unicolour.FromXyz(0.6422, 0.7765, 0.6976);
    public static readonly Unicolour Green50 = Unicolour.FromXyz(0.5454, 0.7765, 0.4658);
    public static readonly Unicolour Green75 = Unicolour.FromXyz(0.4532, 0.7765, 0.277);
    public static readonly Unicolour GreenRef = Unicolour.FromXyz(0.3593, 0.7765, 0.1393);
    public static readonly Unicolour GreenCyan25 = Unicolour.FromXyz(0.7458, 0.8476, 0.9456);
    public static readonly Unicolour GreenCyan50 = Unicolour.FromXyz(0.6621, 0.8476, 0.9261);
    public static readonly Unicolour GreenCyan75 = Unicolour.FromXyz(0.5829, 0.8476, 0.9106);
    public static readonly Unicolour GreenCyanRef = Unicolour.FromXyz(0.5038, 0.8476, 0.8602);
    public static readonly Unicolour Cyan25 = Unicolour.FromXyz(0.6551, 0.7227, 0.9162);
    public static readonly Unicolour Cyan50 = Unicolour.FromXyz(0.6035, 0.7227, 0.9951);
    public static readonly Unicolour Cyan75 = Unicolour.FromXyz(0.5531, 0.7227, 1.0844);
    public static readonly Unicolour CyanRef = Unicolour.FromXyz(0.4955, 0.7227, 1.1491);
    public static readonly Unicolour CyanBlue25 = Unicolour.FromXyz(0.4239, 0.4526, 0.6559);
    public static readonly Unicolour CyanBlue50 = Unicolour.FromXyz(0.3995, 0.4526, 0.7771);
    public static readonly Unicolour CyanBlue75 = Unicolour.FromXyz(0.3785, 0.4526, 0.9185);
    public static readonly Unicolour CyanBlueRef = Unicolour.FromXyz(0.3725, 0.4526, 1.1112);
    public static readonly Unicolour Blue25 = Unicolour.FromXyz(0.1077, 0.1077, 0.2369);
    public static readonly Unicolour Blue50 = Unicolour.FromXyz(0.1153, 0.1077, 0.3905);
    public static readonly Unicolour Blue75 = Unicolour.FromXyz(0.1436, 0.1077, 0.5971);
    public static readonly Unicolour BlueRef = Unicolour.FromXyz(0.2168, 0.1077, 1.0509);
    public static readonly Unicolour BlueMagenta25 = Unicolour.FromXyz(0.2715, 0.233, 0.4033);
    public static readonly Unicolour BlueMagenta50 = Unicolour.FromXyz(0.324, 0.233, 0.5633);
    public static readonly Unicolour BlueMagenta75 = Unicolour.FromXyz(0.3774, 0.233, 0.7725);
    public static readonly Unicolour BlueMagentaRef = Unicolour.FromXyz(0.4479, 0.233, 1.0563);
    public static readonly Unicolour Magenta25 = Unicolour.FromXyz(0.4286, 0.3683, 0.555);
    public static readonly Unicolour Magenta50 = Unicolour.FromXyz(0.5064, 0.3683, 0.6745);
    public static readonly Unicolour Magenta75 = Unicolour.FromXyz(0.5935, 0.3683, 0.8176);
    public static readonly Unicolour MagentaRed25 = Unicolour.FromXyz(0.4174, 0.3553, 0.4524);
    public static readonly Unicolour MagentaRef = Unicolour.FromXyz(0.6873, 0.3683, 1.0685);
    public static readonly Unicolour MagentaRed50 = Unicolour.FromXyz(0.4923, 0.3553, 0.5121);
    public static readonly Unicolour MagentaRed75 = Unicolour.FromXyz(0.5677, 0.3553, 0.5313);
    public static readonly Unicolour MagentaRedRef = Unicolour.FromXyz(0.6436, 0.3553, 0.5366);

    public static readonly List<Unicolour> AllRed = new() { Red25, Red50, Red75, RedRef };
    public static readonly List<Unicolour> AllRedYellow = new() { RedYellow25, RedYellow50, RedYellow75, RedYellowRef };
    public static readonly List<Unicolour> AllYellow = new() { Yellow25, Yellow50, Yellow75, YellowRef };
    public static readonly List<Unicolour> AllYellowGreen = new() { YellowGreen25, YellowGreen50, YellowGreen75, YellowGreenRef };
    public static readonly List<Unicolour> AllGreen = new() { Green25, Green50, Green75, GreenRef };
    public static readonly List<Unicolour> AllGreenCyan = new() { GreenCyan25, GreenCyan50, GreenCyan75, GreenCyanRef };
    public static readonly List<Unicolour> AllCyan = new() { Cyan25, Cyan50, Cyan75, CyanRef };
    public static readonly List<Unicolour> AllCyanBlue = new() { CyanBlue25, CyanBlue50, CyanBlue75, CyanBlueRef };
    public static readonly List<Unicolour> AllBlue = new() { Blue25, Blue50, Blue75, BlueRef };
    public static readonly List<Unicolour> AllBlueMagenta = new() { BlueMagenta25, BlueMagenta50, BlueMagenta75, BlueMagentaRef };
    public static readonly List<Unicolour> AllMagenta = new() { Magenta25, Magenta50, Magenta75, MagentaRef };
    public static readonly List<Unicolour> AllMagentaRed = new() { MagentaRed25, MagentaRed50, MagentaRed75, MagentaRedRef };
    public static readonly List<Unicolour> All25 = new() { Red25, RedYellow25, Yellow25, YellowGreen25, Green25, GreenCyan25, Cyan25, CyanBlue25, Blue25, BlueMagenta25, Magenta25, MagentaRed25 };
    public static readonly List<Unicolour> All50 = new() { Red50, RedYellow50, Yellow50, YellowGreen50, Green50, GreenCyan50, Cyan50, CyanBlue50, Blue50, BlueMagenta50, Magenta50, MagentaRed50 };
    public static readonly List<Unicolour> All75 = new() { Red75, RedYellow75, Yellow75, YellowGreen75, Green75, GreenCyan75, Cyan75, CyanBlue75, Blue75, BlueMagenta75, Magenta75, MagentaRed75 };
    public static readonly List<Unicolour> AllRef = new() { RedRef, RedYellowRef, YellowRef, YellowGreenRef, GreenRef, GreenCyanRef, CyanRef, CyanBlueRef, BlueRef, BlueMagentaRef, MagentaRef, MagentaRedRef };
}