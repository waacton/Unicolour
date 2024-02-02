using System.Text.RegularExpressions;

var sourceRoot = Path.GetFullPath("./docs");
var solutionRoot = AppDomain.CurrentDomain.BaseDirectory.Split("Unicolour.Readme")[0];
var docsRoot = Path.Combine(solutionRoot, "docs");

ProcessReadme();
CopyDirectory(sourceRoot, docsRoot);
return;

void ProcessReadme()
{
    const string readme = "README.md";
    const string readmeAmerican = "README_us.md";
    
    var readmeText = File.ReadAllText(readme);

    var readmeRootText = readmeText
        .Replace("../", string.Empty);
    File.WriteAllText(Path.Combine(solutionRoot, readme), readmeRootText);
    
    var readmeDocsText = readmeText
        .Replace("docs/", string.Empty)
        .Replace("../", "https://github.com/waacton/Unicolour/tree/main/");

    // until GitHub Pages supports Mermaid 😑 - just remove it
    readmeDocsText = Regex.Replace(readmeDocsText, @"<details>(.|\n)*<\/details>", string.Empty);

    var readmeUkText = readmeDocsText;
    var readmeUsText = readmeDocsText;

    readmeUkText += Environment.NewLine;
    readmeUkText += $"Also available in [American]({readmeAmerican}) \ud83c\uddfa\ud83c\uddf8.";
    File.WriteAllText(Path.Combine(sourceRoot, readme), readmeUkText);

    // could use regex but why bother? also want to be careful not to change spelling of "unicolour", "ColourSpace", etc.
    readmeUsText = readmeUsText
        .Replace("Colour ", "Color ")
        .Replace("Colour&", "Color&")
        .Replace(" colour", " color")
        .Replace("-colour", "-color")
        .Replace("ise ", "ize ")
        .Replace("ises ", "izes ")
        .Replace("isation ", "ization ");
    readmeUsText += Environment.NewLine;
    readmeUsText += $"Also available in [British]({readme}) \ud83c\uddec\ud83c\udde7.";
    File.WriteAllText(Path.Combine(sourceRoot, readmeAmerican), readmeUsText);
}

void CopyDirectory(string sourcePath, string targetPath)
{
    Directory.CreateDirectory(targetPath);

    var sourceFiles = Directory.GetFiles(sourcePath);
    foreach (var sourceFile in sourceFiles)
    {
        var fileName = Path.GetFileName(sourceFile);
        var targetFile = Path.Combine(targetPath, fileName);
        File.Copy(sourceFile, targetFile, overwrite: true);
    }

    var sourceDirectories = Directory.GetDirectories(sourcePath);
    foreach (var sourceDirectory in sourceDirectories)
    {
        var directoryName = Path.GetFileName(sourceDirectory);
        var targetSubDirectory = Path.Combine(targetPath, directoryName);
        CopyDirectory(sourceDirectory, targetSubDirectory);
    }
}