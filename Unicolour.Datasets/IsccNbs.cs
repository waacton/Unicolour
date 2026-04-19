namespace Wacton.Unicolour.Datasets;

// https://nvlpubs.nist.gov/nistpubs/jres/61/jresv61n5p427_A1b.pdf
public static class IsccNbs
{
    private static readonly Configuration Config = new(RgbConfiguration.StandardRgb, xyzConfig: new(Illuminant.C, Observer.Degree2));

    public static readonly Unicolour VividPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.5, "R"), 7, 13);
    public static readonly Unicolour StrongPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.5, "R"), 7.5, 9.1);
    public static readonly Unicolour DeepPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.9, "R"), 6.0, 11.1);
    public static readonly Unicolour LightPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.5, "R"), 8.6, 5.2);
    public static readonly Unicolour ModeratePink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.5, "R"), 7.2, 5.2);
    public static readonly Unicolour DarkPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.5, "R"), 6.0, 6.0);
    public static readonly Unicolour PalePink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.5, "R"), 8.8, 2.3);
    public static readonly Unicolour GrayishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.5, "R"), 7.2, 2.3);
    public static readonly Unicolour PinkishWhite = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.6, "R"), 9.2, 1.0);
    public static readonly Unicolour PinkishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.6, "R"), 7.5, 1.0);
    public static readonly Unicolour VividRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5, "R"), 4, 15);
    public static readonly Unicolour StrongRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "R"), 4.5, 12.0);
    public static readonly Unicolour DeepRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "R"), 2.8, 9.9);
    public static readonly Unicolour VeryDeepRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "R"), 1.4, 9.0);
    public static readonly Unicolour ModerateRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "R"), 4.5, 9.1);
    public static readonly Unicolour DarkRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "R"), 2.8, 6.8);
    public static readonly Unicolour VeryDarkRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.5, "R"), 1.2, 4.9);
    public static readonly Unicolour LightGrayishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.9, "R"), 6.0, 3.4);
    public static readonly Unicolour GrayishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.6, "R"), 4.5, 4.7);
    public static readonly Unicolour DarkGrayishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.5, "R"), 2.7, 2.2);
    public static readonly Unicolour BlackishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.5, "R"), 1.1, 1.6);
    public static readonly Unicolour ReddishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.5, "R"), 5.5, 1.1);
    public static readonly Unicolour DarkReddishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "R"), 3.5, 1.1);
    public static readonly Unicolour ReddishBlack = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "R"), 0.9, 0.8);
    public static readonly Unicolour VividYellowishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "R"), 7, 13);
    public static readonly Unicolour StrongYellowishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "R"), 7.5, 9.0);
    public static readonly Unicolour DeepYellowishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.6, "R"), 6.0, 12.4);
    public static readonly Unicolour LightYellowishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.7, "YR"), 8.6, 4.8);
    public static readonly Unicolour ModerateYellowishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.7, "YR"), 7.2, 4.8);
    public static readonly Unicolour DarkYellowishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "R"), 6.0, 6.0);
    public static readonly Unicolour PaleYellowishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.1, "YR"), 8.8, 2.2);
    public static readonly Unicolour GrayishYellowishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "YR"), 7.2, 2.3);
    public static readonly Unicolour BrownishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.5, "YR"), 7.2, 2.2);
    public static readonly Unicolour VividReddishOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "R"), 5.5, 15.5);
    public static readonly Unicolour StrongReddishOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "R"), 5.5, 12.0);
    public static readonly Unicolour DeepReddishOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "R"), 4.0, 12.0);
    public static readonly Unicolour ModerateReddishOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "R"), 5.5, 9.1);
    public static readonly Unicolour DarkReddishOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "R"), 4.0, 9.1);
    public static readonly Unicolour GrayishReddishOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "YR"), 5.5, 6.0);
    public static readonly Unicolour StrongReddishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "YR"), 3.0, 11.2);
    public static readonly Unicolour DeepReddishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.8, "R"), 1.5, 7.5);
    public static readonly Unicolour LightReddishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "YR"), 5.5, 4.1);
    public static readonly Unicolour ModerateReddishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.6, "R"), 3.4, 5.2);
    public static readonly Unicolour DarkReddishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "R"), 1.3, 3.6);
    public static readonly Unicolour LightGrayishReddishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.0, "YR"), 5.5, 2.3);
    public static readonly Unicolour GrayishReddishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.9, "R"), 3.4, 2.3);
    public static readonly Unicolour DarkGrayishReddishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "R"), 2.0, 2.0);
    public static readonly Unicolour VividOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "YR"), 6.6, 16);
    public static readonly Unicolour BrilliantOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "YR"), 8.0, 12.1);
    public static readonly Unicolour StrongOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "YR"), 6.5, 12.1);
    public static readonly Unicolour DeepOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "YR"), 5.0, 12.1);
    public static readonly Unicolour LightOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "YR"), 8, 8.1);
    public static readonly Unicolour ModerateOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.6, "YR"), 6.5, 8.1);
    public static readonly Unicolour BrownishOrange = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "YR"), 5.0, 8.2);
    public static readonly Unicolour StrongBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "YR"), 3.5, 8);
    public static readonly Unicolour DeepBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "YR"), 2.1, 6);
    public static readonly Unicolour LightBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.5, "YR"), 5.5, 4.6);
    public static readonly Unicolour ModerateBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.5, "YR"), 3.5, 3.9);
    public static readonly Unicolour DarkBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.5, "YR"), 1.6, 3.6);
    public static readonly Unicolour LightGrayishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.5, "YR"), 5.5, 2.2);
    public static readonly Unicolour GrayishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.6, "YR"), 3.5, 2.0);
    public static readonly Unicolour DarkGrayishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.5, "YR"), 2.0, 1.7);
    public static readonly Unicolour LightBrownishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.8, "YR"), 5.5, 1.0);
    public static readonly Unicolour BrownishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.4, "YR"), 3.5, 0.9);
    public static readonly Unicolour BrownishBlack = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.5, "YR"), 0.8, 0.8);
    public static readonly Unicolour VividOrangeYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "YR"), 7.2, 16);
    public static readonly Unicolour BrilliantOrangeYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "YR"), 8.4, 12.1);
    public static readonly Unicolour StrongOrangeYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "YR"), 7.2, 12.1);
    public static readonly Unicolour DeepOrangeYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "YR"), 6.0, 12.1);
    public static readonly Unicolour LightOrangeYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "YR"), 8.6, 8.1);
    public static readonly Unicolour ModerateOrangeYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "YR"), 7.2, 8.1);
    public static readonly Unicolour DarkOrangeYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "YR"), 6.0, 8.1);
    public static readonly Unicolour PaleOrangeYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.1, "YR"), 8.6, 4.4);
    public static readonly Unicolour StrongYellowishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "YR"), 4.6, 9.6);
    public static readonly Unicolour DeepYellowishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "YR"), 2.9, 6);
    public static readonly Unicolour LightYellowishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.2, "YR"), 6.6, 4.7);
    public static readonly Unicolour ModerateYellowishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "YR"), 4.5, 4.0);
    public static readonly Unicolour DarkYellowishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "YR"), 2.2, 3.7);
    public static readonly Unicolour LightGrayishYellowishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "YR"), 6.4, 2.4);
    public static readonly Unicolour GrayishYellowishBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "YR"), 4.6, 2.1);
    public static readonly Unicolour DarkGrayishYellowBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "YR"), 2.4, 1.8);
    public static readonly Unicolour VividYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "Y"), 7.8, 14.5);
    public static readonly Unicolour BrilliantYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "Y"), 8.8, 9.5);
    public static readonly Unicolour StrongYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "Y"), 7.2, 9.5);
    public static readonly Unicolour DeepYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "Y"), 6.0, 9.5);
    public static readonly Unicolour LightYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "Y"), 8.8, 6.6);
    public static readonly Unicolour ModerateYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "Y"), 7.2, 6.6);
    public static readonly Unicolour DarkYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "Y"), 6.0, 6.6);
    public static readonly Unicolour PaleYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.2, "Y"), 8.9, 3.6);
    public static readonly Unicolour GrayishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.2, "Y"), 7.2, 3.6);
    public static readonly Unicolour DarkGrayishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "Y"), 6.0, 4.1);
    public static readonly Unicolour YellowishWhite = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.1, "Y"), 9.2, 1.3);
    public static readonly Unicolour YellowishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.1, "Y"), 7.5, 1.3);
    public static readonly Unicolour LightOliveBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.5, "Y"), 5.0, 8.4);
    public static readonly Unicolour ModerateOliveBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.5, "Y"), 3.5, 6);
    public static readonly Unicolour DarkOliveBrown = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.5, "Y"), 1.8, 2.5);
    public static readonly Unicolour VividGreenishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "Y"), 7.8, 14.5);
    public static readonly Unicolour BrilliantGreenishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "Y"), 8.8, 9.5);
    public static readonly Unicolour StrongGreenishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "Y"), 7.2, 9.5);
    public static readonly Unicolour DeepGreenishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "Y"), 6.0, 9.5);
    public static readonly Unicolour LightGreenishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "Y"), 8.8, 6.6);
    public static readonly Unicolour ModerateGreenishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "Y"), 7.2, 6.6);
    public static readonly Unicolour DarkGreenishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "Y"), 6.0, 6.6);
    public static readonly Unicolour PaleGreenishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "Y"), 8.9, 4.1);
    public static readonly Unicolour GrayishGreenishYellow = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "Y"), 7.2, 4.1);
    public static readonly Unicolour LightOlive = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.1, "Y"), 5.1, 8.0);
    public static readonly Unicolour ModerateOlive = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "Y"), 3.5, 5.8);
    public static readonly Unicolour DarkOlive = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "Y"), 1.7, 3.2);
    public static readonly Unicolour LightGrayishOlive = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.3, "Y"), 5.5, 2.4);
    public static readonly Unicolour GrayishOlive = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.1, "Y"), 3.5, 2.3);
    public static readonly Unicolour DarkGrayishOlive = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "Y"), 2.0, 2.0);
    public static readonly Unicolour LightOliveGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.4, "Y"), 5.5, 1.3);
    public static readonly Unicolour OliveGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.1, "Y"), 3.5, 1.0);
    public static readonly Unicolour OliveBlack = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "Y"), 0.9, 0.8);
    public static readonly Unicolour VividYellowGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 7.5, 14.5);
    public static readonly Unicolour BrilliantYellowGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 8.5, 9.1);
    public static readonly Unicolour StrongYellowGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 6.0, 9.1);
    public static readonly Unicolour DeepYellowGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 4.0, 9.1);
    public static readonly Unicolour LightYellowGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 8.6, 5.2);
    public static readonly Unicolour ModerateYellowGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 6.0, 5.2);
    public static readonly Unicolour PaleYellowGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.5, "GY"), 8.7, 2.2);
    public static readonly Unicolour GrayishYellowGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.4, "GY"), 6.1, 2.2);
    public static readonly Unicolour StrongOliveGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 3.2, 7.8);
    public static readonly Unicolour DeepOliveGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 2.4, 7.1);
    public static readonly Unicolour ModerateOliveGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 3.5, 5.2);
    public static readonly Unicolour DarkOliveGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 1.8, 3.7);
    public static readonly Unicolour GrayishOliveGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 3.5, 2.2);
    public static readonly Unicolour DarkGrayishOliveGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.0, "GY"), 2.0, 2.0);
    public static readonly Unicolour VividYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 7, 15.5);
    public static readonly Unicolour BrilliantYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 8.0, 9.1);
    public static readonly Unicolour StrongYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 5.5, 9.1);
    public static readonly Unicolour DeepYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 3.5, 11.5);
    public static readonly Unicolour VeryDeepYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 2.0, 9.2);
    public static readonly Unicolour VeryLightYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 9.1, 5.1);
    public static readonly Unicolour LightYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 7.5, 5.1);
    public static readonly Unicolour ModerateYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 5.5, 5.1);
    public static readonly Unicolour DarkYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 3.5, 5.1);
    public static readonly Unicolour VeryDarkYellowishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.5, "G"), 1.5, 4.9);
    public static readonly Unicolour VividGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.5, "G"), 5.2, 18);
    public static readonly Unicolour BrilliantGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "G"), 7.4, 9.1);
    public static readonly Unicolour StrongGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "G"), 4.5, 9.1);
    public static readonly Unicolour DeepGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "G"), 2.3, 9.1);
    public static readonly Unicolour VeryLightGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "G"), 8.6, 5.1);
    public static readonly Unicolour LightGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "G"), 6.5, 5.1);
    public static readonly Unicolour ModerateGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "G"), 4.5, 5.1);
    public static readonly Unicolour DarkGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "G"), 2.7, 5.0);
    public static readonly Unicolour VeryDarkGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "G"), 1.3, 4.9);
    public static readonly Unicolour VeryPaleGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "G"), 8.7, 1.8);
    public static readonly Unicolour PaleGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "G"), 6.5, 1.8);
    public static readonly Unicolour GrayishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "G"), 4.5, 1.8);
    public static readonly Unicolour DarkGrayishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "G"), 2.8, 1.6);
    public static readonly Unicolour BlackishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "G"), 1.1, 1.4);
    public static readonly Unicolour GreenishWhite = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "G"), 9.2, 0.8);
    public static readonly Unicolour LightGreenishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "G"), 7.5, 0.8);
    public static readonly Unicolour GreenishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "G"), 5.5, 0.8);
    public static readonly Unicolour DarkGreenishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "G"), 3.5, 0.8);
    public static readonly Unicolour GreenishBlack = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.5, "G"), 0.9, 0.7);
    public static readonly Unicolour VividBluishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "BG"), 5.3, 15.5);
    public static readonly Unicolour BrilliantBluishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "BG"), 7.3, 9.0);
    public static readonly Unicolour StrongBluishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "BG"), 4.5, 9.0);
    public static readonly Unicolour DeepBluishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "BG"), 2.3, 9.0);
    public static readonly Unicolour VeryLightBluishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "BG"), 8.5, 5.0);
    public static readonly Unicolour LightBluishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "BG"), 6.5, 5.0);
    public static readonly Unicolour ModerateBluishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "BG"), 4.5, 5.0);
    public static readonly Unicolour DarkBluishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "BG"), 2.7, 5.0);
    public static readonly Unicolour VeryDarkBluishGreen = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "BG"), 1.3, 4.9);
    public static readonly Unicolour VividGreenishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "B"), 5.2, 13);
    public static readonly Unicolour BrilliantGreenishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "B"), 6.8, 9.0);
    public static readonly Unicolour StrongGreenishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "B"), 4.5, 9.0);
    public static readonly Unicolour DeepGreenishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "B"), 2.5, 9.0);
    public static readonly Unicolour VeryLightGreenishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "B"), 8.2, 5.2);
    public static readonly Unicolour LightGreenishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "B"), 6.5, 5.2);
    public static readonly Unicolour ModerateGreenishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "B"), 4.5, 5.2);
    public static readonly Unicolour DarkGreenishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "B"), 2.7, 5.0);
    public static readonly Unicolour VeryDarkGreenishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.5, "B"), 1.3, 4.9);
    public static readonly Unicolour VividBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.0, "PB"), 4.2, 15);
    public static readonly Unicolour BrilliantBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.0, "PB"), 6.4, 11.0);
    public static readonly Unicolour StrongBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.0, "PB"), 4.2, 11.0);
    public static readonly Unicolour DeepBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.0, "PB"), 2.0, 9.0);
    public static readonly Unicolour VeryLightBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.4, "PB"), 8.2, 7.2);
    public static readonly Unicolour LightBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.7, "PB"), 6.5, 7.3);
    public static readonly Unicolour ModerateBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.9, "PB"), 4.2, 7.2);
    public static readonly Unicolour DarkBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(2.8, "PB"), 1.7, 5.0);
    public static readonly Unicolour VeryPaleBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.9, "PB"), 8.4, 3.0);
    public static readonly Unicolour PaleBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.9, "PB"), 6.5, 3.0);
    public static readonly Unicolour GrayishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(0.9, "PB"), 4.2, 3.0);
    public static readonly Unicolour DarkGrayishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "B"), 2.6, 1.9);
    public static readonly Unicolour BlackishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "B"), 1.1, 1.5);
    public static readonly Unicolour BluishWhite = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "B"), 9.1, 1.0);
    public static readonly Unicolour LightBluishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "B"), 7.5, 1.0);
    public static readonly Unicolour BluishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "B"), 5.5, 1.0);
    public static readonly Unicolour DarkBluishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "B"), 3.5, 1.0);
    public static readonly Unicolour BluishBlack = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.5, "B"), 1.0, 0.7);
    public static readonly Unicolour VividPurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "PB"), 2.5, 22);
    public static readonly Unicolour BrilliantPurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "PB"), 6.0, 10.8);
    public static readonly Unicolour StrongPurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "PB"), 4.1, 11.4);
    public static readonly Unicolour DeepPurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "PB"), 1.6, 9.1);
    public static readonly Unicolour VeryLightPurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "PB"), 7.8, 6.1);
    public static readonly Unicolour LightPurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.5, "PB"), 6.0, 6.8);
    public static readonly Unicolour ModeratePurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(8.0, "PB"), 3.5, 6.9);
    public static readonly Unicolour DarkPurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.9, "PB"), 1.1, 4.7);
    public static readonly Unicolour VeryPalePurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "PB"), 8.2, 4.1);
    public static readonly Unicolour PalePurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "PB"), 6.0, 4.1);
    public static readonly Unicolour GrayishPurplishBlue = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.2, "PB"), 3.3, 4.0);
    public static readonly Unicolour VividViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 2.7, 21);
    public static readonly Unicolour BrilliantViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 5.9, 11.1);
    public static readonly Unicolour StrongViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 3.5, 11.1);
    public static readonly Unicolour DeepViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 1.4, 10.3);
    public static readonly Unicolour VeryLightViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 7.9, 7);
    public static readonly Unicolour LightViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 6.0, 7.2);
    public static readonly Unicolour ModerateViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 3.5, 7.2);
    public static readonly Unicolour DarkViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 1.3, 5.0);
    public static readonly Unicolour VeryPaleViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 8.2, 4.1);
    public static readonly Unicolour PaleViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 6.0, 4.1);
    public static readonly Unicolour GrayishViolet = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "P"), 3.3, 4.1);
    public static readonly Unicolour VividPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 3.5, 20);
    public static readonly Unicolour BrilliantPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 6.7, 11.1);
    public static readonly Unicolour StrongPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 4.5, 11.1);
    public static readonly Unicolour DeepPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 2.8, 10.2);
    public static readonly Unicolour VeryDeepPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 1.2, 10.2);
    public static readonly Unicolour VeryLightPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 8.0, 7.2);
    public static readonly Unicolour LightPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 6.5, 7.2);
    public static readonly Unicolour ModeratePurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 4.5, 7.2);
    public static readonly Unicolour DarkPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 2.7, 5.1);
    public static readonly Unicolour VeryDarkPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "P"), 1.1, 4.9);
    public static readonly Unicolour VeryPalePurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(5.4, "P"), 8.4, 3.3);
    public static readonly Unicolour PalePurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.8, "P"), 6.4, 3.0);
    public static readonly Unicolour GrayishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.8, "P"), 4.5, 2.9);
    public static readonly Unicolour DarkGrayishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(10.0, "P"), 2.8, 2.0);
    public static readonly Unicolour BlackishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(10.0, "P"), 1.0, 1.4);
    public static readonly Unicolour PurplishWhite = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "P"), 9.1, 1.0);
    public static readonly Unicolour LightPurplishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(9.0, "P"), 7.5, 1.0);
    public static readonly Unicolour PurplishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(10.0, "P"), 5.5, 1.0);
    public static readonly Unicolour DarkPurplishGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(10.0, "P"), 3.5, 1.0);
    public static readonly Unicolour PurplishBlack = new(Config, ColourSpace.Munsell, Hue.FromMunsell(10.0, "P"), 1.0, 0.7);
    public static readonly Unicolour VividReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 4, 19);
    public static readonly Unicolour StrongReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 4.5, 11.1);
    public static readonly Unicolour DeepReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 2.8, 10.3);
    public static readonly Unicolour VeryDeepReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 1.3, 10.3);
    public static readonly Unicolour LightReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 6.0, 7.2);
    public static readonly Unicolour ModerateReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 4.5, 7.2);
    public static readonly Unicolour DarkReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 2.7, 5.1);
    public static readonly Unicolour VeryDarkReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 1.1, 5.0);
    public static readonly Unicolour PaleReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 6.0, 4.1);
    public static readonly Unicolour GrayishReddishPurple = new(Config, ColourSpace.Munsell, Hue.FromMunsell(1.0, "RP"), 4.5, 4.1);
    public static readonly Unicolour BrilliantPurplishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "RP"), 7.9, 11);
    public static readonly Unicolour StrongPurplishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "RP"), 7, 14.5);
    public static readonly Unicolour DeepPurplishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "RP"), 6.0, 12.0);
    public static readonly Unicolour LightPurplishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "RP"), 8.3, 7.1);
    public static readonly Unicolour ModeratePurplishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "RP"), 7.0, 7.1);
    public static readonly Unicolour DarkPurplishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(6.0, "RP"), 6.0, 7.2);
    public static readonly Unicolour PalePurplishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "RP"), 8.5, 3.5);
    public static readonly Unicolour GrayishPurplishPink = new(Config, ColourSpace.Munsell, Hue.FromMunsell(4.0, "RP"), 7.0, 3.5);
    public static readonly Unicolour VividPurplishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "RP"), 4, 17);
    public static readonly Unicolour StrongPurplishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "RP"), 4.5, 11.9);
    public static readonly Unicolour DeepPurplishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "RP"), 2.8, 11.0);
    public static readonly Unicolour VeryDeepPurplishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "RP"), 1.3, 9.0);
    public static readonly Unicolour ModeratePurplishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "RP"), 4.5, 9.0);
    public static readonly Unicolour DarkPurplishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "RP"), 2.7, 6.3);
    public static readonly Unicolour VeryDarkPurplishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "RP"), 1.2, 4.9);
    public static readonly Unicolour LightGrayishPurplishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "RP"), 6.0, 4.0);
    public static readonly Unicolour GrayishPurplishRed = new(Config, ColourSpace.Munsell, Hue.FromMunsell(7.0, "RP"), 4.5, 5.2);
    public static readonly Unicolour White = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.0, "Y"), 9.25, 0.06);
    public static readonly Unicolour LightGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.0, "Y"), 7.5, 0.06);
    public static readonly Unicolour MediumGray = new(Config, ColourSpace.Munsell, Hue.FromMunsell(3.0, "Y"), 5.5, 0.06);
    public static readonly Unicolour DarkGray = new(Config, ColourSpace.Munsell, 3.5);
    public static readonly Unicolour Black = new(Config, ColourSpace.Munsell, 1.25);
    
    public static IEnumerable<Unicolour> All =>
    [
        VividPink, StrongPink, DeepPink, LightPink, ModeratePink, DarkPink, PalePink, GrayishPink, PinkishWhite, PinkishGray,
        VividRed, StrongRed, DeepRed, VeryDeepRed, ModerateRed, DarkRed, VeryDarkRed, LightGrayishRed, GrayishRed, DarkGrayishRed, BlackishRed, ReddishGray, DarkReddishGray, ReddishBlack,
        VividYellowishPink, StrongYellowishPink, DeepYellowishPink, LightYellowishPink, ModerateYellowishPink, DarkYellowishPink, PaleYellowishPink, GrayishYellowishPink, BrownishPink,
        VividReddishOrange, StrongReddishOrange, DeepReddishOrange, ModerateReddishOrange, DarkReddishOrange, GrayishReddishOrange, StrongReddishBrown, DeepReddishBrown, LightReddishBrown, ModerateReddishBrown, DarkReddishBrown, LightGrayishReddishBrown, GrayishReddishBrown, DarkGrayishReddishBrown,
        VividOrange, BrilliantOrange, StrongOrange, DeepOrange, LightOrange, ModerateOrange, BrownishOrange, StrongBrown, DeepBrown, LightBrown, ModerateBrown, DarkBrown, LightGrayishBrown, GrayishBrown, DarkGrayishBrown, LightBrownishGray, BrownishGray, BrownishBlack,
        VividOrangeYellow, BrilliantOrangeYellow, StrongOrangeYellow, DeepOrangeYellow, LightOrangeYellow, ModerateOrangeYellow, DarkOrangeYellow, PaleOrangeYellow, StrongYellowishBrown, DeepYellowishBrown, LightYellowishBrown, ModerateYellowishBrown, DarkYellowishBrown, LightGrayishYellowishBrown, GrayishYellowishBrown, DarkGrayishYellowBrown,
        VividYellow, BrilliantYellow, StrongYellow, DeepYellow, LightYellow, ModerateYellow, DarkYellow, PaleYellow, GrayishYellow, DarkGrayishYellow, YellowishWhite, YellowishGray, LightOliveBrown, ModerateOliveBrown, DarkOliveBrown,
        VividGreenishYellow, BrilliantGreenishYellow, StrongGreenishYellow, DeepGreenishYellow, LightGreenishYellow, ModerateGreenishYellow, DarkGreenishYellow, PaleGreenishYellow, GrayishGreenishYellow, LightOlive, ModerateOlive, DarkOlive, LightGrayishOlive, GrayishOlive, DarkGrayishOlive, LightOliveGray, OliveGray, OliveBlack,
        VividYellowGreen, BrilliantYellowGreen, StrongYellowGreen, DeepYellowGreen, LightYellowGreen, ModerateYellowGreen, PaleYellowGreen, GrayishYellowGreen, StrongOliveGreen, DeepOliveGreen, ModerateOliveGreen, DarkOliveGreen, GrayishOliveGreen, DarkGrayishOliveGreen,
        VividYellowishGreen, BrilliantYellowishGreen, StrongYellowishGreen, DeepYellowishGreen, VeryDeepYellowishGreen, VeryLightYellowishGreen, LightYellowishGreen, ModerateYellowishGreen, DarkYellowishGreen, VeryDarkYellowishGreen,
        VividGreen, BrilliantGreen, StrongGreen, DeepGreen, VeryLightGreen, LightGreen, ModerateGreen, DarkGreen, VeryDarkGreen, VeryPaleGreen, PaleGreen, GrayishGreen, DarkGrayishGreen, BlackishGreen, GreenishWhite, LightGreenishGray, GreenishGray, DarkGreenishGray, GreenishBlack,
        VividBluishGreen, BrilliantBluishGreen, StrongBluishGreen, DeepBluishGreen, VeryLightBluishGreen, LightBluishGreen, ModerateBluishGreen, DarkBluishGreen, VeryDarkBluishGreen, 
        VividGreenishBlue, BrilliantGreenishBlue, StrongGreenishBlue, DeepGreenishBlue, VeryLightGreenishBlue, LightGreenishBlue, ModerateGreenishBlue, DarkGreenishBlue, VeryDarkGreenishBlue,
        VividBlue, BrilliantBlue, StrongBlue, DeepBlue, VeryLightBlue, LightBlue, ModerateBlue, DarkBlue, VeryPaleBlue, PaleBlue, GrayishBlue, DarkGrayishBlue, BlackishBlue, BluishWhite, LightBluishGray, BluishGray, DarkBluishGray, BluishBlack,
        VividPurplishBlue, BrilliantPurplishBlue, StrongPurplishBlue, DeepPurplishBlue, VeryLightPurplishBlue, LightPurplishBlue, ModeratePurplishBlue, DarkPurplishBlue, VeryPalePurplishBlue, PalePurplishBlue, GrayishPurplishBlue,
        VividViolet, BrilliantViolet, StrongViolet, DeepViolet, VeryLightViolet, LightViolet, ModerateViolet, DarkViolet, VeryPaleViolet, PaleViolet, GrayishViolet,
        VividPurple, BrilliantPurple, StrongPurple, DeepPurple, VeryDeepPurple, VeryLightPurple, LightPurple, ModeratePurple, DarkPurple, VeryDarkPurple, VeryPalePurple, PalePurple, GrayishPurple, DarkGrayishPurple, BlackishPurple, PurplishWhite, LightPurplishGray, PurplishGray, DarkPurplishGray, PurplishBlack,
        VividReddishPurple, StrongReddishPurple, DeepReddishPurple, VeryDeepReddishPurple, LightReddishPurple, ModerateReddishPurple, DarkReddishPurple, VeryDarkReddishPurple, PaleReddishPurple, GrayishReddishPurple, BrilliantPurplishPink, StrongPurplishPink, DeepPurplishPink, LightPurplishPink, ModeratePurplishPink, DarkPurplishPink, PalePurplishPink, GrayishPurplishPink,
        VividPurplishRed, StrongPurplishRed, DeepPurplishRed, VeryDeepPurplishRed, ModeratePurplishRed, DarkPurplishRed, VeryDarkPurplishRed, LightGrayishPurplishRed, GrayishPurplishRed,
        White, LightGray, MediumGray, DarkGray, Black
    ];

    public static Unicolour? FromName(string name) => Utils.FromName(name, Lookup);
    
    private static readonly Dictionary<string, Unicolour> Lookup = new()
    {
        { "vivid pink", VividPink },
        { "strong pink", StrongPink },
        { "deep pink", DeepPink },
        { "light pink", LightPink },
        { "moderate pink", ModeratePink },
        { "dark pink", DarkPink },
        { "pale pink", PalePink },
        { "grayish pink", GrayishPink },
        { "pinkish white", PinkishWhite },
        { "pinkish gray", PinkishGray },
        { "vivid red", VividRed },
        { "strong red", StrongRed },
        { "deep red", DeepRed },
        { "very deep red", VeryDeepRed },
        { "moderate red", ModerateRed },
        { "dark red", DarkRed },
        { "very dark red", VeryDarkRed },
        { "light grayish red", LightGrayishRed },
        { "grayish red", GrayishRed },
        { "dark grayish red", DarkGrayishRed },
        { "blackish red", BlackishRed },
        { "reddish gray", ReddishGray },
        { "dark reddish gray", DarkReddishGray },
        { "reddish black", ReddishBlack },
        { "vivid yellowish pink", VividYellowishPink },
        { "strong yellowish pink", StrongYellowishPink },
        { "deep yellowish pink", DeepYellowishPink },
        { "light yellowish pink", LightYellowishPink },
        { "moderate yellowish pink", ModerateYellowishPink },
        { "dark yellowish pink", DarkYellowishPink },
        { "pale yellowish pink", PaleYellowishPink },
        { "grayish yellowish pink", GrayishYellowishPink },
        { "brownish pink", BrownishPink },
        { "vivid reddish orange", VividReddishOrange },
        { "strong reddish orange", StrongReddishOrange },
        { "deep reddish orange", DeepReddishOrange },
        { "moderate reddish orange", ModerateReddishOrange },
        { "dark reddish orange", DarkReddishOrange },
        { "grayish reddish orange", GrayishReddishOrange },
        { "strong reddish brown", StrongReddishBrown },
        { "deep reddish brown", DeepReddishBrown },
        { "light reddish brown", LightReddishBrown },
        { "moderate reddish brown", ModerateReddishBrown },
        { "dark reddish brown", DarkReddishBrown },
        { "light grayish reddish brown", LightGrayishReddishBrown },
        { "grayish reddish brown", GrayishReddishBrown },
        { "dark grayish reddish brown", DarkGrayishReddishBrown },
        { "vivid orange", VividOrange },
        { "brilliant orange", BrilliantOrange },
        { "strong orange", StrongOrange },
        { "deep orange", DeepOrange },
        { "light orange", LightOrange },
        { "moderate orange", ModerateOrange },
        { "brownish orange", BrownishOrange },
        { "strong brown", StrongBrown },
        { "deep brown", DeepBrown },
        { "light brown", LightBrown },
        { "moderate brown", ModerateBrown },
        { "dark brown", DarkBrown },
        { "light grayish brown", LightGrayishBrown },
        { "grayish brown", GrayishBrown },
        { "dark grayish brown", DarkGrayishBrown },
        { "light brownish gray", LightBrownishGray },
        { "brownish gray", BrownishGray },
        { "brownish black", BrownishBlack },
        { "vivid orange yellow", VividOrangeYellow },
        { "brilliant orange yellow", BrilliantOrangeYellow },
        { "strong orange yellow", StrongOrangeYellow },
        { "deep orange yellow", DeepOrangeYellow },
        { "light orange yellow", LightOrangeYellow },
        { "moderate orange yellow", ModerateOrangeYellow },
        { "dark orange yellow", DarkOrangeYellow },
        { "pale orange yellow", PaleOrangeYellow },
        { "strong yellowish brown", StrongYellowishBrown },
        { "deep yellowish brown", DeepYellowishBrown },
        { "light yellowish brown", LightYellowishBrown },
        { "moderate yellowish brown", ModerateYellowishBrown },
        { "dark yellowish brown", DarkYellowishBrown },
        { "light grayish yellowish brown", LightGrayishYellowishBrown },
        { "grayish yellowish brown", GrayishYellowishBrown },
        { "dark grayish yellow brown", DarkGrayishYellowBrown },
        { "vivid yellow", VividYellow },
        { "brilliant yellow", BrilliantYellow },
        { "strong yellow", StrongYellow },
        { "deep yellow", DeepYellow },
        { "light yellow", LightYellow },
        { "moderate yellow", ModerateYellow },
        { "dark yellow", DarkYellow },
        { "pale yellow", PaleYellow },
        { "grayish yellow", GrayishYellow },
        { "dark grayish yellow", DarkGrayishYellow },
        { "yellowish white", YellowishWhite },
        { "yellowish gray", YellowishGray },
        { "light olive brown", LightOliveBrown },
        { "moderate olive brown", ModerateOliveBrown },
        { "dark olive brown", DarkOliveBrown },
        { "vivid greenish yellow", VividGreenishYellow },
        { "brilliant greenish yellow", BrilliantGreenishYellow },
        { "strong greenish yellow", StrongGreenishYellow },
        { "deep greenish yellow", DeepGreenishYellow },
        { "light greenish yellow", LightGreenishYellow },
        { "moderate greenish yellow", ModerateGreenishYellow },
        { "dark greenish yellow", DarkGreenishYellow },
        { "pale greenish yellow", PaleGreenishYellow },
        { "grayish greenish yellow", GrayishGreenishYellow },
        { "light olive", LightOlive },
        { "moderate olive", ModerateOlive },
        { "dark olive", DarkOlive },
        { "light grayish olive", LightGrayishOlive },
        { "grayish olive", GrayishOlive },
        { "dark grayish olive", DarkGrayishOlive },
        { "light olive gray", LightOliveGray },
        { "olive gray", OliveGray },
        { "olive black", OliveBlack },
        { "vivid yellow green", VividYellowGreen },
        { "brilliant yellow green", BrilliantYellowGreen },
        { "strong yellow green", StrongYellowGreen },
        { "deep yellow green", DeepYellowGreen },
        { "light yellow green", LightYellowGreen },
        { "moderate yellow green", ModerateYellowGreen },
        { "pale yellow green", PaleYellowGreen },
        { "grayish yellow green", GrayishYellowGreen },
        { "strong olive green", StrongOliveGreen },
        { "deep olive green", DeepOliveGreen },
        { "moderate olive green", ModerateOliveGreen },
        { "dark olive green", DarkOliveGreen },
        { "grayish olive green", GrayishOliveGreen },
        { "dark grayish olive green", DarkGrayishOliveGreen },
        { "vivid yellowish green", VividYellowishGreen },
        { "brilliant yellowish green", BrilliantYellowishGreen },
        { "strong yellowish green", StrongYellowishGreen },
        { "deep yellowish green", DeepYellowishGreen },
        { "very deep yellowish green", VeryDeepYellowishGreen },
        { "very light yellowish green", VeryLightYellowishGreen },
        { "light yellowish green", LightYellowishGreen },
        { "moderate yellowish green", ModerateYellowishGreen },
        { "dark yellowish green", DarkYellowishGreen },
        { "very dark yellowish green", VeryDarkYellowishGreen },
        { "vivid green", VividGreen },
        { "brilliant green", BrilliantGreen },
        { "strong green", StrongGreen },
        { "deep green", DeepGreen },
        { "very light green", VeryLightGreen },
        { "light green", LightGreen },
        { "moderate green", ModerateGreen },
        { "dark green", DarkGreen },
        { "very dark green", VeryDarkGreen },
        { "very pale green", VeryPaleGreen },
        { "pale green", PaleGreen },
        { "grayish green", GrayishGreen },
        { "dark grayish green", DarkGrayishGreen },
        { "blackish green", BlackishGreen },
        { "greenish white", GreenishWhite },
        { "light greenish gray", LightGreenishGray },
        { "greenish gray", GreenishGray },
        { "dark greenish gray", DarkGreenishGray },
        { "greenish black", GreenishBlack },
        { "vivid bluish green", VividBluishGreen },
        { "brilliant bluish green", BrilliantBluishGreen },
        { "strong bluish green", StrongBluishGreen },
        { "deep bluish green", DeepBluishGreen },
        { "very light bluish green", VeryLightBluishGreen },
        { "light bluish green", LightBluishGreen },
        { "moderate bluish green", ModerateBluishGreen },
        { "dark bluish green", DarkBluishGreen },
        { "very dark bluish green", VeryDarkBluishGreen },
        { "vivid greenish blue", VividGreenishBlue },
        { "brilliant greenish blue", BrilliantGreenishBlue },
        { "strong greenish blue", StrongGreenishBlue },
        { "deep greenish blue", DeepGreenishBlue },
        { "very light greenish blue", VeryLightGreenishBlue },
        { "light greenish blue", LightGreenishBlue },
        { "moderate greenish blue", ModerateGreenishBlue },
        { "dark greenish blue", DarkGreenishBlue },
        { "very dark greenish blue", VeryDarkGreenishBlue },
        { "vivid blue", VividBlue },
        { "brilliant blue", BrilliantBlue },
        { "strong blue", StrongBlue },
        { "deep blue", DeepBlue },
        { "very light blue", VeryLightBlue },
        { "light blue", LightBlue },
        { "moderate blue", ModerateBlue },
        { "dark blue", DarkBlue },
        { "very pale blue", VeryPaleBlue },
        { "pale blue", PaleBlue },
        { "grayish blue", GrayishBlue },
        { "dark grayish blue", DarkGrayishBlue },
        { "blackish blue", BlackishBlue },
        { "bluish white", BluishWhite },
        { "light bluish gray", LightBluishGray },
        { "bluish gray", BluishGray },
        { "dark bluish gray", DarkBluishGray },
        { "bluish black", BluishBlack },
        { "vivid purplish blue", VividPurplishBlue },
        { "brilliant purplish blue", BrilliantPurplishBlue },
        { "strong purplish blue", StrongPurplishBlue },
        { "deep purplish blue", DeepPurplishBlue },
        { "very light purplish blue", VeryLightPurplishBlue },
        { "light purplish blue", LightPurplishBlue },
        { "moderate purplish blue", ModeratePurplishBlue },
        { "dark purplish blue", DarkPurplishBlue },
        { "very pale purplish blue", VeryPalePurplishBlue },
        { "pale purplish blue", PalePurplishBlue },
        { "grayish purplish blue", GrayishPurplishBlue },
        { "vivid violet", VividViolet },
        { "brilliant violet", BrilliantViolet },
        { "strong violet", StrongViolet },
        { "deep violet", DeepViolet },
        { "very light violet", VeryLightViolet },
        { "light violet", LightViolet },
        { "moderate violet", ModerateViolet },
        { "dark violet", DarkViolet },
        { "very pale violet", VeryPaleViolet },
        { "pale violet", PaleViolet },
        { "grayish violet", GrayishViolet },
        { "vivid purple", VividPurple },
        { "brilliant purple", BrilliantPurple },
        { "strong purple", StrongPurple },
        { "deep purple", DeepPurple },
        { "very deep purple", VeryDeepPurple },
        { "very light purple", VeryLightPurple },
        { "light purple", LightPurple },
        { "moderate purple", ModeratePurple },
        { "dark purple", DarkPurple },
        { "very dark purple", VeryDarkPurple },
        { "very pale purple", VeryPalePurple },
        { "pale purple", PalePurple },
        { "grayish purple", GrayishPurple },
        { "dark grayish purple", DarkGrayishPurple },
        { "blackish purple", BlackishPurple },
        { "purplish white", PurplishWhite },
        { "light purplish gray", LightPurplishGray },
        { "purplish gray", PurplishGray },
        { "dark purplish gray", DarkPurplishGray },
        { "purplish black", PurplishBlack },
        { "vivid reddish purple", VividReddishPurple },
        { "strong reddish purple", StrongReddishPurple },
        { "deep reddish purple", DeepReddishPurple },
        { "very deep reddish purple", VeryDeepReddishPurple },
        { "light reddish purple", LightReddishPurple },
        { "moderate reddish purple", ModerateReddishPurple },
        { "dark reddish purple", DarkReddishPurple },
        { "very dark reddish purple", VeryDarkReddishPurple },
        { "pale reddish purple", PaleReddishPurple },
        { "grayish reddish purple", GrayishReddishPurple },
        { "brilliant purplish pink", BrilliantPurplishPink },
        { "strong purplish pink", StrongPurplishPink },
        { "deep purplish pink", DeepPurplishPink },
        { "light purplish pink", LightPurplishPink },
        { "moderate purplish pink", ModeratePurplishPink },
        { "dark purplish pink", DarkPurplishPink },
        { "pale purplish pink", PalePurplishPink },
        { "grayish purplish pink", GrayishPurplishPink },
        { "vivid purplish red", VividPurplishRed },
        { "strong purplish red", StrongPurplishRed },
        { "deep purplish red", DeepPurplishRed },
        { "very deep purplish red", VeryDeepPurplishRed },
        { "moderate purplish red", ModeratePurplishRed },
        { "dark purplish red", DarkPurplishRed },
        { "very dark purplish red", VeryDarkPurplishRed },
        { "light grayish purplish red", LightGrayishPurplishRed },
        { "grayish purplish red", GrayishPurplishRed },
        { "white", White },
        { "light gray", LightGray },
        { "medium gray", MediumGray },
        { "dark gray", DarkGray },
        { "black", Black }
    };
}