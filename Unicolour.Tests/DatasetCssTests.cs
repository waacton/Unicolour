namespace Wacton.Unicolour.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using Wacton.Unicolour.Tests.Utils;
using static Datasets.Css;

public class DatasetCssTests
{
    // https://www.w3.org/TR/css-color-4/#named-colors
    private static readonly List<TestCaseData> Rgb255TestData = new()
    {
        new TestCaseData(AliceBlue, 240, 248, 255).SetName("Alice Blue"),
        new TestCaseData(AntiqueWhite, 250, 235, 215).SetName("Antique White"),
        new TestCaseData(Aqua, 0, 255, 255).SetName("Aqua"),
        new TestCaseData(Aquamarine, 127, 255, 212).SetName("Aquamarine"),
        new TestCaseData(Azure, 240, 255, 255).SetName("Azure"),
        new TestCaseData(Beige, 245, 245, 220).SetName("Beige"),
        new TestCaseData(Bisque, 255, 228, 196).SetName("Bisque"),
        new TestCaseData(Black, 0, 0, 0).SetName("Black"),
        new TestCaseData(BlanchedAlmond, 255, 235, 205).SetName("Blanched Almond"),
        new TestCaseData(Blue, 0, 0, 255).SetName("Blue"),
        new TestCaseData(BlueViolet, 138, 43, 226).SetName("Blue Violet"),
        new TestCaseData(Brown, 165, 42, 42).SetName("Brown"),
        new TestCaseData(Burlywood, 222, 184, 135).SetName("Burlywood"),
        new TestCaseData(CadetBlue, 95, 158, 160).SetName("Cadet Blue"),
        new TestCaseData(Chartreuse, 127, 255, 0).SetName("Chartreuse"),
        new TestCaseData(Chocolate, 210, 105, 30).SetName("Chocolate"),
        new TestCaseData(Coral, 255, 127, 80).SetName("Coral"),
        new TestCaseData(CornflowerBlue, 100, 149, 237).SetName("Cornflower Blue"),
        new TestCaseData(Cornsilk, 255, 248, 220).SetName("Cornsilk"),
        new TestCaseData(Crimson, 220, 20, 60).SetName("Crimson"),
        new TestCaseData(Cyan, 0, 255, 255).SetName("Cyan"),
        new TestCaseData(DarkBlue, 0, 0, 139).SetName("Dark Blue"),
        new TestCaseData(DarkCyan, 0, 139, 139).SetName("Dark Cyan"),
        new TestCaseData(DarkGoldenrod, 184, 134, 11).SetName("Dark Goldenrod"),
        new TestCaseData(DarkGray, 169, 169, 169).SetName("Dark Gray"),
        new TestCaseData(DarkGreen, 0, 100, 0).SetName("Dark Green"),
        new TestCaseData(DarkGrey, 169, 169, 169).SetName("Dark Grey"),
        new TestCaseData(DarkKhaki, 189, 183, 107).SetName("Dark Khaki"),
        new TestCaseData(DarkMagenta, 139, 0, 139).SetName("Dark Magenta"),
        new TestCaseData(DarkOliveGreen, 85, 107, 47).SetName("Dark Olive Green"),
        new TestCaseData(DarkOrange, 255, 140, 0).SetName("Dark Orange"),
        new TestCaseData(DarkOrchid, 153, 50, 204).SetName("Dark Orchid"),
        new TestCaseData(DarkRed, 139, 0, 0).SetName("Dark Red"),
        new TestCaseData(DarkSalmon, 233, 150, 122).SetName("Dark Salmon"),
        new TestCaseData(DarkSeaGreen, 143, 188, 143).SetName("Dark Sea Green"),
        new TestCaseData(DarkSlateBlue, 72, 61, 139).SetName("Dark Slate Blue"),
        new TestCaseData(DarkSlateGray, 47, 79, 79).SetName("Dark Slate Gray"),
        new TestCaseData(DarkSlateGrey, 47, 79, 79).SetName("Dark Slate Grey"),
        new TestCaseData(DarkTurquoise, 0, 206, 209).SetName("Dark Turquoise"),
        new TestCaseData(DarkViolet, 148, 0, 211).SetName("Dark Violet"),
        new TestCaseData(DeepPink, 255, 20, 147).SetName("Deep Pink"),
        new TestCaseData(DeepSkyBlue, 0, 191, 255).SetName("Deep Sky Blue"),
        new TestCaseData(DimGray, 105, 105, 105).SetName("Dim Gray"),
        new TestCaseData(DimGrey, 105, 105, 105).SetName("Dim Grey"),
        new TestCaseData(DodgerBlue, 30, 144, 255).SetName("Dodger Blue"),
        new TestCaseData(FireBrick, 178, 34, 34).SetName("Fire Brick"),
        new TestCaseData(FloralWhite, 255, 250, 240).SetName("Floral White"),
        new TestCaseData(ForestGreen, 34, 139, 34).SetName("Forest Green"),
        new TestCaseData(Fuchsia, 255, 0, 255).SetName("Fuchsia"),
        new TestCaseData(Gainsboro, 220, 220, 220).SetName("Gainsboro"),
        new TestCaseData(GhostWhite, 248, 248, 255).SetName("Ghost White"),
        new TestCaseData(Gold, 255, 215, 0).SetName("Gold"),
        new TestCaseData(Goldenrod, 218, 165, 32).SetName("Goldenrod"),
        new TestCaseData(Gray, 128, 128, 128).SetName("Gray"),
        new TestCaseData(Green, 0, 128, 0).SetName("Green"),
        new TestCaseData(GreenYellow, 173, 255, 47).SetName("Green Yellow"),
        new TestCaseData(Grey, 128, 128, 128).SetName("Grey"),
        new TestCaseData(Honeydew, 240, 255, 240).SetName("Honeydew"),
        new TestCaseData(HotPink, 255, 105, 180).SetName("Hot Pink"),
        new TestCaseData(IndianRed, 205, 92, 92).SetName("Indian Red"),
        new TestCaseData(Indigo, 75, 0, 130).SetName("Indigo"),
        new TestCaseData(Ivory, 255, 255, 240).SetName("Ivory"),
        new TestCaseData(Khaki, 240, 230, 140).SetName("Khaki"),
        new TestCaseData(Lavender, 230, 230, 250).SetName("Lavender"),
        new TestCaseData(LavenderBlush, 255, 240, 245).SetName("Lavender Blush"),
        new TestCaseData(LawnGreen, 124, 252, 0).SetName("Lawn Green"),
        new TestCaseData(LemonChiffon, 255, 250, 205).SetName("Lemon Chiffon"),
        new TestCaseData(LightBlue, 173, 216, 230).SetName("Light Blue"),
        new TestCaseData(LightCoral, 240, 128, 128).SetName("Light Coral"),
        new TestCaseData(LightCyan, 224, 255, 255).SetName("Light Cyan"),
        new TestCaseData(LightGoldenrodYellow, 250, 250, 210).SetName("Light Goldenrod Yellow"),
        new TestCaseData(LightGray, 211, 211, 211).SetName("Light Gray"),
        new TestCaseData(LightGreen, 144, 238, 144).SetName("Light Green"),
        new TestCaseData(LightGrey, 211, 211, 211).SetName("Light Grey"),
        new TestCaseData(LightPink, 255, 182, 193).SetName("Light Pink"),
        new TestCaseData(LightSalmon, 255, 160, 122).SetName("Light Salmon"),
        new TestCaseData(LightSeaGreen, 32, 178, 170).SetName("Light Sea Green"),
        new TestCaseData(LightSkyBlue, 135, 206, 250).SetName("Light Sky Blue"),
        new TestCaseData(LightSlateGray, 119, 136, 153).SetName("Light Slate Gray"),
        new TestCaseData(LightSlateGrey, 119, 136, 153).SetName("Light Slate Grey"),
        new TestCaseData(LightSteelBlue, 176, 196, 222).SetName("Light Steel Blue"),
        new TestCaseData(LightYellow, 255, 255, 224).SetName("Light Yellow"),
        new TestCaseData(Lime, 0, 255, 0).SetName("Lime"),
        new TestCaseData(LimeGreen, 50, 205, 50).SetName("Lime Green"),
        new TestCaseData(Linen, 250, 240, 230).SetName("Linen"),
        new TestCaseData(Magenta, 255, 0, 255).SetName("Magenta"),
        new TestCaseData(Maroon, 128, 0, 0).SetName("Maroon"),
        new TestCaseData(MediumAquamarine, 102, 205, 170).SetName("Medium Aquamarine"),
        new TestCaseData(MediumBlue, 0, 0, 205).SetName("Medium Blue"),
        new TestCaseData(MediumOrchid, 186, 85, 211).SetName("Medium Orchid"),
        new TestCaseData(MediumPurple, 147, 112, 219).SetName("Medium Purple"),
        new TestCaseData(MediumSeaGreen, 60, 179, 113).SetName("Medium Sea Green"),
        new TestCaseData(MediumSlateBlue, 123, 104, 238).SetName("Medium Slate Blue"),
        new TestCaseData(MediumSpringGreen, 0, 250, 154).SetName("Medium Spring Green"),
        new TestCaseData(MediumTurquoise, 72, 209, 204).SetName("Medium Turquoise"),
        new TestCaseData(MediumVioletRed, 199, 21, 133).SetName("Medium Violet Red"),
        new TestCaseData(MidnightBlue, 25, 25, 112).SetName("Midnight Blue"),
        new TestCaseData(MintCream, 245, 255, 250).SetName("Mint Cream"),
        new TestCaseData(MistyRose, 255, 228, 225).SetName("Misty Rose"),
        new TestCaseData(Moccasin, 255, 228, 181).SetName("Moccasin"),
        new TestCaseData(NavajoWhite, 255, 222, 173).SetName("Navajo White"),
        new TestCaseData(Navy, 0, 0, 128).SetName("Navy"),
        new TestCaseData(OldLace, 253, 245, 230).SetName("Old Lace"),
        new TestCaseData(Olive, 128, 128, 0).SetName("Olive"),
        new TestCaseData(OliveDrab, 107, 142, 35).SetName("Olive Drab"),
        new TestCaseData(Orange, 255, 165, 0).SetName("Orange"),
        new TestCaseData(OrangeRed, 255, 69, 0).SetName("Orange Red"),
        new TestCaseData(Orchid, 218, 112, 214).SetName("Orchid"),
        new TestCaseData(PaleGoldenrod, 238, 232, 170).SetName("Pale Goldenrod"),
        new TestCaseData(PaleGreen, 152, 251, 152).SetName("Pale Green"),
        new TestCaseData(PaleTurquoise, 175, 238, 238).SetName("Pale Turquoise"),
        new TestCaseData(PaleVioletRed, 219, 112, 147).SetName("Pale Violet Red"),
        new TestCaseData(PapayaWhip, 255, 239, 213).SetName("Papaya Whip"),
        new TestCaseData(PeachPuff, 255, 218, 185).SetName("Peach Puff"),
        new TestCaseData(Peru, 205, 133, 63).SetName("Peru"),
        new TestCaseData(Pink, 255, 192, 203).SetName("Pink"),
        new TestCaseData(Plum, 221, 160, 221).SetName("Plum"),
        new TestCaseData(PowderBlue, 176, 224, 230).SetName("Powder Blue"),
        new TestCaseData(Purple, 128, 0, 128).SetName("Purple"),
        new TestCaseData(RebeccaPurple, 102, 51, 153).SetName("Rebecca Purple"),
        new TestCaseData(Red, 255, 0, 0).SetName("Red"),
        new TestCaseData(RosyBrown, 188, 143, 143).SetName("Rosy Brown"),
        new TestCaseData(RoyalBlue, 65, 105, 225).SetName("Royal Blue"),
        new TestCaseData(SaddleBrown, 139, 69, 19).SetName("Saddle Brown"),
        new TestCaseData(Salmon, 250, 128, 114).SetName("Salmon"),
        new TestCaseData(SandyBrown, 244, 164, 96).SetName("Sandy Brown"),
        new TestCaseData(SeaGreen, 46, 139, 87).SetName("Sea Green"),
        new TestCaseData(Seashell, 255, 245, 238).SetName("Seashell"),
        new TestCaseData(Sienna, 160, 82, 45).SetName("Sienna"),
        new TestCaseData(Silver, 192, 192, 192).SetName("Silver"),
        new TestCaseData(SkyBlue, 135, 206, 235).SetName("Sky Blue"),
        new TestCaseData(SlateBlue, 106, 90, 205).SetName("Slate Blue"),
        new TestCaseData(SlateGray, 112, 128, 144).SetName("Slate Gray"),
        new TestCaseData(SlateGrey, 112, 128, 144).SetName("Slate Grey"),
        new TestCaseData(Snow, 255, 250, 250).SetName("Snow"),
        new TestCaseData(SpringGreen, 0, 255, 127).SetName("Spring Green"),
        new TestCaseData(SteelBlue, 70, 130, 180).SetName("Steel Blue"),
        new TestCaseData(Tan, 210, 180, 140).SetName("Tan"),
        new TestCaseData(Teal, 0, 128, 128).SetName("Teal"),
        new TestCaseData(Thistle, 216, 191, 216).SetName("Thistle"),
        new TestCaseData(Tomato, 255, 99, 71).SetName("Tomato"),
        new TestCaseData(Turquoise, 64, 224, 208).SetName("Turquoise"),
        new TestCaseData(Violet, 238, 130, 238).SetName("Violet"),
        new TestCaseData(Wheat, 245, 222, 179).SetName("Wheat"),
        new TestCaseData(White, 255, 255, 255).SetName("White"),
        new TestCaseData(WhiteSmoke, 245, 245, 245).SetName("White Smoke"),
        new TestCaseData(Yellow, 255, 255, 0).SetName("Yellow"),
        new TestCaseData(YellowGreen, 154, 205, 50).SetName("Yellow Green")
    };
    
    private static readonly List<TestCaseData> DuplicateTestData = new()
    {
        new TestCaseData(Cyan, Aqua).SetName("Cyan"),
        new TestCaseData(Magenta, Fuchsia).SetName("Magenta"),
        new TestCaseData(DarkGrey, DarkGray).SetName("Dark Grey"),
        new TestCaseData(DarkSlateGrey, DarkSlateGray).SetName("Dark Slate Grey"),
        new TestCaseData(DimGrey, DimGray).SetName("Dim Grey"),
        new TestCaseData(Grey, Gray).SetName("Grey"),
        new TestCaseData(LightGrey, LightGray).SetName("Light Grey"),
        new TestCaseData(LightSlateGrey, LightSlateGray).SetName("Light Slate Grey"),
        new TestCaseData(SlateGrey, SlateGray).SetName("Slate Grey")
    };

    [TestCaseSource(nameof(Rgb255TestData))]
    public void Rgb255(Unicolour unicolour, int expectedR, int expectedG, int expectedB)
    {
        var expectedRgb255 = new Rgb255(expectedR, expectedG, expectedB);
        TestUtils.AssertTriplet<Rgb255>(unicolour, expectedRgb255.Triplet, 0);
    }

    [Test]
    public void Transparent()
    {
        TestUtils.AssertTriplet<Rgb255>(Css.Transparent, new(0, 0, 0), 0);
        Assert.That(Css.Transparent.Alpha.A, Is.EqualTo(0));
    }
    
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

    [TestCase("aliceblue")]
    [TestCase("antiquewhite")]
    [TestCase("aqua")]
    [TestCase("aquamarine")]
    [TestCase("azure")]
    [TestCase("beige")]
    [TestCase("bisque")]
    [TestCase("black")]
    [TestCase("blanchedalmond")]
    [TestCase("blue")]
    [TestCase("blueviolet")]
    [TestCase("brown")]
    [TestCase("burlywood")]
    [TestCase("cadetblue")]
    [TestCase("chartreuse")]
    [TestCase("chocolate")]
    [TestCase("coral")]
    [TestCase("cornflowerblue")]
    [TestCase("cornsilk")]
    [TestCase("crimson")]
    [TestCase("cyan")]
    [TestCase("darkblue")]
    [TestCase("darkcyan")]
    [TestCase("darkgoldenrod")]
    [TestCase("darkgray")]
    [TestCase("darkgreen")]
    [TestCase("darkgrey")]
    [TestCase("darkkhaki")]
    [TestCase("darkmagenta")]
    [TestCase("darkolivegreen")]
    [TestCase("darkorange")]
    [TestCase("darkorchid")]
    [TestCase("darkred")]
    [TestCase("darksalmon")]
    [TestCase("darkseagreen")]
    [TestCase("darkslateblue")]
    [TestCase("darkslategray")]
    [TestCase("darkslategrey")]
    [TestCase("darkturquoise")]
    [TestCase("darkviolet")]
    [TestCase("deeppink")]
    [TestCase("deepskyblue")]
    [TestCase("dimgray")]
    [TestCase("dimgrey")]
    [TestCase("dodgerblue")]
    [TestCase("firebrick")]
    [TestCase("floralwhite")]
    [TestCase("forestgreen")]
    [TestCase("fuchsia")]
    [TestCase("gainsboro")]
    [TestCase("ghostwhite")]
    [TestCase("gold")]
    [TestCase("goldenrod")]
    [TestCase("gray")]
    [TestCase("green")]
    [TestCase("greenyellow")]
    [TestCase("grey")]
    [TestCase("honeydew")]
    [TestCase("hotpink")]
    [TestCase("indianred")]
    [TestCase("indigo")]
    [TestCase("ivory")]
    [TestCase("khaki")]
    [TestCase("lavender")]
    [TestCase("lavenderblush")]
    [TestCase("lawngreen")]
    [TestCase("lemonchiffon")]
    [TestCase("lightblue")]
    [TestCase("lightcoral")]
    [TestCase("lightcyan")]
    [TestCase("lightgoldenrodyellow")]
    [TestCase("lightgray")]
    [TestCase("lightgreen")]
    [TestCase("lightgrey")]
    [TestCase("lightpink")]
    [TestCase("lightsalmon")]
    [TestCase("lightseagreen")]
    [TestCase("lightskyblue")]
    [TestCase("lightslategray")]
    [TestCase("lightslategrey")]
    [TestCase("lightsteelblue")]
    [TestCase("lightyellow")]
    [TestCase("lime")]
    [TestCase("limegreen")]
    [TestCase("linen")]
    [TestCase("magenta")]
    [TestCase("maroon")]
    [TestCase("mediumaquamarine")]
    [TestCase("mediumblue")]
    [TestCase("mediumorchid")]
    [TestCase("mediumpurple")]
    [TestCase("mediumseagreen")]
    [TestCase("mediumslateblue")]
    [TestCase("mediumspringgreen")]
    [TestCase("mediumturquoise")]
    [TestCase("mediumvioletred")]
    [TestCase("midnightblue")]
    [TestCase("mintcream")]
    [TestCase("mistyrose")]
    [TestCase("moccasin")]
    [TestCase("navajowhite")]
    [TestCase("navy")]
    [TestCase("oldlace")]
    [TestCase("olive")]
    [TestCase("olivedrab")]
    [TestCase("orange")]
    [TestCase("orangered")]
    [TestCase("orchid")]
    [TestCase("palegoldenrod")]
    [TestCase("palegreen")]
    [TestCase("paleturquoise")]
    [TestCase("palevioletred")]
    [TestCase("papayawhip")]
    [TestCase("peachpuff")]
    [TestCase("peru")]
    [TestCase("pink")]
    [TestCase("plum")]
    [TestCase("powderblue")]
    [TestCase("purple")]
    [TestCase("rebeccapurple")]
    [TestCase("red")]
    [TestCase("rosybrown")]
    [TestCase("royalblue")]
    [TestCase("saddlebrown")]
    [TestCase("salmon")]
    [TestCase("sandybrown")]
    [TestCase("seagreen")]
    [TestCase("seashell")]
    [TestCase("sienna")]
    [TestCase("silver")]
    [TestCase("skyblue")]
    [TestCase("slateblue")]
    [TestCase("slategray")]
    [TestCase("slategrey")]
    [TestCase("snow")]
    [TestCase("springgreen")]
    [TestCase("steelblue")]
    [TestCase("tan")]
    [TestCase("teal")]
    [TestCase("thistle")]
    [TestCase("tomato")]
    [TestCase("transparent")]
    [TestCase("turquoise")]
    [TestCase("violet")]
    [TestCase("wheat")]
    [TestCase("white")]
    [TestCase("whitesmoke")]
    [TestCase("yellow")]
    [TestCase("yellowgreen")]
    public void Name(string name)
    {
        var unicolour = FromName(name);
        Assert.That(unicolour, Is.Not.Null);
    }

    [TestCase("\taliceblue\n")]
    [TestCase("\ta ntiquewhit e\n")]
    [TestCase("\ta qu a\n")]
    [TestCase("\ta quamarin e\n")]
    [TestCase("\ta zur e\n")]
    [TestCase("\tb eig e\n")]
    [TestCase("\tb isqu e\n")]
    [TestCase("\tb lac k\n")]
    [TestCase("\tb lanchedalmon d\n")]
    [TestCase("\tb lu e\n")]
    [TestCase("\tb lueviole t\n")]
    [TestCase("\tb row n\n")]
    [TestCase("\tb urlywoo d\n")]
    [TestCase("\tc adetblu e\n")]
    [TestCase("\tc hartreus e\n")]
    [TestCase("\tc hocolat e\n")]
    [TestCase("\tc ora l\n")]
    [TestCase("\tc ornflowerblu e\n")]
    [TestCase("\tc ornsil k\n")]
    [TestCase("\tc rimso n\n")]
    [TestCase("\tc ya n\n")]
    [TestCase("\td arkblu e\n")]
    [TestCase("\td arkcya n\n")]
    [TestCase("\td arkgoldenro d\n")]
    [TestCase("\td arkgra y\n")]
    [TestCase("\td arkgree n\n")]
    [TestCase("\td arkgre y\n")]
    [TestCase("\td arkkhak i\n")]
    [TestCase("\td arkmagent a\n")]
    [TestCase("\td arkolivegree n\n")]
    [TestCase("\td arkorang e\n")]
    [TestCase("\td arkorchi d\n")]
    [TestCase("\td arkre d\n")]
    [TestCase("\td arksalmo n\n")]
    [TestCase("\td arkseagree n\n")]
    [TestCase("\td arkslateblu e\n")]
    [TestCase("\td arkslategra y\n")]
    [TestCase("\td arkslategre y\n")]
    [TestCase("\td arkturquois e\n")]
    [TestCase("\td arkviole t\n")]
    [TestCase("\td eeppin k\n")]
    [TestCase("\td eepskyblu e\n")]
    [TestCase("\td imgra y\n")]
    [TestCase("\td imgre y\n")]
    [TestCase("\td odgerblu e\n")]
    [TestCase("\tf irebric k\n")]
    [TestCase("\tf loralwhit e\n")]
    [TestCase("\tf orestgree n\n")]
    [TestCase("\tf uchsi a\n")]
    [TestCase("\tg ainsbor o\n")]
    [TestCase("\tg hostwhit e\n")]
    [TestCase("\tg ol d\n")]
    [TestCase("\tg oldenro d\n")]
    [TestCase("\tg ra y\n")]
    [TestCase("\tg ree n\n")]
    [TestCase("\tg reenyello w\n")]
    [TestCase("\tg re y\n")]
    [TestCase("\th oneyde w\n")]
    [TestCase("\th otpin k\n")]
    [TestCase("\ti ndianre d\n")]
    [TestCase("\ti ndig o\n")]
    [TestCase("\ti vor y\n")]
    [TestCase("\tk hak i\n")]
    [TestCase("\tl avende r\n")]
    [TestCase("\tl avenderblus h\n")]
    [TestCase("\tl awngree n\n")]
    [TestCase("\tl emonchiffo n\n")]
    [TestCase("\tl ightblu e\n")]
    [TestCase("\tl ightcora l\n")]
    [TestCase("\tl ightcya n\n")]
    [TestCase("\tl ightgoldenrodyello w\n")]
    [TestCase("\tl ightgra y\n")]
    [TestCase("\tl ightgree n\n")]
    [TestCase("\tl ightgre y\n")]
    [TestCase("\tl ightpin k\n")]
    [TestCase("\tl ightsalmo n\n")]
    [TestCase("\tl ightseagree n\n")]
    [TestCase("\tl ightskyblu e\n")]
    [TestCase("\tl ightslategra y\n")]
    [TestCase("\tl ightslategre y\n")]
    [TestCase("\tl ightsteelblu e\n")]
    [TestCase("\tl ightyello w\n")]
    [TestCase("\tl im e\n")]
    [TestCase("\tl imegree n\n")]
    [TestCase("\tl ine n\n")]
    [TestCase("\tm agent a\n")]
    [TestCase("\tm aroo n\n")]
    [TestCase("\tm ediumaquamarin e\n")]
    [TestCase("\tm ediumblu e\n")]
    [TestCase("\tm ediumorchi d\n")]
    [TestCase("\tm ediumpurpl e\n")]
    [TestCase("\tm ediumseagree n\n")]
    [TestCase("\tm ediumslateblu e\n")]
    [TestCase("\tm ediumspringgree n\n")]
    [TestCase("\tm ediumturquois e\n")]
    [TestCase("\tm ediumvioletre d\n")]
    [TestCase("\tm idnightblu e\n")]
    [TestCase("\tm intcrea m\n")]
    [TestCase("\tm istyros e\n")]
    [TestCase("\tm occasi n\n")]
    [TestCase("\tn avajowhit e\n")]
    [TestCase("\tn av y\n")]
    [TestCase("\to ldlac e\n")]
    [TestCase("\to liv e\n")]
    [TestCase("\to livedra b\n")]
    [TestCase("\to rang e\n")]
    [TestCase("\to rangere d\n")]
    [TestCase("\to rchi d\n")]
    [TestCase("\tp alegoldenro d\n")]
    [TestCase("\tp alegree n\n")]
    [TestCase("\tp aleturquois e\n")]
    [TestCase("\tp alevioletre d\n")]
    [TestCase("\tp apayawhi p\n")]
    [TestCase("\tp eachpuf f\n")]
    [TestCase("\tp er u\n")]
    [TestCase("\tp in k\n")]
    [TestCase("\tp lu m\n")]
    [TestCase("\tp owderblu e\n")]
    [TestCase("\tp urpl e\n")]
    [TestCase("\tr ebeccapurpl e\n")]
    [TestCase("\tr e d\n")]
    [TestCase("\tr osybrow n\n")]
    [TestCase("\tr oyalblu e\n")]
    [TestCase("\ts addlebrow n\n")]
    [TestCase("\ts almo n\n")]
    [TestCase("\ts andybrow n\n")]
    [TestCase("\ts eagree n\n")]
    [TestCase("\ts eashel l\n")]
    [TestCase("\ts ienn a\n")]
    [TestCase("\ts ilve r\n")]
    [TestCase("\ts kyblu e\n")]
    [TestCase("\ts lateblu e\n")]
    [TestCase("\ts lategra y\n")]
    [TestCase("\ts lategre y\n")]
    [TestCase("\ts no w\n")]
    [TestCase("\ts pringgree n\n")]
    [TestCase("\ts teelblu e\n")]
    [TestCase("\tt a n\n")]
    [TestCase("\tt ea l\n")]
    [TestCase("\tt histl e\n")]
    [TestCase("\tt omat o\n")]
    [TestCase("\tt ransparen t\n")]
    [TestCase("\tt urquois e\n")]
    [TestCase("\tv iole t\n")]
    [TestCase("\tw hea t\n")]
    [TestCase("\tw hit e\n")]
    [TestCase("\tw hitesmok e\n")]
    [TestCase("\ty ello w\n")]
    [TestCase("\ty ellowgre en\n")]
    public void NameWithWhitespace(string name)
    {
        var unicolour = FromName(name);
        Assert.That(unicolour, Is.Not.Null);
    }
    
    // some of these names taken from https://en.wikipedia.org/wiki/X11_color_names but are not defined in the CSS specification
    // other names look like they should exist based on other names, but don't
    // also: only handling casing and whitespace, not attempting to strip punctuation
    [TestCase("light goldenrod")]
    [TestCase("navy blue")]
    [TestCase("web gray")]
    [TestCase("web grey")]
    [TestCase("web green")]
    [TestCase("web maroon")]
    [TestCase("web purple")]
    [TestCase("goldenrod yellow")]
    [TestCase("olive green")]
    [TestCase("medium goldenrod")]
    [TestCase("medium red")]
    [TestCase("violet red")]
    [TestCase("biege")]
    [TestCase("chartruese")]
    [TestCase("dodgeblue")]
    [TestCase("orang")]
    [TestCase("stealblue")]
    [TestCase("tourquise")]
    [TestCase("periwinkle")]
    [TestCase("unicolour")]
    [TestCase("a")]
    [TestCase("\"aliceblue\"")]
    [TestCase("123")]
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