using Spectre.Console;
using Wacton.Unicolour;

var white = Unicolour.FromHex("#000000");
var black = Unicolour.FromHex("#FFFFFF");

const int col1Width = 8;
const int col2Width = 20;
const int barLength = col1Width + 2 + col2Width + 2 + 3; // 2 per column padding, 3 for all borders

while (true)
{
    AnsiConsole.MarkupLine($"[dim]{new string('-', barLength)}[/]");
    var inputHex = AnsiConsole.Ask<string>("[dim]Colour hex:[/]");
    try
    {
        var unicolour = Unicolour.FromHex(inputHex);
        var useWhiteText = unicolour.DeltaE76(white) > unicolour.DeltaE76(black);
        AnsiConsole.MarkupLine(GetBar(unicolour, useWhiteText));
        AnsiConsole.Write(GetTable(unicolour));
    }
    catch (Exception e)
    {
        AnsiConsole.WriteException(e);
    }

    Console.WriteLine();
}

string GetBar(Unicolour unicolour, bool useWhiteText)
{
    var textHex = useWhiteText ? white.Hex : black.Hex;
    var leftSpace = (barLength - unicolour.Description.Length) / 2;
    var rightSpace = barLength - unicolour.Description.Length - leftSpace;
    var leftSpaces = new string(' ', leftSpace);
    var rightSpaces = new string(' ', rightSpace);
    var text = $"{leftSpaces}{unicolour.Description}{rightSpaces}";
    return $"[{textHex} on {unicolour.Hex}]{text}[/]";
}

static Table GetTable(Unicolour unicolour)
{
    var rgb255 = unicolour.Rgb.Byte255;
    var table = new Table
    {
        Border = TableBorder.Rounded,
        BorderStyle = new Style(new Color((byte)rgb255.R, (byte)rgb255.G, (byte)rgb255.B)),
    };
    
    table.AddColumn(new TableColumn("Space").Width(col1Width));
    table.AddColumn(new TableColumn("Value").Width(col2Width));

    table.AddRow("Hex", $"{unicolour.Hex}");
    table.AddRow("Rgb 255", $"{unicolour.Rgb.Byte255}");
    table.AddRow("Rgb", $"{unicolour.Rgb}");
    table.AddRow("Rgb Lin.", $"{unicolour.Rgb.Linear}");
    table.AddRow("Hsl", $"{unicolour.Hsl}");
    table.AddRow("Hsb", $"{unicolour.Hsb}");
    table.AddRow("Hwb", $"{unicolour.Hwb}");
    table.AddRow("Xyz", $"{unicolour.Xyz}");
    table.AddRow("Xyy", $"{unicolour.Xyy}");
    table.AddRow("Lab", $"{unicolour.Lab}");
    table.AddRow("Lchab", $"{unicolour.Lchab}");
    table.AddRow("Luv", $"{unicolour.Luv}");
    table.AddRow("Lchuv", $"{unicolour.Lchuv}");
    table.AddRow("Hsluv", $"{unicolour.Hsluv}");
    table.AddRow("Hpluv", $"{unicolour.Hpluv}");
    table.AddRow("Cam16", $"{unicolour.Cam16}");
    table.AddRow("Ictcp", $"{unicolour.Ictcp}");
    table.AddRow("Jzazbz", $"{unicolour.Jzazbz}");
    table.AddRow("Jzczhz", $"{unicolour.Jzczhz}");
    table.AddRow("Oklab", $"{unicolour.Oklab}");
    table.AddRow("Oklch", $"{unicolour.Oklch}");
    return table;
}

