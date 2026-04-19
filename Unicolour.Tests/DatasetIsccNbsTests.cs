using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wacton.Unicolour.Datasets;
using static Wacton.Unicolour.Datasets.IsccNbs;

namespace Wacton.Unicolour.Tests;

public class DatasetIsccNbsTests
{
    private static readonly string[] Names =
    [
        "vivid pink",
        "strong pink",
        "deep pink",
        "light pink",
        "moderate pink",
        "dark pink",
        "pale pink",
        "grayish pink",
        "pinkish white",
        "pinkish gray",
        "vivid red",
        "strong red",
        "deep red",
        "very deep red",
        "moderate red",
        "dark red",
        "very dark red",
        "light grayish red",
        "grayish red",
        "dark grayish red",
        "blackish red",
        "reddish gray",
        "dark reddish gray",
        "reddish black",
        "vivid yellowish pink",
        "strong yellowish pink",
        "deep yellowish pink",
        "light yellowish pink",
        "moderate yellowish pink",
        "dark yellowish pink",
        "pale yellowish pink",
        "grayish yellowish pink",
        "brownish pink",
        "vivid reddish orange",
        "strong reddish orange",
        "deep reddish orange",
        "moderate reddish orange",
        "dark reddish orange",
        "grayish reddish orange",
        "strong reddish brown",
        "deep reddish brown",
        "light reddish brown",
        "moderate reddish brown",
        "dark reddish brown",
        "light grayish reddish brown",
        "grayish reddish brown",
        "dark grayish reddish brown",
        "vivid orange",
        "brilliant orange",
        "strong orange",
        "deep orange",
        "light orange",
        "moderate orange",
        "brownish orange",
        "strong brown",
        "deep brown",
        "light brown",
        "moderate brown",
        "dark brown",
        "light grayish brown",
        "grayish brown",
        "dark grayish brown",
        "light brownish gray",
        "brownish gray",
        "brownish black",
        "vivid orange yellow",
        "brilliant orange yellow",
        "strong orange yellow",
        "deep orange yellow",
        "light orange yellow",
        "moderate orange yellow",
        "dark orange yellow",
        "pale orange yellow",
        "strong yellowish brown",
        "deep yellowish brown",
        "light yellowish brown",
        "moderate yellowish brown",
        "dark yellowish brown",
        "light grayish yellowish brown",
        "grayish yellowish brown",
        "dark grayish yellow brown",
        "vivid yellow",
        "brilliant yellow",
        "strong yellow",
        "deep yellow",
        "light yellow",
        "moderate yellow",
        "dark yellow",
        "pale yellow",
        "grayish yellow",
        "dark grayish yellow",
        "yellowish white",
        "yellowish gray",
        "light olive brown",
        "moderate olive brown",
        "dark olive brown",
        "vivid greenish yellow",
        "brilliant greenish yellow",
        "strong greenish yellow",
        "deep greenish yellow",
        "light greenish yellow",
        "moderate greenish yellow",
        "dark greenish yellow",
        "pale greenish yellow",
        "grayish greenish yellow",
        "light olive",
        "moderate olive",
        "dark olive",
        "light grayish olive",
        "grayish olive",
        "dark grayish olive",
        "light olive gray",
        "olive gray",
        "olive black",
        "vivid yellow green",
        "brilliant yellow green",
        "strong yellow green",
        "deep yellow green",
        "light yellow green",
        "moderate yellow green",
        "pale yellow green",
        "grayish yellow green",
        "strong olive green",
        "deep olive green",
        "moderate olive green",
        "dark olive green",
        "grayish olive green",
        "dark grayish olive green",
        "vivid yellowish green",
        "brilliant yellowish green",
        "strong yellowish green",
        "deep yellowish green",
        "very deep yellowish green",
        "very light yellowish green",
        "light yellowish green",
        "moderate yellowish green",
        "dark yellowish green",
        "very dark yellowish green",
        "vivid green",
        "brilliant green",
        "strong green",
        "deep green",
        "very light green",
        "light green",
        "moderate green",
        "dark green",
        "very dark green",
        "very pale green",
        "pale green",
        "grayish green",
        "dark grayish green",
        "blackish green",
        "greenish white",
        "light greenish gray",
        "greenish gray",
        "dark greenish gray",
        "greenish black",
        "vivid bluish green",
        "brilliant bluish green",
        "strong bluish green",
        "deep bluish green",
        "very light bluish green",
        "light bluish green",
        "moderate bluish green",
        "dark bluish green",
        "very dark bluish green",
        "vivid greenish blue",
        "brilliant greenish blue",
        "strong greenish blue",
        "deep greenish blue",
        "very light greenish blue",
        "light greenish blue",
        "moderate greenish blue",
        "dark greenish blue",
        "very dark greenish blue",
        "vivid blue",
        "brilliant blue",
        "strong blue",
        "deep blue",
        "very light blue",
        "light blue",
        "moderate blue",
        "dark blue",
        "very pale blue",
        "pale blue",
        "grayish blue",
        "dark grayish blue",
        "blackish blue",
        "bluish white",
        "light bluish gray",
        "bluish gray",
        "dark bluish gray",
        "bluish black",
        "vivid purplish blue",
        "brilliant purplish blue",
        "strong purplish blue",
        "deep purplish blue",
        "very light purplish blue",
        "light purplish blue",
        "moderate purplish blue",
        "dark purplish blue",
        "very pale purplish blue",
        "pale purplish blue",
        "grayish purplish blue",
        "vivid violet",
        "brilliant violet",
        "strong violet",
        "deep violet",
        "very light violet",
        "light violet",
        "moderate violet",
        "dark violet",
        "very pale violet",
        "pale violet",
        "grayish violet",
        "vivid purple",
        "brilliant purple",
        "strong purple",
        "deep purple",
        "very deep purple",
        "very light purple",
        "light purple",
        "moderate purple",
        "dark purple",
        "very dark purple",
        "very pale purple",
        "pale purple",
        "grayish purple",
        "dark grayish purple",
        "blackish purple",
        "purplish white",
        "light purplish gray",
        "purplish gray",
        "dark purplish gray",
        "purplish black",
        "vivid reddish purple",
        "strong reddish purple",
        "deep reddish purple",
        "very deep reddish purple",
        "light reddish purple",
        "moderate reddish purple",
        "dark reddish purple",
        "very dark reddish purple",
        "pale reddish purple",
        "grayish reddish purple",
        "brilliant purplish pink",
        "strong purplish pink",
        "deep purplish pink",
        "light purplish pink",
        "moderate purplish pink",
        "dark purplish pink",
        "pale purplish pink",
        "grayish purplish pink",
        "vivid purplish red",
        "strong purplish red",
        "deep purplish red",
        "very deep purplish red",
        "moderate purplish red",
        "dark purplish red",
        "very dark purplish red",
        "light grayish purplish red",
        "grayish purplish red",
        "white",
        "light gray",
        "medium gray",
        "dark gray",
        "black"
    ];
    
    [Test]
    public void All()
    {
        Assert.That(IsccNbs.All.Count(), Is.EqualTo(267));
        Assert.That(IsccNbs.All.Distinct().Count(), Is.EqualTo(267));
    }
    
    [Test]
    public void Name([ValueSource(nameof(Names))] string name)
    {
        var colour = FromName(name);
        Assert.That(colour, Is.Not.Null);
    }

    private static readonly List<string> NamesWithSpaces = Names.Select(AddWhitespaceAndChangeCasing).ToList();
    private static string AddWhitespaceAndChangeCasing(string text)
    {
        var multicase = string.Concat(text.Select((x, i) => i % 2 == 0 ? char.ToLower(x) : char.ToUpper(x)));
        return multicase.Insert(multicase.Length, "\n").Insert(multicase.Length - 1, " ").Insert(0, "\t").Insert(2, " ");
    }
    
    [Test]
    public void NameWithWhitespaceAndCasing([ValueSource(nameof(NamesWithSpaces))] string name)
    {
        var colour = FromName(name);
        Assert.That(colour, Is.Not.Null);
    }
    
    private static readonly List<string> NamesWithoutSpaces = Names.Select(RemoveWhitespaceAndChangeCasing).ToList();
    private static string RemoveWhitespaceAndChangeCasing(string text)
    {
        var multicase = string.Concat(text.Select((x, i) => i % 2 == 0 ? char.ToLower(x) : char.ToUpper(x)));
        return multicase.Replace(" ", string.Empty);
    }
    
    [Test]
    public void NameWithoutWhitespaceAndCasing([ValueSource(nameof(NamesWithoutSpaces))] string name)
    {
        var colour = FromName(name);
        Assert.That(colour, Is.Not.Null);
    }
    
    // names that look like they might exist
    [TestCase("red")]
    [TestCase("yellow")]
    [TestCase("green")]
    [TestCase("blue")]
    // names that look like they should exist based on other names
    [TestCase("very dark pink")]
    [TestCase("very pale olive")]
    // misspellings
    [TestCase("vivid punk")]
    [TestCase("gray")] // 😢
    // common text and numbers
    [TestCase("unicolour")]
    [TestCase("abc")]
    [TestCase("123")]
    // punctuation and whitespace
    [TestCase("\"vivid pink\"")]
    [TestCase(".")]
    [TestCase(" ")]
    [TestCase("")]
    [TestCase("\t")]
    [TestCase("\n")]
    [TestCase(null)]
    public void NameNotFound(string name)
    {
        var colour = FromName(name);
        Assert.That(colour, Is.Null);
    }
}