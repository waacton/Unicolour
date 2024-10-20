﻿using Spectre.Console;
using Wacton.Unicolour;
using Wacton.Unicolour.Icc;

var config = new Configuration(iccConfiguration: new("./SWOP2006_Coated5v2.icc", Intent.RelativeColorimetric, "SWOP2006"));

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
        var unicolour = new Unicolour(config, inputHex);
        var useWhiteText = unicolour.Difference(white, DeltaE.Cie76) > unicolour.Difference(black, DeltaE.Cie76);
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
        BorderStyle = new Style(new Color((byte)rgb255.R, (byte)rgb255.G, (byte)rgb255.B))
    };
    
    table.AddColumn(new TableColumn("Space").Width(col1Width));
    table.AddColumn(new TableColumn("Value").Width(col2Width));

    table.AddRow("Hex", $"{unicolour.Hex}");
    table.AddRow("Rgb 255", $"{unicolour.Rgb.Byte255}");
    table.AddRow("Rgb", $"{unicolour.Rgb}");
    table.AddRow("Rgb Lin.", $"{unicolour.RgbLinear}");
    table.AddRow("Hsl", $"{unicolour.Hsl}");
    table.AddRow("Hsb", $"{unicolour.Hsb}");
    table.AddRow("Hwb", $"{unicolour.Hwb}");
    table.AddRow("Hsi", $"{unicolour.Hsi}");
    table.AddRow("Xyz", $"{unicolour.Xyz}");
    table.AddRow("Xyy", $"{unicolour.Xyy}");
    table.AddRow("Wxy", $"{unicolour.Wxy}");
    table.AddRow("Lab", $"{unicolour.Lab}");
    table.AddRow("Lchab", $"{unicolour.Lchab}");
    table.AddRow("Luv", $"{unicolour.Luv}");
    table.AddRow("Lchuv", $"{unicolour.Lchuv}");
    table.AddRow("Hsluv", $"{unicolour.Hsluv}");
    table.AddRow("Hpluv", $"{unicolour.Hpluv}");
    table.AddRow("Ypbpr", $"{unicolour.Ypbpr}");
    table.AddRow("Ycbcr", $"{unicolour.Ycbcr}");
    table.AddRow("Ycgco", $"{unicolour.Ycgco}");
    table.AddRow("Yuv", $"{unicolour.Yuv}");
    table.AddRow("Yiq", $"{unicolour.Yiq}");
    table.AddRow("Ydbdr", $"{unicolour.Ydbdr}");
    table.AddRow("Tsl", $"{unicolour.Tsl}");
    table.AddRow("Xyb", $"{unicolour.Xyb}");
    table.AddRow("Ipt", $"{unicolour.Ipt}");
    table.AddRow("Ictcp", $"{unicolour.Ictcp}");
    table.AddRow("Jzazbz", $"{unicolour.Jzazbz}");
    table.AddRow("Jzczhz", $"{unicolour.Jzczhz}");
    table.AddRow("Oklab", $"{unicolour.Oklab}");
    table.AddRow("Oklch", $"{unicolour.Oklch}");
    table.AddRow("Okhsv", $"{unicolour.Okhsv}");
    table.AddRow("Okhsl", $"{unicolour.Okhsl}");
    table.AddRow("Okhwb", $"{unicolour.Okhwb}");
    table.AddRow("Cam02", $"{unicolour.Cam02}");
    table.AddRow("Cam16", $"{unicolour.Cam16}");
    table.AddRow("Hct", $"{unicolour.Hct}");
    table.AddRow("Icc", $"{unicolour.Icc}");
    return table;
}

