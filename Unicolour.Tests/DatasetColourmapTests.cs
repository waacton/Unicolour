namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;

public class DatasetColourmapTests
{
    private static List<Colourmap> allColourmaps = new()
    {
        Colourmaps.Viridis, Colourmaps.Plasma, Colourmaps.Inferno, Colourmaps.Magma,
        Colourmaps.Cividis,
        Colourmaps.Mako, Colourmaps.Rocket, Colourmaps.Crest, Colourmaps.Flare,
        Colourmaps.Vlag, Colourmaps.Icefire,
        Colourmaps.Twilight, Colourmaps.TwilightShifted,
        Colourmaps.Turbo,
        Colourmaps.Cubehelix
    };
    
    private static readonly Dictionary<Colourmap, Unicolour[]> Lookups = new()
    {
        { Colourmaps.Viridis, Viridis.Lookup },
        { Colourmaps.Plasma, Plasma.Lookup },
        { Colourmaps.Inferno, Inferno.Lookup },
        { Colourmaps.Magma, Magma.Lookup },
        { Colourmaps.Cividis, Cividis.Lookup },
        { Colourmaps.Mako, Mako.Lookup },
        { Colourmaps.Rocket, Rocket.Lookup },
        { Colourmaps.Crest, Crest.Lookup },
        { Colourmaps.Flare, Flare.Lookup },
        { Colourmaps.Vlag, Vlag.Lookup },
        { Colourmaps.Icefire, Icefire.Lookup },
        { Colourmaps.Twilight, Twilight.Lookup },
        { Colourmaps.TwilightShifted, TwilightShifted.Lookup },
        { Colourmaps.Turbo, Turbo.Lookup }
        // Cubehelix is based on a function, not a lookup table
    };
    
    private static readonly List<TestCaseData> LookupCountTestData = new()
    {
        new TestCaseData(Turbo.Lookup, 256).SetName(nameof(Turbo)),
        new TestCaseData(Viridis.Lookup, 256).SetName(nameof(Viridis)),
        new TestCaseData(Plasma.Lookup, 256).SetName(nameof(Plasma)),
        new TestCaseData(Inferno.Lookup, 256).SetName(nameof(Inferno)),
        new TestCaseData(Magma.Lookup, 256).SetName(nameof(Magma)),
        new TestCaseData(Cividis.Lookup, 255).SetName(nameof(Cividis)),
        new TestCaseData(Mako.Lookup, 256).SetName(nameof(Mako)),
        new TestCaseData(Rocket.Lookup, 256).SetName(nameof(Rocket)),
        new TestCaseData(Crest.Lookup, 256).SetName(nameof(Crest)),
        new TestCaseData(Flare.Lookup, 256).SetName(nameof(Flare)),
        new TestCaseData(Vlag.Lookup, 256).SetName(nameof(Vlag)),
        new TestCaseData(Icefire.Lookup, 256).SetName(nameof(Icefire)),
        new TestCaseData(Twilight.Lookup, 510).SetName(nameof(Twilight)),
        new TestCaseData(TwilightShifted.Lookup, 510).SetName(nameof(TwilightShifted))
    };
    
    private static readonly List<TestCaseData> IndexTestData = new()
    {
        new TestCaseData(Colourmaps.Turbo, 0, 0.18995, 0.07176, 0.23217).SetName($"{nameof(Turbo)}[0]"),
        new TestCaseData(Colourmaps.Turbo, 1, 0.19483, 0.08339, 0.26149).SetName($"{nameof(Turbo)}[1]"),
        new TestCaseData(Colourmaps.Turbo, 127, 0.63323, 0.99195, 0.23937).SetName($"{nameof(Turbo)}[127]"),
        new TestCaseData(Colourmaps.Turbo, 128, 0.64362, 0.98999, 0.23356).SetName($"{nameof(Turbo)}[128]"),
        new TestCaseData(Colourmaps.Turbo, 254, 0.49321, 0.01963, 0.00955).SetName($"{nameof(Turbo)}[254]"),
        new TestCaseData(Colourmaps.Turbo, 255, 0.4796, 0.01583, 0.01055).SetName($"{nameof(Turbo)}[255]"),
        
        new TestCaseData(Colourmaps.Cividis, 0, 0, 0.138068, 0.311105).SetName($"{nameof(Cividis)}[0]"),
        new TestCaseData(Colourmaps.Cividis, 1, 0, 0.141013, 0.317579).SetName($"{nameof(Cividis)}[1]"),
        new TestCaseData(Colourmaps.Cividis, 127, 0.488697, 0.485318, 0.471008).SetName($"{nameof(Cividis)}[127]"),
        new TestCaseData(Colourmaps.Cividis, 253, 0.995503, 0.903866, 0.21237).SetName($"{nameof(Cividis)}[253]"),
        new TestCaseData(Colourmaps.Cividis, 254, 0.995737, 0.909344, 0.217772).SetName($"{nameof(Cividis)}[254]"),
        
        new TestCaseData(Colourmaps.Crest, 0, 0.6468274, 0.80289262, 0.56592265).SetName($"{nameof(Crest)}[0]"),
        new TestCaseData(Colourmaps.Crest, 1, 0.64233318, 0.80081141, 0.56639461).SetName($"{nameof(Crest)}[1]"),
        new TestCaseData(Colourmaps.Crest, 127, 0.20350004, 0.5231837, 0.55370601).SetName($"{nameof(Crest)}[127]"),
        new TestCaseData(Colourmaps.Crest, 128, 0.20094292, 0.52087429, 0.55342884).SetName($"{nameof(Crest)}[128]"),
        new TestCaseData(Colourmaps.Crest, 254, 0.17313804, 0.19389201, 0.44672488).SetName($"{nameof(Crest)}[254]"),
        new TestCaseData(Colourmaps.Crest, 255, 0.17363177, 0.19076859, 0.44549087).SetName($"{nameof(Crest)}[255]"),

        new TestCaseData(Colourmaps.Flare, 0, 0.92907237, 0.68878959, 0.50411509).SetName($"{nameof(Flare)}[0]"),
        new TestCaseData(Colourmaps.Flare, 1, 0.92891402, 0.68494686, 0.50173994).SetName($"{nameof(Flare)}[1]"),
        new TestCaseData(Colourmaps.Flare, 127, 0.76214598, 0.25492998, 0.40539471).SetName($"{nameof(Flare)}[127]"),
        new TestCaseData(Colourmaps.Flare, 128, 0.75861834, 0.25356035, 0.40663694).SetName($"{nameof(Flare)}[128]"),
        new TestCaseData(Colourmaps.Flare, 254, 0.2975886, 0.1383403, 0.38552159).SetName($"{nameof(Flare)}[254]"),
        new TestCaseData(Colourmaps.Flare, 255, 0.29408557, 0.13721193, 0.38442775).SetName($"{nameof(Flare)}[255]"),
        
        new TestCaseData(Colourmaps.Icefire, 0, 0.73936227, 0.90443867, 0.85757238).SetName($"{nameof(Icefire)}[0]"),
        new TestCaseData(Colourmaps.Icefire, 1, 0.72888063, 0.89639109, 0.85488394).SetName($"{nameof(Icefire)}[1]"),
        new TestCaseData(Colourmaps.Icefire, 127, 0.12127766, 0.11828837, 0.11878534).SetName($"{nameof(Icefire)}[127]"),
        new TestCaseData(Colourmaps.Icefire, 128, 0.12284806, 0.1179729, 0.11772022).SetName($"{nameof(Icefire)}[128]"),
        new TestCaseData(Colourmaps.Icefire, 254, 0.99740544, 0.82008078, 0.66150571).SetName($"{nameof(Icefire)}[254]"),
        new TestCaseData(Colourmaps.Icefire, 255, 0.9992197, 0.83100723, 0.6764127).SetName($"{nameof(Icefire)}[255]"),

        new TestCaseData(Colourmaps.Inferno, 0, 0.001462, 0.000466, 0.013866).SetName($"{nameof(Inferno)}[0]"),
        new TestCaseData(Colourmaps.Inferno, 1, 0.002267, 0.00127, 0.01857).SetName($"{nameof(Inferno)}[1]"),
        new TestCaseData(Colourmaps.Inferno, 127, 0.729909, 0.212759, 0.333861).SetName($"{nameof(Inferno)}[127]"),
        new TestCaseData(Colourmaps.Inferno, 128, 0.735683, 0.215906, 0.330245).SetName($"{nameof(Inferno)}[128]"),
        new TestCaseData(Colourmaps.Inferno, 254, 0.982257, 0.994109, 0.631017).SetName($"{nameof(Inferno)}[254]"),
        new TestCaseData(Colourmaps.Inferno, 255, 0.988362, 0.998364, 0.644924).SetName($"{nameof(Inferno)}[255]"),

        new TestCaseData(Colourmaps.Magma, 0, 0.001462, 0.000466, 0.013866).SetName($"{nameof(Magma)}[0]"),
        new TestCaseData(Colourmaps.Magma, 1, 0.002258, 0.001295, 0.018331).SetName($"{nameof(Magma)}[1]"),
        new TestCaseData(Colourmaps.Magma, 127, 0.709962, 0.212797, 0.477201).SetName($"{nameof(Magma)}[127]"),
        new TestCaseData(Colourmaps.Magma, 128, 0.716387, 0.214982, 0.47529).SetName($"{nameof(Magma)}[128]"),
        new TestCaseData(Colourmaps.Magma, 254, 0.987387, 0.984288, 0.742002).SetName($"{nameof(Magma)}[254]"),
        new TestCaseData(Colourmaps.Magma, 255, 0.987053, 0.991438, 0.749504).SetName($"{nameof(Magma)}[255]"),
        
        new TestCaseData(Colourmaps.Mako, 0, 0.04503935, 0.01482344, 0.02092227).SetName($"{nameof(Mako)}[0]"),
        new TestCaseData(Colourmaps.Mako, 1, 0.04933018, 0.01709292, 0.02535719).SetName($"{nameof(Mako)}[1]"),
        new TestCaseData(Colourmaps.Mako, 127, 0.20722876, 0.47763224, 0.63711608).SetName($"{nameof(Mako)}[127]"),
        new TestCaseData(Colourmaps.Mako, 128, 0.20692679, 0.48201774, 0.63812656).SetName($"{nameof(Mako)}[128]"),
        new TestCaseData(Colourmaps.Mako, 254, 0.86429066, 0.95635719, 0.89067759).SetName($"{nameof(Mako)}[254]"),
        new TestCaseData(Colourmaps.Mako, 255, 0.87218969, 0.95960708, 0.89725384).SetName($"{nameof(Mako)}[255]"),
        
        new TestCaseData(Colourmaps.Plasma, 0, 0.050383, 0.029803, 0.527975).SetName($"{nameof(Plasma)}[0]"),
        new TestCaseData(Colourmaps.Plasma, 1, 0.063536, 0.028426, 0.533124).SetName($"{nameof(Plasma)}[1]"),
        new TestCaseData(Colourmaps.Plasma, 127, 0.794549, 0.27577, 0.473117).SetName($"{nameof(Plasma)}[127]"),
        new TestCaseData(Colourmaps.Plasma, 128, 0.798216, 0.280197, 0.469538).SetName($"{nameof(Plasma)}[128]"),
        new TestCaseData(Colourmaps.Plasma, 254, 0.941896, 0.96859, 0.140956).SetName($"{nameof(Plasma)}[254]"),
        new TestCaseData(Colourmaps.Plasma, 255, 0.940015, 0.975158, 0.131326).SetName($"{nameof(Plasma)}[255]"),
        
        new TestCaseData(Colourmaps.Rocket, 0, 0.01060815, 0.01808215, 0.10018654).SetName($"{nameof(Rocket)}[0]"),
        new TestCaseData(Colourmaps.Rocket, 1, 0.01428972, 0.02048237, 0.10374486).SetName($"{nameof(Rocket)}[1]"),
        new TestCaseData(Colourmaps.Rocket, 127, 0.79085854, 0.10184672, 0.313391).SetName($"{nameof(Rocket)}[127]"),
        new TestCaseData(Colourmaps.Rocket, 128, 0.7965014, 0.10506637, 0.31063031).SetName($"{nameof(Rocket)}[128]"),
        new TestCaseData(Colourmaps.Rocket, 254, 0.98067764, 0.91476465, 0.85676611).SetName($"{nameof(Rocket)}[254]"),
        new TestCaseData(Colourmaps.Rocket, 255, 0.98137749, 0.92061729, 0.86536915).SetName($"{nameof(Rocket)}[255]"),

        new TestCaseData(Colourmaps.Turbo, 0, 0.18995, 0.07176, 0.23217).SetName($"{nameof(Turbo)}[0]"),
        new TestCaseData(Colourmaps.Turbo, 1, 0.19483, 0.08339, 0.26149).SetName($"{nameof(Turbo)}[1]"),
        new TestCaseData(Colourmaps.Turbo, 127, 0.63323, 0.99195, 0.23937).SetName($"{nameof(Turbo)}[127]"),
        new TestCaseData(Colourmaps.Turbo, 128, 0.64362, 0.98999, 0.23356).SetName($"{nameof(Turbo)}[128]"),
        new TestCaseData(Colourmaps.Turbo, 254, 0.49321, 0.01963, 0.00955).SetName($"{nameof(Turbo)}[254]"),
        new TestCaseData(Colourmaps.Turbo, 255, 0.4796, 0.01583, 0.01055).SetName($"{nameof(Turbo)}[255]"),
        
        new TestCaseData(Colourmaps.Twilight, 0, 0.885750158407544, 0.850009249430678, 0.887973650642719).SetName($"{nameof(Twilight)}[0]"),
        new TestCaseData(Colourmaps.Twilight, 1, 0.885750158407544, 0.850009249430678, 0.887973650642719).SetName($"{nameof(Twilight)}[1]"),
        new TestCaseData(Colourmaps.Twilight, 255, 0.187392283426976, 0.0771020968995883, 0.216188753763095).SetName($"{nameof(Twilight)}[255]"),
        new TestCaseData(Colourmaps.Twilight, 509, 0.885547148119523, 0.849871742836315, 0.883362061211709).SetName($"{nameof(Twilight)}[509]"),
        new TestCaseData(Colourmaps.Twilight, 510, 0.885711551228456, 0.850021861158563, 0.885725389900871).SetName($"{nameof(Twilight)}[510]"),
        
        new TestCaseData(Colourmaps.TwilightShifted, 0, 0.189758536390946, 0.0750198618621437, 0.219300507565299).SetName($"{nameof(TwilightShifted)}[0]"),
        new TestCaseData(Colourmaps.TwilightShifted, 1, 0.191994491846062, 0.0731828306492733, 0.222433852434336).SetName($"{nameof(TwilightShifted)}[1]"),
        new TestCaseData(Colourmaps.TwilightShifted, 254, 0.885750158407544, 0.850009249430678, 0.887973650642719).SetName($"{nameof(TwilightShifted)}[254]"),
        new TestCaseData(Colourmaps.TwilightShifted, 255, 0.885711551228456, 0.850021861158563, 0.885725389900871).SetName($"{nameof(TwilightShifted)}[255]"),
        new TestCaseData(Colourmaps.TwilightShifted, 256, 0.885547148119523, 0.849871742836315, 0.883362061211709).SetName($"{nameof(TwilightShifted)}[256]"),
        new TestCaseData(Colourmaps.TwilightShifted, 509, 0.184880355093961, 0.0794257302797238, 0.213076516489849).SetName($"{nameof(TwilightShifted)}[509]"),
        new TestCaseData(Colourmaps.TwilightShifted, 510, 0.187392283426976, 0.0771020968995883, 0.216188753763095).SetName($"{nameof(TwilightShifted)}[510]"),
        
        new TestCaseData(Colourmaps.Viridis, 0, 0.267004, 0.004874, 0.329415).SetName($"{nameof(Viridis)}[0]"),
        new TestCaseData(Colourmaps.Viridis, 1, 0.26851, 0.009605, 0.335427).SetName($"{nameof(Viridis)}[1]"),
        new TestCaseData(Colourmaps.Viridis, 127, 0.128729, 0.563265, 0.551229).SetName($"{nameof(Viridis)}[127]"),
        new TestCaseData(Colourmaps.Viridis, 128, 0.127568, 0.566949, 0.550556).SetName($"{nameof(Viridis)}[128]"),
        new TestCaseData(Colourmaps.Viridis, 254, 0.983868, 0.904867, 0.136897).SetName($"{nameof(Viridis)}[254]"),
        new TestCaseData(Colourmaps.Viridis, 255, 0.993248, 0.906157, 0.143936).SetName($"{nameof(Viridis)}[255]"),
        
        new TestCaseData(Colourmaps.Vlag, 0, 0.13850039, 0.41331206, 0.74052025).SetName($"{nameof(Vlag)}[0]"),
        new TestCaseData(Colourmaps.Vlag, 1, 0.15077609, 0.41762684, 0.73970427).SetName($"{nameof(Vlag)}[1]"),
        new TestCaseData(Colourmaps.Vlag, 127, 0.97978484, 0.96222949, 0.95935496).SetName($"{nameof(Vlag)}[127]"),
        new TestCaseData(Colourmaps.Vlag, 128, 0.9805997, 0.96155216, 0.95813083).SetName($"{nameof(Vlag)}[128]"),
        new TestCaseData(Colourmaps.Vlag, 254, 0.6638441, 0.22201742, 0.2359961).SetName($"{nameof(Vlag)}[254]"),
        new TestCaseData(Colourmaps.Vlag, 255, 0.66080672, 0.21526712, 0.23069468).SetName($"{nameof(Vlag)}[255]"),
    };
    
    private static readonly List<TestCaseData> InterpolatedTestData = new()
    {
        new TestCaseData(Colourmaps.Cividis, -0.5, 0, 0.138068, 0.311105).SetName($"{nameof(Cividis)}[-0.5]"),
        new TestCaseData(Colourmaps.Cividis, 0.25, 0, 0.13880425, 0.3127235).SetName($"{nameof(Cividis)}[0.25]"),
        new TestCaseData(Colourmaps.Cividis, 0.5, 0, 0.1395405, 0.314342).SetName($"{nameof(Cividis)}[0.5]"),
        new TestCaseData(Colourmaps.Cividis, 126.5, 0.486919, 0.4838845, 0.470696).SetName($"{nameof(Cividis)}[126.5]"),
        new TestCaseData(Colourmaps.Cividis, 127.5, 0.4904875, 0.486758, 0.4712305).SetName($"{nameof(Cividis)}[127.5]"),
        new TestCaseData(Colourmaps.Cividis, 253.5, 0.99562, 0.906605, 0.215071).SetName($"{nameof(Cividis)}[254.5]"),
        new TestCaseData(Colourmaps.Cividis, 253.75, 0.9956785, 0.9079745, 0.2164215).SetName($"{nameof(Cividis)}[254.75]"),
        new TestCaseData(Colourmaps.Cividis, 254.5, 0.995737, 0.909344, 0.217772).SetName($"{nameof(Cividis)}[255.5]"),
        
        new TestCaseData(Colourmaps.Crest, -0.5, 0.6468274, 0.80289262, 0.56592265).SetName($"{nameof(Crest)}[-0.5]"),
        new TestCaseData(Colourmaps.Crest, 0.25, 0.645703845, 0.8023723175, 0.56604064).SetName($"{nameof(Crest)}[0.25]"),
        new TestCaseData(Colourmaps.Crest, 0.5, 0.64458029, 0.801852015, 0.56615863).SetName($"{nameof(Crest)}[0.5]"),
        new TestCaseData(Colourmaps.Crest, 127.5, 0.20222148, 0.522028995, 0.553567425).SetName($"{nameof(Crest)}[127.5]"),
        new TestCaseData(Colourmaps.Crest, 254.5, 0.173384905, 0.1923303, 0.446107875).SetName($"{nameof(Crest)}[254.5]"),
        new TestCaseData(Colourmaps.Crest, 254.75, 0.1735083375, 0.191549445, 0.4457993725).SetName($"{nameof(Crest)}[254.75]"),
        new TestCaseData(Colourmaps.Crest, 255.5, 0.17363177, 0.19076859, 0.44549087).SetName($"{nameof(Crest)}[255.5]"),

        new TestCaseData(Colourmaps.Flare, -0.5, 0.92907237, 0.68878959, 0.50411509).SetName($"{nameof(Flare)}[-0.5]"),
        new TestCaseData(Colourmaps.Flare, 0.25, 0.9290327825, 0.6878289075, 0.5035213025).SetName($"{nameof(Flare)}[0.25]"),
        new TestCaseData(Colourmaps.Flare, 0.5, 0.928993195, 0.686868225, 0.502927515).SetName($"{nameof(Flare)}[0.5]"),
        new TestCaseData(Colourmaps.Flare, 127.5, 0.76038216, 0.254245165, 0.406015825).SetName($"{nameof(Flare)}[127.5]"),
        new TestCaseData(Colourmaps.Flare, 254.5, 0.295837085, 0.137776115, 0.38497467).SetName($"{nameof(Flare)}[254.5]"),
        new TestCaseData(Colourmaps.Flare, 254.75, 0.2949613275, 0.1374940225, 0.38470121).SetName($"{nameof(Flare)}[254.75]"),
        new TestCaseData(Colourmaps.Flare, 255.5, 0.29408557, 0.13721193, 0.38442775).SetName($"{nameof(Flare)}[255.5]"),

        new TestCaseData(Colourmaps.Icefire, -0.5, 0.73936227, 0.90443867, 0.85757238).SetName($"{nameof(Icefire)}[-0.5]"),
        new TestCaseData(Colourmaps.Icefire, 0.25, 0.73674186, 0.902426775, 0.85690027).SetName($"{nameof(Icefire)}[0.25]"),
        new TestCaseData(Colourmaps.Icefire, 0.5, 0.73412145, 0.90041488, 0.85622816).SetName($"{nameof(Icefire)}[0.5]"),
        new TestCaseData(Colourmaps.Icefire, 127.5, 0.12206286, 0.118130635, 0.11825278).SetName($"{nameof(Icefire)}[127.5]"),
        new TestCaseData(Colourmaps.Icefire, 254.5, 0.99831257, 0.825544005, 0.668959205).SetName($"{nameof(Icefire)}[254.5]"),
        new TestCaseData(Colourmaps.Icefire, 254.75, 0.998766135, 0.8282756175, 0.6726859525).SetName($"{nameof(Icefire)}[254.75]"),
        new TestCaseData(Colourmaps.Icefire, 255.5, 0.9992197, 0.83100723, 0.6764127).SetName($"{nameof(Icefire)}[255.5]"),
        
        new TestCaseData(Colourmaps.Inferno, -0.5, 0.001462, 0.000466, 0.013866).SetName($"{nameof(Inferno)}[-0.5]"),
        new TestCaseData(Colourmaps.Inferno, 0.25, 0.00166325, 0.000667, 0.015042).SetName($"{nameof(Inferno)}[0.25]"),
        new TestCaseData(Colourmaps.Inferno, 0.5, 0.0018645, 0.000868, 0.016218).SetName($"{nameof(Inferno)}[0.5]"),
        new TestCaseData(Colourmaps.Inferno, 127.5, 0.732796, 0.2143325, 0.332053).SetName($"{nameof(Inferno)}[127.5]"),
        new TestCaseData(Colourmaps.Inferno, 254.5, 0.9853095, 0.9962365, 0.6379705).SetName($"{nameof(Inferno)}[254.5]"),
        new TestCaseData(Colourmaps.Inferno, 254.75, 0.98683575, 0.99730025, 0.64144725).SetName($"{nameof(Inferno)}[254.75]"),
        new TestCaseData(Colourmaps.Inferno, 255.5, 0.988362, 0.998364, 0.644924).SetName($"{nameof(Inferno)}[255.5]"),
        
        new TestCaseData(Colourmaps.Magma, -0.5, 0.001462, 0.000466, 0.013866).SetName($"{nameof(Magma)}[-0.5]"),
        new TestCaseData(Colourmaps.Magma, 0.25, 0.001661, 0.00067325, 0.01498225).SetName($"{nameof(Magma)}[0.25]"),
        new TestCaseData(Colourmaps.Magma, 0.5, 0.00186, 0.0008805, 0.0160985).SetName($"{nameof(Magma)}[0.5]"),
        new TestCaseData(Colourmaps.Magma, 127.5, 0.7131745, 0.2138895, 0.4762455).SetName($"{nameof(Magma)}[127.5]"),
        new TestCaseData(Colourmaps.Magma, 254.5, 0.98722, 0.987863, 0.745753).SetName($"{nameof(Magma)}[254.5]"),
        new TestCaseData(Colourmaps.Magma, 254.75, 0.9871365, 0.9896505, 0.7476285).SetName($"{nameof(Magma)}[254.75]"),
        new TestCaseData(Colourmaps.Magma, 255.5, 0.987053, 0.991438, 0.749504).SetName($"{nameof(Magma)}[255.5]"),
        
        new TestCaseData(Colourmaps.Mako, -0.5, 0.04503935, 0.01482344, 0.02092227).SetName($"{nameof(Mako)}[-0.5]"),
        new TestCaseData(Colourmaps.Mako, 0.25, 0.0461120575, 0.01539081, 0.022031).SetName($"{nameof(Mako)}[0.25]"),
        new TestCaseData(Colourmaps.Mako, 0.5, 0.047184765, 0.01595818, 0.02313973).SetName($"{nameof(Mako)}[0.5]"),
        new TestCaseData(Colourmaps.Mako, 127.5, 0.207077775, 0.47982499, 0.63762132).SetName($"{nameof(Mako)}[127.5]"),
        new TestCaseData(Colourmaps.Mako, 254.5, 0.868240175, 0.957982135, 0.893965715).SetName($"{nameof(Mako)}[254.5]"),
        new TestCaseData(Colourmaps.Mako, 254.75, 0.8702149325, 0.9587946075, 0.8956097775).SetName($"{nameof(Mako)}[254.75]"),
        new TestCaseData(Colourmaps.Mako, 255.5, 0.87218969, 0.95960708, 0.89725384).SetName($"{nameof(Mako)}[255.5]"),
        
        new TestCaseData(Colourmaps.Plasma, -0.5, 0.050383, 0.029803, 0.527975).SetName($"{nameof(Plasma)}[-0.5]"),
        new TestCaseData(Colourmaps.Plasma, 0.25, 0.05367125, 0.02945875, 0.52926225).SetName($"{nameof(Plasma)}[0.25]"),
        new TestCaseData(Colourmaps.Plasma, 0.5, 0.0569595, 0.0291145, 0.5305495).SetName($"{nameof(Plasma)}[0.5]"),
        new TestCaseData(Colourmaps.Plasma, 127.5, 0.7963825, 0.2779835, 0.4713275).SetName($"{nameof(Plasma)}[127.5]"),
        new TestCaseData(Colourmaps.Plasma, 254.5, 0.9409555, 0.971874, 0.136141).SetName($"{nameof(Plasma)}[254.5]"),
        new TestCaseData(Colourmaps.Plasma, 254.75, 0.94048525, 0.973516, 0.1337335).SetName($"{nameof(Plasma)}[254.75]"),
        new TestCaseData(Colourmaps.Plasma, 255.5, 0.940015, 0.975158, 0.131326).SetName($"{nameof(Plasma)}[255.5]"),
        
        new TestCaseData(Colourmaps.Rocket, -0.5, 0.01060815, 0.01808215, 0.10018654).SetName($"{nameof(Rocket)}[-0.5]"),
        new TestCaseData(Colourmaps.Rocket, 0.25, 0.0115285425, 0.018682205, 0.10107612).SetName($"{nameof(Rocket)}[0.25]"),
        new TestCaseData(Colourmaps.Rocket, 0.5, 0.012448935, 0.01928226, 0.1019657).SetName($"{nameof(Rocket)}[0.5]"),
        new TestCaseData(Colourmaps.Rocket, 127.5, 0.79367997, 0.103456545, 0.312010655).SetName($"{nameof(Rocket)}[127.5]"),
        new TestCaseData(Colourmaps.Rocket, 254.5, 0.981027565, 0.91769097, 0.86106763).SetName($"{nameof(Rocket)}[254.5]"),
        new TestCaseData(Colourmaps.Rocket, 254.75, 0.9812025275, 0.91915413, 0.86321839).SetName($"{nameof(Rocket)}[254.75]"),
        new TestCaseData(Colourmaps.Rocket, 255.5, 0.98137749, 0.92061729, 0.86536915).SetName($"{nameof(Rocket)}[255.5]"),

        new TestCaseData(Colourmaps.Turbo, -0.5, 0.18995, 0.07176, 0.23217).SetName($"{nameof(Turbo)}[-0.5]"),
        new TestCaseData(Colourmaps.Turbo, 0.25, 0.19117, 0.0746675, 0.2395).SetName($"{nameof(Turbo)}[0.25]"),
        new TestCaseData(Colourmaps.Turbo, 0.5, 0.19239, 0.077575, 0.24683).SetName($"{nameof(Turbo)}[0.5]"),
        new TestCaseData(Colourmaps.Turbo, 127.5, 0.638425, 0.99097, 0.236465).SetName($"{nameof(Turbo)}[127.5]"),
        new TestCaseData(Colourmaps.Turbo, 254.5, 0.486405, 0.01773, 0.01005).SetName($"{nameof(Turbo)}[254.5]"),
        new TestCaseData(Colourmaps.Turbo, 254.75, 0.4830025, 0.01678, 0.0103).SetName($"{nameof(Turbo)}[254.75]"),
        new TestCaseData(Colourmaps.Turbo, 255.5, 0.4796, 0.01583, 0.01055).SetName($"{nameof(Turbo)}[255.5]"),
        
        new TestCaseData(Colourmaps.Twilight, -0.5, 0.885750158407544, 0.850009249430678, 0.887973650642719).SetName($"{nameof(Twilight)}[-0.5]"),
        new TestCaseData(Colourmaps.Twilight, 0.25, 0.885750158407544, 0.850009249430678, 0.887973650642719).SetName($"{nameof(Twilight)}[0.25]"),
        new TestCaseData(Colourmaps.Twilight, 0.5, 0.885750158407544, 0.850009249430678, 0.887973650642719).SetName($"{nameof(Twilight)}[0.5]"),
        new TestCaseData(Colourmaps.Twilight, 254.5, 0.188575409908961, 0.076060979380866, 0.217744630664197).SetName($"{nameof(Twilight)}[254.5]"),
        new TestCaseData(Colourmaps.Twilight, 255.5, 0.186136319260469, 0.0782639135896561, 0.214632635126472).SetName($"{nameof(Twilight)}[255.5]"),
        new TestCaseData(Colourmaps.Twilight, 509.5, 0.88562934967399, 0.849946801997439, 0.88454372555629).SetName($"{nameof(Twilight)}[509.5]"),
        new TestCaseData(Colourmaps.Twilight, 509.75, 0.885670450451223, 0.849984331578001, 0.885134557728581).SetName($"{nameof(Twilight)}[509.75]"),
        new TestCaseData(Colourmaps.Twilight, 510.5, 0.885711551228456, 0.850021861158563, 0.885725389900871).SetName($"{nameof(Twilight)}[510.5]"),
        
        new TestCaseData(Colourmaps.TwilightShifted, -0.5, 0.189758536390946, 0.0750198618621437, 0.219300507565299).SetName($"{nameof(TwilightShifted)}[-0.5]"),
        new TestCaseData(Colourmaps.TwilightShifted, 0.25, 0.190317525254725, 0.0745606040589261, 0.220083843782558).SetName($"{nameof(TwilightShifted)}[0.25]"),
        new TestCaseData(Colourmaps.TwilightShifted, 0.5, 0.190876514118504, 0.0741013462557085, 0.220867179999818).SetName($"{nameof(TwilightShifted)}[0.5]"),
        new TestCaseData(Colourmaps.TwilightShifted, 254.5, 0.885730854818, 0.85001555529462, 0.886849520271795).SetName($"{nameof(TwilightShifted)}[254.5]"),
        new TestCaseData(Colourmaps.TwilightShifted, 255.5, 0.88562934967399, 0.849946801997439, 0.88454372555629).SetName($"{nameof(TwilightShifted)}[255.5]"),
        new TestCaseData(Colourmaps.TwilightShifted, 509.5, 0.186136319260469, 0.0782639135896561, 0.214632635126472).SetName($"{nameof(TwilightShifted)}[509.5]"),
        new TestCaseData(Colourmaps.TwilightShifted, 509.75, 0.186764301343722, 0.0776830052446222, 0.215410694444784).SetName($"{nameof(TwilightShifted)}[509.75]"),
        new TestCaseData(Colourmaps.TwilightShifted, 510.5, 0.187392283426976, 0.0771020968995883, 0.216188753763095).SetName($"{nameof(TwilightShifted)}[510.5]"),
        
        new TestCaseData(Colourmaps.Viridis, -0.5, 0.267004, 0.004874, 0.329415).SetName($"{nameof(Viridis)}[-0.5]"),
        new TestCaseData(Colourmaps.Viridis, 0.25, 0.2673805, 0.00605675, 0.330918).SetName($"{nameof(Viridis)}[0.25]"),
        new TestCaseData(Colourmaps.Viridis, 0.5, 0.267757, 0.0072395, 0.332421).SetName($"{nameof(Viridis)}[0.5]"),
        new TestCaseData(Colourmaps.Viridis, 127.5, 0.1281485, 0.565107, 0.5508925).SetName($"{nameof(Viridis)}[127.5]"),
        new TestCaseData(Colourmaps.Viridis, 254.5, 0.988558, 0.905512, 0.1404165).SetName($"{nameof(Viridis)}[254.5]"),
        new TestCaseData(Colourmaps.Viridis, 254.75, 0.990903, 0.9058345, 0.14217625).SetName($"{nameof(Viridis)}[254.75]"),
        new TestCaseData(Colourmaps.Viridis, 255.5, 0.993248, 0.906157, 0.143936).SetName($"{nameof(Viridis)}[255.5]"),

        new TestCaseData(Colourmaps.Vlag, -0.5, 0.13850039, 0.41331206, 0.74052025).SetName($"{nameof(Vlag)}[-0.5]"),
        new TestCaseData(Colourmaps.Vlag, 0.25, 0.141569315, 0.414390755, 0.740316255).SetName($"{nameof(Vlag)}[0.25]"),
        new TestCaseData(Colourmaps.Vlag, 0.5, 0.14463824, 0.41546945, 0.74011226).SetName($"{nameof(Vlag)}[0.5]"),
        new TestCaseData(Colourmaps.Vlag, 127.5, 0.98019227, 0.961890825, 0.958742895).SetName($"{nameof(Vlag)}[127.5]"),
        new TestCaseData(Colourmaps.Vlag, 254.5, 0.66232541, 0.21864227, 0.23334539).SetName($"{nameof(Vlag)}[254.5]"),
        new TestCaseData(Colourmaps.Vlag, 254.75, 0.661566065, 0.216954695, 0.232020035).SetName($"{nameof(Vlag)}[254.75]"),
        new TestCaseData(Colourmaps.Vlag, 255.5, 0.66080672, 0.21526712, 0.23069468).SetName($"{nameof(Vlag)}[255.5]"),
    };
    
    [TestCaseSource(nameof(IndexTestData))]
    public void MapToIndex(Colourmap colourmap, int index, double expectedR, double expectedG, double expectedB)
    {
        var lookup = Lookups[colourmap];
        var maxIndex = lookup.Length - 1;
        var distance = index / (double)maxIndex;
        var unicolour = colourmap.Map(distance);
        Assert.That(unicolour, Is.EqualTo(lookup[index]));
        TestUtils.AssertTriplet<Rgb>(unicolour, new ColourTriplet(expectedR, expectedG, expectedB), 0.0);
    }
    

    
    [TestCaseSource(nameof(InterpolatedTestData))]
    public void MapToInterpolated(Colourmap colourmap, double index, double expectedR, double expectedG, double expectedB)
    {
        var maxIndex = Lookups[colourmap].Length - 1;
        var unicolour = colourmap.Map(index / maxIndex);
        TestUtils.AssertTriplet<Rgb>(unicolour, new ColourTriplet(expectedR, expectedG, expectedB), 0.0000000000001);
    }
    
    [Test]
    public void MapBelowMin([ValueSource(nameof(allColourmaps))] Colourmap colourmap)
    {
        var expected = colourmap.Map(0);
        var unicolour = colourmap.Map(-0.000000000000001);
        Assert.That(unicolour, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapAboveMax([ValueSource(nameof(allColourmaps))] Colourmap colourmap)
    {
        var expected = colourmap.Map(1);
        var unicolour = colourmap.Map(1.000000000000001);
        Assert.That(unicolour, Is.EqualTo(expected));
    }
    
    [Test]
    public void MapToClippedLower([ValueSource(nameof(allColourmaps))] Colourmap colourmap)
    {
        var unicolour = colourmap.MapWithClipping(-0.000000000000001);
        Assert.That(unicolour, Is.EqualTo(Colourmap.Black));
    }
    
    [Test]
    public void MapToClippedLowerCustom([ValueSource(nameof(allColourmaps))] Colourmap colourmap)
    {
        var clipColour = new Unicolour(ColourSpace.Rgb, 2, 0, 0);
        var unicolour = colourmap.MapWithClipping(-0.000000000000001, lowerClipColour: clipColour);
        Assert.That(unicolour, Is.EqualTo(clipColour));
    }
    
    [Test]
    public void MapToClippedUpper([ValueSource(nameof(allColourmaps))] Colourmap colourmap)
    {
        var unicolour = colourmap.MapWithClipping(1.000000000000001);
        Assert.That(unicolour, Is.EqualTo(Colourmap.White));
    }
    
    [Test]
    public void MapToClippedUpperCustom([ValueSource(nameof(allColourmaps))] Colourmap colourmap)
    {
        var clipColour = new Unicolour(ColourSpace.Rgb, 0, 0, 2);
        var unicolour = colourmap.MapWithClipping(1.000000000000001, upperClipColour: clipColour);
        Assert.That(unicolour, Is.EqualTo(clipColour));
    }
    
    [Test]
    public void MapToClippedInRange([ValueSource(nameof(allColourmaps))] Colourmap colourmap)
    {
        var clipColour = new Unicolour(ColourSpace.Rgb, 0, 2, 0);
        var unicolour = colourmap.MapWithClipping(0.5, lowerClipColour: clipColour, upperClipColour: clipColour);
        Assert.That(unicolour, Is.Not.EqualTo(clipColour));
    }
    
    [TestCaseSource(nameof(LookupCountTestData))]
    public void Count(Unicolour[] lookup, int expectedCount) => Assert.That(lookup.Distinct().Count(), Is.EqualTo(expectedCount));

    [Test]
    public void CubehelixDefault([Range(0, 999)] int lookupIndex)
    {
        var expected = CubehelixColours.Default[lookupIndex];
        var fraction = lookupIndex / 999.0;
        var actual = Colourmaps.Cubehelix.Map(fraction);
        TestUtils.AssertTriplet<Rgb>(actual, expected, 0.0005);
    }
    
    [Test]
    public void CubehelixCustom([Range(0, 999)] int lookupIndex)
    {
        var expected = CubehelixColours.Custom[lookupIndex];
        var fraction = lookupIndex / 999.0;
        var actual = Cubehelix.Map(fraction, -6.6, 0.6, 1.75, 0.75);
        TestUtils.AssertTriplet<Rgb>(actual, expected, 0.0005);
    }
}