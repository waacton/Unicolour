namespace Wacton.Unicolour.Datasets;

// https://xkcd.com/color/rgb/ · https://xkcd.com/color/rgb.txt
public static class Xkcd
{
    private static readonly Configuration Config = new(RgbConfiguration.StandardRgb, XyzConfiguration.D65);

    public static readonly Unicolour AcidGreen = new(Config, "#8ffe09");
    public static readonly Unicolour Adobe = new(Config, "#bd6c48");
    public static readonly Unicolour Algae = new(Config, "#54ac68");
    public static readonly Unicolour AlgaeGreen = new(Config, "#21c36f");
    public static readonly Unicolour AlmostBlack = new(Config, "#070d0d");
    public static readonly Unicolour Amber = new(Config, "#feb308");
    public static readonly Unicolour Amethyst = new(Config, "#9b5fc0");
    public static readonly Unicolour Apple = new(Config, "#6ecb3c");
    public static readonly Unicolour AppleGreen = new(Config, "#76cd26");
    public static readonly Unicolour Apricot = new(Config, "#ffb16d");
    public static readonly Unicolour Aqua = new(Config, "#13eac9");
    public static readonly Unicolour AquaBlue = new(Config, "#02d8e9");
    public static readonly Unicolour AquaGreen = new(Config, "#12e193");
    public static readonly Unicolour Aquamarine = new(Config, "#04d8b2");
    public static readonly Unicolour AquaMarine = new(Config, "#2ee8bb");
    public static readonly Unicolour ArmyGreen = new(Config, "#4b5d16");
    public static readonly Unicolour Asparagus = new(Config, "#77ab56");
    public static readonly Unicolour Aubergine = new(Config, "#3d0734");
    public static readonly Unicolour Auburn = new(Config, "#9a3001");
    public static readonly Unicolour Avocado = new(Config, "#90b134");
    public static readonly Unicolour AvocadoGreen = new(Config, "#87a922");
    public static readonly Unicolour Azul = new(Config, "#1d5dec");
    public static readonly Unicolour Azure = new(Config, "#069af3");
    public static readonly Unicolour BabyBlue = new(Config, "#a2cffe");
    public static readonly Unicolour BabyGreen = new(Config, "#8cff9e");
    public static readonly Unicolour BabyPink = new(Config, "#ffb7ce");
    public static readonly Unicolour BabyPoo = new(Config, "#ab9004");
    public static readonly Unicolour BabyPoop = new(Config, "#937c00");
    public static readonly Unicolour BabyPoopGreen = new(Config, "#8f9805");
    public static readonly Unicolour BabyPukeGreen = new(Config, "#b6c406");
    public static readonly Unicolour BabyPurple = new(Config, "#ca9bf7");
    public static readonly Unicolour BabyShitBrown = new(Config, "#ad900d");
    public static readonly Unicolour BabyShitGreen = new(Config, "#889717");
    public static readonly Unicolour Banana = new(Config, "#ffff7e");
    public static readonly Unicolour BananaYellow = new(Config, "#fafe4b");
    public static readonly Unicolour BarbiePink = new(Config, "#fe46a5");
    public static readonly Unicolour BarfGreen = new(Config, "#94ac02");
    public static readonly Unicolour Barney = new(Config, "#ac1db8");
    public static readonly Unicolour BarneyPurple = new(Config, "#a00498");
    public static readonly Unicolour BattleshipGrey = new(Config, "#6b7c85");
    public static readonly Unicolour Beige = new(Config, "#e6daa6");
    public static readonly Unicolour Berry = new(Config, "#990f4b");
    public static readonly Unicolour Bile = new(Config, "#b5c306");
    public static readonly Unicolour Black = new(Config, "#000000");
    public static readonly Unicolour Bland = new(Config, "#afa88b");
    public static readonly Unicolour Blood = new(Config, "#770001");
    public static readonly Unicolour BloodOrange = new(Config, "#fe4b03");
    public static readonly Unicolour BloodRed = new(Config, "#980002");
    public static readonly Unicolour Blue = new(Config, "#0343df");
    public static readonly Unicolour Blue_Green = new(Config, "#0f9b8e");
    public static readonly Unicolour Blue_Grey = new(Config, "#758da3");
    public static readonly Unicolour Blue_Purple = new(Config, "#5a06ef");
    public static readonly Unicolour Blueberry = new(Config, "#464196");
    public static readonly Unicolour BlueBlue = new(Config, "#2242c7");
    public static readonly Unicolour Bluegreen = new(Config, "#017a79");
    public static readonly Unicolour BlueGreen = new(Config, "#137e6d");
    public static readonly Unicolour BlueGrey = new(Config, "#607c8e");
    public static readonly Unicolour Bluegrey = new(Config, "#85a3b2");
    public static readonly Unicolour BluePurple = new(Config, "#5729ce");
    public static readonly Unicolour BlueViolet = new(Config, "#5d06e9");
    public static readonly Unicolour BlueWithAHintOfPurple = new(Config, "#533cc6");
    public static readonly Unicolour BlueyGreen = new(Config, "#2bb179");
    public static readonly Unicolour BlueyGrey = new(Config, "#89a0b0");
    public static readonly Unicolour BlueyPurple = new(Config, "#6241c7");
    public static readonly Unicolour Bluish = new(Config, "#2976bb");
    public static readonly Unicolour BluishGreen = new(Config, "#10a674");
    public static readonly Unicolour BluishGrey = new(Config, "#748b97");
    public static readonly Unicolour BluishPurple = new(Config, "#703be7");
    public static readonly Unicolour Blurple = new(Config, "#5539cc");
    public static readonly Unicolour Blush = new(Config, "#f29e8e");
    public static readonly Unicolour BlushPink = new(Config, "#fe828c");
    public static readonly Unicolour Booger = new(Config, "#9bb53c");
    public static readonly Unicolour BoogerGreen = new(Config, "#96b403");
    public static readonly Unicolour Bordeaux = new(Config, "#7b002c");
    public static readonly Unicolour BoringGreen = new(Config, "#63b365");
    public static readonly Unicolour BottleGreen = new(Config, "#044a05");
    public static readonly Unicolour Brick = new(Config, "#a03623");
    public static readonly Unicolour BrickOrange = new(Config, "#c14a09");
    public static readonly Unicolour BrickRed = new(Config, "#8f1402");
    public static readonly Unicolour BrightAqua = new(Config, "#0bf9ea");
    public static readonly Unicolour BrightBlue = new(Config, "#0165fc");
    public static readonly Unicolour BrightCyan = new(Config, "#41fdfe");
    public static readonly Unicolour BrightGreen = new(Config, "#01ff07");
    public static readonly Unicolour BrightLavender = new(Config, "#c760ff");
    public static readonly Unicolour BrightLightBlue = new(Config, "#26f7fd");
    public static readonly Unicolour BrightLightGreen = new(Config, "#2dfe54");
    public static readonly Unicolour BrightLilac = new(Config, "#c95efb");
    public static readonly Unicolour BrightLime = new(Config, "#87fd05");
    public static readonly Unicolour BrightLimeGreen = new(Config, "#65fe08");
    public static readonly Unicolour BrightMagenta = new(Config, "#ff08e8");
    public static readonly Unicolour BrightOlive = new(Config, "#9cbb04");
    public static readonly Unicolour BrightOrange = new(Config, "#ff5b00");
    public static readonly Unicolour BrightPink = new(Config, "#fe01b1");
    public static readonly Unicolour BrightPurple = new(Config, "#be03fd");
    public static readonly Unicolour BrightRed = new(Config, "#ff000d");
    public static readonly Unicolour BrightSeaGreen = new(Config, "#05ffa6");
    public static readonly Unicolour BrightSkyBlue = new(Config, "#02ccfe");
    public static readonly Unicolour BrightTeal = new(Config, "#01f9c6");
    public static readonly Unicolour BrightTurquoise = new(Config, "#0ffef9");
    public static readonly Unicolour BrightViolet = new(Config, "#ad0afd");
    public static readonly Unicolour BrightYellow = new(Config, "#fffd01");
    public static readonly Unicolour BrightYellowGreen = new(Config, "#9dff00");
    public static readonly Unicolour BritishRacingGreen = new(Config, "#05480d");
    public static readonly Unicolour Bronze = new(Config, "#a87900");
    public static readonly Unicolour Brown = new(Config, "#653700");
    public static readonly Unicolour BrownGreen = new(Config, "#706c11");
    public static readonly Unicolour BrownGrey = new(Config, "#8d8468");
    public static readonly Unicolour Brownish = new(Config, "#9c6d57");
    public static readonly Unicolour BrownishGreen = new(Config, "#6a6e09");
    public static readonly Unicolour BrownishGrey = new(Config, "#86775f");
    public static readonly Unicolour BrownishOrange = new(Config, "#cb7723");
    public static readonly Unicolour BrownishPink = new(Config, "#c27e79");
    public static readonly Unicolour BrownishPurple = new(Config, "#76424e");
    public static readonly Unicolour BrownishRed = new(Config, "#9e3623");
    public static readonly Unicolour BrownishYellow = new(Config, "#c9b003");
    public static readonly Unicolour BrownOrange = new(Config, "#b96902");
    public static readonly Unicolour BrownRed = new(Config, "#922b05");
    public static readonly Unicolour BrownYellow = new(Config, "#b29705");
    public static readonly Unicolour BrownyGreen = new(Config, "#6f6c0a");
    public static readonly Unicolour BrownyOrange = new(Config, "#ca6b02");
    public static readonly Unicolour Bruise = new(Config, "#7e4071");
    public static readonly Unicolour Bubblegum = new(Config, "#ff6cb5");
    public static readonly Unicolour BubblegumPink = new(Config, "#fe83cc");
    public static readonly Unicolour BubbleGumPink = new(Config, "#ff69af");
    public static readonly Unicolour Buff = new(Config, "#fef69e");
    public static readonly Unicolour Burgundy = new(Config, "#610023");
    public static readonly Unicolour BurntOrange = new(Config, "#c04e01");
    public static readonly Unicolour BurntRed = new(Config, "#9f2305");
    public static readonly Unicolour BurntSiena = new(Config, "#b75203");
    public static readonly Unicolour BurntSienna = new(Config, "#b04e0f");
    public static readonly Unicolour BurntUmber = new(Config, "#a0450e");
    public static readonly Unicolour BurntYellow = new(Config, "#d5ab09");
    public static readonly Unicolour Burple = new(Config, "#6832e3");
    public static readonly Unicolour Butter = new(Config, "#ffff81");
    public static readonly Unicolour Butterscotch = new(Config, "#fdb147");
    public static readonly Unicolour ButterYellow = new(Config, "#fffd74");
    public static readonly Unicolour CadetBlue = new(Config, "#4e7496");
    public static readonly Unicolour Camel = new(Config, "#c69f59");
    public static readonly Unicolour Camo = new(Config, "#7f8f4e");
    public static readonly Unicolour CamoGreen = new(Config, "#526525");
    public static readonly Unicolour CamouflageGreen = new(Config, "#4b6113");
    public static readonly Unicolour Canary = new(Config, "#fdff63");
    public static readonly Unicolour CanaryYellow = new(Config, "#fffe40");
    public static readonly Unicolour CandyPink = new(Config, "#ff63e9");
    public static readonly Unicolour Caramel = new(Config, "#af6f09");
    public static readonly Unicolour Carmine = new(Config, "#9d0216");
    public static readonly Unicolour Carnation = new(Config, "#fd798f");
    public static readonly Unicolour CarnationPink = new(Config, "#ff7fa7");
    public static readonly Unicolour CarolinaBlue = new(Config, "#8ab8fe");
    public static readonly Unicolour Celadon = new(Config, "#befdb7");
    public static readonly Unicolour Celery = new(Config, "#c1fd95");
    public static readonly Unicolour Cement = new(Config, "#a5a391");
    public static readonly Unicolour Cerise = new(Config, "#de0c62");
    public static readonly Unicolour Cerulean = new(Config, "#0485d1");
    public static readonly Unicolour CeruleanBlue = new(Config, "#056eee");
    public static readonly Unicolour Charcoal = new(Config, "#343837");
    public static readonly Unicolour CharcoalGrey = new(Config, "#3c4142");
    public static readonly Unicolour Chartreuse = new(Config, "#c1f80a");
    public static readonly Unicolour Cherry = new(Config, "#cf0234");
    public static readonly Unicolour CherryRed = new(Config, "#f7022a");
    public static readonly Unicolour Chestnut = new(Config, "#742802");
    public static readonly Unicolour Chocolate = new(Config, "#3d1c02");
    public static readonly Unicolour ChocolateBrown = new(Config, "#411900");
    public static readonly Unicolour Cinnamon = new(Config, "#ac4f06");
    public static readonly Unicolour Claret = new(Config, "#680018");
    public static readonly Unicolour Clay = new(Config, "#b66a50");
    public static readonly Unicolour ClayBrown = new(Config, "#b2713d");
    public static readonly Unicolour ClearBlue = new(Config, "#247afd");
    public static readonly Unicolour CloudyBlue = new(Config, "#acc2d9");
    public static readonly Unicolour Cobalt = new(Config, "#1e488f");
    public static readonly Unicolour CobaltBlue = new(Config, "#030aa7");
    public static readonly Unicolour Cocoa = new(Config, "#875f42");
    public static readonly Unicolour Coffee = new(Config, "#a6814c");
    public static readonly Unicolour CoolBlue = new(Config, "#4984b8");
    public static readonly Unicolour CoolGreen = new(Config, "#33b864");
    public static readonly Unicolour CoolGrey = new(Config, "#95a3a6");
    public static readonly Unicolour Copper = new(Config, "#b66325");
    public static readonly Unicolour Coral = new(Config, "#fc5a50");
    public static readonly Unicolour CoralPink = new(Config, "#ff6163");
    public static readonly Unicolour Cornflower = new(Config, "#6a79f7");
    public static readonly Unicolour CornflowerBlue = new(Config, "#5170d7");
    public static readonly Unicolour Cranberry = new(Config, "#9e003a");
    public static readonly Unicolour Cream = new(Config, "#ffffc2");
    public static readonly Unicolour Creme = new(Config, "#ffffb6");
    public static readonly Unicolour Crimson = new(Config, "#8c000f");
    public static readonly Unicolour Custard = new(Config, "#fffd78");
    public static readonly Unicolour Cyan = new(Config, "#00ffff");
    public static readonly Unicolour Dandelion = new(Config, "#fedf08");
    public static readonly Unicolour Dark = new(Config, "#1b2431");
    public static readonly Unicolour DarkAqua = new(Config, "#05696b");
    public static readonly Unicolour DarkAquamarine = new(Config, "#017371");
    public static readonly Unicolour DarkBeige = new(Config, "#ac9362");
    public static readonly Unicolour DarkBlue = new(Config, "#00035b");
    public static readonly Unicolour Darkblue = new(Config, "#030764");
    public static readonly Unicolour DarkBlueGreen = new(Config, "#005249");
    public static readonly Unicolour DarkBlueGrey = new(Config, "#1f3b4d");
    public static readonly Unicolour DarkBrown = new(Config, "#341c02");
    public static readonly Unicolour DarkCoral = new(Config, "#cf524e");
    public static readonly Unicolour DarkCream = new(Config, "#fff39a");
    public static readonly Unicolour DarkCyan = new(Config, "#0a888a");
    public static readonly Unicolour DarkForestGreen = new(Config, "#002d04");
    public static readonly Unicolour DarkFuchsia = new(Config, "#9d0759");
    public static readonly Unicolour DarkGold = new(Config, "#b59410");
    public static readonly Unicolour DarkGrassGreen = new(Config, "#388004");
    public static readonly Unicolour DarkGreen = new(Config, "#033500");
    public static readonly Unicolour Darkgreen = new(Config, "#054907");
    public static readonly Unicolour DarkGreenBlue = new(Config, "#1f6357");
    public static readonly Unicolour DarkGrey = new(Config, "#363737");
    public static readonly Unicolour DarkGreyBlue = new(Config, "#29465b");
    public static readonly Unicolour DarkHotPink = new(Config, "#d90166");
    public static readonly Unicolour DarkIndigo = new(Config, "#1f0954");
    public static readonly Unicolour DarkishBlue = new(Config, "#014182");
    public static readonly Unicolour DarkishGreen = new(Config, "#287c37");
    public static readonly Unicolour DarkishPink = new(Config, "#da467d");
    public static readonly Unicolour DarkishPurple = new(Config, "#751973");
    public static readonly Unicolour DarkishRed = new(Config, "#a90308");
    public static readonly Unicolour DarkKhaki = new(Config, "#9b8f55");
    public static readonly Unicolour DarkLavender = new(Config, "#856798");
    public static readonly Unicolour DarkLilac = new(Config, "#9c6da5");
    public static readonly Unicolour DarkLime = new(Config, "#84b701");
    public static readonly Unicolour DarkLimeGreen = new(Config, "#7ebd01");
    public static readonly Unicolour DarkMagenta = new(Config, "#960056");
    public static readonly Unicolour DarkMaroon = new(Config, "#3c0008");
    public static readonly Unicolour DarkMauve = new(Config, "#874c62");
    public static readonly Unicolour DarkMint = new(Config, "#48c072");
    public static readonly Unicolour DarkMintGreen = new(Config, "#20c073");
    public static readonly Unicolour DarkMustard = new(Config, "#a88905");
    public static readonly Unicolour DarkNavy = new(Config, "#000435");
    public static readonly Unicolour DarkNavyBlue = new(Config, "#00022e");
    public static readonly Unicolour DarkOlive = new(Config, "#373e02");
    public static readonly Unicolour DarkOliveGreen = new(Config, "#3c4d03");
    public static readonly Unicolour DarkOrange = new(Config, "#c65102");
    public static readonly Unicolour DarkPastelGreen = new(Config, "#56ae57");
    public static readonly Unicolour DarkPeach = new(Config, "#de7e5d");
    public static readonly Unicolour DarkPeriwinkle = new(Config, "#665fd1");
    public static readonly Unicolour DarkPink = new(Config, "#cb416b");
    public static readonly Unicolour DarkPlum = new(Config, "#3f012c");
    public static readonly Unicolour DarkPurple = new(Config, "#35063e");
    public static readonly Unicolour DarkRed = new(Config, "#840000");
    public static readonly Unicolour DarkRose = new(Config, "#b5485d");
    public static readonly Unicolour DarkRoyalBlue = new(Config, "#02066f");
    public static readonly Unicolour DarkSage = new(Config, "#598556");
    public static readonly Unicolour DarkSalmon = new(Config, "#c85a53");
    public static readonly Unicolour DarkSand = new(Config, "#a88f59");
    public static readonly Unicolour DarkSeafoam = new(Config, "#1fb57a");
    public static readonly Unicolour DarkSeafoamGreen = new(Config, "#3eaf76");
    public static readonly Unicolour DarkSeaGreen = new(Config, "#11875d");
    public static readonly Unicolour DarkSkyBlue = new(Config, "#448ee4");
    public static readonly Unicolour DarkSlateBlue = new(Config, "#214761");
    public static readonly Unicolour DarkTan = new(Config, "#af884a");
    public static readonly Unicolour DarkTaupe = new(Config, "#7f684e");
    public static readonly Unicolour DarkTeal = new(Config, "#014d4e");
    public static readonly Unicolour DarkTurquoise = new(Config, "#045c5a");
    public static readonly Unicolour DarkViolet = new(Config, "#34013f");
    public static readonly Unicolour DarkYellow = new(Config, "#d5b60a");
    public static readonly Unicolour DarkYellowGreen = new(Config, "#728f02");
    public static readonly Unicolour DeepAqua = new(Config, "#08787f");
    public static readonly Unicolour DeepBlue = new(Config, "#040273");
    public static readonly Unicolour DeepBrown = new(Config, "#410200");
    public static readonly Unicolour DeepGreen = new(Config, "#02590f");
    public static readonly Unicolour DeepLavender = new(Config, "#8d5eb7");
    public static readonly Unicolour DeepLilac = new(Config, "#966ebd");
    public static readonly Unicolour DeepMagenta = new(Config, "#a0025c");
    public static readonly Unicolour DeepOrange = new(Config, "#dc4d01");
    public static readonly Unicolour DeepPink = new(Config, "#cb0162");
    public static readonly Unicolour DeepPurple = new(Config, "#36013f");
    public static readonly Unicolour DeepRed = new(Config, "#9a0200");
    public static readonly Unicolour DeepRose = new(Config, "#c74767");
    public static readonly Unicolour DeepSeaBlue = new(Config, "#015482");
    public static readonly Unicolour DeepSkyBlue = new(Config, "#0d75f8");
    public static readonly Unicolour DeepTeal = new(Config, "#00555a");
    public static readonly Unicolour DeepTurquoise = new(Config, "#017374");
    public static readonly Unicolour DeepViolet = new(Config, "#490648");
    public static readonly Unicolour Denim = new(Config, "#3b638c");
    public static readonly Unicolour DenimBlue = new(Config, "#3b5b92");
    public static readonly Unicolour Desert = new(Config, "#ccad60");
    public static readonly Unicolour Diarrhea = new(Config, "#9f8303");
    public static readonly Unicolour Dirt = new(Config, "#8a6e45");
    public static readonly Unicolour DirtBrown = new(Config, "#836539");
    public static readonly Unicolour DirtyBlue = new(Config, "#3f829d");
    public static readonly Unicolour DirtyGreen = new(Config, "#667e2c");
    public static readonly Unicolour DirtyOrange = new(Config, "#c87606");
    public static readonly Unicolour DirtyPink = new(Config, "#ca7b80");
    public static readonly Unicolour DirtyPurple = new(Config, "#734a65");
    public static readonly Unicolour DirtyYellow = new(Config, "#cdc50a");
    public static readonly Unicolour DodgerBlue = new(Config, "#3e82fc");
    public static readonly Unicolour Drab = new(Config, "#828344");
    public static readonly Unicolour DrabGreen = new(Config, "#749551");
    public static readonly Unicolour DriedBlood = new(Config, "#4b0101");
    public static readonly Unicolour DuckEggBlue = new(Config, "#c3fbf4");
    public static readonly Unicolour DullBlue = new(Config, "#49759c");
    public static readonly Unicolour DullBrown = new(Config, "#876e4b");
    public static readonly Unicolour DullGreen = new(Config, "#74a662");
    public static readonly Unicolour DullOrange = new(Config, "#d8863b");
    public static readonly Unicolour DullPink = new(Config, "#d5869d");
    public static readonly Unicolour DullPurple = new(Config, "#84597e");
    public static readonly Unicolour DullRed = new(Config, "#bb3f3f");
    public static readonly Unicolour DullTeal = new(Config, "#5f9e8f");
    public static readonly Unicolour DullYellow = new(Config, "#eedc5b");
    public static readonly Unicolour Dusk = new(Config, "#4e5481");
    public static readonly Unicolour DuskBlue = new(Config, "#26538d");
    public static readonly Unicolour DuskyBlue = new(Config, "#475f94");
    public static readonly Unicolour DuskyPink = new(Config, "#cc7a8b");
    public static readonly Unicolour DuskyPurple = new(Config, "#895b7b");
    public static readonly Unicolour DuskyRose = new(Config, "#ba6873");
    public static readonly Unicolour Dust = new(Config, "#b2996e");
    public static readonly Unicolour DustyBlue = new(Config, "#5a86ad");
    public static readonly Unicolour DustyGreen = new(Config, "#76a973");
    public static readonly Unicolour DustyLavender = new(Config, "#ac86a8");
    public static readonly Unicolour DustyOrange = new(Config, "#f0833a");
    public static readonly Unicolour DustyPink = new(Config, "#d58a94");
    public static readonly Unicolour DustyPurple = new(Config, "#825f87");
    public static readonly Unicolour DustyRed = new(Config, "#b9484e");
    public static readonly Unicolour DustyRose = new(Config, "#c0737a");
    public static readonly Unicolour DustyTeal = new(Config, "#4c9085");
    public static readonly Unicolour Earth = new(Config, "#a2653e");
    public static readonly Unicolour EasterGreen = new(Config, "#8cfd7e");
    public static readonly Unicolour EasterPurple = new(Config, "#c071fe");
    public static readonly Unicolour Ecru = new(Config, "#feffca");
    public static readonly Unicolour Eggplant = new(Config, "#380835");
    public static readonly Unicolour EggplantPurple = new(Config, "#430541");
    public static readonly Unicolour EggShell = new(Config, "#fffcc4");
    public static readonly Unicolour Eggshell = new(Config, "#ffffd4");
    public static readonly Unicolour EggshellBlue = new(Config, "#c4fff7");
    public static readonly Unicolour ElectricBlue = new(Config, "#0652ff");
    public static readonly Unicolour ElectricGreen = new(Config, "#21fc0d");
    public static readonly Unicolour ElectricLime = new(Config, "#a8ff04");
    public static readonly Unicolour ElectricPink = new(Config, "#ff0490");
    public static readonly Unicolour ElectricPurple = new(Config, "#aa23ff");
    public static readonly Unicolour Emerald = new(Config, "#01a049");
    public static readonly Unicolour EmeraldGreen = new(Config, "#028f1e");
    public static readonly Unicolour Evergreen = new(Config, "#05472a");
    public static readonly Unicolour FadedBlue = new(Config, "#658cbb");
    public static readonly Unicolour FadedGreen = new(Config, "#7bb274");
    public static readonly Unicolour FadedOrange = new(Config, "#f0944d");
    public static readonly Unicolour FadedPink = new(Config, "#de9dac");
    public static readonly Unicolour FadedPurple = new(Config, "#916e99");
    public static readonly Unicolour FadedRed = new(Config, "#d3494e");
    public static readonly Unicolour FadedYellow = new(Config, "#feff7f");
    public static readonly Unicolour Fawn = new(Config, "#cfaf7b");
    public static readonly Unicolour Fern = new(Config, "#63a950");
    public static readonly Unicolour FernGreen = new(Config, "#548d44");
    public static readonly Unicolour FireEngineRed = new(Config, "#fe0002");
    public static readonly Unicolour FlatBlue = new(Config, "#3c73a8");
    public static readonly Unicolour FlatGreen = new(Config, "#699d4c");
    public static readonly Unicolour FluorescentGreen = new(Config, "#08ff08");
    public static readonly Unicolour FluroGreen = new(Config, "#0aff02");
    public static readonly Unicolour FoamGreen = new(Config, "#90fda9");
    public static readonly Unicolour Forest = new(Config, "#0b5509");
    public static readonly Unicolour ForestGreen = new(Config, "#06470c");
    public static readonly Unicolour ForrestGreen = new(Config, "#154406");
    public static readonly Unicolour FrenchBlue = new(Config, "#436bad");
    public static readonly Unicolour FreshGreen = new(Config, "#69d84f");
    public static readonly Unicolour FrogGreen = new(Config, "#58bc08");
    public static readonly Unicolour Fuchsia = new(Config, "#ed0dd9");
    public static readonly Unicolour Gold = new(Config, "#dbb40c");
    public static readonly Unicolour Golden = new(Config, "#f5bf03");
    public static readonly Unicolour GoldenBrown = new(Config, "#b27a01");
    public static readonly Unicolour GoldenRod = new(Config, "#f9bc08");
    public static readonly Unicolour Goldenrod = new(Config, "#fac205");
    public static readonly Unicolour GoldenYellow = new(Config, "#fec615");
    public static readonly Unicolour Grape = new(Config, "#6c3461");
    public static readonly Unicolour Grapefruit = new(Config, "#fd5956");
    public static readonly Unicolour GrapePurple = new(Config, "#5d1451");
    public static readonly Unicolour Grass = new(Config, "#5cac2d");
    public static readonly Unicolour GrassGreen = new(Config, "#3f9b0b");
    public static readonly Unicolour GrassyGreen = new(Config, "#419c03");
    public static readonly Unicolour Green = new(Config, "#15b01a");
    public static readonly Unicolour Green_Blue = new(Config, "#01c08d");
    public static readonly Unicolour Green_Yellow = new(Config, "#b5ce08");
    public static readonly Unicolour GreenApple = new(Config, "#5edc1f");
    public static readonly Unicolour GreenBlue = new(Config, "#06b48b");
    public static readonly Unicolour Greenblue = new(Config, "#23c48b");
    public static readonly Unicolour GreenBrown = new(Config, "#544e03");
    public static readonly Unicolour GreenGrey = new(Config, "#77926f");
    public static readonly Unicolour Greenish = new(Config, "#40a368");
    public static readonly Unicolour GreenishBeige = new(Config, "#c9d179");
    public static readonly Unicolour GreenishBlue = new(Config, "#0b8b87");
    public static readonly Unicolour GreenishBrown = new(Config, "#696112");
    public static readonly Unicolour GreenishCyan = new(Config, "#2afeb7");
    public static readonly Unicolour GreenishGrey = new(Config, "#96ae8d");
    public static readonly Unicolour GreenishTan = new(Config, "#bccb7a");
    public static readonly Unicolour GreenishTeal = new(Config, "#32bf84");
    public static readonly Unicolour GreenishTurquoise = new(Config, "#00fbb0");
    public static readonly Unicolour GreenishYellow = new(Config, "#cdfd02");
    public static readonly Unicolour GreenTeal = new(Config, "#0cb577");
    public static readonly Unicolour GreenyBlue = new(Config, "#42b395");
    public static readonly Unicolour GreenyBrown = new(Config, "#696006");
    public static readonly Unicolour GreenYellow = new(Config, "#c9ff27");
    public static readonly Unicolour GreenyGrey = new(Config, "#7ea07a");
    public static readonly Unicolour GreenyYellow = new(Config, "#c6f808");
    public static readonly Unicolour Grey = new(Config, "#929591");
    public static readonly Unicolour Grey_Blue = new(Config, "#647d8e");
    public static readonly Unicolour Grey_Green = new(Config, "#86a17d");
    public static readonly Unicolour GreyBlue = new(Config, "#6b8ba4");
    public static readonly Unicolour Greyblue = new(Config, "#77a1b5");
    public static readonly Unicolour GreyBrown = new(Config, "#7f7053");
    public static readonly Unicolour GreyGreen = new(Config, "#789b73");
    public static readonly Unicolour Greyish = new(Config, "#a8a495");
    public static readonly Unicolour GreyishBlue = new(Config, "#5e819d");
    public static readonly Unicolour GreyishBrown = new(Config, "#7a6a4f");
    public static readonly Unicolour GreyishGreen = new(Config, "#82a67d");
    public static readonly Unicolour GreyishPink = new(Config, "#c88d94");
    public static readonly Unicolour GreyishPurple = new(Config, "#887191");
    public static readonly Unicolour GreyishTeal = new(Config, "#719f91");
    public static readonly Unicolour GreyPink = new(Config, "#c3909b");
    public static readonly Unicolour GreyPurple = new(Config, "#826d8c");
    public static readonly Unicolour GreyTeal = new(Config, "#5e9b8a");
    public static readonly Unicolour GrossGreen = new(Config, "#a0bf16");
    public static readonly Unicolour Gunmetal = new(Config, "#536267");
    public static readonly Unicolour Hazel = new(Config, "#8e7618");
    public static readonly Unicolour Heather = new(Config, "#a484ac");
    public static readonly Unicolour Heliotrope = new(Config, "#d94ff5");
    public static readonly Unicolour HighlighterGreen = new(Config, "#1bfc06");
    public static readonly Unicolour HospitalGreen = new(Config, "#9be5aa");
    public static readonly Unicolour HotGreen = new(Config, "#25ff29");
    public static readonly Unicolour HotMagenta = new(Config, "#f504c9");
    public static readonly Unicolour HotPink = new(Config, "#ff028d");
    public static readonly Unicolour HotPurple = new(Config, "#cb00f5");
    public static readonly Unicolour HunterGreen = new(Config, "#0b4008");
    public static readonly Unicolour Ice = new(Config, "#d6fffa");
    public static readonly Unicolour IceBlue = new(Config, "#d7fffe");
    public static readonly Unicolour IckyGreen = new(Config, "#8fae22");
    public static readonly Unicolour IndianRed = new(Config, "#850e04");
    public static readonly Unicolour Indigo = new(Config, "#380282");
    public static readonly Unicolour IndigoBlue = new(Config, "#3a18b1");
    public static readonly Unicolour Iris = new(Config, "#6258c4");
    public static readonly Unicolour IrishGreen = new(Config, "#019529");
    public static readonly Unicolour Ivory = new(Config, "#ffffcb");
    public static readonly Unicolour Jade = new(Config, "#1fa774");
    public static readonly Unicolour JadeGreen = new(Config, "#2baf6a");
    public static readonly Unicolour JungleGreen = new(Config, "#048243");
    public static readonly Unicolour KelleyGreen = new(Config, "#009337");
    public static readonly Unicolour KellyGreen = new(Config, "#02ab2e");
    public static readonly Unicolour KermitGreen = new(Config, "#5cb200");
    public static readonly Unicolour KeyLime = new(Config, "#aeff6e");
    public static readonly Unicolour Khaki = new(Config, "#aaa662");
    public static readonly Unicolour KhakiGreen = new(Config, "#728639");
    public static readonly Unicolour Kiwi = new(Config, "#9cef43");
    public static readonly Unicolour KiwiGreen = new(Config, "#8ee53f");
    public static readonly Unicolour Lavender = new(Config, "#c79fef");
    public static readonly Unicolour LavenderBlue = new(Config, "#8b88f8");
    public static readonly Unicolour LavenderPink = new(Config, "#dd85d7");
    public static readonly Unicolour LawnGreen = new(Config, "#4da409");
    public static readonly Unicolour Leaf = new(Config, "#71aa34");
    public static readonly Unicolour LeafGreen = new(Config, "#5ca904");
    public static readonly Unicolour LeafyGreen = new(Config, "#51b73b");
    public static readonly Unicolour Leather = new(Config, "#ac7434");
    public static readonly Unicolour Lemon = new(Config, "#fdff52");
    public static readonly Unicolour LemonGreen = new(Config, "#adf802");
    public static readonly Unicolour LemonLime = new(Config, "#bffe28");
    public static readonly Unicolour LemonYellow = new(Config, "#fdff38");
    public static readonly Unicolour Lichen = new(Config, "#8fb67b");
    public static readonly Unicolour LightAqua = new(Config, "#8cffdb");
    public static readonly Unicolour LightAquamarine = new(Config, "#7bfdc7");
    public static readonly Unicolour LightBeige = new(Config, "#fffeb6");
    public static readonly Unicolour Lightblue = new(Config, "#7bc8f6");
    public static readonly Unicolour LightBlue = new(Config, "#95d0fc");
    public static readonly Unicolour LightBlueGreen = new(Config, "#7efbb3");
    public static readonly Unicolour LightBlueGrey = new(Config, "#b7c9e2");
    public static readonly Unicolour LightBluishGreen = new(Config, "#76fda8");
    public static readonly Unicolour LightBrightGreen = new(Config, "#53fe5c");
    public static readonly Unicolour LightBrown = new(Config, "#ad8150");
    public static readonly Unicolour LightBurgundy = new(Config, "#a8415b");
    public static readonly Unicolour LightCyan = new(Config, "#acfffc");
    public static readonly Unicolour LightEggplant = new(Config, "#894585");
    public static readonly Unicolour LighterGreen = new(Config, "#75fd63");
    public static readonly Unicolour LighterPurple = new(Config, "#a55af4");
    public static readonly Unicolour LightForestGreen = new(Config, "#4f9153");
    public static readonly Unicolour LightGold = new(Config, "#fddc5c");
    public static readonly Unicolour LightGrassGreen = new(Config, "#9af764");
    public static readonly Unicolour Lightgreen = new(Config, "#76ff7b");
    public static readonly Unicolour LightGreen = new(Config, "#96f97b");
    public static readonly Unicolour LightGreenBlue = new(Config, "#56fca2");
    public static readonly Unicolour LightGreenishBlue = new(Config, "#63f7b4");
    public static readonly Unicolour LightGrey = new(Config, "#d8dcd6");
    public static readonly Unicolour LightGreyBlue = new(Config, "#9dbcd4");
    public static readonly Unicolour LightGreyGreen = new(Config, "#b7e1a1");
    public static readonly Unicolour LightIndigo = new(Config, "#6d5acf");
    public static readonly Unicolour LightishBlue = new(Config, "#3d7afd");
    public static readonly Unicolour LightishGreen = new(Config, "#61e160");
    public static readonly Unicolour LightishPurple = new(Config, "#a552e6");
    public static readonly Unicolour LightishRed = new(Config, "#fe2f4a");
    public static readonly Unicolour LightKhaki = new(Config, "#e6f2a2");
    public static readonly Unicolour LightLavendar = new(Config, "#efc0fe");
    public static readonly Unicolour LightLavender = new(Config, "#dfc5fe");
    public static readonly Unicolour LightLightBlue = new(Config, "#cafffb");
    public static readonly Unicolour LightLightGreen = new(Config, "#c8ffb0");
    public static readonly Unicolour LightLilac = new(Config, "#edc8ff");
    public static readonly Unicolour LightLime = new(Config, "#aefd6c");
    public static readonly Unicolour LightLimeGreen = new(Config, "#b9ff66");
    public static readonly Unicolour LightMagenta = new(Config, "#fa5ff7");
    public static readonly Unicolour LightMaroon = new(Config, "#a24857");
    public static readonly Unicolour LightMauve = new(Config, "#c292a1");
    public static readonly Unicolour LightMint = new(Config, "#b6ffbb");
    public static readonly Unicolour LightMintGreen = new(Config, "#a6fbb2");
    public static readonly Unicolour LightMossGreen = new(Config, "#a6c875");
    public static readonly Unicolour LightMustard = new(Config, "#f7d560");
    public static readonly Unicolour LightNavy = new(Config, "#155084");
    public static readonly Unicolour LightNavyBlue = new(Config, "#2e5a88");
    public static readonly Unicolour LightNeonGreen = new(Config, "#4efd54");
    public static readonly Unicolour LightOlive = new(Config, "#acbf69");
    public static readonly Unicolour LightOliveGreen = new(Config, "#a4be5c");
    public static readonly Unicolour LightOrange = new(Config, "#fdaa48");
    public static readonly Unicolour LightPastelGreen = new(Config, "#b2fba5");
    public static readonly Unicolour LightPeach = new(Config, "#ffd8b1");
    public static readonly Unicolour LightPeaGreen = new(Config, "#c4fe82");
    public static readonly Unicolour LightPeriwinkle = new(Config, "#c1c6fc");
    public static readonly Unicolour LightPink = new(Config, "#ffd1df");
    public static readonly Unicolour LightPlum = new(Config, "#9d5783");
    public static readonly Unicolour LightPurple = new(Config, "#bf77f6");
    public static readonly Unicolour LightRed = new(Config, "#ff474c");
    public static readonly Unicolour LightRose = new(Config, "#ffc5cb");
    public static readonly Unicolour LightRoyalBlue = new(Config, "#3a2efe");
    public static readonly Unicolour LightSage = new(Config, "#bcecac");
    public static readonly Unicolour LightSalmon = new(Config, "#fea993");
    public static readonly Unicolour LightSeafoam = new(Config, "#a0febf");
    public static readonly Unicolour LightSeafoamGreen = new(Config, "#a7ffb5");
    public static readonly Unicolour LightSeaGreen = new(Config, "#98f6b0");
    public static readonly Unicolour LightSkyBlue = new(Config, "#c6fcff");
    public static readonly Unicolour LightTan = new(Config, "#fbeeac");
    public static readonly Unicolour LightTeal = new(Config, "#90e4c1");
    public static readonly Unicolour LightTurquoise = new(Config, "#7ef4cc");
    public static readonly Unicolour LightUrple = new(Config, "#b36ff6");
    public static readonly Unicolour LightViolet = new(Config, "#d6b4fc");
    public static readonly Unicolour LightYellow = new(Config, "#fffe7a");
    public static readonly Unicolour LightYellowGreen = new(Config, "#ccfd7f");
    public static readonly Unicolour LightYellowishGreen = new(Config, "#c2ff89");
    public static readonly Unicolour Lilac = new(Config, "#cea2fd");
    public static readonly Unicolour Liliac = new(Config, "#c48efd");
    public static readonly Unicolour Lime = new(Config, "#aaff32");
    public static readonly Unicolour LimeGreen = new(Config, "#89fe05");
    public static readonly Unicolour LimeYellow = new(Config, "#d0fe1d");
    public static readonly Unicolour Lipstick = new(Config, "#d5174e");
    public static readonly Unicolour LipstickRed = new(Config, "#c0022f");
    public static readonly Unicolour MacaroniAndCheese = new(Config, "#efb435");
    public static readonly Unicolour Magenta = new(Config, "#c20078");
    public static readonly Unicolour Mahogany = new(Config, "#4a0100");
    public static readonly Unicolour Maize = new(Config, "#f4d054");
    public static readonly Unicolour Mango = new(Config, "#ffa62b");
    public static readonly Unicolour Manilla = new(Config, "#fffa86");
    public static readonly Unicolour Marigold = new(Config, "#fcc006");
    public static readonly Unicolour Marine = new(Config, "#042e60");
    public static readonly Unicolour MarineBlue = new(Config, "#01386a");
    public static readonly Unicolour Maroon = new(Config, "#650021");
    public static readonly Unicolour Mauve = new(Config, "#ae7181");
    public static readonly Unicolour MediumBlue = new(Config, "#2c6fbb");
    public static readonly Unicolour MediumBrown = new(Config, "#7f5112");
    public static readonly Unicolour MediumGreen = new(Config, "#39ad48");
    public static readonly Unicolour MediumGrey = new(Config, "#7d7f7c");
    public static readonly Unicolour MediumPink = new(Config, "#f36196");
    public static readonly Unicolour MediumPurple = new(Config, "#9e43a2");
    public static readonly Unicolour Melon = new(Config, "#ff7855");
    public static readonly Unicolour Merlot = new(Config, "#730039");
    public static readonly Unicolour MetallicBlue = new(Config, "#4f738e");
    public static readonly Unicolour MidBlue = new(Config, "#276ab3");
    public static readonly Unicolour MidGreen = new(Config, "#50a747");
    public static readonly Unicolour Midnight = new(Config, "#03012d");
    public static readonly Unicolour MidnightBlue = new(Config, "#020035");
    public static readonly Unicolour MidnightPurple = new(Config, "#280137");
    public static readonly Unicolour MilitaryGreen = new(Config, "#667c3e");
    public static readonly Unicolour MilkChocolate = new(Config, "#7f4e1e");
    public static readonly Unicolour Mint = new(Config, "#9ffeb0");
    public static readonly Unicolour MintGreen = new(Config, "#8fff9f");
    public static readonly Unicolour MintyGreen = new(Config, "#0bf77d");
    public static readonly Unicolour Mocha = new(Config, "#9d7651");
    public static readonly Unicolour Moss = new(Config, "#769958");
    public static readonly Unicolour MossGreen = new(Config, "#658b38");
    public static readonly Unicolour MossyGreen = new(Config, "#638b27");
    public static readonly Unicolour Mud = new(Config, "#735c12");
    public static readonly Unicolour MudBrown = new(Config, "#60460f");
    public static readonly Unicolour MuddyBrown = new(Config, "#886806");
    public static readonly Unicolour MuddyGreen = new(Config, "#657432");
    public static readonly Unicolour MuddyYellow = new(Config, "#bfac05");
    public static readonly Unicolour MudGreen = new(Config, "#606602");
    public static readonly Unicolour Mulberry = new(Config, "#920a4e");
    public static readonly Unicolour MurkyGreen = new(Config, "#6c7a0e");
    public static readonly Unicolour Mushroom = new(Config, "#ba9e88");
    public static readonly Unicolour Mustard = new(Config, "#ceb301");
    public static readonly Unicolour MustardBrown = new(Config, "#ac7e04");
    public static readonly Unicolour MustardGreen = new(Config, "#a8b504");
    public static readonly Unicolour MustardYellow = new(Config, "#d2bd0a");
    public static readonly Unicolour MutedBlue = new(Config, "#3b719f");
    public static readonly Unicolour MutedGreen = new(Config, "#5fa052");
    public static readonly Unicolour MutedPink = new(Config, "#d1768f");
    public static readonly Unicolour MutedPurple = new(Config, "#805b87");
    public static readonly Unicolour NastyGreen = new(Config, "#70b23f");
    public static readonly Unicolour Navy = new(Config, "#01153e");
    public static readonly Unicolour NavyBlue = new(Config, "#001146");
    public static readonly Unicolour NavyGreen = new(Config, "#35530a");
    public static readonly Unicolour NeonBlue = new(Config, "#04d9ff");
    public static readonly Unicolour NeonGreen = new(Config, "#0cff0c");
    public static readonly Unicolour NeonPink = new(Config, "#fe019a");
    public static readonly Unicolour NeonPurple = new(Config, "#bc13fe");
    public static readonly Unicolour NeonRed = new(Config, "#ff073a");
    public static readonly Unicolour NeonYellow = new(Config, "#cfff04");
    public static readonly Unicolour NiceBlue = new(Config, "#107ab0");
    public static readonly Unicolour NightBlue = new(Config, "#040348");
    public static readonly Unicolour Ocean = new(Config, "#017b92");
    public static readonly Unicolour OceanBlue = new(Config, "#03719c");
    public static readonly Unicolour OceanGreen = new(Config, "#3d9973");
    public static readonly Unicolour Ocher = new(Config, "#bf9b0c");
    public static readonly Unicolour Ochre = new(Config, "#bf9005");
    public static readonly Unicolour Ocre = new(Config, "#c69c04");
    public static readonly Unicolour OffBlue = new(Config, "#5684ae");
    public static readonly Unicolour OffGreen = new(Config, "#6ba353");
    public static readonly Unicolour OffWhite = new(Config, "#ffffe4");
    public static readonly Unicolour OffYellow = new(Config, "#f1f33f");
    public static readonly Unicolour OldPink = new(Config, "#c77986");
    public static readonly Unicolour OldRose = new(Config, "#c87f89");
    public static readonly Unicolour Olive = new(Config, "#6e750e");
    public static readonly Unicolour OliveBrown = new(Config, "#645403");
    public static readonly Unicolour OliveDrab = new(Config, "#6f7632");
    public static readonly Unicolour OliveGreen = new(Config, "#677a04");
    public static readonly Unicolour OliveYellow = new(Config, "#c2b709");
    public static readonly Unicolour Orange = new(Config, "#f97306");
    public static readonly Unicolour OrangeBrown = new(Config, "#be6400");
    public static readonly Unicolour Orangeish = new(Config, "#fd8d49");
    public static readonly Unicolour OrangePink = new(Config, "#ff6f52");
    public static readonly Unicolour OrangeRed = new(Config, "#fd411e");
    public static readonly Unicolour Orangered = new(Config, "#fe420f");
    public static readonly Unicolour OrangeyBrown = new(Config, "#b16002");
    public static readonly Unicolour OrangeYellow = new(Config, "#ffad01");
    public static readonly Unicolour OrangeyRed = new(Config, "#fa4224");
    public static readonly Unicolour OrangeyYellow = new(Config, "#fdb915");
    public static readonly Unicolour Orangish = new(Config, "#fc824a");
    public static readonly Unicolour OrangishBrown = new(Config, "#b25f03");
    public static readonly Unicolour OrangishRed = new(Config, "#f43605");
    public static readonly Unicolour Orchid = new(Config, "#c875c4");
    public static readonly Unicolour Pale = new(Config, "#fff9d0");
    public static readonly Unicolour PaleAqua = new(Config, "#b8ffeb");
    public static readonly Unicolour PaleBlue = new(Config, "#d0fefe");
    public static readonly Unicolour PaleBrown = new(Config, "#b1916e");
    public static readonly Unicolour PaleCyan = new(Config, "#b7fffa");
    public static readonly Unicolour PaleGold = new(Config, "#fdde6c");
    public static readonly Unicolour PaleGreen = new(Config, "#c7fdb5");
    public static readonly Unicolour PaleGrey = new(Config, "#fdfdfe");
    public static readonly Unicolour PaleLavender = new(Config, "#eecffe");
    public static readonly Unicolour PaleLightGreen = new(Config, "#b1fc99");
    public static readonly Unicolour PaleLilac = new(Config, "#e4cbff");
    public static readonly Unicolour PaleLime = new(Config, "#befd73");
    public static readonly Unicolour PaleLimeGreen = new(Config, "#b1ff65");
    public static readonly Unicolour PaleMagenta = new(Config, "#d767ad");
    public static readonly Unicolour PaleMauve = new(Config, "#fed0fc");
    public static readonly Unicolour PaleOlive = new(Config, "#b9cc81");
    public static readonly Unicolour PaleOliveGreen = new(Config, "#b1d27b");
    public static readonly Unicolour PaleOrange = new(Config, "#ffa756");
    public static readonly Unicolour PalePeach = new(Config, "#ffe5ad");
    public static readonly Unicolour PalePink = new(Config, "#ffcfdc");
    public static readonly Unicolour PalePurple = new(Config, "#b790d4");
    public static readonly Unicolour PaleRed = new(Config, "#d9544d");
    public static readonly Unicolour PaleRose = new(Config, "#fdc1c5");
    public static readonly Unicolour PaleSalmon = new(Config, "#ffb19a");
    public static readonly Unicolour PaleSkyBlue = new(Config, "#bdf6fe");
    public static readonly Unicolour PaleTeal = new(Config, "#82cbb2");
    public static readonly Unicolour PaleTurquoise = new(Config, "#a5fbd5");
    public static readonly Unicolour PaleViolet = new(Config, "#ceaefa");
    public static readonly Unicolour PaleYellow = new(Config, "#ffff84");
    public static readonly Unicolour Parchment = new(Config, "#fefcaf");
    public static readonly Unicolour PastelBlue = new(Config, "#a2bffe");
    public static readonly Unicolour PastelGreen = new(Config, "#b0ff9d");
    public static readonly Unicolour PastelOrange = new(Config, "#ff964f");
    public static readonly Unicolour PastelPink = new(Config, "#ffbacd");
    public static readonly Unicolour PastelPurple = new(Config, "#caa0ff");
    public static readonly Unicolour PastelRed = new(Config, "#db5856");
    public static readonly Unicolour PastelYellow = new(Config, "#fffe71");
    public static readonly Unicolour Pea = new(Config, "#a4bf20");
    public static readonly Unicolour Peach = new(Config, "#ffb07c");
    public static readonly Unicolour PeachyPink = new(Config, "#ff9a8a");
    public static readonly Unicolour PeacockBlue = new(Config, "#016795");
    public static readonly Unicolour PeaGreen = new(Config, "#8eab12");
    public static readonly Unicolour Pear = new(Config, "#cbf85f");
    public static readonly Unicolour PeaSoup = new(Config, "#929901");
    public static readonly Unicolour PeaSoupGreen = new(Config, "#94a617");
    public static readonly Unicolour Periwinkle = new(Config, "#8e82fe");
    public static readonly Unicolour PeriwinkleBlue = new(Config, "#8f99fb");
    public static readonly Unicolour Perrywinkle = new(Config, "#8f8ce7");
    public static readonly Unicolour Petrol = new(Config, "#005f6a");
    public static readonly Unicolour PigPink = new(Config, "#e78ea5");
    public static readonly Unicolour Pine = new(Config, "#2b5d34");
    public static readonly Unicolour PineGreen = new(Config, "#0a481e");
    public static readonly Unicolour Pink = new(Config, "#ff81c0");
    public static readonly Unicolour Pink_Purple = new(Config, "#ef1de7");
    public static readonly Unicolour Pinkish = new(Config, "#d46a7e");
    public static readonly Unicolour PinkishBrown = new(Config, "#b17261");
    public static readonly Unicolour PinkishGrey = new(Config, "#c8aca9");
    public static readonly Unicolour PinkishOrange = new(Config, "#ff724c");
    public static readonly Unicolour PinkishPurple = new(Config, "#d648d7");
    public static readonly Unicolour PinkishRed = new(Config, "#f10c45");
    public static readonly Unicolour PinkishTan = new(Config, "#d99b82");
    public static readonly Unicolour PinkPurple = new(Config, "#db4bda");
    public static readonly Unicolour PinkRed = new(Config, "#f5054f");
    public static readonly Unicolour Pinky = new(Config, "#fc86aa");
    public static readonly Unicolour PinkyPurple = new(Config, "#c94cbe");
    public static readonly Unicolour PinkyRed = new(Config, "#fc2647");
    public static readonly Unicolour PissYellow = new(Config, "#ddd618");
    public static readonly Unicolour Pistachio = new(Config, "#c0fa8b");
    public static readonly Unicolour Plum = new(Config, "#580f41");
    public static readonly Unicolour PlumPurple = new(Config, "#4e0550");
    public static readonly Unicolour PoisonGreen = new(Config, "#40fd14");
    public static readonly Unicolour Poo = new(Config, "#8f7303");
    public static readonly Unicolour PooBrown = new(Config, "#885f01");
    public static readonly Unicolour Poop = new(Config, "#7f5e00");
    public static readonly Unicolour PoopBrown = new(Config, "#7a5901");
    public static readonly Unicolour PoopGreen = new(Config, "#6f7c00");
    public static readonly Unicolour PowderBlue = new(Config, "#b1d1fc");
    public static readonly Unicolour PowderPink = new(Config, "#ffb2d0");
    public static readonly Unicolour PrimaryBlue = new(Config, "#0804f9");
    public static readonly Unicolour PrussianBlue = new(Config, "#004577");
    public static readonly Unicolour Puce = new(Config, "#a57e52");
    public static readonly Unicolour Puke = new(Config, "#a5a502");
    public static readonly Unicolour PukeBrown = new(Config, "#947706");
    public static readonly Unicolour PukeGreen = new(Config, "#9aae07");
    public static readonly Unicolour PukeYellow = new(Config, "#c2be0e");
    public static readonly Unicolour Pumpkin = new(Config, "#e17701");
    public static readonly Unicolour PumpkinOrange = new(Config, "#fb7d07");
    public static readonly Unicolour PureBlue = new(Config, "#0203e2");
    public static readonly Unicolour Purple = new(Config, "#7e1e9c");
    public static readonly Unicolour Purple_Blue = new(Config, "#5d21d0");
    public static readonly Unicolour Purple_Pink = new(Config, "#d725de");
    public static readonly Unicolour PurpleBlue = new(Config, "#632de9");
    public static readonly Unicolour PurpleBrown = new(Config, "#673a3f");
    public static readonly Unicolour PurpleGrey = new(Config, "#866f85");
    public static readonly Unicolour Purpleish = new(Config, "#98568d");
    public static readonly Unicolour PurpleishBlue = new(Config, "#6140ef");
    public static readonly Unicolour PurpleishPink = new(Config, "#df4ec8");
    public static readonly Unicolour PurplePink = new(Config, "#e03fd8");
    public static readonly Unicolour PurpleRed = new(Config, "#990147");
    public static readonly Unicolour Purpley = new(Config, "#8756e4");
    public static readonly Unicolour PurpleyBlue = new(Config, "#5f34e7");
    public static readonly Unicolour PurpleyGrey = new(Config, "#947e94");
    public static readonly Unicolour PurpleyPink = new(Config, "#c83cb9");
    public static readonly Unicolour Purplish = new(Config, "#94568c");
    public static readonly Unicolour PurplishBlue = new(Config, "#601ef9");
    public static readonly Unicolour PurplishBrown = new(Config, "#6b4247");
    public static readonly Unicolour PurplishGrey = new(Config, "#7a687f");
    public static readonly Unicolour PurplishPink = new(Config, "#ce5dae");
    public static readonly Unicolour PurplishRed = new(Config, "#b0054b");
    public static readonly Unicolour Purply = new(Config, "#983fb2");
    public static readonly Unicolour PurplyBlue = new(Config, "#661aee");
    public static readonly Unicolour PurplyPink = new(Config, "#f075e6");
    public static readonly Unicolour Putty = new(Config, "#beae8a");
    public static readonly Unicolour RacingGreen = new(Config, "#014600");
    public static readonly Unicolour RadioactiveGreen = new(Config, "#2cfa1f");
    public static readonly Unicolour Raspberry = new(Config, "#b00149");
    public static readonly Unicolour RawSienna = new(Config, "#9a6200");
    public static readonly Unicolour RawUmber = new(Config, "#a75e09");
    public static readonly Unicolour ReallyLightBlue = new(Config, "#d4ffff");
    public static readonly Unicolour Red = new(Config, "#e50000");
    public static readonly Unicolour RedBrown = new(Config, "#8b2e16");
    public static readonly Unicolour Reddish = new(Config, "#c44240");
    public static readonly Unicolour ReddishBrown = new(Config, "#7f2b0a");
    public static readonly Unicolour ReddishGrey = new(Config, "#997570");
    public static readonly Unicolour ReddishOrange = new(Config, "#f8481c");
    public static readonly Unicolour ReddishPink = new(Config, "#fe2c54");
    public static readonly Unicolour ReddishPurple = new(Config, "#910951");
    public static readonly Unicolour ReddyBrown = new(Config, "#6e1005");
    public static readonly Unicolour RedOrange = new(Config, "#fd3c06");
    public static readonly Unicolour RedPink = new(Config, "#fa2a55");
    public static readonly Unicolour RedPurple = new(Config, "#820747");
    public static readonly Unicolour RedViolet = new(Config, "#9e0168");
    public static readonly Unicolour RedWine = new(Config, "#8c0034");
    public static readonly Unicolour RichBlue = new(Config, "#021bf9");
    public static readonly Unicolour RichPurple = new(Config, "#720058");
    public static readonly Unicolour RobinEggBlue = new(Config, "#8af1fe");
    public static readonly Unicolour RobinsEgg = new(Config, "#6dedfd");
    public static readonly Unicolour RobinsEggBlue = new(Config, "#98eff9");
    public static readonly Unicolour Rosa = new(Config, "#fe86a4");
    public static readonly Unicolour Rose = new(Config, "#cf6275");
    public static readonly Unicolour RosePink = new(Config, "#f7879a");
    public static readonly Unicolour RoseRed = new(Config, "#be013c");
    public static readonly Unicolour RosyPink = new(Config, "#f6688e");
    public static readonly Unicolour Rouge = new(Config, "#ab1239");
    public static readonly Unicolour Royal = new(Config, "#0c1793");
    public static readonly Unicolour RoyalBlue = new(Config, "#0504aa");
    public static readonly Unicolour RoyalPurple = new(Config, "#4b006e");
    public static readonly Unicolour Ruby = new(Config, "#ca0147");
    public static readonly Unicolour Russet = new(Config, "#a13905");
    public static readonly Unicolour Rust = new(Config, "#a83c09");
    public static readonly Unicolour RustBrown = new(Config, "#8b3103");
    public static readonly Unicolour RustOrange = new(Config, "#c45508");
    public static readonly Unicolour RustRed = new(Config, "#aa2704");
    public static readonly Unicolour RustyOrange = new(Config, "#cd5909");
    public static readonly Unicolour RustyRed = new(Config, "#af2f0d");
    public static readonly Unicolour Saffron = new(Config, "#feb209");
    public static readonly Unicolour Sage = new(Config, "#87ae73");
    public static readonly Unicolour SageGreen = new(Config, "#88b378");
    public static readonly Unicolour Salmon = new(Config, "#ff796c");
    public static readonly Unicolour SalmonPink = new(Config, "#fe7b7c");
    public static readonly Unicolour Sand = new(Config, "#e2ca76");
    public static readonly Unicolour SandBrown = new(Config, "#cba560");
    public static readonly Unicolour Sandstone = new(Config, "#c9ae74");
    public static readonly Unicolour Sandy = new(Config, "#f1da7a");
    public static readonly Unicolour SandyBrown = new(Config, "#c4a661");
    public static readonly Unicolour SandYellow = new(Config, "#fce166");
    public static readonly Unicolour SandyYellow = new(Config, "#fdee73");
    public static readonly Unicolour SapGreen = new(Config, "#5c8b15");
    public static readonly Unicolour Sapphire = new(Config, "#2138ab");
    public static readonly Unicolour Scarlet = new(Config, "#be0119");
    public static readonly Unicolour Sea = new(Config, "#3c9992");
    public static readonly Unicolour SeaBlue = new(Config, "#047495");
    public static readonly Unicolour Seafoam = new(Config, "#80f9ad");
    public static readonly Unicolour SeafoamBlue = new(Config, "#78d1b6");
    public static readonly Unicolour SeafoamGreen = new(Config, "#7af9ab");
    public static readonly Unicolour SeaGreen = new(Config, "#53fca1");
    public static readonly Unicolour Seaweed = new(Config, "#18d17b");
    public static readonly Unicolour SeaweedGreen = new(Config, "#35ad6b");
    public static readonly Unicolour Sepia = new(Config, "#985e2b");
    public static readonly Unicolour Shamrock = new(Config, "#01b44c");
    public static readonly Unicolour ShamrockGreen = new(Config, "#02c14d");
    public static readonly Unicolour Shit = new(Config, "#7f5f00");
    public static readonly Unicolour ShitBrown = new(Config, "#7b5804");
    public static readonly Unicolour ShitGreen = new(Config, "#758000");
    public static readonly Unicolour ShockingPink = new(Config, "#fe02a2");
    public static readonly Unicolour SickGreen = new(Config, "#9db92c");
    public static readonly Unicolour SicklyGreen = new(Config, "#94b21c");
    public static readonly Unicolour SicklyYellow = new(Config, "#d0e429");
    public static readonly Unicolour Sienna = new(Config, "#a9561e");
    public static readonly Unicolour Silver = new(Config, "#c5c9c7");
    public static readonly Unicolour Sky = new(Config, "#82cafc");
    public static readonly Unicolour SkyBlue = new(Config, "#75bbfd");
    public static readonly Unicolour Slate = new(Config, "#516572");
    public static readonly Unicolour SlateBlue = new(Config, "#5b7c99");
    public static readonly Unicolour SlateGreen = new(Config, "#658d6d");
    public static readonly Unicolour SlateGrey = new(Config, "#59656d");
    public static readonly Unicolour SlimeGreen = new(Config, "#99cc04");
    public static readonly Unicolour Snot = new(Config, "#acbb0d");
    public static readonly Unicolour SnotGreen = new(Config, "#9dc100");
    public static readonly Unicolour SoftBlue = new(Config, "#6488ea");
    public static readonly Unicolour SoftGreen = new(Config, "#6fc276");
    public static readonly Unicolour SoftPink = new(Config, "#fdb0c0");
    public static readonly Unicolour SoftPurple = new(Config, "#a66fb5");
    public static readonly Unicolour Spearmint = new(Config, "#1ef876");
    public static readonly Unicolour SpringGreen = new(Config, "#a9f971");
    public static readonly Unicolour Spruce = new(Config, "#0a5f38");
    public static readonly Unicolour Squash = new(Config, "#f2ab15");
    public static readonly Unicolour Steel = new(Config, "#738595");
    public static readonly Unicolour SteelBlue = new(Config, "#5a7d9a");
    public static readonly Unicolour SteelGrey = new(Config, "#6f828a");
    public static readonly Unicolour Stone = new(Config, "#ada587");
    public static readonly Unicolour StormyBlue = new(Config, "#507b9c");
    public static readonly Unicolour Straw = new(Config, "#fcf679");
    public static readonly Unicolour Strawberry = new(Config, "#fb2943");
    public static readonly Unicolour StrongBlue = new(Config, "#0c06f7");
    public static readonly Unicolour StrongPink = new(Config, "#ff0789");
    public static readonly Unicolour Sunflower = new(Config, "#ffc512");
    public static readonly Unicolour SunflowerYellow = new(Config, "#ffda03");
    public static readonly Unicolour SunnyYellow = new(Config, "#fff917");
    public static readonly Unicolour SunshineYellow = new(Config, "#fffd37");
    public static readonly Unicolour SunYellow = new(Config, "#ffdf22");
    public static readonly Unicolour Swamp = new(Config, "#698339");
    public static readonly Unicolour SwampGreen = new(Config, "#748500");
    public static readonly Unicolour Tan = new(Config, "#d1b26f");
    public static readonly Unicolour TanBrown = new(Config, "#ab7e4c");
    public static readonly Unicolour Tangerine = new(Config, "#ff9408");
    public static readonly Unicolour TanGreen = new(Config, "#a9be70");
    public static readonly Unicolour Taupe = new(Config, "#b9a281");
    public static readonly Unicolour Tea = new(Config, "#65ab7c");
    public static readonly Unicolour TeaGreen = new(Config, "#bdf8a3");
    public static readonly Unicolour Teal = new(Config, "#029386");
    public static readonly Unicolour TealBlue = new(Config, "#01889f");
    public static readonly Unicolour TealGreen = new(Config, "#25a36f");
    public static readonly Unicolour Tealish = new(Config, "#24bca8");
    public static readonly Unicolour TealishGreen = new(Config, "#0cdc73");
    public static readonly Unicolour Terracota = new(Config, "#cb6843");
    public static readonly Unicolour TerraCotta = new(Config, "#c9643b");
    public static readonly Unicolour Terracotta = new(Config, "#ca6641");
    public static readonly Unicolour TiffanyBlue = new(Config, "#7bf2da");
    public static readonly Unicolour Tomato = new(Config, "#ef4026");
    public static readonly Unicolour TomatoRed = new(Config, "#ec2d01");
    public static readonly Unicolour Topaz = new(Config, "#13bbaf");
    public static readonly Unicolour Toupe = new(Config, "#c7ac7d");
    public static readonly Unicolour ToxicGreen = new(Config, "#61de2a");
    public static readonly Unicolour TreeGreen = new(Config, "#2a7e19");
    public static readonly Unicolour TrueBlue = new(Config, "#010fcc");
    public static readonly Unicolour TrueGreen = new(Config, "#089404");
    public static readonly Unicolour Turquoise = new(Config, "#06c2ac");
    public static readonly Unicolour TurquoiseBlue = new(Config, "#06b1c4");
    public static readonly Unicolour TurquoiseGreen = new(Config, "#04f489");
    public static readonly Unicolour TurtleGreen = new(Config, "#75b84f");
    public static readonly Unicolour Twilight = new(Config, "#4e518b");
    public static readonly Unicolour TwilightBlue = new(Config, "#0a437a");
    public static readonly Unicolour UglyBlue = new(Config, "#31668a");
    public static readonly Unicolour UglyBrown = new(Config, "#7d7103");
    public static readonly Unicolour UglyGreen = new(Config, "#7a9703");
    public static readonly Unicolour UglyPink = new(Config, "#cd7584");
    public static readonly Unicolour UglyPurple = new(Config, "#a442a0");
    public static readonly Unicolour UglyYellow = new(Config, "#d0c101");
    public static readonly Unicolour Ultramarine = new(Config, "#2000b1");
    public static readonly Unicolour UltramarineBlue = new(Config, "#1805db");
    public static readonly Unicolour Umber = new(Config, "#b26400");
    public static readonly Unicolour Velvet = new(Config, "#750851");
    public static readonly Unicolour Vermillion = new(Config, "#f4320c");
    public static readonly Unicolour VeryDarkBlue = new(Config, "#000133");
    public static readonly Unicolour VeryDarkBrown = new(Config, "#1d0200");
    public static readonly Unicolour VeryDarkGreen = new(Config, "#062e03");
    public static readonly Unicolour VeryDarkPurple = new(Config, "#2a0134");
    public static readonly Unicolour VeryLightBlue = new(Config, "#d5ffff");
    public static readonly Unicolour VeryLightBrown = new(Config, "#d3b683");
    public static readonly Unicolour VeryLightGreen = new(Config, "#d1ffbd");
    public static readonly Unicolour VeryLightPink = new(Config, "#fff4f2");
    public static readonly Unicolour VeryLightPurple = new(Config, "#f6cefc");
    public static readonly Unicolour VeryPaleBlue = new(Config, "#d6fffe");
    public static readonly Unicolour VeryPaleGreen = new(Config, "#cffdbc");
    public static readonly Unicolour VibrantBlue = new(Config, "#0339f8");
    public static readonly Unicolour VibrantGreen = new(Config, "#0add08");
    public static readonly Unicolour VibrantPurple = new(Config, "#ad03de");
    public static readonly Unicolour Violet = new(Config, "#9a0eea");
    public static readonly Unicolour VioletBlue = new(Config, "#510ac9");
    public static readonly Unicolour VioletPink = new(Config, "#fb5ffc");
    public static readonly Unicolour VioletRed = new(Config, "#a50055");
    public static readonly Unicolour Viridian = new(Config, "#1e9167");
    public static readonly Unicolour VividBlue = new(Config, "#152eff");
    public static readonly Unicolour VividGreen = new(Config, "#2fef10");
    public static readonly Unicolour VividPurple = new(Config, "#9900fa");
    public static readonly Unicolour Vomit = new(Config, "#a2a415");
    public static readonly Unicolour VomitGreen = new(Config, "#89a203");
    public static readonly Unicolour VomitYellow = new(Config, "#c7c10c");
    public static readonly Unicolour WarmBlue = new(Config, "#4b57db");
    public static readonly Unicolour WarmBrown = new(Config, "#964e02");
    public static readonly Unicolour WarmGrey = new(Config, "#978a84");
    public static readonly Unicolour WarmPink = new(Config, "#fb5581");
    public static readonly Unicolour WarmPurple = new(Config, "#952e8f");
    public static readonly Unicolour WashedOutGreen = new(Config, "#bcf5a6");
    public static readonly Unicolour WaterBlue = new(Config, "#0e87cc");
    public static readonly Unicolour Watermelon = new(Config, "#fd4659");
    public static readonly Unicolour WeirdGreen = new(Config, "#3ae57f");
    public static readonly Unicolour Wheat = new(Config, "#fbdd7e");
    public static readonly Unicolour White = new(Config, "#ffffff");
    public static readonly Unicolour WindowsBlue = new(Config, "#3778bf");
    public static readonly Unicolour Wine = new(Config, "#80013f");
    public static readonly Unicolour WineRed = new(Config, "#7b0323");
    public static readonly Unicolour Wintergreen = new(Config, "#20f986");
    public static readonly Unicolour Wisteria = new(Config, "#a87dc2");
    public static readonly Unicolour Yellow = new(Config, "#ffff14");
    public static readonly Unicolour Yellow_Green = new(Config, "#c8fd3d");
    public static readonly Unicolour YellowBrown = new(Config, "#b79400");
    public static readonly Unicolour Yellowgreen = new(Config, "#bbf90f");
    public static readonly Unicolour YellowGreen = new(Config, "#c0fb2d");
    public static readonly Unicolour Yellowish = new(Config, "#faee66");
    public static readonly Unicolour YellowishBrown = new(Config, "#9b7a01");
    public static readonly Unicolour YellowishGreen = new(Config, "#b0dd16");
    public static readonly Unicolour YellowishOrange = new(Config, "#ffab0f");
    public static readonly Unicolour YellowishTan = new(Config, "#fcfc81");
    public static readonly Unicolour YellowOchre = new(Config, "#cb9d06");
    public static readonly Unicolour YellowOrange = new(Config, "#fcb001");
    public static readonly Unicolour YellowTan = new(Config, "#ffe36e");
    public static readonly Unicolour YellowyBrown = new(Config, "#ae8b0c");
    public static readonly Unicolour YellowyGreen = new(Config, "#bff128");


    public static IEnumerable<Unicolour> All => new List<Unicolour>
    {
        AcidGreen, Adobe, Algae, AlgaeGreen, AlmostBlack, Amber, Amethyst, Apple, AppleGreen, Apricot, Aqua, AquaBlue, AquaGreen, AquaMarine, Aquamarine, ArmyGreen, Asparagus, Aubergine, Auburn, Avocado, AvocadoGreen, Azul, Azure,
        BabyBlue, BabyGreen, BabyPink, BabyPoo, BabyPoop, BabyPoopGreen, BabyPukeGreen, BabyPurple, BabyShitBrown, BabyShitGreen, Banana, BananaYellow, BarbiePink, BarfGreen, Barney, BarneyPurple, BattleshipGrey, Beige, Berry, Bile, Black, Bland, Blood, BloodOrange, BloodRed, Blue, Blue_Green, Blue_Grey, Blue_Purple, Blueberry, BlueBlue, Bluegreen, BlueGreen, Bluegrey, BlueGrey, BluePurple, BlueViolet, BlueWithAHintOfPurple, BlueyGreen, BlueyGrey, BlueyPurple, Bluish, BluishGreen, BluishGrey, BluishPurple, Blurple, Blush, BlushPink, Booger, BoogerGreen, Bordeaux, BoringGreen, BottleGreen, Brick, BrickOrange, BrickRed, BrightAqua, BrightBlue, BrightCyan, BrightGreen, BrightLavender, BrightLightBlue, BrightLightGreen, BrightLilac, BrightLime, BrightLimeGreen, BrightMagenta, BrightOlive, BrightOrange, BrightPink, BrightPurple, BrightRed, BrightSeaGreen, BrightSkyBlue, BrightTeal, BrightTurquoise, BrightViolet, BrightYellow, BrightYellowGreen, BritishRacingGreen, Bronze, Brown, BrownGreen, BrownGrey, Brownish, BrownishGreen, BrownishGrey, BrownishOrange, BrownishPink, BrownishPurple, BrownishRed, BrownishYellow, BrownOrange, BrownRed, BrownYellow, BrownyGreen, BrownyOrange, Bruise, Bubblegum, BubbleGumPink, BubblegumPink, Buff, Burgundy, BurntOrange, BurntRed, BurntSiena, BurntSienna, BurntUmber, BurntYellow, Burple, Butter, Butterscotch, ButterYellow,
        CadetBlue, Camel, Camo, CamoGreen, CamouflageGreen, Canary, CanaryYellow, CandyPink, Caramel, Carmine, Carnation, CarnationPink, CarolinaBlue, Celadon, Celery, Cement, Cerise, Cerulean, CeruleanBlue, Charcoal, CharcoalGrey, Chartreuse, Cherry, CherryRed, Chestnut, Chocolate, ChocolateBrown, Cinnamon, Claret, Clay, ClayBrown, ClearBlue, CloudyBlue, Cobalt, CobaltBlue, Cocoa, Coffee, CoolBlue, CoolGreen, CoolGrey, Copper, Coral, CoralPink, Cornflower, CornflowerBlue, Cranberry, Cream, Creme, Crimson, Custard, Cyan,
        Dandelion, Dark, DarkAqua, DarkAquamarine, DarkBeige, Darkblue, DarkBlue, DarkBlueGreen, DarkBlueGrey, DarkBrown, DarkCoral, DarkCream, DarkCyan, DarkForestGreen, DarkFuchsia, DarkGold, DarkGrassGreen, Darkgreen, DarkGreen, DarkGreenBlue, DarkGrey, DarkGreyBlue, DarkHotPink, DarkIndigo, DarkishBlue, DarkishGreen, DarkishPink, DarkishPurple, DarkishRed, DarkKhaki, DarkLavender, DarkLilac, DarkLime, DarkLimeGreen, DarkMagenta, DarkMaroon, DarkMauve, DarkMint, DarkMintGreen, DarkMustard, DarkNavy, DarkNavyBlue, DarkOlive, DarkOliveGreen, DarkOrange, DarkPastelGreen, DarkPeach, DarkPeriwinkle, DarkPink, DarkPlum, DarkPurple, DarkRed, DarkRose, DarkRoyalBlue, DarkSage, DarkSalmon, DarkSand, DarkSeafoam, DarkSeafoamGreen, DarkSeaGreen, DarkSkyBlue, DarkSlateBlue, DarkTan, DarkTaupe, DarkTeal, DarkTurquoise, DarkViolet, DarkYellow, DarkYellowGreen, DeepAqua, DeepBlue, DeepBrown, DeepGreen, DeepLavender, DeepLilac, DeepMagenta, DeepOrange, DeepPink, DeepPurple, DeepRed, DeepRose, DeepSeaBlue, DeepSkyBlue, DeepTeal, DeepTurquoise, DeepViolet, Denim, DenimBlue, Desert, Diarrhea, Dirt, DirtBrown, DirtyBlue, DirtyGreen, DirtyOrange, DirtyPink, DirtyPurple, DirtyYellow, DodgerBlue, Drab, DrabGreen, DriedBlood, DuckEggBlue, DullBlue, DullBrown, DullGreen, DullOrange, DullPink, DullPurple, DullRed, DullTeal, DullYellow, Dusk, DuskBlue, DuskyBlue, DuskyPink, DuskyPurple, DuskyRose, Dust, DustyBlue, DustyGreen, DustyLavender, DustyOrange, DustyPink, DustyPurple, DustyRed, DustyRose, DustyTeal,
        Earth, EasterGreen, EasterPurple, Ecru, Eggplant, EggplantPurple, EggShell, Eggshell, EggshellBlue, ElectricBlue, ElectricGreen, ElectricLime, ElectricPink, ElectricPurple, Emerald, EmeraldGreen, Evergreen,
        FadedBlue, FadedGreen, FadedOrange, FadedPink, FadedPurple, FadedRed, FadedYellow, Fawn, Fern, FernGreen, FireEngineRed, FlatBlue, FlatGreen, FluorescentGreen, FluroGreen, FoamGreen, Forest, ForestGreen, ForrestGreen, FrenchBlue, FreshGreen, FrogGreen, Fuchsia,
        Gold, Golden, GoldenBrown, GoldenRod, Goldenrod, GoldenYellow, Grape, Grapefruit, GrapePurple, Grass, GrassGreen, GrassyGreen, Green, Green_Blue, Green_Yellow, GreenApple, Greenblue, GreenBlue, GreenBrown, GreenGrey, Greenish, GreenishBeige, GreenishBlue, GreenishBrown, GreenishCyan, GreenishGrey, GreenishTan, GreenishTeal, GreenishTurquoise, GreenishYellow, GreenTeal, GreenyBlue, GreenyBrown, GreenYellow, GreenyGrey, GreenyYellow, Grey, Grey_Blue, Grey_Green, Greyblue, GreyBlue, GreyBrown, GreyGreen, Greyish, GreyishBlue, GreyishBrown, GreyishGreen, GreyishPink, GreyishPurple, GreyishTeal, GreyPink, GreyPurple, GreyTeal, GrossGreen, Gunmetal,
        Hazel, Heather, Heliotrope, HighlighterGreen, HospitalGreen, HotGreen, HotMagenta, HotPink, HotPurple, HunterGreen,
        Ice, IceBlue, IckyGreen, IndianRed, Indigo, IndigoBlue, Iris, IrishGreen, Ivory,
        Jade, JadeGreen, JungleGreen,
        KelleyGreen, KellyGreen, KermitGreen, KeyLime, Khaki, KhakiGreen, Kiwi, KiwiGreen,
        Lavender, LavenderBlue, LavenderPink, LawnGreen, Leaf, LeafGreen, LeafyGreen, Leather, Lemon, LemonGreen, LemonLime, LemonYellow, Lichen, LightAqua, LightAquamarine, LightBeige, Lightblue, LightBlue, LightBlueGreen, LightBlueGrey, LightBluishGreen, LightBrightGreen, LightBrown, LightBurgundy, LightCyan, LightEggplant, LighterGreen, LighterPurple, LightForestGreen, LightGold, LightGrassGreen, Lightgreen, LightGreen, LightGreenBlue, LightGreenishBlue, LightGrey, LightGreyBlue, LightGreyGreen, LightIndigo, LightishBlue, LightishGreen, LightishPurple, LightishRed, LightKhaki, LightLavendar, LightLavender, LightLightBlue, LightLightGreen, LightLilac, LightLime, LightLimeGreen, LightMagenta, LightMaroon, LightMauve, LightMint, LightMintGreen, LightMossGreen, LightMustard, LightNavy, LightNavyBlue, LightNeonGreen, LightOlive, LightOliveGreen, LightOrange, LightPastelGreen, LightPeach, LightPeaGreen, LightPeriwinkle, LightPink, LightPlum, LightPurple, LightRed, LightRose, LightRoyalBlue, LightSage, LightSalmon, LightSeafoam, LightSeafoamGreen, LightSeaGreen, LightSkyBlue, LightTan, LightTeal, LightTurquoise, LightUrple, LightViolet, LightYellow, LightYellowGreen, LightYellowishGreen, Lilac, Liliac, Lime, LimeGreen, LimeYellow, Lipstick, LipstickRed,
        MacaroniAndCheese, Magenta, Mahogany, Maize, Mango, Manilla, Marigold, Marine, MarineBlue, Maroon, Mauve, MediumBlue, MediumBrown, MediumGreen, MediumGrey, MediumPink, MediumPurple, Melon, Merlot, MetallicBlue, MidBlue, MidGreen, Midnight, MidnightBlue, MidnightPurple, MilitaryGreen, MilkChocolate, Mint, MintGreen, MintyGreen, Mocha, Moss, MossGreen, MossyGreen, Mud, MudBrown, MuddyBrown, MuddyGreen, MuddyYellow, MudGreen, Mulberry, MurkyGreen, Mushroom, Mustard, MustardBrown, MustardGreen, MustardYellow, MutedBlue, MutedGreen, MutedPink, MutedPurple,
        NastyGreen, Navy, NavyBlue, NavyGreen, NeonBlue, NeonGreen, NeonPink, NeonPurple, NeonRed, NeonYellow, NiceBlue, NightBlue,
        Ocean, OceanBlue, OceanGreen, Ocher, Ochre, Ocre, OffBlue, OffGreen, OffWhite, OffYellow, OldPink, OldRose, Olive, OliveBrown, OliveDrab, OliveGreen, OliveYellow, Orange, OrangeBrown, Orangeish, OrangePink, Orangered, OrangeRed, OrangeyBrown, OrangeYellow, OrangeyRed, OrangeyYellow, Orangish, OrangishBrown, OrangishRed, Orchid,
        Pale, PaleAqua, PaleBlue, PaleBrown, PaleCyan, PaleGold, PaleGreen, PaleGrey, PaleLavender, PaleLightGreen, PaleLilac, PaleLime, PaleLimeGreen, PaleMagenta, PaleMauve, PaleOlive, PaleOliveGreen, PaleOrange, PalePeach, PalePink, PalePurple, PaleRed, PaleRose, PaleSalmon, PaleSkyBlue, PaleTeal, PaleTurquoise, PaleViolet, PaleYellow, Parchment, PastelBlue, PastelGreen, PastelOrange, PastelPink, PastelPurple, PastelRed, PastelYellow, Pea, Peach, PeachyPink, PeacockBlue, PeaGreen, Pear, PeaSoup, PeaSoupGreen, Periwinkle, PeriwinkleBlue, Perrywinkle, Petrol, PigPink, Pine, PineGreen, Pink, Pink_Purple, Pinkish, PinkishBrown, PinkishGrey, PinkishOrange, PinkishPurple, PinkishRed, PinkishTan, PinkPurple, PinkRed, Pinky, PinkyPurple, PinkyRed, PissYellow, Pistachio, Plum, PlumPurple, PoisonGreen, Poo, PooBrown, Poop, PoopBrown, PoopGreen, PowderBlue, PowderPink, PrimaryBlue, PrussianBlue, Puce, Puke, PukeBrown, PukeGreen, PukeYellow, Pumpkin, PumpkinOrange, PureBlue, Purple, Purple_Blue, Purple_Pink, PurpleBlue, PurpleBrown, PurpleGrey, Purpleish, PurpleishBlue, PurpleishPink, PurplePink, PurpleRed, Purpley, PurpleyBlue, PurpleyGrey, PurpleyPink, Purplish, PurplishBlue, PurplishBrown, PurplishGrey, PurplishPink, PurplishRed, Purply, PurplyBlue, PurplyPink, Putty,
        RacingGreen, RadioactiveGreen, Raspberry, RawSienna, RawUmber, ReallyLightBlue, Red, RedBrown, Reddish, ReddishBrown, ReddishGrey, ReddishOrange, ReddishPink, ReddishPurple, ReddyBrown, RedOrange, RedPink, RedPurple, RedViolet, RedWine, RichBlue, RichPurple, RobinEggBlue, RobinsEgg, RobinsEggBlue, Rosa, Rose, RosePink, RoseRed, RosyPink, Rouge, Royal, RoyalBlue, RoyalPurple, Ruby, Russet, Rust, RustBrown, RustOrange, RustRed, RustyOrange, RustyRed,
        Saffron, Sage, SageGreen, Salmon, SalmonPink, Sand, SandBrown, Sandstone, Sandy, SandyBrown, SandYellow, SandyYellow, SapGreen, Sapphire, Scarlet, Sea, SeaBlue, Seafoam, SeafoamBlue, SeafoamGreen, SeaGreen, Seaweed, SeaweedGreen, Sepia, Shamrock, ShamrockGreen, Shit, ShitBrown, ShitGreen, ShockingPink, SickGreen, SicklyGreen, SicklyYellow, Sienna, Silver, Sky, SkyBlue, Slate, SlateBlue, SlateGreen, SlateGrey, SlimeGreen, Snot, SnotGreen, SoftBlue, SoftGreen, SoftPink, SoftPurple, Spearmint, SpringGreen, Spruce, Squash, Steel, SteelBlue, SteelGrey, Stone, StormyBlue, Straw, Strawberry, StrongBlue, StrongPink, Sunflower, SunflowerYellow, SunnyYellow, SunshineYellow, SunYellow, Swamp, SwampGreen,
        Tan, TanBrown, Tangerine, TanGreen, Taupe, Tea, TeaGreen, Teal, TealBlue, TealGreen, Tealish, TealishGreen, Terracota, TerraCotta, Terracotta, TiffanyBlue, Tomato, TomatoRed, Topaz, Toupe, ToxicGreen, TreeGreen, TrueBlue, TrueGreen, Turquoise, TurquoiseBlue, TurquoiseGreen, TurtleGreen, Twilight, TwilightBlue,
        UglyBlue, UglyBrown, UglyGreen, UglyPink, UglyPurple, UglyYellow, Ultramarine, UltramarineBlue, Umber,
        Velvet, Vermillion, VeryDarkBlue, VeryDarkBrown, VeryDarkGreen, VeryDarkPurple, VeryLightBlue, VeryLightBrown, VeryLightGreen, VeryLightPink, VeryLightPurple, VeryPaleBlue, VeryPaleGreen, VibrantBlue, VibrantGreen, VibrantPurple, Violet, VioletBlue, VioletPink, VioletRed, Viridian, VividBlue, VividGreen, VividPurple, Vomit, VomitGreen, VomitYellow,
        WarmBlue, WarmBrown, WarmGrey, WarmPink, WarmPurple, WashedOutGreen, WaterBlue, Watermelon, WeirdGreen, Wheat, White, WindowsBlue, Wine, WineRed, Wintergreen, Wisteria,
        Yellow, Yellow_Green, YellowBrown, Yellowgreen, YellowGreen, Yellowish, YellowishBrown, YellowishGreen, YellowishOrange, YellowishTan, YellowOchre, YellowOrange, YellowTan, YellowyBrown, YellowyGreen,
    };

    public static Unicolour? FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        
        // first try to use the name as though it is the exact text used in the spec
        var lowercase = name.ToLower();
        Lookup.TryGetValue(lowercase, out Unicolour? value);
        if (value != null)
        {
            return value;
        }

        // if that doesn't match, sanitise both name and keys to find potential matches
        // by removing all whitespace (including line separators) and any punctuation that appears in the keys
        var sanitisedName = Sanitise(lowercase);
        var potentialKeys = Lookup.Keys.Where(x => Sanitise(x) == sanitisedName).ToList();
        return potentialKeys.Any() ? Lookup[potentialKeys.First()] : null;

        string Sanitise(string text)
        {
            var noWhitespace = string.Concat(text.Where(x => !char.IsWhiteSpace(x)));
            return noWhitespace.Replace("/", string.Empty).Replace("'", string.Empty);
        }
    }
    
    private static readonly Dictionary<string, Unicolour> Lookup = new()
    {
        { "acid green", AcidGreen },
        { "adobe", Adobe },
        { "algae", Algae },
        { "algae green", AlgaeGreen },
        { "almost black", AlmostBlack },
        { "amber", Amber },
        { "amethyst", Amethyst },
        { "apple", Apple },
        { "apple green", AppleGreen },
        { "apricot", Apricot },
        { "aqua", Aqua },
        { "aqua blue", AquaBlue },
        { "aqua green", AquaGreen },
        { "aquamarine", Aquamarine },
        { "aqua marine", AquaMarine },
        { "army green", ArmyGreen },
        { "asparagus", Asparagus },
        { "aubergine", Aubergine },
        { "auburn", Auburn },
        { "avocado", Avocado },
        { "avocado green", AvocadoGreen },
        { "azul", Azul },
        { "azure", Azure },
        { "baby blue", BabyBlue },
        { "baby green", BabyGreen },
        { "baby pink", BabyPink },
        { "baby poo", BabyPoo },
        { "baby poop", BabyPoop },
        { "baby poop green", BabyPoopGreen },
        { "baby puke green", BabyPukeGreen },
        { "baby purple", BabyPurple },
        { "baby shit brown", BabyShitBrown },
        { "baby shit green", BabyShitGreen },
        { "banana", Banana },
        { "banana yellow", BananaYellow },
        { "barbie pink", BarbiePink },
        { "barf green", BarfGreen },
        { "barney", Barney },
        { "barney purple", BarneyPurple },
        { "battleship grey", BattleshipGrey },
        { "beige", Beige },
        { "berry", Berry },
        { "bile", Bile },
        { "black", Black },
        { "bland", Bland },
        { "blood", Blood },
        { "blood orange", BloodOrange },
        { "blood red", BloodRed },
        { "blue", Blue },
        { "blue/green", Blue_Green },
        { "blue/grey", Blue_Grey },
        { "blue/purple", Blue_Purple },
        { "blueberry", Blueberry },
        { "blue blue", BlueBlue },
        { "bluegreen", Bluegreen },
        { "blue green", BlueGreen },
        { "blue grey", BlueGrey },
        { "bluegrey", Bluegrey },
        { "blue purple", BluePurple },
        { "blue violet", BlueViolet },
        { "blue with a hint of purple", BlueWithAHintOfPurple },
        { "bluey green", BlueyGreen },
        { "bluey grey", BlueyGrey },
        { "bluey purple", BlueyPurple },
        { "bluish", Bluish },
        { "bluish green", BluishGreen },
        { "bluish grey", BluishGrey },
        { "bluish purple", BluishPurple },
        { "blurple", Blurple },
        { "blush", Blush },
        { "blush pink", BlushPink },
        { "booger", Booger },
        { "booger green", BoogerGreen },
        { "bordeaux", Bordeaux },
        { "boring green", BoringGreen },
        { "bottle green", BottleGreen },
        { "brick", Brick },
        { "brick orange", BrickOrange },
        { "brick red", BrickRed },
        { "bright aqua", BrightAqua },
        { "bright blue", BrightBlue },
        { "bright cyan", BrightCyan },
        { "bright green", BrightGreen },
        { "bright lavender", BrightLavender },
        { "bright light blue", BrightLightBlue },
        { "bright light green", BrightLightGreen },
        { "bright lilac", BrightLilac },
        { "bright lime", BrightLime },
        { "bright lime green", BrightLimeGreen },
        { "bright magenta", BrightMagenta },
        { "bright olive", BrightOlive },
        { "bright orange", BrightOrange },
        { "bright pink", BrightPink },
        { "bright purple", BrightPurple },
        { "bright red", BrightRed },
        { "bright sea green", BrightSeaGreen },
        { "bright sky blue", BrightSkyBlue },
        { "bright teal", BrightTeal },
        { "bright turquoise", BrightTurquoise },
        { "bright violet", BrightViolet },
        { "bright yellow", BrightYellow },
        { "bright yellow green", BrightYellowGreen },
        { "british racing green", BritishRacingGreen },
        { "bronze", Bronze },
        { "brown", Brown },
        { "brown green", BrownGreen },
        { "brown grey", BrownGrey },
        { "brownish", Brownish },
        { "brownish green", BrownishGreen },
        { "brownish grey", BrownishGrey },
        { "brownish orange", BrownishOrange },
        { "brownish pink", BrownishPink },
        { "brownish purple", BrownishPurple },
        { "brownish red", BrownishRed },
        { "brownish yellow", BrownishYellow },
        { "brown orange", BrownOrange },
        { "brown red", BrownRed },
        { "brown yellow", BrownYellow },
        { "browny green", BrownyGreen },
        { "browny orange", BrownyOrange },
        { "bruise", Bruise },
        { "bubblegum", Bubblegum },
        { "bubblegum pink", BubblegumPink },
        { "bubble gum pink", BubbleGumPink },
        { "buff", Buff },
        { "burgundy", Burgundy },
        { "burnt orange", BurntOrange },
        { "burnt red", BurntRed },
        { "burnt siena", BurntSiena },
        { "burnt sienna", BurntSienna },
        { "burnt umber", BurntUmber },
        { "burnt yellow", BurntYellow },
        { "burple", Burple },
        { "butter", Butter },
        { "butterscotch", Butterscotch },
        { "butter yellow", ButterYellow },
        { "cadet blue", CadetBlue },
        { "camel", Camel },
        { "camo", Camo },
        { "camo green", CamoGreen },
        { "camouflage green", CamouflageGreen },
        { "canary", Canary },
        { "canary yellow", CanaryYellow },
        { "candy pink", CandyPink },
        { "caramel", Caramel },
        { "carmine", Carmine },
        { "carnation", Carnation },
        { "carnation pink", CarnationPink },
        { "carolina blue", CarolinaBlue },
        { "celadon", Celadon },
        { "celery", Celery },
        { "cement", Cement },
        { "cerise", Cerise },
        { "cerulean", Cerulean },
        { "cerulean blue", CeruleanBlue },
        { "charcoal", Charcoal },
        { "charcoal grey", CharcoalGrey },
        { "chartreuse", Chartreuse },
        { "cherry", Cherry },
        { "cherry red", CherryRed },
        { "chestnut", Chestnut },
        { "chocolate", Chocolate },
        { "chocolate brown", ChocolateBrown },
        { "cinnamon", Cinnamon },
        { "claret", Claret },
        { "clay", Clay },
        { "clay brown", ClayBrown },
        { "clear blue", ClearBlue },
        { "cloudy blue", CloudyBlue },
        { "cobalt", Cobalt },
        { "cobalt blue", CobaltBlue },
        { "cocoa", Cocoa },
        { "coffee", Coffee },
        { "cool blue", CoolBlue },
        { "cool green", CoolGreen },
        { "cool grey", CoolGrey },
        { "copper", Copper },
        { "coral", Coral },
        { "coral pink", CoralPink },
        { "cornflower", Cornflower },
        { "cornflower blue", CornflowerBlue },
        { "cranberry", Cranberry },
        { "cream", Cream },
        { "creme", Creme },
        { "crimson", Crimson },
        { "custard", Custard },
        { "cyan", Cyan },
        { "dandelion", Dandelion },
        { "dark", Dark },
        { "dark aqua", DarkAqua },
        { "dark aquamarine", DarkAquamarine },
        { "dark beige", DarkBeige },
        { "dark blue", DarkBlue },
        { "darkblue", Darkblue },
        { "dark blue green", DarkBlueGreen },
        { "dark blue grey", DarkBlueGrey },
        { "dark brown", DarkBrown },
        { "dark coral", DarkCoral },
        { "dark cream", DarkCream },
        { "dark cyan", DarkCyan },
        { "dark forest green", DarkForestGreen },
        { "dark fuchsia", DarkFuchsia },
        { "dark gold", DarkGold },
        { "dark grass green", DarkGrassGreen },
        { "dark green", DarkGreen },
        { "darkgreen", Darkgreen },
        { "dark green blue", DarkGreenBlue },
        { "dark grey", DarkGrey },
        { "dark grey blue", DarkGreyBlue },
        { "dark hot pink", DarkHotPink },
        { "dark indigo", DarkIndigo },
        { "darkish blue", DarkishBlue },
        { "darkish green", DarkishGreen },
        { "darkish pink", DarkishPink },
        { "darkish purple", DarkishPurple },
        { "darkish red", DarkishRed },
        { "dark khaki", DarkKhaki },
        { "dark lavender", DarkLavender },
        { "dark lilac", DarkLilac },
        { "dark lime", DarkLime },
        { "dark lime green", DarkLimeGreen },
        { "dark magenta", DarkMagenta },
        { "dark maroon", DarkMaroon },
        { "dark mauve", DarkMauve },
        { "dark mint", DarkMint },
        { "dark mint green", DarkMintGreen },
        { "dark mustard", DarkMustard },
        { "dark navy", DarkNavy },
        { "dark navy blue", DarkNavyBlue },
        { "dark olive", DarkOlive },
        { "dark olive green", DarkOliveGreen },
        { "dark orange", DarkOrange },
        { "dark pastel green", DarkPastelGreen },
        { "dark peach", DarkPeach },
        { "dark periwinkle", DarkPeriwinkle },
        { "dark pink", DarkPink },
        { "dark plum", DarkPlum },
        { "dark purple", DarkPurple },
        { "dark red", DarkRed },
        { "dark rose", DarkRose },
        { "dark royal blue", DarkRoyalBlue },
        { "dark sage", DarkSage },
        { "dark salmon", DarkSalmon },
        { "dark sand", DarkSand },
        { "dark seafoam", DarkSeafoam },
        { "dark seafoam green", DarkSeafoamGreen },
        { "dark sea green", DarkSeaGreen },
        { "dark sky blue", DarkSkyBlue },
        { "dark slate blue", DarkSlateBlue },
        { "dark tan", DarkTan },
        { "dark taupe", DarkTaupe },
        { "dark teal", DarkTeal },
        { "dark turquoise", DarkTurquoise },
        { "dark violet", DarkViolet },
        { "dark yellow", DarkYellow },
        { "dark yellow green", DarkYellowGreen },
        { "deep aqua", DeepAqua },
        { "deep blue", DeepBlue },
        { "deep brown", DeepBrown },
        { "deep green", DeepGreen },
        { "deep lavender", DeepLavender },
        { "deep lilac", DeepLilac },
        { "deep magenta", DeepMagenta },
        { "deep orange", DeepOrange },
        { "deep pink", DeepPink },
        { "deep purple", DeepPurple },
        { "deep red", DeepRed },
        { "deep rose", DeepRose },
        { "deep sea blue", DeepSeaBlue },
        { "deep sky blue", DeepSkyBlue },
        { "deep teal", DeepTeal },
        { "deep turquoise", DeepTurquoise },
        { "deep violet", DeepViolet },
        { "denim", Denim },
        { "denim blue", DenimBlue },
        { "desert", Desert },
        { "diarrhea", Diarrhea },
        { "dirt", Dirt },
        { "dirt brown", DirtBrown },
        { "dirty blue", DirtyBlue },
        { "dirty green", DirtyGreen },
        { "dirty orange", DirtyOrange },
        { "dirty pink", DirtyPink },
        { "dirty purple", DirtyPurple },
        { "dirty yellow", DirtyYellow },
        { "dodger blue", DodgerBlue },
        { "drab", Drab },
        { "drab green", DrabGreen },
        { "dried blood", DriedBlood },
        { "duck egg blue", DuckEggBlue },
        { "dull blue", DullBlue },
        { "dull brown", DullBrown },
        { "dull green", DullGreen },
        { "dull orange", DullOrange },
        { "dull pink", DullPink },
        { "dull purple", DullPurple },
        { "dull red", DullRed },
        { "dull teal", DullTeal },
        { "dull yellow", DullYellow },
        { "dusk", Dusk },
        { "dusk blue", DuskBlue },
        { "dusky blue", DuskyBlue },
        { "dusky pink", DuskyPink },
        { "dusky purple", DuskyPurple },
        { "dusky rose", DuskyRose },
        { "dust", Dust },
        { "dusty blue", DustyBlue },
        { "dusty green", DustyGreen },
        { "dusty lavender", DustyLavender },
        { "dusty orange", DustyOrange },
        { "dusty pink", DustyPink },
        { "dusty purple", DustyPurple },
        { "dusty red", DustyRed },
        { "dusty rose", DustyRose },
        { "dusty teal", DustyTeal },
        { "earth", Earth },
        { "easter green", EasterGreen },
        { "easter purple", EasterPurple },
        { "ecru", Ecru },
        { "eggplant", Eggplant },
        { "eggplant purple", EggplantPurple },
        { "egg shell", EggShell },
        { "eggshell", Eggshell },
        { "eggshell blue", EggshellBlue },
        { "electric blue", ElectricBlue },
        { "electric green", ElectricGreen },
        { "electric lime", ElectricLime },
        { "electric pink", ElectricPink },
        { "electric purple", ElectricPurple },
        { "emerald", Emerald },
        { "emerald green", EmeraldGreen },
        { "evergreen", Evergreen },
        { "faded blue", FadedBlue },
        { "faded green", FadedGreen },
        { "faded orange", FadedOrange },
        { "faded pink", FadedPink },
        { "faded purple", FadedPurple },
        { "faded red", FadedRed },
        { "faded yellow", FadedYellow },
        { "fawn", Fawn },
        { "fern", Fern },
        { "fern green", FernGreen },
        { "fire engine red", FireEngineRed },
        { "flat blue", FlatBlue },
        { "flat green", FlatGreen },
        { "fluorescent green", FluorescentGreen },
        { "fluro green", FluroGreen },
        { "foam green", FoamGreen },
        { "forest", Forest },
        { "forest green", ForestGreen },
        { "forrest green", ForrestGreen },
        { "french blue", FrenchBlue },
        { "fresh green", FreshGreen },
        { "frog green", FrogGreen },
        { "fuchsia", Fuchsia },
        { "gold", Gold },
        { "golden", Golden },
        { "golden brown", GoldenBrown },
        { "golden rod", GoldenRod },
        { "goldenrod", Goldenrod },
        { "golden yellow", GoldenYellow },
        { "grape", Grape },
        { "grapefruit", Grapefruit },
        { "grape purple", GrapePurple },
        { "grass", Grass },
        { "grass green", GrassGreen },
        { "grassy green", GrassyGreen },
        { "green", Green },
        { "green/blue", Green_Blue },
        { "green/yellow", Green_Yellow },
        { "green apple", GreenApple },
        { "green blue", GreenBlue },
        { "greenblue", Greenblue },
        { "green brown", GreenBrown },
        { "green grey", GreenGrey },
        { "greenish", Greenish },
        { "greenish beige", GreenishBeige },
        { "greenish blue", GreenishBlue },
        { "greenish brown", GreenishBrown },
        { "greenish cyan", GreenishCyan },
        { "greenish grey", GreenishGrey },
        { "greenish tan", GreenishTan },
        { "greenish teal", GreenishTeal },
        { "greenish turquoise", GreenishTurquoise },
        { "greenish yellow", GreenishYellow },
        { "green teal", GreenTeal },
        { "greeny blue", GreenyBlue },
        { "greeny brown", GreenyBrown },
        { "green yellow", GreenYellow },
        { "greeny grey", GreenyGrey },
        { "greeny yellow", GreenyYellow },
        { "grey", Grey },
        { "grey/blue", Grey_Blue },
        { "grey/green", Grey_Green },
        { "grey blue", GreyBlue },
        { "greyblue", Greyblue },
        { "grey brown", GreyBrown },
        { "grey green", GreyGreen },
        { "greyish", Greyish },
        { "greyish blue", GreyishBlue },
        { "greyish brown", GreyishBrown },
        { "greyish green", GreyishGreen },
        { "greyish pink", GreyishPink },
        { "greyish purple", GreyishPurple },
        { "greyish teal", GreyishTeal },
        { "grey pink", GreyPink },
        { "grey purple", GreyPurple },
        { "grey teal", GreyTeal },
        { "gross green", GrossGreen },
        { "gunmetal", Gunmetal },
        { "hazel", Hazel },
        { "heather", Heather },
        { "heliotrope", Heliotrope },
        { "highlighter green", HighlighterGreen },
        { "hospital green", HospitalGreen },
        { "hot green", HotGreen },
        { "hot magenta", HotMagenta },
        { "hot pink", HotPink },
        { "hot purple", HotPurple },
        { "hunter green", HunterGreen },
        { "ice", Ice },
        { "ice blue", IceBlue },
        { "icky green", IckyGreen },
        { "indian red", IndianRed },
        { "indigo", Indigo },
        { "indigo blue", IndigoBlue },
        { "iris", Iris },
        { "irish green", IrishGreen },
        { "ivory", Ivory },
        { "jade", Jade },
        { "jade green", JadeGreen },
        { "jungle green", JungleGreen },
        { "kelley green", KelleyGreen },
        { "kelly green", KellyGreen },
        { "kermit green", KermitGreen },
        { "key lime", KeyLime },
        { "khaki", Khaki },
        { "khaki green", KhakiGreen },
        { "kiwi", Kiwi },
        { "kiwi green", KiwiGreen },
        { "lavender", Lavender },
        { "lavender blue", LavenderBlue },
        { "lavender pink", LavenderPink },
        { "lawn green", LawnGreen },
        { "leaf", Leaf },
        { "leaf green", LeafGreen },
        { "leafy green", LeafyGreen },
        { "leather", Leather },
        { "lemon", Lemon },
        { "lemon green", LemonGreen },
        { "lemon lime", LemonLime },
        { "lemon yellow", LemonYellow },
        { "lichen", Lichen },
        { "light aqua", LightAqua },
        { "light aquamarine", LightAquamarine },
        { "light beige", LightBeige },
        { "lightblue", Lightblue },
        { "light blue", LightBlue },
        { "light blue green", LightBlueGreen },
        { "light blue grey", LightBlueGrey },
        { "light bluish green", LightBluishGreen },
        { "light bright green", LightBrightGreen },
        { "light brown", LightBrown },
        { "light burgundy", LightBurgundy },
        { "light cyan", LightCyan },
        { "light eggplant", LightEggplant },
        { "lighter green", LighterGreen },
        { "lighter purple", LighterPurple },
        { "light forest green", LightForestGreen },
        { "light gold", LightGold },
        { "light grass green", LightGrassGreen },
        { "lightgreen", Lightgreen },
        { "light green", LightGreen },
        { "light green blue", LightGreenBlue },
        { "light greenish blue", LightGreenishBlue },
        { "light grey", LightGrey },
        { "light grey blue", LightGreyBlue },
        { "light grey green", LightGreyGreen },
        { "light indigo", LightIndigo },
        { "lightish blue", LightishBlue },
        { "lightish green", LightishGreen },
        { "lightish purple", LightishPurple },
        { "lightish red", LightishRed },
        { "light khaki", LightKhaki },
        { "light lavendar", LightLavendar },
        { "light lavender", LightLavender },
        { "light light blue", LightLightBlue },
        { "light light green", LightLightGreen },
        { "light lilac", LightLilac },
        { "light lime", LightLime },
        { "light lime green", LightLimeGreen },
        { "light magenta", LightMagenta },
        { "light maroon", LightMaroon },
        { "light mauve", LightMauve },
        { "light mint", LightMint },
        { "light mint green", LightMintGreen },
        { "light moss green", LightMossGreen },
        { "light mustard", LightMustard },
        { "light navy", LightNavy },
        { "light navy blue", LightNavyBlue },
        { "light neon green", LightNeonGreen },
        { "light olive", LightOlive },
        { "light olive green", LightOliveGreen },
        { "light orange", LightOrange },
        { "light pastel green", LightPastelGreen },
        { "light peach", LightPeach },
        { "light pea green", LightPeaGreen },
        { "light periwinkle", LightPeriwinkle },
        { "light pink", LightPink },
        { "light plum", LightPlum },
        { "light purple", LightPurple },
        { "light red", LightRed },
        { "light rose", LightRose },
        { "light royal blue", LightRoyalBlue },
        { "light sage", LightSage },
        { "light salmon", LightSalmon },
        { "light seafoam", LightSeafoam },
        { "light seafoam green", LightSeafoamGreen },
        { "light sea green", LightSeaGreen },
        { "light sky blue", LightSkyBlue },
        { "light tan", LightTan },
        { "light teal", LightTeal },
        { "light turquoise", LightTurquoise },
        { "light urple", LightUrple },
        { "light violet", LightViolet },
        { "light yellow", LightYellow },
        { "light yellow green", LightYellowGreen },
        { "light yellowish green", LightYellowishGreen },
        { "lilac", Lilac },
        { "liliac", Liliac },
        { "lime", Lime },
        { "lime green", LimeGreen },
        { "lime yellow", LimeYellow },
        { "lipstick", Lipstick },
        { "lipstick red", LipstickRed },
        { "macaroni and cheese", MacaroniAndCheese },
        { "magenta", Magenta },
        { "mahogany", Mahogany },
        { "maize", Maize },
        { "mango", Mango },
        { "manilla", Manilla },
        { "marigold", Marigold },
        { "marine", Marine },
        { "marine blue", MarineBlue },
        { "maroon", Maroon },
        { "mauve", Mauve },
        { "medium blue", MediumBlue },
        { "medium brown", MediumBrown },
        { "medium green", MediumGreen },
        { "medium grey", MediumGrey },
        { "medium pink", MediumPink },
        { "medium purple", MediumPurple },
        { "melon", Melon },
        { "merlot", Merlot },
        { "metallic blue", MetallicBlue },
        { "mid blue", MidBlue },
        { "mid green", MidGreen },
        { "midnight", Midnight },
        { "midnight blue", MidnightBlue },
        { "midnight purple", MidnightPurple },
        { "military green", MilitaryGreen },
        { "milk chocolate", MilkChocolate },
        { "mint", Mint },
        { "mint green", MintGreen },
        { "minty green", MintyGreen },
        { "mocha", Mocha },
        { "moss", Moss },
        { "moss green", MossGreen },
        { "mossy green", MossyGreen },
        { "mud", Mud },
        { "mud brown", MudBrown },
        { "muddy brown", MuddyBrown },
        { "muddy green", MuddyGreen },
        { "muddy yellow", MuddyYellow },
        { "mud green", MudGreen },
        { "mulberry", Mulberry },
        { "murky green", MurkyGreen },
        { "mushroom", Mushroom },
        { "mustard", Mustard },
        { "mustard brown", MustardBrown },
        { "mustard green", MustardGreen },
        { "mustard yellow", MustardYellow },
        { "muted blue", MutedBlue },
        { "muted green", MutedGreen },
        { "muted pink", MutedPink },
        { "muted purple", MutedPurple },
        { "nasty green", NastyGreen },
        { "navy", Navy },
        { "navy blue", NavyBlue },
        { "navy green", NavyGreen },
        { "neon blue", NeonBlue },
        { "neon green", NeonGreen },
        { "neon pink", NeonPink },
        { "neon purple", NeonPurple },
        { "neon red", NeonRed },
        { "neon yellow", NeonYellow },
        { "nice blue", NiceBlue },
        { "night blue", NightBlue },
        { "ocean", Ocean },
        { "ocean blue", OceanBlue },
        { "ocean green", OceanGreen },
        { "ocher", Ocher },
        { "ochre", Ochre },
        { "ocre", Ocre },
        { "off blue", OffBlue },
        { "off green", OffGreen },
        { "off white", OffWhite },
        { "off yellow", OffYellow },
        { "old pink", OldPink },
        { "old rose", OldRose },
        { "olive", Olive },
        { "olive brown", OliveBrown },
        { "olive drab", OliveDrab },
        { "olive green", OliveGreen },
        { "olive yellow", OliveYellow },
        { "orange", Orange },
        { "orange brown", OrangeBrown },
        { "orangeish", Orangeish },
        { "orange pink", OrangePink },
        { "orange red", OrangeRed },
        { "orangered", Orangered },
        { "orangey brown", OrangeyBrown },
        { "orange yellow", OrangeYellow },
        { "orangey red", OrangeyRed },
        { "orangey yellow", OrangeyYellow },
        { "orangish", Orangish },
        { "orangish brown", OrangishBrown },
        { "orangish red", OrangishRed },
        { "orchid", Orchid },
        { "pale", Pale },
        { "pale aqua", PaleAqua },
        { "pale blue", PaleBlue },
        { "pale brown", PaleBrown },
        { "pale cyan", PaleCyan },
        { "pale gold", PaleGold },
        { "pale green", PaleGreen },
        { "pale grey", PaleGrey },
        { "pale lavender", PaleLavender },
        { "pale light green", PaleLightGreen },
        { "pale lilac", PaleLilac },
        { "pale lime", PaleLime },
        { "pale lime green", PaleLimeGreen },
        { "pale magenta", PaleMagenta },
        { "pale mauve", PaleMauve },
        { "pale olive", PaleOlive },
        { "pale olive green", PaleOliveGreen },
        { "pale orange", PaleOrange },
        { "pale peach", PalePeach },
        { "pale pink", PalePink },
        { "pale purple", PalePurple },
        { "pale red", PaleRed },
        { "pale rose", PaleRose },
        { "pale salmon", PaleSalmon },
        { "pale sky blue", PaleSkyBlue },
        { "pale teal", PaleTeal },
        { "pale turquoise", PaleTurquoise },
        { "pale violet", PaleViolet },
        { "pale yellow", PaleYellow },
        { "parchment", Parchment },
        { "pastel blue", PastelBlue },
        { "pastel green", PastelGreen },
        { "pastel orange", PastelOrange },
        { "pastel pink", PastelPink },
        { "pastel purple", PastelPurple },
        { "pastel red", PastelRed },
        { "pastel yellow", PastelYellow },
        { "pea", Pea },
        { "peach", Peach },
        { "peachy pink", PeachyPink },
        { "peacock blue", PeacockBlue },
        { "pea green", PeaGreen },
        { "pear", Pear },
        { "pea soup", PeaSoup },
        { "pea soup green", PeaSoupGreen },
        { "periwinkle", Periwinkle },
        { "periwinkle blue", PeriwinkleBlue },
        { "perrywinkle", Perrywinkle },
        { "petrol", Petrol },
        { "pig pink", PigPink },
        { "pine", Pine },
        { "pine green", PineGreen },
        { "pink", Pink },
        { "pink/purple", Pink_Purple },
        { "pinkish", Pinkish },
        { "pinkish brown", PinkishBrown },
        { "pinkish grey", PinkishGrey },
        { "pinkish orange", PinkishOrange },
        { "pinkish purple", PinkishPurple },
        { "pinkish red", PinkishRed },
        { "pinkish tan", PinkishTan },
        { "pink purple", PinkPurple },
        { "pink red", PinkRed },
        { "pinky", Pinky },
        { "pinky purple", PinkyPurple },
        { "pinky red", PinkyRed },
        { "piss yellow", PissYellow },
        { "pistachio", Pistachio },
        { "plum", Plum },
        { "plum purple", PlumPurple },
        { "poison green", PoisonGreen },
        { "poo", Poo },
        { "poo brown", PooBrown },
        { "poop", Poop },
        { "poop brown", PoopBrown },
        { "poop green", PoopGreen },
        { "powder blue", PowderBlue },
        { "powder pink", PowderPink },
        { "primary blue", PrimaryBlue },
        { "prussian blue", PrussianBlue },
        { "puce", Puce },
        { "puke", Puke },
        { "puke brown", PukeBrown },
        { "puke green", PukeGreen },
        { "puke yellow", PukeYellow },
        { "pumpkin", Pumpkin },
        { "pumpkin orange", PumpkinOrange },
        { "pure blue", PureBlue },
        { "purple", Purple },
        { "purple/blue", Purple_Blue },
        { "purple/pink", Purple_Pink },
        { "purple blue", PurpleBlue },
        { "purple brown", PurpleBrown },
        { "purple grey", PurpleGrey },
        { "purpleish", Purpleish },
        { "purpleish blue", PurpleishBlue },
        { "purpleish pink", PurpleishPink },
        { "purple pink", PurplePink },
        { "purple red", PurpleRed },
        { "purpley", Purpley },
        { "purpley blue", PurpleyBlue },
        { "purpley grey", PurpleyGrey },
        { "purpley pink", PurpleyPink },
        { "purplish", Purplish },
        { "purplish blue", PurplishBlue },
        { "purplish brown", PurplishBrown },
        { "purplish grey", PurplishGrey },
        { "purplish pink", PurplishPink },
        { "purplish red", PurplishRed },
        { "purply", Purply },
        { "purply blue", PurplyBlue },
        { "purply pink", PurplyPink },
        { "putty", Putty },
        { "racing green", RacingGreen },
        { "radioactive green", RadioactiveGreen },
        { "raspberry", Raspberry },
        { "raw sienna", RawSienna },
        { "raw umber", RawUmber },
        { "really light blue", ReallyLightBlue },
        { "red", Red },
        { "red brown", RedBrown },
        { "reddish", Reddish },
        { "reddish brown", ReddishBrown },
        { "reddish grey", ReddishGrey },
        { "reddish orange", ReddishOrange },
        { "reddish pink", ReddishPink },
        { "reddish purple", ReddishPurple },
        { "reddy brown", ReddyBrown },
        { "red orange", RedOrange },
        { "red pink", RedPink },
        { "red purple", RedPurple },
        { "red violet", RedViolet },
        { "red wine", RedWine },
        { "rich blue", RichBlue },
        { "rich purple", RichPurple },
        { "robin egg blue", RobinEggBlue },
        { "robin's egg", RobinsEgg },
        { "robin's egg blue", RobinsEggBlue },
        { "rosa", Rosa },
        { "rose", Rose },
        { "rose pink", RosePink },
        { "rose red", RoseRed },
        { "rosy pink", RosyPink },
        { "rouge", Rouge },
        { "royal", Royal },
        { "royal blue", RoyalBlue },
        { "royal purple", RoyalPurple },
        { "ruby", Ruby },
        { "russet", Russet },
        { "rust", Rust },
        { "rust brown", RustBrown },
        { "rust orange", RustOrange },
        { "rust red", RustRed },
        { "rusty orange", RustyOrange },
        { "rusty red", RustyRed },
        { "saffron", Saffron },
        { "sage", Sage },
        { "sage green", SageGreen },
        { "salmon", Salmon },
        { "salmon pink", SalmonPink },
        { "sand", Sand },
        { "sand brown", SandBrown },
        { "sandstone", Sandstone },
        { "sandy", Sandy },
        { "sandy brown", SandyBrown },
        { "sand yellow", SandYellow },
        { "sandy yellow", SandyYellow },
        { "sap green", SapGreen },
        { "sapphire", Sapphire },
        { "scarlet", Scarlet },
        { "sea", Sea },
        { "sea blue", SeaBlue },
        { "seafoam", Seafoam },
        { "seafoam blue", SeafoamBlue },
        { "seafoam green", SeafoamGreen },
        { "sea green", SeaGreen },
        { "seaweed", Seaweed },
        { "seaweed green", SeaweedGreen },
        { "sepia", Sepia },
        { "shamrock", Shamrock },
        { "shamrock green", ShamrockGreen },
        { "shit", Shit },
        { "shit brown", ShitBrown },
        { "shit green", ShitGreen },
        { "shocking pink", ShockingPink },
        { "sick green", SickGreen },
        { "sickly green", SicklyGreen },
        { "sickly yellow", SicklyYellow },
        { "sienna", Sienna },
        { "silver", Silver },
        { "sky", Sky },
        { "sky blue", SkyBlue },
        { "slate", Slate },
        { "slate blue", SlateBlue },
        { "slate green", SlateGreen },
        { "slate grey", SlateGrey },
        { "slime green", SlimeGreen },
        { "snot", Snot },
        { "snot green", SnotGreen },
        { "soft blue", SoftBlue },
        { "soft green", SoftGreen },
        { "soft pink", SoftPink },
        { "soft purple", SoftPurple },
        { "spearmint", Spearmint },
        { "spring green", SpringGreen },
        { "spruce", Spruce },
        { "squash", Squash },
        { "steel", Steel },
        { "steel blue", SteelBlue },
        { "steel grey", SteelGrey },
        { "stone", Stone },
        { "stormy blue", StormyBlue },
        { "straw", Straw },
        { "strawberry", Strawberry },
        { "strong blue", StrongBlue },
        { "strong pink", StrongPink },
        { "sunflower", Sunflower },
        { "sunflower yellow", SunflowerYellow },
        { "sunny yellow", SunnyYellow },
        { "sunshine yellow", SunshineYellow },
        { "sun yellow", SunYellow },
        { "swamp", Swamp },
        { "swamp green", SwampGreen },
        { "tan", Tan },
        { "tan brown", TanBrown },
        { "tangerine", Tangerine },
        { "tan green", TanGreen },
        { "taupe", Taupe },
        { "tea", Tea },
        { "tea green", TeaGreen },
        { "teal", Teal },
        { "teal blue", TealBlue },
        { "teal green", TealGreen },
        { "tealish", Tealish },
        { "tealish green", TealishGreen },
        { "terracota", Terracota },
        { "terra cotta", TerraCotta },
        { "terracotta", Terracotta },
        { "tiffany blue", TiffanyBlue },
        { "tomato", Tomato },
        { "tomato red", TomatoRed },
        { "topaz", Topaz },
        { "toupe", Toupe },
        { "toxic green", ToxicGreen },
        { "tree green", TreeGreen },
        { "true blue", TrueBlue },
        { "true green", TrueGreen },
        { "turquoise", Turquoise },
        { "turquoise blue", TurquoiseBlue },
        { "turquoise green", TurquoiseGreen },
        { "turtle green", TurtleGreen },
        { "twilight", Twilight },
        { "twilight blue", TwilightBlue },
        { "ugly blue", UglyBlue },
        { "ugly brown", UglyBrown },
        { "ugly green", UglyGreen },
        { "ugly pink", UglyPink },
        { "ugly purple", UglyPurple },
        { "ugly yellow", UglyYellow },
        { "ultramarine", Ultramarine },
        { "ultramarine blue", UltramarineBlue },
        { "umber", Umber },
        { "velvet", Velvet },
        { "vermillion", Vermillion },
        { "very dark blue", VeryDarkBlue },
        { "very dark brown", VeryDarkBrown },
        { "very dark green", VeryDarkGreen },
        { "very dark purple", VeryDarkPurple },
        { "very light blue", VeryLightBlue },
        { "very light brown", VeryLightBrown },
        { "very light green", VeryLightGreen },
        { "very light pink", VeryLightPink },
        { "very light purple", VeryLightPurple },
        { "very pale blue", VeryPaleBlue },
        { "very pale green", VeryPaleGreen },
        { "vibrant blue", VibrantBlue },
        { "vibrant green", VibrantGreen },
        { "vibrant purple", VibrantPurple },
        { "violet", Violet },
        { "violet blue", VioletBlue },
        { "violet pink", VioletPink },
        { "violet red", VioletRed },
        { "viridian", Viridian },
        { "vivid blue", VividBlue },
        { "vivid green", VividGreen },
        { "vivid purple", VividPurple },
        { "vomit", Vomit },
        { "vomit green", VomitGreen },
        { "vomit yellow", VomitYellow },
        { "warm blue", WarmBlue },
        { "warm brown", WarmBrown },
        { "warm grey", WarmGrey },
        { "warm pink", WarmPink },
        { "warm purple", WarmPurple },
        { "washed out green", WashedOutGreen },
        { "water blue", WaterBlue },
        { "watermelon", Watermelon },
        { "weird green", WeirdGreen },
        { "wheat", Wheat },
        { "white", White },
        { "windows blue", WindowsBlue },
        { "wine", Wine },
        { "wine red", WineRed },
        { "wintergreen", Wintergreen },
        { "wisteria", Wisteria },
        { "yellow", Yellow },
        { "yellow/green", Yellow_Green },
        { "yellow brown", YellowBrown },
        { "yellowgreen", Yellowgreen },
        { "yellow green", YellowGreen },
        { "yellowish", Yellowish },
        { "yellowish brown", YellowishBrown },
        { "yellowish green", YellowishGreen },
        { "yellowish orange", YellowishOrange },
        { "yellowish tan", YellowishTan },
        { "yellow ochre", YellowOchre },
        { "yellow orange", YellowOrange },
        { "yellow tan", YellowTan },
        { "yellowy brown", YellowyBrown },
        { "yellowy green", YellowyGreen }
    };
}