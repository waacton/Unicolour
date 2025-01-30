using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;
using static Wacton.Unicolour.Datasets.Css;

namespace Wacton.Unicolour.Tests;

public class DatasetCssTests
{
    // https://www.w3.org/TR/css-color-4/#named-colors
    private static readonly Dictionary<string, ColourTriplet> Rgb255Lookup = new()
    {
        { "aliceblue", new(240, 248, 255) },
        { "antiquewhite", new(250, 235, 215) },
        { "aqua", new(0, 255, 255) },
        { "aquamarine", new(127, 255, 212) },
        { "azure", new(240, 255, 255) },
        { "beige", new(245, 245, 220) },
        { "bisque", new(255, 228, 196) },
        { "black", new(0, 0, 0) },
        { "blanchedalmond", new(255, 235, 205) },
        { "blue", new(0, 0, 255) },
        { "blueviolet", new(138, 43, 226) },
        { "brown", new(165, 42, 42) },
        { "burlywood", new(222, 184, 135) },
        { "cadetblue", new(95, 158, 160) },
        { "chartreuse", new(127, 255, 0) },
        { "chocolate", new(210, 105, 30) },
        { "coral", new(255, 127, 80) },
        { "cornflowerblue", new(100, 149, 237) },
        { "cornsilk", new(255, 248, 220) },
        { "crimson", new(220, 20, 60) },
        { "cyan", new(0, 255, 255) },
        { "darkblue", new(0, 0, 139) },
        { "darkcyan", new(0, 139, 139) },
        { "darkgoldenrod", new(184, 134, 11) },
        { "darkgray", new(169, 169, 169) },
        { "darkgreen", new(0, 100, 0) },
        { "darkgrey", new(169, 169, 169) },
        { "darkkhaki", new(189, 183, 107) },
        { "darkmagenta", new(139, 0, 139) },
        { "darkolivegreen", new(85, 107, 47) },
        { "darkorange", new(255, 140, 0) },
        { "darkorchid", new(153, 50, 204) },
        { "darkred", new(139, 0, 0) },
        { "darksalmon", new(233, 150, 122) },
        { "darkseagreen", new(143, 188, 143) },
        { "darkslateblue", new(72, 61, 139) },
        { "darkslategray", new(47, 79, 79) },
        { "darkslategrey", new(47, 79, 79) },
        { "darkturquoise", new(0, 206, 209) },
        { "darkviolet", new(148, 0, 211) },
        { "deeppink", new(255, 20, 147) },
        { "deepskyblue", new(0, 191, 255) },
        { "dimgray", new(105, 105, 105) },
        { "dimgrey", new(105, 105, 105) },
        { "dodgerblue", new(30, 144, 255) },
        { "firebrick", new(178, 34, 34) },
        { "floralwhite", new(255, 250, 240) },
        { "forestgreen", new(34, 139, 34) },
        { "fuchsia", new(255, 0, 255) },
        { "gainsboro", new(220, 220, 220) },
        { "ghostwhite", new(248, 248, 255) },
        { "gold", new(255, 215, 0) },
        { "goldenrod", new(218, 165, 32) },
        { "gray", new(128, 128, 128) },
        { "green", new(0, 128, 0) },
        { "greenyellow", new(173, 255, 47) },
        { "grey", new(128, 128, 128) },
        { "honeydew", new(240, 255, 240) },
        { "hotpink", new(255, 105, 180) },
        { "indianred", new(205, 92, 92) },
        { "indigo", new(75, 0, 130) },
        { "ivory", new(255, 255, 240) },
        { "khaki", new(240, 230, 140) },
        { "lavender", new(230, 230, 250) },
        { "lavenderblush", new(255, 240, 245) },
        { "lawngreen", new(124, 252, 0) },
        { "lemonchiffon", new(255, 250, 205) },
        { "lightblue", new(173, 216, 230) },
        { "lightcoral", new(240, 128, 128) },
        { "lightcyan", new(224, 255, 255) },
        { "lightgoldenrodyellow", new(250, 250, 210) },
        { "lightgray", new(211, 211, 211) },
        { "lightgreen", new(144, 238, 144) },
        { "lightgrey", new(211, 211, 211) },
        { "lightpink", new(255, 182, 193) },
        { "lightsalmon", new(255, 160, 122) },
        { "lightseagreen", new(32, 178, 170) },
        { "lightskyblue", new(135, 206, 250) },
        { "lightslategray", new(119, 136, 153) },
        { "lightslategrey", new(119, 136, 153) },
        { "lightsteelblue", new(176, 196, 222) },
        { "lightyellow", new(255, 255, 224) },
        { "lime", new(0, 255, 0) },
        { "limegreen", new(50, 205, 50) },
        { "linen", new(250, 240, 230) },
        { "magenta", new(255, 0, 255) },
        { "maroon", new(128, 0, 0) },
        { "mediumaquamarine", new(102, 205, 170) },
        { "mediumblue", new(0, 0, 205) },
        { "mediumorchid", new(186, 85, 211) },
        { "mediumpurple", new(147, 112, 219) },
        { "mediumseagreen", new(60, 179, 113) },
        { "mediumslateblue", new(123, 104, 238) },
        { "mediumspringgreen", new(0, 250, 154) },
        { "mediumturquoise", new(72, 209, 204) },
        { "mediumvioletred", new(199, 21, 133) },
        { "midnightblue", new(25, 25, 112) },
        { "mintcream", new(245, 255, 250) },
        { "mistyrose", new(255, 228, 225) },
        { "moccasin", new(255, 228, 181) },
        { "navajowhite", new(255, 222, 173) },
        { "navy", new(0, 0, 128) },
        { "oldlace", new(253, 245, 230) },
        { "olive", new(128, 128, 0) },
        { "olivedrab", new(107, 142, 35) },
        { "orange", new(255, 165, 0) },
        { "orangered", new(255, 69, 0) },
        { "orchid", new(218, 112, 214) },
        { "palegoldenrod", new(238, 232, 170) },
        { "palegreen", new(152, 251, 152) },
        { "paleturquoise", new(175, 238, 238) },
        { "palevioletred", new(219, 112, 147) },
        { "papayawhip", new(255, 239, 213) },
        { "peachpuff", new(255, 218, 185) },
        { "peru", new(205, 133, 63) },
        { "pink", new(255, 192, 203) },
        { "plum", new(221, 160, 221) },
        { "powderblue", new(176, 224, 230) },
        { "purple", new(128, 0, 128) },
        { "rebeccapurple", new(102, 51, 153) },
        { "red", new(255, 0, 0) },
        { "rosybrown", new(188, 143, 143) },
        { "royalblue", new(65, 105, 225) },
        { "saddlebrown", new(139, 69, 19) },
        { "salmon", new(250, 128, 114) },
        { "sandybrown", new(244, 164, 96) },
        { "seagreen", new(46, 139, 87) },
        { "seashell", new(255, 245, 238) },
        { "sienna", new(160, 82, 45) },
        { "silver", new(192, 192, 192) },
        { "skyblue", new(135, 206, 235) },
        { "slateblue", new(106, 90, 205) },
        { "slategray", new(112, 128, 144) },
        { "slategrey", new(112, 128, 144) },
        { "snow", new(255, 250, 250) },
        { "springgreen", new(0, 255, 127) },
        { "steelblue", new(70, 130, 180) },
        { "tan", new(210, 180, 140) },
        { "teal", new(0, 128, 128) },
        { "thistle", new(216, 191, 216) },
        { "tomato", new(255, 99, 71) },
        { "turquoise", new(64, 224, 208) },
        { "violet", new(238, 130, 238) },
        { "wheat", new(245, 222, 179) },
        { "white", new(255, 255, 255) },
        { "whitesmoke", new(245, 245, 245) },
        { "yellow", new(255, 255, 0) },
        { "yellowgreen", new(154, 205, 50) },
        { "transparent", new(0, 0, 0) }
    };
    
    private static readonly List<TestCaseData> Rgb255TestData =
    [
        new("aliceblue", AliceBlue),
        new("antiquewhite", AntiqueWhite),
        new("aqua", Aqua),
        new("aquamarine", Aquamarine),
        new("azure", Azure),
        new("beige", Beige),
        new("bisque", Bisque),
        new("black", Black),
        new("blanchedalmond", BlanchedAlmond),
        new("blue", Blue),
        new("blueviolet", BlueViolet),
        new("brown", Brown),
        new("burlywood", Burlywood),
        new("cadetblue", CadetBlue),
        new("chartreuse", Chartreuse),
        new("chocolate", Chocolate),
        new("coral", Coral),
        new("cornflowerblue", CornflowerBlue),
        new("cornsilk", Cornsilk),
        new("crimson", Crimson),
        new("cyan", Cyan),
        new("darkblue", DarkBlue),
        new("darkcyan", DarkCyan),
        new("darkgoldenrod", DarkGoldenrod),
        new("darkgray", DarkGray),
        new("darkgreen", DarkGreen),
        new("darkgrey", DarkGrey),
        new("darkkhaki", DarkKhaki),
        new("darkmagenta", DarkMagenta),
        new("darkolivegreen", DarkOliveGreen),
        new("darkorange", DarkOrange),
        new("darkorchid", DarkOrchid),
        new("darkred", DarkRed),
        new("darksalmon", DarkSalmon),
        new("darkseagreen", DarkSeaGreen),
        new("darkslateblue", DarkSlateBlue),
        new("darkslategray", DarkSlateGray),
        new("darkslategrey", DarkSlateGrey),
        new("darkturquoise", DarkTurquoise),
        new("darkviolet", DarkViolet),
        new("deeppink", DeepPink),
        new("deepskyblue", DeepSkyBlue),
        new("dimgray", DimGray),
        new("dimgrey", DimGrey),
        new("dodgerblue", DodgerBlue),
        new("firebrick", FireBrick),
        new("floralwhite", FloralWhite),
        new("forestgreen", ForestGreen),
        new("fuchsia", Fuchsia),
        new("gainsboro", Gainsboro),
        new("ghostwhite", GhostWhite),
        new("gold", Gold),
        new("goldenrod", Goldenrod),
        new("gray", Gray),
        new("green", Green),
        new("greenyellow", GreenYellow),
        new("grey", Grey),
        new("honeydew", Honeydew),
        new("hotpink", HotPink),
        new("indianred", IndianRed),
        new("indigo", Indigo),
        new("ivory", Ivory),
        new("khaki", Khaki),
        new("lavender", Lavender),
        new("lavenderblush", LavenderBlush),
        new("lawngreen", LawnGreen),
        new("lemonchiffon", LemonChiffon),
        new("lightblue", LightBlue),
        new("lightcoral", LightCoral),
        new("lightcyan", LightCyan),
        new("lightgoldenrodyellow", LightGoldenrodYellow),
        new("lightgray", LightGray),
        new("lightgreen", LightGreen),
        new("lightgrey", LightGrey),
        new("lightpink", LightPink),
        new("lightsalmon", LightSalmon),
        new("lightseagreen", LightSeaGreen),
        new("lightskyblue", LightSkyBlue),
        new("lightslategray", LightSlateGray),
        new("lightslategrey", LightSlateGrey),
        new("lightsteelblue", LightSteelBlue),
        new("lightyellow", LightYellow),
        new("lime", Lime),
        new("limegreen", LimeGreen),
        new("linen", Linen),
        new("magenta", Magenta),
        new("maroon", Maroon),
        new("mediumaquamarine", MediumAquamarine),
        new("mediumblue", MediumBlue),
        new("mediumorchid", MediumOrchid),
        new("mediumpurple", MediumPurple),
        new("mediumseagreen", MediumSeaGreen),
        new("mediumslateblue", MediumSlateBlue),
        new("mediumspringgreen", MediumSpringGreen),
        new("mediumturquoise", MediumTurquoise),
        new("mediumvioletred", MediumVioletRed),
        new("midnightblue", MidnightBlue),
        new("mintcream", MintCream),
        new("mistyrose", MistyRose),
        new("moccasin", Moccasin),
        new("navajowhite", NavajoWhite),
        new("navy", Navy),
        new("oldlace", OldLace),
        new("olive", Olive),
        new("olivedrab", OliveDrab),
        new("orange", Orange),
        new("orangered", OrangeRed),
        new("orchid", Orchid),
        new("palegoldenrod", PaleGoldenrod),
        new("palegreen", PaleGreen),
        new("paleturquoise", PaleTurquoise),
        new("palevioletred", PaleVioletRed),
        new("papayawhip", PapayaWhip),
        new("peachpuff", PeachPuff),
        new("peru", Peru),
        new("pink", Pink),
        new("plum", Plum),
        new("powderblue", PowderBlue),
        new("purple", Purple),
        new("rebeccapurple", RebeccaPurple),
        new("red", Red),
        new("rosybrown", RosyBrown),
        new("royalblue", RoyalBlue),
        new("saddlebrown", SaddleBrown),
        new("salmon", Salmon),
        new("sandybrown", SandyBrown),
        new("seagreen", SeaGreen),
        new("seashell", Seashell),
        new("sienna", Sienna),
        new("silver", Silver),
        new("skyblue", SkyBlue),
        new("slateblue", SlateBlue),
        new("slategray", SlateGray),
        new("slategrey", SlateGrey),
        new("snow", Snow),
        new("springgreen", SpringGreen),
        new("steelblue", SteelBlue),
        new("tan", Tan),
        new("teal", Teal),
        new("thistle", Thistle),
        new("tomato", Tomato),
        new("turquoise", Turquoise),
        new("violet", Violet),
        new("wheat", Wheat),
        new("white", White),
        new("whitesmoke", WhiteSmoke),
        new("yellow", Yellow),
        new("yellowgreen", YellowGreen),
        new("transparent", Css.Transparent)
    ];

    [TestCaseSource(nameof(Rgb255TestData))]
    public void Rgb255(string name, Unicolour unicolour)
    {
        var expected = Rgb255Lookup[name];
        TestUtils.AssertTriplet<Rgb255>(unicolour, expected, 0);
    }

    [Test]
    public void Transparent()
    {
        TestUtils.AssertTriplet<Rgb255>(Css.Transparent, new(0, 0, 0), 0);
        Assert.That(Css.Transparent.Alpha.A, Is.EqualTo(0));
    }
    
    private static readonly List<TestCaseData> DuplicateTestData =
    [
        new TestCaseData(Cyan, Aqua).SetName("cyan = aqua"),
        new TestCaseData(Magenta, Fuchsia).SetName("magenta = fuchsia"),
        new TestCaseData(DarkGrey, DarkGray).SetName("darkgrey = darkgray"),
        new TestCaseData(DarkSlateGrey, DarkSlateGray).SetName("darkslategrey = darkslategray"),
        new TestCaseData(DimGrey, DimGray).SetName("dimgrey = dimgray"),
        new TestCaseData(Grey, Gray).SetName("grey = gray"),
        new TestCaseData(LightGrey, LightGray).SetName("lightgrey = lightgray"),
        new TestCaseData(LightSlateGrey, LightSlateGray).SetName("lightslategrey = lightslategray"),
        new TestCaseData(SlateGrey, SlateGray).SetName("slategrey = slategray")
    ];
    
    [TestCaseSource(nameof(DuplicateTestData))]
    public void Duplicates(Unicolour unicolour1, Unicolour unicolour2)
    {
        Assert.That(ReferenceEquals(unicolour1, unicolour2), Is.False);
        Assert.That(unicolour1.Hex, Is.EqualTo(unicolour2.Hex));
    }
    
    [Test]
    public void DuplicateCount()
    {
        var hexValues = Css.All.Select(x => x.Hex).ToList();
        var duplicateCount = hexValues.Count - hexValues.Distinct().Count();
        Assert.That(duplicateCount, Is.EqualTo(DuplicateTestData.Count));
    }

    [Test]
    public void All()
    {
        Assert.That(Css.All.Count(), Is.EqualTo(148));
        Assert.That(Css.All.Distinct().Count(), Is.EqualTo(139));
    }

    private static readonly List<string> Names = Rgb255Lookup.Keys.ToList();
    
    [Test]
    public void Name([ValueSource(nameof(Names))] string name)
    {
        var unicolour = FromName(name);
        Assert.That(unicolour, Is.Not.Null);
        
        var expected = Rgb255Lookup[name];
        TestUtils.AssertTriplet<Rgb255>(unicolour!, expected, 0);
    }

    private static readonly List<string> TransformedNames = Names.Select(AddWhitespaceAndUppercase).ToList();
    private static string AddWhitespaceAndUppercase(string text)
    {
        var multicase = string.Concat(text.Select((x, i) => i % 2 == 0 ? char.ToLower(x) : char.ToUpper(x)));
        return multicase.Insert(multicase.Length, "\n").Insert(multicase.Length - 1, " ").Insert(0, "\t").Insert(2, " ");
    }
    
    [Test]
    public void NameWithWhitespaceAndCasing([ValueSource(nameof(TransformedNames))] string name)
    {
        var unicolour = FromName(name);
        Assert.That(unicolour, Is.Not.Null);
    }
    
    // names taken from https://en.wikipedia.org/wiki/X11_color_names but are not defined in the CSS specification
    [TestCase("light goldenrod")]
    [TestCase("navy blue")]
    [TestCase("web gray")]
    [TestCase("web grey")]
    [TestCase("web green")]
    [TestCase("web maroon")]
    [TestCase("web purple")]
    // names that look like they should exist based on other names
    [TestCase("goldenrod yellow")]
    [TestCase("olive green")]
    [TestCase("medium goldenrod")]
    [TestCase("medium red")]
    [TestCase("violet red")]
    [TestCase("periwinkle")]
    // misspellings
    [TestCase("biege")]
    [TestCase("chartruese")]
    [TestCase("dodgeblue")]
    [TestCase("orang")]
    [TestCase("stealblue")]
    [TestCase("tourquise")]
    // common text and numbers
    [TestCase("unicolour")]
    [TestCase("abc")]
    [TestCase("123")]
    // punctuation and whitespace
    [TestCase("\"aliceblue\"")]
    [TestCase(".")]
    [TestCase(" ")]
    [TestCase("")]
    [TestCase("\t")]
    [TestCase("\n")]
    [TestCase(null)]
    public void NameNotFound(string name)
    {
        var unicolour = FromName(name);
        Assert.That(unicolour, Is.Null);
    }
}