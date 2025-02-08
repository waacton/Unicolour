using Spectre.Console;
using Wacton.Unicolour;
using Wacton.Unicolour.Icc;

var config = new Configuration(iccConfig: new("./SWOP2006_Coated5v2.icc", Intent.RelativeColorimetric, "SWOP2006"));

var white = new Unicolour("#000000");
var black = new Unicolour("#FFFFFF");

const int col1Width = 8;
const int col2Width = 32;
const int barLength = col1Width + 2 + col2Width + 2 + 3; // 2 per column padding, 3 for all borders

while (true)
{
    AnsiConsole.MarkupLine($"[dim]{new string('-', barLength)}[/]");
    var inputHex = AnsiConsole.Ask<string>("[dim]Colour hex:[/]");
    try
    {
        var colour = new Unicolour(config, inputHex);
        var useWhiteText = colour.Difference(white, DeltaE.Cie76) > colour.Difference(black, DeltaE.Cie76);
        AnsiConsole.MarkupLine(GetBar(colour, useWhiteText));
        AnsiConsole.Write(GetTable(colour));
    }
    catch (Exception e)
    {
        AnsiConsole.WriteException(e);
    }

    Console.WriteLine();
}

string GetBar(Unicolour colour, bool useWhiteText)
{
    var textHex = useWhiteText ? white.Hex : black.Hex;
    var leftSpace = (barLength - colour.Description.Length) / 2;
    var rightSpace = barLength - colour.Description.Length - leftSpace;
    var leftSpaces = new string(' ', leftSpace);
    var rightSpaces = new string(' ', rightSpace);
    var text = $"{leftSpaces}{colour.Description}{rightSpaces}";
    return $"[{textHex} on {colour.Hex}]{text}[/]";
}

static Table GetTable(Unicolour colour)
{
    var rgb255 = colour.Rgb.Byte255;
    var table = new Table
    {
        Border = TableBorder.Rounded,
        BorderStyle = new Style(new Color((byte)rgb255.R, (byte)rgb255.G, (byte)rgb255.B))
    };
    
    table.AddColumn(new TableColumn("Space").Width(col1Width));
    table.AddColumn(new TableColumn("Value").Width(col2Width));

    table.AddRow("Hex", $"{colour.Hex}");
    table.AddRow("Rgb 255", $"{colour.Rgb.Byte255}");
    table.AddRow("Rgb", $"{colour.Rgb}");
    table.AddRow("Rgb Lin.", $"{colour.RgbLinear}");
    table.AddRow("Hsl", $"{colour.Hsl}");
    table.AddRow("Hsb", $"{colour.Hsb}");
    table.AddRow("Hwb", $"{colour.Hwb}");
    table.AddRow("Hsi", $"{colour.Hsi}");
    table.AddRow("Xyz", $"{colour.Xyz}");
    table.AddRow("Xyy", $"{colour.Xyy}");
    table.AddRow("Wxy", $"{colour.Wxy}");
    table.AddRow("Lab", $"{colour.Lab}");
    table.AddRow("Lchab", $"{colour.Lchab}");
    table.AddRow("Luv", $"{colour.Luv}");
    table.AddRow("Lchuv", $"{colour.Lchuv}");
    table.AddRow("Hsluv", $"{colour.Hsluv}");
    table.AddRow("Hpluv", $"{colour.Hpluv}");
    table.AddRow("Ypbpr", $"{colour.Ypbpr}");
    table.AddRow("Ycbcr", $"{colour.Ycbcr}");
    table.AddRow("Ycgco", $"{colour.Ycgco}");
    table.AddRow("Yuv", $"{colour.Yuv}");
    table.AddRow("Yiq", $"{colour.Yiq}");
    table.AddRow("Ydbdr", $"{colour.Ydbdr}");
    table.AddRow("Tsl", $"{colour.Tsl}");
    table.AddRow("Xyb", $"{colour.Xyb}");
    table.AddRow("Ipt", $"{colour.Ipt}");
    table.AddRow("Ictcp", $"{colour.Ictcp}");
    table.AddRow("Jzazbz", $"{colour.Jzazbz}");
    table.AddRow("Jzczhz", $"{colour.Jzczhz}");
    table.AddRow("Oklab", $"{colour.Oklab}");
    table.AddRow("Oklch", $"{colour.Oklch}");
    table.AddRow("Okhsv", $"{colour.Okhsv}");
    table.AddRow("Okhsl", $"{colour.Okhsl}");
    table.AddRow("Okhwb", $"{colour.Okhwb}");
    table.AddRow("Cam02", $"{colour.Cam02}");
    table.AddRow("Cam16", $"{colour.Cam16}");
    table.AddRow("Hct", $"{colour.Hct}");
    table.AddRow("Icc", $"{colour.Icc}");
    return table;
}

